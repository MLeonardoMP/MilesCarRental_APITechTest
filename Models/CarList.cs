namespace MilesCarRentalAPITechTest.Data.Models
{
    public class CarList
    {
        public List<Car>? cars { get; set; }
        public string? searchTerm { get; set; }
        public int? totalPages { get; set; }
        public int? currentPage { get; set; }

    }
}
