using BE;
using BLL;
using SERVICIOS;
using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Http;

public class AnimalesApiController : ApiController
{
    private readonly bllAnimal _bll = new bllAnimal();

    // GET api/animales
    [HttpGet]
    [Route("api/animales")]
    public IHttpActionResult GetAll()
    {
        List<Animal> lista = _bll.RetornarAnimales();
        return Ok(lista);
    }

    // GET api/animales/5
    // {codigo:int}: un código no numérico da 404 en vez de llegar a la BD y devolver un 500
    [HttpGet]
    [Route("api/animales/{codigo:int}")]
    public IHttpActionResult GetPorCodigo(string codigo)
    {
        Animal animal = _bll.BuscarAnimalPorCodigo(codigo);
        if (animal == null) return NotFound();
        return Ok(animal);
    }

    // POST api/animales
    [HttpPost]
    [Route("api/animales")]
    public IHttpActionResult Post([FromBody] Animal nuevo)
    {
        IHttpActionResult rechazo = ValidarAcceso("ANIMAL_ALTA");
        if (rechazo != null) return rechazo;
        if (nuevo == null) return BadRequest("Falta el animal en el cuerpo del request");

        try
        {
            _bll.AltaAnimal(nuevo.especie, nuevo.raza, nuevo.nombre,
                             nuevo.tamaño, nuevo.sexo, nuevo.estadoAdopcion, nuevo.vivo);
            return Ok("Animal creado correctamente");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    // PUT api/animales/5
    [HttpPut]
    [Route("api/animales/{codigo:int}")]
    public IHttpActionResult Put(string codigo, [FromBody] Animal datos)
    {
        IHttpActionResult rechazo = ValidarAcceso("ANIMAL_MODIFICAR");
        if (rechazo != null) return rechazo;
        if (datos == null) return BadRequest("Falta el animal en el cuerpo del request");

        try
        {
            _bll.Modificar(codigo, datos.especie, datos.raza, datos.nombre,
                            datos.tamaño, datos.sexo, datos.estadoAdopcion, datos.vivo);
            return Ok("Animal modificado correctamente");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    // DELETE api/animales/5
    [HttpDelete]
    [Route("api/animales/{codigo:int}")]
    public IHttpActionResult Delete(string codigo)
    {
        IHttpActionResult rechazo = ValidarAcceso("ANIMAL_BAJA");
        if (rechazo != null) return rechazo;

        try
        {
            _bll.Baja(codigo);
            return Ok("Animal eliminado correctamente");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    // Mismo permiso que pide el botón equivalente en RegistroAnimales.aspx (data-permiso).
    // Sin sesión -> 401; logueado pero sin ese permiso -> 403. Devuelve null si puede seguir.
    private IHttpActionResult ValidarAcceso(string permiso)
    {
        if (!claseSession.Gestor.Session())
            return Unauthorized();

        if (!PermisoSesion.TienePermiso(permiso))
            return StatusCode(HttpStatusCode.Forbidden);

        return null;
    }
}
