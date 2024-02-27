namespace MilesCarRentalAPITechTest.Data.Models
{
    public class Location
    {
        public required int locationId { get; set; }
        public required string name { get; set; }
        public List<Car>? carList { get; set; }

    }
}
