namespace TugceErciyesProject.Models
{
    public class CourseModel
    {
        public int Id { get; set; }
        public TermEnum Term { get; set; }
        public string CourseCode { get; set; }
        public string CourseName { get; set; }
        public int ECTSCredits { get; set; }
        public string LetterGrade { get; set; }
    }
}