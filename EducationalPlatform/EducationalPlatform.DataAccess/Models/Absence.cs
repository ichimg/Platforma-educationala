namespace EducationalPlatform.DataAccess.Models
{
    public class Absence
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int SubjectId { get; set; }
        public Subject Subject { get; set; }
        public int StudentId { get; set; }
        public Student Student { get; set; }
        public int TeacherId { get; set; }
        public Teacher Teacher { get; set; }
        public bool? IsMotivated { get; set; }
        public ESemester Semester { get; set; }

        public Absence()
        {      }

        public Absence(Student student, Teacher teacher, Subject subject)
        {
            Student = student;
            Teacher = teacher;
            Subject = subject;
            Date = DateTime.Now;
            IsMotivated = false;
        }
    }
}
