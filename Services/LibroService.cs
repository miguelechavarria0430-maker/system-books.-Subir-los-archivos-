using System;
using System.Collections.Generic;
using System.Linq;
using SystemBooks.Models;

namespace SystemBooks.Services;

public class LibroService
{
    private readonly List<Libro> libros = new List<Libro>();

    public void AgregarLibro(Libro libro)
    {
        if (libro == null)
            throw new ArgumentNullException(nameof(libro));

        libros.Add(libro);
    }

    public bool EliminarLibro(int id)
    {
        Libro? libro = libros.FirstOrDefault(l => l.Id == id);

        if (libro == null || !libro.Disponible)
            return false;

        return libros.Remove(libro);
    }

    public List<Libro> ObtenerTodos()
    {
        return libros.ToList();
    }

    public List<Libro> ObtenerDisponibles()
    {
        return libros.Where(l => l.Disponible).ToList();
    }

    public List<Libro> ObtenerPrestados()
    {
        return libros.Where(l => !l.Disponible).ToList();
    }

    public Libro? BuscarPorId(int id)
    {
        return libros.FirstOrDefault(l => l.Id == id);
    }

    public Libro? BuscarPorISBN(string isbn)
    {
        return libros.FirstOrDefault(l =>
            l.ISBN.Equals(isbn, StringComparison.OrdinalIgnoreCase));
    }

    public List<Libro> BuscarPorTitulo(string titulo)
    {
        return libros.Where(l =>
            l.Titulo.Contains(titulo, StringComparison.OrdinalIgnoreCase))
            .ToList();
    }

    public List<Libro> BuscarPorAutor(string autor)
    {
        return libros.Where(l =>
            l.Autor.Contains(autor, StringComparison.OrdinalIgnoreCase))
            .ToList();
    }

    public List<Libro> OrdenarPorTitulo()
    {
        return libros.OrderBy(l => l.Titulo).ToList();
    }

    public int TotalLibros()
    {
        return libros.Count;
    }

    public int TotalDisponibles()
    {
        return libros.Count(l => l.Disponible);
    }

    public int TotalPrestados()
    {
        return libros.Count(l => !l.Disponible);
    }
}