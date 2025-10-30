namespace SchoolV01.Application.Requests
{
    public abstract class PagedRequest
    {
        public int PageSize { get; set; } = int.MaxValue;
        public int PageNumber { get; set; } = 1;

        public string[] Orderby { get; set; } = []; // of the form fieldname [ascending|descending],fieldname [ascending|descending]...
    }
}