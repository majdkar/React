namespace SchoolV01.Shared.ViewModels.Articles
{
    public class ArticleCategoryViewModel
    {
        public int Id { set; get; }

        public string Name { set; get; }

        public string Description { set; get; }

        public string CategoryType { get; set; }

        public bool IsActive { get; set; } = true;

    }
}
