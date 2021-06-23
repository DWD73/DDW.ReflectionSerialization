using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace DDW.ReflectionSerialization
{
    class Program
    {
        static Stopwatch stopwatchSerialize = new Stopwatch();
        static Stopwatch stopwatchSerializeOut = new Stopwatch();
        static Stopwatch stopwatchSerializeJSON = new Stopwatch();
        static Stopwatch stopwatchSerializeOutJSON = new Stopwatch();
        static Stopwatch stopwatchSerializeCSV = new Stopwatch();
        static Stopwatch stopwatchSerializeOutCSV = new Stopwatch();

        static void Main(string[] args)
        {
            Start();
        }

        public static void Start()
        {
            Car car = default;
            Car carTest = default;
            List<Car> cars = new List<Car>();
            string StrObjSer = default;
            string jsonText = default;

            for (int i = 0; i < 1000000; i++)
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
                $"\n\t{carTest.HashcodeCar}\n"
                );


            for (int i = 0; i < 10000; i++)
            {
                car = new Car(Guid.NewGuid());

                stopwatchSerializeJSON.Start();
                GetSerializeJSON(car, out jsonText);
                stopwatchSerializeJSON.Stop();

                stopwatchSerializeOutJSON.Start();
                GetDeSerializeJSON(jsonText, out carTest);
                stopwatchSerializeOutJSON.Stop();

                if (i % 2000 == 0) Console.Write($" - { i } - ");
            }

            Console.WriteLine(new string('-', 80));

            Console.WriteLine($"\nВремя json-сериализации\n\t {TimeElapsedToString(stopwatchSerializeJSON.Elapsed)}\nРезультат:\n\t{jsonText}");

            Console.WriteLine($"\nВремя json-десериализации\n\t {TimeElapsedToString(stopwatchSerializeOutJSON.Elapsed)}" +
                $"\nРезультат:" +
                $"\n\t{carTest.ID}" +
                $"\n\t{carTest.Model}" +
                $"\n\t{carTest.Price}" +
                $"\n\t{carTest.Quantity}" +
                $"\n\t{carTest.HashcodeCar}\n"
                );

            for (int i = 0; i < 1000; i++)
            {
                car = new Car(Guid.NewGuid());

                stopwatchSerializeCSV.Start();
                SetDataCSV(car);
                stopwatchSerializeCSV.Stop();

                stopwatchSerializeOutCSV.Start();
                GetDataFromCSV(out cars);
                stopwatchSerializeOutCSV.Stop();

                if (i % 100 == 0) Console.Write($" - { i } - ");
            }


            Console.WriteLine(new string('-', 80));

            Console.WriteLine($"\nВремя csv-сериализации\n\t {TimeElapsedToString(stopwatchSerializeCSV.Elapsed)}\nРезультат:- запись в файл");

            foreach (Car car2 in cars)
            {           
            Console.WriteLine($"\nВремя csv-десериализации\n\t {TimeElapsedToString(stopwatchSerializeOutCSV.Elapsed)}" +
                $"\nРезультат:" +
                $"\n\t{car2.ID}" +
                $"\n\t{car2.Model}" +
                $"\n\t{car2.Price}" +
                $"\n\t{car2.Quantity}" +
                $"\n\t{car2.HashcodeCar}"
                );
            }
        }

        public static void GetDataFromCSV(out List<Car> cars)
        {           
            cars = CarSerReflection<Car>.GetDataCSV();          
        }

        public static void SetDataCSV(Car car)
        {
            CarSerReflection<Car>.SetDataToCSV(car);
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
