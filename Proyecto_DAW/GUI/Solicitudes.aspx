<%@ Page Title="Solicitudes" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Solicitudes.aspx.cs" Inherits="Solicitudes" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="head" runat="server">
    <link href="Estilos/EstilosAdopciones.css" rel="stylesheet" />
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="page-container">
    <h2 class="titulo" runat="server" data-traduccion="pageSolicitudes_titEstadoAdopcion">Estado de Adopcion</h2>

    <div class="layout-adopcion">

        <%-- GRILLA + ESTADO --%>
        <div class="columna-grillas">

            <p class="seccion-titulo" runat="server" data-traduccion="pageSolicitudes_titAnimales">Animales:</p>
            <asp:GridView ID="gvAnimales" runat="server"
                CssClass="grid"
                AutoGenerateColumns="false"
                AutoGenerateSelectButton="true"
                DataKeyNames="codigoAnimal"
                OnSelectedIndexChanged="gvAnimales_SelectedIndexChanged"
                EmptyDataText="No hay animales para mostrar" data-traduccion-vacio="pageSolicitudes_msgGrillaVacia">
                <Columns>
                    <asp:BoundField DataField="codigoAnimal"    HeaderText="Codigo" AccessibleHeaderText="pageSolicitudes_colCodigo" />
                    <asp:BoundField DataField="especie"         HeaderText="Especie" AccessibleHeaderText="pageSolicitudes_colEspecie" />
                    <asp:BoundField DataField="raza"            HeaderText="Raza" AccessibleHeaderText="pageSolicitudes_colRaza" />
                    <asp:BoundField DataField="nombre"          HeaderText="Nombre" AccessibleHeaderText="pageSolicitudes_colNombre" />
                    <asp:BoundField DataField="estadoAdopcion"  HeaderText="Estado actual" AccessibleHeaderText="pageSolicitudes_colEstadoActual" />
                </Columns>
            </asp:GridView>

            <%-- ESTADO --%>
            <fieldset class="estado-group">
                <legend runat="server" data-traduccion="pageSolicitudes_lblEstado">Estado</legend>
                <div class="radio-fila">
                    <asp:RadioButton ID="rbEnEvaluacion" runat="server" GroupName="Estado" Text="En evaluacion" data-traduccion="pageSolicitudes_rbEnEvaluacion" />
                </div>
                <div class="radio-fila">
                    <asp:RadioButton ID="rbDisponible" runat="server" GroupName="Estado" Text="Disponible" data-traduccion="pageSolicitudes_rbDisponible" />
                </div>
                <div class="radio-fila">
                    <asp:RadioButton ID="rbAdoptado" runat="server" GroupName="Estado" Text="Adoptado" data-traduccion="pageSolicitudes_rbAdoptado" />
                </div>
            </fieldset>

            <asp:Panel ID="pnlAlerta" runat="server" Visible="false" CssClass="alert">
                <asp:Label ID="lbMensaje" runat="server" Text=""></asp:Label>
            </asp:Panel>

        </div>

        <%-- BOTONES --%>
        <div class="columna-botones">
            <asp:Button ID="btnAceptar" runat="server" Text="Aceptar" data-traduccion="pageSolicitudes_btnAceptar" CssClass="btn" OnClick="btnAceptar_Click" data-permiso="SOLICITUD_ACEPTAR"/>
            <asp:Button ID="btnSalir" runat="server" Text="Salir" data-traduccion="pageSolicitudes_btnSalir" CssClass="btn btn-secundario" OnClick="btnSalir_Click"/>
        </div>

    </div>
</div>

</asp:Content>
