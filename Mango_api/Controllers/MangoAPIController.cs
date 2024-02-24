using Mango_api.Data;
using Mango_api.Models.Dto;
using Mango_API.Data;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace Mango_api.Controllers
{
    [ApiController]
    [Route("api/MangoAPI")]
    public class MangoAPIController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        public MangoAPIController(ApplicationDbContext db) 
        {
            _db = db;
        }
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<MangoDTO>> GetMangoes()
        {
            return Ok(_db.Mangoes.ToList());
        }

        [HttpGet("{id:int}", Name="GetMango")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<MangoDTO> GetMango(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }
            var mango = _db.Mangoes.ToList().FirstOrDefault(u => u.Id == id);
            if (mango == null)
            {
                return NotFound();
            }

            return Ok(mango);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<MangoDTO> CreateMango([FromBody] MangoDTO mangoDTO)
        {
            if (_db.Mangoes.FirstOrDefault(u => u.Name.ToLower() == mangoDTO.Name.ToLower()) != null)
            {
                ModelState.AddModelError("CustomError", "Mango already Exists!");
                return BadRequest(ModelState);
            }
            if (mangoDTO == null)
            {
                return BadRequest(mangoDTO);
            }
            if (mangoDTO.Id > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            mangoDTO.Id = MangoStore.mangoList.OrderByDescending(u => u.Id).FirstOrDefault().Id + 1;
            MangoStore.mangoList.Add(mangoDTO);


            return CreatedAtRoute("GetMango", new { id = mangoDTO.Id }, mangoDTO);
        }


        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpDelete("{id:int}", Name = "DeleteMango")]
        public IActionResult DeleteMango(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }
            var mango = MangoStore.mangoList.FirstOrDefault(u => u.Id == id);
            if (mango == null)
            {
                return NotFound();
            }
            MangoStore.mangoList.Remove(mango);
            return NoContent();

        }

        [HttpPut("{id:int}", Name = "UpdateMango")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult UpdateMango(int id, [FromBody] MangoDTO mangoDTO)
        {
            if (mangoDTO == null || id != mangoDTO.Id)
            {
                return BadRequest();
            }
            var mango = MangoStore.mangoList.FirstOrDefault(u => u.Id == id);
            mango.Name = mangoDTO.Name;
            mango.Price = mangoDTO.Price;
            mango.Weight = mangoDTO.Weight;

            return NoContent();

        }

        [HttpPatch("{id:int}", Name = "UpdatePartialMango")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult UpdatePartialMango(int id, JsonPatchDocument<MangoDTO> patchDTO)
        {
            if (patchDTO == null || id == 0)
            {
                return BadRequest();
            }
            var mango = MangoStore.mangoList.FirstOrDefault(u => u.Id == id);
            if (mango == null)
            {
                return BadRequest();
            }
            patchDTO.ApplyTo(mango, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return NoContent();
        }

    }
}
