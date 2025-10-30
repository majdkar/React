using SchoolV01.Application.Features.Owners.Commands;
using SchoolV01.Application.Features.Owners.Queries;
using SchoolV01.Application.Features.Owners.Queries.GetOwnerImage;
using SchoolV01.Shared.Constants.Permission;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace SchoolV01.Server.Controllers.v1.OwnersManagement
{
    public class OwnersController : BaseApiController<OwnersController>
    {
        /// <summary>
        /// Get All Owners
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchString"></param>
        /// <param name="orderBy"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Owners.View)]
        [HttpGet]
        public async Task<IActionResult> GetAll(int pageNumber, int pageSize, string searchString, string orderBy = null)
        {
            var owners = await Mediator.Send(new GetAllOwnersQuery(pageNumber, pageSize, searchString, orderBy));
            return Ok(owners);
        }

        /// <summary>
        /// Get a Owner Image by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Owners.View)]
        [HttpGet("image/{id}")]
        public async Task<IActionResult> GetOwnerImageAsync(int id)
        {
            var result = await Mediator.Send(new GetOwnerImageQuery(id));
            return Ok(result);
        }

        /// <summary>
        /// Add/Edit a Owner
        /// </summary>
        /// <param name="command"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Owners.Create)]
        [HttpPost]
        public async Task<IActionResult> Post(AddEditOwnerCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        /// <summary>
        /// Delete a Owner
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 OK response</returns>
        [Authorize(Policy = Permissions.Owners.Delete)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await Mediator.Send(new DeleteOwnerCommand { Id = id }));
        }

        /// <summary>
        /// Search Owners and Export to Excel
        /// </summary>
        /// <param name="searchString"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Owners.View)]
        [HttpGet("export")]
        public async Task<IActionResult> Export(string searchString = "")
        {
            return Ok(await Mediator.Send(new ExportOwnersQuery(searchString)));
        }
    }
}