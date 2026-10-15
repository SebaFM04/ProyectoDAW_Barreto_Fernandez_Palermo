<%@ Page Title="Registro de Animales" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="RegistroAnimales.aspx.cs" Inherits="RegistroAnimales" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="head" runat="server">
    <link href="Estilos/EstilosRegistroAnimales.css" rel="stylesheet" />
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<div class="page-container">

    <h2 class="titulo" runat="server" data-traduccion="pageAnimal_titRegistroAnimales">Registro de Animales</h2>

    <div class="layout">

        <!-- GRID -->
        <div class="grid-container">
            <asp:GridView ID="gvAnimales" runat="server" CssClass="grid" AutoGenerateColumns="false" AutoGenerateSelectButton="true" DataKeyNames="codigoAnimal" OnSelectedIndexChanged="gvAnimales_SelectedIndexChanged" EmptyDataText="No hay Animales para mostrar" data-traduccion-vacio="pageAnimal_msgGrillaVacia">
                <Columns>
                    <asp:BoundField DataField="codigoAnimal" HeaderText="Código" AccessibleHeaderText="pageAnimal_colCodigo" />
                    <asp:BoundField DataField="especie" HeaderText="Especie" AccessibleHeaderText="pageAnimal_colEspecie" />
                    <asp:BoundField DataField="raza" HeaderText="Raza" AccessibleHeaderText="pageAnimal_colRaza" />
                    <asp:BoundField DataField="nombre" HeaderText="Nombre" AccessibleHeaderText="pageAnimal_colNombre" />
                    <asp:BoundField DataField="tamaño" HeaderText="Tamaño" AccessibleHeaderText="pageAnimal_colTamano" />
                    <asp:BoundField DataField="sexo" HeaderText="sexo" AccessibleHeaderText="pageAnimal_colSexo" />
                    <asp:BoundField DataField="estadoAdopcion" HeaderText="Estado Adopcion" AccessibleHeaderText="pageAnimal_colEstadoAdopcion" />
                    <asp:BoundField DataField="vivo" HeaderText="Vivo" AccessibleHeaderText="pageAnimal_colVivo" />
                    <asp:TemplateField HeaderText="Ficha Ingreso" AccessibleHeaderText="pageAnimal_colFichaIngreso">
                        <ItemTemplate>
                            <asp:HyperLink runat="server" 
                                NavigateUrl='<%# "FichaIngreso.aspx?codigoAnimal=" + Eval("codigoAnimal") %>' 
                                Text="Ver Ficha" data-traduccion="pageAnimal_lnkVerFicha"
                                CssClass="btn-link" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Ficha Médica" AccessibleHeaderText="pageAnimal_colFichaMedica">
                        <ItemTemplate>
                            <asp:HyperLink runat="server" 
                                NavigateUrl='<%# "FichaMedica.aspx?codigoAnimal=" + Eval("codigoAnimal") %>' 
                                Text="Ver Ficha Médica" data-traduccion="pageAnimal_lnkVerFichaMedica"
                                CssClass="btn-link" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Certificados" AccessibleHeaderText="pageAnimal_colCertificados">
                        <ItemTemplate>
                            <asp:HyperLink runat="server" 
                                NavigateUrl='<%# "Certificados.aspx?codigoAnimal=" + Eval("codigoAnimal") %>' 
                                Text="Ver Certificados" data-traduccion="pageAnimal_lnkVerCertificados"
                                CssClass="btn-link" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>

            <div class="xml-box">
                <asp:Button ID="btnExportar" runat="server" Text="Exportar a XML" data-traduccion="pageAnimal_btnExportarXML" CssClass="btn" OnClick="btnExportar_Click" data-permiso="ANIMAL_EXPORTAR_XML"/>

                <asp:FileUpload ID="fuImportar" runat="server" CssClass="input" />
                <asp:Button ID="btnImportar" runat="server" Text="Importar desde XML" data-traduccion="pageAnimal_btnImportarXML" CssClass="btn" OnClick="btnImportar_Click" data-permiso="ANIMAL_IMPORTAR_XML"/>

                <asp:Label ID="lbMensajeXML" runat="server" Text="" />
            </div>
        </div>

        <!-- INPUTS -->
        <div class="form-box">

            <div class="form-inputs">

                <label runat="server" data-traduccion="pageAnimal_lblEspecie">Especie</label>
                <asp:TextBox ID="txtEspecie" runat="server" CssClass="input" />

                <label runat="server" data-traduccion="pageAnimal_lblRaza">Raza</label>
                <asp:TextBox ID="txtRaza" runat="server" CssClass="input" />

                <asp:Button ID="btnBuscarInfo" runat="server" Text="Buscar info de raza" data-traduccion="pageAnimal_btnBuscarInfoRaza" CssClass="btn secundario" OnClientClick="buscarInfoRaza(); return false;" />

                <div id="infoRaza" class="info-raza-box" style="display:none;">
                    <img id="imgRaza" src="" alt="Foto de referencia" />
                    <p id="txtInfoRaza"></p>
                </div>

                <label runat="server" data-traduccion="pageAnimal_lblNombre">Nombre</label>
                <asp:TextBox ID="txtNombre" runat="server" CssClass="input" />

                <label runat="server" data-traduccion="pageAnimal_lblTamano">Tamaño</label>
                <asp:DropDownList ID="ddlTamano" runat="server" CssClass="input">
                    <asp:ListItem Text="Pequeño" data-traduccion="pageAnimal_optPequeno" />
                    <asp:ListItem Text="Mediano" data-traduccion="pageAnimal_optMediano" />
                    <asp:ListItem Text="Grande" data-traduccion="pageAnimal_optGrande" />
                </asp:DropDownList>

                <label runat="server" data-traduccion="pageAnimal_lblSexo">Sexo</label>
                <asp:DropDownList ID="ddlSexo" runat="server" CssClass="input">
                    <asp:ListItem Text="Macho" data-traduccion="pageAnimal_optMacho" />
                    <asp:ListItem Text="Hembra" data-traduccion="pageAnimal_optHembra" />
                </asp:DropDownList>

                <label runat="server" data-traduccion="pageAnimal_lblEstado">Estado</label>
                <asp:DropDownList ID="ddlEstado" runat="server" CssClass="input">
                    <asp:ListItem Text="En Adopcion" data-traduccion="pageAnimal_optEnAdopcion" />
                    <asp:ListItem Text="Adoptado" data-traduccion="pageAnimal_optAdoptado" />
                </asp:DropDownList>
                
                <asp:Panel ID="pnlAlerta" runat="server" Visible="false" CssClass="alert">
                    <asp:Label ID="lbMensaje" runat="server" Text=""></asp:Label>
                </asp:Panel>
            </div>

            <!-- BOTONES -->
            <div class="form-buttons">
                <asp:button ID="btnAlta" runat="server" Text="Alta" data-traduccion="pageAnimal_btnAltaAnimal" CssClass="btn" OnClick="btnAlta_Click" data-permiso="ANIMAL_ALTA"/>
                <asp:button ID="btnModificar" runat="server" Text="Modificar" data-traduccion="pageAnimal_btnModificarAnimal" CssClass="btn" OnClick="btnModificar_Click" data-permiso="ANIMAL_MODIFICAR"/>
                <asp:button ID="btnBaja" runat="server" Text="Baja" data-traduccion="pageAnimal_btnBajaAnimal" CssClass="btn" OnClientClick="return confirmarBaja();" OnClick="btnBaja_Click" data-permiso="ANIMAL_BAJA"/>

                <hr />

                <asp:button ID="btnLimpiar" runat="server" Text="Limpiar" data-traduccion="pageAnimal_btnLimpiar" CssClass="btn secundario" OnClientClick="limpiarFormulario(); ocultarAlerta(true); return false;"/>
                <asp:button ID="btnSalir" runat="server" Text="Salir" data-traduccion="pageAnimal_btnSalir" CssClass="btn salir" OnClientClick="salir(); return false;"/>
            </div>

        </div>

    </div>

</div>

<script>
    var ids = {
        especie: '<%= txtEspecie.ClientID %>',
        raza: '<%= txtRaza.ClientID %>',
        nombre: '<%= txtNombre.ClientID %>',
        tamano: '<%= ddlTamano.ClientID %>',
        sexo: '<%= ddlSexo.ClientID %>',
        estado: '<%= ddlEstado.ClientID %>',
        alerta: '<%= pnlAlerta.ClientID %>'
    };
</script>
<script src="<%= ResolveUrl("~/Scripts/ScriptRegistroAnimales.js") %>"></script>
</asp:Content>
