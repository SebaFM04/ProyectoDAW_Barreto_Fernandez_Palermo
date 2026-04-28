<%@ Page Language="C#" AutoEventWireup="true" CodeFile="GestionVacuna.aspx.cs" Inherits="GestionVacuna" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
        </div>
        <asp:GridView ID="gvVacunas" runat="server">
            <Columns>
                 <asp:BoundField DataField="codigoVacuna" HeaderText="Código vacuna" />
                 <asp:BoundField DataField="codigoAnimal" HeaderText="Código animal" />
                 <asp:BoundField DataField="nombreVacuna" HeaderText="Nombre de vacuna" />
                 <asp:BoundField DataField="fechaAplicacion" HeaderText="Fecha de aplicación" DataFormatString="{0:dd:MM:yyyy}" />
                 <asp:BoundField DataField="fechaAplicacionProxima" HeaderText="Proxima aplicación" DataFormatString="{0:dd:MM:yyyy}" />
            </Columns>
        </asp:GridView>
        <asp:GridView ID="gvAnimales" runat="server">
            <Columns>
                <asp:BoundField DataField="codigoAnimal" HeaderText="Código animal" />
                <asp:BoundField DataField="especie" HeaderText="Especie" />
                <asp:BoundField DataField="raza" HeaderText="Raza" />
                <asp:BoundField DataField="nombre" HeaderText="Nombre" />
                <asp:BoundField DataField="tamaño" HeaderText="Tamaño" />
                <asp:BoundField DataField="estadoAdopcion" HeaderText="Estado de adopción" />
                <asp:BoundField DataField="vivo" HeaderText="Vivo" />
            </Columns>
        </asp:GridView>
        <asp:Button ID="btnAlta" runat="server" Text="Alta" />
        <asp:TextBox ID="txtFechaAplicacionProxima" runat="server"></asp:TextBox>
        <asp:TextBox ID="txtFechaAplicacion" runat="server"></asp:TextBox>
        <asp:TextBox ID="txtNombreVacuna" runat="server"></asp:TextBox>
        <asp:TextBox ID="txtCodigoVacuna" runat="server"></asp:TextBox>
        <asp:Button ID="btnBaja" runat="server" Text="Baja" />
    </form>
</body>
</html>
