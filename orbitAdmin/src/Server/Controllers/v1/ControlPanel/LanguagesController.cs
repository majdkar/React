using SchoolV01.Application.Services;
using SchoolV01.Shared.ViewModels.Settings;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace SchoolV01.Api.Controllers
{
    public class LanguagesController : ApiControllerBase
    {
        private readonly ILanguageService languageService;

        public LanguagesController(ILanguageService languageService)
        {
            this.languageService = languageService;
        }

        [HttpGet]
        public async Task<ActionResult> Get()
        {
            try
            {
                var data = await languageService.GetLanguages();

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


        [HttpGet("{id:int}")]
        public async Task<ActionResult<LanguageViewModel>> Get(int id)
        {
            try
            {
                var result = await languageService.GetLanguageById(id);

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
        public async Task<ActionResult<LanguageViewModel>> Create(LanguageInsertModel languageInsertModel)
        {
            try
            {
                if (languageInsertModel == null)
                    return BadRequest();

                var createdLanguage = await languageService.AddLanguage(languageInsertModel);

                if (createdLanguage != null)
                {
                    return CreatedAtAction(nameof(Get),
                      new { id = createdLanguage.Id }, createdLanguage);
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
        public async Task<ActionResult<LanguageViewModel>> Update(int id, LanguageUpdateModel languageUpdateModel)
        {
            try
            {
                if (languageUpdateModel.Id != id)
                {
                    return NotFound($"IDs are not matching");
                }
                var languageToUpdate = await languageService.GetLanguageById(id);

                if (languageToUpdate == null)
                    return NotFound($"Record with Id = {languageUpdateModel.Id} not found");

                var updatedLanguage = await languageService.UpdateLanguage(languageUpdateModel);

                if (updatedLanguage != null)
                {
                    return Ok(updatedLanguage);
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
        public async Task<ActionResult<LanguageViewModel>> Delete(int id)
        {
            try
            {
                var languageToDelete = await languageService.GetLanguageById(id);

                if (languageToDelete == null)
                {
                    return NotFound($"Record with Id = {id} not found");
                }

                var result = await languageService.SoftDeleteLanguage(id);
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
