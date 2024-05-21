namespace WebApi.Models
{
    public class PagingResult<T>
    {
        /// <summary>
        /// 当前页码
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        /// 每页数量
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// 总数
        /// </summary>
        public long Total { get; set; }

        /// <summary>
        /// 数据记录
        /// </summary>
        public IEnumerable<T> Records { get; set; }
    }
}
