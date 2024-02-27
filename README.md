## Prueba t�cnica para Browser Travel Solutions
### Candidato: Leonardo Mosquera
### 27 de febrero de 2024
Hola, a continuaci�n, les contar� cu�l fue la metodolog�a base utilizada para el desarrollo de esta API, seguido de una documentaci�n de la misma.
1. Investigaci�n r�pida de la p�gina de Miles Rent Car para tener mayor contexto del enunciado de la prueba
2. Modelado inicial donde se definen los archivos, clases y servicios necesarios para desarrollar la API
* Se definen 4 clases
	1. `Car.cs`:   Clase autom�vil con par�metros de inter�s para su renta.
	2. `Location.cs`: Clase de localizaciones de renta de veh�culos con su respectiva lista de autom�viles.
	3. `Market.cs`:  Clase de mercados de renta de veh�culos con su respectiva lista de autom�viles.
	4. `CarList.cs`: Clase de utilidad para almacenar la lista final de veh�culos a retornar y 3 par�metros adicionales para la paginaci�n y la b�squeda.
* `CarRentController.cs`: Archivo principal de la API contiene todo lo necesario para que esta funcione correctamente, desde la definici�n de la ruta de acceso, configuraciones, conexi�n con base de datos y el m�todo `GetRentCars` para retornar la lista de autom�viles deseada seg�n los criterios de b�squeda y paginaci�n.
* `DatabaseContext.cs`: Definici�n del context para la conexi�n con bases de datos y definici�n inicial de conjuntos de datos de los modelos para interactuar con ellos en el controlador.
### Notas
* El desarrollo de est� api se realiz� con .Net 8 y Visual Studio 2022
# Documentaci�n de la API de alquiler de autos

## Descripci�n

Esta API permite obtener los carros disponibles para rentar seg�n los par�metros de b�squeda, incluyendo par�metros de paginaci�n y ordenamiento.

## Ruta

`/api/CarRent/GetRentCars`

## M�todo

`GET`

## Par�metros

| Nombre                | Tipo     | Descripci�n                                | Obligatorio |
|-----------------------|----------|--------------------------------------------|-------------|
| pickUpLocationName    | string   | Nombre de la ubicaci�n de recogida         | S�          |
| dropOffLocationName   | string   | Nombre de la ubicaci�n de entrega          | S� / No          |
| marketName            | string   | Nombre del mercado                         | S� / No          |
| pageNum               | int      | N�mero de p�gina (por defecto 1)           | No          |
| pageSize              | int      | Tama�o de p�gina (por defecto 10)          | No          |
| searchTerm            | string   | T�rmino de b�squeda                        | No          |
| sortOrder             | string   | Criterio de ordenamiento (por defecto "model") | No       |

Valores v�lidos para `sortOrder`:

- `model_desc`: Ordena por modelo de forma descendente
- `brand`: Ordena por marca de forma ascendente
- `brand_desc`: Ordena por marca de forma descendente
- `type`: Ordena por tipo de forma ascendente
- `type_desc`: Ordena por tipo de forma descendente
- `transmissionType`: Ordena por tipo de transmisi�n de forma ascendente
- `transmissionType_desc`: Ordena por tipo de transmisi�n de forma descendente
- `passagersCapacity`: Ordena por capacidad de pasajeros de forma ascendente
- `passagersCapacity_desc`: Ordena por capacidad de pasajeros de forma descendente
- `""`: Ordena por modelo de forma ascendente (por defecto)

## C�digos de respuesta

- `200 OK`: Se encontraron carros disponibles y se enviaron correctamente.
- `400 Bad Request`: Uno o m�s de los par�metros no son v�lidos.
- `404 Not Found`: No se encontraron carros disponibles para los par�metros especificados.
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
GET /api/CarRent/GetRentCars?pickUpLocationName=Bogot�&dropOffLocationName=Medell�n&marketName=Colombia&pageNum=2&pageSize=20&searchTerm=SUV

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