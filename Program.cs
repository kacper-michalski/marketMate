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

        static void createWarehouse()
        {
            string path = @"warehouse.txt";
            StreamWriter sw = File.CreateText(path);
            sw.Close();
        }
        static StreamReader streamReaderWarehouse()
        {
            string path = @"warehouse.txt";
            StreamReader sr = File.OpenText(path);
            return sr;
        }

        static StreamWriter streamWriterWarehouse()
        {
            string path = @"warehouse.txt";
            StreamWriter sw = new StreamWriter(path, true);
            return sw;
        }

        static string findProductInWarehouse(string id)
        {
            StreamReader sr = streamReaderWarehouse();
            string line = "";
            while ((line = sr.ReadLine()) != null)
            {
                string[] elements = line.Split(' ');
                if (elements[0] == id)
                {
                    sr.Close();
                    return line + " szt.";
                }
            }
            return "Nie znaleziono takiego produktu";
        }
        static int findProductIndexInWarehouse(string id)
        {
            StreamReader sr = streamReaderWarehouse();
            string line = "";
            int index = 0;
            while ((line = sr.ReadLine()) != null)
            {
                string[] elements = line.Split(' ');
                if (elements[0] == id)
                {
                    sr.Close();
                    return index;
                }
                index++;
            }
            sr.Close();
            return -1;
        }


        static void addNewProductToWarehouse(string id, string quantity)
        {
            StreamWriter sw = streamWriterWarehouse();
            sw.WriteLine(id + " " + quantity);
            sw.Close();
        }


        static void removeProductFromWarehouse(string id)
        {
            string[] lines = File.ReadAllLines("warehouse.txt");
            lines = lines.Where((line, index) => index != findProductIndexInWarehouse(id)).ToArray();

            File.WriteAllLines("warehouse.txt", lines);
        }
        static void changeTheQuantityOfProductInWarehouse(string id, int quantityToReduce)
        {
            string[] lines = File.ReadAllLines("warehouse.txt");
            int productIndex = findProductIndexInWarehouse(id);
            string[] line = lines[productIndex].Split(' ');
            if ((line[0]) == id)
            {
                line[1] = (int.Parse(line[1]) + quantityToReduce).ToString();
                lines[productIndex] = line[0] + ' ' + line[1];
            }
            File.WriteAllLines("warehouse.txt", lines);
        }

        static void createPriceList()
        {
            string path = @"priceList.txt";
            StreamWriter sw = File.CreateText(path);
            sw.Close();
        }
        static StreamReader streamReaderPriceList()
        {
            string path = @"priceList.txt";
            StreamReader sr = File.OpenText(path);
            return sr;
        }

        static StreamWriter streamWriterPriceList()
        {
            string path = @"priceList.txt";
            StreamWriter sw = new StreamWriter(path, true);
            return sw;
        }
        static void initializeWarehouse()
        {
            string path = @"warehouse.txt";
            StreamWriter sw;

            if (!File.Exists(path))
            {
                sw = File.CreateText(path);
            }
            else
            {
                sw = new StreamWriter(path, true);
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

        static void initializePriceList()
        {
            string path = @"priceList.txt";
            StreamWriter sw;

            if (!File.Exists(path))
            {
                sw = File.CreateText(path);
            }
            else
            {
                sw = new StreamWriter(path, true);
            }

            sw.Close();
            StreamReader sr = File.OpenText(path);
            string s = "";
            int i = 1;
            Console.WriteLine("Zawartość cennika:");
            while ((s = sr.ReadLine()) != null)
            {
                Console.WriteLine(i++ + ". " + s);
            }

            sr.Close();
        }

        static void listFeatures()
        {
            string id = "";
            string quantity = "";
            string[] features = { "1. Magazyn", "2. Cennik", "3. Sprzedaż"};
            foreach (var feature in features)
            {
                Console.WriteLine(feature);
            }

            Console.Write("Wybierz funkcjonalność podając odpowiednią liczbę: ");
            string selectedFeature = Console.ReadLine();
            Console.WriteLine();

            switch (selectedFeature)
            {
                case "1":
                    Console.WriteLine("Wybrano magazyn");
                    string[] warehouseFeatures = { "1. Dodawanie", "2. Usuwanie", "3. Szukanie"};
                    foreach (var feature in warehouseFeatures)
                    {
                        Console.WriteLine(feature);
                    }

                    Console.Write("Wybierz funkcjonalność podając odpowiednią liczbę: ");
                    selectedFeature = Console.ReadLine();
                    Console.WriteLine();
                    switch (selectedFeature)
                    {
                        case "1":
                            Console.WriteLine("Wybrano dodawanie");
                            Console.WriteLine("Podaj id produktu: ");
                            id = Console.ReadLine();
                            Console.WriteLine("Podaj ilość produktu: ");
                            quantity = Console.ReadLine();
                            addNewProductToWarehouse(id, quantity);
                            break;
                        case "2":
                            Console.WriteLine("Wybrano usuwanie");
                            Console.WriteLine("Podaj id produktu: ");
                            id = Console.ReadLine();
                            removeProductFromWarehouse(id);
                            break;
                        case "3":
                            Console.WriteLine("Podaj id produktu: ");
                            id = Console.ReadLine();
                            findProductIndexInWarehouse(id);
                            Console.WriteLine("Wybrano szukanie");
                            break;
                        default:
                            Console.WriteLine("Err!");
                            break;
                    }
                    break;
                case "2":
                    Console.WriteLine("Wybrano cennik");
                    break;
                case "3":
                    Console.WriteLine("Wybrano sprzedaż");
                    break;
                default:
                    Console.WriteLine("Err!");
                    break;
            }
        }

        static void endOfApp()
        {
            Console.Write("Naciśnij dowolny klawisz...");
            Console.ReadKey();
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Dzień dobry, z tej strony marketMate!");
            changeTheQuantityOfProductInWarehouse("km421", -2);
            endOfApp();

        }
    }
}
