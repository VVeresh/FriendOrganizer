using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace FriendOrganizer.Model
{
    /// <summary>
    /// Model for instance of Friend
    /// </summary>
    public class Friend
    {

        public Friend()
        {
            PhoneNumbers = new Collection<FriendPhoneNumber>();
        }

        [Key]   // Not needed
        public int Id { get; set; }

        [Required]      // Data annotation
        [StringLength(50)]
        public string FirstName { get; set; }

        [StringLength(50)]
        public string LastName { get; set; }

        [StringLength(50)]
        [EmailAddress]
        public string Email { get; set; }

        public int? FavoriteLanguageId { get; set; }

        public ProgrammingLanguage FavoriteLanguage { get; set; }

        public ICollection<FriendPhoneNumber> PhoneNumbers { get; set; }
    }
}
