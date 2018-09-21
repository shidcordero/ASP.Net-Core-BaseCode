using ASP.Extensions;
using AutoMapper;
using Data.Models.Entities;
using Data.Utilities;
using Data.ViewModels.Common;
using Data.ViewModels.Region;
using Domain.Contracts;
using Domain.Handlers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using static Data.Utilities.Extensions;
using Helpers = ASP.Utilities.Helpers;

namespace ASP.Controllers
{
    [Authorize]
    [Route("[controller]/[action]")]
    public class RegionController : Controller
    {
        private readonly IRegionService _regionService;
        private readonly IEmailService _emailService;
        private readonly IMapper _mapper;

        public RegionController(IRegionService regionService, IEmailService emailService, IMapper mapper)
        {
            _regionService = regionService;
            _emailService = emailService;
            _mapper = mapper;
        }
        
        /// <summary>
        /// Loads the Region List View
        /// </summary>
        /// <param name="searchViewModel">Holds the search parameters</param>
        [HttpGet]
        public async Task<IActionResult> List(RegionSearchViewModel searchViewModel)
        {
            try
            {
                var list = await _regionService.FindRegions(searchViewModel);

                var tuple = new Tuple<RegionSearchViewModel, PaginatedList<Region>>(searchViewModel, list);

                return View(tuple);
            }
            catch (Exception ex)
            {
                var exceptionMessage = await Helpers.GetErrors(ex, _emailService);
                ModelState.AddModelError(new ValidationResult(exceptionMessage));
            }

            return View();
        }

        /// <summary>
        /// Loads the Create View
        /// </summary>
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// Post request for Creating Region
        /// </summary>
        /// <param name="regionViewModel">Holds the region Data</param>
        [HttpPost]
        public async Task<IActionResult> Create(RegionViewModel regionViewModel)
        {
            try
            {
                if (!ModelState.IsValid) return View(regionViewModel);

                // Check if isNew is true, this means that the data is from Edit screen
                // which is deleted and would like to create it again
                if (regionViewModel.IsNew)
                {
                    // clear RegionId and RowVersion since it was deleted
                    regionViewModel.RegionId = null;
                    regionViewModel.RowVersion = null;
                }

                // Map View model to model for creation
                var region = _mapper.Map<Region>(regionViewModel);

                var validationResult = await new RegionHandler(_regionService).CanAdd(region);
                if (validationResult == null)
                {
                    // if no validation error, create region data
                    await _regionService.Create(region);

                    //set TempData for Success Modal
                    TempData[Constants.Common.ModalMessage] = Constants.Message.RecordSuccessAdd;

                    return RedirectToAction(nameof(List));
                }

                ModelState.AddModelError(validationResult);

                return View(regionViewModel);
            }
            catch (Exception ex)
            {
                var exceptionMessage = await Helpers.GetErrors(ex, _emailService);
                ModelState.AddModelError(new ValidationResult(exceptionMessage));
            }

            ModelState.AddModelError(string.Empty, "Invalid creation attempt.");

            return View(regionViewModel);
        }

        /// <summary>
        /// Loads the Edit View
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var region = await _regionService.FindById(id);
            if (region == null) return View();

            var regionViewModel = _mapper.Map<RegionViewModel>(region);
            return View(regionViewModel);
        }

        /// <summary>
        /// Post request for Edit
        /// </summary>
        /// <param name="regionViewModel">Holds the region data</param>
        [HttpPost]
        public async Task<IActionResult> Edit(RegionViewModel regionViewModel)
        {
            if (!ModelState.IsValid) return View(regionViewModel);

            if (regionViewModel.RegionId != null)
            {
                // get region to be updated, if null this means it was deleted by another user
                var regionToUpdate = await _regionService.FindById(regionViewModel.RegionId.Value);
                if (regionToUpdate == null)
                {
                    // set IsNew to true
                    regionViewModel.IsNew = true;
                    // show error and update model to view
                    return HandleDeletedRegion(regionViewModel);
                }

                try
                {
                    // Map View model to model
                    var region = _mapper.Map<Region>(regionViewModel);
                    
                    var validationResult = await new RegionHandler(_regionService).CanUpdate(region);
                    if (validationResult == null)
                    {
                        // if no error, update region data
                        var result = await _regionService.Update(region);
                        if (result.ValidationResults.Count == 0)
                        {
                            // if no validation errors upon updating, set TempData for Success Modal
                            TempData[Constants.Common.ModalMessage] = Constants.Message.RecordSuccessUpdate;

                            return RedirectToAction(nameof(List));
                        }

                        // add all errors to ModelState
                        ModelState.AddModelErrors(result.ValidationResults);
                        // Must clear the model error for the next postback.
                        ModelState.Remove(Constants.Region.RowVersion);
                        // update Row Version on model for next postback
                        regionViewModel.RowVersion = result.RowVersion;
                        return View(regionViewModel);
                    }

                    ModelState.AddModelError(validationResult);

                    return View(regionViewModel);
                }
                catch (Exception ex)
                {
                    var exceptionMessage = await Helpers.GetErrors(ex, _emailService);
                    ModelState.AddModelError(new ValidationResult(exceptionMessage));
                }
            }
            ModelState.AddModelError(string.Empty, "Invalid update attempt.");

            return View(regionViewModel);
        }
        
        /// <summary>
        /// Post request for Delete
        /// </summary>
        /// <param name="id">Holds the id to be deleted</param>
        /// <param name="searchViewModel">Holds the search param</param>
        [HttpPost]
        public async Task<IActionResult> Delete(int id, RegionSearchViewModel searchViewModel)
        {
            try
            {
                var validationResult = await new RegionHandler(_regionService).CanDelete(id);
                if (validationResult == null)
                {
                    // if no validation error, delete the record
                    await _regionService.DeleteById(id);
                    // set TempData for Success Modal
                    TempData[Constants.Common.ModalMessage] = Constants.Message.RecordSuccessDelete;

                    return RedirectToAction(nameof(List), searchViewModel);
                }

                ModelState.AddModelError(validationResult);
            }
            catch (Exception ex)
            {
                var exceptionMessage = await Helpers.GetErrors(ex, _emailService);
                ModelState.AddModelError(new ValidationResult(exceptionMessage));
            }

            ModelState.AddModelError(string.Empty, "Invalid delete attempt.");

            // set TempData for Error Modal
            TempData[Constants.Common.ModalTitle] = Constants.Message.Error;
            TempData[Constants.Common.ModalMessage] = Helpers.CreateValidationSummary(ModelState);

            return RedirectToAction(nameof(List), searchViewModel);
        }

        #region Helpers
        /// <summary>
        /// Helper function for Handling deleted region
        /// </summary>
        /// <param name="regionViewModel">Holds the region data</param>
        private IActionResult HandleDeletedRegion(RegionViewModel regionViewModel)
        {
            ModelState.AddModelError(string.Empty, @"Unable to save. The region was deleted by another user.
                If you want to create this record, click Save button.");
            return View(regionViewModel);
        }

        #endregion
    }
}