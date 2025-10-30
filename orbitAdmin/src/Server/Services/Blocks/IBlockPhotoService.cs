using SchoolV01.Shared.ViewModels.Blocks;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SchoolV01.Application.Services
{
    public interface IBlockPhotoService
    {
        Task<List<BlockPhotoViewModel>> GetPhotoByBlockId(int blockId);

        Task<BlockPhotoViewModel> GetPhotoById(int translationId);

        Task<BlockPhotoViewModel> AddPhoto(BlockPhotoInsertModel translationInsertModel);

        Task<BlockPhotoViewModel> UpdatePhoto(BlockPhotoUpdateModel translationUpdateModel);

        Task<bool> SoftDeletePhoto(int translationId);

        Task SaveAsync();
    }
}
