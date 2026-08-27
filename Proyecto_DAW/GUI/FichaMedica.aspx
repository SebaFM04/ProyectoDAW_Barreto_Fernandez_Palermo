<%@ Page Title="Ficha Médica" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="FichaMedica.aspx.cs" Inherits="FichaMedica" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="head" runat="server">
    <link href="Estilos/EstilosFichaMedica.css" rel="stylesheet" />
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<div class="page-container">
    <h2 class="titulo">Ficha Médica</h2>

    <asp:Label ID="lbInfoAnimal" runat="server" CssClass="info-animal" />

    <div class="form-box">
        <label>
            <asp:CheckBox ID="chkCastrado" runat="server" OnClientClick="return confirmarCastrado(this);" /> Castrado
        </label>

        <label>Dieta:</label>
        <asp:TextBox ID="txtDieta" runat="server" CssClass="input" />

        <label>Medicamento:</label>
        <asp:TextBox ID="txtMedicamento" runat="server" CssClass="input" />

        <label>Observaciones:</label>
        <asp:TextBox ID="txtObservaciones" runat="server" CssClass="input" TextMode="MultiLine" Rows="3" />

        <asp:Button ID="btnRegistrar" runat="server" Text="Registrar ficha médica" CssClass="btn" OnClick="btnRegistrar_Click" />
        <asp:Button ID="btnVolver" runat="server" Text="Volver al listado" CssClass="btn secundario" OnClick="btnVolver_Click" />

        <asp:Panel ID="pnlAlerta" runat="server" Visible="false" CssClass="alert">
            <asp:Label ID="lbMensaje" runat="server" Text=""></asp:Label>
        </asp:Panel>
    </div>

    <h3 class="titulo">Historial médico</h3>
    <asp:GridView ID="gvHistorial" runat="server" CssClass="grid" AutoGenerateColumns="false" EmptyDataText="Sin fichas médicas registradas todavía">
        <Columns>
            <asp:BoundField DataField="fecha" HeaderText="Fecha" DataFormatString="{0:dd/MM/yyyy HH:mm}" />
            <asp:BoundField DataField="castrado" HeaderText="Castrado" />
            <asp:BoundField DataField="dieta" HeaderText="Dieta" />
            <asp:BoundField DataField="medicamento" HeaderText="Medicamento" />
            <asp:BoundField DataField="observaciones" HeaderText="Observaciones" />
        </Columns>
    </asp:GridView>

</div>

<script>
    function confirmarCastrado(checkbox) {
        if (checkbox.checked) {
            return confirm("¿Confirma que el animal fue castrado? Este dato no se podrá revertir.");
        }
        return true;
    }
</script>

</asp:Content>