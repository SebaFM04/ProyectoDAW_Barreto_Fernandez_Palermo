<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Bitacora.aspx.cs" Inherits="_Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:GridView ID="gvProductos" runat="server" AutoGenerateColumns="true"
    CssClass="table table-bordered"
    EmptyDataText="No hay datos para mostrar">
    </asp:GridView>
    <div>
        <!--Panel generico para mostrar errores-->
    <asp:Panel ID="pnlAlerta" runat="server" Visible="false" CssClass="login-alert login-alert-error">
        <asp:Label ID="lblMensajeError" runat="server" Text=""></asp:Label>
    </asp:Panel>
    </div>
    <p>
    </p>
</asp:Content>

