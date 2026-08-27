<%@ Page Title="Certificados de Adopción" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Certificados.aspx.cs" Inherits="Certificados" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="head" runat="server">
    <link href="Estilos/EstilosCertificadoAdopcion.css" rel="stylesheet" />
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<div class="page-container">
    <h2 class="titulo">Certificados de Adopci&oacute;n</h2>
        
    <asp:Label ID="lbFiltroActivo" runat="server" CssClass="info-animal" />

    <asp:GridView ID="gvCertificados" runat="server" CssClass="grid" AutoGenerateColumns="false" EmptyDataText="No hay certificados para mostrar">
        <Columns>
            <asp:BoundField DataField="codigo" HeaderText="C&oacute;digo" HtmlEncode="false" />
            <asp:BoundField DataField="fecha" HeaderText="Fecha" DataFormatString="{0:dd/MM/yyyy}" />
            <asp:BoundField DataField="nombreAnimal" HeaderText="Animal" />
            <asp:BoundField DataField="especie" HeaderText="Especie" />
            <asp:BoundField DataField="raza" HeaderText="Raza" />
            <asp:BoundField DataField="nombreAdoptante" HeaderText="Adoptante" />
            <asp:BoundField DataField="apellidoAdoptante" HeaderText="Apellido" />
            <asp:BoundField DataField="dni" HeaderText="DNI" />
        </Columns>
    </asp:GridView>

    <asp:Button ID="btnVerTodos" runat="server" Text="Ver todos los certificados" CssClass="btn secundario" OnClick="btnVerTodos_Click" Visible="false" />
</div>

</asp:Content>