<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Bitacora.aspx.cs" Inherits="_Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <link href="Estilos/EstilosBitacora.css" rel="stylesheet" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="bitacora-wrapper">
        <%-- GRILLA --%>
        <asp:GridView ID="gvProductos" runat="server" AutoGenerateColumns="false"
            CssClass="table table-bordered"
            EmptyDataText="No hay datos para mostrar">
            <Columns>
                 <asp:BoundField DataField="codigo" HeaderText="Código" />
                 <asp:BoundField DataField="login" HeaderText="Login" />
                 <asp:BoundField DataField="fecha" HeaderText="Fecha" DataFormatString="{0:dd:MM:yyyy}" />
                 <asp:TemplateField HeaderText="Hora">
                    <ItemTemplate>
                        <%# ((TimeSpan)Eval("hora")).ToString(@"hh\:mm\:ss") %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="modulo" HeaderText="Modulo" />
                <asp:BoundField DataField="evento" HeaderText="Evento" />
                <asp:BoundField DataField="criticidad" HeaderText="Criticidad" />
            </Columns>
        </asp:GridView>

        <hr />

        <%-- FILTROS --%>
        <div style="padding: 0 10px;">

            <%-- Fila 1: Fechas --%>
            <div class="fila-filtro">
                <div class="campo-grupo">
                    <asp:Label runat="server" Text="Fecha inicio:" />
                    <asp:TextBox ID="txtFechaInicio" runat="server" TextMode="Date" CssClass="ctrl" Width="175px" />
                </div>
                <div class="campo-grupo">
                    <asp:Label runat="server" Text="Fecha fin:" />
                    <asp:TextBox ID="txtFechaFinal" runat="server" TextMode="Date" CssClass="ctrl" Width="175px" />
                </div>
            </div>

            <%-- Fila 2: Modulo, Evento, Criticidad --%>
            <div class="fila-filtro">
                <div class="campo-grupo">
                    <asp:Label runat="server" Text="Modulo:" />
                    <asp:DropDownList ID="dlModulo" runat="server" CssClass="ctrl" Width="190px" />
                </div>
                <div class="campo-grupo">
                    <asp:Label runat="server" Text="Evento:" />
                    <asp:DropDownList ID="dlEvento" runat="server" CssClass="ctrl" Width="190px" />
                </div>
                <div class="campo-grupo">
                    <asp:Label runat="server" Text="Criticidad:" />
                    <asp:DropDownList ID="dlCriticidad" runat="server" CssClass="ctrl" Width="190px" />
                </div>
            </div>

        </div>

        <%-- BOTONES --%>
        <div class="botones-fila">
            <asp:Button ID="btnLimpiarFiltros" runat="server" Text="Limpiar" CssClass="btn-gris" OnClick="btnLimpiarFiltros_Click" />
            <asp:Button ID="btnAplicarFiltros" runat="server" Text="Aplicar" CssClass="btn-verde" OnClick="btnAplicarFiltros_Click" />
        </div>

        <%-- PANEL DE ERRORES --%>
        <asp:Panel ID="pnlAlerta" runat="server" Visible="false" CssClass="login-alert login-alert-error">
            <asp:Label ID="lblMensajeError" runat="server" Text=""></asp:Label>
        </asp:Panel>
    </div>
</asp:Content>

