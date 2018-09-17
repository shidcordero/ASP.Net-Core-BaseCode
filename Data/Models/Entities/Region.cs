using System.ComponentModel.DataAnnotations;

namespace Data.Models.Entities
{
    public class Region
    {
        [Key]
        public int RegionId { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Region")]
        public string RegionName { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Region Code")]
        public string RegionCode { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Region Key")]
        public string RegionKey { get; set; }

        [StringLength(100)]
        [Display(Name = "Description")]
        public string Description { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }
    }
}