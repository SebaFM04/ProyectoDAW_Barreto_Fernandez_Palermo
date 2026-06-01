<%@ Page Title="Animales" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ListadoAnimales.aspx.cs" Inherits="ListadoAnimales" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="Estilos/EstilosInicio.css" rel="stylesheet" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <section class="animals-section">
        <h2 class="section-title">Animales Disponibles para la Adopcion</h2>

        <div class="animals-container">

            <%--  FILTROS --%>
            <aside class="filters-sidebar">
                <div class="filter-header">
                    <span class="filter-icon">&#9663;</span>
                    <span>Filtros</span>
                </div>

                <div class="filter-group">
                    <label class="filter-label">Especie</label>
                    <asp:DropDownList ID="ddlEspecie" runat="server" CssClass="filter-select"
                        AutoPostBack="true" OnSelectedIndexChanged="ddlEspecie_Changed">
                        <asp:ListItem Value="">Todos</asp:ListItem>

                    </asp:DropDownList>
                </div>

                <div class="filter-group">
                    <label class="filter-label">Raza</label>
                    <asp:DropDownList ID="ddlRaza" runat="server" CssClass="filter-select"
                        AutoPostBack="true" OnSelectedIndexChanged="ddlRaza_Changed">
                        <asp:ListItem Value="">Todos</asp:ListItem>
                    </asp:DropDownList>
                </div>

                <div class="filter-group">
                    <label class="filter-label">Genero</label>
                    <asp:DropDownList ID="ddlGenero" runat="server" CssClass="filter-select"
                        AutoPostBack="true" OnSelectedIndexChanged="Filtros_Changed">
                        <asp:ListItem Value="">Todos</asp:ListItem>
                        <asp:ListItem Value="Macho">Macho</asp:ListItem>
                        <asp:ListItem Value="Hembra">Hembra</asp:ListItem>
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
                                        <span class="info-label">Tamaño:</span>
                                        <span class="info-val"><%# Eval("tamano") %></span>
                                    </div>
                                    <div class="info-row">
                                        <span class="info-label">Genero:</span>
                                        <span class="info-val"><%# Eval("sexo") %></span>
                                    </div>
                                    <div class="info-row">
                                        <span class="info-label">Estado:</span>
                                        <span class="info-val"><%# Eval("estadoAdopcion") %></span>
                                    </div>
                                </div>


                            </div>

                        </div>
                    </ItemTemplate>
                </asp:Repeater>

                <asp:Panel ID="pnlSinAnimales" runat="server" Visible="false" CssClass="sin-animales">
                    <p>No hay animales disponibles con los filtros seleccionados.</p>
                </asp:Panel>

            </div>
        </div>
    </section>

</asp:Content>
