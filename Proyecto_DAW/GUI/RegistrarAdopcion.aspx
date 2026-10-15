<%@ Page Title="Registrar Adopción" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="RegistrarAdopcion.aspx.cs" Inherits="RegistrarAdopcion" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="head" runat="server">
    <link href="Estilos/EstilosCertificadoAdopcion.css" rel="stylesheet" />
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<div class="page-container">
    <h2 class="titulo" runat="server" data-traduccion="pageAdopcion_titRegistrarAdopcion">Registrar Adopción</h2>

    <p class="seccion-titulo" runat="server" data-traduccion="pageAdopcion_titAnimalesDisponibles">Animales disponibles:</p>
    <asp:GridView ID="gvAnimales" runat="server" CssClass="grid" AutoGenerateColumns="false" AutoGenerateSelectButton="true" DataKeyNames="codigoAnimal" OnSelectedIndexChanged="gvAnimales_SelectedIndexChanged" EmptyDataText="No hay animales disponibles para adopción" data-traduccion-vacio="pageAdopcion_msgSinAnimales">
        <Columns>
            <asp:BoundField DataField="codigoAnimal" HeaderText="Código" AccessibleHeaderText="pageAdopcion_colCodigo" />
            <asp:BoundField DataField="especie" HeaderText="Especie" AccessibleHeaderText="pageAdopcion_colEspecie" />
            <asp:BoundField DataField="raza" HeaderText="Raza" AccessibleHeaderText="pageAdopcion_colRaza" />
            <asp:BoundField DataField="nombre" HeaderText="Nombre" AccessibleHeaderText="pageAdopcion_colNombre" />
        </Columns>
    </asp:GridView>

    <p class="seccion-titulo" runat="server" data-traduccion="pageAdopcion_titAdoptantesActivos">Adoptantes activos:</p>
    <asp:GridView ID="gvAdoptantes" runat="server" CssClass="grid" AutoGenerateColumns="false" AutoGenerateSelectButton="true" DataKeyNames="dni" OnSelectedIndexChanged="gvAdoptantes_SelectedIndexChanged" EmptyDataText="No hay adoptantes activos" data-traduccion-vacio="pageAdopcion_msgSinAdoptantes">
        <Columns>
            <asp:BoundField DataField="dni" HeaderText="DNI" AccessibleHeaderText="pageAdopcion_colDni" />
            <asp:BoundField DataField="nombre" HeaderText="Nombre" AccessibleHeaderText="pageAdopcion_colNombre" />
            <asp:BoundField DataField="apellido" HeaderText="Apellido" AccessibleHeaderText="pageAdopcion_colApellido" />
        </Columns>
    </asp:GridView>

    <div class="form-box">
        <asp:Button ID="btnRegistrar" runat="server" Text="Registrar Adopción" data-traduccion="pageAdopcion_btnRegistrarAdopcion" CssClass="btn" OnClick="btnRegistrar_Click" />
        <asp:Button ID="btnVolver" runat="server" Text="Volver" data-traduccion="pageAdopcion_btnVolver" CssClass="btn secundario" OnClick="btnVolver_Click" />

        <asp:Panel ID="pnlAlerta" runat="server" Visible="false" CssClass="alert">
            <asp:Label ID="lbMensaje" runat="server" Text=""></asp:Label>
        </asp:Panel>
    </div>
</div>

</asp:Content>