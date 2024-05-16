using ThucHanhAPI2.Model.Domain;

namespace ThucHanhAPI2.Model.DTO
{
    public class AuthorDTO
    {
        public int Id { get; set; }
        public string? FullName { get; set; }
    }
    public class AuthorNoIdDTO
    {
        public string? FullName { get; set; }
    }
}
