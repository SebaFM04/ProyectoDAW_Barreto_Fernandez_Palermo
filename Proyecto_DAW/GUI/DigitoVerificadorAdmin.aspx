<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="DigitoVerificadorAdmin.aspx.cs" Inherits="DigitoVerificadorAdmin" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <link href="Estilos/EstilosDigitoWebMaster.css" rel="stylesheet" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="dv-page">
        <div class="dv-container">
            <h2 class="dv-title">Inconsistencia en la base de datos</h2>
            <asp:Label ID="lblMensaje" runat="server"
                Text="Se detectaron inconsistencias en la base de datos. Por favor, comuníquese con el Web Master para resolver el problema."
                CssClass="dv-label" />
        </div>
    </div>
</asp:Content>