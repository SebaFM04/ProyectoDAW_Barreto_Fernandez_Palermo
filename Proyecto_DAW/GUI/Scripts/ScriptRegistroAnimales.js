function limpiarFormulario() {
    document.getElementById(ids.especie).value = '';
    document.getElementById(ids.raza).value = '';
    document.getElementById(ids.nombre).value = '';

    document.getElementById(ids.tamano).selectedIndex = 0;
    document.getElementById(ids.sexo).selectedIndex = 0;
    document.getElementById(ids.estado).selectedIndex = 0;

}

function ocultarAlerta(inmediato = false) {
    var panel = document.getElementById(ids.alerta);

    if (panel) {

        if (inmediato) {
            panel.style.display = 'none'; // Oculta al instante
        }
        else
        {
            setTimeout(function () {
                panel.style.display = 'none';
            }, 5000); // Oculta después de 5 segundos
        }
    }
}

// Vuelvo a la pagina principal
function salir() {
    window.location.href = 'MenuPrincipal.aspx';
}

// Manda un mensaje para confirmar
function confirmarBaja() {
    return confirm("¿Estás seguro que querés dar de baja este animal?");
}