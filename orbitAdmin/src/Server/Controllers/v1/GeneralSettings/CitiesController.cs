using SchoolV01.Application.Features.Cities.Queries;
using SchoolV01.Shared.Constants.Permission;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using SchoolV01.Application.Features.Cities.Commands;

namespace SchoolV01.Server.Controllers.v1.GeneralSettings
{
    public class CitiesController : BaseApiController<CitiesController>
    {
        /// <summary>
        /// Get All Cities
        /// </summary>
        /// <returns>Status 200 OK</returns>
        [AllowAnonymous]
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var Cities = await Mediator.Send(new GetAllCitiesQuery());
            return Ok(Cities);
        }

        /// <summary>
        /// Get a City By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 Ok</returns>
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var City = await Mediator.Send(new GetCityByIdQuery() { Id = id });
            return Ok(City);
        }

        /// <summary>
        /// Create/Update a City
        /// </summary>
        /// <param name="command"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Cities.Create)]
        [HttpPost]
        public async Task<IActionResult> Post(AddEditCityCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        /// <summary>
        /// Delete a City
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Cities.Delete)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await Mediator.Send(new DeleteCityCommand { Id = id }));
        }

        /// <summary>
        /// Search Cities and Export to Excel
        /// </summary>
        /// <param name="searchString"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("export")]
        public async Task<IActionResult> Export(string searchString = "")
        {
            return Ok(await Mediator.Send(new ExportCitiesQuery(searchString)));
        }
    }
}