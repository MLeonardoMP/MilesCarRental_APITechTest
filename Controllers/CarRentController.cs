using Microsoft.AspNetCore.Mvc;
using MilesCarRentalAPITechTest.Data.Models;

namespace MilesCarRentalAPITechTest.Data

{   //especificamos la ruta de de exposición de la API y lautilización de respuestas HTTP API
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CarRentController : ControllerBase
    {
        private DatabaseContext? _databaseContext;

        public CarRentController(DatabaseContext databaseContext) { 
            _databaseContext = databaseContext;
        }
        //Definimos un método encontrar los autos disponibles para rentar según los parámetros de búsqueda,  incluyendo parámetros para paginación y ordenamiento 
        [HttpGet]
        public IActionResult GetRentCars(//Localización de recogida, entrega y mercado
                                         string pickUpLocationName,
                                         string dropOffLocationName, 
                                         string marketName,
                                         //parámetros de paginación
                                         int pageNum =1,
                                         int pageSize = 10,
                                         string searchTerm="",//Término de búsqueda 
                                         string sortOrder=""// Ordenamiento decendente o acendente 
                                         ) {
            //Consultamos las localizaciones y mercado para ver los carros disponibles en cada
            var pickUpLocation = _databaseContext?.locations.FirstOrDefault( x => x.name == pickUpLocationName );
            var dropOffLocation = _databaseContext?.locations.FirstOrDefault(x=> x.name == dropOffLocationName );
            var market = _databaseContext?.markets.FirstOrDefault(x => x.name == marketName);
            //interceptamos las listas de carros disponibles en ambas localizaciones y en el mercado
            //
            var cars = new List<Car>();//Lista de carros que cumplan los parámetros de búsqueda
            if (pickUpLocation != null && dropOffLocation != null && market != null) {
                 cars = pickUpLocation.carList
                           .Intersect(dropOffLocation.carList)
                           .Intersect(market.carList)
                           .ToList();
            }
            else {  cars = pickUpLocation.carList; }// Solo se contempla el caso en el que solo se envía la localicación de recogida pero podrían contemplarse otros casos
            //comprobamos que se encotnraron carros
            if (cars == null)
            {

                return NotFound();//En este caso se envía código de estatus 404

            }
            //Creamos un query de selección según el término de búsqueda para mostrar solamente carros según lo que el cliente escriba en la caja de búsqueda
            //Teniendo en cuenta todos los posible parámetros de búsqueda
            searchTerm = searchTerm.ToLower();
            var query = cars.Where(x => string.IsNullOrEmpty(searchTerm) ||
                                       x.model.ToLower().Contains(searchTerm) ||
                                       x.brand.ToLower().Contains(searchTerm) ||
                                       x.type.ToLower().Contains(searchTerm) ||
                                       x.transmissionType.ToLower().Contains(searchTerm) ||
                                       x.passagersCapacity.ToString().Contains(searchTerm))
                                       .AsQueryable();
            //Creamos un diccionario para realizar el ordenamiento de manera acendente y decendente por cada uno de los parámetros
         
            var sortDictionary = new Dictionary<string, Func<IQueryable<Car>, IOrderedQueryable<Car>>>
            {
                ["model_desc"] = q => q.OrderByDescending(x => x.model),
                ["brand"] = q => q.OrderBy(x => x.brand),
                ["brand_desc"] = q => q.OrderByDescending(x => x.brand),
                ["type"] = q => q.OrderBy(x => x.type),
                ["type_desc"] = q => q.OrderByDescending(x => x.type),
                ["transmissionType"] = q => q.OrderBy(x => x.transmissionType),
                ["transmissionType_desc"] = q => q.OrderByDescending(x => x.transmissionType),
                ["passagersCapacity"] = q => q.OrderBy(x => x.passagersCapacity),
                ["passagersCapacity_desc"] = q => q.OrderByDescending(x => x.passagersCapacity),
                [""] = q => q.OrderBy(x => x.model)//el ordenamiento por defecto será acendente por modelo (tal vez no sea lo mejor pero podría cambiarse facilmente)

            };
            //Creamos variable data para almacenar los carros que además de cumplir los criterios de búsqueda cumplan los criterios de paginación actuales
            var data = query.Skip((pageNum-1)*pageSize)
                            .Take(pageSize)
                            .Select(x=> new Car {
                                carId = x.carId,
                                model = x.model,
                                brand = x.brand,
                                type = x.type,
                                transmissionType = x.transmissionType,
                                passagersCapacity = x.passagersCapacity
                            }).ToList();
            //Cantidad de carros que cumplent condiciones de búsqueda
            int totalItems = query.Count();
            //cantidad de páginas requeridas para ordenar esa cantidad de carros
            int totalPages = (int)Math.Ceiling((double)totalItems / pageSize);
            //Se crea la lista final de carros para enviar junto con parámetros de paginación y término de búsqueda empleado
            var carDto = new CarList
            {
                cars = data,
                searchTerm = searchTerm,
                totalPages = totalPages,
                currentPage = pageNum
            };
            //si todo sale bien se envían el modelo final con un código de estatus 200
            return Ok(carDto);
        }
        
    }
}
