
using System;
using SystemBooks.Models;

namespace SystemBooks;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("====================================");
        Console.WriteLine("       SISTEMA DE BIBLIOTECA");
        Console.WriteLine("====================================");

        // Crear libros
        Libro libro1 = new Libro(
            1, "Cien años de soledad",
            "Gabriel García Márquez", "9780307474728", 1967);

        Libro libro2 = new Libro(
            2, "El principito",
            "Antoine de Saint-Exupéry", "9780156012195", 1943);

        // Crear usuarios
        Usuario usuario1 = new Usuario(
            1, "Carlos Pérez", "carlos@email.com", "3001234567");

        Usuario usuario2 = new Usuario(
            2, "Laura Gómez", "laura@email.com", "3109876543");

        // Crear préstamo
        Prestamo prestamo1 = new Prestamo(
            1, libro1, usuario1,
            DateTime.Today.AddDays(-3),
            DateTime.Today.AddDays(12));

        libro1.Disponible = false;

        // Mostrar libros
        Console.WriteLine("\n--- LIBROS ---");
        Console.WriteLine(libro1.DetalleCompleto());
        Console.WriteLine();
        Console.WriteLine(libro2.DetalleCompleto());

        // Mostrar usuarios
        Console.WriteLine("\n--- USUARIOS ---");
        Console.WriteLine(usuario1.DetalleCompleto());
        Console.WriteLine();
        Console.WriteLine(usuario2.DetalleCompleto());

        // Mostrar préstamo
        Console.WriteLine("\n--- PRÉSTAMO ---");
        Console.WriteLine(prestamo1.DetalleCompleto());

        // Validaciones
        Console.WriteLine("\n--- VALIDACIONES ---");
        Console.WriteLine($"Libro 1 disponible: {libro1.Disponible}");
        Console.WriteLine($"Libro 2 disponible: {libro2.Disponible}");
        Console.WriteLine($"Usuario 1 activo: {usuario1.Activo}");
        Console.WriteLine($"Estado del préstamo: {prestamo1.Estado}");
        Console.WriteLine($"Préstamo vencido: {prestamo1.EstaVencido()}");
        Console.WriteLine($"Días transcurridos: {prestamo1.DiasTranscurridos()}");

        Console.WriteLine("\n====================================");
        Console.WriteLine("       FIN DEL PROGRAMA");
        Console.WriteLine("====================================");
    }
}
