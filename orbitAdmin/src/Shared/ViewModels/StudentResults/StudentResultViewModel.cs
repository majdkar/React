using System.Collections.Generic;

namespace SchoolV01.Shared.ViewModels.StudentResults
{
    public class StudentResultViewModel
    {
        public int StudentId { get; set; }

        public int StudentClassId { get; set; }

        public int GradeId { get; set; }

        public int? RuleClassId { get; set; } // Pass this value if you want to apply success rules for this class (not grade)

        public int TotalMarks { get; set; }

        public int AbsencePercentage { get; set; }

        public List<SubjectResult> SubjectResults { get; set; }
    }

    public class SubjectResult
    {
        public int SubjectId { get; set; }

        public string SubjectName { get; set; }

        public int Mark { get; set; }

        public bool IsSucceeded { get; set; }

        public bool IsMain { get; set; }

        public bool IsArabic { get; set; }

        public bool IsBehavioral { get; set; }
    }
}
