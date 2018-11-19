﻿namespace Noter.DAL.Helpers
{
    public class TopicsResourceParameters
    {
        const int maxPageSize = 32;
        public int PageNumber { get; set; } = 1;
        private int pageSize = 8;
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

        public string SearchTitle { get; set; }
    }
}
