
namespace SchoolV01.Application.Requests
{
    public class UploadRequests
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public UploadRequest UploadRequest { get; set; }
    }
}