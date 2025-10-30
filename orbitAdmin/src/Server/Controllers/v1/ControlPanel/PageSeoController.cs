using SchoolV01.Application.Services;
using SchoolV01.Shared.ViewModels.Pages;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using SchoolV01.Shared.Wrapper;
using AutoMapper;
using SchoolV01.Shared.ViewModels.Menus;
using System.Collections.Generic;

namespace SchoolV01.Api.Controllers
{
    public class PageSeoController : ApiControllerBase
    {
        private readonly IPageSeoService SeoService;

        public PageSeoController(IPageSeoService SeoService)
        {
            this.SeoService = SeoService;
        }
        [HttpGet]
        [Route("all/{PageId:int}")]
        public async Task<ActionResult<List<PageSeoViewModel>>> GetAll(int PageId)
        {
            try
            {
                List<PageSeoViewModel> data = await SeoService.GetSeoByPageId(PageId);

                if (data != null)
                {
                    return Ok(data);
                }
                else
                    return StatusCode(StatusCodes.Status500InternalServerError,
                  "Error retrieving data");

            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data");
            }
        }



        [HttpGet("{id:int}")]
        public async Task<ActionResult<PageSeoViewModel>> Get(int id)
        {
            try
            {
                var result = await SeoService.GetSeoById(id);

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
        public async Task<ActionResult<PageSeoViewModel>> Create(PageSeoInsertModel SeoInsertModel)
        {
            try
            {
                if (SeoInsertModel == null)
                    return BadRequest();

                var createdSeo = await SeoService.AddSeo(SeoInsertModel);

                if (createdSeo != null)
                {
                    return CreatedAtAction(nameof(Get),
                      new { id = createdSeo.Id }, createdSeo);
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
        public async Task<ActionResult<PageSeoViewModel>> Update(int id, PageSeoUpdateModel SeoUpdateModel)
        {
            try
            {
                if (SeoUpdateModel.Id != id)
                {
                    return NotFound("IDs are not matching");
                }
                var transaltionToUpdate = await SeoService.GetSeoById(id);

                if (transaltionToUpdate == null)
                    return NotFound($"Record with Id = {SeoUpdateModel.Id} not found");

                var updatedTransaltion = await SeoService.UpdateSeo(SeoUpdateModel);

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
        public async Task<ActionResult<PageSeoViewModel>> Delete(int id)
        {
            try
            {
                var transaltionToDelete = await SeoService.GetSeoById(id);

                if (transaltionToDelete == null)
                {
                    return NotFound($"Transaltion with Id = {id} not found");
                }

                var result = await SeoService.SoftDeleteSeo(id);
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
