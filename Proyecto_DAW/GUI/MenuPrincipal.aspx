<%@ Page Title="MenuPrincipal" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="MenuPrincipal.aspx.cs" Inherits="Default2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="Estilos/EstilosInicio.css" rel="stylesheet" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <section class="hero-section">
        <h1 class="hero-title" runat="server" data-traduccion="pageMenu_titBienvenidos">Bienvenidos al Refugio Oeste</h1>
        <p class="hero-desc" runat="server" data-traduccion="pageMenu_lblDescripcion">
            Somos un refugio dedicado al rescate, cuidado y rehabilitación de animales en
            situación de abandono. Nuestro objetivo es brindarles una segunda oportunidad y
            encontrarles un hogar lleno de amor. Cada animal tiene una historia única y merece
            ser parte de una familia que lo valore. Te invitamos a conocer a nuestros
            compañeros peludos y considerar darle un hogar a uno de ellos.
        </p>
    </section>

    <%-- ===== SECCIÓN ANIMALES ===== --%>
    <section class="animals-section">
        <h2 class="section-title" runat="server" data-traduccion="pageMenu_titAnimalesDisponibles">Animales Disponibles para la Adopcion</h2>

        <div class="animals-container">

            <%-- FILTROS --%>
            <aside class="filters-sidebar">
                <div class="filter-header">
                    <span class="filter-icon">&#9663;</span>
                    <span runat="server" data-traduccion="pageMenu_titFiltros">Filtros</span>
                </div>

                <div class="filter-group">
                    <label class="filter-label" runat="server" data-traduccion="pageMenu_lblEspecie">Especie</label>
                    <asp:DropDownList ID="ddlEspecie" runat="server" CssClass="filter-select"
                        AutoPostBack="true" OnSelectedIndexChanged="ddlEspecie_Changed">
                        <asp:ListItem Value="" data-traduccion="pageMenu_optTodos">Todos</asp:ListItem>

                    </asp:DropDownList>
                </div>

                <div class="filter-group">
                    <label class="filter-label" runat="server" data-traduccion="pageMenu_lblRaza">Raza</label>
                    <asp:DropDownList ID="ddlRaza" runat="server" CssClass="filter-select"
                        AutoPostBack="true" OnSelectedIndexChanged="ddlRaza_Changed">
                        <asp:ListItem Value="" data-traduccion="pageMenu_optTodos">Todos</asp:ListItem>
                    </asp:DropDownList>
                </div>

                <div class="filter-group">
                    <label class="filter-label" runat="server" data-traduccion="pageMenu_lblGenero">Genero</label>
                    <asp:DropDownList ID="ddlGenero" runat="server" CssClass="filter-select"
                        AutoPostBack="true" OnSelectedIndexChanged="Filtros_Changed">
                        <asp:ListItem Value="" data-traduccion="pageMenu_optTodos">Todos</asp:ListItem>
                        <asp:ListItem Value="Macho" data-traduccion="pageMenu_optMacho">Macho</asp:ListItem>
                        <asp:ListItem Value="Hembra" data-traduccion="pageMenu_optHembra">Hembra</asp:ListItem>
                    </asp:DropDownList>
                </div>
            </aside>

            <div class="cards-grid">

                <asp:Repeater ID="rptAnimales" runat="server">
                    <ItemTemplate>
                        <div class="animal-card">

                            <%-- Placeholder visual A FUTURO AGREGAR IMAGENES POR CADA ANIMAL--%>
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
                                        <span class="info-label"><%# Traducir("pageMenu_lblTamano", "Tamaño:") %></span>
                                        <span class="info-val"><%# Eval("tamaño") %></span>
                                    </div>
                                    <div class="info-row">
                                        <span class="info-label"><%# Traducir("pageMenu_lblGenero2", "Genero:") %></span>
                                        <span class="info-val"><%# Eval("sexo") %></span>
                                    </div>
                                    <div class="info-row">
                                        <span class="info-label"><%# Traducir("pageMenu_lblEstado", "Estado:") %></span>
                                        <span class="info-val"><%# Eval("estadoAdopcion") %></span>
                                    </div>
                                </div>
                                                                
                            </div>

                        </div>
                    </ItemTemplate>
                </asp:Repeater>

                <asp:Panel ID="pnlSinAnimales" runat="server" Visible="false" CssClass="sin-animales">
                    <p runat="server" data-traduccion="pageMenu_msgSinAnimales">No hay animales disponibles con los filtros seleccionados.</p>
                </asp:Panel>

            </div>
        </div>
    </section>

</asp:Content>
