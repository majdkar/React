using System;

namespace SchoolV01.Shared.ViewModels.Articles
{
    public class ArticleViewModel
    {
        public int Id { set; get; }

        public string Title { set; get; }

        public DateTime CreateDate { get; set; }

        public bool IsArchived { get; set; }

        public bool IsActive { get; set; }

        public string Description { set; get; }

        public string Language { get; set; }

        public int CategoryId { get; set; }
        public virtual ArticleCategoryViewModel ArticleCategory { get; set; }

        public bool ShowTranslation { get; set; } = false;

        public string Image { get; set; }

        public string File { get; set; }

        public int RecordOrder { get; set; }

        public string Slug { get; set; }
    }
}
