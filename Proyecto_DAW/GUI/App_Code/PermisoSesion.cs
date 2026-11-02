using BE;
using BLL;
using SERVICIOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

// Claves de permiso del usuario logueado, cacheadas en Session.
// Está en App_Code para que la usen tanto la MasterPage como la API (que no tiene MasterPage).
public static class PermisoSesion
{
    public static HashSet<string> ObtenerClavesUsuario()
    {
        Usuario usuario = claseSession.Gestor.RetornarUsuarioSession();
        if (usuario == null)
            return new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        var session = HttpContext.Current.Session;
        List<string> cacheadas = session["ClavesUsuario"] as List<string>;
        if (cacheadas != null)
            return new HashSet<string>(cacheadas, StringComparer.OrdinalIgnoreCase);

        bllRol bllRol = new bllRol();
        Rol rol = bllRol.ObtenerRoles().FirstOrDefault(r => r.Nombre == usuario.rol);

        HashSet<string> claves = (rol == null)
            ? new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            : PermisoHelper.ObtenerClaves(bllRol.ObtenerAccesosPorRol(rol.ID));

        session["ClavesUsuario"] = claves.ToList();
        return claves;
    }

    public static bool TienePermiso(string clave)
    {
        return ObtenerClavesUsuario().Contains(clave);
    }
}
