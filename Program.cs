
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
    bool volver = false;

    while (!volver)
    {
        Console.Clear();
        Console.WriteLine("========== MENU DE USUARIOS ==========");
        Console.WriteLine("1. Registrar usuario");
        Console.WriteLine("2. Listar usuarios");
        Console.WriteLine("3. Consultar usuario");
        Console.WriteLine("4. Actualizar usuario");
        Console.WriteLine("5. Activar / Desactivar usuario");
        Console.WriteLine("6. Eliminar usuario");
        Console.WriteLine("7. Volver");
        Console.WriteLine("======================================");

        int opcion = ReadOption("Seleccione una opcion: ", 1, 7);

        switch (opcion)
        {
            case 1: RegisterUser(); break;
            case 2: ListUsers(); break;
            case 3: ViewUserDetail(); break;
            case 4: UpdateUserMenu(); break;
            case 5: ToggleUserActiveStatus(); break;
            case 6: DeleteUser(); break;
            case 7: volver = true; break;
        }
    }
}

static void RegisterUser()
{
    ShowModuleAction("Registrar usuario",
        "Se solicitaran los datos del nuevo usuario.");
}

static void ListUsers()
{
    ShowModuleAction("Listar usuarios",
        "Se mostraran todos los usuarios registrados.");
}

static void ViewUserDetail()
{
    ShowModuleAction("Consultar usuario",
        "Se podra consultar un usuario por ID o documento.");
}

static void UpdateUserMenu()
{
    bool volver = false;

    while (!volver)
    {
        Console.Clear();
        Console.WriteLine("========== ACTUALIZAR USUARIO ==========");
        Console.WriteLine("1. Editar nombre");
        Console.WriteLine("2. Editar contacto");
        Console.WriteLine("3. Activar / Desactivar");
        Console.WriteLine("4. Volver");
        Console.WriteLine("========================================");

        int opcion = ReadOption("Seleccione una opcion: ", 1, 4);

        switch (opcion)
        {
            case 1: EditUserName(); break;
            case 2: EditUserContact(); break;
            case 3: ToggleUserActiveStatus(); break;
            case 4: volver = true; break;
        }
    }
}

static void EditUserName()
{
    ShowModuleAction("Editar nombre",
        "Se permitira modificar el nombre del usuario.");
}

static void EditUserContact()
{
    ShowModuleAction("Editar contacto",
        "Se permitira modificar el correo y telefono.");
}

static void ToggleUserActiveStatus()
{
    ShowModuleAction("Estado del usuario",
        "Se permitira activar o desactivar un usuario.");
}

static void DeleteUser()
{
    ShowModuleAction("Eliminar usuario",
        "Se verificara que el usuario no tenga prestamos activos.");
}
static void ShowModuleAction(string titulo, string mensaje)
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
    static void ShowLoansMenu()
    {
        ShowModulePlaceholder("PRESTAMOS");
    }

    static void ShowSearchReportsMenu()
{
    bool volver = false;

    while (!volver)
    {
        Console.Clear();
        Console.WriteLine("======= BUSQUEDAS Y REPORTES =======");
        Console.WriteLine("1. Buscar libro");
        Console.WriteLine("2. Buscar usuario");
        Console.WriteLine("3. Reportes");
        Console.WriteLine("4. Volver");
        Console.WriteLine("===================================");

        int opcion = ReadOption("Seleccione una opcion: ", 1, 4);

        switch (opcion)
        {
            case 1: SearchBook(); break;
            case 2: SearchUser(); break;
            case 3: ReportsMenu(); break;
            case 4: volver = true; break;
        }
    }
}

static void SearchBook()
{
    ShowModuleAction("Buscar libro",
        "Se permitira buscar libros por titulo, autor o ISBN.");
}

static void SearchUser()
{
    ShowModuleAction("Buscar usuario",
        "Se permitira buscar usuarios por nombre o documento.");
}

static void ReportsMenu()
{
    bool volver = false;

    while (!volver)
    {
        Console.Clear();
        Console.WriteLine("========== REPORTES ==========");
        Console.WriteLine("1. Reporte por usuario");
        Console.WriteLine("2. Reporte por libro");
        Console.WriteLine("3. Prestamos vencidos");
        Console.WriteLine("4. Resumen general");
        Console.WriteLine("5. Volver");
        Console.WriteLine("==============================");

        int opcion = ReadOption("Seleccione una opcion: ", 1, 5);

        switch (opcion)
        {
            case 1: ReportByUser(); break;
            case 2: ReportByBook(); break;
            case 3: ReportOverdue(); break;
            case 4: ReportSummary(); break;
            case 5: volver = true; break;
        }
    }
}

static void ReportByUser()
{
    ShowModuleAction("Reporte por usuario",
        "Se mostraran los prestamos asociados a un usuario.");
}

static void ReportByBook()
{
    ShowModuleAction("Reporte por libro",
        "Se mostrara el historial de prestamos de un libro.");
}

static void ReportOverdue()
{
    ShowModuleAction("Prestamos vencidos",
        "Se identificaran los prestamos que superaron su fecha limite.");
}

static void ReportSummary()
{
    ShowModuleAction("Resumen general",
        "Se mostrara un resumen de libros, usuarios y prestamos.");
}

    static void ShowPersistenceMenu()
{
    bool volver = false;

    while (!volver)
    {
        Console.Clear();
        Console.WriteLine("======= GUARDAR / CARGAR DATOS =======");
        Console.WriteLine("1. Guardar datos");
        Console.WriteLine("2. Cargar datos");
        Console.WriteLine("3. Reiniciar datos");
        Console.WriteLine("4. Volver");
        Console.WriteLine("======================================");

        int opcion = ReadOption("Seleccione una opcion: ", 1, 4);

        switch (opcion)
        {
            case 1: SaveData(); break;
            case 2: LoadData(); break;
            case 3: ResetData(); break;
            case 4: volver = true; break;
        }
    }
}

static void SaveData()
{
    ShowModuleAction("Guardar datos",
        "Se guardaran los datos del sistema.");
}

static void LoadData()
{
    ShowModuleAction("Cargar datos",
        "Se cargaran los datos previamente guardados.");
}

static bool ConfirmResetData()
{
    while (true)
    {
        Console.Write("¿Esta seguro de reiniciar los datos? (S/N): ");
        string? respuesta = Console.ReadLine()?.Trim().ToUpperInvariant();

        if (respuesta == "S") return true;
        if (respuesta == "N") return false;

        Console.WriteLine("Respuesta invalida. Escriba S o N.");
    }
}

static void ResetData()
{
    if (ConfirmResetData())
    {
        ShowModuleAction("Reiniciar datos",
            "Se reiniciaran los datos del sistema.");
    }
    else
    {
        ShowModuleAction("Operacion cancelada",
            "No se realizaron cambios en los datos.");
    }
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
        Console.WriteLine();
        Console.WriteLine("========== CONFIRMAR SALIDA ==========");
        Console.WriteLine("S. Guardar y salir");
        Console.WriteLine("N. Salir sin guardar");
        Console.WriteLine("C. Cancelar y volver al menu");
        Console.WriteLine("======================================");

        Console.Write("Seleccione una opcion: ");
        string? respuesta = Console.ReadLine()?.Trim().ToUpperInvariant();

        switch (respuesta)
        {
            case "S":
                SaveData();
                Console.WriteLine("Saliendo del sistema...");
                return true;

            case "N":
                Console.WriteLine("Saliendo sin guardar...");
                return true;

            case "C":
                Console.WriteLine("Regresando al menu principal...");
                return false;

            default:
                Console.WriteLine("Opcion invalida. Escriba S, N o C.");
                break;
        }
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
        Console.WriteLine(libro1.ResumenCorto());
        Console.WriteLine(libro1.DetalleCompleto());
        Console.WriteLine();
        Console.WriteLine(libro2.ResumenCorto());
        Console.WriteLine(libro2.DetalleCompleto());

        Console.WriteLine("\n--- USUARIOS ---");
        Console.WriteLine(usuario1.ResumenCorto());
        Console.WriteLine(usuario1.DetalleCompleto());
        Console.WriteLine();
        Console.WriteLine(usuario2.ResumenCorto());
        Console.WriteLine(usuario2.DetalleCompleto());

        Console.WriteLine("\n--- PRESTAMO ---");
        Console.WriteLine(prestamo1.ResumenCorto());
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

