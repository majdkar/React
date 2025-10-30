namespace SchoolV01.Shared.ViewModels.Settings
{
    public class UnitViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int? BaseUnitId { get; set; }
        public virtual UnitViewModel BaseUnit { get; set; }

        public decimal? BaseUnitRatio { get; set; }

        public bool IsActive { get; set; }
    }
}
