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

        static string findProductInWarehouse()
        {
            Console.WriteLine("Podaj id produktu: ");
            string id = Console.ReadLine();
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
        static int findProductIndexInWarehouse()
        {

            Console.WriteLine("Podaj id produktu: ");
            string id = Console.ReadLine();
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

        static void addNewProductToWarehouse()
        {
            Console.WriteLine("Podaj id produktu: ");
            string id = Console.ReadLine();
            Console.WriteLine("Podaj ilość produktu: ");
            string quantity = Console.ReadLine();
            StreamWriter sw = streamWriterWarehouse();
            sw.WriteLine(id + " " + quantity);
            sw.Close();
        }


        static void removeProductFromWarehouse()
        {
            string[] lines = File.ReadAllLines("warehouse.txt");
            lines = lines.Where((line, index) => index != findProductIndexInWarehouse()).ToArray();

            File.WriteAllLines("warehouse.txt", lines);
        }
        static void changeTheQuantityOfProductInWarehouse()
        {
            string[] lines = File.ReadAllLines("warehouse.txt");
            int quantityToChange;

            string[] features = { "1. Dodawanie ilości produktu", "2. Odejmowanie ilości produktu"};
            foreach (var feature in features)
            {
                Console.WriteLine(feature);
            }

            Console.Write("Wybierz funkcjonalność podając odpowiednią liczbę: ");
            string selectedFeature = Console.ReadLine();
            Console.WriteLine("Podaj id produktu: ");
            string id = Console.ReadLine();
            switch (selectedFeature)
            {
                case "1":
                    Console.WriteLine("Wybrano dodawanie ilości produktu");
                    Console.WriteLine("Podaj ile dodać ilości do stanu tego produktu: ");
                    quantityToChange = int.Parse(Console.ReadLine());

                    break;
                case "2":
                    Console.WriteLine("Wybrano odejmowanie ilości produktu");
                    Console.WriteLine("Podaj ile odjąć ilości do stanu tego produktu: ");
                    quantityToChange = int.Parse(Console.ReadLine()) * -1;

                    break;
                default:
                    Console.WriteLine("Err!");
                    return;
            }
            int productIndex = findProductIndexInWarehouse(id);
            string[] line = lines[productIndex].Split(' ');
            if ((line[0]) == id)
            {
                line[1] = (int.Parse(line[1]) + quantityToChange).ToString();
                lines[productIndex] = line[0] + ' ' + line[1];
            }
            File.WriteAllLines("warehouse.txt", lines);
        }

        static string findProductInPriceList()
        {
            Console.WriteLine("Podaj id produktu: ");
            string id = Console.ReadLine();
            StreamReader sr = streamReaderPriceList();
            string line = "";
            while ((line = sr.ReadLine()) != null)
            {
                string[] elements = line.Split(' ');
                if (elements[0] == id)
                {
                    sr.Close();
                    return line + " zł";
                }
            }
            return "Nie znaleziono takiego produktu";
        }
        static int findProductIndexInPriceList()
        {

            Console.WriteLine("Podaj id produktu: ");
            string id = Console.ReadLine();
            StreamReader sr = streamReaderPriceList();
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

        static void addNewProductToPriceList()
        {
            Console.WriteLine("Podaj id produktu: ");
            string id = Console.ReadLine();
            Console.WriteLine("Podaj cenę produktu: ");
            string price = Console.ReadLine();
            StreamWriter sw = streamWriterPriceList();
            sw.WriteLine(id + " " + price);
            sw.Close();
        }

        static void removeProductFromPriceList()
        {
            string[] lines = File.ReadAllLines("priceList.txt");
            lines = lines.Where((line, index) => index != findProductIndexInWarehouse()).ToArray();

            File.WriteAllLines("priceList.txt", lines);
        }

        static void changeThePriceOfProductInPriceList(string id, int quantityToReduce)
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
                            addNewProductToWarehouse();
                            break;
                        case "2":
                            Console.WriteLine("Wybrano usuwanie");
                            removeProductFromWarehouse();
                            break;
                        case "3":
                            Console.WriteLine("Wybrano szukanie");
                            findProductInWarehouse();
                            break;
                        default:
                            Console.WriteLine("Err!");
                            break;
                    }
                    break;
                case "2":
                    Console.WriteLine("Wybrano cennik");
                    string[] priceListFeatures = { "1. Dodawanie", "2. Usuwanie", "3. Szukanie" };
                    foreach (var feature in priceListFeatures)
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
                            addNewProductToPriceList();
                            break;
                        case "2":
                            Console.WriteLine("Wybrano usuwanie");
                            removeProductFromPriceList();
                            break;
                        case "3":
                            Console.WriteLine("Wybrano szukanie");
                            findProductInPriceList();
                            break;
                        default:
                            Console.WriteLine("Err!");
                            break;
                    }
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

            endOfApp();

        }
    }
}
