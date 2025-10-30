using System;
using SchoolV01.Application.Specifications.Base;
using SchoolV01.Domain.Entities.OwnersManagement;

namespace SchoolV01.Application.Specifications.OwnersManagement
{
    public class PassportFilterSpecification : HeroSpecification<Passport>
    {
        public PassportFilterSpecification(string searchString)
        {
            if (!string.IsNullOrEmpty(searchString))
            {
                Criteria = p => true;
            }
            else
            {
                Criteria = p => true;
            }
        }
    }
}
