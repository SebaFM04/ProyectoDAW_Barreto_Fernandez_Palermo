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

function buscarInfoRaza() {
    var raza = document.getElementById(ids.raza).value.trim();

    if (raza === '') {
        alert('Ingresá una raza primero.');
        return;
    }

    var apiKey = 'live_gLkC0lnwJ6s2OjVe1YcxFTHeGkdEAdfMvSGGKiiRYeZ5o976tXtI4q6LNu161gLT';

    fetch('https://api.thecatapi.com/v1/breeds/search?q=' + encodeURIComponent(raza), {
        headers: { 'x-api-key': apiKey }
    })
        .then(function (response) {
            if (!response.ok) {
                throw new Error('Error al consultar el servicio');
            }
            return response.json();
        })
        .then(function (data) {
            var caja = document.getElementById('infoRaza');
            var img = document.getElementById('imgRaza');
            var texto = document.getElementById('txtInfoRaza');

            if (data.length === 0) {
                caja.style.display = 'block';
                img.style.display = 'none';
                texto.textContent = 'No se encontró información para esa raza.';
                return;
            }

            var breed = data[0];

            texto.textContent = 'Temperamento: ' + traducirTemperamento(breed.temperament || 'No especificado') +
                ' | Esperanza de vida: ' + (breed.life_span || 'N/D') + ' años';

            if (breed.reference_image_id) {
                img.src = 'https://cdn2.thecatapi.com/images/' + breed.reference_image_id + '.jpg';
                img.style.display = 'block';
            } else {
                img.style.display = 'none';
            }

            caja.style.display = 'block';
        })
        .catch(function (error) {
            alert('No se pudo consultar el servicio: ' + error.message);
        });
}

var traducciones = {
    'Active': 'Activo', 'Agile': 'Ágil', 'Clever': 'Inteligente',
    'Sociable': 'Sociable', 'Loving': 'Cariñoso', 'Energetic': 'Enérgico',
    'Curious': 'Curioso', 'Intelligent': 'Inteligente', 'Loyal': 'Leal',
    'Sweet': 'Dulce', 'Playful': 'Juguetón', 'Gentle': 'Gentil',
    'Independent': 'Independiente', 'Affectionate': 'Afectuoso'
};

function traducirTemperamento(texto) {
    var palabras = texto.split(', ');
    var traducidas = palabras.map(function (p) {
        return traducciones[p.trim()] || p.trim();
    });
    return traducidas.join(', ');
}
