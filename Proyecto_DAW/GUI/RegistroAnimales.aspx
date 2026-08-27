<%@ Page Title="Registro de Animales" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="RegistroAnimales.aspx.cs" Inherits="RegistroAnimales" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="head" runat="server">
    <link href="Estilos/EstilosRegistroAnimales.css" rel="stylesheet" />
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<div class="page-container">

    <h2 class="titulo">Registro de Animales</h2>

    <div class="layout">

        <!-- GRID -->
        <div class="grid-container">
            <asp:GridView ID="gvAnimales" runat="server" CssClass="grid" AutoGenerateColumns="false" AutoGenerateSelectButton="true" DataKeyNames="codigoAnimal" OnSelectedIndexChanged="gvAnimales_SelectedIndexChanged" EmptyDataText="No hay Animales para mostrar">
                <Columns>
                    <asp:BoundField DataField="codigoAnimal" HeaderText="Código" />
                    <asp:BoundField DataField="especie" HeaderText="Especie" />
                    <asp:BoundField DataField="raza" HeaderText="Raza" />
                    <asp:BoundField DataField="nombre" HeaderText="Nombre" />
                    <asp:BoundField DataField="tamaño" HeaderText="Tamaño" />
                    <asp:BoundField DataField="sexo" HeaderText="sexo" />
                    <asp:BoundField DataField="estadoAdopcion" HeaderText="Estado Adopcion" />
                    <asp:BoundField DataField="vivo" HeaderText="Vivo" />
                    <asp:TemplateField HeaderText="Ficha Ingreso">
                        <ItemTemplate>
                            <asp:HyperLink runat="server" 
                                NavigateUrl='<%# "FichaIngreso.aspx?codigoAnimal=" + Eval("codigoAnimal") %>' 
                                Text="Ver Ficha" 
                                CssClass="btn-link" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Ficha Médica">
                        <ItemTemplate>
                            <asp:HyperLink runat="server" 
                                NavigateUrl='<%# "FichaMedica.aspx?codigoAnimal=" + Eval("codigoAnimal") %>' 
                                Text="Ver Ficha Médica" 
                                CssClass="btn-link" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Certificados">
                        <ItemTemplate>
                            <asp:HyperLink runat="server" 
                                NavigateUrl='<%# "Certificados.aspx?codigoAnimal=" + Eval("codigoAnimal") %>' 
                                Text="Ver Certificados" 
                                CssClass="btn-link" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>

            <div class="xml-box">
                <asp:Button ID="btnExportar" runat="server" Text="Exportar a XML" CssClass="btn" OnClick="btnExportar_Click" />

                <asp:FileUpload ID="fuImportar" runat="server" CssClass="input" />
                <asp:Button ID="btnImportar" runat="server" Text="Importar desde XML" CssClass="btn" OnClick="btnImportar_Click" />

                <asp:Label ID="lbMensajeXML" runat="server" Text="" />
            </div>
        </div>

        <!-- INPUTS -->
        <div class="form-box">

            <div class="form-inputs">

                <label>Especie</label>
                <asp:TextBox ID="txtEspecie" runat="server" CssClass="input" />

                <label>Raza</label>
                <asp:TextBox ID="txtRaza" runat="server" CssClass="input" />

                <asp:Button ID="btnBuscarInfo" runat="server" Text="Buscar info de raza" CssClass="btn secundario" OnClientClick="buscarInfoRaza(); return false;" />

                <div id="infoRaza" class="info-raza-box" style="display:none;">
                    <img id="imgRaza" src="" alt="Foto de referencia" />
                    <p id="txtInfoRaza"></p>
                </div>

                <label>Nombre</label>
                <asp:TextBox ID="txtNombre" runat="server" CssClass="input" />

                <label>Tamaño</label>
                <asp:DropDownList ID="ddlTamano" runat="server" CssClass="input">
                    <asp:ListItem Text="Pequeño" />
                    <asp:ListItem Text="Mediano" />
                    <asp:ListItem Text="Grande" />
                </asp:DropDownList>

                <label>Sexo</label>
                <asp:DropDownList ID="ddlSexo" runat="server" CssClass="input">
                    <asp:ListItem Text="Macho" />
                    <asp:ListItem Text="Hembra" />
                </asp:DropDownList>

                <label>Estado</label>
                <asp:DropDownList ID="ddlEstado" runat="server" CssClass="input">
                    <asp:ListItem Text="En Adopcion"/>
                    <asp:ListItem Text="Adoptado" />
                </asp:DropDownList>
                
                <asp:Panel ID="pnlAlerta" runat="server" Visible="false" CssClass="alert">
                    <asp:Label ID="lbMensaje" runat="server" Text=""></asp:Label>
                </asp:Panel>
            </div>

            <!-- BOTONES -->
            <div class="form-buttons">
                <asp:button ID="btnAlta" runat="server" Text="Alta" CssClass="btn" OnClick="btnAlta_Click"/>
                <asp:button ID="btnModificar" runat="server" Text="Modificar" CssClass="btn" OnClick="btnModificar_Click"/>
                <asp:button ID="btnBaja" runat="server" Text="Baja" CssClass="btn" OnClientClick="return confirmarBaja();" OnClick="btnBaja_Click"/>

                <hr />

                <asp:button ID="btnLimpiar" runat="server" Text="Limpiar" CssClass="btn secundario" OnClientClick="limpiarFormulario(); ocultarAlerta(true); return false;"/>
                <asp:button ID="btnSalir" runat="server" Text="Salir" CssClass="btn salir" OnClientClick="salir(); return false;"/>
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
