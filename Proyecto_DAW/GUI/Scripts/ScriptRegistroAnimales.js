function alta() {
    alert("Alta de animal (front)");
}

function modificar() {
    alert("Modificar animal");
}

function baja() {
    alert("Baja de animal");
}

function limpiar() {
    document.querySelectorAll(".input").forEach(i => i.value = "");
}

function salir() {
    window.location.href = "MenuPrincipal.aspx";
}