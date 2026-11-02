<%@ Page Title="Servicio Web" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ServicioWebAnimales.aspx.cs" Inherits="ServicioWebAnimales" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="Estilos/EstilosServicioWeb.css" rel="stylesheet" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <section class="servicio-section">
        <h2 class="servicio-titulo" runat="server" data-traduccion="pageServicioWeb_titServicioWeb">Servicio Web de Animales</h2>
        <p class="servicio-desc" runat="server" data-traduccion="pageServicioWeb_lblDescripcion">Esta página consume el servicio web REST propio del refugio. Los datos llegan en formato JSON desde el endpoint y se muestran sin recargar la página.</p>

        <%-- ENDPOINTS --%>
        <div class="servicio-panel">
            <div class="servicio-fila">
                <span class="metodo">GET</span>
                <a class="endpoint" href="api/animales" target="_blank">/api/animales</a>
                <button type="button" class="btn" id="btnListar" runat="server" ClientIDMode="Static" data-traduccion="pageServicioWeb_btnConsultarListado">Consultar listado</button>
            </div>

            <div class="servicio-fila">
                <span class="metodo">GET</span>
                <span class="endpoint">/api/animales/{codigo}</span>
                <input type="number" id="txtCodigo" class="servicio-input" min="1" placeholder="Código" runat="server" ClientIDMode="Static" data-traduccion-title="pageServicioWeb_txtCodigo" title="Código" />
                <button type="button" class="btn" id="btnBuscar" runat="server" ClientIDMode="Static" data-traduccion="pageServicioWeb_btnBuscarCodigo">Buscar por código</button>
            </div>
        </div>

        <%-- ESTADO DE LA ÚLTIMA LLAMADA --%>
        <p id="lblEstado" class="servicio-estado"></p>

        <%-- RESULTADO EN TABLA --%>
        <div class="servicio-tabla-contenedor">
            <table class="grid" id="tblAnimales">
                <thead>
                    <tr>
                        <th runat="server" data-traduccion="pageServicioWeb_colCodigo">Código</th>
                        <th runat="server" data-traduccion="pageServicioWeb_colNombre">Nombre</th>
                        <th runat="server" data-traduccion="pageServicioWeb_colEspecie">Especie</th>
                        <th runat="server" data-traduccion="pageServicioWeb_colRaza">Raza</th>
                        <th runat="server" data-traduccion="pageServicioWeb_colTamano">Tamaño</th>
                        <th runat="server" data-traduccion="pageServicioWeb_colSexo">Sexo</th>
                        <th runat="server" data-traduccion="pageServicioWeb_colEstado">Estado</th>
                    </tr>
                </thead>
                <tbody></tbody>
            </table>
        </div>

        <%-- RESPUESTA JSON TAL CUAL LLEGA --%>
        <h3 class="servicio-subtitulo" runat="server" data-traduccion="pageServicioWeb_titRespuestaJson">Respuesta JSON</h3>
        <pre id="preJson" class="servicio-json"></pre>

        <%-- Textos que arma el JS: se traducen del lado del servidor y el script los lee de acá --%>
        <div class="oculto">
            <span data-msg="cargando" runat="server" data-traduccion="pageServicioWeb_msgCargando">Consultando el servicio...</span>
            <span data-msg="cantidad" runat="server" data-traduccion="pageServicioWeb_msgCantidad">animales recibidos</span>
            <span data-msg="noEncontrado" runat="server" data-traduccion="pageServicioWeb_msgNoEncontrado">No existe un animal con ese código.</span>
            <span data-msg="codigoInvalido" runat="server" data-traduccion="pageServicioWeb_msgCodigoInvalido">Ingresá un código numérico.</span>
            <span data-msg="error" runat="server" data-traduccion="pageServicioWeb_msgError">No se pudo consultar el servicio web.</span>
            <span data-msg="fallecido" runat="server" data-traduccion="pageServicioWeb_msgFallecido">Fallecido</span>
        </div>
    </section>

    <script src="Scripts/ScriptServicioWeb.js"></script>
</asp:Content>
