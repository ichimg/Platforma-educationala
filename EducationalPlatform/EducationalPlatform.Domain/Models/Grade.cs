namespace EducationalPlatform.Domain.Models
{
    public class Grade
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public Student Student { get; set; }
        public int SubjectId { get; set; }
        public Subject Subject { get; set; }
        public DateTime Date { get; set; }
        public int Value { get; set; }
        public bool IsThesis { get; set; }
        public bool IsCanceled { get; set; }
        public ESemester Semester { get; set; }

        public Grade()
        {
        }

        public Grade(int studentId, int subjectId)
        {
            StudentId = studentId;
            SubjectId = subjectId;
            Date = DateTime.Now;
            IsCanceled = false;
        }

    }

    public enum ESemester
    {
        First = 1,
        Second = 2
    }

}
