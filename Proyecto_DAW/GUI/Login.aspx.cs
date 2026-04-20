using BE;
using SERVICIOS;
using BLL;
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
    private const int MAXINTENTOS = 3;

    protected void PageLoad(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            txtNombreUsuario.Focus();
        }
    }

    protected void btnIngresarClick(object sender, EventArgs e)
    {
        try
        {
            if (txtNombreUsuario.Text == "" || txtContraseñaUsuario.Text == "") throw new Exception("Faltan ingresar datos");
            if (sessionManager.Gestor.RetornarUsuarioSession() != null) throw new Exception("Ya hay una sesión iniciada");
            else
            {
                if (bllUsuario.ValidarExistenciaNombreUsuario(txtNombreUsuario.Text.Trim()))
                {
                    Usuario usuario = bllUsuario.RetornarUsuarios().Find(x => x.nombreUsuario == txtNombreUsuario.Text);
                    if (bllUsuario.UsuarioActivo(usuario))
                    {
                        if (!(bllUsuario.UsuarioBloqueado(usuario)))
                        {
                            if (bllUsuario.ValidarContraseñaActual(usuario.nombreUsuario, txtContraseñaUsuario.Text))
                            {
                                bllUsuario.ReiniciarIntentos(usuario);

                                sessionManager.Gestor.SetUsuario(usuario);
                                if (bllDigitoVerificador.Deteccion())
                                {
                                    if (usuario.rol == "Administrador")
                                    {
                                        digitoVerificador.ShowDialog();
                                    }
                                    else
                                    {
                                        MessageBox.Show("No se puede iniciar el sistema. Por favor, contactese con un administrador");
                                        sessionManager.Gestor.UnsetUsuario();
                                    }
                                }
                                else
                                {
                                    if (bllUsuario.VerificarContraseñaNoSeaDNIyApellido(sessionManager.Gestor.RetornarUsuarioSession().contraseña))
                                    {
                                        MessageBox.Show("Primer inicio de sesión. Debe cambiar su contraseña", "CAMBIO DE CONTRASEÑA REQUERIDO", MessageBoxButtons.OK);
                                        formularioCambiarContraseña.ShowDialog();
                                    }
                                    sessionManager.Gestor.Idioma = usuario.lenguaje;
                                    TraductorSubject.Instancia.Notificar(usuario.lenguaje);
                                    var usuario = sessionManager.Gestor.RetornarUsuarioSession();
                                    var perfilNombre = usuario.rol;
                                    var permisosSimples = bllUsuario.ObtenerPermisosSimplesDeUsuario(perfilNombre);

                                    sessionManager.Gestor.SetPermisosUsuario(permisosSimples);
                                    bllBitacoraEvento.Alta(sessionManager.Gestor.RetornarUsuarioSession().nombreUsuario, "Iniciar sesión", "Incio de sesión de usuario", 1);
                                    GestorFormulario.gestorFormSG.DefinirEstado(new EstadoMenu());
                                }
                            }
                            else
                            {
                                if (bllUsuario.AumentarIntentos(usuario) == 3)
                                {
                                    MessageBox.Show("Usted a sido bloqueado");
                                }
                                else
                                {
                                    MessageBox.Show("Contraseña incorrecta");
                                }
                            }
                        }
                        else
                        {

                            MessageBox.Show("Usuario bloqueado");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Usuario inactivo");
                    }
                }
                else
                {
                    MessageBox.Show("Usuario no encontrado");
                }
            }
        }
        catch (Exception ex) { MessageBox.Show(ex.Message); }











        // Limpiar paneles de mensajes anteriores
        LimpiarAlertas();

        string nombreUsuario = txtNombreUsuario.Text.Trim();
        string passwordIngresada = txtContraseñaUsuario.Text;

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

            if (intentosActualizados >= MAXINTENTOS)
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
            intentos, MAXINTENTOS
        );

        // Puntitos indicadores
        StringBuilder dots = new StringBuilder();
        for (int i = 1; i <= MAXINTENTOS; i++)
        {
            string css = i <= intentos ? "login-dot login-dot-lleno" : "login-dot";
            dots.AppendFormat("<span class=\"{0}\"></span>", css);
        }
    }

    private void MostrarBloqueado()
    {
        LimpiarAlertas();
        pnlBloqueado.Visible = true;
        btnIngresar.Enabled = false;
        txtNombreUsuario.Enabled = false;
        txtContraseñaUsuario.Enabled = false;
    }

    #endregion

}