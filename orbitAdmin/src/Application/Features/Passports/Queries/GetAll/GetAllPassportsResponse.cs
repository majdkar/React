using System;
namespace SchoolV01.Application.Features.Passports.Queries
{
    public class GetAllPassportsResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
		
        //public decimal Tax { get; set; }
    }
}