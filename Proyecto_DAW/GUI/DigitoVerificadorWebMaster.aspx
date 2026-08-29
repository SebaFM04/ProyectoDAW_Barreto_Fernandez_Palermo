<%@ Page Title="Dígito Verificador" Language="C#" MasterPageFile="~/MasterPageLogin.master" AutoEventWireup="true" CodeFile="DigitoVerificadorWebMaster.aspx.cs" Inherits="_Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <link href="Estilos/EstilosDigitoWebMaster.css" rel="stylesheet" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

    <div class="dv-page">

        <div id="popupOverlay" class="dv-popup-overlay" style="display:none;">
            <div class="dv-popup-box">
                <p id="popupMessage"></p>
                <button type="button" class="dv-btn" onclick="cerrarPopup()">Aceptar</button>
            </div>
        </div>

        <div class="dv-container">

            <h2 class="dv-title">Inconsistencia en la base de datos</h2>

            <asp:Button ID="btnRecalcular" runat="server" CssClass="dv-btn"
                Text="Recalcular" OnClick="btnRecalcular_Click" data-permiso="DV_RECALCULAR"/>

           <asp:DropDownList ID="ddlBackups" runat="server" CssClass="dv-select"></asp:DropDownList>

            <asp:Button ID="btnRestore" runat="server" CssClass="dv-btn"
                Text="Restore" OnClick="btnRestore_Click" data-permiso="DV_RESTORE"/>

            <div>
                <label class="dv-label">Tablas con inconsistencias:</label>
                <asp:ListBox ID="lstTablas" runat="server" CssClass="dv-listbox" Rows="6">
                </asp:ListBox>
            </div>

            <div>
                <asp:Button ID="btnCancelar" runat="server" CssClass="dv-btn" Text="Cancelar" OnClick="btnCancelar_Click"/>
            </div>

        </div>
    </div>

    <script src="Scripts/ScriptDigitoWebMaster.js"></script>
</asp:Content>