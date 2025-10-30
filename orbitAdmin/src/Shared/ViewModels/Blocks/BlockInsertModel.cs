using FluentValidation;
using System;

namespace SchoolV01.Shared.ViewModels.Blocks
{
    public class BlockInsertModel
    {
        public string NameAr { set; get; }

        public string NameEn { set; get; }
        public string NameGe { set; get; }

        public string DescriptionAr { set; get; }
        public string DescriptionEn { set; get; }
        public string DescriptionGe { set; get; }

        public string DescriptionAr1 { set; get; }
        public string DescriptionEn1 { set; get; }
        public string DescriptionGe1 { set; get; }

        public string DescriptionAr2 { set; get; }
        public string DescriptionEn2 { set; get; }
        public string DescriptionGe2 { set; get; }

        public string DescriptionAr3 { set; get; }
        public string DescriptionEn3 { set; get; }
        public string DescriptionGe3 { set; get; }

        public string Image1 { get; set; }

        public string Image2 { get; set; }

        public string Image3 { get; set; }
        public string EndpointAr { get; set; }
        public string EndpointEn { get; set; }
        public string EndpointGe { get; set; }

        public DateTime? Date { set; get; }
        public DateTime? StartDate { set; get; }
        public DateTime? EndDate { set; get; }

        public string Location { set; get; }
        public int CategoryId { get; set; }

        public bool IsVisible { get; set; }

        public string Image { get; set; }

        public string Url { get; set; }
        public string Url1 { get; set; }

        public string File { get; set; }

        public bool IsActive { get; set; }
        public int AuthorId { get; set; }
        public DateTime? CreateAt { set; get; }
        public int RecordOrder { get; set; }
        public int? ParentId { get; set; }
        public string Keywords { get; set; }
        public string SeoDescription { get; set; }
    }

    public class BlockInsertValidator : AbstractValidator<BlockInsertModel>
    {
        public BlockInsertValidator()
        {
            RuleFor(p => p.NameAr).NotEmpty().WithMessage("You must enter Name");
            RuleFor(p => p.EndpointEn).NotEmpty().WithMessage("You must enter Endpoint En");
            //RuleFor(p => p.Description).NotEmpty().WithMessage("You must enter description");
            RuleFor(p => p.CategoryId).NotEmpty().WithMessage("You must choose category");
            RuleFor(p => p.RecordOrder).GreaterThanOrEqualTo(0).WithMessage("Value Must be Greater or Equal to Zero");
        }
    }
}
