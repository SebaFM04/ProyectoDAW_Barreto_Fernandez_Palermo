<%@ Page Title="Animales" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ListadoAnimales.aspx.cs" Inherits="ListadoAnimales" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="Estilos/EstilosInicio.css" rel="stylesheet" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <section class="animals-section">
        <h2 class="section-title" runat="server" data-traduccion="pageListadoAnimales_titAnimalesDisponibles">Animales Disponibles para la Adopcion</h2>

        <div class="animals-container">

            <%--  FILTROS --%>
            <aside class="filters-sidebar">
                <div class="filter-header">
                    <span class="filter-icon">&#9663;</span>
                    <span runat="server" data-traduccion="pageListadoAnimales_titFiltros">Filtros</span>
                </div>

                <div class="filter-group">
                    <label class="filter-label" runat="server" data-traduccion="pageListadoAnimales_lblEspecie">Especie</label>
                    <asp:DropDownList ID="ddlEspecie" runat="server" CssClass="filter-select"
                        AutoPostBack="true" OnSelectedIndexChanged="ddlEspecie_Changed">
                        <asp:ListItem Value="" data-traduccion="pageListadoAnimales_optTodos">Todos</asp:ListItem>

                    </asp:DropDownList>
                </div>

                <div class="filter-group">
                    <label class="filter-label" runat="server" data-traduccion="pageListadoAnimales_lblRaza">Raza</label>
                    <asp:DropDownList ID="ddlRaza" runat="server" CssClass="filter-select"
                        AutoPostBack="true" OnSelectedIndexChanged="ddlRaza_Changed">
                        <asp:ListItem Value="" data-traduccion="pageListadoAnimales_optTodos">Todos</asp:ListItem>
                    </asp:DropDownList>
                </div>

                <div class="filter-group">
                    <label class="filter-label" runat="server" data-traduccion="pageListadoAnimales_lblGenero">Genero</label>
                    <asp:DropDownList ID="ddlGenero" runat="server" CssClass="filter-select"
                        AutoPostBack="true" OnSelectedIndexChanged="Filtros_Changed">
                        <asp:ListItem Value="" data-traduccion="pageListadoAnimales_optTodos">Todos</asp:ListItem>
                        <asp:ListItem Value="Macho" data-traduccion="pageListadoAnimales_optMacho">Macho</asp:ListItem>
                        <asp:ListItem Value="Hembra" data-traduccion="pageListadoAnimales_optHembra">Hembra</asp:ListItem>
                    </asp:DropDownList>
                </div>

            </aside>

            <%-- GRID DE CARDS --%>
            <div class="cards-grid">

                <asp:Repeater ID="rptAnimales" runat="server">
                    <ItemTemplate>
                        <div class="animal-card">

                            <img src='<%# Eval("especie").ToString().ToLower() == "gato" ? "Imagenes/gato.png" : "Imagenes/perro.png" %>'
                                alt='<%# Eval("especie") %>'
                                class="animal-img" />

                            <div class="animal-body">
                                <div class="animal-header">
                                    <span class="animal-name"><%# Eval("nombre") %></span>
                                    <span class='badge badge-<%# Eval("especie").ToString().ToLower() %>'>
                                        <%# Eval("especie") %>
                                    </span>
                                </div>

                                <p class="animal-breed"><%# Eval("raza") %></p>

                                <div class="animal-info">
                                    <div class="info-row">
                                        <span class="info-label"><%# Traducir("pageListadoAnimales_lblTamano", "Tamaño:") %></span>
                                        <span class="info-val"><%# Eval("tamano") %></span>
                                    </div>
                                    <div class="info-row">
                                        <span class="info-label"><%# Traducir("pageListadoAnimales_lblGenero2", "Genero:") %></span>
                                        <span class="info-val"><%# Eval("sexo") %></span>
                                    </div>
                                    <div class="info-row">
                                        <span class="info-label"><%# Traducir("pageListadoAnimales_lblEstado", "Estado:") %></span>
                                        <span class="info-val"><%# Eval("estadoAdopcion") %></span>
                                    </div>
                                </div>


                            </div>

                        </div>
                    </ItemTemplate>
                </asp:Repeater>

                <asp:Panel ID="pnlSinAnimales" runat="server" Visible="false" CssClass="sin-animales">
                    <p runat="server" data-traduccion="pageListadoAnimales_msgSinAnimales">No hay animales disponibles con los filtros seleccionados.</p>
                </asp:Panel>

            </div>
        </div>
    </section>

</asp:Content>
