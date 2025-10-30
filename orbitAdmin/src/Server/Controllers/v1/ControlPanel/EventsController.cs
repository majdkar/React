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
    public class EventsController : ApiControllerBase
    {
        private readonly IEventService eventService;

        public EventsController(IEventService eventService)
        {
            this.eventService = eventService;
        }

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

                var filteredData = await eventService.GetPagedEvents(searchString, orderBy);

                if (convertedCategoryId != 0)
                {
                    filteredData = filteredData.Where(x => x.CategoryId == convertedCategoryId).ToList();
                }
                if (pageSize == 0) pageSize = 10;
                var pagedData = filteredData
                .Skip((pageNumber) * pageSize)
               .Take(pageSize)
               .ToList();

                var response = new PagedResponse<EventViewModel>(pagedData, pageNumber, pageSize, filteredData.Count());
                return Ok(response);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data");
            }
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<EventViewModel>> Get(int id)
        {
            try
            {
                var result = await eventService.GetEventById(id);

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
        public async Task<ActionResult<EventViewModel>> Create(EventInsertModel eventInsertModel)
        {
            try
            {
                if (eventInsertModel == null)
                    return BadRequest();

                // TODO : implement restriction to prevent adding an existing  model 
                var createdEvent = await eventService.AddEvent(eventInsertModel);

                if (createdEvent != null)
                {
                    return CreatedAtAction(nameof(Get),
                      new { id = createdEvent.Id }, createdEvent);
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
        public async Task<ActionResult<EventViewModel>> Update(int id, EventUpdateModel eventUpdateModel)
        {
            try
            {
                if (eventUpdateModel.Id != id)
                {
                    return BadRequest("IDs  are not matching");
                }
                var eventToUpdate = await eventService.GetEventById(id);

                if (eventToUpdate == null)
                    return NotFound($"Record with Id = {id} is not found");

                var updatedEvent = await eventService.UpdateEvent(eventUpdateModel);

                if (updatedEvent != null)
                {
                    return Ok(updatedEvent);
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
        public async Task<ActionResult<EventViewModel>> Delete(int id)
        {
            try
            {
                var eventToDelete = await eventService.GetEventById(id);

                if (eventToDelete == null)
                {
                    return NotFound($"Record with Id = {id} not found");
                }

                var result = await eventService.SoftDeleteEvent(id);
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
