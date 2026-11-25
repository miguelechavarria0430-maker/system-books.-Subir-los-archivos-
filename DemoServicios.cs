using System;
using System.Collections.Generic;
using SystemBooks.Models;
using SystemBooks.Services;

namespace SystemBooks;

/// <summary>
/// Demostración de la capa Services (EV08): datos de prueba,
/// búsquedas, ordenaciones y KPIs.
/// Se ejecuta con: dotnet run -- --pruebas-servicios
/// </summary>
public static class DemoServicios
{
    public static void Ejecutar()
    {
        LibroService libroService = new LibroService();
        UsuarioService usuarioService = new UsuarioService();
        PrestamoService prestamoService = new PrestamoService();

        // ---------- 1. DATOS DE PRUEBA ----------
        Titulo("1. CREAR SERVICIOS Y AGREGAR DATOS DE PRUEBA");

        Libro l1 = new Libro(1, "Cien años de soledad", "Gabriel García Márquez", "978-0307474728", 1967);
        Libro l2 = new Libro(2, "El coronel no tiene quien le escriba", "Gabriel García Márquez", "978-0060753610", 1961);
        Libro l3 = new Libro(3, "Rayuela", "Julio Cortázar", "978-8437604572", 1963);
        Libro l4 = new Libro(4, "Don Quijote de la Mancha", "Miguel de Cervantes", "978-8420412146", 1605);

        libroService.AgregarLibro(l1);
        libroService.AgregarLibro(l2);
        libroService.AgregarLibro(l3);
        libroService.AgregarLibro(l4);

        Usuario u1 = new Usuario(1, "Luis Gómez", "luis@correo.com", "3004445566");
        Usuario u2 = new Usuario(2, "Ana Pérez", "ana@correo.com", "3001112233");
        Usuario u3 = new Usuario(3, "Carlos Ruiz", "carlos@correo.com", "3007778899");
        u3.CambiarEstado(false); // usuario inactivo para la prueba

        usuarioService.AgregarUsuario(u1);
        usuarioService.AgregarUsuario(u2);
        usuarioService.AgregarUsuario(u3);

        Prestamo p1 = new Prestamo(0, l1, u1, DateTime.Today, DateTime.Today.AddDays(30));
        Prestamo p2 = new Prestamo(0, l3, u2, DateTime.Today, DateTime.Today.AddDays(5));
        Prestamo p3 = new Prestamo(0, l2, u3, DateTime.Today, DateTime.Today.AddDays(10));

        Console.WriteLine($"Préstamo 1 (Luis, Cien años...) registrado: {prestamoService.RegistrarPrestamo(p1)}");
        Console.WriteLine($"Préstamo 2 (Ana, Rayuela) registrado: {prestamoService.RegistrarPrestamo(p2)}");
        Console.WriteLine($"Préstamo 3 (Carlos inactivo) registrado: {prestamoService.RegistrarPrestamo(p3)}  <- rechazado por usuario inactivo");
        Console.WriteLine($"Devolución del préstamo {p2.Id}: {prestamoService.RegistrarDevolucion(p2.Id)}");

        Mostrar("Libros en el orden en que se agregaron", libroService.ObtenerTodos());
        Mostrar("Usuarios en el orden en que se agregaron", usuarioService.ObtenerTodos());
        Mostrar("Préstamos en el orden en que se agregaron", prestamoService.ObtenerTodos());

        // ---------- 2. BÚSQUEDAS ----------
        Titulo("2. BÚSQUEDAS");

        Mostrar("Libros por título que contenga 'soledad'", libroService.BuscarPorTitulo("soledad"));
        Mostrar("Libros por autor que contenga 'márquez'", libroService.BuscarPorAutor("márquez"));
        Console.WriteLine();
        Console.WriteLine($"Libro por ISBN 978-8437604572: {libroService.BuscarPorISBN("978-8437604572")}");
        Mostrar("Usuarios por nombre que contenga 'ana'", usuarioService.BuscarPorNombre("ana"));
        Console.WriteLine();
        Console.WriteLine($"Usuario por ID 1: {usuarioService.BuscarPorId(1)}");
        Mostrar("Préstamos con estado Activo", prestamoService.BuscarPorEstado(EstadoPrestamo.Activo));
        Mostrar("Préstamos con estado Devuelto", prestamoService.BuscarPorEstado(EstadoPrestamo.Devuelto));

        // ---------- 3. ORDENACIONES ----------
        Titulo("3. ORDENACIONES");

        Mostrar("Libros ordenados por título (A-Z)", libroService.OrdenarPorTitulo());
        Mostrar("Usuarios ordenados por nombre (A-Z)", usuarioService.OrdenarPorNombre());
        Mostrar("Préstamos ordenados por fecha de vencimiento", prestamoService.OrdenarPorFechaVencimiento());

        // ---------- 4. KPIs ----------
        Titulo("4. ESTADÍSTICAS (KPIs)");

        Console.WriteLine("--- LibroService ---");
        Console.WriteLine($"Total de libros: {libroService.TotalLibros()}");
        Console.WriteLine($"Disponibles: {libroService.TotalDisponibles()} | Prestados: {libroService.TotalPrestados()}");
        Console.WriteLine();
        Console.WriteLine("--- UsuarioService ---");
        Console.WriteLine($"Total de usuarios: {usuarioService.TotalUsuarios()}");
        Console.WriteLine($"Activos: {usuarioService.TotalActivos()} | Inactivos: {usuarioService.TotalInactivos()}");
        Console.WriteLine();
        Console.WriteLine("--- PrestamoService ---");
        Console.WriteLine($"Total de préstamos: {prestamoService.TotalPrestamos()}");
        Console.WriteLine($"Activos: {prestamoService.TotalActivos()}");
        Console.WriteLine($"Vencidos: {prestamoService.TotalVencidos()}");
        Console.WriteLine($"Devueltos: {prestamoService.TotalDevueltos()}");
        Console.WriteLine($"Promedio de días de préstamo: {prestamoService.PromedioDiasPrestamo():F1}");
    }

    private static void Titulo(string texto)
    {
        Console.WriteLine();
        Console.WriteLine("==================================================");
        Console.WriteLine(texto);
        Console.WriteLine("==================================================");
    }

    private static void Mostrar<T>(string titulo, List<T> elementos)
    {
        Console.WriteLine();
        Console.WriteLine($"> {titulo}:");

        if (elementos.Count == 0)
        {
            Console.WriteLine("  (sin resultados)");
            return;
        }

        foreach (T elemento in elementos)
            Console.WriteLine($"  {elemento}");
    }
}