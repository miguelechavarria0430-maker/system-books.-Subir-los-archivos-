
namespace SystemBooks.Models;

/// <summary>
/// Representa a un usuario registrado en la biblioteca.
/// </summary>
public class Usuario
{
    public int Id { get; set; }
    public string Documento { get; set; }
    public string Nombre { get; set; }
    public string Correo { get; set; }
    public string Telefono { get; set; }
    public bool Activo { get; set; }

    public Usuario()
    {
        Documento = string.Empty;
        Nombre = string.Empty;
        Correo = string.Empty;
        Telefono = string.Empty;
        Activo = true;
    }

    // Constructor anterior
    public Usuario(int id, string nombre, string correo, string telefono)
        : this(id, string.Empty, nombre, correo, telefono)
    {
    }

    // Constructor con documento
    public Usuario(int id, string documento, string nombre, string correo, string telefono)
    {
        Id = id;
        Documento = documento;
        Nombre = nombre;
        Correo = correo;
        Telefono = telefono;
        Activo = true;
    }

    public string ResumenCorto()
    {
        return $"[{Id}] {Nombre} - {Correo}";
    }

    public string DetalleCompleto()
    {
        return $"ID: {Id}\n" +
               $"Documento: {Documento}\n" +
               $"Nombre: {Nombre}\n" +
               $"Correo: {Correo}\n" +
               $"Telefono: {Telefono}\n" +
               $"Activo: {(Activo ? "Si" : "No")}";
    }

    public bool ActualizarContacto(string correo, string telefono)
    {
        if (string.IsNullOrWhiteSpace(correo) ||
            string.IsNullOrWhiteSpace(telefono))
            return false;

        Correo = correo;
        Telefono = telefono;
        return true;
    }

    public void CambiarEstado(bool activo)
    {
        Activo = activo;
    }

    public override string ToString()
    {
        return ResumenCorto();
    }
}
