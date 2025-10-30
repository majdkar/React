using SchoolV01.Application.Features.Passports.Queries;
using SchoolV01.Shared.Constants.Permission;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using SchoolV01.Application.Features.Passports.Commands;

namespace SchoolV01.Server.Controllers.v1.OwnersManagement
{
    public class PassportsController : BaseApiController<PassportsController>
    {
        /// <summary>
        /// Get All Passports
        /// </summary>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Passports.View)]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var passports = await Mediator.Send(new GetAllPassportsQuery());
            return Ok(passports);
        }

        /// <summary>
        /// Get a Passport By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 Ok</returns>
        [Authorize(Policy = Permissions.Passports.View)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var passport = await Mediator.Send(new GetPassportByIdQuery() { Id = id });
            return Ok(passport);
        }

        /// <summary>
        /// Create/Update a Passport
        /// </summary>
        /// <param name="command"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Passports.Create)]
        [HttpPost]
        public async Task<IActionResult> Post(AddEditPassportCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        /// <summary>
        /// Delete a Passport
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Passports.Delete)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await Mediator.Send(new DeletePassportCommand { Id = id }));
        }

        /// <summary>
        /// Search Passports and Export to Excel
        /// </summary>
        /// <param name="searchString"></param>
        /// <returns></returns>
        [Authorize(Policy = Permissions.Passports.View)]
        [HttpGet("export")]
        public async Task<IActionResult> Export(string searchString = "")
        {
            return Ok(await Mediator.Send(new ExportPassportsQuery(searchString)));
        }
    }
}