using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;


namespace SchoolV01.Shared.Wrapper
{
    public class PagedResponse<T>
    {
        public List<T> Items { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public Uri FirstPage { get; set; }
        public Uri LastPage { get; set; }
        public int TotalPages { get; set; }
        public int TotalRecords { get; set; }
        public Uri NextPage { get; set; }
        public Uri PreviousPage { get; set; }
        [JsonConstructor]
        public PagedResponse()
        {
        }
        public PagedResponse(List<T> data, int pageNumber, int pageSize, int totalRecords)
        {
            this.PageNumber = pageNumber;
            this.PageSize = pageSize;
            this.Items = data;
            this.TotalRecords = totalRecords;

        }
    }
}
