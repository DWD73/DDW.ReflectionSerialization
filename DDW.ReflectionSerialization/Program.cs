using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting;
using System.Text.RegularExpressions;

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

            for (int i = 0; i < 1; i++)
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

            //Dictionary<string, string> valuePairs = new Dictionary<string, string>();

            //var result = propertyInfos.Select(x => new {PropertyName = x.Name, Value = x.GetValue(obj)});
            var result = propertyInfos.Select(x => new {v = x.Name + "=" + x.GetValue(obj) });



            strObjSer = string.Join(separator, result);
            

        }





        public static void CarDeSerialize(T obj, string txt)
        {
            Type type = typeof(T);

            PropertyInfo[] propertyInfos = type.GetProperties();

            FieldInfo[] fieldInfos = type.GetFields();

            Object result = Activator.CreateInstance(type);

           

            var NumValuePair = txt.Split(separator);
            
            

            for (int i = 0; i < NumValuePair.Length; i++)
            {

                ValueRR(NumValuePair[i].Split(','), out string gg);
                
                //mystring.Substring(mystring.IndexOf("-") + 1)
                Console.WriteLine(gg);
            }


        }

        private static void ValueRR(string[] vs, out string propertyText)
        {
            int indexOfChar;
            propertyText = default;

            //Match re = Regex.Match(vs[0], @"\b(\w+)");

            Match re = Regex.Match(vs[0], @"\b(\w+)*");

            for (int i = 0; i < vs.Length; i++)
            {
                indexOfChar = vs[i].IndexOf('=') + 1;
                propertyText = vs[i].Substring(indexOfChar).Trim();
            }
        }
    }
}
