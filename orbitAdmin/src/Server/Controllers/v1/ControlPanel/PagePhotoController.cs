using SchoolV01.Application.Services;
using SchoolV01.Shared.ViewModels.Pages;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace SchoolV01.Api.Controllers
{
    public class PagePhotoController : ApiControllerBase
    {
        private readonly IPagePhotoService photoService;

        public PagePhotoController(IPagePhotoService photoService)
        {
            this.photoService = photoService;
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<PagePhotoViewModel>> Get(int id)
        {
            try
            {
                var result = await photoService.GetPhotoById(id);

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
        public async Task<ActionResult<PagePhotoViewModel>> Create(PagePhotoInsertModel photoInsertModel)
        {
            try
            {
                if (photoInsertModel == null)
                    return BadRequest();

                var createdPhoto = await photoService.AddPhoto(photoInsertModel);

                if (createdPhoto != null)
                {
                    return CreatedAtAction(nameof(Get),
                      new { id = createdPhoto.Id }, createdPhoto);
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
        public async Task<ActionResult<PagePhotoViewModel>> Update(int id, PagePhotoUpdateModel photoUpdateModel)
        {
            try
            {
                if (photoUpdateModel.Id != id)
                {
                    return NotFound("IDs are not matching");
                }
                var transaltionToUpdate = await photoService.GetPhotoById(id);

                if (transaltionToUpdate == null)
                    return NotFound($"Record with Id = {photoUpdateModel.Id} not found");

                var updatedTransaltion = await photoService.UpdatePhoto(photoUpdateModel);

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
        public async Task<ActionResult<PagePhotoViewModel>> Delete(int id)
        {
            try
            {
                var transaltionToDelete = await photoService.GetPhotoById(id);

                if (transaltionToDelete == null)
                {
                    return NotFound($"Transaltion with Id = {id} not found");
                }

                var result = await photoService.SoftDeletePhoto(id);
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
