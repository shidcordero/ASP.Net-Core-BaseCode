using System.Collections.Generic;
using Data.Contracts;
using Data.Models.Entities;
using Data.Utilities;
using Data.ViewModels.Region;
using Domain.Contracts;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Domain.Services
{
    public class RegionService : IRegionService
    {
        private readonly IRegionRepository _regionRepository;

        public RegionService(IRegionRepository regionRepository)
        {
            _regionRepository = regionRepository;
        }

        public async Task<Region> FindById(int id)
        {
            return await _regionRepository.FindById(id);
        }

        public async Task<Extensions.PaginatedList<Region>> FindRegions(RegionSearchViewModel searchViewModel)
        {
            return await _regionRepository.FindRegions(searchViewModel);
        }

        public async Task Create(Region region)
        {
            await _regionRepository.Create(region);
        }

        public async Task<UpdateRegionViewModel> Update(Region region)
        {
            return await _regionRepository.Update(region);
        }

        public async Task Delete(Region region)
        {
            await _regionRepository.Delete(region);
        }

        public async Task DeleteById(int id)
        {
            await _regionRepository.DeleteById(id);
        }

        public async Task<bool> IsRegionExists(string name)
        {
            return await _regionRepository.IsRegionExists(name);
        }

        public async Task<bool> IsRegionInUsed(int id)
        {
            return await _regionRepository.IsRegionInUsed(id);
        }

        public async Task<List<SelectListItem>> GetRegionDropdown()
        {
            return await _regionRepository.GetRegionDropdown();
        }
    }
}