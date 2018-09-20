using System.Collections.Generic;
using Data.Models.Entities;
using Data.Utilities;
using Data.ViewModels.Region;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Domain.Contracts
{
    public interface IRegionService
    {
        Task<Region> FindById(int id);

        Task<Extensions.PaginatedList<Region>> FindRegions(RegionSearchViewModel searchViewModel);

        Task Create(Region region);

        Task<UpdateRegionViewModel> Update(Region region);

        Task Delete(Region region);

        Task DeleteById(int id);

        Task<bool> IsRegionExists(string name);

        Task<bool> IsRegionInUsed(int id);

        Task<List<SelectListItem>> GetRegionDropdown();
    }
}