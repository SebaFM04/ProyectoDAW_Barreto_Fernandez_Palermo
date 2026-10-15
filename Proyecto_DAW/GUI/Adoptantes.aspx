<%@ Page Title="Adoptantes" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Adoptantes.aspx.cs" Inherits="Adoptantes" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="head" runat="server">
    <link href="Estilos/EstilosAdopciones.css" rel="stylesheet" />
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<div class="page-container">
    <h2 class="titulo" runat="server" data-traduccion="pageAdoptantes_titGestionAdoptantes">Gestión de Adoptantes</h2>

    <div class="layout-adopcion">

        <div class="columna-grillas">

            <%-- GRILLA ADOPTANTES --%>
            <asp:GridView ID="gvAdoptantes" runat="server"
                CssClass="grid"
                AutoGenerateColumns="false"
                AutoGenerateSelectButton="true"
                DataKeyNames="dni"
                OnSelectedIndexChanged="gvAdoptantes_SelectedIndexChanged"
                EmptyDataText="No hay adoptantes para mostrar" data-traduccion-vacio="pageAdoptantes_msgGrillaVacia">
                <Columns>
                    <asp:BoundField DataField="dni" HeaderText="DNI" AccessibleHeaderText="pageAdoptantes_colDni" />
                    <asp:BoundField DataField="nombre" HeaderText="Nombre" AccessibleHeaderText="pageAdoptantes_colNombre" />
                    <asp:BoundField DataField="apellido" HeaderText="Apellido" AccessibleHeaderText="pageAdoptantes_colApellido" />
                    <asp:BoundField DataField="telefono" HeaderText="Teléfono" AccessibleHeaderText="pageAdoptantes_colTelefono" />
                    <asp:BoundField DataField="edad" HeaderText="Edad" AccessibleHeaderText="pageAdoptantes_colEdad" />
                    <asp:BoundField DataField="domicilio" HeaderText="Domicilio" AccessibleHeaderText="pageAdoptantes_colDomicilio" />
                    <asp:BoundField DataField="mascotas" HeaderText="Tiene mascotas" AccessibleHeaderText="pageAdoptantes_colTieneMascotas" />
                    <asp:BoundField DataField="activo" HeaderText="Activo" AccessibleHeaderText="pageAdoptantes_colActivo" />
                    <asp:TemplateField HeaderText="Certificados" AccessibleHeaderText="pageAdoptantes_colCertificados">
                        <ItemTemplate>
                            <asp:HyperLink runat="server" 
                                NavigateUrl='<%# "Certificados.aspx?dni=" + Eval("dni") %>' 
                                Text="Ver Certificados" data-traduccion="pageAdoptantes_lnkVerCertificados"
                                CssClass="btn-link" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>

            <%-- CAMPOS DEL FORMULARIO --%>
            <div class="fila-campos">
                <div class="campo-grupo">
                    <label runat="server" data-traduccion="pageAdoptantes_lblDni">DNI:</label>
                    <asp:TextBox ID="txtDni" runat="server" CssClass="input" />
                </div>
                <div class="campo-grupo">
                    <label runat="server" data-traduccion="pageAdoptantes_lblNombre">Nombre:</label>
                    <asp:TextBox ID="txtNombre" runat="server" CssClass="input" />
                </div>
                <div class="campo-grupo">
                    <label runat="server" data-traduccion="pageAdoptantes_lblApellido">Apellido:</label>
                    <asp:TextBox ID="txtApellido" runat="server" CssClass="input" />
                </div>
                <div class="campo-grupo">
                    <label runat="server" data-traduccion="pageAdoptantes_lblTelefono">Teléfono:</label>
                    <asp:TextBox ID="txtTelefono" runat="server" CssClass="input" />
                </div>
                <div class="campo-grupo">
                    <label runat="server" data-traduccion="pageAdoptantes_lblEdad">Edad:</label>
                    <asp:TextBox ID="txtEdad" runat="server" CssClass="input" />
                </div>
                <div class="campo-grupo">
                    <label runat="server" data-traduccion="pageAdoptantes_lblDomicilio">Domicilio:</label>
                    <asp:TextBox ID="txtDomicilio" runat="server" CssClass="input" />
                </div>
                <div class="campo-grupo">
                    <asp:CheckBox ID="chkMascotas" runat="server" Text=" ¿Tiene mascotas?" data-traduccion="pageAdoptantes_chkTieneMascotas" />
                </div>
            </div>

            <asp:Panel ID="pnlAlerta" runat="server" Visible="false" CssClass="alert">
                <asp:Label ID="lbMensaje" runat="server" Text=""></asp:Label>
            </asp:Panel>

        </div>

        <%-- BOTONES --%>
        <div class="columna-botones">
            <asp:Button ID="btnAlta" runat="server" Text="Alta" data-traduccion="pageAdoptantes_btnAlta" CssClass="btn" OnClick="btnAlta_Click" data-permiso="ADOPTANTE_ALTA"/>
            <asp:Button ID="btnModificar" runat="server" Text="Modificar" data-traduccion="pageAdoptantes_btnModificar" CssClass="btn" OnClick="btnModificar_Click" data-permiso="ADOPTANTE_MODIFICAR"/>
            <asp:Button ID="btnActivarDesactivar" runat="server" Text="Activar / Desactivar" data-traduccion="pageAdoptantes_btnActivarDesactivar" CssClass="btn" OnClick="btnActivarDesactivar_Click" data-permiso="ADOPTANTE_ACT_DESAC"/>

            <hr />

            <asp:Button ID="btnLimpiar" runat="server" Text="Limpiar" data-traduccion="pageAdoptantes_btnLimpiar" CssClass="btn btn-secundario" OnClientClick="limpiarFormulario(); ocultarAlerta(true); return false;"/>
            <asp:Button ID="btnSalir" runat="server" Text="Salir" data-traduccion="pageAdoptantes_btnSalir" CssClass="btn btn-secundario" OnClientClick="salir(); return false;"/>
        </div>

    </div>
</div>

<script>
    var ids = {
        dni: '<%= txtDni.ClientID %>',
        nombre: '<%= txtNombre.ClientID %>',
        apellido: '<%= txtApellido.ClientID %>',
        telefono: '<%= txtTelefono.ClientID %>',
        edad: '<%= txtEdad.ClientID %>',
        domicilio: '<%= txtDomicilio.ClientID %>',
        mascotas: '<%= chkMascotas.ClientID %>',
        alerta: '<%= pnlAlerta.ClientID %>'
    };
</script>
<script src="<%= ResolveUrl("~/Scripts/ScriptAdoptantes.js") %>"></script>

</asp:Content>