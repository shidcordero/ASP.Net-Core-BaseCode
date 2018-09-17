using Data.Utilities;
using System.ComponentModel.DataAnnotations;

namespace Data.ViewModels.Region
{
    public class RegionSearchViewModel
    {
        [Display(Name = "Find by Region")]
        public string RegionName { get; set; } = string.Empty;

        public int Page { get; set; } = 1;

        public int PageSize { get; set; } = 15;

        public string SortBy { get; set; } = nameof(RegionName);

        public string SortOrder { get; set; } = Constants.SortDirection.Ascending;
    }
}