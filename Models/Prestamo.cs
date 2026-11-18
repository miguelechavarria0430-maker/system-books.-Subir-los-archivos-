
namespace SystemBooks.Models;

/// <summary>
/// Representa el préstamo de un libro a un usuario.
/// </summary>
public class Prestamo
{
    public int Id { get; set; }
    public Libro Libro { get; set; }
    public Usuario Usuario { get; set; }
    public DateTime FechaPrestamo { get; set; }
    public DateTime FechaVencimiento { get; set; }
    public DateTime? FechaDevolucion { get; set; }
    public EstadoPrestamo Estado { get; set; }

    public Prestamo()
    {
        Libro = new Libro();
        Usuario = new Usuario();
        FechaPrestamo = DateTime.Today;
        FechaVencimiento = DateTime.Today.AddDays(15);
        FechaDevolucion = null;
        Estado = EstadoPrestamo.Activo;
    }

    public Prestamo(
        int id,
        Libro libro,
        Usuario usuario,
        DateTime fechaPrestamo,
        DateTime fechaVencimiento)
    {
        Id = id;
        Libro = libro ?? throw new ArgumentNullException(nameof(libro));
        Usuario = usuario ?? throw new ArgumentNullException(nameof(usuario));
        FechaPrestamo = fechaPrestamo;
        FechaVencimiento = fechaVencimiento;
        FechaDevolucion = null;
        Estado = EstadoPrestamo.Activo;
    }

    public bool EstaVencido()
    {
        return Estado == EstadoPrestamo.Activo
            && FechaDevolucion == null
            && DateTime.Today > FechaVencimiento.Date;
    }

    public int DiasTranscurridos()
    {
        DateTime fechaFinal = FechaDevolucion?.Date ?? DateTime.Today;
        return Math.Max(0, (fechaFinal - FechaPrestamo.Date).Days);
    }

    public string ResumenCorto()
    {
        return $"Préstamo #{Id} | {Libro.Titulo} | {Usuario.Nombre} | {Estado}";
    }

    public string DetalleCompleto()
    {
        return $"ID del préstamo: {Id}\n" +
               $"Libro: {Libro.Titulo}\n" +
               $"Usuario: {Usuario.Nombre}\n" +
               $"Fecha del préstamo: {FechaPrestamo:dd/MM/yyyy}\n" +
               $"Fecha de vencimiento: {FechaVencimiento:dd/MM/yyyy}\n" +
               $"Fecha de devolución: {(FechaDevolucion.HasValue ? FechaDevolucion.Value.ToString("dd/MM/yyyy") : "Pendiente")}\n" +
               $"Estado: {Estado}\n" +
               $"¿Está vencido?: {(EstaVencido() ? "Sí" : "No")}\n" +
               $"Días transcurridos: {DiasTranscurridos()}";
    }

    public override string ToString()
    {
        return ResumenCorto();
    }
}
