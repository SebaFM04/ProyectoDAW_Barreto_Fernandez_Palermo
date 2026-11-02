-- Traducciones de la página ServicioWebAnimales.aspx y su ítem de menú.
-- Se puede ejecutar más de una vez: solo inserta lo que falta.
USE [dawRefugio]
GO

DECLARE @textos TABLE (Clave NVARCHAR(200), Es NVARCHAR(1000), En NVARCHAR(1000));
INSERT INTO @textos (Clave, Es, En) VALUES
(N'master_menuServicioWeb',            N'Servicio Web (JSON)',          N'Web Service (JSON)'),
(N'pageServicioWeb_titServicioWeb',    N'Servicio Web de Animales',     N'Animals Web Service'),
(N'pageServicioWeb_lblDescripcion',    N'Esta página consume el servicio web REST propio del refugio. Los datos llegan en formato JSON desde el endpoint y se muestran sin recargar la página.',
                                       N'This page consumes the shelter''s own REST web service. The data arrives as JSON from the endpoint and is shown without reloading the page.'),
(N'pageServicioWeb_btnConsultarListado', N'Consultar listado',           N'Get list'),
(N'pageServicioWeb_txtCodigo',         N'Código',                       N'Code'),
(N'pageServicioWeb_btnBuscarCodigo',   N'Buscar por código',            N'Search by code'),
(N'pageServicioWeb_colCodigo',         N'Código',                       N'Code'),
(N'pageServicioWeb_colNombre',         N'Nombre',                       N'Name'),
(N'pageServicioWeb_colEspecie',        N'Especie',                      N'Species'),
(N'pageServicioWeb_colRaza',           N'Raza',                         N'Breed'),
(N'pageServicioWeb_colTamano',         N'Tamaño',                       N'Size'),
(N'pageServicioWeb_colSexo',           N'Sexo',                         N'Sex'),
(N'pageServicioWeb_colEstado',         N'Estado',                       N'Status'),
(N'pageServicioWeb_titRespuestaJson',  N'Respuesta JSON',               N'JSON response'),
(N'pageServicioWeb_msgCargando',       N'Consultando el servicio...',   N'Calling the service...'),
(N'pageServicioWeb_msgCantidad',       N'animales recibidos',           N'animals received'),
(N'pageServicioWeb_msgNoEncontrado',   N'No existe un animal con ese código.', N'There is no animal with that code.'),
(N'pageServicioWeb_msgCodigoInvalido', N'Ingresá un código numérico.',  N'Enter a numeric code.'),
(N'pageServicioWeb_msgError',          N'No se pudo consultar el servicio web.', N'The web service could not be reached.'),
(N'pageServicioWeb_msgFallecido',      N'Fallecido',                    N'Deceased');

INSERT INTO Etiqueta (Clave)
SELECT t.Clave FROM @textos t
WHERE NOT EXISTS (SELECT 1 FROM Etiqueta e WHERE e.Clave = t.Clave);

INSERT INTO Traduccion (ID_Idioma, ID_Etiqueta, Texto)
SELECT i.ID, e.ID, CASE i.Codigo WHEN N'es' THEN t.Es ELSE t.En END
FROM @textos t
JOIN Etiqueta e ON e.Clave = t.Clave
JOIN Idioma i ON i.Codigo IN (N'es', N'en')
WHERE NOT EXISTS (SELECT 1 FROM Traduccion x WHERE x.ID_Idioma = i.ID AND x.ID_Etiqueta = e.ID);
GO
