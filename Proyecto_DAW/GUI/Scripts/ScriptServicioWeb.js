// Consume el servicio web REST propio (GET api/animales) y muestra el JSON recibido.

// Textos traducidos que dejó el servidor en la página
function mensaje(clave) {
    var span = document.querySelector('[data-msg="' + clave + '"]');
    return span ? span.textContent : clave;
}

function mostrarEstado(texto, esError) {
    var lbl = document.getElementById('lblEstado');
    lbl.textContent = texto;
    lbl.className = esError ? 'servicio-estado error' : 'servicio-estado';
}

function llenarTabla(animales) {
    var cuerpo = document.querySelector('#tblAnimales tbody');
    cuerpo.innerHTML = '';

    animales.forEach(function (a) {
        var fila = document.createElement('tr');
        var estado = a.vivo ? a.estadoAdopcion : mensaje('fallecido');
        [a.codigoAnimal, a.nombre, a.especie, a.raza, a.tamano, a.sexo, estado].forEach(function (valor) {
            var celda = document.createElement('td');
            celda.textContent = valor;   // textContent: nunca se interpreta como HTML
            fila.appendChild(celda);
        });
        cuerpo.appendChild(fila);
    });
}

function consultar(url) {
    var inicio = Date.now();
    mostrarEstado(mensaje('cargando'), false);

    fetch(url, { headers: { 'Accept': 'application/json' } })
        .then(function (response) {
            var linea = 'GET ' + url + ' → ' + response.status + ' ' + response.statusText + ' (' + (Date.now() - inicio) + ' ms)';

            if (response.status === 404) {
                llenarTabla([]);
                document.getElementById('preJson').textContent = '';
                mostrarEstado(linea + ' · ' + mensaje('noEncontrado'), true);
                return;
            }
            if (!response.ok) throw new Error(linea);

            return response.json().then(function (datos) {
                var animales = Array.isArray(datos) ? datos : [datos];
                llenarTabla(animales);
                document.getElementById('preJson').textContent = JSON.stringify(datos, null, 2);
                mostrarEstado(linea + ' · ' + animales.length + ' ' + mensaje('cantidad'), false);
            });
        })
        .catch(function (error) {
            mostrarEstado(mensaje('error') + ' ' + error.message, true);
        });
}

document.getElementById('btnListar').addEventListener('click', function () {
    consultar('api/animales');
});

document.getElementById('btnBuscar').addEventListener('click', function () {
    var codigo = document.getElementById('txtCodigo').value.trim();

    if (!/^\d+$/.test(codigo)) {
        mostrarEstado(mensaje('codigoInvalido'), true);
        return;
    }
    consultar('api/animales/' + codigo);
});

// Enter en el código busca, en vez de hacer postback del formulario
document.getElementById('txtCodigo').addEventListener('keydown', function (e) {
    if (e.key === 'Enter') {
        e.preventDefault();
        document.getElementById('btnBuscar').click();
    }
});

// Al abrir la página ya se muestra el listado
consultar('api/animales');
