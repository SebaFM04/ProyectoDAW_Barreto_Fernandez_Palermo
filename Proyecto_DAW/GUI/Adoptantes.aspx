<%@ Page Title="Adoptantes" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Adoptantes.aspx.cs" Inherits="Adoptantes" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="head" runat="server">
    <link href="Estilos/EstilosAdopciones.css" rel="stylesheet" />
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<div class="page-container">
    <h2 class="titulo">Evaluacion del Adoptante</h2>

    <div class="layout-adopcion">

        <div class="columna-grillas">

            <%-- GRILLA EVALUACIONES --%>
            <p class="seccion-titulo">Evaluaciones:</p>
            <asp:GridView ID="gvEvaluaciones" runat="server"
                CssClass="grid"
                AutoGenerateColumns="false"
                AutoGenerateSelectButton="true"
                OnSelectedIndexChanged="gvEvaluaciones_SelectedIndexChanged"
                EmptyDataText="No hay evaluaciones para mostrar">
                <Columns>
                    <asp:BoundField HeaderText="Codigo" />
                    <asp:BoundField HeaderText="DNI" />
                    <asp:BoundField HeaderText="Motivo" />
                    <asp:BoundField HeaderText="Condicion economica" />
                    <asp:BoundField HeaderText="Vivienda" />
                </Columns>
            </asp:GridView>

            <%-- CAMPOS DEL FORMULARIO --%>
            <div class="fila-campos">
                <div class="campo-grupo">
                    <label>Motivo:</label>
                    <asp:TextBox ID="txtMotivo" runat="server" CssClass="input" />
                </div>
                <div class="campo-grupo">
                    <label>Condicion economica:</label>
                    <asp:DropDownList ID="ddlCondicionEconomica" runat="server" CssClass="input">
                        <asp:ListItem Text="Alta"  Value="Alta" />
                        <asp:ListItem Text="Media" Value="Media" />
                        <asp:ListItem Text="Baja"  Value="Baja" />
                    </asp:DropDownList>
                </div>
                <div class="campo-grupo">
                    <label>Vivienda:</label>
                    <asp:TextBox ID="txtVivienda" runat="server" CssClass="input" />
                </div>
            </div>

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

            <asp:Panel ID="pnlAlerta" runat="server" Visible="false" CssClass="alert">
                <asp:Label ID="lbMensaje" runat="server" Text=""></asp:Label>
            </asp:Panel>

        </div>

        <%-- BOTONES --%>
        <div class="columna-botones">
            <asp:Button ID="btnGenerarEvaluacion" runat="server" Text="Generar evaluacion del adoptante" CssClass="btn btn-especial" OnClick="btnGenerarEvaluacion_Click" />
            <asp:Button ID="btnModificar"         runat="server" Text="Modificar"                        CssClass="btn"             OnClick="btnModificar_Click" />
            <asp:Button ID="btnAplicar"           runat="server" Text="Aplicar"                          CssClass="btn"             OnClick="btnAplicar_Click" />
            <asp:Button ID="btnCancelar"          runat="server" Text="Cancelar"                         CssClass="btn btn-secundario" OnClick="btnCancelar_Click" />
            <asp:Button ID="btnSalir"             runat="server" Text="Salir"                            CssClass="btn btn-secundario" OnClick="btnSalir_Click" />
        </div>

    </div>
</div>

</asp:Content>
