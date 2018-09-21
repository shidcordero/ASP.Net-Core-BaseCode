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

        /// <summary>
        /// Used to Find region by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Region> FindById(int id)
        {
            return await _regionRepository.FindById(id);
        }

        /// <summary>
        /// Gets region paginated list
        /// </summary>
        /// <param name="searchViewModel">holds the search parameter</param>
        /// <returns>Paginated List</returns>
        public async Task<Extensions.PaginatedList<Region>> FindRegions(RegionSearchViewModel searchViewModel)
        {
            return await _regionRepository.FindRegions(searchViewModel);
        }

        /// <summary>
        /// Used to create Region
        /// </summary>
        /// <param name="region">holds the region data</param>
        public async Task Create(Region region)
        {
            await _regionRepository.Create(region);
        }

        /// <summary>
        /// Used to update Region
        /// </summary>
        /// <param name="region">holds the region data</param>
        /// <returns>UpdateRegionViewModel</returns>
        public async Task<UpdateRegionViewModel> Update(Region region)
        {
            return await _regionRepository.Update(region);
        }

        /// <summary>
        /// Used to delete Region
        /// </summary>
        /// <param name="region">holds the region data</param>
        public async Task Delete(Region region)
        {
            await _regionRepository.Delete(region);
        }

        /// <summary>
        /// Used to delete Region by id
        /// </summary>
        /// <param name="id">holds the region id</param>
        public async Task DeleteById(int id)
        {
            await _regionRepository.DeleteById(id);
        }
        
        /// <summary>
        /// Checks if region is still exists in the database
        /// </summary>
        /// <param name="name">holds the region name</param>
        /// <returns>Boolean data</returns>
        public async Task<bool> IsRegionExists(string name)
        {
            return await _regionRepository.IsRegionExists(name);
        }

        /// <summary>
        /// Checks if region is still in use
        /// </summary>
        /// <param name="id">holds the region id</param>
        /// <returns>Boolean data</returns>
        public async Task<bool> IsRegionInUsed(int id)
        {
            return await _regionRepository.IsRegionInUsed(id);
        }

        /// <summary>
        /// Gets the dropdown list items
        /// </summary>
        /// <returns>Region dropdown item</returns>
        public async Task<List<SelectListItem>> GetRegionDropdownItem()
        {
            return await _regionRepository.GetRegionDropdownItem();
        }
    }
}