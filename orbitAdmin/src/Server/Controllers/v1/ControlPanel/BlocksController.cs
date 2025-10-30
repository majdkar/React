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
using SchoolV01.Shared.ViewModels.Menus;
using System.Collections.Generic;

namespace SchoolV01.Api.Controllers
{
    public class BlocksController : ApiControllerBase
    {
        private readonly IBlockService blockService;

        public BlocksController(IBlockService blockService)
        {
            this.blockService = blockService;
        }
        [AllowAnonymous]

        [HttpGet]
        public async Task<ActionResult> Get(string categoryId, int pageNumber, int pageSize, string searchString, string orderBy)
        {
            try
            {
                int convertedCategoryId;
                var isConvertable = Int32.TryParse(categoryId, out convertedCategoryId);

                if (!isConvertable)
                {
                    return BadRequest($"try correct category Id!");
                }

                var filteredData = await blockService.GetPagedBlocks(searchString, orderBy);

                if (convertedCategoryId != 0)
                {
                    filteredData = filteredData.Where(x => x.CategoryId == convertedCategoryId).ToList();
                }
                if (pageSize == 0) pageSize = 10;
                var pagedData = filteredData
                    .Skip((pageNumber) * pageSize)
                    .Take(pageSize)
                    .ToList();

                var response = new PagedResponse<BlockViewModel>(pagedData, pageNumber, pageSize, filteredData.Count());
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
        public async Task<IActionResult> GetMaster(string categoryId, int? blockId, string searchString, string orderBy)
        {
            try
            {
                int convertedCategoryId;
                var isConvertable = Int32.TryParse(categoryId, out convertedCategoryId);

                if (!isConvertable)
                {
                    return BadRequest($"try correct category Id!");
                }

                var filteredData = await blockService.GetPagedBlocks(searchString, orderBy);

                if (convertedCategoryId != 0)
                {
                    filteredData = filteredData.Where(x => x.CategoryId == convertedCategoryId && x.ParentId == blockId).OrderBy(x => x.RecordOrder).ToList();
                }

                var response = new PagedResponse<BlockViewModel>(filteredData, 0, 10, filteredData.Count());
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
        public async Task<IActionResult> Get(string searchString, string orderBy)
        {
            try
            {
                var filteredData = await blockService.GetPagedBlocks(searchString, orderBy);
                var response = new PagedResponse<BlockViewModel>(filteredData, 0, 10, filteredData.Count());
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
        [Route("allEndpoint")]
        public async Task<ActionResult<List<BlockEndpointViewModel>>> GetAllEndpoint(int categoryId)
        {
            try
            {
                var result = await blockService.GetEndpointBlocks(categoryId);

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

        [HttpGet("{id:int}")]
        public async Task<ActionResult<BlockViewModel>> Get(int id)
        {
            try
            {
                var result = await blockService.GetBlockById(id);

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

        [HttpGet("BlockByEndpoint/{Endpoint}")]
        public async Task<ActionResult<BlockViewModel>> GetBlockByEndpoint(string Endpoint)
        {
            try
            {
                var result = await blockService.GetBlockByEndpoint(Endpoint);

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
        public async Task<ActionResult<BlockViewModel>> Create(BlockInsertModel blockInsertModel)
        {
            try
            {
                if (blockInsertModel == null)
                    return BadRequest();

                // TODO : implement restriction to prevent adding an existing  model 
                var createdBlock = await blockService.AddBlock(blockInsertModel);

                if (createdBlock != null)
                {
                    return CreatedAtAction(nameof(Get),
                      new { id = createdBlock.Id }, createdBlock);
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
        public async Task<ActionResult<BlockViewModel>> Update(int id, BlockUpdateModel blockUpdateModel)
        {
            try
            {
                if (blockUpdateModel.Id != id)
                {
                    return BadRequest("IDs  are not matching");
                }
                var blockToUpdate = await blockService.GetBlockById(id);

                if (blockToUpdate == null)
                    return NotFound($"Record with Id = {id} is not found");

                var updatedBlock = await blockService.UpdateBlock(blockUpdateModel);

                if (updatedBlock != null)
                {
                    return Ok(updatedBlock);
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
        public async Task<ActionResult<BlockViewModel>> Delete(int id)
        {
            try
            {
                var blockToDelete = await blockService.GetBlockById(id);

                if (blockToDelete == null)
                {
                    return NotFound($"Record with Id = {id} not found");
                }

                var result = await blockService.SoftDeleteBlock(id);
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
