function mostrarPopup(mensaje, redirectUrl) {
    document.getElementById("popupMessage").textContent = mensaje;
    document.getElementById("popupOverlay").style.display = "flex";
    window.__popupRedirect = redirectUrl || null;
}

function cerrarPopup() {
    document.getElementById("popupOverlay").style.display = "none";
    if (window.__popupRedirect) {
        window.location = window.__popupRedirect;
    }
}