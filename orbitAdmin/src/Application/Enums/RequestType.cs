namespace SchoolV01.Application.Enums
{
    public enum RequestType : byte
    {
        
        Lost = 1,
        Ruined = 2,
        Renew = 3
    }
    public enum RequestState : byte
    {
        InProcess = 0,
        Approved = 1,
        Rejected = 2,
    }
}