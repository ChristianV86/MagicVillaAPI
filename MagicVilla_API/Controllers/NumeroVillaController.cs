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
    [ApiController]
    [Route("api/[controller]")]    
    public class NumeroVillaController : ControllerBase
    {
        private readonly ILogger<NumeroVillaController> _logger;
        private readonly IVillaRepositorio _villaRepo;
        private readonly INumeroVillaRepositorio _numVillaRepo;
        private readonly IMapper _mapper;
        protected APIResponse _apiResponse;

        public NumeroVillaController(ILogger<NumeroVillaController> logger,
                                                    IVillaRepositorio villaRepo,
                                                    INumeroVillaRepositorio numeroVillaRepo, 
                                                    IMapper mapper)
        {
            _logger = logger;
            _villaRepo = villaRepo;
            _numVillaRepo = numeroVillaRepo;            
            _mapper = mapper;
            _apiResponse = new();
        }

        // Trae todos los registros
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetNumeroVillas()
        {
            try
            {
                _logger.LogInformation("Obtener las Número Villas");
                IEnumerable<NumeroVilla> numVillaList = await _numVillaRepo.ObtenerTodos();
                _apiResponse.Resultado = _mapper.Map<IEnumerable<NumeroVillaDto>>(numVillaList);
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
        [HttpGet("id:int",Name = "GetNumeroVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetNumeroVilla(int id)
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

                var numVilla = await _numVillaRepo.Obtener(v => v.NumVilla == id);

                if (numVilla == null)
                {
                    _apiResponse.IsExitoso = false;
                    _apiResponse.statusCode = HttpStatusCode.NotFound;                    
                    return NotFound(_apiResponse);
                }

                _apiResponse.Resultado = _mapper.Map<NumeroVillaDto>(numVilla);
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
        public async Task<ActionResult<APIResponse>> CrearNumeroVilla([FromBody] NumeroVillaCreateDto createNumDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (await _numVillaRepo.Obtener(v => v.NumVilla == createNumDto.NumVilla) != null)
                {
                    ModelState.AddModelError("NumeroExiste", "Ya Existe ese Número de Villa");
                    return BadRequest(ModelState);
                }

                if(await _villaRepo.Obtener(v => v.Id == createNumDto.VillaId) == null)
                {
                    ModelState.AddModelError("ClaveForanea", "No Existe Villa con ese Id");
                    return BadRequest(ModelState);
                }

                if (createNumDto == null)
                {
                    return BadRequest();
                }

                NumeroVilla modelo = _mapper.Map<NumeroVilla>(createNumDto);
                
                await _numVillaRepo.Crear(modelo);
                _apiResponse.Resultado = modelo;
                _apiResponse.statusCode = HttpStatusCode.Created;

                return CreatedAtRoute("GetNumeroVilla", new { id = modelo.NumVilla }, _apiResponse);
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
        public async Task<IActionResult> DeleteNumeroVilla(int id)
        {
            try
            {             
                if (id == 0 || id < 0)
                {
                    _apiResponse.IsExitoso = false;
                    _apiResponse.statusCode=HttpStatusCode.BadRequest;
                    return BadRequest(_apiResponse);
                }

                var numVilla = await _numVillaRepo.Obtener(v => v.NumVilla == id);

                if (numVilla == null)
                {
                    _apiResponse.IsExitoso = false;
                    _apiResponse.statusCode=HttpStatusCode.NotFound;
                    return NotFound(_apiResponse);
                }

                await _numVillaRepo.Remover(numVilla);
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
        public async Task<IActionResult> UpdateNumeroVilla(int id, [FromBody] NumeroVillaUpdateDto updateNumDto)
        {
            if (id == 0 || id < 0)
            {
                _apiResponse.IsExitoso = false;
                _apiResponse.statusCode = HttpStatusCode.BadRequest;
                return BadRequest(_apiResponse);
            }

            if (updateNumDto == null)
            {
                _apiResponse.IsExitoso = false;
                _apiResponse.statusCode = HttpStatusCode.InternalServerError;
                return BadRequest(_apiResponse);
            }

            if (id != updateNumDto.NumVilla)
            {
                _apiResponse.IsExitoso = false;
                _apiResponse.statusCode = HttpStatusCode.BadRequest;
                return BadRequest(_apiResponse);
            }   
            
            if(await _villaRepo.Obtener(v => v.Id == updateNumDto.NumVilla) == null)
            {
                ModelState.AddModelError("ClaveForanea", "No Existe Villa con ese Id");
                return BadRequest(ModelState);
            }

            NumeroVilla modelo = _mapper.Map<NumeroVilla>(updateNumDto);
            
            await _numVillaRepo.Actualizar(modelo);
            _apiResponse.statusCode = HttpStatusCode.NoContent;
            return Ok(_apiResponse);
        }        
    }
}
