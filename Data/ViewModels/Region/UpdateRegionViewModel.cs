using System.Collections.Generic;
using Data.ViewModels.Common;

namespace Data.ViewModels.Region
{
    public class UpdateRegionViewModel
    {
        public UpdateRegionViewModel()
        {
            ValidationResults = new List<ValidationResult>();
        }

        public List<ValidationResult> ValidationResults { get; set; }

        public byte[] RowVersion { get; set; }
    }
}
