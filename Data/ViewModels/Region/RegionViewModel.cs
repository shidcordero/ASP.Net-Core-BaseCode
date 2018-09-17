using System.ComponentModel.DataAnnotations;

namespace Data.ViewModels.Region
{
    public class RegionViewModel
    {
        public RegionViewModel()
        {
            IsNew = false;
        }

        public int? RegionId { get; set; }

        [Required]
        [Display(Name = "Region")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 1)]
        public string RegionName { get; set; }

        [Required]
        [Display(Name = "Region Code")]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 1)]
        public string RegionCode { get; set; }

        [Required]
        [Display(Name = "Region Key")]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 1)]
        public string RegionKey { get; set; }

        [Display(Name = "Description")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 0)]
        public string Description { get; set; }

        public bool IsNew { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }
    }
}