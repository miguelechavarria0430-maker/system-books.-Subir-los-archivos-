using System;
using System.Collections.Generic;
using System.Linq;
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
    static readonly PrestamoService prestamoService = new PrestamoService();
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

        bool tieneActivos = prestamoService
            .BuscarPorUsuario(usuario.Id)
            .Any(p => p.Estado == EstadoPrestamo.Activo);

        if (tieneActivos)
        {
            Pause("No se puede eliminar: el usuario tiene préstamos activos.");
            return;
        }

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
    // 3) MENU DE PRESTAMOS
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
        Header("Crear préstamo");
        Console.WriteLine("Validaciones: usuario existente y activo, libro existente y disponible,");
        Console.WriteLine("días de préstamo entre 1 y 60.");
        Console.WriteLine();

        Usuario? usuario = AskUserById();
        if (usuario == null) return;

        if (!usuario.Activo)
        {
            Pause("No se puede crear el préstamo: el usuario está inactivo.");
            return;
        }

        Libro? libro = AskBookById();
        if (libro == null) return;

        if (!libro.Disponible)
        {
            Pause("No se puede crear el préstamo: el libro no está disponible.");
            return;
        }

        Console.Write("Días de préstamo (1-60): ");
        if (!int.TryParse(Console.ReadLine(), out int dias) || dias < 1 || dias > 60)
        {
            Pause("Cantidad de días inválida.");
            return;
        }

        Prestamo nuevo = new Prestamo(
            0, libro, usuario, DateTime.Today, DateTime.Today.AddDays(dias));

        if (!prestamoService.RegistrarPrestamo(nuevo))
        {
            Pause("No se pudo registrar el préstamo.");
            return;
        }

        Console.WriteLine();
        Console.WriteLine("Préstamo creado correctamente:");
        Console.WriteLine(nuevo.DetalleCompleto());
        Pause();
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

    static void ShowLoanList(string titulo, List<Prestamo> prestamos)
    {
        Header(titulo);

        if (prestamos.Count == 0)
            Console.WriteLine("No hay préstamos para mostrar.");
        else
            foreach (Prestamo p in prestamos)
                Console.WriteLine(p.ResumenCorto());

        Pause();
    }

    static void ListLoansAll() =>
        ShowLoanList("Todos los préstamos", prestamoService.ObtenerTodos());

    static void ListLoansActive() =>
        ShowLoanList("Préstamos activos", prestamoService.ObtenerActivos());

    static void ListLoansClosed() =>
        ShowLoanList("Préstamos cerrados", prestamoService.ObtenerDevueltos());

    static Prestamo? AskLoanById()
    {
        Console.Write("ID del préstamo: ");
        if (!int.TryParse(Console.ReadLine(), out int id))
        {
            Pause("ID inválido.");
            return null;
        }

        Prestamo? prestamo = prestamoService.BuscarPorId(id);
        if (prestamo == null)
            Pause("No existe un préstamo con ese ID.");

        return prestamo;
    }

    static void ViewLoanDetail()
    {
        Header("Detalle de préstamo");
        Prestamo? prestamo = AskLoanById();
        if (prestamo == null) return;

        Console.WriteLine();
        Console.WriteLine(prestamo.DetalleCompleto());
        Pause();
    }

    static void RegisterReturn()
    {
        Header("Registrar devolución");
        Prestamo? prestamo = AskLoanById();
        if (prestamo == null) return;

        if (prestamoService.RegistrarDevolucion(prestamo.Id))
            Pause("Préstamo marcado como DEVUELTO. El libro ahora está DISPONIBLE.");
        else
            Pause("No se pudo registrar: el préstamo ya fue devuelto.");
    }

    static void DeleteLoan()
    {
        Header("Eliminar préstamo");
        Console.WriteLine("Regla: solo se pueden eliminar préstamos cerrados (devueltos).");
        Console.WriteLine();

        Prestamo? prestamo = AskLoanById();
        if (prestamo == null) return;

        if (prestamo.Estado != EstadoPrestamo.Devuelto)
        {
            Pause("Solo se pueden eliminar préstamos cerrados (devueltos).");
            return;
        }

        if (AskYesNo($"¿Eliminar el préstamo #{prestamo.Id}?"))
        {
            prestamoService.EliminarPrestamo(prestamo.Id);
            Pause("Préstamo eliminado.");
        }
        else
        {
            Pause("Operación cancelada.");
        }
    }

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

    static void ReportByUser()
    {
        Header("Reporte por usuario");
        Usuario? usuario = AskUserById();
        if (usuario == null) return;

        Console.WriteLine($"\nPréstamos de {usuario.Nombre}:");
        List<Prestamo> lista = prestamoService.BuscarPorUsuario(usuario.Id);

        if (lista.Count == 0)
            Console.WriteLine("Este usuario no tiene préstamos.");
        else
            foreach (Prestamo p in lista)
                Console.WriteLine(p.ResumenCorto());

        Pause();
    }

    static void ReportByBook()
    {
        Header("Reporte por libro");
        Libro? libro = AskBookById();
        if (libro == null) return;

        Console.WriteLine($"\nHistorial de '{libro.Titulo}':");
        List<Prestamo> lista = prestamoService.BuscarPorLibro(libro.Id);

        if (lista.Count == 0)
            Console.WriteLine("Este libro no tiene préstamos.");
        else
            foreach (Prestamo p in lista)
                Console.WriteLine(p.ResumenCorto());

        Pause();
    }

    static void ReportOverdue() =>
        ShowLoanList("Préstamos vencidos", prestamoService.ObtenerVencidos());

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
        Console.WriteLine();
        Console.WriteLine("--- Préstamos ---");
        Console.WriteLine($"Total: {prestamoService.TotalPrestamos()}");
        Console.WriteLine($"Activos: {prestamoService.TotalActivos()}");
        Console.WriteLine($"Vencidos: {prestamoService.TotalVencidos()}");
        Console.WriteLine($"Devueltos: {prestamoService.TotalDevueltos()}");
        Console.WriteLine($"Promedio de días: {prestamoService.PromedioDiasPrestamo():F1}");
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
        Console.WriteLine("===== LIBROS =====");
        Libro l1 = new Libro(1, "Cien años de soledad", "Gabriel García Márquez", "978-0307474728", 1967);
        Libro l2 = new Libro(2, "El coronel no tiene quien le escriba", "Gabriel García Márquez", "978-0060753610", 1961);

        Console.WriteLine(l1.ResumenCorto());
        Console.WriteLine(l1.DetalleCompleto());
        Console.WriteLine(l2.ResumenCorto());
        Console.WriteLine(l2.DetalleCompleto());
        Console.WriteLine($"Disponible l1: {l1.Disponible}");
        Console.WriteLine($"Disponible l2: {l2.Disponible}");

        Console.WriteLine();
        Console.WriteLine("===== USUARIOS =====");
        Usuario u1 = new Usuario(1, "Ana Pérez", "ana@correo.com", "3001112233");
        Usuario u2 = new Usuario(2, "Luis Gómez", "luis@correo.com", "3004445566");

        Console.WriteLine(u1.ResumenCorto());
        Console.WriteLine(u1.DetalleCompleto());
        Console.WriteLine(u2.ResumenCorto());
        Console.WriteLine(u2.DetalleCompleto());
        Console.WriteLine($"Activo u1: {u1.Activo}");
        Console.WriteLine($"Activo u2: {u2.Activo}");

        Console.WriteLine();
        Console.WriteLine("===== PRÉSTAMO =====");
        Prestamo p1 = new Prestamo(
            1, l1, u1, DateTime.Today.AddDays(-20), DateTime.Today.AddDays(-5));
        l1.Disponible = false;

        Console.WriteLine(p1.ResumenCorto());
        Console.WriteLine(p1.DetalleCompleto());
        Console.WriteLine($"Estado: {p1.Estado}");
        Console.WriteLine($"EstaVencido(): {p1.EstaVencido()}");
        Console.WriteLine($"DiasTranscurridos(): {p1.DiasTranscurridos()}");
        Console.WriteLine($"FechaDevolucion es null: {p1.FechaDevolucion == null}");
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
        titulosList.Add("Libro C");    // se puede seguir agregando
        titulosList.Remove("Libro A"); // y eliminando
        Console.WriteLine($"Elementos en la lista: {titulosList.Count}");

        Console.WriteLine();
        Console.WriteLine("DIFERENCIA:");
        Console.WriteLine("- El array tiene tamaño fijo definido al crearlo.");
        Console.WriteLine("- La List crece y se reduce dinámicamente (Add/Remove).");
        Console.WriteLine("- Por eso los Services usan List<T> para gestionar los datos.");
    }
}