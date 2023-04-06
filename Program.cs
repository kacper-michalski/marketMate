using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace marketMate
{
    class Program
    {

        static void initialWarehouse()
        {
            string path = @"warehouse.txt";
            StreamWriter sw;

            if (!File.Exists(path))
            {
                sw = File.CreateText(path);
                Console.WriteLine("Magazyn został utworzony!");
            }
            else
            {
                sw = new StreamWriter(path, true);
                Console.WriteLine("Magazyn został otwarty!");
            }

            sw.Close();
            StreamReader sr = File.OpenText(path);
            string s = "";
            int i = 1;
            Console.WriteLine("Zawartość magazynu:");
            while ((s = sr.ReadLine()) != null)
            {
                Console.WriteLine(i++ + ". " + s);
            }

            sr.Close();

 
            
        }

        static void endOfApp()
        {
            Console.Write("Naciśnij dowolny klawisz");
            Console.ReadKey();
        }

        static void Main(string[] args)
        {
            initialWarehouse();

            endOfApp();

        }
    }
}
