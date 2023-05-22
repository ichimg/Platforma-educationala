namespace EducationalPlatform.DataAccess.Exceptions
{
    public class EntityNotFoundException : Exception
    {
        public EntityNotFoundException(int id) : base($"Entity with ID '{id}' was not found.") {}
        public EntityNotFoundException(int firstId, int secondId) : base($"Entity with IDs '{firstId}', `{secondId}' was not found.") { }

        public EntityNotFoundException(string message) : base(message) {}
    }
}
