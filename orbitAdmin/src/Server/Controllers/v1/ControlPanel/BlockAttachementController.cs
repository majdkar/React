using SchoolV01.Application.Services;
using SchoolV01.Shared.ViewModels.Blocks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace SchoolV01.Api.Controllers
{
    public class BlockAttachementController : ApiControllerBase
    {
        private readonly IBlockAttachementService AttachementService;

        public BlockAttachementController(IBlockAttachementService AttachementService)
        {
            this.AttachementService = AttachementService;
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<BlockAttachementViewModel>> Get(int id)
        {
            try
            {
                var result = await AttachementService.GetAttachementById(id);

                if (result == null)
                    return NotFound();

                return result;
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data");
            }
        }

        [HttpPost]
        public async Task<ActionResult<BlockAttachementViewModel>> Create(BlockAttachementInsertModel AttachementInsertModel)
        {
            try
            {
                if (AttachementInsertModel == null)
                    return BadRequest();

                var createdAttachement = await AttachementService.AddAttachement(AttachementInsertModel);

                if (createdAttachement != null)
                {
                    return CreatedAtAction(nameof(Get),
                      new { id = createdAttachement.Id }, createdAttachement);
                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error creating new record");
                }
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error creating new record");
            }
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<BlockAttachementViewModel>> Update(int id, BlockAttachementUpdateModel AttachementUpdateModel)
        {
            try
            {
                if (AttachementUpdateModel.Id != id)
                {
                    return NotFound("IDs are not matching");
                }
                var transaltionToUpdate = await AttachementService.GetAttachementById(id);

                if (transaltionToUpdate == null)
                    return NotFound($"Record with Id = {AttachementUpdateModel.Id} not found");

                var updatedTransaltion = await AttachementService.UpdateAttachement(AttachementUpdateModel);

                if (updatedTransaltion != null)
                {
                    return Ok(updatedTransaltion);
                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError,
                   "Error updating data");
                }
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error updating data");
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<BlockAttachementViewModel>> Delete(int id)
        {
            try
            {
                var transaltionToDelete = await AttachementService.GetAttachementById(id);

                if (transaltionToDelete == null)
                {
                    return NotFound($"Transaltion with Id = {id} not found");
                }

                var result = await AttachementService.SoftDeleteAttachement(id);
                if (result)
                {
                    return Ok();
                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error deleting data");
                }
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error deleting data");
            }
        }

    }
}
