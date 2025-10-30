using SchoolV01.Application.Services;
using SchoolV01.Shared.ViewModels.Events;
using SchoolV01.Shared.Wrapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolV01.Api.Controllers
{
    public class EventCategoriesController : ApiControllerBase
    {
        private readonly IEventCategoryService eventCategoryService;

        public EventCategoriesController(IEventCategoryService eventCategoryService)
        {
            this.eventCategoryService = eventCategoryService;
        }

        [HttpGet]
        [Route("all")]
        public async Task<ActionResult> Get()
        {
            try
            {
                var data = await eventCategoryService.GetEventCategories();

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
        public async Task<ActionResult> Get(int pageNumber, int pageSize, string searchString, string orderBy)
        {
            try
            {
                var filteredData = await eventCategoryService.GetPagedEventCategories(searchString, orderBy);
                if (pageSize == 0) pageSize = 10;
                var pagedData = filteredData
                .Skip((pageNumber) * pageSize)
               .Take(pageSize)
               .ToList();

                var response = new PagedResponse<EventCategoryViewModel>(pagedData, pageNumber, pageSize, filteredData.Count());
                return Ok(response);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data");
            }
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<EventCategoryViewModel>> Get(int id)
        {
            try
            {
                var result = await eventCategoryService.GetEventCategoryByID(id);

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
        public async Task<ActionResult<EventCategoryViewModel>> Create(EventCategoryInsertModel eventCategoryInsertModel)
        {
            try
            {
                if (eventCategoryInsertModel == null)
                    return BadRequest();
                // TODO : implement restriction to prevent adding an existing category model 
                var createdEventCategory = await eventCategoryService.AddEventCategory(eventCategoryInsertModel);

                if (createdEventCategory != null)
                {
                    return CreatedAtAction(nameof(Get),
                      new { id = createdEventCategory.Id }, createdEventCategory);
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
        public async Task<ActionResult<EventCategoryViewModel>> Update(int id, EventCategoryUpdateModel eventCategoryUpdateModel)
        {
            try
            {
                if (eventCategoryUpdateModel.Id != id)
                {
                    return NotFound("IDs are not matching");
                }
                var eventCategoryToUpdate = await eventCategoryService.GetEventCategoryByID(id);

                if (eventCategoryToUpdate == null)
                    return NotFound($"Record with Id = {eventCategoryUpdateModel.Id} not found");

                var updatedEventCategory = await eventCategoryService.UpdateEventCategory(eventCategoryUpdateModel);

                if (updatedEventCategory != null)
                {
                    return Ok(updatedEventCategory);
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

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<EventCategoryViewModel>> Delete(int id)
        {
            try
            {
                var eventCategoryToDelete = await eventCategoryService.GetEventCategoryByID(id);

                if (eventCategoryToDelete == null)
                {
                    return NotFound($"Record with Id = {id} not found");
                }

                var result = await eventCategoryService.SoftDeleteEventCategory(id);
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
