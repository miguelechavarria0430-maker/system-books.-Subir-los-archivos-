using System;
using System.Collections.Generic;
using System.Linq;
using SystemBooks.Models;

namespace SystemBooks.Services;

public class PrestamoService
{
    private readonly List<Prestamo> prestamos = new();
    private int siguienteId = 1;

    public bool RegistrarPrestamo(Prestamo prestamo)
    {
        if (prestamo == null)
            throw new ArgumentNullException(nameof(prestamo));

        if (!prestamo.Usuario.Activo || !prestamo.Libro.Disponible)
            return false;

        if (prestamos.Any(p =>
            p.Libro.Id == prestamo.Libro.Id &&
            p.Estado == EstadoPrestamo.Activo))
            return false;

        prestamo.Id = siguienteId++;
        prestamo.Estado = EstadoPrestamo.Activo;
        prestamo.FechaPrestamo = DateTime.Today;

        if (prestamo.FechaVencimiento.Date <= DateTime.Today)
            prestamo.FechaVencimiento = DateTime.Today.AddDays(15);

        prestamo.Libro.Disponible = false;
        prestamos.Add(prestamo);

        return true;
    }

    public Prestamo? BuscarPorId(int id)
    {
        return prestamos.FirstOrDefault(p => p.Id == id);
    }

    public List<Prestamo> ObtenerTodos()
    {
        return prestamos.ToList();
    }

    public List<Prestamo> BuscarPorUsuario(int usuarioId)
    {
        return prestamos.Where(p => p.Usuario.Id == usuarioId).ToList();
    }

    public List<Prestamo> BuscarPorLibro(int libroId)
    {
        return prestamos.Where(p => p.Libro.Id == libroId).ToList();
    }

    public bool RegistrarDevolucion(int id)
    {
        Prestamo? prestamo = BuscarPorId(id);

        if (prestamo == null)
            return false;

        bool devuelto = prestamo.RegistrarDevolucion(DateTime.Today);

        if (!devuelto)
            return false;

        prestamo.Libro.Disponible = true;
        return true;
    }

    public bool RenovarPrestamo(int id, int diasAdicionales)
    {
        Prestamo? prestamo = BuscarPorId(id);

        if (prestamo == null)
            return false;

        return prestamo.Renovar(diasAdicionales);
    }

    public List<Prestamo> ObtenerActivos()
    {
        return prestamos.Where(p => p.Estado == EstadoPrestamo.Activo).ToList();
    }

    public List<Prestamo> ObtenerVencidos()
    {
        return prestamos.Where(p => p.EstaVencido()).ToList();
    }

    public List<Prestamo> ObtenerDevueltos()
    {
        return prestamos.Where(p => p.Estado == EstadoPrestamo.Devuelto).ToList();
    }

    public int TotalPrestamos()
    {
        return prestamos.Count;
    }

    public int TotalActivos()
    {
        return ObtenerActivos().Count;
    }

    public int TotalVencidos()
    {
        return ObtenerVencidos().Count;
    }

    public int TotalDevueltos()
    {
        return ObtenerDevueltos().Count;
    }
}
