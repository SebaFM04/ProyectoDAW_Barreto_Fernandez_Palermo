using BE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Login : System.Web.UI.Page
{
    private const int MAX_INTENTOS = 3;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            txtUsuario.Focus();
        }
    }

    protected void btnIngresar_Click(object sender, EventArgs e)
    {
        // Limpiar paneles de mensajes anteriores
        LimpiarAlertas();

        string nombreUsuario = txtUsuario.Text.Trim();
        string passwordIngresada = txtPassword.Text;

        // Aplicar hash SHA-256 a la contraseña ingresada antes de comparar
        string hashIngresado = HashSHA256(passwordIngresada);

        // Consulta a BLL 
        // UsuarioBLL usuarioBLL = new UsuarioBLL();
        // Usuario usuario = usuarioBLL.ObtenerPorNombreUsuario(nombreUsuario);

        // TEMPORAL (eliminar al conectar con BLL)
        // Reemplazar este bloque con la lógica real de BLL
        Usuario usuario = null; // Aquí irá la llamada a BLL 
        
        if (usuario == null)
        {
            MostrarErrorGenerico();
            return;
        }

        // Verificar si la cuenta está bloqueada
        if (usuario.Bloqueado)
        {
            MostrarBloqueado();
            return;
        }

        // Verificar contraseña comparando hashes
        if (usuario.Contrasena != hashIngresado)
        {
            int intentosActualizados = usuario.Intentos + 1;

            // Actualizar intentos en BLL
            // usuarioBLL.ActualizarIntentos(usuario.DNI, intentosActualizados);

            if (intentosActualizados >= MAX_INTENTOS)
            {
                // Bloquear usuario en BLL 
                // usuarioBLL.BloquearUsuario(usuario.DNI);
                MostrarBloqueado();
            }
            else
            {
                MostrarIntentoFallido(intentosActualizados);
            }
            return;
        }

        // Login exitoso: resetear intentos y crear sesión
        // Resetear en BLL
        // usuarioBLL.ResetearIntentos(usuario.DNI);

        Session["UsuarioActual"] = usuario;
        Session["Rol"] = usuario.Rol;
        Session["NombreUsuario"] = usuario.NombreUsuario;

        // Registrar en bitácora 
        // BitacoraBLL bitacora = new BitacoraBLL();
        // bitacora.Registrar(usuario.NombreUsuario, "Login", "Inicio de sesión exitoso", criticidad: 4);

        // Redirigir según rol
        switch (usuario.Rol.ToLower())
        {
            case "webmaster":
            case "administrador":
                Response.Redirect("~/Admin/Default.aspx"); //Ejemplos
                break;
            default:
                Response.Redirect("~/Default.aspx");
                break;
        }
    }

    #region Métodos de UI para mostrar estados de error

    private void LimpiarAlertas()
    {
        pnlErrorLogin.Visible = false;
        pnlBloqueado.Visible = false;
        pnlIntentos.Visible = false;
        pnlDots.Visible = false;
    }

    private void MostrarErrorGenerico()
    {
        pnlErrorLogin.Visible = true;
        litMensajeError.Text = "Usuario o contraseña incorrectos.";
    }

    private void MostrarIntentoFallido(int intentos)
    {
        // Mensaje de error
        pnlErrorLogin.Visible = true;
        litMensajeError.Text = "Usuario o contraseña incorrectos.";

        // Aviso de intentos restantes
        pnlIntentos.Visible = true;
        litIntentos.Text = string.Format(
            "Intento {0} de {1}. Tu cuenta se bloqueará al tercer intento fallido.",
            intentos, MAX_INTENTOS
        );

        // Puntitos indicadores
        pnlDots.Visible = true;
        StringBuilder dots = new StringBuilder();
        for (int i = 1; i <= MAX_INTENTOS; i++)
        {
            string css = i <= intentos ? "login-dot login-dot-lleno" : "login-dot";
            dots.AppendFormat("<span class=\"{0}\"></span>", css);
        }
        litDots.Text = dots.ToString();
    }

    private void MostrarBloqueado()
    {
        LimpiarAlertas();
        pnlBloqueado.Visible = true;
        btnIngresar.Enabled = false;
        txtUsuario.Enabled = false;
        txtPassword.Enabled = false;
    }

    #endregion

    #region Encriptación SHA-256

    public static string HashSHA256(string input)
    {
        using (SHA256 sha = SHA256.Create())
        {
            byte[] bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(input));
            StringBuilder sb = new StringBuilder();
            foreach (byte b in bytes)
                sb.Append(b.ToString("x2"));
            return sb.ToString();
        }
    }

    #endregion
}