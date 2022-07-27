using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarServer
{
    public class CarDatabase
    {
        public List<Car> cars;
        public string GetCars()
        {
            return JsonConvert.SerializeObject(cars);
        }
        public void Update(Car car)
        {
            int ind = 0;
            for (int i = 0; i < cars.Count; i++)
            {            
                if (cars[i].Id == car.Id)
                {                    
                    cars[i] = car;
                }
            }
            
        }
        public void Add(Car car)
        {
            cars.Add(car);
        }
        public void Delete(Car car)
        {
            int ind = 0;
            for (int i = 0; i < cars.Count; i++)
            {
                if (cars[i].Id == car.Id)
                {                    
                    cars.RemoveAt(i);
                }
            }
        }
    }
}
