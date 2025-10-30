using SchoolV01.Application.Specifications.Base;
using SchoolV01.Domain.Entities.Misc;

namespace SchoolV01.Application.Specifications.Misc
{
    public class DocumentFilterSpecification : HeroSpecification<Document>
    {
        public DocumentFilterSpecification(string searchString, string userId)
        {
            if (!string.IsNullOrEmpty(searchString))
            {
                Criteria = p => p.Title.Contains(searchString) || p.Description.Contains(searchString) || p.User.Contains(searchString);
            }
            else
            {
                Criteria = p => true;
            }
        }
    }
}