<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="GestionIntermediaVacunaAnimal.aspx.cs" Inherits="GestionIntermediaVacunaAnimal" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <link href="Estilos/EstilosVacunas.css" rel="stylesheet" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="vacuna-wrapper">

    <%-- GRILLA VACUNAS --%>
    <p class="seccion-titulo">Vacunas registradas</p>
    <asp:GridView ID="gvVacunas" runat="server" DataKeyNames="codigoVacuna"
        CssClass="table"
        AutoGenerateColumns="False"
        AutoGenerateSelectButton="True"
        OnSelectedIndexChanged="gvVacunas_SelectedIndexChanged"
        EmptyDataText="No hay datos para mostrar">
        <SelectedRowStyle BackColor="#d4e0b5" ForeColor="#2d5a1b" Font-Bold="true" />
        <Columns>
            <asp:BoundField DataField="codigoVacuna" HeaderText="Código vacuna" />
            <asp:BoundField DataField="nombreVacuna" HeaderText="Nombre de vacuna" />
        </Columns>
    </asp:GridView>

    <asp:Button ID="btnGestionarVacunas" runat="server" Text="Gestionar vacunas" CssClass="btn-verde" OnClick="btnGestionarVacunas_Click"/>


    <hr />

    <%-- GRILLA ANIMALES --%>
    <p class="seccion-titulo">Seleccionar animal</p>
    <asp:GridView ID="gvAnimales" runat="server" DataKeyNames="codigoAnimal"
        CssClass="table"
        AutoGenerateColumns="False"
        AutoGenerateSelectButton="True"
        OnSelectedIndexChanged="gvAnimales_SelectedIndexChanged"
        EmptyDataText="No hay datos para mostrar">
        <SelectedRowStyle BackColor="#d4e0b5" ForeColor="#2d5a1b" Font-Bold="true" />
        <Columns>
            <asp:BoundField DataField="codigoAnimal" HeaderText="Código animal" />
            <asp:BoundField DataField="especie" HeaderText="Especie" />
            <asp:BoundField DataField="raza" HeaderText="Raza" />
            <asp:BoundField DataField="nombre" HeaderText="Nombre" />
            <asp:BoundField DataField="tamaño" HeaderText="Tamaño" />
            <asp:BoundField DataField="estadoAdopcion" HeaderText="Estado de adopción" />
            <asp:BoundField DataField="vivo" HeaderText="Vivo" />
        </Columns>
    </asp:GridView>

    <hr />

    <%-- GRILLA INTERMEDIA --%>
     <p class="seccion-titulo">Vacunas asignadas</p>
    <asp:GridView ID="gvIntermedia" runat="server" 
        CssClass="table"
        AutoGenerateColumns="False"
        AutoGenerateSelectButton="True"
        DataKeyNames="codigo"
        OnSelectedIndexChanged="gvIntermedia_SelectedIndexChanged"
        EmptyDataText="No hay datos para mostrar">
        <SelectedRowStyle BackColor="#d4e0b5" ForeColor="#2d5a1b" Font-Bold="true" />
        <Columns>
            <asp:BoundField DataField="codigo" HeaderText="Código" />
            <asp:BoundField DataField="codigoVacuna" HeaderText="Código vacuna" />
            <asp:BoundField DataField="codigoAnimal" HeaderText="Código animal" />
            <asp:BoundField DataField="nombreVacuna" HeaderText="Nombre de vacuna" />
            <asp:BoundField DataField="fechaAplicacion" HeaderText="Fecha de aplicación" DataFormatString="{0:dd/MM/yyyy}" />
            <asp:BoundField DataField="fechaProximaAplicacion" HeaderText="Próxima aplicación" DataFormatString="{0:dd/MM/yyyy}" />
        </Columns>
    </asp:GridView>

    <hr />

    <%-- FORMULARIO --%>
    <p class="seccion-titulo">Datos de la vacuna</p>
    <div style="padding: 0 10px;">
        <div class="fila-form">
            <div class="campo-grupo">
            </div>
            <div class="campo-grupo">
            </div>
        </div>
        <div class="fila-form">
            <div class="campo-grupo">
                <asp:Label runat="server" Text="Fecha aplicación:" />
                <asp:TextBox ID="txtFechaAplicacion" runat="server" CssClass="ctrl" TextMode="Date" Width="175px" />
            </div>
            <div class="campo-grupo">
                <asp:Label runat="server" Text="Próxima aplicación:" />
                <asp:TextBox ID="txtFechaAplicacionProxima" runat="server" CssClass="ctrl" TextMode="Date" Width="175px" />
            </div>
        </div>
    </div>

    <%-- BOTONES --%>
    <div class="botones-fila">
        <asp:Button ID="btnAlta" runat="server" Text="Alta" CssClass="btn-verde" OnClick="btnAlta_Click" />
        <asp:Button ID="btnModificar" runat="server" Text="Modificar" CssClass="btn-verde" OnClick="btnModificar_Click"/>
    </div>

    <%-- MENSAJE --%>
    <asp:Panel ID="pnlAlerta" runat="server" Visible="false" CssClass="alert">
        <asp:Label ID="lbMensaje" runat="server" Text=""></asp:Label>
    </asp:Panel>

</div>
</asp:Content>