using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Remoting;
using System.Text.RegularExpressions;
using System.Threading;

namespace DDW.ReflectionSerialization
{
    class Program
    {
        static Stopwatch stopwatchSerialize = new Stopwatch();
        static Stopwatch stopwatchSerializeOut = new Stopwatch();        

        static void Main(string[] args)
        {
            Start();
        }      

        public static void Start()
        {
            Car car = default;
            Car carTest = default;
            string StrObjSer = default;
            string jsonText = default;
            
            


            for (int i = 0; i < 10000; i++)
            {
               
                car = new Car(Guid.NewGuid());
                stopwatchSerialize.Start();
                GetStringSerialize(car, out StrObjSer);
                stopwatchSerialize.Stop();

                stopwatchSerializeOut.Start();
                GetObjectDeSerialize(StrObjSer, out carTest);
                stopwatchSerializeOut.Stop();


                if (i % 80000 == 0) Console.Write($" - { i } - ");
            }          

            Console.WriteLine($"\nВремя сериализации\n\t {TimeElapsedToString(stopwatchSerialize.Elapsed)}\nРезультат:\n\t{StrObjSer}");          

            Console.WriteLine($"\nВремя десериализации\n\t {TimeElapsedToString(stopwatchSerializeOut.Elapsed)}" +
                $"\nРезультат:" +
                $"\n\t{carTest.ID}" +
                $"\n\t{carTest.Model}" +
                $"\n\t{carTest.Price}" +
                $"\n\t{carTest.Quantity}" +
                $"\n\t{carTest.HashcodeCar}"
                );


            for (int i = 0; i < 1000; i++)
            {              
                car = new Car(Guid.NewGuid());
                
                stopwatchSerialize.Start();             
                GetSerializeJSON(car, out jsonText);              
                stopwatchSerialize.Stop();

                stopwatchSerializeOut.Start();
                GetDeSerializeJSON(jsonText, out carTest);
                stopwatchSerializeOut.Stop();
            }

            Console.WriteLine(new string('-', 80));

            Console.WriteLine($"\nВремя json-сериализации\n\t {TimeElapsedToString(stopwatchSerialize.Elapsed)}\nРезультат:\n\t{jsonText}");

            Console.WriteLine($"\nВремя json-десериализации\n\t {TimeElapsedToString(stopwatchSerializeOut.Elapsed)}" +
                $"\nРезультат:" +
                $"\n\t{carTest.ID}" +
                $"\n\t{carTest.Model}" +
                $"\n\t{carTest.Price}" +
                $"\n\t{carTest.Quantity}" +
                $"\n\t{carTest.HashcodeCar}"
                );


            

            for (int i = 0; i < 10; i++)
            {
                car = new Car(Guid.NewGuid());

                stopwatchSerialize.Start();
                SetDataFromCSV(car);
                stopwatchSerialize.Stop();

                stopwatchSerializeOut.Start();
                GetDataFromCSV();
                stopwatchSerializeOut.Stop();
            }


            Console.WriteLine(new string('-', 80));

            Console.WriteLine($"\nВремя json-сериализации\n\t {TimeElapsedToString(stopwatchSerialize.Elapsed)}\nРезультат:\n\t{jsonText}");

            Console.WriteLine($"\nВремя json-десериализации\n\t {TimeElapsedToString(stopwatchSerializeOut.Elapsed)}" +
                $"\nРезультат:" +
                $"\n\t{carTest.ID}" +
                $"\n\t{carTest.Model}" +
                $"\n\t{carTest.Price}" +
                $"\n\t{carTest.Quantity}" +
                $"\n\t{carTest.HashcodeCar}"
                );



        }

        public static void GetDataFromCSV()
        {           
            var cars = CarSerReflection<Car>.GetDataCSV();
            foreach(Car car in cars)
            {
                Console.WriteLine(car.Model);
            }          
        }

        public static void SetDataFromCSV(Car car)
        {
            CarSerReflection<Car>.SetDataCSV(car);
        }

        public static void GetStringSerialize(Car car, out string StrObjSer)
        {                       
            CarSerReflection<Car>.CarSerialize(car, out StrObjSer);    
        }       

        public static void GetObjectDeSerialize(string StrObjSer, out Car car)
        {
            CarSerReflection<Car>.CarDeSerialize(StrObjSer, out car);                    
        }

        public static void GetSerializeJSON(Car car, out string jsonText)
        {
            jsonText = CarSerReflection<Car>.CarSerializeJSON(car);
        }

        public static void GetDeSerializeJSON(string jsonText, out Car car)       
        {
            car = CarSerReflection<Car>.CarDeSerializeJSON(jsonText);
        }

        private static string TimeElapsedToString(TimeSpan ts)
        {
            return String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
            ts.Hours, ts.Minutes, ts.Seconds,
            ts.Milliseconds / 10);
        }
    }   
}
