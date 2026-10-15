<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.master" CodeFile="RolesFamilias.aspx.cs" Inherits="RolesFamilias" %>
<%@ MasterType VirtualPath="~/MasterPage.master" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="head" runat="server">
    <link href="Estilos/EstilosRegistroAnimales.css" rel="stylesheet" />
    <link href="Estilos/RolesFamilias.css" rel="stylesheet" />
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="page-container">

        <h2 class="titulo" runat="server" data-traduccion="pageRolesFamilias_titRolesFamilias">Roles y Familias</h2>

        <div class="pf-layout">

            <%-- COLUMNA 1: permisos simples y familias disponibles --%>
            <div class="pf-columna">
                <h3 runat="server" data-traduccion="pageRolesFamilias_titPermisosFamilias">Permisos y Familias</h3>
                <asp:TreeView ID="twPermisosFamilias" runat="server" CssClass="pf-tree" ShowLines="true">
                    <NodeStyle ForeColor="#333333" />
                    <SelectedNodeStyle
                        BackColor="DarkOliveGreen"
                        ForeColor="White"
                        Font-Bold="true" />
                </asp:TreeView>
            </div>

            <%-- COLUMNA 2: roles existentes con sus accesos --%>
            <div class="pf-columna">
                <h3 runat="server" data-traduccion="pageRolesFamilias_titRoles">Roles</h3>
                <asp:TreeView ID="twRoles" runat="server" CssClass="pf-tree" ShowLines="true"> 
                    <NodeStyle ForeColor="#333333" />
                    <SelectedNodeStyle
                        BackColor="DarkOliveGreen"
                        ForeColor="White"
                        Font-Bold="true" />
                </asp:TreeView>
            </div>

            <%-- COLUMNA 3: accesos del rol/familia elegido, o para uno nuevo --%>
            <div class="pf-columna">
                <h3 runat="server" data-traduccion="pageRolesFamilias_titPermisosSeleccionados">Permisos Seleccionados</h3>
                <asp:TreeView ID="twPermisosSeleccionados" runat="server" CssClass="pf-tree" ShowLines="true">
                    <NodeStyle ForeColor="#333333" />
                    <SelectedNodeStyle
                        BackColor="DarkOliveGreen"
                        ForeColor="White"
                        Font-Bold="true" />
                </asp:TreeView>
            </div>

            <div class="pf-acciones">

                <label runat="server" data-traduccion="pageRolesFamilias_lblTrabajarSobre">Trabajar sobre</label>
                <asp:RadioButtonList ID="rblTipo" runat="server" RepeatDirection="Horizontal"
                    AutoPostBack="true" OnSelectedIndexChanged="rblTipo_SelectedIndexChanged">
                    <asp:ListItem Text="Roles" Value="R" Selected="True" data-permiso="ROLES_RB" data-traduccion="pageRolesFamilias_optRoles"/>
                    <asp:ListItem Text="Familia" Value="F" data-permiso="FAMILIAS_RB" data-traduccion="pageRolesFamilias_optFamilia"/>
                </asp:RadioButtonList>

                <label runat="server" data-traduccion="pageRolesFamilias_lblNombre">Nombre</label>
                <asp:TextBox ID="txtNombre" runat="server" CssClass="input" />

                <asp:Label ID="lblLista" runat="server" Text="Roles" AssociatedControlID="ddlRolesFamilias" />
                <asp:DropDownList ID="ddlRolesFamilias" runat="server" CssClass="input"
                    AutoPostBack="true" OnSelectedIndexChanged="ddlRolesFamilias_SelectedIndexChanged" />

                <asp:Button ID="btnCrear" runat="server" Text="Crear" data-traduccion="pageRolesFamilias_btnCrear" CssClass="btn" OnClick="btnCrear_Click" data-permiso="ROL_FAMILIA_ALTA"/>
                <asp:Button ID="btnModificar" runat="server" Text="Modificar" data-traduccion="pageRolesFamilias_btnModificar" CssClass="btn" OnClick="btnModificar_Click" data-permiso="ROL_FAMILIA_MODIFICAR"/>
                <asp:Button ID="btnEliminar" runat="server" Text="Eliminar" data-traduccion="pageRolesFamilias_btnEliminar" CssClass="btn" OnClick="btnEliminar_Click" data-permiso="ROL_FAMILIA_BAJA"/>

                <asp:Panel ID="pnlAlerta" runat="server" Visible="false" CssClass="alert">
                    <asp:Label ID="lblMensaje" runat="server" Text="" />
                </asp:Panel>
                <asp:Panel ID="pnlExito" runat="server" Visible="false" CssClass="exito">
                    <asp:Label ID="lblMensajeExito" runat="server" Text="" />
                </asp:Panel>

                <hr />

                <asp:Button ID="btnAsignar" runat="server" Text="Asignar" data-traduccion="pageRolesFamilias_btnAsignar" CssClass="btn" OnClick="btnAsignar_Click" data-permiso="ROL_FAMILIA_ASIGNAR_PERMISO"/>
                <asp:Button ID="btnDesasignar" runat="server" Text="Desasignar" data-traduccion="pageRolesFamilias_btnDesasignar" CssClass="btn" OnClick="btnDesasignar_Click" data-permiso="ROL_FAMILIA_DESASIGNAR_PERMISO"/>

                <hr />

                <asp:Button ID="btnLimpiar" runat="server" Text="Limpiar" data-traduccion="pageRolesFamilias_btnLimpiar" CssClass="btn secundario"
                    OnClick="btnLimpiar_Click" CausesValidation="false"/>
                <asp:Button ID="btnSalir" runat="server" Text="Salir" data-traduccion="pageRolesFamilias_btnSalir" CssClass="btn secundario"
                    PostBackUrl="~/MenuPrincipal.aspx" CausesValidation="false"/>
            </div>

        </div>

    </div>
    <script src="<%= ResolveUrl("~/Scripts/ScriptRolesFamilias.js") %>"></script>
</asp:Content>