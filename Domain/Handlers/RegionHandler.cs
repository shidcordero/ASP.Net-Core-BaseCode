using Data.Models.Entities;
using Data.Utilities;
using Data.ViewModels.Common;
using Domain.Contracts;
using System.Threading.Tasks;

namespace Domain.Handlers
{
    public class RegionHandler
    {
        private readonly IRegionService _regionService;

        /// <summary>
        /// Instantiate RegionService
        /// </summary>
        /// <param name="regionService"></param>
        public RegionHandler(IRegionService regionService)
        {
            _regionService = regionService;
        }

        /// <summary>
        /// Used to validate if region data can be added in the database
        /// </summary>
        /// <returns>Validation Result</returns>
        public async Task<ValidationResult> CanAdd(Region region)
        {
            ValidationResult validationResult = null;

            if (region != null)
            {
                if (await _regionService.IsRegionExists(region.RegionName))
                {
                    validationResult = new ValidationResult(Constants.Message.ErrorRecordExists);
                }
            }
            else
            {
                validationResult = new ValidationResult(Constants.Message.ErrorRecordInvalid);
            }
            return validationResult;
        }

        /// <summary>
        /// Used to validate if region data can be updated in the database
        /// </summary>
        /// <returns>Validation Result</returns>
        public async Task<ValidationResult> CanUpdate(Region region)
        {
            ValidationResult validationResult = null;

            if (region != null)
            {
                var dbRegion = await _regionService.Find(region.RegionId);
                if (dbRegion != null)
                {
                    if (!region.RegionId.Equals(dbRegion.RegionId) && await _regionService.IsRegionExists(region.RegionName))
                    {
                        validationResult = new ValidationResult(Constants.Message.ErrorRecordExists);
                    }
                }
                else
                {
                    validationResult = new ValidationResult(Constants.Message.ErrorRecordNotExists);
                }
            }
            else
            {
                validationResult = new ValidationResult(Constants.Message.ErrorRecordInvalid);
            }
            return validationResult;
        }

        /// <summary>
        /// Used to validate if region data can be deleted in the database
        /// </summary>
        /// <param name="regionId"></param>
        /// <returns>Validation Result</returns>
        public async Task<ValidationResult> CanDelete(int regionId)
        {
            ValidationResult validationResult = null;
            var region = _regionService.Find(regionId);

            if (region == null)
            {
                validationResult = new ValidationResult(Constants.Message.ErrorRecordNotExists);
            }
            else
            {
                if (await _regionService.IsRegionInUsed(regionId))
                {
                    validationResult = new ValidationResult(Constants.Message.ErrorRecordInUse);
                }
            }
            return validationResult;
        }
    }
}