using BE;
using BLL;
using SERVICIOS;
using System;
using System.Collections.Generic;
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
    [HttpGet]
    [Route("api/animales/{codigo}")]
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
        if (!claseSession.Gestor.Session())
            return Unauthorized();

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
    [Route("api/animales/{codigo}")]
    public IHttpActionResult Put(string codigo, [FromBody] Animal datos)
    {
        if (!claseSession.Gestor.Session())
            return Unauthorized();

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
    [Route("api/animales/{codigo}")]
    public IHttpActionResult Delete(string codigo)
    {
        if (!claseSession.Gestor.Session())
            return Unauthorized();

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
}