using System.ComponentModel.DataAnnotations;

namespace ThucHanhAPI2.Model.DTO
{
    public class LoginRequestDTO
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
