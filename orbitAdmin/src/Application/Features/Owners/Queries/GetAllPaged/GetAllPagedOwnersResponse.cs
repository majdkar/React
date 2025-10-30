using System;
namespace SchoolV01.Application.Features.Owners.Queries
{
    public class GetAllPagedOwnersResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
		public string Description { get; set; }
		
public string Passport { get; set; }
public int PassportId { get; set; }
        //public string Barcode { get; set; }
        //public decimal Rate { get; set; }
        //public string Brand { get; set; }
        //public int BrandId { get; set; }
    }
}