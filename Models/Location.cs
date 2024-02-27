namespace MilesCarRentalAPITechTest.Data.Models
{
    //Clase de localizaciones de renta de vehículos con su respectiva lista de automóviles
    public class Location
    {
        public required int locationId { get; set; }
        public required string name { get; set; }
        public List<Car>? carList { get; set; }

    }
}
