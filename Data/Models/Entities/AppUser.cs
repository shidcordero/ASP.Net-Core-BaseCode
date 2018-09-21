using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Models.Entities
{
    // Add profile data for application users by adding properties to this class
    public class AppUser : IdentityUser
    {
        // Extended Properties

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public int? RegionId { get; set; }

        [ForeignKey("RegionId")]
        public virtual Region Region { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }
    }
}