using System.Threading.Tasks;
using TomatoNovels.Shared.DTOs.BookInfo;

namespace TomatoNovels.Services
{
    public interface IBookInfoService
    {
        Task<BookHeaderDto> GetBookHeaderAsync(int bookId);

        Task<BookContentDto> GetBookContentAsync(int bookId);

        Task<ChapterReadResponseDto> GetChapterContentAsync(int bookId, int volumeSort, int chapterNum);
    }
}
