using System;
using System.Collections.Generic;
using System.Linq;
using SystemBooks.Models;

namespace SystemBooks.Services;

public class UsuarioService
{
    private readonly List<Usuario> usuarios = new List<Usuario>();

    public void AgregarUsuario(Usuario usuario)
    {
        if (usuario == null)
            throw new ArgumentNullException(nameof(usuario));

        usuarios.Add(usuario);
    }

    public bool EliminarUsuario(int id)
    {
        Usuario? usuario = usuarios.FirstOrDefault(u => u.Id == id);

        if (usuario == null)
            return false;

        return usuarios.Remove(usuario);
    }

    public List<Usuario> ObtenerTodos()
    {
        return usuarios.ToList();
    }

    public Usuario? BuscarPorId(int id)
    {
        return usuarios.FirstOrDefault(u => u.Id == id);
    }

    public List<Usuario> BuscarPorNombre(string nombre)
    {
        return usuarios.Where(u =>
            u.Nombre.Contains(nombre, StringComparison.OrdinalIgnoreCase))
            .ToList();
    }

    public List<Usuario> OrdenarPorNombre()
    {
        return usuarios.OrderBy(u => u.Nombre).ToList();
    }

    public int TotalUsuarios()
    {
        return usuarios.Count;
    }

    public int TotalActivos()
    {
        return usuarios.Count(u => u.Activo);
    }

    public int TotalInactivos()
    {
        return usuarios.Count(u => !u.Activo);
    }
}
