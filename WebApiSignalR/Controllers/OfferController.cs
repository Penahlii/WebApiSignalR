using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Linq;
using System.Text.Json;
using WebApiSignalR.Entities;

namespace WebApiSignalR.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OfferController : ControllerBase
    {
        private static readonly string DataFilePath = "data.json";

        [HttpGet]
        public IActionResult Get()
        {
            var jsonData = System.IO.File.ReadAllText(DataFilePath);
            var data = JsonSerializer.Deserialize<CarCollection>(jsonData);
            return Ok(data?.Cars ?? Array.Empty<Car>());
        }

        [HttpGet("Room")]
        public IActionResult Get(string room)
        {
            var carData = GetCarData();
            var car = carData.FirstOrDefault(c => c.Name == room);
            return car == null ? NotFound() : Ok(car.Price);
        }

        [HttpGet("IncreaseRoom")]
        public IActionResult IncreaseRoom(string room, double data)
        {
            var carData = GetCarData();
            var car = carData.FirstOrDefault(c => c.Name == room);
            if (car == null) return NotFound();

            car.Price += (decimal)data;
            SaveCarData(carData);
            return Ok();
        }

        private Car[] GetCarData()
        {
            var jsonData = System.IO.File.ReadAllText(DataFilePath);
            var data = JsonSerializer.Deserialize<CarCollection>(jsonData);
            return data?.Cars ?? Array.Empty<Car>();
        }

        private void SaveCarData(Car[] carData)
        {
            var data = new CarCollection { Cars = carData };
            var jsonData = JsonSerializer.Serialize(data);
            System.IO.File.WriteAllText(DataFilePath, jsonData);
        }

        private class CarCollection
        {
            public Car[] Cars { get; set; }
        }
    }
}
