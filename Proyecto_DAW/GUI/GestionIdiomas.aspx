<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="GestionIdiomas.aspx.cs" Inherits="GestionIdiomas" %>
<%@ MasterType VirtualPath="~/MasterPage.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <link href="Estilos/EstilosVacunas.css" rel="stylesheet" />
    <link href="Estilos/EstilosIdiomas.css" rel="stylesheet" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="vacuna-wrapper">

    <h2 class="idiomas-titulo" runat="server" data-traduccion="pageIdiomas_titGestionIdiomas">Gestión de Idiomas</h2>

    <%-- ===================== IDIOMAS ===================== --%>
    <p class="seccion-titulo" runat="server" data-traduccion="pageIdiomas_titIdiomas">Idiomas</p>
    <asp:GridView ID="gvIdiomas" runat="server"
        CssClass="table"
        AutoGenerateColumns="False"
        AutoGenerateSelectButton="True"
        DataKeyNames="ID"
        OnSelectedIndexChanged="gvIdiomas_SelectedIndexChanged"
        EmptyDataText="No hay idiomas cargados" data-traduccion-vacio="pageIdiomas_msgSinIdiomas">
        <SelectedRowStyle BackColor="#d4e0b5" ForeColor="#2d5a1b" Font-Bold="true" />
        <Columns>
            <asp:BoundField DataField="Codigo" HeaderText="Código" AccessibleHeaderText="pageIdiomas_colCodigo" />
            <asp:BoundField DataField="Nombre" HeaderText="Nombre" AccessibleHeaderText="pageIdiomas_colNombre" />
            <asp:TemplateField HeaderText="Estado" AccessibleHeaderText="pageIdiomas_colEstado">
                <ItemTemplate>
                    <span class='<%# (bool)Eval("Activo") ? "estado-activo" : "estado-baja" %>'>
                        <%# (bool)Eval("Activo") ? Traducir("pageIdiomas_lblActivo", "Activo") : Traducir("pageIdiomas_lblDadoDeBaja", "Dado de baja") %>
                    </span>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Predeterminado" AccessibleHeaderText="pageIdiomas_colPredeterminado">
                <ItemTemplate>
                    <%# (bool)Eval("EsPredeterminado") ? Traducir("pageIdiomas_lblSi", "Sí") : "" %>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>

    <p class="seccion-titulo espacio-arriba" runat="server" data-traduccion="pageIdiomas_titDatosIdioma">Datos del idioma</p>
    <div style="padding: 0 10px;">
        <div class="fila-form">
            <div class="campo-grupo">
                <asp:Label runat="server" Text="Código:" data-traduccion="pageIdiomas_lblCodigo" />
                <asp:TextBox ID="txtCodigo" runat="server" CssClass="ctrl" Width="120px" MaxLength="10"
                    placeholder="es, en, pt" />
            </div>
            <div class="campo-grupo">
                <asp:Label runat="server" Text="Nombre:" data-traduccion="pageIdiomas_lblNombre" />
                <asp:TextBox ID="txtNombre" runat="server" CssClass="ctrl" Width="250px" MaxLength="50" />
            </div>
        </div>
    </div>

    <div class="botones-fila botones-wrap">
        <asp:Button ID="btnAlta" runat="server" Text="Alta" data-traduccion="pageIdiomas_btnAlta" CssClass="btn-verde" OnClick="btnAlta_Click" data-permiso="IDIOMA_ALTA" />
        <asp:Button ID="btnModificar" runat="server" Text="Modificar" data-traduccion="pageIdiomas_btnModificar" CssClass="btn-verde" OnClick="btnModificar_Click" data-permiso="IDIOMA_MODIFICAR" />
        <asp:Button ID="btnPredeterminado" runat="server" Text="Marcar predeterminado" data-traduccion="pageIdiomas_btnPredeterminado" CssClass="btn-verde" OnClick="btnPredeterminado_Click" data-permiso="IDIOMA_MODIFICAR" />
        <asp:Button ID="btnBaja" runat="server" Text="Dar de baja" data-traduccion="pageIdiomas_btnBaja" CssClass="btn-rojo" OnClick="btnBaja_Click" data-permiso="IDIOMA_BAJA" />
        <asp:Button ID="btnRehabilitar" runat="server" Text="Rehabilitar" data-traduccion="pageIdiomas_btnRehabilitar" CssClass="btn-verde" OnClick="btnRehabilitar_Click" data-permiso="IDIOMA_REHABILITAR" />
        <asp:Button ID="btnLimpiar" runat="server" Text="Limpiar" data-traduccion="pageIdiomas_btnLimpiar" CssClass="btn-gris" OnClick="btnLimpiar_Click" />
    </div>

    <asp:Panel ID="pnlAlertaIdioma" runat="server" Visible="false" CssClass="alert">
        <asp:Label ID="lbMensajeIdioma" runat="server" Text=""></asp:Label>
    </asp:Panel>

    <hr />

    <%-- ===================== TRADUCCIONES ===================== --%>
    <p class="seccion-titulo" runat="server" data-traduccion="pageIdiomas_titTraducciones">Traducciones</p>
    <div class="fila-form">
        <div class="campo-grupo">
            <asp:Label runat="server" Text="Idioma:" data-traduccion="pageIdiomas_lblIdioma" />
            <asp:DropDownList ID="ddlIdiomaTraduccion" runat="server" CssClass="ctrl" Width="200px"
                AutoPostBack="true" OnSelectedIndexChanged="ddlFiltroTraducciones_SelectedIndexChanged" />
        </div>
        <div class="campo-grupo">
            <asp:Label runat="server" Text="Página:" data-traduccion="pageIdiomas_lblPagina" />
            <asp:DropDownList ID="ddlPagina" runat="server" CssClass="ctrl" Width="220px"
                AutoPostBack="true" OnSelectedIndexChanged="ddlFiltroTraducciones_SelectedIndexChanged" />
        </div>
    </div>
    <p class="ayuda" runat="server" data-traduccion="pageIdiomas_lblAyudaPendientes">Los textos entre [corchetes] todavía no fueron traducidos.</p>

    <asp:GridView ID="gvTraducciones" runat="server"
        CssClass="table tabla-traducciones"
        AutoGenerateColumns="False"
        AutoGenerateSelectButton="True"
        DataKeyNames="IdEtiqueta,Clave"
        EmptyDataText="No hay textos para mostrar" data-traduccion-vacio="pageIdiomas_msgSinTraducciones">
        <SelectedRowStyle BackColor="#d4e0b5" ForeColor="#2d5a1b" Font-Bold="true" />
        <Columns>
            <asp:BoundField DataField="Clave" HeaderText="Clave" AccessibleHeaderText="pageIdiomas_colClave" ItemStyle-CssClass="col-clave" />
            <asp:BoundField DataField="TextoPredeterminado" HeaderText="Texto predeterminado" AccessibleHeaderText="pageIdiomas_colTextoPredeterminado" />
            <asp:TemplateField HeaderText="Traducción" AccessibleHeaderText="pageIdiomas_colTraduccion">
                <ItemTemplate>
                    <asp:TextBox ID="txtTraduccion" runat="server" CssClass="ctrl txt-traduccion" MaxLength="1000"
                        Text='<%# Eval("Texto") %>' />
                    <asp:HiddenField ID="hfOriginal" runat="server" Value='<%# Eval("Texto") %>' />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>

    <div class="botones-fila">
        <asp:Button ID="btnGuardarTraducciones" runat="server" Text="Guardar cambios" data-traduccion="pageIdiomas_btnGuardarTraducciones" CssClass="btn-verde" OnClick="btnGuardarTraducciones_Click" data-permiso="TRADUCCION_GESTIONAR" />
        <asp:Button ID="btnMarcarPendiente" runat="server" Text="Marcar como pendiente" data-traduccion="pageIdiomas_btnMarcarPendiente" CssClass="btn-gris" OnClick="btnMarcarPendiente_Click" data-permiso="TRADUCCION_GESTIONAR" />
    </div>

    <asp:Panel ID="pnlAlertaTraduccion" runat="server" Visible="false" CssClass="alert">
        <asp:Label ID="lbMensajeTraduccion" runat="server" Text=""></asp:Label>
    </asp:Panel>

</div>
</asp:Content>
