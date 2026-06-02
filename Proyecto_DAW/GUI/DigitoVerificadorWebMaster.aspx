<%@ Page Title="Dígito Verificador" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="DigitoVerificadorWebMaster.aspx.cs" Inherits="_Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <link href="digito-verificador.css" rel="stylesheet" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

    <div class="dv-page">
        <div class="dv-container">

            <h2 class="dv-title">Inconsistencia en la base de datos</h2>

            <asp:Button ID="btnRecalcular" runat="server" CssClass="dv-btn"
                Text="Recalcular" OnClick="btnRecalcular_Click" />

            <%-- Desplegable con los .bak disponibles en el servidor --%>
            <asp:DropDownList ID="ddlBackups" runat="server" CssClass="dv-select">
            </asp:DropDownList>

            <asp:Button ID="btnRestore" runat="server" CssClass="dv-btn"
                Text="Restore" OnClick="btnRestore_Click" />

            <div>
                <label class="dv-label">Tablas con inconsistencias:</label>
                <asp:ListBox ID="lstTablas" runat="server" CssClass="dv-listbox" Rows="6">
                </asp:ListBox>
            </div>

        </div>
    </div>

</asp:Content>