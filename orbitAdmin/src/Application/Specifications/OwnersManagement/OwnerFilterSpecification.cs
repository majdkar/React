using System;
using SchoolV01.Application.Specifications.Base;
using SchoolV01.Domain.Entities.OwnersManagement;

namespace SchoolV01.Application.Specifications.OwnersManagement
{
    public class OwnerFilterSpecification : HeroSpecification<Owner>
    {
        public OwnerFilterSpecification(string searchString)
        {

            Includes.Add(a => a.Passport);
            //Includes.Add(a => a.Brand);
            if (!string.IsNullOrEmpty(searchString))
            {
                Criteria = p =>  p.Name.Contains(searchString) || p.Description.Contains(searchString);
            }
            else
            {
                Criteria = p => true;
            }
        }
    }
}