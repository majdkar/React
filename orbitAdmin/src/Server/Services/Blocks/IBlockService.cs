using SchoolV01.Shared.ViewModels.Blocks;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SchoolV01.Application.Services
{
    public interface IBlockService
    {
        Task<List<BlockViewModel>> GetBlocks();
        Task<List<BlockEndpointViewModel>> GetEndpointBlocks(int categoryId);

        Task<List<BlockViewModel>> GetPagedBlocks(string searchString, string orderBy);

        Task<List<BlockViewModel>> GetBlocksByCategoryId(int categoryId);

        Task<BlockViewModel> GetBlockById(int blockId);
        Task<BlockViewModel> GetBlockByEndpoint(string Endpoint);

        Task<BlockViewModel> AddBlock(BlockInsertModel blockInsertModel);

        Task<BlockViewModel> UpdateBlock(BlockUpdateModel blockUpdateModel);

        Task<bool> SoftDeleteBlock(int blockId);

        Task SaveAsync();
    }
}
