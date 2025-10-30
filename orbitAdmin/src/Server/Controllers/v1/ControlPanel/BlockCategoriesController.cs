using SchoolV01.Application.Services;
using SchoolV01.Shared.ViewModels.Blocks;
using SchoolV01.Shared.Wrapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using SchoolV01.Shared.Constants.Permission;
using Microsoft.AspNetCore.Authorization;

namespace SchoolV01.Api.Controllers
{

    public class BlockCategoriesController : ApiControllerBase
    {
        private readonly IBlockCategoryService blockCategoryService;

        public BlockCategoriesController(IBlockCategoryService blockCategoryService)
        {
            this.blockCategoryService = blockCategoryService;
        }

        [HttpGet]
        [Route("all")]
        public async Task<ActionResult> Get()
        {
            try
            {
                var data = await blockCategoryService.GetBlockCategories();

                if(data != null)
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
        public async Task<ActionResult> Get(int pageNumber, int pageSize, string searchString, string orderBy)
        {
            try
            {
                var filteredData = await blockCategoryService.GetPagedBlockCategories(searchString, orderBy);

                var pagedData = filteredData
                .Skip((pageNumber) * pageSize)
               .Take(pageSize)
               .ToList();

                var response = new PagedResponse<BlockCategoryViewModel>(pagedData, pageNumber, pageSize, filteredData.Count());
                return Ok(response);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data");
            }
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<BlockCategoryViewModel>> Get(int id)
        {
            try
            {
                var result = await blockCategoryService.GetBlockCategoryByID(id);

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
        public async Task<ActionResult<BlockCategoryViewModel>> Create(BlockCategoryInsertModel blockCategoryInsertModel)
        {
            try
            {
                if (blockCategoryInsertModel == null)
                    return BadRequest();
                // TODO : implement restriction to prevent adding an existing category model 
                var createdBlockCategory = await blockCategoryService.AddBlockCategory(blockCategoryInsertModel);

                if (createdBlockCategory != null)
                {
                    return createdBlockCategory;
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
        public async Task<ActionResult<BlockCategoryViewModel>> Update(int id, BlockCategoryUpdateModel blockCategoryUpdateModel)
        {
            try
            {
                if (blockCategoryUpdateModel.Id != id)
                {
                    return NotFound("IDs are not matching");
                }
                var blockCategoryToUpdate = await blockCategoryService.GetBlockCategoryByID(id);

                if (blockCategoryToUpdate == null)
                    return NotFound($"Record with Id = {blockCategoryUpdateModel.Id} not found");

                var updatedBlockCategory = await blockCategoryService.UpdateBlockCategory(blockCategoryUpdateModel);

                if (updatedBlockCategory != null)
                {
                    return Ok(updatedBlockCategory);
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
        public async Task<ActionResult<BlockCategoryViewModel>> Delete(int id)
        {
            try
            {
                var blockCategoryToDelete = await blockCategoryService.GetBlockCategoryByID(id);

                if (blockCategoryToDelete == null)
                {
                    return NotFound($"Record with Id = {id} not found");
                }

                var result = await blockCategoryService.SoftDeleteBlockCategory(id);
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
