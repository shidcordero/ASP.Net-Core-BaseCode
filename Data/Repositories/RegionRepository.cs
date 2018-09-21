using Data.Base;
using Data.Contracts;
using Data.Models.Entities;
using Data.Utilities;
using Data.ViewModels.Region;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using ValidationResult = Data.ViewModels.Common.ValidationResult;

namespace Data.Repositories
{
    public class RegionRepository : BaseRepository, IRegionRepository
    {
        public RegionRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        /// <summary>
        /// Find Region using id
        /// </summary>
        /// <param name="id">Holds the region id</param>
        /// <returns>Region Entity Model</returns>
        public async Task<Region> FindById(int id)
        {
            return await GetDbSet<Region>().AsNoTracking().FirstOrDefaultAsync(x => x.RegionId == id);
        }

        /// <summary>
        /// Get Region List
        /// </summary>
        /// <param name="searchViewModel">Holds the search parameters</param>
        /// <returns>PaginatedList</returns>
        public async Task<Extensions.PaginatedList<Region>> FindRegions(RegionSearchViewModel searchViewModel)
        {
            // gets sort direction
            var sortDir = Constants.SortDirection.Ascending;
            if ((!string.IsNullOrEmpty(searchViewModel.SortOrder) && searchViewModel.SortOrder.Equals(Constants.SortDirection.Descending)))
                sortDir = Constants.SortDirection.Descending;

            // get list of region
            var regions = GetDbSet<Region>()
                .AsNoTracking()
                .Where(x => (string.IsNullOrEmpty(searchViewModel.RegionName) || 
                             x.RegionName.Contains(searchViewModel.RegionName, StringComparison.OrdinalIgnoreCase)))
                .OrderByPropertyName(searchViewModel.SortBy, sortDir);

            // generate a paginated list
            return await Extensions.PaginatedList<Region>.CreateAsync(regions.AsNoTracking(), searchViewModel.Page, searchViewModel.PageSize);
        }

        /// <summary>
        /// Creates Region data
        /// </summary>
        /// <param name="region">Region data</param>
        public async Task Create(Region region)
        {
            await GetDbSet<Region>().AddAsync(region);
            await UnitOfWork.SaveChangesAsync();
        }

        /// <summary>
        /// Updates Region data
        /// </summary>
        /// <param name="region">Region data</param>
        /// <returns>UpdateRegionViewModel</returns>
        public async Task<UpdateRegionViewModel> Update(Region region)
        {
            // initialize view model and Region data to be updated
            var updateViewModel = new UpdateRegionViewModel();
            var regionUpdate = await GetDbSet<Region>().FirstOrDefaultAsync(x => x.RegionId == region.RegionId);

            try
            {
                if (regionUpdate != null)
                {
                    // Update the RowVersion to the value when this entity was
                    // fetched. If the entity has been updated after it was
                    // fetched, RowVersion won't match the DB RowVersion and
                    // a DbUpdateConcurrencyException is thrown.
                    // A second postback will make them match, unless a new
                    // concurrency issue happens.
                    Context.Entry(regionUpdate).Property(Constants.Region.RowVersion).OriginalValue = region.RowVersion;

                    // update each property
                    regionUpdate.RegionName = region.RegionName;
                    regionUpdate.RegionCode = region.RegionCode;
                    regionUpdate.RegionKey = region.RegionKey;
                    regionUpdate.Description = region.Description;
                }

                // if RowVersion does not match from database, it will throw a DbUpdateConcurrencyException
                await UnitOfWork.SaveChangesAsync();
            }
            // Catch Concurrency exception. Any exception other than this will be catch in controller
            catch (DbUpdateConcurrencyException ex)
            {
                // Get current data
                var exceptionEntry = ex.Entries.Single();
                var clientValues = (Region) exceptionEntry.Entity;
                var databaseEntry = exceptionEntry.GetDatabaseValues();
                if (databaseEntry == null)
                {
                    // if database data is null, this means data is deleted by another user.
                    updateViewModel.ValidationResults.Add(
                        new ValidationResult("Unable to save changes. The region was deleted by another user."));
                }
                else
                {
                    // Get database data
                    var databaseValues = (Region) databaseEntry.ToObject();

                    //get all properties of the class(Region)
                    var properties = databaseValues.GetType().GetProperties();

                    //iterate class properties and add to validation result list
                    updateViewModel.ValidationResults.AddRange(from prop in properties
                        where prop.Name != Constants.Region.RegionId &&
                              prop.Name != Constants.Region.RowVersion //exclude Id and RowVersion from the iteration
                        let dbValue = prop.GetValue(databaseValues) //get database value
                        let currentValue = prop.GetValue(clientValues) //get current value
                        where !dbValue.Equals(currentValue) //check if db and current value is not equal
                        let displayName = prop
                            .GetCustomAttributes(typeof(DisplayAttribute), false) //get display name of property
                            .Cast<DisplayAttribute>()
                            .Single()
                            .Name
                        select new ValidationResult(
                            $"{displayName} current value: {prop.GetValue(databaseValues)}")); //add error to validation result

                    updateViewModel.ValidationResults.Add(new ValidationResult(
                        @"The record you attempted to edit was modified by another user after you got the original value.
                            The edit operation was canceled and the current values in the database have been displayed. If you still want to edit this record, 
                            click the Save button again."));

                    // Save the current RowVersion so next postback
                    // matches unless a new concurrency issue happens.
                    if (regionUpdate != null)
                        regionUpdate.RowVersion = updateViewModel.RowVersion = databaseValues.RowVersion;
                }
            }

            return updateViewModel;
        }

        /// <summary>
        /// Delete a Region data by Region Entity Model
        /// </summary>
        /// <param name="region">Holds the region datta</param>
        public async Task Delete(Region region)
        {
            GetDbSet<Region>().Remove(region);
            await UnitOfWork.SaveChangesAsync();
        }

        /// <summary>
        /// Delete a Region data by id
        /// </summary>
        /// <param name="id">Holds the region id</param>
        public async Task DeleteById(int id)
        {
            var regionDelete = await GetDbSet<Region>().FirstOrDefaultAsync(x => x.RegionId == id);
            if (regionDelete != null)
            {
                GetDbSet<Region>().Remove(regionDelete);
            }
            await UnitOfWork.SaveChangesAsync();
        }

        /// <summary>
        /// Checks if Region is still exists in the database
        /// </summary>
        /// <param name="name">Holds the region name</param>
        /// <returns>Boolean data</returns>
        public async Task<bool> IsRegionExists(string name)
        {
            return await GetDbSet<Region>().AsNoTracking().AnyAsync(x =>
                x.RegionName.Equals(name, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Checks if Region is still in used
        /// </summary>
        /// <param name="id">Holds the region id</param>
        /// <returns>Boolean data</returns>
        public async Task<bool> IsRegionInUsed(int id)
        {
            return await GetDbSet<AppUser>().AsNoTracking().AnyAsync(x => x.RegionId == id);
        }

        /// <summary>
        /// Gets the dropdown list items
        /// </summary>
        /// <returns>Region dropdown item</returns>
        public async Task<List<SelectListItem>> GetRegionDropdownItem()
        {
            return await GetDbSet<Region>().Select(x =>
                new SelectListItem(x.RegionName, x.RegionId.ToString())
            ).ToListAsync();
        }
    }
}