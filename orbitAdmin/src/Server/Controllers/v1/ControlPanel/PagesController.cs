using SchoolV01.Application.Services;
using SchoolV01.Shared.Constants.Permission;
using SchoolV01.Shared.ViewModels.Pages;
using SchoolV01.Shared.Wrapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolV01.Api.Controllers
{

    public class PagesController : ApiControllerBase
    {
        private readonly IPageService pageService;

        public PagesController(IPageService pageService)
        {
            this.pageService = pageService;
        }
        [AllowAnonymous]

        [HttpGet]
        public async Task<ActionResult> Get(int pageNumber, int pageSize, string searchString, string orderBy)
        {
            try
            {
                var filteredData = await pageService.GetPagedPages(searchString, orderBy);

                var pagedData = filteredData
                .Skip((pageNumber) * pageSize)
               .Take(pageSize)
               .ToList();

                var response = new PagedResponse<PageViewModel>(pagedData, pageNumber, pageSize, filteredData.Count());
                return Ok(response);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data");
            }
        }
        [AllowAnonymous]

        [HttpGet("{id:int}")]
        public async Task<ActionResult<PageViewModel>> Get(int id)
        {
            try
            {
                var result = await pageService.GetPageByID(id);

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

        [AllowAnonymous]

        [HttpGet("PageByEndpoint/{Endpoint}")]
        public async Task<ActionResult<PageViewModel>> GetPageByEndpoint(string Endpoint)
        {
            try
            {
                var result = await pageService.GetPageByEndpoint(Endpoint);

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




        [Authorize(Policy = Permissions.WebSiteManagement.Create)]
        [HttpPost]
        public async Task<ActionResult<PageViewModel>> Create(PageInsertModel pageInsertModel)
        {
            try
            {
                if (pageInsertModel == null)
                    return BadRequest();
                // TODO : implement restriction to prevent adding an existing model 
                var createdPage = await pageService.AddPage(pageInsertModel);

                if (createdPage != null)
                {
                    return CreatedAtAction(nameof(Get), new { id = createdPage.Id }, createdPage);
                }
                else
                {
                    //return StatusCode(StatusCodes.Status500InternalServerError,
                    //"Error creating new record");
                    return StatusCode(StatusCodes.Status500InternalServerError, "Error creating new record");
                }
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error creating new record");
            }
        }

        [Authorize(Policy = Permissions.WebSiteManagement.Edit)]
        [HttpPut("{id:int}")]
        public async Task<ActionResult<PageViewModel>> Update(int id, PageUpdateModel pageUpdateModel)
        {
            try
            {
                if (pageUpdateModel.Id != id)
                {
                    return NotFound("IDs are not matching");
                }
                var pageToUpdate = await pageService.GetPageByID(id);

                if (pageToUpdate == null)
                    return NotFound($"Record with Id = {pageUpdateModel.Id} not found");

                var updatedPage = await pageService.UpdatePage(pageUpdateModel);

                if (updatedPage != null)
                {
                    return Ok(updatedPage);
                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error updateing data");
                }
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error updating data");
            }
        }

        [Authorize(Policy = Permissions.WebSiteManagement.Delete)]
        [HttpDelete("{id:int}")]
        public async Task<ActionResult<PageViewModel>> Delete(int id)
        {
            try
            {
                var pageToDelete = await pageService.GetPageByID(id);

                if (pageToDelete == null)
                {
                    return NotFound($"Record with Id = {id} not found");
                }

                var result = await pageService.SoftDeletePage(id);
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
