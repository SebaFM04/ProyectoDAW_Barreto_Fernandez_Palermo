<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="BackupRestore.aspx.cs" Inherits="_Default" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <link rel="stylesheet" href="../Estilos/EstilosBackupRestore.css" type="text/css" />

    <div class="backup-wrapper">
        <h2 class="backup-titulo" runat="server" data-traduccion="pageBackup_titBackupRestore">Backup & Restore</h2>

        <%-- PANEL DE ALERTA --%>
        <asp:Panel ID="pnlAlerta" runat="server" Visible="false">
            <asp:Label ID="lblMensajeError" runat="server" />
        </asp:Panel>

        <%-- SECCIÓN BACKUP --%>
        <div class="backup-seccion">
            <h3 runat="server" data-traduccion="pageBackup_titBackup">Backup</h3>
            <p runat="server" data-traduccion="pageBackup_lblDescripcionBackup">Generá una copia de seguridad de la base de datos.</p>
            <asp:Button ID="btnBackUp" runat="server"
                        OnClick="btnBackUp_Click"
                        Text="Realizar Backup" data-traduccion="pageBackup_btnRealizarBackup"
                        CssClass="btn-verde" 
                        data-permiso="REALIZAR_BACKUP"/>
        </div>

        <hr />

        <%-- SECCIÓN RESTORE --%>
        <div class="backup-seccion">
            <h3 runat="server" data-traduccion="pageBackup_titRestore">Restore</h3>
            <p runat="server" data-traduccion="pageBackup_lblDescripcionRestore">Seleccioná un backup de la lista para restaurar la base de datos.</p>
            <div class="fila-restore">
                <asp:DropDownList ID="ddlBackups" runat="server" CssClass="ctrl">
                </asp:DropDownList>
                <asp:Button ID="btnRestore" runat="server"
                            OnClick="btnRestore_Click"
                            Text="Restaurar" data-traduccion="pageBackup_btnRestaurar"
                            CssClass="btn-rojo" 
                            data-permiso="REALIZAR_RESTORE"/>
            </div>
        </div>
    </div>
</asp:Content>