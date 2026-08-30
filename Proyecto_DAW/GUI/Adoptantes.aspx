<%@ Page Title="Adoptantes" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Adoptantes.aspx.cs" Inherits="Adoptantes" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="head" runat="server">
    <link href="Estilos/EstilosAdopciones.css" rel="stylesheet" />
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<div class="page-container">
    <h2 class="titulo">Gestión de Adoptantes</h2>

    <div class="layout-adopcion">

        <div class="columna-grillas">

            <%-- GRILLA ADOPTANTES --%>
            <asp:GridView ID="gvAdoptantes" runat="server"
                CssClass="grid"
                AutoGenerateColumns="false"
                AutoGenerateSelectButton="true"
                DataKeyNames="dni"
                OnSelectedIndexChanged="gvAdoptantes_SelectedIndexChanged"
                EmptyDataText="No hay adoptantes para mostrar">
                <Columns>
                    <asp:BoundField DataField="dni" HeaderText="DNI" />
                    <asp:BoundField DataField="nombre" HeaderText="Nombre" />
                    <asp:BoundField DataField="apellido" HeaderText="Apellido" />
                    <asp:BoundField DataField="telefono" HeaderText="Teléfono" />
                    <asp:BoundField DataField="edad" HeaderText="Edad" />
                    <asp:BoundField DataField="domicilio" HeaderText="Domicilio" />
                    <asp:BoundField DataField="mascotas" HeaderText="Tiene mascotas" />
                    <asp:BoundField DataField="activo" HeaderText="Activo" />
                    <asp:TemplateField HeaderText="Certificados">
                        <ItemTemplate>
                            <asp:HyperLink runat="server" 
                                NavigateUrl='<%# "Certificados.aspx?dni=" + Eval("dni") %>' 
                                Text="Ver Certificados" 
                                CssClass="btn-link" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>

            <%-- CAMPOS DEL FORMULARIO --%>
            <div class="fila-campos">
                <div class="campo-grupo">
                    <label>DNI:</label>
                    <asp:TextBox ID="txtDni" runat="server" CssClass="input" />
                </div>
                <div class="campo-grupo">
                    <label>Nombre:</label>
                    <asp:TextBox ID="txtNombre" runat="server" CssClass="input" />
                </div>
                <div class="campo-grupo">
                    <label>Apellido:</label>
                    <asp:TextBox ID="txtApellido" runat="server" CssClass="input" />
                </div>
                <div class="campo-grupo">
                    <label>Teléfono:</label>
                    <asp:TextBox ID="txtTelefono" runat="server" CssClass="input" />
                </div>
                <div class="campo-grupo">
                    <label>Edad:</label>
                    <asp:TextBox ID="txtEdad" runat="server" CssClass="input" />
                </div>
                <div class="campo-grupo">
                    <label>Domicilio:</label>
                    <asp:TextBox ID="txtDomicilio" runat="server" CssClass="input" />
                </div>
                <div class="campo-grupo">
                    <label>
                        <asp:CheckBox ID="chkMascotas" runat="server" /> ¿Tiene mascotas?
                    </label>
                </div>
            </div>

            <asp:Panel ID="pnlAlerta" runat="server" Visible="false" CssClass="alert">
                <asp:Label ID="lbMensaje" runat="server" Text=""></asp:Label>
            </asp:Panel>

        </div>

        <%-- BOTONES --%>
        <div class="columna-botones">
            <asp:Button ID="btnAlta" runat="server" Text="Alta" CssClass="btn" OnClick="btnAlta_Click" data-permiso="ADOPTANTE_ALTA"/>
            <asp:Button ID="btnModificar" runat="server" Text="Modificar" CssClass="btn" OnClick="btnModificar_Click" data-permiso="ADOPTANTE_MODIFICAR"/>
            <asp:Button ID="btnActivarDesactivar" runat="server" Text="Activar / Desactivar" CssClass="btn" OnClick="btnActivarDesactivar_Click" data-permiso="ADOPTANTE_ACT_DESAC"/>

            <hr />

            <asp:Button ID="btnLimpiar" runat="server" Text="Limpiar" CssClass="btn btn-secundario" OnClientClick="limpiarFormulario(); ocultarAlerta(true); return false;"/>
            <asp:Button ID="btnSalir" runat="server" Text="Salir" CssClass="btn btn-secundario" OnClientClick="salir(); return false;"/>
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