namespace Noter.DAL.Helpers
{
    public class CommentariesResourceParameters
    {
        const int maxPageSize = 15;
        public int PageNumber { get; set; } = 1;
        private int pageSize = 5;
        public int PageSize
        {
            get
            {
                return pageSize;
            }

            set
            {
                pageSize = (value > maxPageSize) ? maxPageSize : value;
            }
        }
    }
}
