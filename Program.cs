using System;
using System.Collections.Generic;
using System.Text;
using SystemBooks.Models;
using SystemBooks.Services;

namespace SystemBooks;

class Program
{
    // ==============================
    // SERVICIOS Y CONTADORES DE ID
    // ==============================
    static readonly LibroService libroService = new LibroService();
    static readonly UsuarioService usuarioService = new UsuarioService();
    static int siguienteIdLibro = 1;
    static int siguienteIdUsuario = 1;

    static void Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;

        if (args.Length > 0 && args[0] == "--pruebas-modelos")
        {
            MostrarPruebasObjetos();
            return;
        }

        if (args.Length > 0 && args[0] == "--array-vs-list")
        {
            CompararArrayVsList();
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
            Console.WriteLine("3. Préstamos");
            Console.WriteLine("4. Búsquedas y reportes");
            Console.WriteLine("5. Guardar / Cargar datos");
            Console.WriteLine("6. Salir");
            Console.WriteLine("====================================");

            int opcion = ReadOption("Seleccione una opción: ", 1, 6);

            switch (opcion)
            {
                case 1: ShowBooksMenu(); break;
                case 2: ShowUsersMenu(); break;
                case 3: ShowLoansMenu(); break;
                case 4: ShowSearchReportsMenu(); break;
                case 5: ShowPersistenceMenu(); break;
                case 6: salir = ConfirmExitAndSave(); break;
            }
        }
    }

    // ==============================
    // UTILIDADES (validación y pausas)
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
                $"Opción inválida. Ingrese un número entre {minimo} y {maximo}.");
        }
    }

    static bool AskYesNo(string mensaje)
    {
        while (true)
        {
            Console.Write(mensaje + " (S/N): ");
            string r = (Console.ReadLine() ?? "").Trim().ToUpperInvariant();

            if (r == "S") return true;
            if (r == "N") return false;

            Console.WriteLine("Respuesta inválida. Escriba S o N.");
        }
    }

    static void Pause(string mensaje = "")
    {
        if (mensaje != "") Console.WriteLine(mensaje);
        Console.WriteLine();
        Console.WriteLine("Presione una tecla para continuar...");
        Console.ReadKey(true);
    }

    static void ShowAction(string titulo, string mensaje)
    {
        Console.Clear();
        Console.WriteLine("====================================");
        Console.WriteLine($"  {titulo.ToUpperInvariant()}");
        Console.WriteLine("====================================");
        Console.WriteLine(mensaje);
        Pause();
    }

    static void Header(string titulo)
    {
        Console.Clear();
        Console.WriteLine($"========== {titulo.ToUpperInvariant()} ==========");
    }

    // ==============================
    // 1) MENU DE LIBROS
    // ==============================
    static void ShowBooksMenu()
    {
        bool volver = false;

        while (!volver)
        {
            Console.Clear();
            Console.WriteLine("====================================");
            Console.WriteLine("          MENÚ DE LIBROS");
            Console.WriteLine("====================================");
            Console.WriteLine("1. Registrar libro");
            Console.WriteLine("2. Listar libros");
            Console.WriteLine("3. Ver detalle (ID/ISBN)");
            Console.WriteLine("4. Actualizar libro");
            Console.WriteLine("5. Eliminar libro");
            Console.WriteLine("6. Volver al menú principal");
            Console.WriteLine("====================================");

            int opcion = ReadOption("Seleccione una opción: ", 1, 6);

            switch (opcion)
            {
                case 1: RegisterBook(); break;
                case 2: ListBooksMenu(); break;
                case 3: ViewBookDetail(); break;
                case 4: UpdateBookMenu(); break;
                case 5: DeleteBook(); break;
                case 6: volver = true; break;
            }
        }
    }

    static void RegisterBook()
    {
        Header("Registrar libro");

        Console.Write("Título: ");
        string titulo = (Console.ReadLine() ?? "").Trim();
        Console.Write("Autor: ");
        string autor = (Console.ReadLine() ?? "").Trim();
        Console.Write("ISBN: ");
        string isbn = (Console.ReadLine() ?? "").Trim();

        if (string.IsNullOrWhiteSpace(titulo) ||
            string.IsNullOrWhiteSpace(autor) ||
            string.IsNullOrWhiteSpace(isbn))
        {
            Pause("Todos los campos son obligatorios.");
            return;
        }

        Console.Write("Año de publicación: ");
        if (!int.TryParse(Console.ReadLine(), out int anio) || anio <= 0)
        {
            Pause("El año ingresado no es válido.");
            return;
        }

        if (libroService.BuscarPorISBN(isbn) != null)
        {
            Pause("Ya existe un libro con ese ISBN.");
            return;
        }

        Libro nuevo = new Libro(siguienteIdLibro, titulo, autor, isbn, anio);
        libroService.AgregarLibro(nuevo);
        siguienteIdLibro++;

        Console.WriteLine();
        Console.WriteLine("Libro registrado correctamente:");
        Console.WriteLine(nuevo.DetalleCompleto());
        Pause();
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

            int opcion = ReadOption("Seleccione una opción: ", 1, 4);

            switch (opcion)
            {
                case 1: ListBooksAll(); break;
                case 2: ListBooksAvailable(); break;
                case 3: ListBooksBorrowed(); break;
                case 4: volver = true; break;
            }
        }
    }

    static void ListBooksAll()
    {
        ShowBookList("Todos los libros", libroService.ObtenerTodos());
    }

    static void ListBooksAvailable()
    {
        ShowBookList("Libros disponibles", libroService.ObtenerDisponibles());
    }

    static void ListBooksBorrowed()
    {
        ShowBookList("Libros prestados", libroService.ObtenerPrestados());
    }

    static void ShowBookList(string titulo, List<Libro> libros)
    {
        Header(titulo);

        if (libros.Count == 0)
            Console.WriteLine("No hay libros para mostrar.");
        else
            foreach (Libro l in libros)
                Console.WriteLine(l.ResumenCorto());

        Pause();
    }

    static void ViewBookDetail()
    {
        Header("Detalle de libro");
        Console.Write("Ingrese el ID o el ISBN: ");
        string entrada = (Console.ReadLine() ?? "").Trim();

        Libro? libro = int.TryParse(entrada, out int id)
            ? libroService.BuscarPorId(id)
            : libroService.BuscarPorISBN(entrada);

        if (libro == null)
        {
            Pause("No se encontró el libro.");
            return;
        }

        Console.WriteLine();
        Console.WriteLine(libro.DetalleCompleto());
        Pause();
    }

    static void UpdateBookMenu()
    {
        bool volver = false;

        while (!volver)
        {
            Console.Clear();
            Console.WriteLine("========== ACTUALIZAR LIBRO ==========");
            Console.WriteLine("1. Editar título");
            Console.WriteLine("2. Editar autor");
            Console.WriteLine("3. Editar año / categoría");
            Console.WriteLine("4. Volver");
            Console.WriteLine("======================================");

            int opcion = ReadOption("Seleccione una opción: ", 1, 4);

            switch (opcion)
            {
                case 1: EditBookTitle(); break;
                case 2: EditBookAuthor(); break;
                case 3: EditBookYearCategory(); break;
                case 4: volver = true; break;
            }
        }
    }

    static Libro? AskBookById()
    {
        Console.Write("ID del libro: ");
        if (!int.TryParse(Console.ReadLine(), out int id))
        {
            Pause("ID inválido.");
            return null;
        }

        Libro? libro = libroService.BuscarPorId(id);
        if (libro == null)
            Pause("No existe un libro con ese ID.");

        return libro;
    }

    static void EditBookTitle()
    {
        Header("Editar título");
        Libro? libro = AskBookById();
        if (libro == null) return;

        Console.Write($"Título actual: {libro.Titulo}\nNuevo título: ");
        string nuevo = (Console.ReadLine() ?? "").Trim();

        if (nuevo == "")
        {
            Pause("El título no puede estar vacío.");
            return;
        }

        libro.Titulo = nuevo;
        Pause("Título actualizado correctamente.");
    }

    static void EditBookAuthor()
    {
        Header("Editar autor");
        Libro? libro = AskBookById();
        if (libro == null) return;

        Console.Write($"Autor actual: {libro.Autor}\nNuevo autor: ");
        string nuevo = (Console.ReadLine() ?? "").Trim();

        if (nuevo == "")
        {
            Pause("El autor no puede estar vacío.");
            return;
        }

        libro.Autor = nuevo;
        Pause("Autor actualizado correctamente.");
    }

    static void EditBookYearCategory()
    {
        Header("Editar año / categoría");
        Libro? libro = AskBookById();
        if (libro == null) return;

        Console.Write($"Año actual: {libro.AnioPublicacion}\nNuevo año: ");
        if (!int.TryParse(Console.ReadLine(), out int anio) || anio <= 0)
        {
            Pause("El año ingresado no es válido.");
            return;
        }

        libro.AnioPublicacion = anio;
        Pause("Año actualizado. (La categoría no existe aún en el modelo Libro.)");
    }

    static void DeleteBook()
    {
        Header("Eliminar libro");
        Libro? libro = AskBookById();
        if (libro == null) return;

        if (!libro.Disponible)
        {
            Pause("No se puede eliminar: el libro está prestado.");
            return;
        }

        if (AskYesNo($"¿Eliminar '{libro.Titulo}'?"))
        {
            libroService.EliminarLibro(libro.Id);
            Pause("Libro eliminado.");
        }
        else
        {
            Pause("Operación cancelada.");
        }
    }

    // ==============================
    // 2) MENU DE USUARIOS
    // ==============================
    static void ShowUsersMenu()
    {
        bool volver = false;

        while (!volver)
        {
            Console.Clear();
            Console.WriteLine("====================================");
            Console.WriteLine("         MENÚ DE USUARIOS");
            Console.WriteLine("====================================");
            Console.WriteLine("1. Registrar usuario");
            Console.WriteLine("2. Listar usuarios");
            Console.WriteLine("3. Ver detalle (ID)");
            Console.WriteLine("4. Actualizar usuario");
            Console.WriteLine("5. Eliminar usuario");
            Console.WriteLine("6. Volver al menú principal");
            Console.WriteLine("====================================");

            int opcion = ReadOption("Seleccione una opción: ", 1, 6);

            switch (opcion)
            {
                case 1: RegisterUser(); break;
                case 2: ListUsers(); break;
                case 3: ViewUserDetail(); break;
                case 4: UpdateUserMenu(); break;
                case 5: DeleteUser(); break;
                case 6: volver = true; break;
            }
        }
    }

    static void RegisterUser()
    {
        Header("Registrar usuario");

        Console.Write("Nombre: ");
        string nombre = (Console.ReadLine() ?? "").Trim();
        Console.Write("Correo: ");
        string correo = (Console.ReadLine() ?? "").Trim();
        Console.Write("Teléfono: ");
        string telefono = (Console.ReadLine() ?? "").Trim();

        if (nombre == "" || correo == "" || telefono == "")
        {
            Pause("Todos los campos son obligatorios.");
            return;
        }

        if (!correo.Contains('@'))
        {
            Pause("El correo no es válido.");
            return;
        }

        Usuario nuevo = new Usuario(siguienteIdUsuario, nombre, correo, telefono);
        usuarioService.AgregarUsuario(nuevo);
        siguienteIdUsuario++;

        Console.WriteLine();
        Console.WriteLine("Usuario registrado correctamente:");
        Console.WriteLine(nuevo.DetalleCompleto());
        Pause();
    }

    static void ListUsers()
    {
        Header("Listar usuarios");
        List<Usuario> usuarios = usuarioService.ObtenerTodos();

        if (usuarios.Count == 0)
            Console.WriteLine("No hay usuarios para mostrar.");
        else
            foreach (Usuario u in usuarios)
                Console.WriteLine($"{u.ResumenCorto()} | Activo: {(u.Activo ? "Sí" : "No")}");

        Pause();
    }

    static Usuario? AskUserById()
    {
        Console.Write("ID del usuario: ");
        if (!int.TryParse(Console.ReadLine(), out int id))
        {
            Pause("ID inválido.");
            return null;
        }

        Usuario? usuario = usuarioService.BuscarPorId(id);
        if (usuario == null)
            Pause("No existe un usuario con ese ID.");

        return usuario;
    }

    static void ViewUserDetail()
    {
        Header("Detalle de usuario");
        Usuario? usuario = AskUserById();
        if (usuario == null) return;

        Console.WriteLine();
        Console.WriteLine(usuario.DetalleCompleto());
        Pause();
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
            Console.WriteLine("3. Activar / desactivar");
            Console.WriteLine("4. Volver");
            Console.WriteLine("========================================");

            int opcion = ReadOption("Seleccione una opción: ", 1, 4);

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
        Header("Editar nombre");
        Usuario? usuario = AskUserById();
        if (usuario == null) return;

        Console.Write($"Nombre actual: {usuario.Nombre}\nNuevo nombre: ");
        string nuevo = (Console.ReadLine() ?? "").Trim();

        if (nuevo == "")
        {
            Pause("El nombre no puede estar vacío.");
            return;
        }

        usuario.Nombre = nuevo;
        Pause("Nombre actualizado correctamente.");
    }

    static void EditUserContact()
    {
        Header("Editar contacto");
        Usuario? usuario = AskUserById();
        if (usuario == null) return;

        Console.Write("Nuevo correo: ");
        string correo = (Console.ReadLine() ?? "").Trim();
        Console.Write("Nuevo teléfono: ");
        string telefono = (Console.ReadLine() ?? "").Trim();

        if (usuario.ActualizarContacto(correo, telefono))
            Pause("Contacto actualizado correctamente.");
        else
            Pause("Correo y teléfono son obligatorios.");
    }

    static void ToggleUserActiveStatus()
    {
        Header("Activar / desactivar usuario");
        Usuario? usuario = AskUserById();
        if (usuario == null) return;

        usuario.CambiarEstado(!usuario.Activo);
        Pause($"El usuario ahora está: {(usuario.Activo ? "ACTIVO" : "INACTIVO")}.");
    }

    static void DeleteUser()
    {
        Header("Eliminar usuario");
        Usuario? usuario = AskUserById();
        if (usuario == null) return;

        Console.WriteLine("Regla: no se permite eliminar si tiene préstamos activos.");

        if (AskYesNo($"¿Eliminar a '{usuario.Nombre}'?"))
        {
            usuarioService.EliminarUsuario(usuario.Id);
            Pause("Usuario eliminado.");
        }
        else
        {
            Pause("Operación cancelada.");
        }
    }

    // ==============================
    // 3) MENU DE PRESTAMOS (mensajes)
    // ==============================
    static void ShowLoansMenu()
    {
        bool volver = false;

        while (!volver)
        {
            Console.Clear();
            Console.WriteLine("====================================");
            Console.WriteLine("        MENÚ DE PRÉSTAMOS");
            Console.WriteLine("====================================");
            Console.WriteLine("1. Crear préstamo");
            Console.WriteLine("2. Listar préstamos");
            Console.WriteLine("3. Ver detalle de préstamo");
            Console.WriteLine("4. Registrar devolución");
            Console.WriteLine("5. Eliminar préstamo");
            Console.WriteLine("6. Volver al menú principal");
            Console.WriteLine("====================================");

            int opcion = ReadOption("Seleccione una opción: ", 1, 6);

            switch (opcion)
            {
                case 1: CreateLoan(); break;
                case 2: ListLoansMenu(); break;
                case 3: ViewLoanDetail(); break;
                case 4: RegisterReturn(); break;
                case 5: DeleteLoan(); break;
                case 6: volver = true; break;
            }
        }
    }

    static void CreateLoan()
    {
        ShowAction("Crear préstamo",
            "Se creará un préstamo nuevo.\n\n" +
            "Validaciones sugeridas:\n" +
            " - El usuario debe existir y estar activo.\n" +
            " - El libro debe existir y estar disponible.\n" +
            " - La fecha límite debe ser posterior a la fecha actual.\n" +
            " - El usuario no debe superar el máximo de préstamos activos.");
    }

    static void ListLoansMenu()
    {
        bool volver = false;

        while (!volver)
        {
            Console.Clear();
            Console.WriteLine("========== LISTAR PRÉSTAMOS ==========");
            Console.WriteLine("1. Todos");
            Console.WriteLine("2. Activos");
            Console.WriteLine("3. Cerrados");
            Console.WriteLine("4. Volver");
            Console.WriteLine("======================================");

            int opcion = ReadOption("Seleccione una opción: ", 1, 4);

            switch (opcion)
            {
                case 1: ListLoansAll(); break;
                case 2: ListLoansActive(); break;
                case 3: ListLoansClosed(); break;
                case 4: volver = true; break;
            }
        }
    }

    static void ListLoansAll() =>
        ShowAction("Listar todos los préstamos", "Se mostrará el listado completo de préstamos.");

    static void ListLoansActive() =>
        ShowAction("Listar préstamos activos", "Se mostrarán los préstamos que aún no han sido devueltos.");

    static void ListLoansClosed() =>
        ShowAction("Listar préstamos cerrados", "Se mostrarán los préstamos ya devueltos.");

    static void ViewLoanDetail() =>
        ShowAction("Detalle de préstamo", "Se consultará un préstamo por su ID.");

    static void RegisterReturn() =>
        ShowAction("Registrar devolución",
            "Se marcará el préstamo como DEVUELTO y el libro quedará DISPONIBLE.");

    static void DeleteLoan() =>
        ShowAction("Eliminar préstamo",
            "Reglas sugeridas:\n" +
            " - Solo se pueden eliminar préstamos cerrados (devueltos).\n" +
            " - No eliminar préstamos activos ni vencidos.");

    // ==============================
    // 4) BUSQUEDAS Y REPORTES
    // ==============================
    static void ShowSearchReportsMenu()
    {
        bool volver = false;

        while (!volver)
        {
            Console.Clear();
            Console.WriteLine("====================================");
            Console.WriteLine("      BÚSQUEDAS Y REPORTES");
            Console.WriteLine("====================================");
            Console.WriteLine("1. Buscar libro");
            Console.WriteLine("2. Buscar usuario");
            Console.WriteLine("3. Reportes");
            Console.WriteLine("4. Volver al menú principal");
            Console.WriteLine("====================================");

            int opcion = ReadOption("Seleccione una opción: ", 1, 4);

            switch (opcion)
            {
                case 1: SearchBook(); break;
                case 2: SearchUser(); break;
                case 3: ShowReportsMenu(); break;
                case 4: volver = true; break;
            }
        }
    }

    static void SearchBook()
    {
        Header("Buscar libro");
        Console.WriteLine("1. Por título");
        Console.WriteLine("2. Por autor");
        Console.WriteLine("3. Por ISBN");
        Console.WriteLine("4. Por ID");
        int criterio = ReadOption("Criterio: ", 1, 4);

        Console.Write("Texto a buscar: ");
        string texto = (Console.ReadLine() ?? "").Trim();

        List<Libro> resultados = new List<Libro>();

        switch (criterio)
        {
            case 1:
                resultados = libroService.BuscarPorTitulo(texto);
                break;
            case 2:
                resultados = libroService.BuscarPorAutor(texto);
                break;
            case 3:
                Libro? porIsbn = libroService.BuscarPorISBN(texto);
                if (porIsbn != null) resultados.Add(porIsbn);
                break;
            case 4:
                if (int.TryParse(texto, out int id))
                {
                    Libro? porId = libroService.BuscarPorId(id);
                    if (porId != null) resultados.Add(porId);
                }
                break;
        }

        Console.WriteLine();
        if (resultados.Count == 0)
            Console.WriteLine("No se encontraron libros.");
        else
            foreach (Libro l in resultados)
                Console.WriteLine(l.ResumenCorto());

        Pause();
    }

    static void SearchUser()
    {
        Header("Buscar usuario");
        Console.WriteLine("1. Por nombre");
        Console.WriteLine("2. Por ID");
        int criterio = ReadOption("Criterio: ", 1, 2);

        Console.Write("Texto a buscar: ");
        string texto = (Console.ReadLine() ?? "").Trim();

        List<Usuario> resultados = new List<Usuario>();

        if (criterio == 1)
        {
            resultados = usuarioService.BuscarPorNombre(texto);
        }
        else if (int.TryParse(texto, out int id))
        {
            Usuario? u = usuarioService.BuscarPorId(id);
            if (u != null) resultados.Add(u);
        }

        Console.WriteLine();
        if (resultados.Count == 0)
            Console.WriteLine("No se encontraron usuarios.");
        else
            foreach (Usuario u in resultados)
                Console.WriteLine(u.ResumenCorto());

        Pause();
    }

    static void ShowReportsMenu()
    {
        bool volver = false;

        while (!volver)
        {
            Console.Clear();
            Console.WriteLine("========== REPORTES ==========");
            Console.WriteLine("1. Por usuario");
            Console.WriteLine("2. Por libro");
            Console.WriteLine("3. Vencidos");
            Console.WriteLine("4. Resumen general");
            Console.WriteLine("5. Volver");
            Console.WriteLine("==============================");

            int opcion = ReadOption("Seleccione una opción: ", 1, 5);

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

    static void ReportByUser() =>
        ShowAction("Reporte por usuario", "Se mostrarán los préstamos asociados a un usuario.");

    static void ReportByBook() =>
        ShowAction("Reporte por libro", "Se mostrará el historial de préstamos de un libro.");

    static void ReportOverdue() =>
        ShowAction("Reporte de vencidos", "Se mostrarán los préstamos cuya fecha límite ya pasó.");

    static void ReportSummary()
    {
        Header("Resumen general");
        Console.WriteLine("--- Libros ---");
        Console.WriteLine($"Total: {libroService.TotalLibros()}");
        Console.WriteLine($"Disponibles: {libroService.TotalDisponibles()}");
        Console.WriteLine($"Prestados: {libroService.TotalPrestados()}");
        Console.WriteLine();
        Console.WriteLine("--- Usuarios ---");
        Console.WriteLine($"Total: {usuarioService.TotalUsuarios()}");
        Console.WriteLine($"Activos: {usuarioService.TotalActivos()}");
        Console.WriteLine($"Inactivos: {usuarioService.TotalInactivos()}");
        Pause();
    }

    // ==============================
    // 5) GUARDAR / CARGAR DATOS
    // ==============================
    static void ShowPersistenceMenu()
    {
        bool volver = false;

        while (!volver)
        {
            Console.Clear();
            Console.WriteLine("====================================");
            Console.WriteLine("       GUARDAR / CARGAR DATOS");
            Console.WriteLine("====================================");
            Console.WriteLine("1. Guardar datos");
            Console.WriteLine("2. Cargar datos");
            Console.WriteLine("3. Reiniciar datos");
            Console.WriteLine("4. Volver al menú principal");
            Console.WriteLine("====================================");

            int opcion = ReadOption("Seleccione una opción: ", 1, 4);

            switch (opcion)
            {
                case 1: SaveData(); break;
                case 2: LoadData(); break;
                case 3: ResetData(); break;
                case 4: volver = true; break;
            }
        }
    }

    static void SaveData() =>
        ShowAction("Guardar datos", "Se guardarán los datos del sistema (simulado).");

    static void LoadData() =>
        ShowAction("Cargar datos", "Se cargarán los datos guardados del sistema (simulado).");

    static void ResetData()
    {
        if (ConfirmResetData())
            ShowAction("Reiniciar datos", "Los datos serán reiniciados (simulado).");
        else
            ShowAction("Reiniciar datos", "Operación cancelada. No se modificó nada.");
    }

    static bool ConfirmResetData()
    {
        Console.Clear();
        Console.WriteLine("ADVERTENCIA: se borrarán todos los datos.");
        return AskYesNo("¿Está seguro de reiniciar los datos?");
    }

    // ==============================
    // 6) SALIR
    // ==============================
    static bool ConfirmExitAndSave()
    {
        Console.Clear();

        if (AskYesNo("¿Guardar antes de salir?"))
            SaveData();

        Console.WriteLine("Gracias por usar el Sistema de Biblioteca. ¡Hasta pronto!");
        return true;
    }

    // ==============================
    // PRUEBAS DE MODELOS (dotnet run -- --pruebas-modelos)
    // ==============================
    static void MostrarPruebasObjetos()
    {
        Console.WriteLine("===== PRUEBA DE OBJETOS: LIBROS =====");
        Libro l1 = new Libro(1, "Cien años de soledad", "Gabriel García Márquez", "978-0307474728", 1967);
        Libro l2 = new Libro(2, "El coronel no tiene quien le escriba", "Gabriel García Márquez", "978-0060753610", 1961);

        Console.WriteLine(l1.ResumenCorto());
        Console.WriteLine(l1.DetalleCompleto());
        Console.WriteLine(l2.ResumenCorto());
        Console.WriteLine(l2.DetalleCompleto());
        Console.WriteLine($"Disponible l1: {l1.Disponible}");
        Console.WriteLine($"Disponible l2: {l2.Disponible}");

        Console.WriteLine();
        Console.WriteLine("===== PRUEBA DE OBJETOS: USUARIOS =====");
        Usuario u1 = new Usuario(1, "Ana Pérez", "ana@correo.com", "3001112233");
        Usuario u2 = new Usuario(2, "Luis Gómez", "luis@correo.com", "3004445566");

        Console.WriteLine(u1.ResumenCorto());
        Console.WriteLine(u1.DetalleCompleto());
        Console.WriteLine(u2.ResumenCorto());
        Console.WriteLine(u2.DetalleCompleto());
        Console.WriteLine($"Activo u1: {u1.Activo}");
        Console.WriteLine($"Activo u2: {u2.Activo}");

        // NOTA: aquí va la parte del préstamo (Estado, EstaVencido(), DiasTranscurridos())
        // si quieres conservarla, copia aquí tu código original de MostrarPruebasObjetos.
    }

    // ==============================
    // ARRAY VS LIST (dotnet run -- --array-vs-list)
    // ==============================
    static void CompararArrayVsList()
    {
        Console.WriteLine("===== ARRAY =====");
        string[] titulosArray = new string[2];   // tamaño fijo
        titulosArray[0] = "Libro A";
        titulosArray[1] = "Libro B";
        // titulosArray[2] = "Libro C"; // ERROR: fuera de rango, no se puede crecer
        Console.WriteLine($"Elementos en el array: {titulosArray.Length}");

        Console.WriteLine();
        Console.WriteLine("===== LIST =====");
        List<string> titulosList = new List<string>();   // tamaño dinámico
        titulosList.Add("Libro A");
        titulosList.Add("Libro B");
        titulosList.Add("Libro C");   // se puede seguir agregando
        titulosList.Remove("Libro A"); // y eliminando
        Console.WriteLine($"Elementos en la lista: {titulosList.Count}");

        Console.WriteLine();
        Console.WriteLine("DIFERENCIA:");
        Console.WriteLine("- El array tiene tamaño fijo definido al crearlo.");
        Console.WriteLine("- La List crece y se reduce dinámicamente (Add/Remove).");
        Console.WriteLine("- Por eso los Services usan List<T> para gestionar los datos.");
    }
}