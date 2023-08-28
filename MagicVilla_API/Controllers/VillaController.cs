using AutoMapper;
using MagicVilla_API.Datos;
using MagicVilla_API.Modelos;
using MagicVilla_API.Modelos.Dto;
using MagicVilla_API.Repositorio.IRepositorio;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace MagicVilla_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VillaController : ControllerBase
    {
        private readonly ILogger<VillaController> _logger;        
        private readonly IVillaRepositorio _villaRepo;
        private readonly IMapper _mapper;
        protected APIResponse _apiResponse;

        public VillaController(ILogger<VillaController> logger,                                                           
                               IVillaRepositorio villaRepo, 
                               IMapper mapper)
        {
            _logger = logger;
            _villaRepo = villaRepo;            
            _mapper = mapper;
            _apiResponse = new();
        }

        // Trae todos los registros
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetVillas()
        {
            try
            {
                _logger.LogInformation("Obtener las Villas");
                IEnumerable<Villa> villaList = await _villaRepo.ObtenerTodos();
                _apiResponse.Resultado = _mapper.Map<IEnumerable<VillaDto>>(villaList);
                _apiResponse.statusCode = HttpStatusCode.OK;
                return Ok(_apiResponse);
            }
            catch (Exception ex)
            {
                _apiResponse.IsExitoso = false;
                _apiResponse.ErrorMessages = new List<string>() { ex.ToString() };
            }
            return _apiResponse;
        }

        // Trae un registro filtrado por id 
        [HttpGet("id:int",Name = "GetVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetVilla(int id)
        {
            try
            {
                if (id == 0)
                {
                    _logger.LogError("Error al traer Villa con Id " + id);
                    _apiResponse.IsExitoso = false;
                    _apiResponse.statusCode = HttpStatusCode.BadRequest;                    
                    return BadRequest(_apiResponse);
                }

                var villa = await _villaRepo.Obtener(v => v.Id == id);

                if (villa == null)
                {
                    _apiResponse.IsExitoso = false;
                    _apiResponse.statusCode = HttpStatusCode.NotFound;                    
                    return NotFound(_apiResponse);
                }

                _apiResponse.Resultado = _mapper.Map<VillaDto>(villa);
                _apiResponse.statusCode = HttpStatusCode.OK;
                return Ok(_apiResponse);
            }
            catch (Exception ex)
            {
                _apiResponse.IsExitoso = false;
                _apiResponse.ErrorMessages = new List<string>() { ex.ToString() };
            }
            return _apiResponse;            
        }

        // Crear nuevos registros
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> CrearVilla([FromBody] VillaCreateDto createDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (await _villaRepo.Obtener(v => v.Nombre.ToLower() == createDto.Nombre.ToLower()) != null)
                {
                    ModelState.AddModelError("NombreExiste", "Ya Existe Villa Con Ese Nombre");
                    return BadRequest(ModelState);
                }

                if (createDto == null)
                {
                    return BadRequest();
                }

                Villa modelo = _mapper.Map<Villa>(createDto);

                await _villaRepo.Crear(modelo);
                _apiResponse.Resultado = modelo;
                _apiResponse.statusCode = HttpStatusCode.Created;

                return CreatedAtRoute("GetVilla", new { id = modelo.Id }, _apiResponse);
            }
            catch (Exception ex)
            {
                _apiResponse.IsExitoso = false;
                _apiResponse.ErrorMessages = new List<string>() { ex.ToString() };
            }
            return _apiResponse;
        }

        // Eliminar registros por id
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteVilla(int id)
        {
            try
            {             
                if (id == 0 || id < 0)
                {
                    _apiResponse.IsExitoso = false;
                    _apiResponse.statusCode=HttpStatusCode.BadRequest;
                    return BadRequest(_apiResponse);
                }

                var villa = await _villaRepo.Obtener(v => v.Id == id);

                if (villa == null)
                {
                    _apiResponse.IsExitoso = false;
                    _apiResponse.statusCode=HttpStatusCode.NotFound;
                    return NotFound(_apiResponse);
                }

                await _villaRepo.Remover(villa);
                _apiResponse.statusCode = HttpStatusCode.NoContent;
                return Ok(_apiResponse);
            }
            catch (Exception ex)
            {
                _apiResponse.IsExitoso = false;
                _apiResponse.ErrorMessages = new List<string>() { ex.ToString() };
            }
            return BadRequest(_apiResponse);
        }

        // Actualiza registros
        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateVilla(int id, [FromBody] VillaUpdateDto updateDto)
        {
            if (id == 0 || id < 0)
            {
                _apiResponse.IsExitoso = false;
                _apiResponse.statusCode = HttpStatusCode.BadRequest;
                return BadRequest(_apiResponse);
            }

            if (updateDto == null)
            {
                _apiResponse.IsExitoso = false;
                _apiResponse.statusCode = HttpStatusCode.InternalServerError;
                return BadRequest(_apiResponse);
            }

            if (id != updateDto.Id)
            {
                _apiResponse.IsExitoso = false;
                _apiResponse.statusCode = HttpStatusCode.BadRequest;
                return BadRequest(_apiResponse);
            }            

            Villa modelo = _mapper.Map<Villa>(updateDto);

            await _villaRepo.Actualizar(modelo);
            _apiResponse.statusCode = HttpStatusCode.NoContent;
            return Ok(_apiResponse);
        }

        // Actualizar parcial
        [HttpPatch("{id:int}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> UpdatePartialVilla(int id, JsonPatchDocument<VillaUpdateDto> patchDto)
        {
            if (patchDto == null || id == 0 || id < 0)
            {    
                _apiResponse.IsExitoso = false;
                _apiResponse.statusCode = HttpStatusCode.BadRequest;
                return BadRequest();
            }            
            
            var villa = await _villaRepo.Obtener(v => v.Id == id, tracked:false);

            VillaUpdateDto villaDto = _mapper.Map<VillaUpdateDto>(villa);

            patchDto.ApplyTo(villaDto, ModelState);

            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Villa modelo = _mapper.Map<Villa>(villaDto);

            await _villaRepo.Actualizar(modelo);
            _apiResponse.statusCode = HttpStatusCode.NoContent;
            return Ok(_apiResponse);            
        }
    }
}
