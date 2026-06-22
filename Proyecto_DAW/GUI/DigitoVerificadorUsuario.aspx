<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPageLogin.master" AutoEventWireup="true" CodeFile="DigitoVerificadorUsuario.aspx.cs" Inherits="DigitoVerificadorUsuario" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <link href="Estilos/EstilosDigitoWebMaster.css" rel="stylesheet" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="dv-page">
        <div class="dv-container">
            <h2 class="dv-title">Página no disponible</h2>
            <asp:Label ID="lblMensaje" runat="server"
                Text="El sistema no se encuentra disponible en este momento. Intente más tarde."
                CssClass="dv-label" />
        </div>
    </div>
</asp:Content>