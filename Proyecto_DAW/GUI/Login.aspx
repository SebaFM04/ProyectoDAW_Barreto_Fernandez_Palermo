<%@ Page Title="Iniciar sesión" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="Login" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="head" runat="server">
    <link href="Estilos/EstilosLogin.css" rel="stylesheet" />
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="login-wrapper">
        <div class="login-card">

            <!-- Encabezado -->
            <div class="login-card-header">
                <div class="login-icon-circle">&#x1F43E;</div>
                <h2 class="login-title">Bienvenido</h2>
                <p class="login-subtitle">Ingresá tus credenciales para continuar</p>
            </div>

            <!-- Cuerpo -->
            <div class="login-card-body">

                <!-- Panel: cuenta bloqueada -->
                <asp:Panel ID="pnlBloqueado" runat="server" CssClass="login-alert login-alert-bloqueado" Visible="false">
                    &#x1F512; Tu cuenta fue bloqueada por 3 intentos fallidos consecutivos.
                </asp:Panel>

                <!-- Panel: error -->
                <asp:Panel ID="pnlErrorLogin" runat="server" CssClass="login-alert login-alert-error" Visible="false">
                    &#x26A0; <asp:Literal ID="litMensajeError" runat="server" />
                </asp:Panel>

                <!-- Panel: intentos -->
                <asp:Panel ID="pnlIntentos" runat="server" CssClass="login-alert login-alert-intentos" Visible="false">
                    <asp:Literal ID="litIntentos" runat="server" />
                </asp:Panel>

                <!-- Usuario -->
                <div class="login-field">
                    <label class="login-label">Nombre de usuario</label>
                    <asp:TextBox ID="txtNombreUsuario" runat="server" CssClass="login-input" MaxLength="50" />
                </div>

                <!-- Password -->
                <div class="login-field">
                    <label class="login-label">Contraseña</label>

                    <div class="login-pass-wrap">
                        <asp:TextBox
                            ID="txtContraseñaUsuario"
                            runat="server"
                            CssClass="login-input"
                            TextMode="Password"
                            ClientIDMode="Static" />

                        <button type="button"
                                class="login-btn-ojo toggle-password"
                                title="Mostrar/ocultar contraseña">
                            👁
                        </button>
                    </div>
                </div>

                <!-- Botón -->
                <asp:Button
                    ID="btnIngresar"
                    runat="server"
                    Text="Ingresar al sistema"
                    CssClass="login-btn-primary"
                    OnClick="btnIngresar_Click" />

            </div>
        </div>
    </div>

    <!-- JS -->
    <script src="Scripts/ScriptLogin.js"></script>
    <script src="Scripts/Alertas.js"></script>
</asp:Content>

