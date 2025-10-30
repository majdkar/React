using FluentValidation;
using System;

namespace SchoolV01.Shared.ViewModels.Pages
{
    public class PageSeoInsertModel
    {
        
        public int Id { get; set; }
        public int PageId { get; set; }

        public string MetaTitleAr { get; set; }
        public string MetaTitleEn { get; set; }
        public string MetaTitleGe { get; set; }


        public string MetaNameAr { get; set; }
        public string MetaNameEn { get; set; }
        public string MetaNameGe { get; set; }

        public string MetaUrlAr { get; set; }
        public string MetaUrlEn { get; set; }
        public string MetaUrlGe { get; set; }


        public string MetaKeywordsAr { get; set; }
        public string MetaKeywordsEn { get; set; }
        public string MetaKeywordsGe { get; set; }

        public string MetaDescriptionsAr { get; set; }
        public string MetaDescriptionsEn { get; set; }
        public string MetaDescriptionsGe { get; set; }


        public string ImageAlt1Ar { get; set; }
        public string ImageAlt1En { get; set; }
        public string ImageAlt1Ge { get; set; }

        public string ImageAlt2Ar { get; set; }
        public string ImageAlt2En { get; set; }
        public string ImageAlt2Ge { get; set; }

        public string ImageAlt3Ar { get; set; }
        public string ImageAlt3En { get; set; }
        public string ImageAlt3Ge { get; set; }

        public string ImageAlt4Ar { get; set; }
        public string ImageAlt4En { get; set; }
        public string ImageAlt4Ge { get; set; }


        public string MetaRobots { get; set; }

    }

    public class PageSeoInsertValidator : AbstractValidator<PageSeoInsertModel>
    {
        public PageSeoInsertValidator()
        {
            //RuleFor(p => p.NameAr).NotEmpty().WithMessage("You must enter Name");
            ////RuleFor(p => p.Description).NotEmpty().WithMessage("You must enter description");
            //RuleFor(p => p.CategoryId).NotEmpty().WithMessage("You must choose category");
            //RuleFor(p => p.RecordOrder).GreaterThanOrEqualTo(0).WithMessage("Value Must be Greater or Equal to Zero");
        }
    }
}
