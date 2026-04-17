<%@ Page Title="Iniciar sesión" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="Login" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="head" runat="server">
    <link href="Estilos/EstilosLogin.css" rel="stylesheet" />
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="login-wrapper">
        <div class="login-card">

            <!-- Encabezado -->
            <div class="login-card-header">
                <div class="login-icon-circle">&#x1F43E;</div> <!-- Huellas de Perro -->
                <h2 class="login-title">Bienvenido</h2>
                <p class="login-subtitle">Ingresá tus credenciales para continuar</p>
            </div>

            <!-- Cuerpo -->
            <div class="login-card-body">

                <!-- Panel: cuenta bloqueada -->
                <asp:Panel ID="pnlBloqueado" runat="server" CssClass="login-alert login-alert-bloqueado" Visible="false">
                    &#x1F512; Tu cuenta fue bloqueada por 3 intentos fallidos consecutivos.
                    Comunicate con el WebMaster para desbloquearla.
                </asp:Panel>

                <!-- Panel: credenciales incorrectas -->
                <asp:Panel ID="pnlErrorLogin" runat="server" CssClass="login-alert login-alert-error" Visible="false">
                    &#x26A0; <asp:Literal ID="litMensajeError" runat="server" Text="Usuario o contraseña incorrectos." />
                </asp:Panel>

                <!-- Panel: aviso de intentos restantes -->
                <asp:Panel ID="pnlIntentos" runat="server" CssClass="login-alert login-alert-intentos" Visible="false">
                    <asp:Literal ID="litIntentos" runat="server" />
                </asp:Panel>

                <!-- Campo: nombre de usuario -->
                <div class="login-field">
                    <label class="login-label" for="txtUsuario">Nombre de usuario</label>
                    <asp:TextBox
                        ID="txtUsuario"
                        runat="server"
                        CssClass="login-input"
                        placeholder="Ej: lara.palermo/sair.barreto/seba.fernandez"
                        MaxLength="50" />
                    <asp:RequiredFieldValidator
                        ID="rfvUsuario"
                        runat="server"
                        ControlToValidate="txtUsuario"
                        ErrorMessage="El nombre de usuario es obligatorio."
                        CssClass="login-validacion"
                        Display="Dynamic"
                        ValidationGroup="vgLogin" />
                </div>

                <!-- Campo: contraseña -->
                <div class="login-field">
                    <label class="login-label" for="txtPassword">Contraseña</label>
                    <div class="login-pass-wrap">
                        <asp:TextBox
                            ID="txtPassword"
                            runat="server"
                            CssClass="login-input"
                            TextMode="Password"
                            placeholder="••••••••"
                            MaxLength="100" />
                        <button type="button" class="login-btn-ojo" onclick="togglePassword(this)" title="Mostrar/ocultar contraseña">
                            &#x1F441;
                        </button>
                    </div>
                    <asp:RequiredFieldValidator
                        ID="rfvPassword"
                        runat="server"
                        ControlToValidate="txtPassword"
                        ErrorMessage="La contraseña es obligatoria."
                        CssClass="login-validacion"
                        Display="Dynamic"
                        ValidationGroup="vgLogin" />
                </div>

                <!-- Indicador visual de intentos -->
                <asp:Panel ID="pnlDots" runat="server" CssClass="login-dots-fila" Visible="false">
                    <asp:Literal ID="litDots" runat="server" />
                </asp:Panel>

                <!-- Botón principal -->
                <asp:Button
                    ID="btnIngresar"
                    runat="server"
                    Text="Ingresar al sistema"
                    CssClass="login-btn-primary"
                    OnClick="btnIngresar_Click"
                    ValidationGroup="vgLogin" />

            </div>
        </div>
    </div>

    <!-- TEMPORAL (DESPUES PASARLO A UN ARCHIVO JS) -->
    <script type="text/javascript">
        function togglePassword(btn) {
            var wrap = btn.parentElement;
            var input = wrap.querySelector('input[type="password"], input[type="text"]');
            if (input.type === 'password') {
                input.type = 'text';
                btn.title = 'Ocultar contraseña';
            } else {
                input.type = 'password';
                btn.title = 'Mostrar contraseña';
            }
        }
    </script>

</asp:Content>

