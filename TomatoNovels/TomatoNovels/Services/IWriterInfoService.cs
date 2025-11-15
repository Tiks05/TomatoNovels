using System.Collections.Generic;
using System.Threading.Tasks;
using TomatoNovels.Shared.DTOs.WriterInfo;

namespace TomatoNovels.Services
{
    /// <summary>
    /// 作者详情页相关服务接口
    /// 对应 Flask 的 writerinfo_service.py
    /// </summary>
    public interface IWriterInfoService
    {
        /// <summary>
        /// 获取作者头部信息（头像、昵称、签名、总字数、粉丝数等）
        /// </summary>
        Task<WriterDto> GetWriterHeaderAsync(int writerId);

        /// <summary>
        /// 获取作者全部作品信息列表
        /// </summary>
        Task<List<WorkDto>> GetWriterWorksAsync(int writerId);
    }
}
