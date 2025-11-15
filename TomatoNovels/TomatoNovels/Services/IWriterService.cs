using System.Collections.Generic;
using System.Threading.Tasks;
using TomatoNovels.Shared.DTOs.Writer;

namespace TomatoNovels.Services
{
    /// <summary>
    /// 作家专区相关服务接口
    /// 对应原 Flask 的 writer_service.py
    /// </summary>
    public interface IWriterService
    {
        /// <summary>
        /// 根据类型获取公告 / 活动列表
        /// news_type: picnotice / notice / active
        /// 返回值采用 object，是因为不同类型对应不同的 DTO 列表：
        /// - picnotice -> List&lt;PicNoticeDto&gt;
        /// - notice    -> List&lt;NoticeDto&gt;
        /// - active    -> List&lt;ActiveDto&gt;
        /// </summary>
        Task<object> GetNewsListByTypeAsync(string newsType, int limit = 5);

        /// <summary>
        /// 根据分类获取创作课堂列表
        /// categoryType 为空则返回所有类型
        /// </summary>
        Task<List<ClassroomOutDto>> GetClassroomByCategoryAsync(string categoryType);
    }
}
