<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.master" CodeFile="RolesFamilias.aspx.cs" Inherits="RolesFamilias" %>
<%@ MasterType VirtualPath="~/MasterPage.master" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="head" runat="server">
    <link href="Estilos/EstilosRegistroAnimales.css" rel="stylesheet" />
    <link href="Estilos/RolesFamilias.css" rel="stylesheet" />
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="page-container">

        <h2 class="titulo">Roles y Familias</h2>

        <div class="pf-layout">

            <%-- COLUMNA 1: permisos simples y familias disponibles --%>
            <div class="pf-columna">
                <h3>Permisos y Familias</h3>
                <asp:TreeView ID="twPermisosFamilias" runat="server" CssClass="pf-tree" ShowLines="true" />
            </div>

            <%-- COLUMNA 2: roles existentes con sus accesos --%>
            <div class="pf-columna">
                <h3>Roles</h3>
                <asp:TreeView ID="twRoles" runat="server" CssClass="pf-tree" ShowLines="true" />
            </div>

            <%-- COLUMNA 3: accesos del rol/familia elegido, o para uno nuevo --%>
            <div class="pf-columna">
                <h3>Permisos Seleccionados</h3>
                <asp:TreeView ID="twPermisosSeleccionados" runat="server" CssClass="pf-tree" ShowLines="true" />
            </div>

            <div class="pf-acciones">

                <label>Trabajar sobre</label>
                <asp:RadioButtonList ID="rblTipo" runat="server" RepeatDirection="Horizontal"
                    AutoPostBack="true" OnSelectedIndexChanged="rblTipo_SelectedIndexChanged">
                    <asp:ListItem Text="Roles" Value="R" Selected="True" data-permiso="ROLES_RB"/>
                    <asp:ListItem Text="Familia" Value="F" data-permiso="FAMILIAS_RB"/>
                </asp:RadioButtonList>

                <label>Nombre</label>
                <asp:TextBox ID="txtNombre" runat="server" CssClass="input" />

                <asp:Label ID="lblLista" runat="server" Text="Roles" AssociatedControlID="ddlRolesFamilias" />
                <asp:DropDownList ID="ddlRolesFamilias" runat="server" CssClass="input"
                    AutoPostBack="true" OnSelectedIndexChanged="ddlRolesFamilias_SelectedIndexChanged" />

                <asp:Button ID="btnCrear" runat="server" Text="Crear" CssClass="btn" OnClick="btnCrear_Click" data-permiso="ROL_FAMILIA_ALTA"/>
                <asp:Button ID="btnModificar" runat="server" Text="Modificar" CssClass="btn" OnClick="btnModificar_Click" data-permiso="ROL_FAMILIA_MODIFICAR"/>
                <asp:Button ID="btnEliminar" runat="server" Text="Eliminar" CssClass="btn" OnClick="btnEliminar_Click" data-permiso="ROL_FAMILIA_BAJA"/>

                <hr />

                <asp:Button ID="btnAsignar" runat="server" Text="Asignar" CssClass="btn" OnClick="btnAsignar_Click" data-permiso="ROL_FAMILIA_ASIGNAR_PERMISO"/>
                <asp:Button ID="btnDesasignar" runat="server" Text="Desasignar" CssClass="btn" OnClick="btnDesasignar_Click" data-permiso="ROL_FAMILIA_DESASIGNAR_PERMISO"/>

                <hr />

                <asp:Button ID="btnLimpiar" runat="server" Text="Limpiar" CssClass="btn secundario"
                    OnClick="btnLimpiar_Click" CausesValidation="false" data-permiso="LIMPIAR"/>
                <asp:Button ID="btnSalir" runat="server" Text="Salir" CssClass="btn secundario"
                    PostBackUrl="~/MenuPrincipal.aspx" CausesValidation="false" data-permiso="SALIR"/>

                <asp:Panel ID="pnlAlerta" runat="server" Visible="false" CssClass="alert">
                    <asp:Label ID="lbMensaje" runat="server" Text="" />
                </asp:Panel>

            </div>

        </div>

    </div>

</asp:Content>