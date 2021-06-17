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

        public Model Model { get; private set; }
        public double Price { get; set; }      
        public int Quantity { get; set; }
        public int HashcodeCar { get; set; }
        public readonly Guid ID;

        public Car()
        {
            ID = Guid.NewGuid();
            Model = (Model)rnd.Next(4);
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
        Toyote, Volvo, Hyundai, Kia
    }

    public enum CarBoxing : int
    {
        F,
        S
    }
}
