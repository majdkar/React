using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SchoolV01.Api.Controllers;
using SchoolV01.Server.Services.Blocks;
using SchoolV01.Shared.Constants.Permission;
using SchoolV01.Shared.ViewModels.Blocks;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SchoolV01.Server.Controllers.v1.ControlPanel
{
    public class BlockVideoController : ApiControllerBase
    {
        private readonly IBlockVideoService VideoService;

        public BlockVideoController(IBlockVideoService VideoService)
        {
            this.VideoService = VideoService;
        }


        [HttpGet("{id:int}")]
        public async Task<ActionResult<BlockVideoViewModel>> Get(int id)
        {
            try
            {
                var result = await VideoService.GetVideoById(id);

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





        [HttpGet("GetVideoByBlockId")]
        public async Task<ActionResult<List<BlockVideoViewModel>>> GetVideoByBlock(int id)
        {
            try
            {
                var result = await VideoService.GetVideoByBlockId(id);

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
        public async Task<ActionResult<BlockVideoViewModel>> Create(BlockVideoInsertModel VideoInsertModel)
        {
            try
            {
                if (VideoInsertModel == null)
                    return BadRequest();

                var createdVideo = await VideoService.AddVideo(VideoInsertModel);

                if (createdVideo != null)
                {
                    return CreatedAtAction(nameof(Get),
                      new { id = createdVideo.Id }, createdVideo);
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
        public async Task<ActionResult<BlockVideoViewModel>> Update(int id, BlockVideoUpdateModel VideoUpdateModel)
        {
            try
            {
                if (VideoUpdateModel.Id != id)
                {
                    return NotFound("IDs are not matching");
                }
                var transaltionToUpdate = await VideoService.GetVideoById(id);

                if (transaltionToUpdate == null)
                    return NotFound($"Record with Id = {VideoUpdateModel.Id} not found");

                var updatedTransaltion = await VideoService.UpdateVideo(VideoUpdateModel);

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
        public async Task<ActionResult<BlockVideoViewModel>> Delete(int id)
        {
            try
            {
                var transaltionToDelete = await VideoService.GetVideoById(id);

                if (transaltionToDelete == null)
                {
                    return NotFound($"Transaltion with Id = {id} not found");
                }

                var result = await VideoService.SoftDeleteVideo(id);
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
