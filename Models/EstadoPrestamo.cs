namespace SystemBooks.Models;

/// <summary>
/// Define los posibles estados de un prestamo.
/// </summary>
public enum EstadoPrestamo
{
    /// <summary>El prestamo sigue activo.</summary>
    Activo,

    /// <summary>El libro fue devuelto.</summary>
    Devuelto,

    /// <summary>El prestamo supero su fecha de vencimiento.</summary>
    Vencido
}
