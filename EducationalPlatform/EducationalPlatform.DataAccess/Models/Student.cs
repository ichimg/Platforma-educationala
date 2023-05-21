namespace EducationalPlatform.DataAccess.Models
{
    public class Student
    {
        public int Id { get; set; }
        public int PersonId { get; set; }
        public Person Person { get; set; }
        public int? ClassroomId { get; set; }
        public Classroom Classroom { get; set; }
        public int? SpecializationId { get; set; }
        public Specialization Specialization { get; set; }

        public Student()
        {
        }

        public Student(int personId)
        {
            PersonId = personId;
        }
        
    }
}
