namespace Data.Utilities
{
    /// <summary>
    /// Global Helpers
    /// </summary>
    public static class Helpers
    {
        /// <summary>
        /// Gets the next sort direction
        /// </summary>
        /// <param name="source">holds the header source</param>
        /// <param name="sortBy">holds the current sort by</param>
        /// <param name="sortDirection">holds the current sort direction</param>
        /// <returns></returns>
        public static string GetSortDirection(string source, string sortBy, string sortDirection)
        {
            if (!source.Equals(sortBy)) return Constants.SortDirection.Ascending;

            switch (sortDirection)
            {
                case Constants.SortDirection.Ascending:
                    return Constants.SortDirection.Descending;

                default:
                    return Constants.SortDirection.Ascending;
            }
        }

        /// <summary>
        /// Gets the icon to be displayed in the table
        /// </summary>
        /// <param name="source">holds the header source</param>
        /// <param name="sortBy">holds the current sort by</param>
        /// <param name="sortDirection">holds the current sort direction</param>
        /// <returns></returns>
        public static string GetSortIcon(string source, string sortBy, string sortDirection)
        {
            if (!source.Equals(sortBy)) return Constants.SortIcon.Unsorted;

            switch (sortDirection)
            {
                case Constants.SortDirection.Ascending:
                    return Constants.SortIcon.SortAscending;

                default:
                    return Constants.SortIcon.SortDescending;
            }
        }
    }
}