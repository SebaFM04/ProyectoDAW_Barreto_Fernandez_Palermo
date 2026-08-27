<%@ Page Title="Registrar Adopción" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="RegistrarAdopcion.aspx.cs" Inherits="RegistrarAdopcion" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="head" runat="server">
    <link href="Estilos/EstilosCertificadoAdopcion.css" rel="stylesheet" />
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<div class="page-container">
    <h2 class="titulo">Registrar Adopción</h2>

    <p class="seccion-titulo">Animales disponibles:</p>
    <asp:GridView ID="gvAnimales" runat="server" CssClass="grid" AutoGenerateColumns="false" AutoGenerateSelectButton="true" DataKeyNames="codigoAnimal" OnSelectedIndexChanged="gvAnimales_SelectedIndexChanged" EmptyDataText="No hay animales disponibles para adopción">
        <Columns>
            <asp:BoundField DataField="codigoAnimal" HeaderText="Código" />
            <asp:BoundField DataField="especie" HeaderText="Especie" />
            <asp:BoundField DataField="raza" HeaderText="Raza" />
            <asp:BoundField DataField="nombre" HeaderText="Nombre" />
        </Columns>
    </asp:GridView>

    <p class="seccion-titulo">Adoptantes activos:</p>
    <asp:GridView ID="gvAdoptantes" runat="server" CssClass="grid" AutoGenerateColumns="false" AutoGenerateSelectButton="true" DataKeyNames="dni" OnSelectedIndexChanged="gvAdoptantes_SelectedIndexChanged" EmptyDataText="No hay adoptantes activos">
        <Columns>
            <asp:BoundField DataField="dni" HeaderText="DNI" />
            <asp:BoundField DataField="nombre" HeaderText="Nombre" />
            <asp:BoundField DataField="apellido" HeaderText="Apellido" />
        </Columns>
    </asp:GridView>

    <div class="form-box">
        <asp:Button ID="btnRegistrar" runat="server" Text="Registrar Adopción" CssClass="btn" OnClick="btnRegistrar_Click" />
        <asp:Button ID="btnVolver" runat="server" Text="Volver" CssClass="btn secundario" OnClick="btnVolver_Click" />

        <asp:Panel ID="pnlAlerta" runat="server" Visible="false" CssClass="alert">
            <asp:Label ID="lbMensaje" runat="server" Text=""></asp:Label>
        </asp:Panel>
    </div>
</div>

</asp:Content>