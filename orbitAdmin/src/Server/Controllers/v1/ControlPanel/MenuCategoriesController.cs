using Microsoft.AspNetCore.Mvc;
using SchoolV01.Application.Services;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Http;
using SchoolV01.Shared.ViewModels.Menus;
using System.Linq;
using SchoolV01.Shared.Wrapper;
using SchoolV01.Server.Controllers;
using SchoolV01.Api.Controllers;
using SchoolV01.Shared.Constants.Permission;
using Microsoft.AspNetCore.Authorization;

namespace SchoolV01.Server.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class MenuCategoriesController : ApiControllerBase
    {
        private readonly IMenuCategoryService menuCategoryService;

        public MenuCategoriesController(IMenuCategoryService menuCategoryService)
        {
            this.menuCategoryService = menuCategoryService;
        }

        [HttpGet]
        [Route("all")]
        public async Task<ActionResult> Get()
        {
            try
            {
                var data = await menuCategoryService.GetMenuCategories();

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

        [HttpGet]
        public async Task<ActionResult> Get( string searchString, string orderBy)
        {
            try
            {
                var filteredData = await menuCategoryService.GetPagedMenuCategories(searchString, orderBy);

                var response = new PagedResponse<MenuCategoryViewModel>(filteredData, 0, 10, filteredData.Count());
                return Ok(response);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data");
            }
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<MenuCategoryViewModel>> Get(int id)
        {
            try
            {
                var result = await menuCategoryService.GetMenuCategoryByID(id);

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
        public async Task<ActionResult<MenuCategoryViewModel>> Create(MenuCategoryInsertModel menuCategoryInsertModel)
        {
            try
            {
                if (menuCategoryInsertModel == null)
                    return BadRequest();
                // TODO : implement restriction to prevent adding an existing category model 
                var createdMenuCategory = await menuCategoryService.AddMenuCategory(menuCategoryInsertModel);

                if (createdMenuCategory != null)
                {
                    return CreatedAtAction(nameof(Get),
                      new { id = createdMenuCategory.Id }, createdMenuCategory);
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

        [Authorize(Policy = Permissions.WebSiteManagement.Edit)]
        [HttpPut("{id:int}")]
        public async Task<ActionResult<MenuCategoryViewModel>> Update(int id, MenuCategoryUpdateModel menuCategoryUpdateModel)
        {
            try
            {
                if (menuCategoryUpdateModel.Id != id)
                {
                    return NotFound("Category IDs are not matching");
                }
                var menuCategoryToUpdate = await menuCategoryService.GetMenuCategoryByID(id);

                if (menuCategoryToUpdate == null)
                    return NotFound($"Record with Id = {menuCategoryUpdateModel.Id} not found");

                var updatedMenuCategory = await menuCategoryService.UpdateMenuCategory(menuCategoryUpdateModel);

                if (updatedMenuCategory != null)
                {
                    return Ok(updatedMenuCategory);
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

        [Authorize(Policy = Permissions.WebSiteManagement.Delete)]
        [HttpDelete("{id:int}")]
        public async Task<ActionResult<MenuCategoryViewModel>> Delete(int id)
        {
            try
            {
                var menuCategoryToDelete = await menuCategoryService.GetMenuCategoryByID(id);

                if (menuCategoryToDelete == null)
                {
                    return NotFound($"Record with Id = {id} not found");
                }

                var result = await menuCategoryService.SoftDeleteMenuCategory(id);
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
   