using SchoolV01.Application.Services;
using SchoolV01.Shared.ViewModels.Pages;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace SchoolV01.Api.Controllers
{
    public class PageAttachementController : ApiControllerBase
    {
        private readonly IPageAttachementService AttachementService;

        public PageAttachementController(IPageAttachementService AttachementService)
        {
            this.AttachementService = AttachementService;
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<PageAttachementViewModel>> Get(int id)
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
        public async Task<ActionResult<PageAttachementViewModel>> Create(PageAttachementInsertModel AttachementInsertModel)
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
        public async Task<ActionResult<PageAttachementViewModel>> Update(int id, PageAttachementUpdateModel AttachementUpdateModel)
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
        public async Task<ActionResult<PageAttachementViewModel>> Delete(int id)
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
