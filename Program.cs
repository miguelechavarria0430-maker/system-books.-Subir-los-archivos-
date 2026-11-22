
using System;
using SystemBooks.Models;

namespace SystemBooks;

class Program
{
    static void Main(string[] args)
    {
        if (args.Length > 0 && args[0] == "--pruebas-modelos")
        {
            MostrarPruebasObjetos();
            return;
        }

        ShowMainMenu();
    }

    // ==============================
    // MENU PRINCIPAL
    // ==============================

    static void ShowMainMenu()
    {
        bool salir = false;

        while (!salir)
        {
            Console.Clear();
            Console.WriteLine("====================================");
            Console.WriteLine("       SISTEMA DE BIBLIOTECA");
            Console.WriteLine("====================================");
            Console.WriteLine("1. Libros");
            Console.WriteLine("2. Usuarios");
            Console.WriteLine("3. Prestamos");
            Console.WriteLine("4. Busquedas y reportes");
            Console.WriteLine("5. Guardar / Cargar datos");
            Console.WriteLine("6. Salir");
            Console.WriteLine("====================================");

            int opcion = ReadOption("Seleccione una opcion: ", 1, 6);

            switch (opcion)
            {
                case 1:
                    ShowBooksMenu();
                    break;
                case 2:
                    ShowUsersMenu();
                    break;
                case 3:
                    ShowLoansMenu();
                    break;
                case 4:
                    ShowSearchReportsMenu();
                    break;
                case 5:
                    ShowPersistenceMenu();
                    break;
                case 6:
                    salir = ConfirmExitAndSave();
                    break;
            }
        }
    }

    // ==============================
    // VALIDACION DE OPCIONES
    // ==============================

    static int ReadOption(string mensaje, int minimo, int maximo)
    {
        while (true)
        {
            Console.Write(mensaje);
            string? entrada = Console.ReadLine();

            if (int.TryParse(entrada, out int opcion) &&
                opcion >= minimo && opcion <= maximo)
            {
                return opcion;
            }

            Console.WriteLine(
                $"Opcion invalida. Ingrese un numero entre {minimo} y {maximo}.");
        }
    }

    // ==============================
    // MENU DE LIBROS
    // ==============================

    static void ShowBooksMenu()
    {
        bool volver = false;

        while (!volver)
        {
            Console.Clear();
            Console.WriteLine("====================================");
            Console.WriteLine("          MENU DE LIBROS");
            Console.WriteLine("====================================");
            Console.WriteLine("1. Registrar libro");
            Console.WriteLine("2. Listar libros");
            Console.WriteLine("3. Consultar detalle de libro");
            Console.WriteLine("4. Actualizar libro");
            Console.WriteLine("5. Eliminar libro");
            Console.WriteLine("6. Volver al menu principal");
            Console.WriteLine("====================================");

            int opcion = ReadOption("Seleccione una opcion: ", 1, 6);

            switch (opcion)
            {
                case 1:
                    RegisterBook();
                    break;
                case 2:
                    ListBooksMenu();
                    break;
                case 3:
                    ViewBookDetail();
                    break;
                case 4:
                    UpdateBookMenu();
                    break;
                case 5:
                    DeleteBook();
                    break;
                case 6:
                    volver = true;
                    break;
            }
        }
    }

    static void RegisterBook()
    {
        ShowBookAction(
            "Registrar libro",
            "Aqui se solicitaran los datos del nuevo libro.");
    }

    static void ListBooksMenu()
    {
        bool volver = false;

        while (!volver)
        {
            Console.Clear();
            Console.WriteLine("========== LISTAR LIBROS ==========");
            Console.WriteLine("1. Listar todos");
            Console.WriteLine("2. Listar disponibles");
            Console.WriteLine("3. Listar prestados");
            Console.WriteLine("4. Volver");
            Console.WriteLine("===================================");

            int opcion = ReadOption("Seleccione una opcion: ", 1, 4);

            switch (opcion)
            {
                case 1:
                    ListBooksAll();
                    break;
                case 2:
                    ListBooksAvailable();
                    break;
                case 3:
                    ListBooksBorrowed();
                    break;
                case 4:
                    volver = true;
                    break;
            }
        }
    }

    static void ListBooksAll()
    {
        ShowBookAction(
            "Listar todos los libros",
            "Se mostrara el listado completo de libros registrados.");
    }

    static void ListBooksAvailable()
    {
        ShowBookAction(
            "Listar libros disponibles",
            "Se mostraran solamente los libros disponibles.");
    }

    static void ListBooksBorrowed()
    {
        ShowBookAction(
            "Listar libros prestados",
            "Se mostraran los libros que actualmente estan prestados.");
    }

    static void ViewBookDetail()
    {
        ShowBookAction(
            "Consultar detalle de libro",
            "Se podra consultar un libro mediante su ID o ISBN.");
    }

    static void UpdateBookMenu()
    {
        bool volver = false;

        while (!volver)
        {
            Console.Clear();
            Console.WriteLine("========== ACTUALIZAR LIBRO ==========");
            Console.WriteLine("1. Editar titulo");
            Console.WriteLine("2. Editar autor");
            Console.WriteLine("3. Editar año y categoria");
            Console.WriteLine("4. Volver");
            Console.WriteLine("======================================");

            int opcion = ReadOption("Seleccione una opcion: ", 1, 4);

            switch (opcion)
            {
                case 1:
                    EditBookTitle();
                    break;
                case 2:
                    EditBookAuthor();
                    break;
                case 3:
                    EditBookYearCategory();
                    break;
                case 4:
                    volver = true;
                    break;
            }
        }
    }

    static void EditBookTitle()
    {
        ShowBookAction(
            "Editar titulo",
            "Se permitira modificar el titulo de un libro.");
    }

    static void EditBookAuthor()
    {
        ShowBookAction(
            "Editar autor",
            "Se permitira modificar el autor de un libro.");
    }

    static void EditBookYearCategory()
    {
        ShowBookAction(
            "Editar año y categoria",
            "Se permitira modificar el año y la categoria del libro.");
    }

    static void DeleteBook()
    {
        ShowBookAction(
            "Eliminar libro",
            "Se validara que el libro no este prestado antes de eliminarlo.");
    }

    static void ShowBookAction(string titulo, string mensaje)
    {
        Console.Clear();
        Console.WriteLine("====================================");
        Console.WriteLine($"       {titulo.ToUpperInvariant()}");
        Console.WriteLine("====================================");
        Console.WriteLine(mensaje);
        Console.WriteLine();
        Console.WriteLine("Presione una tecla para continuar...");
        Console.ReadKey(true);
    }

    // ==============================
    // OTROS MODULOS
    // ==============================

    static void ShowUsersMenu()
    {
        ShowModulePlaceholder("USUARIOS");
    }

    static void ShowLoansMenu()
    {
        ShowModulePlaceholder("PRESTAMOS");
    }

    static void ShowSearchReportsMenu()
    {
        ShowModulePlaceholder("BUSQUEDAS Y REPORTES");
    }

    static void ShowPersistenceMenu()
    {
        ShowModulePlaceholder("GUARDAR / CARGAR DATOS");
    }

    static void ShowModulePlaceholder(string nombreModulo)
    {
        Console.Clear();
        Console.WriteLine("====================================");
        Console.WriteLine($"       MODULO DE {nombreModulo}");
        Console.WriteLine("====================================");
        Console.WriteLine("Este modulo se desarrollara en su rama feature correspondiente.");
        Console.WriteLine();
        Console.WriteLine("Presione una tecla para volver al menu principal...");
        Console.ReadKey(true);
    }

    // ==============================
    // SALIDA DEL SISTEMA
    // ==============================

    static bool ConfirmExitAndSave()
    {
        while (true)
        {
            Console.Write("¿Guardar antes de salir? (S/N): ");
            string? respuesta = Console.ReadLine()?.Trim().ToUpperInvariant();

            if (respuesta == "S")
            {
                Console.WriteLine(
                    "La opcion de guardar se desarrollara en el modulo de persistencia.");
                Console.WriteLine("Saliendo del sistema...");
                return true;
            }

            if (respuesta == "N")
            {
                Console.WriteLine("Saliendo sin guardar...");
                return true;
            }

            Console.WriteLine("Respuesta invalida. Escriba S o N.");
        }
    }

    // ==============================
    // PRUEBAS DE MODELOS EV07
    // ==============================

    static void MostrarPruebasObjetos()
    {
        Console.WriteLine("====================================");
        Console.WriteLine("       PRUEBAS DE OBJETOS");
        Console.WriteLine("====================================");

        Libro libro1 = new Libro(
            1, "Cien años de soledad",
            "Gabriel García Márquez", "9780307474728", 1967);

        Libro libro2 = new Libro(
            2, "El principito",
            "Antoine de Saint-Exupéry", "9780156012195", 1943);

        Usuario usuario1 = new Usuario(
            1, "Carlos Pérez", "carlos@email.com", "3001234567");

        Usuario usuario2 = new Usuario(
            2, "Laura Gómez", "laura@email.com", "3109876543");

        Prestamo prestamo1 = new Prestamo(
            1, libro1, usuario1,
            DateTime.Today.AddDays(-3),
            DateTime.Today.AddDays(12));

        libro1.Disponible = false;

        Console.WriteLine("\n--- LIBROS ---");
        Console.WriteLine(libro1.DetalleCompleto());
        Console.WriteLine();
        Console.WriteLine(libro2.DetalleCompleto());

        Console.WriteLine("\n--- USUARIOS ---");
        Console.WriteLine(usuario1.DetalleCompleto());
        Console.WriteLine();
        Console.WriteLine(usuario2.DetalleCompleto());

        Console.WriteLine("\n--- PRESTAMO ---");
        Console.WriteLine(prestamo1.DetalleCompleto());

        Console.WriteLine("\n--- VALIDACIONES ---");
        Console.WriteLine($"Libro 1 disponible: {libro1.Disponible}");
        Console.WriteLine($"Libro 2 disponible: {libro2.Disponible}");
        Console.WriteLine($"Usuario 1 activo: {usuario1.Activo}");
        Console.WriteLine($"Estado del prestamo: {prestamo1.Estado}");
        Console.WriteLine($"Prestamo vencido: {prestamo1.EstaVencido()}");
        Console.WriteLine($"Dias transcurridos: {prestamo1.DiasTranscurridos()}");
    }
}
