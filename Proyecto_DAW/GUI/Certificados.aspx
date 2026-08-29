<%@ Page Title="Certificados" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Certificados.aspx.cs" Inherits="Certificados" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="head" runat="server">
    <link href="Estilos/EstilosAdopciones.css" rel="stylesheet" />
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="page-container">
    <h2 class="titulo">Certificados de Adopcion</h2>

    <div class="layout-adopcion">

        <div class="columna-grillas">

            <%-- GRILLA CERTIFICADOS --%>
            <p class="seccion-titulo">Certificados de adopcion:</p>
            <asp:GridView ID="gvCertificados" runat="server"
                CssClass="grid"
                AutoGenerateColumns="false"
                AutoGenerateSelectButton="true"
                OnSelectedIndexChanged="gvCertificados_SelectedIndexChanged"
                EmptyDataText="No hay certificados para mostrar">
                <Columns>
                    <asp:BoundField HeaderText="Codigo" />
                    <asp:BoundField HeaderText="DNI adoptante" />
                    <asp:BoundField HeaderText="Codigo animal" />
                    <asp:BoundField HeaderText="Especie" />
                    <asp:BoundField HeaderText="Fecha" />
                </Columns>
            </asp:GridView>

            <%-- GRILLA ADOPTANTES --%>
            <p class="seccion-titulo">Adoptantes:</p>
            <asp:GridView ID="gvAdoptantes" runat="server"
                CssClass="grid"
                AutoGenerateColumns="false"
                AutoGenerateSelectButton="true"
                OnSelectedIndexChanged="gvAdoptantes_SelectedIndexChanged"
                EmptyDataText="No hay adoptantes para mostrar">
                <Columns>
                    <asp:BoundField DataField="dni"      HeaderText="DNI" />
                    <asp:BoundField DataField="nombre"   HeaderText="Nombre" />
                    <asp:BoundField DataField="apellido" HeaderText="Apellido" />
                </Columns>
            </asp:GridView>

            <%-- GRILLA ANIMALES --%>
            <p class="seccion-titulo">Animales:</p>
            <asp:GridView ID="gvAnimales" runat="server"
                CssClass="grid"
                AutoGenerateColumns="false"
                AutoGenerateSelectButton="true"
                OnSelectedIndexChanged="gvAnimales_SelectedIndexChanged"
                EmptyDataText="No hay animales para mostrar">
                <Columns>
                    <asp:BoundField DataField="codigoAnimal" HeaderText="Codigo" />
                    <asp:BoundField DataField="especie"      HeaderText="Especie" />
                    <asp:BoundField DataField="raza"         HeaderText="Raza" />
                    <asp:BoundField DataField="nombre"       HeaderText="Nombre" />
                </Columns>
            </asp:GridView>

            <asp:Panel ID="pnlAlerta" runat="server" Visible="false" CssClass="alert">
                <asp:Label ID="lbMensaje" runat="server" Text=""></asp:Label>
            </asp:Panel>

        </div>

        <%-- BOTONES --%>
        <div class="columna-botones">
            <asp:Button ID="btnGenerarCertificado" runat="server" Text="Generar certificado de adopcion" CssClass="btn btn-especial" OnClick="btnGenerarCertificado_Click" data-permiso="CERTIFICADO_GENERAR"/>
            <asp:Button ID="btnModificar" runat="server" Text="Modificar" CssClass="btn" OnClick="btnModificar_Click" data-permiso="CERTIFICADO_MODIFICAR"/>
            <asp:Button ID="btnAplicar" runat="server" Text="Aplicar" CssClass="btn" OnClick="btnAplicar_Click" data-permiso="CERTIFICADO_APLICAR"/>
            <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" CssClass="btn btn-secundario" OnClick="btnCancelar_Click"/>
            <asp:Button ID="btnSalir" runat="server" Text="Salir" CssClass="btn btn-secundario" OnClick="btnSalir_Click"/>

            <hr class="separador" />

            <asp:Button ID="btnReporte" runat="server" Text="Generar reporte inteligente" CssClass="btn btn-especial" OnClick="btnReporte_Click" data-permiso="CERTIFICADO_GENERAR_REPORTE"/>
        </div>

    </div>
</div>

</asp:Content>
