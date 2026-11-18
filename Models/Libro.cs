
namespace SystemBooks.Models;

/// <summary>
/// Representa un libro registrado en la biblioteca.
/// </summary>
public class Libro
{
    public int Id { get; set; }
    public string Titulo { get; set; }
    public string Autor { get; set; }
    public string ISBN { get; set; }
    public int AnioPublicacion { get; set; }
    public bool Disponible { get; set; }

    public Libro()
    {
        Titulo = string.Empty;
        Autor = string.Empty;
        ISBN = string.Empty;
        Disponible = true;
    }

    public Libro(int id, string titulo, string autor, string isbn, int anioPublicacion)
    {
        Id = id;
        Titulo = titulo;
        Autor = autor;
        ISBN = isbn;
        AnioPublicacion = anioPublicacion;
        Disponible = true;
    }

    public string ResumenCorto()
    {
        return $"[{Id}] {Titulo} - {Autor}";
    }

    public string DetalleCompleto()
    {
        return $"ID: {Id}\n" +
               $"Título: {Titulo}\n" +
               $"Autor: {Autor}\n" +
               $"ISBN: {ISBN}\n" +
               $"Año de publicación: {AnioPublicacion}\n" +
               $"Disponible: {(Disponible ? "Sí" : "No")}";
    }

    public override string ToString()
    {
        return ResumenCorto();
    }
}
