<%@ Page Title="Gestion de Usuarios" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="GestionUsuarios.aspx.cs" Inherits="GestionUsuarios" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="head" runat="server">
    <link href="Estilos/EstilosRegistroAnimales.css" rel="stylesheet" />
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="page-container">

        <h2 class="titulo" runat="server" data-traduccion="pageUsuarios_titGestionUsuarios">Gestion de Usuarios</h2>
                        <!-- POPUP 1: Alta -->
                <asp:Panel ID="pnlPopupAlta" runat="server" Visible="false" CssClass="popup-overlay">
                    <div class="popup-box">
                        <h3 runat="server" data-traduccion="pageUsuarios_titNuevoUsuario">Nuevo usuario</h3>
                        <label runat="server" data-traduccion="pageUsuarios_lblDni">DNI</label>
                        <asp:TextBox ID="txtDniAlta" runat="server" CssClass="input" />
                        <asp:RequiredFieldValidator ControlToValidate="txtDniAlta" ErrorMessage="Requerido" data-traduccion="pageUsuarios_valRequerido" ValidationGroup="vgAlta" runat="server" ForeColor="Red"/><br />

                        <label runat="server" data-traduccion="pageUsuarios_lblNombre">Nombre</label>
                        <asp:TextBox ID="txtNombreAlta" runat="server" CssClass="input" />
                        <asp:RequiredFieldValidator ControlToValidate="txtNombreAlta" ErrorMessage="Requerido" data-traduccion="pageUsuarios_valRequerido" ValidationGroup="vgAlta" runat="server" ForeColor="Red"/><br />

                        <label runat="server" data-traduccion="pageUsuarios_lblApellido">Apellido</label>
                        <asp:TextBox ID="txtApellidoAlta" runat="server" CssClass="input" />
                        <asp:RequiredFieldValidator ControlToValidate="txtApellidoAlta" ErrorMessage="Requerido" data-traduccion="pageUsuarios_valRequerido" ValidationGroup="vgAlta" runat="server" ForeColor="Red"/><br />

                        <label runat="server" data-traduccion="pageUsuarios_lblRol">Rol</label>
                        <%--<asp:DropDownList ID="ddlRolAlta" runat="server" CssClass="input">
                            <asp:ListItem Text="Usuario" Value="usuario" />
                            <asp:ListItem Text="Admin" Value="admin" />
                            <asp:ListItem Text="Web Master" Value="webmaster" />
                        </asp:DropDownList><br />--%>
                        <asp:DropDownList ID="ddlRolAlta" runat="server" CssClass="input" /><br />

                        <label runat="server" data-traduccion="pageUsuarios_lblEmail">Email</label>
                        <asp:TextBox ID="txtEmailAlta" runat="server" CssClass="input" />
                        <asp:RequiredFieldValidator ControlToValidate="txtEmailAlta" ErrorMessage="Requerido" data-traduccion="pageUsuarios_valRequerido" ValidationGroup="vgAlta" runat="server" ForeColor="Red"/><br />

                        <label runat="server" data-traduccion="pageUsuarios_lblContrasena">Contraseña</label>
                        <asp:TextBox ID="txtContraseñaAlta" runat="server" TextMode="Password" CssClass="input" />
                        <asp:RequiredFieldValidator ControlToValidate="txtContraseñaAlta" ErrorMessage="Requerido" data-traduccion="pageUsuarios_valRequerido" ValidationGroup="vgAlta" runat="server" ForeColor="Red"/><br />
                        <asp:RegularExpressionValidator runat="server"
                            ControlToValidate="txtContraseñaAlta"
                            ValidationExpression="^(?=.*[a-z])(?=.*[A-Z])(?=.*[^a-zA-Z0-9]).{8,}$"
                            ErrorMessage="8+ caracteres, mayúscula, minúscula y un carácter especial." data-traduccion="pageUsuarios_valFormatoContrasena"
                            ValidationGroup="vgAlta" Display="Dynamic" CssClass="validation-error" />

                        <label runat="server" data-traduccion="pageUsuarios_lblConfirmarContrasena">Confirmar contraseña</label>
                        <asp:TextBox ID="txtConfirmarContraseñaAlta" runat="server" TextMode="Password" CssClass="input" />
                        <asp:CompareValidator runat="server"
                            ControlToValidate="txtConfirmarContraseñaAlta"
                            ControlToCompare="txtContraseñaAlta"
                            ErrorMessage="Las contraseñas no coinciden." data-traduccion="pageUsuarios_valContrasenasNoCoinciden"
                            ValidationGroup="vgAlta" Display="Dynamic" CssClass="validation-error" /><br />

                        <asp:Button ID="btnGuardarAlta" runat="server" Text="Guardar" data-traduccion="pageUsuarios_btnGuardar" CssClass="btn" OnClick="btnGuardarAlta_Click" ValidationGroup="vgAlta"/>
                        <asp:Button ID="btnCancelarAlta" runat="server" Text="Cancelar" data-traduccion="pageUsuarios_btnCancelar" CssClass="btn secundario" OnClick="btnCancelarAlta_Click" CausesValidation="false"/>
                    </div>
                </asp:Panel>

                <!-- POPUP 2: Cambiar contraseña -->
                <asp:Panel ID="pnlPopupContraseña" runat="server" Visible="false" CssClass="popup-overlay">
                    <div class="popup-box">
                        <h3 runat="server" data-traduccion="pageUsuarios_titCambiarContrasena">Cambiar contraseña</h3>
                        <label runat="server" data-traduccion="pageUsuarios_lblNuevaContrasena">Nueva contraseña</label>
                        <asp:TextBox ID="txtNuevaContraseñaPopup" runat="server" TextMode="Password" CssClass="input" />
                        <asp:RequiredFieldValidator ControlToValidate="txtNuevaContraseñaPopup" ErrorMessage="Requerido" data-traduccion="pageUsuarios_valRequerido" ValidationGroup="vgPassword" runat="server" ForeColor="Red" /><br />

                        <label runat="server" data-traduccion="pageUsuarios_lblConfirmarContrasena">Confirmar contraseña</label>
                        <asp:TextBox ID="txtConfirmarContraseñaPopup" runat="server" TextMode="Password" CssClass="input" />
                        <br />
                        <br />
                        <asp:CompareValidator runat="server"
                            ControlToValidate="txtConfirmarContraseñaPopup"
                            ControlToCompare="txtNuevaContraseñaPopup"
                            ErrorMessage="Las contraseñas no coinciden." data-traduccion="pageUsuarios_valContrasenasNoCoinciden"
                            ValidationGroup="vgPassword" Display="Dynamic" CssClass="validation-error" />
                        <br />
                        <asp:RegularExpressionValidator runat="server"
                            ControlToValidate="txtNuevaContraseñaPopup"
                            ValidationExpression="^(?=.*[a-z])(?=.*[A-Z])(?=.*[^a-zA-Z0-9]).{8,}$"
                            ErrorMessage="8+ caracteres, mayúscula, minúscula y un carácter especial." data-traduccion="pageUsuarios_valFormatoContrasena"
                            ValidationGroup="vgPassword" Display="Dynamic" CssClass="validation-error" />
                        <br />

                        <asp:Button ID="btnGuardarContraseña" runat="server" Text="Guardar" data-traduccion="pageUsuarios_btnGuardar" CssClass="btn" OnClick="btnGuardarContraseña_Click" ValidationGroup="vgPassword"/>
                        <asp:Button ID="btnCancelarContraseña" runat="server" Text="Cancelar" data-traduccion="pageUsuarios_btnCancelar" CssClass="btn secundario" OnClick="btnCancelarContraseña_Click" CausesValidation="false"/>
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
                    EmptyDataText="No hay usuarios para mostrar" data-traduccion-vacio="pageUsuarios_msgGrillaVacia">
                    <Columns>
                        <asp:BoundField DataField="dni" HeaderText="DNI" AccessibleHeaderText="pageUsuarios_colDni" />
                        <asp:BoundField DataField="nombreUsuario" HeaderText="Usuario" AccessibleHeaderText="pageUsuarios_colUsuario" />
                        <asp:BoundField DataField="nombre" HeaderText="Nombre" AccessibleHeaderText="pageUsuarios_colNombre" />
                        <asp:BoundField DataField="apellido" HeaderText="Apellido" AccessibleHeaderText="pageUsuarios_colApellido" />
                        <asp:BoundField DataField="rol" HeaderText="Rol" AccessibleHeaderText="pageUsuarios_colRol" />
                        <asp:BoundField DataField="email" HeaderText="Email" AccessibleHeaderText="pageUsuarios_colEmail" />
                        <asp:BoundField DataField="activo" HeaderText="Activo" AccessibleHeaderText="pageUsuarios_colActivo" />
                        <asp:BoundField DataField="bloqueo" HeaderText="Bloqueado" AccessibleHeaderText="pageUsuarios_colBloqueado" />
                    </Columns>
                </asp:GridView>
            </div>

            <%-- FORM --%>
            <div class="form-box">
                <div class="form-inputs">

                    <label runat="server" data-traduccion="pageUsuarios_lblDni">DNI</label>
                    <asp:TextBox ID="txtDni" runat="server" CssClass="input" MaxLength="15" />

                    <label runat="server" data-traduccion="pageUsuarios_lblNombre">Nombre</label>
                    <asp:TextBox ID="txtNombre" runat="server" CssClass="input" />

                    <label runat="server" data-traduccion="pageUsuarios_lblApellido">Apellido</label>
                    <asp:TextBox ID="txtApellido" runat="server" CssClass="input" />

                    <label runat="server" data-traduccion="pageUsuarios_lblEmail">Email</label>
                    <asp:TextBox ID="txtEmail" runat="server" CssClass="input" TextMode="Email" />

                    <label runat="server" data-traduccion="pageUsuarios_lblNombreUsuario">Nombre Usuario</label>
                    <asp:TextBox ID="txtNombreUsuario" runat="server" CssClass="input" />
                    <label runat="server" data-traduccion="pageUsuarios_lblRol">Rol</label>
                    <%--<asp:DropDownList ID="ddlRol" runat="server" CssClass="input">
                        <asp:ListItem Text="admin" Value="admin" />
                        <asp:ListItem Text="adoptante" Value="adoptante" />
                        <asp:ListItem Text="empleado" Value="empleado" />
                        <asp:ListItem Text="webmaster" Value="webmaster" />
                    </asp:DropDownList>--%>
                    <asp:DropDownList ID="ddlRol" runat="server" CssClass="input" />

                    <label runat="server" data-traduccion="pageUsuarios_lblActivo">Activo</label>
                    <asp:DropDownList ID="ddlActivo" runat="server" CssClass="input">
                        <asp:ListItem Text="Si" Value="true" data-traduccion="pageUsuarios_optSi" />
                        <asp:ListItem Text="No" Value="false" data-traduccion="pageUsuarios_optNo" />
                    </asp:DropDownList>

                    <asp:Panel ID="pnlAlerta" runat="server" Visible="false" CssClass="alert">
                        <asp:Label ID="lbMensaje" runat="server" Text=""></asp:Label>
                    </asp:Panel>

                </div>



                    <asp:HiddenField ID="hdnDniSeleccionado" runat="server" />
                <%-- BOTONES --%>
                <div class="form-buttons">
                    <asp:Button ID="btnNuevoUsuario" runat="server" Text="Nuevo usuario" data-traduccion="pageUsuarios_btnNuevoUsuario" CssClass="btn" OnClick="btnNuevoUsuario_Click" CausesValidation="false" data-permiso="USUARIO_ALTA"/>
                    <asp:Button ID="btnCambiarContraseña" runat="server" Text="Cambiar contraseña" data-traduccion="pageUsuarios_btnCambiarContrasena" CssClass="btn" OnClick="btnCambiarContraseña_Click" CausesValidation="false" data-permiso="MODIFICAR_PASSWORD"/>

                    <asp:Button ID="btnModificar" runat="server" Text="Modificar" data-traduccion="pageUsuarios_btnModificar" CssClass="btn" OnClick="btnModificar_Click" data-permiso="USUARIO_MODIFICAR"/>
                    <asp:Button ID="btnDesbloquear" runat="server" Text="Desbloquear" data-traduccion="pageUsuarios_btnDesbloquear" CssClass="btn secundario" OnClick="btnDesbloquear_Click" data-permiso="USUARIO_DESBLOQUEAR"/>
                    <hr />

                    <asp:Button ID="btnLimpiar" runat="server" Text="Limpiar" data-traduccion="pageUsuarios_btnLimpiar" CssClass="btn secundario" OnClick="btnLimpiar_Click"/>
                </div>

            </div>
        </div>

    </div>

</asp:Content>
