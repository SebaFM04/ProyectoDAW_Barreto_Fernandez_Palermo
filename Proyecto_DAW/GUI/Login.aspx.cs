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
    bllUsuario bllU;
    bllBitacora bllBitacora;
    bllDigitoVerificador bllDigitoVerificador;

    protected void Page_Load(object sender, EventArgs e)
    {
        
        bllU = new bllUsuario();
        bllBitacora = new bllBitacora();
        bllDigitoVerificador = new bllDigitoVerificador();
    }

    protected void btnIngresar_Click(object sender, EventArgs e)
    {
        // Limpiar paneles de mensajes anteriores
        LimpiarAlertas();
        if (txtNombreUsuario.Text == "" || txtContraseñaUsuario.Text == "")
        {
            lblMensajeError.Text = "Faltan ingresar datos";
            pnlAlerta.CssClass = "login-alert login-alert-error";
            ActivarAlertas();
            return;
        }
        if (claseSession.Gestor.RetornarUsuarioSession() != null)
        {
            lblMensajeError.Text = "Ya hay una sesión iniciada";
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

                            // ===== DETECCIÓN DEL DÍGITO VERIFICADOR =====
                            if (bllDigitoVerificador.Deteccion())
                            {
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
                                lblMensajeError.Text = "Usted ha sido bloqueado";
                                pnlAlerta.CssClass = "login-alert login-alert-error";
                                ActivarAlertas();
                                return;
                            }
                            else
                            {
                                lblMensajeError.Text = "Contraseña o usuario incorrecto";
                                pnlAlerta.CssClass = "login-alert login-alert-error";
                                ActivarAlertas();
                                return;
                            }
                        }
                    }
                    else
                    {
                        lblMensajeError.Text = "Usuario bloqueado";
                        pnlAlerta.CssClass = "login-alert login-alert-error";
                        ActivarAlertas();
                        return;
                    }
                }
                else
                {
                    lblMensajeError.Text = "Usuario inactivo";
                    pnlAlerta.CssClass = "login-alert login-alert-intentos";
                    ActivarAlertas();
                    return;
                }
            }
            else
            {
                lblMensajeError.Text = "Usuario no encontrado";
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