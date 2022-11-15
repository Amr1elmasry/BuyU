namespace BuyU.Classes
{
    public class HomeFilters
    {
        public string? sortOrder { get; set; }
        public string? searchKey { get; set; }
        public string? category { get; set; } 
        public int? pageSize { get; set; }
        public int? min { get; set; }
        public int? max { get; set; }
        public int? pageNumber { get; set; }
    }
}