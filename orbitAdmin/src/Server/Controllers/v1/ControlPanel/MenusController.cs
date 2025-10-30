using SchoolV01.Application.Services;
using SchoolV01.Shared.ViewModels.Menus;
using SchoolV01.Shared.Wrapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using SchoolV01.Server.Controllers;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using SchoolV01.Shared.Constants.Permission;

namespace SchoolV01.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class MenusController : ApiControllerBase
    {
        private readonly IMenuService menuService;

        public MenusController(IMenuService menuService)
        {
            this.menuService = menuService;
        }
        [AllowAnonymous]

        [HttpGet]
        [Route("all")]
        public async Task<ActionResult<List<MenuViewModel>>> Get()
        {
            try
            {
                List<MenuViewModel> data = await menuService.GetMenus();

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
        [AllowAnonymous]

        [HttpGet]
        public async Task<IActionResult> Get(string categoryId, string searchString, string orderBy)
        {
            try
            {
                int convertedCategoryId;
                var isConvertable = Int32.TryParse(categoryId, out convertedCategoryId);

                if (!isConvertable)
                {
                    return BadRequest($"try correct category Id!");
                }

                var filteredData = await menuService.GetPagedMenus(searchString, orderBy);

                if (convertedCategoryId != 0)
                {
                    filteredData = filteredData.Where(x => x.CategoryId == convertedCategoryId).ToList();
                }

                var response = new PagedResponse<MenuViewModel>(filteredData, 0, 10, filteredData.Count());
                return Ok(response);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data");
            }
        }
        [AllowAnonymous]

        [HttpGet("GetMaster")]
        public async Task<IActionResult> GetMaster(string categoryId, int? menuId, string searchString, string orderBy)
        {
            try
            {
                int convertedCategoryId;
                var isConvertable = Int32.TryParse(categoryId, out convertedCategoryId);

                if (!isConvertable)
                {
                    return BadRequest($"try correct category Id!");
                }

                var filteredData = await menuService.GetPagedMenus(searchString, orderBy);

                if (convertedCategoryId != 0)
                {
                    filteredData = filteredData.Where(x => x.CategoryId == convertedCategoryId && x.ParentId == menuId).OrderBy(x => x.LevelOrder).ToList();
                }

                var response = new PagedResponse<MenuViewModel>(filteredData, 0, 10, filteredData.Count());
                return Ok(response);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data");
            }
        }
        [AllowAnonymous]

        [HttpGet]
        [Route("NoCategory")]
        public async Task<IActionResult> Get( string searchString, string orderBy)
        {
            try
            {
                var filteredData = await menuService.GetPagedMenus(searchString, orderBy);
                var response = new PagedResponse<MenuViewModel>(filteredData, 0, 10, filteredData.Count());
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
        public async Task<ActionResult<MenuViewModel>> Get(int id)
        {
            try
            {
                var result = await menuService.GetMenuById(id);

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
        public async Task<ActionResult<MenuViewModel>> Create(MenuInsertModel menuInsertModel)
        {
            try
            {
                if (menuInsertModel == null)
                    return BadRequest();

                // TODO : implement restriction to prevent adding an existing model 
                var createdMenu = await menuService.AddMenu(menuInsertModel);

                if (createdMenu != null)
                {
                    return CreatedAtAction(nameof(Get),
                      new { id = createdMenu.Id }, createdMenu);
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
        public async Task<ActionResult<MenuViewModel>> Update(int id, MenuUpdateModel menuUpdateModel)
        {
            try
            {
                if (menuUpdateModel.Id != id)
                {
                    return BadRequest("IDs  are not matching");
                }
                //var menuToUpdate = await menuService.GetMenuById(id);

                //if (menuToUpdate == null)
                //    return NotFound($"Record with Id = {id} is not found");

                var updatedMenu = await menuService.UpdateMenu(menuUpdateModel);

                if (updatedMenu != null)
                {
                    return Ok(updatedMenu);
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
        public async Task<ActionResult<MenuViewModel>> Delete(int id)
        {
            try
            {
                var menuToDelete = await menuService.GetMenuById(id);

                if (menuToDelete == null)
                {
                    return NotFound($"Menu with Id = {id} not found");
                }

                var result = await menuService.SoftDeleteMenu(id);
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
