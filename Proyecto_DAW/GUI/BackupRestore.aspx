<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="BackupRestore.aspx.cs" Inherits="_Default" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <link rel="stylesheet" href="../Estilos/EstilosBackupRestore.css" type="text/css" />

    <div class="backup-wrapper">
        <h2 class="backup-titulo">Backup & Restore</h2>

        <%-- PANEL DE ALERTA --%>
        <asp:Panel ID="pnlAlerta" runat="server" Visible="false">
            <asp:Label ID="lblMensajeError" runat="server" />
        </asp:Panel>

        <%-- SECCIÓN BACKUP --%>
        <div class="backup-seccion">
            <h3>Backup</h3>
            <p>Generá una copia de seguridad de la base de datos.</p>
            <asp:Button ID="btnBackUp" runat="server"
                        OnClick="btnBackUp_Click"
                        Text="Realizar Backup"
                        CssClass="btn-verde" 
                        data-permiso="REALIZAR_BACKUP"/>
        </div>

        <hr />

        <%-- SECCIÓN RESTORE --%>
        <div class="backup-seccion">
            <h3>Restore</h3>
            <p>Seleccioná un backup de la lista para restaurar la base de datos.</p>
            <div class="fila-restore">
                <asp:DropDownList ID="ddlBackups" runat="server" CssClass="ctrl">
                </asp:DropDownList>
                <asp:Button ID="btnRestore" runat="server"
                            OnClick="btnRestore_Click"
                            Text="Restaurar"
                            CssClass="btn-rojo" 
                            data-permiso="REALIZAR_RESTORE"/>
            </div>
        </div>
    </div>
</asp:Content>