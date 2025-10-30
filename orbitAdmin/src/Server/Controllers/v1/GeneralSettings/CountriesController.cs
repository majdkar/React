using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using SchoolV01.Application.Features.Countries.Commands;
using SchoolV01.Application.Features.Countries.Queries;
using SchoolV01.Shared.Constants.Permission;

namespace SchoolV01.Server.Controllers.v1.GeneralSettings
{
    public class CountriesController : BaseApiController<CountriesController>
    {
        /// <summary>
        /// Get All Countries
        /// </summary>
        /// <returns>Status 200 OK</returns>
        [AllowAnonymous]
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var Countries = await Mediator.Send(new GetAllCountriesQuery());
            return Ok(Countries);
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
            var City = await Mediator.Send(new GetCountryByIdQuery() { Id = id });
            return Ok(City);
        }

        /// <summary>
        /// Create/Update a City
        /// </summary>
        /// <param name="command"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Countries.Create)]
        [HttpPost]
        public async Task<IActionResult> Post(AddEditCountryCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        /// <summary>
        /// Delete a City
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Countries.Delete)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await Mediator.Send(new DeleteCountryCommand { Id = id }));
        }

        /// <summary>
        /// Search Countries and Export to Excel
        /// </summary>
        /// <param name="searchString"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("export")]
        public async Task<IActionResult> Export(string searchString = "")
        {
            return Ok(await Mediator.Send(new ExportCountriesQuery(searchString)));
        }

    }
}