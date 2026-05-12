<%@ Page Language="C#" AutoEventWireup="true" CodeFile="GestionAnimal.aspx.cs" Inherits="GestionAnimal" MasterPageFile="~/MasterPage.master" %>

<asp:Content ID="ContentHead" ContentPlaceHolderID="head" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</asp:Content>

<asp:Content ID="ContentMain" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
    </div>
    <asp:Button ID="btnVacunas" runat="server" OnClick="btnVacunas_Click" Text="Agregar vacunas" />
</asp:Content>
