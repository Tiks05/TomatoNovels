using System.Threading.Tasks;
using TomatoNovels.Shared.DTOs.Library;

namespace TomatoNovels.Services
{
    public interface ILibraryService
    {
        /// <summary>
        /// 获取筛选后的书籍结果
        /// </summary>
        Task<BookListResultDto> GetFilteredBooksAsync(BookListQueryDto query);
    }
}
