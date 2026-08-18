<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ABMIdioma.aspx.cs" Inherits="ABMIdioma" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <link href="Estilos/EstilosVacunas.css" rel="stylesheet" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<div class="vacuna-wrapper">

    <%-- GRILLA IDIOMAS --%>
    <p class="seccion-titulo">Idiomas registrados</p>
    <asp:GridView ID="gvIdiomas" runat="server"
        CssClass="table"
        AutoGenerateColumns="False"
        AutoGenerateSelectButton="True"
        DataKeyNames="codigo"
        OnSelectedIndexChanged="gvIdiomas_SelectedIndexChanged"
        EmptyDataText="No hay datos para mostrar">
        <SelectedRowStyle BackColor="#d4e0b5" ForeColor="#2d5a1b" Font-Bold="true" />
        <Columns>
            <asp:BoundField DataField="codigo" HeaderText="Código" />
            <asp:BoundField DataField="nombre" HeaderText="Nombre" />
            <asp:BoundField DataField="isDisponible" HeaderText="Disponible" />
        </Columns>
    </asp:GridView>

    <hr />

    <%-- FORMULARIO IDIOMA --%>
    <p class="seccion-titulo">Datos del idioma</p>
    <div style="padding: 0 10px;">
        <div class="fila-form">
            <div class="campo-grupo">
                <asp:Label runat="server" Text="Nombre:" />
                <asp:TextBox ID="txtNombre" runat="server" CssClass="ctrl" Width="250px" />
            </div>
        </div>
    </div>

    <%-- BOTONES --%>
    <div class="botones-fila">
        <asp:Button ID="btnAlta" runat="server" Text="Nuevo idioma" CssClass="btn-verde" OnClick="btnAlta_Click" />
        <asp:Button ID="btnModificar" runat="server" Text="Renombrar" CssClass="btn-verde" OnClick="btnModificar_Click" />
        <asp:Button ID="btnAplicar" runat="server" Text="Aplicar" CssClass="btn-verde" OnClick="btnAplicar_Click" Visible="false" />
        <%-- Nunca se borra un idioma: solo se activa/desactiva (soft-delete). --%>
        <asp:Button ID="btnToggleDisponibilidad" runat="server" Text="Activar / Desactivar" CssClass="btn-verde" OnClick="btnToggleDisponibilidad_Click" Enabled="false" />
        <asp:Button ID="btnSalir" runat="server" OnClick="btnSalir_Click" Text="Volver" CssClass="btn-verde" />
    </div>

    <%-- MENSAJE --%>
    <asp:Panel ID="pnlAlerta" runat="server" Visible="false" CssClass="alert">
        <asp:Label ID="lbMensaje" runat="server" Text=""></asp:Label>
    </asp:Panel>

    <hr />

    <%-- TRADUCCIONES DEL IDIOMA SELECCIONADO --%>
    <asp:Panel ID="pnlTraducciones" runat="server" Visible="false">
        <p class="seccion-titulo">Traducciones — <asp:Label ID="lblIdiomaSeleccionado" runat="server" /></p>

        <div class="fila-form">
            <div class="campo-grupo">
                <asp:Label runat="server" Text="Formulario:" />
                <asp:DropDownList ID="ddlFormulario" runat="server" CssClass="ctrl" AutoPostBack="true" OnSelectedIndexChanged="ddlFormulario_SelectedIndexChanged" />
            </div>
        </div>

        <%-- Los controles sin traducción cargada se muestran como "[NombreControl]",
             tal como los resuelve GestorIdioma.Traducir() en tiempo de ejecución. --%>
        <asp:GridView ID="gvTraducciones" runat="server"
            CssClass="table"
            AutoGenerateColumns="False"
            DataKeyNames="codigoControl"
            OnRowCommand="gvTraducciones_RowCommand"
            EmptyDataText="Este formulario todavía no tiene controles registrados para traducir.">
            <Columns>
                <asp:BoundField DataField="nombreControl" HeaderText="Control" />
                <asp:TemplateField HeaderText="Texto traducido">
                    <ItemTemplate>
                        <asp:TextBox ID="txtTexto" runat="server" CssClass="ctrl" Width="350px"
                            Text='<%# Eval("textoTraducido") ?? string.Format("[{0}]", Eval("nombreControl")) %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="">
                    <ItemTemplate>
                        <asp:Button ID="btnGuardarFila" runat="server" Text="Guardar" CssClass="btn-verde"
                            CommandName="GuardarTraduccion"
                            CommandArgument='<%# Eval("codigoControl") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </asp:Panel>

</div>
</asp:Content>
