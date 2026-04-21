<%@ Page Title="Registro de Animales" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="RegistroAnimales.aspx.cs" Inherits="RegistroAnimales" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="head" runat="server">
    <link href="Estilos/EstilosRegistroAnimales.css" rel="stylesheet" />
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<div class="page-container">

    <h2 class="titulo">Registro de Animales</h2>

    <div class="layout">

        <!-- IZQUIERDA: GRID -->
        <div class="grid-container">
            <asp:GridView ID="gvAnimales" runat="server" CssClass="grid" AutoGenerateColumns="false">
                <Columns>
                    <asp:BoundField DataField="Codigo" HeaderText="Código" />
                    <asp:BoundField DataField="Especie" HeaderText="Especie" />
                    <asp:BoundField DataField="Raza" HeaderText="Raza" />
                    <asp:BoundField DataField="Nombre" HeaderText="Nombre" />
                </Columns>
            </asp:GridView>
        </div>

        <!-- DERECHA: FORM -->
        <div class="form-box">

            <div class="form-left">

                <label>Especie</label>
                <asp:TextBox ID="txtEspecie" runat="server" CssClass="input" />

                <label>Raza</label>
                <asp:TextBox ID="txtRaza" runat="server" CssClass="input" />

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
                    <asp:ListItem Text="Disponible" />
                    <asp:ListItem Text="Adoptado" />
                </asp:DropDownList>

            </div>

            <!-- BOTONES -->
            <div class="form-right">
                <button type="button" class="btn" onclick="alta()">Alta</button>
                <button type="button" class="btn" onclick="modificar()">Modificar</button>
                <button type="button" class="btn" onclick="baja()">Baja</button>

                <hr />

                <button type="button" class="btn secundario" onclick="limpiar()">Limpiar</button>
                <button type="button" class="btn salir" onclick="salir()">Salir</button>
            </div>

        </div>

    </div>

</div>

<script src="Scripts/ScriptRegistroAnimales.js"></script>

</asp:Content>
