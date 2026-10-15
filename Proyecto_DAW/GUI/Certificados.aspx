<%@ Page Title="Certificados de Adopción" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Certificados.aspx.cs" Inherits="Certificados" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="head" runat="server">
    <link href="Estilos/EstilosCertificadoAdopcion.css" rel="stylesheet" />
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<div class="page-container">
    <h2 class="titulo" runat="server" data-traduccion="pageCertificados_titCertificados">Certificados de Adopci&oacute;n</h2>

    <asp:Label ID="lbFiltroActivo" runat="server" CssClass="info-animal" />

    <asp:GridView ID="gvCertificados" runat="server" CssClass="grid" AutoGenerateColumns="false" 
        OnRowCommand="gvCertificados_RowCommand" EmptyDataText="No hay certificados para mostrar" data-traduccion-vacio="pageCertificados_msgGrillaVacia">
        <Columns>
            <asp:BoundField DataField="codigo" HeaderText="C&oacute;digo" AccessibleHeaderText="pageCertificados_colCodigo" HtmlEncode="false" />
            <asp:BoundField DataField="fecha" HeaderText="Fecha" AccessibleHeaderText="pageCertificados_colFecha" DataFormatString="{0:dd/MM/yyyy}" />
            <asp:BoundField DataField="nombreAnimal" HeaderText="Animal" AccessibleHeaderText="pageCertificados_colAnimal" />
            <asp:BoundField DataField="especie" HeaderText="Especie" AccessibleHeaderText="pageCertificados_colEspecie" />
            <asp:BoundField DataField="raza" HeaderText="Raza" AccessibleHeaderText="pageCertificados_colRaza" />
            <asp:BoundField DataField="nombreAdoptante" HeaderText="Adoptante" AccessibleHeaderText="pageCertificados_colAdoptante" />
            <asp:BoundField DataField="apellidoAdoptante" HeaderText="Apellido" AccessibleHeaderText="pageCertificados_colApellido" />
            <asp:BoundField DataField="dni" HeaderText="DNI" AccessibleHeaderText="pageCertificados_colDni" />
            <asp:TemplateField HeaderText="Estado" AccessibleHeaderText="pageCertificados_colEstado">
                <ItemTemplate>
                    <%# (bool)Eval("activo") ? Traducir("pageCertificados_lblVigente", "Vigente") : Traducir("pageCertificados_lblCancelado", "Cancelado") %>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Acciones" AccessibleHeaderText="pageCertificados_colAcciones">
                <ItemTemplate>
                    <asp:Button ID="btnCancelar" runat="server" Text="Cancelar Adopci&oacute;n" data-traduccion="pageCertificados_btnCancelarAdopcion" CssClass="btn secundario"
                        CommandName="Cancelar" CommandArgument='<%# Eval("codigo") %>'
                        OnClientClick="return confirmarCancelarAdopcion();"
                        Visible='<%# (bool)Eval("activo") %>' />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>

    <asp:Panel ID="pnlAlerta" runat="server" Visible="false" CssClass="alert">
        <asp:Label ID="lbMensaje" runat="server" Text=""></asp:Label>
    </asp:Panel>

    <asp:Button ID="btnVerTodos" runat="server" Text="Ver todos los certificados" data-traduccion="pageCertificados_btnVerTodos" CssClass="btn secundario" OnClick="btnVerTodos_Click" Visible="false" />
</div>

<script src="<%= ResolveUrl("~/Scripts/ScriptCertificados.js") %>"></script>

</asp:Content>