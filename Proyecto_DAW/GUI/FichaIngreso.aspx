<%@ Page Title="Ficha de Ingreso" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="FichaIngreso.aspx.cs" Inherits="FichaIngreso" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="head" runat="server">
    <link href="Estilos/EstilosFichaIngreso.css" rel="stylesheet" />
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<div class="page-container">
    <h2 class="titulo">Ficha de Ingreso</h2>

    <asp:Label ID="lbInfoAnimal" runat="server" CssClass="info-animal" />

    <div class="form-box">
        <label>Motivo del ingreso:</label>
        <asp:TextBox ID="txtMotivo" runat="server" CssClass="input" TextMode="MultiLine" Rows="3" />

        <asp:Button ID="btnRegistrar" runat="server" Text="Registrar ingreso" CssClass="btn" OnClick="btnRegistrar_Click" />
        <asp:Button ID="btnVolver" runat="server" Text="Volver al listado" CssClass="btn secundario" OnClick="btnVolver_Click" />

        <asp:Panel ID="pnlAlerta" runat="server" Visible="false" CssClass="alert">
            <asp:Label ID="lbMensaje" runat="server" Text=""></asp:Label>
        </asp:Panel>
    </div>

    <h3 class="titulo">Historial de ingresos</h3>
    <asp:GridView ID="gvHistorial" runat="server" CssClass="grid" AutoGenerateColumns="false" EmptyDataText="Sin ingresos registrados todavía">
        <Columns>
            <asp:BoundField DataField="fecha" HeaderText="Fecha" DataFormatString="{0:dd/MM/yyyy HH:mm}" />
            <asp:BoundField DataField="motivo" HeaderText="Motivo" />
        </Columns>
    </asp:GridView>

</div>

</asp:Content>