namespace Data.Utilities
{
    public static class Helpers
    {
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

        public static string GetSortIcon(string source, string sortBy, string sortDirection)
        {
            if (!source.Equals(sortBy)) return Constants.SortIcon.Sort;

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