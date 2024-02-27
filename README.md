## Prueba técnica para Browser Travel Solutions
### Candidato: Leonardo Mosquera
### 27 de febrero de 2024
Hola, a continuación, les contaré cuál fue la metodología base utilizada para el desarrollo de esta API, seguido de una documentación de la misma.
1. Investigación rápida de la página de Miles Rent Car para tener mayor contexto del enunciado de la prueba
2. Modelado inicial donde se definen los archivos, clases y servicios necesarios para desarrollar la API
* Se definen 4 clases
	1. `Car.cs`:   Clase automóvil con parámetros de interés para su renta.
	2. `Location.cs`: Clase de localizaciones de renta de vehículos con su respectiva lista de automóviles.
	3. `Market.cs`:  Clase de mercados de renta de vehículos con su respectiva lista de automóviles.
	4. `CarList.cs`: Clase de utilidad para almacenar la lista final de vehículos a retornar y 3 parámetros adicionales para la paginación y la búsqueda.
* `CarRentController.cs`: Archivo principal de la API contiene todo lo necesario para que esta funcione correctamente, desde la definición de la ruta de acceso, configuraciones, conexión con base de datos y el método `GetRentCars` para retornar la lista de automóviles deseada según los criterios de búsqueda y paginación.
* `DatabaseContext.cs`: Definición del context para la conexión con bases de datos y definición inicial de conjuntos de datos de los modelos para interactuar con ellos en el controlador.
### Notas
* El desarrollo de está api se realizó con .Net 8 y Visual Studio 2022
# Documentación de la API de alquiler de autos

## Descripción

Esta API permite obtener los carros disponibles para rentar según los parámetros de búsqueda, incluyendo parámetros de paginación y ordenamiento.

## Ruta

`/api/CarRent/GetRentCars`

## Método

`GET`

## Parámetros

| Nombre                | Tipo     | Descripción                                | Obligatorio |
|-----------------------|----------|--------------------------------------------|-------------|
| pickUpLocationName    | string   | Nombre de la ubicación de recogida         | Sí          |
| dropOffLocationName   | string   | Nombre de la ubicación de entrega          | Sí / No          |
| marketName            | string   | Nombre del mercado                         | Sí / No          |
| pageNum               | int      | Número de página (por defecto 1)           | No          |
| pageSize              | int      | Tamaño de página (por defecto 10)          | No          |
| searchTerm            | string   | Término de búsqueda                        | No          |
| sortOrder             | string   | Criterio de ordenamiento (por defecto "model") | No       |

Valores válidos para `sortOrder`:

- `model_desc`: Ordena por modelo de forma descendente
- `brand`: Ordena por marca de forma ascendente
- `brand_desc`: Ordena por marca de forma descendente
- `type`: Ordena por tipo de forma ascendente
- `type_desc`: Ordena por tipo de forma descendente
- `transmissionType`: Ordena por tipo de transmisión de forma ascendente
- `transmissionType_desc`: Ordena por tipo de transmisión de forma descendente
- `passagersCapacity`: Ordena por capacidad de pasajeros de forma ascendente
- `passagersCapacity_desc`: Ordena por capacidad de pasajeros de forma descendente
- `""`: Ordena por modelo de forma ascendente (por defecto)

## Códigos de respuesta

- `200 OK`: Se encontraron carros disponibles y se enviaron correctamente.
- `400 Bad Request`: Uno o más de los parámetros no son válidos.
- `404 Not Found`: No se encontraron carros disponibles para los parámetros especificados.
- `500 Internal Server Error`: Se produjo un error interno en el servidor.
**Modelo de respuesta:**

C#

```
public class CarList
{
    public List<Car> cars { get; set; }
    public string searchTerm { get; set; }
    public int totalPages { get; set; }
    public int currentPage { get; set; }
}

public class Car
{
    public int carId { get; set; }
    public string model { get; set; }
    public string brand { get; set; }
    public string type { get; set; }
    public string transmissionType { get; set; }
    public int passengersCapacity { get; set; }
}

```

content_copy

**Ejemplo de uso:**

```
GET /api/CarRent/GetRentCars?pickUpLocationName=Bogotá&dropOffLocationName=Medellín&marketName=Colombia&pageNum=2&pageSize=20&searchTerm=SUV

```

**Respuesta:**

JSON

```
{
    "cars": [
        {
            "carId": 1,
            "model": "RAV4",
            "brand": "Toyota",
            "type": "SUV",
            "transmissionType": "Automatic",
            "passengersCapacity": 5
        },
        {
            "carId": 2,
            "model": "Captiva",
            "brand": "Chevrolet",
            "type": "SUV",
            "transmissionType": "Automatic",
            "passengersCapacity": 7
        },
        ...
    ],
    "searchTerm": "SUV",
    "totalPages": 5,
    "currentPage": 2
}

```