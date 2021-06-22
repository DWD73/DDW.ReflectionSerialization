using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text.Json;
using CsvHelper;
using CsvHelper.Configuration;
using System.Threading.Tasks;
using System.Globalization;

namespace DDW.ReflectionSerialization
{
    public class CarSerReflection<T>
    {
        static readonly string separator = @"\t";

        public static void CarSerialize(T obj, out string strObjSer)
        {
            Type type = typeof(T);          

            PropertyInfo[] propertyInfos = type.GetProperties();          

            string[] result = new string[propertyInfos.Length];
            int i = 0;

            foreach (PropertyInfo prop in propertyInfos)
            {               
                result[i] = $"{prop.Name}:{prop.GetValue(obj)}";
                i++;
            }

            strObjSer = string.Join(separator, result);
            
        }

        public static string CarSerializeJSON(T obj)
        {
            return JsonSerializer.Serialize(obj);
        }

        public static T CarDeSerializeJSON(string jsonText)
        {
            return JsonSerializer.Deserialize<T>(jsonText);
        }

        //public static async Task CarSerializeJSON(Car obj)
        //{           
        //    using (FileStream fs = new FileStream("car.json", FileMode.OpenOrCreate))
        //    {              
        //        await JsonSerializer.SerializeAsync<Car>(fs, obj);              
        //    }          
        //}

        //public static async Task<Car> CarDeSerializeJSON()
        //{
        //    Car car;

        //    using (FileStream fs = new FileStream("car.json", FileMode.OpenOrCreate))
        //    {
        //        car = await JsonSerializer.DeserializeAsync<Car>(fs);
        //    }

        //    return car;
        //}

        public static void CarDeSerialize(string txt, out T resultOut)
        {
            Type type = typeof(T);

            PropertyInfo[] propertyInfos = type.GetProperties();          

            Object result = Activator.CreateInstance(type);        

            var NumValuePair = txt.Split(separator);           

            for (int i = 0; i < NumValuePair.Length; i++)
            {               
                var nameValue = NumValuePair[i].Split(':');

                var propertyName = type.GetProperty(nameValue[0]);               

                propertyName.SetValue(result, Convert.ChangeType(nameValue[1], propertyName.PropertyType));                            
            }

            resultOut = (T)result;

        }

        public static void GetDataCSV2(T obj)
        {
            string[] slist;

            IEnumerable<string> lines = File.ReadLines("Test.csv");
            foreach (var line in lines)
            {
                slist = line.Split(',');
            }
        }

        public static void SetDataCSV(T obj)
        {
            var list = new List<T>();
            list.Add(obj);
           
            var cfg = new CsvConfiguration(CultureInfo.InvariantCulture);
            cfg.Delimiter = ",";
          
            using (var sw = new StreamWriter("test.csv"))
            using (var csv = new CsvWriter(sw, cfg))
            {
                csv.WriteRecords(list);
            }

            ////read from csv
            //using (var sr = new StreamReader("test.csv"))
            //using (var csv = new CsvReader(sr, cfg))
            //{
            //    var records = csv.GetRecords<MyObject>();
            //    foreach (var item in records)
            //    {
            //        System.Console.WriteLine(item);
            //    }
            //}
        }

        //public static IEnumerable<T> GetDataCSV(T obj)
        public static List<T> GetDataCSV()
        {
            var list = new List<T>();
            //list.Add(obj);

            var cfg = new CsvConfiguration(CultureInfo.InvariantCulture);
            cfg.Delimiter = ",";

            //using (var sw = new StreamWriter("test.csv"))
            //using (var csv = new CsvWriter(sw, cfg))
            //{
            //    csv.WriteRecords(list);
            //}

            //read from csv
            using (var sr = new StreamReader("test.csv"))
            using (var csv = new CsvReader(sr, cfg))
            {
                var records = csv.GetRecords<T>();
                //list.Add(csv.GetRecords<T>());
                foreach (var item in records)
                {
                    list.Add(item);
                    //System.Console.WriteLine(item);
                }
            }

            return list;


        }

    }

    class MyObject
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double SomeValue { get; set; }
        public bool SomeBoolean { get; set; }

        public string ToCSV(bool msexcel)
        {
            var delimiter = msexcel ? ";" : ",";

            return $"{Id}{delimiter}" +
                $"{Name}{delimiter}" +
                $"{SomeValue.ToString(CultureInfo.InvariantCulture)}{delimiter}" +
                $"{SomeBoolean}";
        }
    }


}
