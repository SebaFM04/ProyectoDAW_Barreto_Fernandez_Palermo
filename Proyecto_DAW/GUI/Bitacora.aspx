<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Bitacora.aspx.cs" Inherits="_Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="Estilos/EstilosBitacora.css" rel="stylesheet" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <asp:ScriptManager ID="ScriptManager1" runat="server" />

    <asp:UpdatePanel ID="upBitacora" runat="server">
        <ContentTemplate>

            <div class="bitacora-wrapper">
                <%-- GRILLA --%>
                <asp:GridView ID="gvBitacora" runat="server" AutoGenerateColumns="false"
                    CssClass="table table-bordered"
                    EmptyDataText="No hay datos para mostrar" data-traduccion-vacio="pageBitacora_msgGrillaVacia"
                    AllowPaging="true"
                    PageSize="10"
                    EnableViewState="true"
                    OnPageIndexChanging="gvBitacora_PageIndexChanging">
                    <Columns>
                        <asp:BoundField DataField="codigo" HeaderText="Código" AccessibleHeaderText="pageBitacora_colCodigo" />
                        <asp:BoundField DataField="login" HeaderText="Login" AccessibleHeaderText="pageBitacora_colLogin" />
                        <asp:BoundField DataField="fecha" HeaderText="Fecha" AccessibleHeaderText="pageBitacora_colFecha" DataFormatString="{0:dd/MM/yyyy}" />
                        <asp:TemplateField HeaderText="Hora" AccessibleHeaderText="pageBitacora_colHora">
                            <ItemTemplate>
                                <%# ((TimeSpan)Eval("hora")).ToString(@"hh\:mm\:ss") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="modulo" HeaderText="Modulo" AccessibleHeaderText="pageBitacora_colModulo" />
                        <asp:BoundField DataField="evento" HeaderText="Evento" AccessibleHeaderText="pageBitacora_colEvento" />
                        <asp:BoundField DataField="criticidad" HeaderText="Criticidad" AccessibleHeaderText="pageBitacora_colCriticidad" />
                    </Columns>
                    <PagerStyle CssClass="paginador" HorizontalAlign="Center" />
                </asp:GridView>

                <hr />

                <%-- FILTROS --%>
                <div style="padding: 0 10px;">
                    <%-- Fila 1: Fechas --%>
                    <div class="fila-filtro">
                        <div class="campo-grupo">
                            <asp:Label runat="server" Text="Fecha inicio:" data-traduccion="pageBitacora_lblFechaInicio" />
                            <asp:TextBox ID="txtFechaInicio" runat="server" TextMode="Date" CssClass="ctrl" Width="175px" />
                        </div>
                        <div class="campo-grupo">
                            <asp:Label runat="server" Text="Fecha fin:" data-traduccion="pageBitacora_lblFechaFin" />
                            <asp:TextBox ID="txtFechaFinal" runat="server" TextMode="Date" CssClass="ctrl" Width="175px" />
                        </div>
                    </div>
                    <%-- Fila 2: Modulo, Evento, Criticidad --%>
                    <div class="fila-filtro">
                        <div class="campo-grupo">
                            <asp:Label runat="server" Text="Modulo:" data-traduccion="pageBitacora_lblModulo" />
                            <asp:DropDownList ID="dlModulo" runat="server" CssClass="ctrl" Width="190px"
                                AutoPostBack="true"
                                OnSelectedIndexChanged="dlFiltro_SelectedIndexChanged" />
                        </div>
                        <div class="campo-grupo">
                            <asp:Label runat="server" Text="Evento:" data-traduccion="pageBitacora_lblEvento" />
                            <asp:DropDownList ID="dlEvento" runat="server" CssClass="ctrl" Width="190px"
                                AutoPostBack="true"
                                OnSelectedIndexChanged="dlFiltro_SelectedIndexChanged" />
                        </div>
                        <div class="campo-grupo">
                            <asp:Label runat="server" Text="Criticidad:" data-traduccion="pageBitacora_lblCriticidad" />
                            <asp:DropDownList ID="dlCriticidad" runat="server" CssClass="ctrl" Width="190px"
                                AutoPostBack="true"
                                OnSelectedIndexChanged="dlFiltro_SelectedIndexChanged" />
                        </div>
                    </div>
                </div>

                <%-- BOTONES --%>
                <div class="botones-fila">
                    <asp:Button ID="btnLimpiarFiltros" runat="server" Text="Limpiar" data-traduccion="pageBitacora_btnLimpiar" CssClass="btn-gris" OnClick="btnLimpiarFiltros_Click"/>
                    <asp:Button ID="btnAplicarFiltros" runat="server" Text="Aplicar" data-traduccion="pageBitacora_btnAplicar" CssClass="btn-verde" OnClick="btnAplicarFiltros_Click" data-permiso="BITACORA_APLICAR_FILTROS"/>
                </div>

                <%-- PANEL DE ERRORES --%>
                <asp:Panel ID="pnlAlerta" runat="server" Visible="false" CssClass="login-alert login-alert-error">
                    <asp:Label ID="lblMensajeError" runat="server" Text=""></asp:Label>
                </asp:Panel>
            </div>

        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>
