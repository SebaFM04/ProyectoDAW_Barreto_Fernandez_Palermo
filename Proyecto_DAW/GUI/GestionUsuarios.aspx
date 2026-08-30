<%@ Page Title="Gestion de Usuarios" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="GestionUsuarios.aspx.cs" Inherits="GestionUsuarios" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="head" runat="server">
    <link href="Estilos/EstilosRegistroAnimales.css" rel="stylesheet" />
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="page-container">

        <h2 class="titulo">Gestion de Usuarios</h2>
                        <!-- POPUP 1: Alta -->
                <asp:Panel ID="pnlPopupAlta" runat="server" Visible="false" CssClass="popup-overlay">
                    <div class="popup-box">
                        <h3>Nuevo usuario</h3>
                        <label>DNI</label>
                        <asp:TextBox ID="txtDniAlta" runat="server" CssClass="input" />
                        <asp:RequiredFieldValidator ControlToValidate="txtDniAlta" ErrorMessage="Requerido" ValidationGroup="vgAlta" runat="server" ForeColor="Red"/><br />

                        <label>Nombre</label>
                        <asp:TextBox ID="txtNombreAlta" runat="server" CssClass="input" />
                        <asp:RequiredFieldValidator ControlToValidate="txtNombreAlta" ErrorMessage="Requerido" ValidationGroup="vgAlta" runat="server" ForeColor="Red"/><br />

                        <label>Apellido</label>
                        <asp:TextBox ID="txtApellidoAlta" runat="server" CssClass="input" />
                        <asp:RequiredFieldValidator ControlToValidate="txtApellidoAlta" ErrorMessage="Requerido" ValidationGroup="vgAlta" runat="server" ForeColor="Red"/><br />

                        <label>Rol</label>
                        <%--<asp:DropDownList ID="ddlRolAlta" runat="server" CssClass="input">
                            <asp:ListItem Text="Usuario" Value="usuario" />
                            <asp:ListItem Text="Admin" Value="admin" />
                            <asp:ListItem Text="Web Master" Value="webmaster" />
                        </asp:DropDownList><br />--%>
                        <asp:DropDownList ID="ddlRolAlta" runat="server" CssClass="input" /><br />

                        <label>Email</label>
                        <asp:TextBox ID="txtEmailAlta" runat="server" CssClass="input" />
                        <asp:RequiredFieldValidator ControlToValidate="txtEmailAlta" ErrorMessage="Requerido" ValidationGroup="vgAlta" runat="server" ForeColor="Red"/><br />

                        <label>Contraseña</label>
                        <asp:TextBox ID="txtContraseñaAlta" runat="server" TextMode="Password" CssClass="input" />
                        <asp:RequiredFieldValidator ControlToValidate="txtContraseñaAlta" ErrorMessage="Requerido" ValidationGroup="vgAlta" runat="server" ForeColor="Red"/><br />
                        <asp:RegularExpressionValidator runat="server"
                            ControlToValidate="txtContraseñaAlta"
                            ValidationExpression="^(?=.*[a-z])(?=.*[A-Z])(?=.*[^a-zA-Z0-9]).{8,}$"
                            ErrorMessage="8+ caracteres, mayúscula, minúscula y un carácter especial."
                            ValidationGroup="vgAlta" Display="Dynamic" CssClass="validation-error" />

                        <label>Confirmar contraseña</label>
                        <asp:TextBox ID="txtConfirmarContraseñaAlta" runat="server" TextMode="Password" CssClass="input" />
                        <asp:CompareValidator runat="server"
                            ControlToValidate="txtConfirmarContraseñaAlta"
                            ControlToCompare="txtContraseñaAlta"
                            ErrorMessage="Las contraseñas no coinciden."
                            ValidationGroup="vgAlta" Display="Dynamic" CssClass="validation-error" /><br />

                        <asp:Button ID="btnGuardarAlta" runat="server" Text="Guardar" CssClass="btn" OnClick="btnGuardarAlta_Click" ValidationGroup="vgAlta"/>
                        <asp:Button ID="btnCancelarAlta" runat="server" Text="Cancelar" CssClass="btn secundario" OnClick="btnCancelarAlta_Click" CausesValidation="false"/>
                    </div>
                </asp:Panel>

                <!-- POPUP 2: Cambiar contraseña -->
                <asp:Panel ID="pnlPopupContraseña" runat="server" Visible="false" CssClass="popup-overlay">
                    <div class="popup-box">
                        <h3>Cambiar contraseña</h3>
                        <label>Nueva contraseña</label>
                        <asp:TextBox ID="txtNuevaContraseñaPopup" runat="server" TextMode="Password" CssClass="input" />
                        <asp:RequiredFieldValidator ControlToValidate="txtNuevaContraseñaPopup" ErrorMessage="Requerido" ValidationGroup="vgPassword" runat="server" ForeColor="Red" /><br />

                        <label>Confirmar contraseña</label>
                        <asp:TextBox ID="txtConfirmarContraseñaPopup" runat="server" TextMode="Password" CssClass="input" />
                        <br />
                        <br />
                        <asp:CompareValidator runat="server"
                            ControlToValidate="txtConfirmarContraseñaPopup"
                            ControlToCompare="txtNuevaContraseñaPopup"
                            ErrorMessage="Las contraseñas no coinciden."
                            ValidationGroup="vgPassword" Display="Dynamic" CssClass="validation-error" />
                        <br />
                        <asp:RegularExpressionValidator runat="server"
                            ControlToValidate="txtNuevaContraseñaPopup"
                            ValidationExpression="^(?=.*[a-z])(?=.*[A-Z])(?=.*[^a-zA-Z0-9]).{8,}$"
                            ErrorMessage="8+ caracteres, mayúscula, minúscula y un carácter especial."
                            ValidationGroup="vgPassword" Display="Dynamic" CssClass="validation-error" />
                        <br />

                        <asp:Button ID="btnGuardarContraseña" runat="server" Text="Guardar" CssClass="btn" OnClick="btnGuardarContraseña_Click" ValidationGroup="vgPassword"/>
                        <asp:Button ID="btnCancelarContraseña" runat="server" Text="Cancelar" CssClass="btn secundario" OnClick="btnCancelarContraseña_Click" CausesValidation="false"/>
                    </div>
                </asp:Panel>
        <div class="layout">

            <%-- GRID --%>
            <div class="grid-container">
                <asp:GridView ID="gvUsuarios" runat="server"
                    CssClass="grid"
                    AutoGenerateColumns="false"
                    AutoGenerateSelectButton="true"
                    DataKeyNames="dni"
                    OnSelectedIndexChanged="gvUsuarios_SelectedIndexChanged"
                    EmptyDataText="No hay usuarios para mostrar">
                    <Columns>
                        <asp:BoundField DataField="dni" HeaderText="DNI" />
                        <asp:BoundField DataField="nombreUsuario" HeaderText="Usuario" />
                        <asp:BoundField DataField="nombre" HeaderText="Nombre" />
                        <asp:BoundField DataField="apellido" HeaderText="Apellido" />
                        <asp:BoundField DataField="rol" HeaderText="Rol" />
                        <asp:BoundField DataField="email" HeaderText="Email" />
                        <asp:BoundField DataField="activo" HeaderText="Activo" />
                        <asp:BoundField DataField="bloqueo" HeaderText="Bloqueado" />
                    </Columns>
                </asp:GridView>
            </div>

            <%-- FORM --%>
            <div class="form-box">
                <div class="form-inputs">

                    <label>DNI</label>
                    <asp:TextBox ID="txtDni" runat="server" CssClass="input" MaxLength="15" />

                    <label>Nombre</label>
                    <asp:TextBox ID="txtNombre" runat="server" CssClass="input" />

                    <label>Apellido</label>
                    <asp:TextBox ID="txtApellido" runat="server" CssClass="input" />

                    <label>Email</label>
                    <asp:TextBox ID="txtEmail" runat="server" CssClass="input" TextMode="Email" />

                    <label>Nombre Usuario</label>
                    <asp:TextBox ID="txtNombreUsuario" runat="server" CssClass="input" />
                    <label>Rol</label>
                    <%--<asp:DropDownList ID="ddlRol" runat="server" CssClass="input">
                        <asp:ListItem Text="admin" Value="admin" />
                        <asp:ListItem Text="adoptante" Value="adoptante" />
                        <asp:ListItem Text="empleado" Value="empleado" />
                        <asp:ListItem Text="webmaster" Value="webmaster" />
                    </asp:DropDownList>--%>
                    <asp:DropDownList ID="ddlRol" runat="server" CssClass="input" />

                    <label>Activo</label>
                    <asp:DropDownList ID="ddlActivo" runat="server" CssClass="input">
                        <asp:ListItem Text="Si" Value="true" />
                        <asp:ListItem Text="No" Value="false" />
                    </asp:DropDownList>

                    <asp:Panel ID="pnlAlerta" runat="server" Visible="false" CssClass="alert">
                        <asp:Label ID="lbMensaje" runat="server" Text=""></asp:Label>
                    </asp:Panel>

                </div>



                    <asp:HiddenField ID="hdnDniSeleccionado" runat="server" />
                <%-- BOTONES --%>
                <div class="form-buttons">
                    <asp:Button ID="btnNuevoUsuario" runat="server" Text="Nuevo usuario" CssClass="btn" OnClick="btnNuevoUsuario_Click" CausesValidation="false" data-permiso="USUARIO_ALTA"/>
                    <asp:Button ID="btnCambiarContraseña" runat="server" Text="Cambiar contraseña" CssClass="btn" OnClick="btnCambiarContraseña_Click" CausesValidation="false" data-permiso="MODIFICAR_PASSWORD"/>

                    <asp:Button ID="btnModificar" runat="server" Text="Modificar" CssClass="btn" OnClick="btnModificar_Click" data-permiso="USUARIO_MODIFICAR"/>
                    <asp:Button ID="btnDesbloquear" runat="server" Text="Desbloquear" CssClass="btn secundario" OnClick="btnDesbloquear_Click" data-permiso="USUARIO_DESBLOQUEAR"/>
                    <hr />

                    <asp:Button ID="btnLimpiar" runat="server" Text="Limpiar" CssClass="btn secundario" OnClick="btnLimpiar_Click"/>
                </div>

            </div>
        </div>

    </div>

</asp:Content>
