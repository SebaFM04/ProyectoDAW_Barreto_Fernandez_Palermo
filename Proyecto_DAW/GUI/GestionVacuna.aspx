<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="GestionVacuna.aspx.cs" Inherits="GestionVacuna" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <link href="Estilos/EstilosVacunas.css" rel="stylesheet" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<div class="vacuna-wrapper">

    <%-- GRILLA VACUNAS --%>
    <p class="seccion-titulo">Vacunas registradas</p>
    <asp:GridView ID="gvVacunas" runat="server"
        CssClass="table"
        AutoGenerateColumns="False"
        AutoGenerateSelectButton="True"
        DataKeyNames="codigoVacuna"
        OnSelectedIndexChanged="gvVacunas_SelectedIndexChanged"
        EmptyDataText="No hay datos para mostrar">
        <SelectedRowStyle BackColor="#d4e0b5" ForeColor="#2d5a1b" Font-Bold="true" />
        <Columns>
            <asp:BoundField DataField="codigoVacuna" HeaderText="Código vacuna" />
            <asp:BoundField DataField="nombreVacuna" HeaderText="Nombre de vacuna" />
            <asp:BoundField DataField="activo" HeaderText="Activo" />
        </Columns>
    </asp:GridView>

    <hr />

    <%-- FORMULARIO --%>
    <p class="seccion-titulo">Datos de la vacuna</p>
    <div style="padding: 0 10px;">
        <div class="fila-form">
            <div class="campo-grupo">
                <asp:Label runat="server" Text="Código:" />
                <asp:TextBox ID="txtCodigo" runat="server" CssClass="ctrl" Width="150px" />
            </div>
            <div class="campo-grupo">
                <asp:Label runat="server" Text="Nombre:" />
                <asp:TextBox ID="txtNombre" runat="server" CssClass="ctrl" Width="250px" />
            </div>
            <div class="campo-grupo">
                <asp:Label runat="server" Text="Estado:" />
                <asp:RadioButton ID="rbActivo" runat="server" Text="Activo" GroupName="Estado" Checked="true" />
                <asp:RadioButton ID="rbInactivo" runat="server" Text="Inactivo" GroupName="Estado" />
            </div>
        </div>
    </div>

    <%-- BOTONES --%>
    <div class="botones-fila">
        <asp:Button ID="btnAlta" runat="server" Text="Alta" CssClass="btn-verde" OnClick="btnAlta_Click" />
        <asp:Button ID="btnModificar" runat="server" Text="Modificar" CssClass="btn-verde" OnClick="btnModificar_Click" />
        <asp:Button ID="btnAplicar" runat="server" Text="Aplicar" CssClass="btn-verde" OnClick="btnAplicar_Click" Visible="false" />
        <asp:Button ID="btnSalir" runat="server" OnClick="btnSalir_Click" Text="Volver" CssClass="btn-verde"  />
    </div>

    <%-- MENSAJE --%>
    <asp:Panel ID="pnlAlerta" runat="server" Visible="false" CssClass="alert">
        <asp:Label ID="lbMensaje" runat="server" Text=""></asp:Label>
    </asp:Panel>

</div>
</asp:Content>