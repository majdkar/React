using SchoolV01.Shared.ViewModels.Blocks;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SchoolV01.Application.Services
{
    public interface IBlockCategoryService : IDisposable
    {
        Task<List<BlockCategoryViewModel>> GetBlockCategories();

        Task<List<BlockCategoryViewModel>> GetPagedBlockCategories(string searchString, string orderBy);

        Task<BlockCategoryViewModel> GetBlockCategoryByID(int blockCategoryId);

        Task<BlockCategoryViewModel> AddBlockCategory(BlockCategoryInsertModel blockCategoryInsertModel);

        Task<BlockCategoryViewModel> UpdateBlockCategory(BlockCategoryUpdateModel blockCategoryUpdateModel);

        Task<bool> SoftDeleteBlockCategory(int blockCategoryId);

        Task SaveAsync();
    }
}
