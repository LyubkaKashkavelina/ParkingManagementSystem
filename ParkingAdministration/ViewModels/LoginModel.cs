using System.ComponentModel.DataAnnotations;

namespace ParkingAdministration.ViewModels
{
    public class LoginModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
