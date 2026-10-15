<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="GestionVacuna.aspx.cs" Inherits="GestionVacuna" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <link href="Estilos/EstilosVacunas.css" rel="stylesheet" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="vacuna-wrapper">

    <%-- GRILLA VACUNAS --%>
    <p class="seccion-titulo" runat="server" data-traduccion="pageVacuna_titVacunasRegistradas">Vacunas registradas</p>
    <asp:GridView ID="gvVacunas" runat="server"
        CssClass="table"
        AutoGenerateColumns="False"
        AutoGenerateSelectButton="True"
        DataKeyNames="codigoVacuna"
        OnSelectedIndexChanged="gvVacunas_SelectedIndexChanged"
        EmptyDataText="No hay datos para mostrar" data-traduccion-vacio="pageVacuna_msgGrillaVacia">
        <SelectedRowStyle BackColor="#d4e0b5" ForeColor="#2d5a1b" Font-Bold="true" />
        <Columns>
            <asp:BoundField DataField="codigoVacuna" HeaderText="Código vacuna" AccessibleHeaderText="pageVacuna_colCodigoVacuna" />
            <asp:BoundField DataField="nombreVacuna" HeaderText="Nombre de vacuna" AccessibleHeaderText="pageVacuna_colNombreVacuna" />
            <asp:BoundField DataField="activo" HeaderText="Activo" AccessibleHeaderText="pageVacuna_colActivo" />
        </Columns>
    </asp:GridView>

    <hr />

    <%-- FORMULARIO --%>
    <p class="seccion-titulo" runat="server" data-traduccion="pageVacuna_titDatosVacuna">Datos de la vacuna</p>
    <div style="padding: 0 10px;">
        <div class="fila-form">
            <div class="campo-grupo">
                <asp:Label runat="server" Text="Código:" data-traduccion="pageVacuna_lblCodigo" />
                <asp:TextBox ID="txtCodigo" runat="server" CssClass="ctrl" Width="150px" />
            </div>
            <div class="campo-grupo">
                <asp:Label runat="server" Text="Nombre:" data-traduccion="pageVacuna_lblNombre" />
                <asp:TextBox ID="txtNombre" runat="server" CssClass="ctrl" Width="250px" />
            </div>
            <div class="campo-grupo">
                <asp:Label runat="server" Text="Estado:" data-traduccion="pageVacuna_lblEstado" />
                <asp:RadioButton ID="rbActivo" runat="server" Text="Activo" GroupName="Estado" Checked="true" data-traduccion="pageVacuna_rbActivo" />
                <asp:RadioButton ID="rbInactivo" runat="server" Text="Inactivo" GroupName="Estado" data-traduccion="pageVacuna_rbInactivo" />
            </div>
        </div>
    </div>

    <%-- BOTONES --%>
    <div class="botones-fila">
        <asp:Button ID="btnAlta" runat="server" Text="Alta" data-traduccion="pageVacuna_btnAlta" CssClass="btn-verde" OnClick="btnAlta_Click" data-permiso="VACUNA_ALTA"/>
        <asp:Button ID="btnModificar" runat="server" Text="Modificar" data-traduccion="pageVacuna_btnModificar" CssClass="btn-verde" OnClick="btnModificar_Click" data-permiso="VACUNA_MODIFICAR"/>
        <asp:Button ID="btnAplicar" runat="server" Text="Aplicar" data-traduccion="pageVacuna_btnAplicar" CssClass="btn-verde" OnClick="btnAplicar_Click" Visible="false" data-permiso="VACUNA_APLICAR"/>
        <asp:Button ID="btnSalir" runat="server" OnClick="btnSalir_Click" Text="Volver" CssClass="btn-verde" data-traduccion="pageVacuna_btnVolver"/>
    </div>

    <%-- MENSAJE --%>
    <asp:Panel ID="pnlAlerta" runat="server" Visible="false" CssClass="alert">
        <asp:Label ID="lbMensaje" runat="server" Text=""></asp:Label>
    </asp:Panel>

</div>
</asp:Content>