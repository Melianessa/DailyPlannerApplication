using System.ComponentModel.DataAnnotations;

namespace DailyPlanner.Identity.Models
{
    public class UserLoginModel
    {
        [Required]
        [EmailAddress]
        public string Username { get; set; }
        [Required]
        [MinLength(6)]
        [MaxLength(256)]
        public string Password { get; set; }
        public string Client_id { get; set; }
        public string Client_secret { get; set; }
        public string Grant_type { get; set; }


    }
}
