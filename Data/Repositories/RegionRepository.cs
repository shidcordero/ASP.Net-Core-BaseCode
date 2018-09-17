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
using ValidationResult = Data.ViewModels.Common.ValidationResult;

namespace Data.Repositories
{
    public class RegionRepository : BaseRepository, IRegionRepository
    {
        public RegionRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public async Task<Region> Find(int id)
        {
            return await GetDbSet<Region>().FirstOrDefaultAsync(x => x.RegionId == id);
        }

        public async Task<Extensions.PaginatedList<Region>> FindRegions(RegionSearchViewModel searchViewModel)
        {
            var sortDir = Constants.SortDirection.Ascending;
            if ((!string.IsNullOrEmpty(searchViewModel.SortOrder) && searchViewModel.SortOrder.Equals(Constants.SortDirection.Descending)))
                sortDir = Constants.SortDirection.Descending;

            var regions = GetDbSet<Region>()
                .Where(x => (string.IsNullOrEmpty(searchViewModel.RegionName) || x.RegionName.Contains(searchViewModel.RegionName, StringComparison.OrdinalIgnoreCase)))
                .OrderByPropertyName(searchViewModel.SortBy, sortDir);

            return await Extensions.PaginatedList<Region>.CreateAsync(regions.AsNoTracking(), searchViewModel.Page, searchViewModel.PageSize);
        }

        public async Task Create(Region region)
        {
            await GetDbSet<Region>().AddAsync(region);
            await UnitOfWork.SaveChangesAsync();
        }

        public async Task<UpdateRegionViewModel> Update(Region region)
        {
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

                    regionUpdate.RegionName = region.RegionName;
                    regionUpdate.RegionCode = region.RegionCode;
                    regionUpdate.RegionKey = region.RegionKey;
                    regionUpdate.Description = region.Description;
                }

                await UnitOfWork.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                var exceptionEntry = ex.Entries.Single();
                var clientValues = (Region)exceptionEntry.Entity;
                var databaseEntry = exceptionEntry.GetDatabaseValues();
                if (databaseEntry == null)
                {
                    updateViewModel.ValidationResults.Add(new ValidationResult("Unable to save changes. The region was deleted by another user."));
                }
                else
                {
                    var databaseValues = (Region)databaseEntry.ToObject();

                    //get all properties of the class(Region)
                    var properties = databaseValues.GetType().GetProperties();

                    //iterate class properties and add to validation result list
                    updateViewModel.ValidationResults.AddRange(from prop in properties
                        where prop.Name != Constants.Region.RegionId && prop.Name != Constants.Region.RowVersion  //exclude Id and RowVersion from the iteration
                        let dbValue = prop.GetValue(databaseValues)                                               //get database value
                        let currentValue = prop.GetValue(clientValues)                                            //get current value
                        where !dbValue.Equals(currentValue)                                                       //check if db and current value is not equal
                        let displayName = prop.GetCustomAttributes(typeof(DisplayAttribute), false)               //get display name of property
                            .Cast<DisplayAttribute>()
                            .Single()
                            .Name
                        select new ValidationResult($"{displayName} current value: {prop.GetValue(databaseValues)}")); //add error to validation result

                    updateViewModel.ValidationResults.Add(new ValidationResult(@"The record you attempted to edit was modified by another user after you got the original value.
                            The edit operation was canceled and the current values in the database have been displayed. If you still want to edit this record, 
                            click the Save button again."));

                    // Save the current RowVersion so next postback
                    // matches unless a new concurrency issue happens.
                    if (regionUpdate != null) regionUpdate.RowVersion = updateViewModel.RowVersion = databaseValues.RowVersion;
                }
            }

            return updateViewModel;
        }

        public async Task Delete(Region region)
        {
            GetDbSet<Region>().Remove(region);
            await UnitOfWork.SaveChangesAsync();
        }

        public async Task DeleteById(int id)
        {
            var regionDelete = GetDbSet<Region>().FirstOrDefault(x => x.RegionId == id);
            if (regionDelete != null)
            {
                GetDbSet<Region>().Remove(regionDelete);
            }
            await UnitOfWork.SaveChangesAsync();
        }

        public async Task<bool> IsRegionExists(string name)
        {
            return await GetDbSet<Region>().AnyAsync(x =>
                x.RegionName.Equals(name, StringComparison.OrdinalIgnoreCase));
        }

        public async Task<bool> IsRegionInUsed(int id)
        {
            //TODO add implementation when region is used

            return false;
        }
    }
}