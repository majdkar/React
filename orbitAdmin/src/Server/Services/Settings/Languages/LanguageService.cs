using AutoMapper;

using SchoolV01.Shared.ViewModels.Settings;
using Microsoft.EntityFrameworkCore;
using SchoolV01.Application.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SchoolV01.Domain.Entities.GeneralSettings;

namespace SchoolV01.Application.Services
{
    public class LanguageService : ILanguageService
    {
        private readonly IUnitOfWork<int> uow;
        private readonly IMapper mapper;

        public LanguageService(IUnitOfWork<int> uow, IMapper mapper)
        {
            this.uow = uow;
            this.mapper = mapper;
        }

        public async Task<List<LanguageViewModel>> GetLanguages()
        {
            var languagesEntities = await uow.Query<Language>().ToListAsync();
            var languagesVM = mapper.Map<List<Language>, List<LanguageViewModel>>(languagesEntities);
            return languagesVM;
        }

        public async Task<LanguageViewModel> GetLanguageById(int languageId)
        {
            var languageEntity = await uow.Query<Language>().Where(x => x.Id == languageId).FirstOrDefaultAsync();
            var languageVM = mapper.Map<Language, LanguageViewModel>(languageEntity);
            return languageVM;
        }

        public async Task<LanguageViewModel> AddLanguage(LanguageInsertModel languageInsertModel)
        {
            try
            {
                var languageEntity = mapper.Map<LanguageInsertModel, Language>(languageInsertModel);
                var result = uow.Add(languageEntity);
                await SaveAsync();
                if (result != null)
                {
                    var resultVM = mapper.Map<Language, LanguageViewModel>(result);
                    return resultVM;
                }
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<LanguageViewModel> UpdateLanguage(LanguageUpdateModel languageUpdateModel)
        {
            try
            {
                var languageEntity = uow.Query<Language>().Where(x => x.Id == languageUpdateModel.Id).FirstOrDefault();
                if (languageEntity != null)
                {
                    languageEntity.Name = languageUpdateModel.Name;
                    languageEntity.LanguageCode = languageUpdateModel.LanguageCode;
                    languageEntity.IsActive = languageUpdateModel.IsActive;

                    uow.Update(languageEntity);
                    await SaveAsync();
                    var resultVM = mapper.Map<Language, LanguageViewModel>(languageEntity);
                    return resultVM;
                }
                else
                    return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<bool> SoftDeleteLanguage(int languageId)
        {
            try
            {
                var languageEntity = uow.Query<Language>().Where(x => x.Id == languageId).FirstOrDefault();
                if (languageEntity != null)
                {
                    languageEntity.IsActive = !languageEntity.IsActive;
                    uow.Update(languageEntity);
                    await SaveAsync();
                    return true;
                }
                else
                    return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task SaveAsync()
        {
            await uow.CommitAsync();
        }

        public void Dispose()
        {
            uow.Dispose();
        }
    }
}
