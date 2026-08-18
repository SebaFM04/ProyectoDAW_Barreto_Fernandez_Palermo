using BE;
using SERVICIOS;
using SERVICIOS.MultiIdioma_Observer;
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
    bllUsuario bllU;
    bllBitacora bllBitacora;
    bllDigitoVerificador bllDigitoVerificador;

    protected void Page_Load(object sender, EventArgs e)
    {
        
        bllU = new bllUsuario();
        bllBitacora = new bllBitacora();
        bllDigitoVerificador = new bllDigitoVerificador();
    }

    // Atajo para traducir mensajes propios de esta página.
    private string TraducirMsg(string claveMensaje, string textoEspanolFallback)
    {
        return GestorIdioma.Instancia.TraducirMensaje("Login", claveMensaje, textoEspanolFallback);
    }

    protected void btnIngresar_Click(object sender, EventArgs e)
    {
        // Limpiar paneles de mensajes anteriores
        LimpiarAlertas();
        if (!Page.IsValid)
            return;
        if (txtNombreUsuario.Text == "" || txtContraseñaUsuario.Text == "")
        {
            lblMensajeError.Text = TraducirMsg("MSG_FALTAN_DATOS", "Faltan ingresar datos");
            pnlAlerta.CssClass = "login-alert login-alert-error";
            ActivarAlertas();
            return;
        }
        if (claseSession.Gestor.RetornarUsuarioSession() != null)
        {
            lblMensajeError.Text = TraducirMsg("MSG_SESION_YA_INICIADA", "Ya hay una sesión iniciada");
            pnlAlerta.CssClass = "login-alert login-alert-error";
            ActivarAlertas();
            return;
        }
        else
        {
            if (bllU.ValidarExistenciaNombreUsuario(txtNombreUsuario.Text.Trim()))
            {
                Usuario usuario = bllU.RetornarUsuarios().Find(x => x.nombreUsuario == txtNombreUsuario.Text);
                if (bllU.UsuarioActivo(usuario))
                {
                    if (!(bllU.UsuarioBloqueado(usuario)))
                    {
                        if (bllU.ValidarContraseñaActual(usuario.nombreUsuario, txtContraseñaUsuario.Text))
                        {
                            bllU.ReiniciarIntentos(usuario);
                            claseSession.Gestor.SetUsuario(usuario);

                            // Carga en el GestorIdioma (Sujeto del Observer) el idioma
                            // preferido del usuario para el resto de la sesión.
                            new bllIdioma().InicializarIdioma(usuario.codigoIdioma);

                            // ===== DETECCIÓN DEL DÍGITO VERIFICADOR =====
                            if (bllDigitoVerificador.Deteccion())
                            {
                                Session["AccesoDigitoVerificador"] = true;
                                // Hay inconsistencias: según el rol, va a una pantalla distinta
                                if (usuario.rol == "web master")
                                {
                                    // El Web Master puede recalcular / restaurar
                                    Response.Redirect("DigitoVerificadorWebMaster.aspx");
                                }
                                else if (usuario.rol == "admin")
                                {
                                    // El admin solo ve el aviso para contactar al Web Master
                                    Response.Redirect("DigitoVerificadorAdmin.aspx");
                                }
                                else
                                {
                                    // Cualquier otro rol: pantalla de no disponible
                                    Response.Redirect("DigitoVerificadorUsuario.aspx");
                                }

                            }
                            else
                            {
                                // No hay inconsistencias → login normal
                                bllBitacora.Alta(claseSession.Gestor.RetornarUsuarioSession().nombreUsuario,
                                                 "Usuario", "Inicio de sesión de usuario", 1);
                                Response.Redirect("MenuPrincipal.aspx");
                            }
                        }
                        else
                        {
                            if (bllU.Intentos(usuario) == 3)
                            {
                                lblMensajeError.Text = TraducirMsg("MSG_USUARIO_BLOQUEADO_AHORA", "Usted ha sido bloqueado");
                                pnlAlerta.CssClass = "login-alert login-alert-error";
                                ActivarAlertas();
                                return;
                            }
                            else
                            {
                                lblMensajeError.Text = TraducirMsg("MSG_CREDENCIALES_INCORRECTAS", "Contraseña o usuario incorrecto");
                                pnlAlerta.CssClass = "login-alert login-alert-error";
                                ActivarAlertas();
                                return;
                            }
                        }
                    }
                    else
                    {
                        lblMensajeError.Text = TraducirMsg("MSG_USUARIO_BLOQUEADO", "Usuario bloqueado");
                        pnlAlerta.CssClass = "login-alert login-alert-error";
                        ActivarAlertas();
                        return;
                    }
                }
                else
                {
                    lblMensajeError.Text = TraducirMsg("MSG_USUARIO_INACTIVO", "Usuario inactivo");
                    pnlAlerta.CssClass = "login-alert login-alert-intentos";
                    ActivarAlertas();
                    return;
                }
            }
            else
            {
                lblMensajeError.Text = TraducirMsg("MSG_USUARIO_NO_ENCONTRADO_LOGIN", "Usuario no encontrado");
                pnlAlerta.CssClass = "login-alert login-alert-error";
                ActivarAlertas();
                return;
            }
        }
    }

    private void LimpiarAlertas()
    {
        pnlAlerta.Visible = false;
    }

    private void ActivarAlertas()
    {
        pnlAlerta.Visible = true;
    }

}