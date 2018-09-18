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

        [HttpGet, HttpPost]
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

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(RegionViewModel regionViewModel)
        {
            try
            {
                if (!ModelState.IsValid) return View(regionViewModel);

                if (regionViewModel.IsNew)
                {
                    regionViewModel.RegionId = null;
                    regionViewModel.RowVersion = null;
                }

                var region = _mapper.Map<Region>(regionViewModel);

                var validationResult = await new RegionHandler(_regionService).CanAdd(region);
                if (validationResult == null)
                {
                    await _regionService.Create(region);

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

        public async Task<IActionResult> Edit(int id)
        {
            var region = await _regionService.Find(id);
            if (region == null) return View();

            var regionViewModel = _mapper.Map<RegionViewModel>(region);
            return View(regionViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(RegionViewModel regionViewModel)
        {
            if (!ModelState.IsValid) return View(regionViewModel);

            if (regionViewModel.RegionId != null)
            {
                var regionToUpdate = await _regionService.Find(regionViewModel.RegionId.Value);
                if (regionToUpdate == null)
                {
                    // set IsNew to true and update model on view
                    regionViewModel.IsNew = true;
                    return HandleDeletedRegion(regionViewModel);
                }

                try
                {
                    var region = _mapper.Map<Region>(regionViewModel);
                    
                    var validationResult = await new RegionHandler(_regionService).CanUpdate(region);
                    if (validationResult == null)
                    {
                        var result = await _regionService.Update(region);
                        if (result.ValidationResults.Count == 0) return RedirectToAction(nameof(List));

                        ModelState.AddModelErrors(result.ValidationResults);
                        // Must clear the model error for the next postback.
                        ModelState.Remove(Constants.Region.RowVersion);

                        // update Row Version on model
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

        public IActionResult Delete()
        {
            return PartialView("_ModalDelete");
        }
        
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var validationResult = await new RegionHandler(_regionService).CanDelete(id);
                if (validationResult == null)
                {
                    await _regionService.DeleteById(id);

                    return RedirectToAction(nameof(List));
                }

                ModelState.AddModelError(validationResult);
            }
            catch (Exception ex)
            {
                var exceptionMessage = await Helpers.GetErrors(ex, _emailService);
                ModelState.AddModelError(new ValidationResult(exceptionMessage));
            }

            ModelState.AddModelError(string.Empty, "Invalid delete attempt.");

            return RedirectToAction(nameof(List));
        }

        private IActionResult HandleDeletedRegion(RegionViewModel regionViewModel)
        {
            ModelState.AddModelError(string.Empty, @"Unable to save. The region was deleted by another user.
                If you want to create this record, click Save button.");
            return View(regionViewModel);
        }
    }
}