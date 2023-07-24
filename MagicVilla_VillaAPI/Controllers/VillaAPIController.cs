using MagicVilla_VillaAPI.Data;
using MagicVilla_VillaAPI.Logging;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Models.Dto;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace MagicVilla_VillaAPI.Controllers
{
    [Route("api/VillaAPI")] //Route does not change since i had code the controller name
    [ApiController]
    public class VillaAPIController : ControllerBase
    {
        /*private readonly ILogger<VillaAPIController> _logger;

        //Using dependency injection to get Ilogger implementation
        public VillaAPIController(ILogger<VillaAPIController> logger)
        {
            _logger = logger;
        } */



        /* private readonly ILogging _logger;

        public VillaAPIController(ILogging logger)
        {
            _logger = logger;
        } */

        public VillaAPIController()
        {
            
        }

        //Get endpoint
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<VillaDTO>> GetVillas()
        {
            //_logger.Log("Getting all villas", "");
            return Ok(VillaStore.villalist);
        }

        [HttpGet("id:int", Name = "GetVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]


        //[ProducesResponseType(200, Type = typeof(VillaDTO))]
        //[ProducesResponseType(404)]
        //[ProducesResponseType(400)]
        public ActionResult<VillaDTO> GetVilla(int id) //<VillaDTO> the return type
        {
            //Add validation
            if (id == 0)
            {
               // _logger.Log("Get Villa Error with Id" + id, "error");
                return BadRequest();

            }
            var villa = VillaStore.villalist.FirstOrDefault(u => u.Id == id);
            if (villa == null)
            {
                return NotFound(); //404 status code
            }

            return Ok();

        }

        [HttpPost] //To create a resource
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<VillaDTO> CreateVilla([FromBody] VillaDTO villaDTO)
        {
            //Adding custom Validations
            //if(!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}

            if (VillaStore.villalist.FirstOrDefault(u => u.Name.ToLower() == villaDTO.Name.ToLower()) != null)
            {
                ModelState.AddModelError("CustomError", "Villa already Exists!");
                return BadRequest(ModelState);
            }

            if (villaDTO == null)
            {
                return BadRequest(villaDTO);
            }
            if (villaDTO.Id > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError); //Return custom error
            }
            villaDTO.Id = VillaStore.villalist.OrderByDescending(u => u.Id).FirstOrDefault().Id + 1;
            VillaStore.villalist.Add(villaDTO);

            return CreatedAtRoute("GetVilla", new { id = villaDTO.Id }, villaDTO);
        }


        [HttpDelete("id:int", Name = "DeleteVilla")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public IActionResult DeleteVilla(int id) //Return type is IActionResult
        {
            if (id == 0)
            {
                return BadRequest();
            }
            var villa = VillaStore.villalist.FirstOrDefault(u => u.Id == id);
            if (villa == null)
            {
                return NotFound();
            }
            VillaStore.villalist.Remove(villa);
            return NoContent();

        }


         [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult UpdateVilla(int id, [FromBody] VillaDTO villaDTO)
        {
            if (villaDTO == null || id != villaDTO.Id)
            {
                return BadRequest();

            }
            var villa = VillaStore.villalist.FirstOrDefault(u => u.Id == id);
            villa.Name = villaDTO.Name;
            villa.sqft = villaDTO.sqft;
            villa.Occupancy = villaDTO.Occupancy;

            return NoContent();

        }


        [ HttpPatch("id:int", Name = "UpdatePartialVilla")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public IActionResult UpdatePartialVila(int id, JsonPatchDocument<VillaDTO> patchDTO ) 
        {
            if(patchDTO == null || id == 0)
            {
                return BadRequest();
            }
            var villa = VillaStore.villalist.FirstOrDefault(u => u.Id == id);
            if(villa == null)
            {
                return BadRequest();
            }
            patchDTO.ApplyTo(villa, ModelState); //Store error in the model state
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return NoContent();


        }
    }
}
