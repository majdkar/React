using System;
namespace SchoolV01.Application.Requests.OwnersManagement
{
    public class GetAllPagedOwnersRequest : PagedRequest
    {
        public string SearchString { get; set; }
    }
}