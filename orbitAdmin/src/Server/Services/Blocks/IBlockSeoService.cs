using SchoolV01.Shared.ViewModels.Blocks;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SchoolV01.Application.Services
{
    public interface IBlockSeoService
    {
        Task<List<BlockSeoViewModel>> GetSeoByBlockId(int blockId);
        Task<BlockSeoViewModel> GetSeoViewByBlockId(int blockId);

        Task<BlockSeoViewModel> GetSeoById(int translationId);

        Task<BlockSeoViewModel> AddSeo(BlockSeoInsertModel translationInsertModel);

        Task<BlockSeoViewModel> UpdateSeo(BlockSeoUpdateModel translationUpdateModel);

        Task<bool> SoftDeleteSeo(int translationId);

        Task SaveAsync();
    }
}
