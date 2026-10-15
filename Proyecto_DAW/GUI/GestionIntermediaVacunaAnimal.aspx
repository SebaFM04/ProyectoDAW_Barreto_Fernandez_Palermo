<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="GestionIntermediaVacunaAnimal.aspx.cs" Inherits="GestionIntermediaVacunaAnimal" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <link href="Estilos/EstilosVacunas.css" rel="stylesheet" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="vacuna-wrapper">

    <%-- GRILLA VACUNAS --%>
    <p class="seccion-titulo" runat="server" data-traduccion="pageVacunaAnimal_titVacunasRegistradas">Vacunas registradas</p>
    <asp:GridView ID="gvVacunas" runat="server" DataKeyNames="codigoVacuna"
        CssClass="table"
        AutoGenerateColumns="False"
        AutoGenerateSelectButton="True"
        OnSelectedIndexChanged="gvVacunas_SelectedIndexChanged"
        EmptyDataText="No hay datos para mostrar" data-traduccion-vacio="pageVacunaAnimal_msgGrillaVacia">
        <SelectedRowStyle BackColor="#d4e0b5" ForeColor="#2d5a1b" Font-Bold="true" />
        <Columns>
            <asp:BoundField DataField="codigoVacuna" HeaderText="Código vacuna" AccessibleHeaderText="pageVacunaAnimal_colCodigoVacuna" />
            <asp:BoundField DataField="nombreVacuna" HeaderText="Nombre de vacuna" AccessibleHeaderText="pageVacunaAnimal_colNombreVacuna" />
        </Columns>
    </asp:GridView>

    <asp:Button ID="btnGestionarVacunas" runat="server" Text="Gestionar vacunas" data-traduccion="pageVacunaAnimal_btnGestionarVacunas" CssClass="btn-verde" OnClick="btnGestionarVacunas_Click" data-permiso="GESTIONAR_VACUNAS"/>


    <hr />

    <%-- GRILLA ANIMALES --%>
    <p class="seccion-titulo" runat="server" data-traduccion="pageVacunaAnimal_titSeleccionarAnimal">Seleccionar animal</p>
    <asp:GridView ID="gvAnimales" runat="server" DataKeyNames="codigoAnimal"
        CssClass="table"
        AutoGenerateColumns="False"
        AutoGenerateSelectButton="True"
        OnSelectedIndexChanged="gvAnimales_SelectedIndexChanged"
        EmptyDataText="No hay datos para mostrar" data-traduccion-vacio="pageVacunaAnimal_msgGrillaVacia">
        <SelectedRowStyle BackColor="#d4e0b5" ForeColor="#2d5a1b" Font-Bold="true" />
        <Columns>
            <asp:BoundField DataField="codigoAnimal" HeaderText="Código animal" AccessibleHeaderText="pageVacunaAnimal_colCodigoAnimal" />
            <asp:BoundField DataField="especie" HeaderText="Especie" AccessibleHeaderText="pageVacunaAnimal_colEspecie" />
            <asp:BoundField DataField="raza" HeaderText="Raza" AccessibleHeaderText="pageVacunaAnimal_colRaza" />
            <asp:BoundField DataField="nombre" HeaderText="Nombre" AccessibleHeaderText="pageVacunaAnimal_colNombre" />
            <asp:BoundField DataField="tamaño" HeaderText="Tamaño" AccessibleHeaderText="pageVacunaAnimal_colTamano" />
            <asp:BoundField DataField="estadoAdopcion" HeaderText="Estado de adopción" AccessibleHeaderText="pageVacunaAnimal_colEstadoAdopcion" />
            <asp:BoundField DataField="vivo" HeaderText="Vivo" AccessibleHeaderText="pageVacunaAnimal_colVivo" />
        </Columns>
    </asp:GridView>

    <hr />

    <%-- GRILLA INTERMEDIA --%>
     <p class="seccion-titulo" runat="server" data-traduccion="pageVacunaAnimal_titVacunasAsignadas">Vacunas asignadas</p>
    <asp:GridView ID="gvIntermedia" runat="server" 
        CssClass="table"
        AutoGenerateColumns="False"
        AutoGenerateSelectButton="True"
        DataKeyNames="codigo"
        OnSelectedIndexChanged="gvIntermedia_SelectedIndexChanged"
        EmptyDataText="No hay datos para mostrar" data-traduccion-vacio="pageVacunaAnimal_msgGrillaVacia">
        <SelectedRowStyle BackColor="#d4e0b5" ForeColor="#2d5a1b" Font-Bold="true" />
        <Columns>
            <asp:BoundField DataField="codigo" HeaderText="Código" AccessibleHeaderText="pageVacunaAnimal_colCodigo" />
            <asp:BoundField DataField="codigoVacuna" HeaderText="Código vacuna" AccessibleHeaderText="pageVacunaAnimal_colCodigoVacuna" />
            <asp:BoundField DataField="codigoAnimal" HeaderText="Código animal" AccessibleHeaderText="pageVacunaAnimal_colCodigoAnimal" />
            <asp:BoundField DataField="nombreVacuna" HeaderText="Nombre de vacuna" AccessibleHeaderText="pageVacunaAnimal_colNombreVacuna" />
            <asp:BoundField DataField="fechaAplicacion" HeaderText="Fecha de aplicación" AccessibleHeaderText="pageVacunaAnimal_colFechaAplicacion" DataFormatString="{0:dd/MM/yyyy}" />
            <asp:BoundField DataField="fechaProximaAplicacion" HeaderText="Próxima aplicación" AccessibleHeaderText="pageVacunaAnimal_colProximaAplicacion" DataFormatString="{0:dd/MM/yyyy}" />
        </Columns>
    </asp:GridView>

    <hr />

    <%-- FORMULARIO --%>
    <p class="seccion-titulo" runat="server" data-traduccion="pageVacunaAnimal_titDatosVacuna">Datos de la vacuna</p>
    <div style="padding: 0 10px;">
        <div class="fila-form">
            <div class="campo-grupo">
            </div>
            <div class="campo-grupo">
            </div>
        </div>
        <div class="fila-form">
            <div class="campo-grupo">
                <asp:Label runat="server" Text="Fecha aplicación:" data-traduccion="pageVacunaAnimal_lblFechaAplicacion" />
                <asp:TextBox ID="txtFechaAplicacion" runat="server" CssClass="ctrl" TextMode="Date" Width="175px" />
            </div>
            <div class="campo-grupo">
                <asp:Label runat="server" Text="Próxima aplicación:" data-traduccion="pageVacunaAnimal_lblProximaAplicacion" />
                <asp:TextBox ID="txtFechaAplicacionProxima" runat="server" CssClass="ctrl" TextMode="Date" Width="175px" />
            </div>
        </div>
    </div>

    <%-- BOTONES --%>
    <div class="botones-fila">
        <asp:Button ID="btnAlta" runat="server" Text="Alta" data-traduccion="pageVacunaAnimal_btnAlta" CssClass="btn-verde" OnClick="btnAlta_Click" data-permiso="ANIMAL_VACUNA_ALTA"/>
        <asp:Button ID="btnModificar" runat="server" Text="Modificar" data-traduccion="pageVacunaAnimal_btnModificar" CssClass="btn-verde" OnClick="btnModificar_Click" data-permiso="ANIMAL_VACUNA_MODIFICAR"/>
    </div>

    <%-- MENSAJE --%>
    <asp:Panel ID="pnlAlerta" runat="server" Visible="false" CssClass="alert">
        <asp:Label ID="lbMensaje" runat="server" Text=""></asp:Label>
    </asp:Panel>

</div>
</asp:Content>