using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDW.ReflectionSerialization
{
    public class Car
    {
       
        Random rnd = new Random();

        public string ID { get; set; }
        public string Model { get; set; }
        public double Price { get; set; }      
        public int Quantity { get; set; }
        public int HashcodeCar { get; set; }
        

        public Car() { }
      
        public Car(Guid guid)
        {
            ID = guid.ToString();
            Model = ((Model)rnd.Next(4)).ToString();
            Price = Convert.ToDouble(rnd.Next(1000, 15000) * 1.5);
            Quantity = rnd.Next(1, 10);
            HashcodeCar = this.GetHashCode();
        }     

        public override string ToString()
        {
            return $"Общая сумма заказа {Price * Quantity}";
        }

        
    }

    public enum Model : int
    {
        Toyota, Volvo, Hyundai, Kia, Mercedes
    }

    public enum CarBoxing : int
    {
        F,
        S
    }
}
