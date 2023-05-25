namespace EducationalPlatform.Domain.Models
{
    public class Classroom
    {
        public int Id { get; set; }
        public int Year { get; set; }
        public char Letter { get; set; }
        public int SpecializationId { get; set; }
        public Specialization Specialization { get; set; }
        public int? TeacherId { get; set; }
        public Teacher? Teacher {get; set;}
        public ICollection<Student> Students { get; set; }
        public ICollection<Teacher> Teachers { get; set; }

        public string FullName => $"{Year}{Letter}";
        
        public Classroom()
        {
            
        }

        public Classroom(int id, int year, char letter, int specializationId)
        {
            Id = id;
            Year = year;
            Letter = letter;
            SpecializationId = specializationId;
        }

        public override string ToString()
        {
            return $"{Year}{Letter}";
        }
    }
}
