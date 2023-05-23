namespace EducationalPlatform.DataAccess.Models
{
    public class TeachingMaterial
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public byte[] Bytes { get; set; }
        public int? SubjectId { get; set; }
        public Subject? Subject { get; set; }
        public ESemester Semester { get; set; }

        public TeachingMaterial() { }
    }
}
