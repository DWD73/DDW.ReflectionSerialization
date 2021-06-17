using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace DDW.ReflectionSerialization
{
    class Program
    {
        static Stopwatch stopwatchSerialize = new Stopwatch();

        static void Main(string[] args)
        {
            Start();
        }      

        public static void Start()
        {
            Car car = default;
            string StrObjSer = default;

            for (int i = 0; i < 1000; i++)
            {
                car = new Car();
                stopwatchSerialize.Start();
                GetStringSerialize(car, out StrObjSer);
                stopwatchSerialize.Stop();
                if (i % 20000 == 0) Console.Write(new string('-', 1));
            }

            Console.WriteLine($"\nВремя сериализации\n\t {TimeElapsedToString(stopwatchSerialize.Elapsed)}\nРезультат сериализации в строку:\n\t{StrObjSer}");

            GetObjectDeSerialize(car, StrObjSer);


        }      

        public static void GetStringSerialize(Car car, out string StrObjSer)
        {                       
            CarSerReflection<Car>.CarSerialize(car, out StrObjSer);           

            string elapsedTime = TimeElapsedToString(stopwatchSerialize.Elapsed);
      
        }






        public static void GetObjectDeSerialize(Car car, string StrObjSer)
        {
            CarSerReflection<Car>.CarDeSerialize(car, StrObjSer);

            string elapsedTime = TimeElapsedToString(stopwatchSerialize.Elapsed);

        }

        private static string TimeElapsedToString(TimeSpan ts)
        {
            return String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
            ts.Hours, ts.Minutes, ts.Seconds,
            ts.Milliseconds / 10);
        }
    }

    

    public class CarSerReflection<T>
    {

        static readonly string separator = @"\t";

        public static void CarSerialize(T obj, out string strObjSer)
        {
            Type type = typeof(T);          

            PropertyInfo[] propertyInfos = type.GetProperties();

            FieldInfo[] fieldInfos = type.GetFields();

            var hh = from k in propertyInfos select k;

            strObjSer = string.Join(separator, propertyInfos.Select(x => new {_ = x.Name, Value = x.GetValue(obj)}));
            //strObjSer = string.Join(separator, propertyInfos.Select(x => x.Name x.GetValue(obj)));

            //string txt2 = string.Join(separator, fieldInfos.Select(x => x.Name));

            

            //var queryLondonCustomers = from cust in customers
            //                           where cust.City == "London"
            //                           select cust;

        }





        public static void CarDeSerialize(T obj, string txt)
        {
            Type type = typeof(T);

            PropertyInfo[] propertyInfos = type.GetProperties();

            FieldInfo[] fieldInfos = type.GetFields();

            var result = Activator.CreateInstance(type);

            var NumValuePair = txt.Split(separator);

            var t = NumValuePair[0].Split(',');

            var tt = type.GetField(t[0]);

            Console.WriteLine(tt);
            
            //return null;
        }
    }
}
