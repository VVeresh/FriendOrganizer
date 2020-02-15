using System.ComponentModel.DataAnnotations;

namespace FriendOrganizer.Model
{
    /// <summary>
    /// Model for instance of Friend
    /// </summary>
    public class Friend
    {
        [Key]   // Not needed
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }

        [StringLength(50)]
        public string LastName { get; set; }

        [StringLength(50)]
        public string Email { get; set; }
    }
}
