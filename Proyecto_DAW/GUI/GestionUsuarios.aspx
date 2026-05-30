<%@ Page Title="Gestion de Usuarios" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="GestionUsuarios.aspx.cs" Inherits="GestionUsuarios" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="head" runat="server">
    <link href="Estilos/EstilosRegistroAnimales.css" rel="stylesheet" />
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<div class="page-container">

    <h2 class="titulo">Gestion de Usuarios</h2>

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
                    <asp:BoundField DataField="dni"           HeaderText="DNI" />
                    <asp:BoundField DataField="nombreUsuario" HeaderText="Usuario" />
                    <asp:BoundField DataField="nombre"        HeaderText="Nombre" />
                    <asp:BoundField DataField="apellido"      HeaderText="Apellido" />
                    <asp:BoundField DataField="rol"           HeaderText="Rol" />
                    <asp:BoundField DataField="email"         HeaderText="Email" />
                    <asp:BoundField DataField="activo"        HeaderText="Activo" />
                    <asp:BoundField DataField="bloqueo"       HeaderText="Bloqueado" />
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

                <label>Rol</label>
                <asp:DropDownList ID="ddlRol" runat="server" CssClass="input">
                    <asp:ListItem Text="admin"     Value="admin" />
                    <asp:ListItem Text="adoptante" Value="adoptante" />
                    <asp:ListItem Text="empleado"  Value="empleado" />
                    <asp:ListItem Text="webmaster" Value="webmaster" />
                </asp:DropDownList>

                <label>Activo</label>
                <asp:DropDownList ID="ddlActivo" runat="server" CssClass="input">
                    <asp:ListItem Text="Si" Value="true" />
                    <asp:ListItem Text="No" Value="false" />
                </asp:DropDownList>

                <asp:Panel ID="pnlAlerta" runat="server" Visible="false" CssClass="alert">
                    <asp:Label ID="lbMensaje" runat="server" Text=""></asp:Label>
                </asp:Panel>

            </div>

            <%-- BOTONES --%>
            <div class="form-buttons">
                <asp:Button ID="btnAlta"        runat="server" Text="Alta"        CssClass="btn"           OnClick="btnAlta_Click" />
                <asp:Button ID="btnModificar"   runat="server" Text="Modificar"   CssClass="btn"           OnClick="btnModificar_Click" />
                <asp:Button ID="btnDesbloquear" runat="server" Text="Desbloquear" CssClass="btn secundario" OnClick="btnDesbloquear_Click" />

                <hr />

                <asp:Button ID="btnLimpiar"     runat="server" Text="Limpiar"     CssClass="btn secundario" OnClick="btnLimpiar_Click" />
            </div>

        </div>
    </div>

</div>

</asp:Content>
