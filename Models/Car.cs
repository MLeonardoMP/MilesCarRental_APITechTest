namespace MilesCarRentalAPITechTest.Data.Models
{
    //Clase automóvil con parámetros de interés para su renta
    public class Car
    {
        public required int carId { get; set; }
        public required string type { get; set; }
        public required string model { get; set; }
        public required string brand { get; set; }
        public required string transmissionType { get; set; }
        public required int passagersCapacity { get; set; }


    }
}
