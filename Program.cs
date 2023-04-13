using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace marketMate
{
    class Program
    {
        enum Features
        {
            Warehouse = 1,
            PriceList = 2
        }
        enum WarehouseFeatures
        {
            Adding = 1,
            Subtraction = 2,
            Find = 3,
            Value = 4,
            ChangeQuantity = 5,
            DisplayWarehouseContents = 6
        }
        enum PriceListFeatures
        {
            Adding = 1,
            Subtraction = 2,
            Find = 3,
            ChangePrice = 4,
            DisplayPriceListContents = 5
        }
        static void CreateWarehouse()
        {
            string path = @"warehouse.txt";
            StreamWriter sw = File.CreateText(path);
            sw.Close();
        }
        static StreamReader StreamReaderWarehouse()
        {
            string path = @"warehouse.txt";
            StreamReader sr = File.OpenText(path);
            return sr;
        }

        static StreamWriter StreamWriterWarehouse()
        {
            string path = @"warehouse.txt";
            StreamWriter sw = new StreamWriter(path, true);
            return sw;
        }

        static void FindProductInWarehouse()
        {
            Console.Write("Podaj id produktu: ");
            string id = Console.ReadLine();
            StreamReader sr = StreamReaderWarehouse();
            string line = "";
            while ((line = sr.ReadLine()) != null)
            {
                string[] elements = line.Split(' ');
                if (elements[0] == id)
                {
                    sr.Close();
                    Console.WriteLine(line + " szt.");
                    return;
                }
            }
            Console.WriteLine("Nie znaleziono takiego produktu");
        }
        static int FindProductIndexInWarehouse()
        {

            Console.Write("Podaj id produktu: ");
            string id = Console.ReadLine();
            StreamReader sr = StreamReaderWarehouse();
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
            Console.WriteLine("Nie znaleziono takiego produktu");
            return -1;
        }

        static int FindProductIndexInWarehouse(string id)
        {
            StreamReader sr = StreamReaderWarehouse();
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

        static void AddNewProductToWarehouse()
        {
            Console.Write("Podaj id produktu: ");
            string id = Console.ReadLine();
            if (FindProductIndexInWarehouse(id) >= 0)
            {
                Console.WriteLine("Taki produkt już istnieje w magazynie!");
                return;
            }
            Console.Write("Podaj ilość produktu: ");
            string quantity = Console.ReadLine();
            StreamWriter sw = StreamWriterWarehouse();
            sw.WriteLine(id + " " + quantity);
            sw.Close();
        }


        static void RemoveProductFromWarehouse()
        {
            string[] lines = File.ReadAllLines("warehouse.txt");
            Console.Write("Podaj id produktu: ");
            string id = Console.ReadLine();
            if (FindProductIndexInWarehouse(id) < 0)
            {
                Console.WriteLine("Nie ma produktu z podanym id");
                return;
            }
            lines = lines.Where((line, index) => index != FindProductIndexInWarehouse(id)).ToArray();

            File.WriteAllLines("warehouse.txt", lines);
        }
        static void ChangeTheQuantityOfProductInWarehouse()
        {
            string[] lines = File.ReadAllLines("warehouse.txt");
            int quantityToChange;
            string pattern = "^[1-2]$";
            string[] features = { "1. Dodawanie ilości produktu", "2. Odejmowanie ilości produktu"};
            string selectedFeature = "";

            Console.Write("Podaj id produktu: ");
            string id = Console.ReadLine();
            int productIndex = FindProductIndexInWarehouse(id);

            if (productIndex < 0)
            {
                Console.WriteLine("Nie ma produktu z podanym id");
                return;
            }

            foreach (var feature in features)
            {
                Console.WriteLine(feature);
            }

            try
            {
                Console.Write("Wybierz funkcjonalność podając odpowiednią liczbę: ");
                selectedFeature = Console.ReadLine();
                if (!Regex.IsMatch(selectedFeature, pattern))
                {
                    throw new ArgumentException();
                }
            }
            catch
            {
                while (!Regex.IsMatch(selectedFeature.ToString(), pattern))
                {
                    Console.WriteLine("Podana wartość jest nieprawidłowa!");
                    Console.Write("Wybierz funkcjonalność podając odpowiednią liczbę: ");
                    selectedFeature = Console.ReadLine();
                }
            }

            switch (selectedFeature)
            {
                case "1":
                    Console.WriteLine("Wybrano dodawanie ilości produktu");
                    Console.WriteLine("Podaj ile dodać ilości do stanu tego produktu: ");
                    try
                    {
                        quantityToChange = int.Parse(Console.ReadLine());
                    }
                    catch
                    {
                        Console.WriteLine("Podana wartość jest nieprawidłowa!");
                        return;
                    }
                    break;
                case "2":
                    Console.WriteLine("Wybrano odejmowanie ilości produktu");
                    Console.WriteLine("Podaj ile odjąć ilości od stanu tego produktu: ");
                    try
                    {
                        quantityToChange = int.Parse(Console.ReadLine()) * -1;
                    }
                    catch
                    {
                        Console.WriteLine("Podana wartość jest nieprawidłowa!");
                        return;
                    }

                    break;
                default:
                    Console.WriteLine("Błąd!");
                    return;
            }
            string[] line = lines[productIndex].Split(' ');
            if ((line[0]) == id)
            {
                line[1] = (int.Parse(line[1]) + quantityToChange).ToString();
                if (int.Parse(line[1]) < 0)
                {
                    Console.WriteLine("Ilość produktu nie może być mniejsza od 0");
                    return;
                }
                lines[productIndex] = line[0] + ' ' + line[1];
            }
            File.WriteAllLines("warehouse.txt", lines);
        }

        static string WarehouseValue()
        {
            BigInteger total = 0;
            int price;
            int quantity;
            string[] priceListLines = File.ReadAllLines("priceList.txt");
            string[] warehouseLines = File.ReadAllLines("warehouse.txt");
            if (priceListLines.Length == 0 || warehouseLines.Length == 0)
            {
                return total.ToString();
            }
            string[,] productsFromWarehouse = new string[warehouseLines.Length, 2];

            for (int i = 0; i < warehouseLines.Length; i++)
            {
                productsFromWarehouse[i, 0] = warehouseLines[i].Split(' ')[0];
                productsFromWarehouse[i, 1] = warehouseLines[i].Split(' ')[1];
            }

            for (int i = 0; i < warehouseLines.Length; i++)
            {
                string priceString = Array.Find(priceListLines, line => line.Split(' ')[0] == productsFromWarehouse[i, 0]).Split(' ')[1];
                price = priceString != null ? int.Parse(priceString) : 0;
                quantity = int.Parse(productsFromWarehouse[i, 1]);
                total += BigInteger.Multiply(price, quantity);
            }

            return total.ToString();
        }

        static void DisplayWarehouseContents()
        {
            int index = 1;
            string id;
            string quantity;
            string[] warehouseLines = File.ReadAllLines("warehouse.txt");
            if (warehouseLines.Length == 0)
            {
                Console.WriteLine("Magazyn jest pusty");
                return;
            }
            Dictionary<string, string> productQuantities = new Dictionary<string, string>();
            foreach (var line in warehouseLines)
            {
                id = line.Split(' ')[0];
                quantity = line.Split(' ')[1];
                productQuantities.Add(id, quantity);
            }
            foreach (KeyValuePair<string, string> product in productQuantities)
            {
                Console.WriteLine($"{index}. {product.Key} {product.Value} szt.");
                index++;
            }

        }

        static void FindProductInPriceList()
        {
            Console.Write("Podaj id produktu: ");
            string id = Console.ReadLine();
            StreamReader sr = StreamReaderPriceList();
            string line = "";
            while ((line = sr.ReadLine()) != null)
            {
                string[] elements = line.Split(' ');
                if (elements[0] == id)
                {
                    sr.Close();
                    Console.WriteLine(line + " zł");
                    return;
                }
            }
            Console.WriteLine("Nie znaleziono takiego produktu");
        }
        static int FindProductIndexInPriceList()
        {

            Console.Write("Podaj id produktu: ");
            string id = Console.ReadLine();
            StreamReader sr = StreamReaderPriceList();
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
            Console.WriteLine("Nie znaleziono takiego produktu");
            return -1;
        }

        static int FindProductIndexInPriceList(string id)
        {
            StreamReader sr = StreamReaderPriceList();
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

        static void AddNewProductToPriceList()
        {
            Console.WriteLine("Podaj id produktu: ");
            string id = Console.ReadLine();
            if (FindProductIndexInPriceList(id) >= 0)
            {
                Console.WriteLine("Taki produkt już istnieje w cenniku!");
                return;
            }
            Console.WriteLine("Podaj cenę produktu: ");
            string price = Console.ReadLine();
            StreamWriter sw = StreamWriterPriceList();
            sw.WriteLine(id + " " + price);
            sw.Close();
        }

        static void RemoveProductFromPriceList()
        {
            string[] lines = File.ReadAllLines("priceList.txt");
            Console.Write("Podaj id produktu: ");
            string id = Console.ReadLine();
            if (FindProductIndexInPriceList(id)<0)
            {
                Console.WriteLine("Nie znaleziono takiego produktu");
                return;
            }
            lines = lines.Where((line, index) => index != FindProductIndexInWarehouse(id)).ToArray();

            File.WriteAllLines("priceList.txt", lines);
        }

        static void ChangeThePriceOfProductInPriceList()
        {
            string[] lines = File.ReadAllLines("priceList.txt");
            List<string> linesList = new List<string>();
            foreach (var line in lines)
            {
                linesList.Add(line);
            }
            Console.Write("Podaj id produktu: ");
            string id = Console.ReadLine();
            if (FindProductIndexInPriceList(id)<0)
            {
                Console.WriteLine("Nie znaleziono takiego produktu");
                return;
            }
            Console.Write("Podaj nową cenę: ");
            string newPrice = Console.ReadLine();
            int indexToModifty = linesList.FindIndex(line => line.Split(' ')[0] == id);
            linesList[indexToModifty] = linesList[indexToModifty].Split(' ')[0] + " " + newPrice;
            lines = linesList.ToArray();
            File.WriteAllLines("priceList.txt", lines);
        }

        static void DisplayPriceListContents()
        {
            int index = 1;
            string id;
            string price;
            string[] priceListLines = File.ReadAllLines("priceList.txt");
            if (priceListLines.Length == 0)
            {
                Console.WriteLine("Cennik jest pusty");
                return;
            }
            Dictionary<string, string> productPrices = new Dictionary<string, string>();
            foreach (var line in priceListLines)
            {
                id = line.Split(' ')[0];
                price = line.Split(' ')[1];
                productPrices.Add(id, price);
            }
            foreach (KeyValuePair<string, string> product in productPrices)
            {
                Console.WriteLine($"{index}. {product.Key} {product.Value} zł");
                index++;
            }

        }

        static void CreatePriceList()
        {
            string path = @"priceList.txt";
            StreamWriter sw = File.CreateText(path);
            sw.Close();
        }
        static StreamReader StreamReaderPriceList()
        {
            string path = @"priceList.txt";
            StreamReader sr = File.OpenText(path);
            return sr;
        }

        static StreamWriter StreamWriterPriceList()
        {
            string path = @"priceList.txt";
            StreamWriter sw = new StreamWriter(path, true);
            return sw;
        }
        static void InitializeWarehouse()
        {
            string path = @"warehouse.txt";

            if (!File.Exists(path))
            {
                CreateWarehouse();
            }
        }

        static void InitializePriceList()
        {
            string path = @"priceList.txt";

            if (!File.Exists(path))
            {
                CreatePriceList();
            }

        }

        static void ListFeatures()
        {
            string pattern = "^[1-2]$";
            int selectedFeature = 0;
            foreach (var feature in new string[]{"1. Magazyn", "2. Cennik"})
            {
                Console.WriteLine(feature);
            }
            try
            {
                Console.Write("Wybierz funkcjonalność podając odpowiednią liczbę: ");
                selectedFeature = int.Parse(Console.ReadLine());
                if (!Regex.IsMatch(selectedFeature.ToString(), pattern))
                {
                    throw new ArgumentException();
                }
            }
            catch
            {
                while (!Regex.IsMatch(selectedFeature.ToString(), pattern))
                {
                    Console.WriteLine("Podana wartość jest nieprawidłowa!");
                    Console.Write("Wybierz funkcjonalność podając odpowiednią liczbę: ");
                    try
                    {
                        selectedFeature = int.Parse(Console.ReadLine());
                    }
                    catch
                    {
                        continue;
                    }
                }
            }
           
            
            Console.WriteLine();

            switch (selectedFeature)
            {
                case (int)Features.Warehouse:
                    pattern = "^[1-6]$";
                    selectedFeature = 0;
                    Console.WriteLine("Wybrano magazyn");
                    foreach (var feature in new string[]{"1. Dodawanie", "2. Usuwanie", "3. Szukanie", "4. Wartość magazynu", "5. Zmiana ilości produktu", "6. Wyświetlenie zawartości magazynu"})
                    {
                        Console.WriteLine(feature);
                    }

                    try
                    {
                        Console.Write("Wybierz funkcjonalność podając odpowiednią liczbę: ");
                        selectedFeature = int.Parse(Console.ReadLine());
                        if (!Regex.IsMatch(selectedFeature.ToString(), pattern))
                        {
                            throw new ArgumentException();
                        }
                    }
                    catch
                    {
                        while (!Regex.IsMatch(selectedFeature.ToString(), pattern))
                        {
                            Console.WriteLine("Podana wartość jest nieprawidłowa!");
                            Console.Write("Wybierz funkcjonalność podając odpowiednią liczbę: ");
                            try
                            {
                                selectedFeature = int.Parse(Console.ReadLine());
                            }
                            catch
                            {
                                continue;
                            }
                        }
                    }

                    Console.WriteLine();
                    switch (selectedFeature)
                    {
                        case (int)WarehouseFeatures.Adding:
                            Console.WriteLine("Wybrano dodawanie");
                            AddNewProductToWarehouse();
                            break;
                        case (int)WarehouseFeatures.Subtraction:
                            Console.WriteLine("Wybrano usuwanie");
                            RemoveProductFromWarehouse();
                            break;
                        case (int)WarehouseFeatures.Find:
                            Console.WriteLine("Wybrano szukanie");
                            FindProductInWarehouse();
                            break;
                        case (int)WarehouseFeatures.Value:
                            Console.WriteLine("Wybrano wartość magazynu");
                            Console.WriteLine("Wartość magazynu: " + WarehouseValue() + " zł");
                            break;
                        case (int)WarehouseFeatures.ChangeQuantity:
                            Console.WriteLine("Wybrano zmianę ilości produktu");
                            ChangeTheQuantityOfProductInWarehouse();
                            break;
                        case (int)WarehouseFeatures.DisplayWarehouseContents:
                            Console.WriteLine("Wybrano wyświetlenie zawartości magazynu");
                            DisplayWarehouseContents();
                            break;
                        default:
                            Console.WriteLine("Błąd!");
                            break;
                    }
                    break;
                case (int)Features.PriceList:
                    pattern = "^[1-5]$";
                    selectedFeature = 0;
                    Console.WriteLine("Wybrano cennik");
                    foreach (var feature in new string[]{"1. Dodawanie", "2. Usuwanie", "3. Szukanie", "4. Zmiana ceny", "5. Wyświetlenie zawartości cennika" })
                    {
                        Console.WriteLine(feature);
                    }

                    try
                    {
                        Console.Write("Wybierz funkcjonalność podając odpowiednią liczbę: ");
                        selectedFeature = int.Parse(Console.ReadLine());
                        if (!Regex.IsMatch(selectedFeature.ToString(), pattern))
                        {
                            throw new ArgumentException();
                        }
                    }
                    catch
                    {
                        while (!Regex.IsMatch(selectedFeature.ToString(), pattern))
                        {
                            Console.WriteLine("Podana wartość jest nieprawidłowa!");
                            Console.Write("Wybierz funkcjonalność podając odpowiednią liczbę: ");
                            try
                            {
                                selectedFeature = int.Parse(Console.ReadLine());
                            }
                            catch
                            {
                                continue;
                            }
                        }
                    }

                    Console.WriteLine();
                    switch (selectedFeature)
                    {
                        case (int)PriceListFeatures.Adding:
                            Console.WriteLine("Wybrano dodawanie");
                            AddNewProductToPriceList();
                            break;
                        case (int)PriceListFeatures.Subtraction:
                            Console.WriteLine("Wybrano usuwanie");
                            RemoveProductFromPriceList();
                            break;
                        case (int)PriceListFeatures.Find:
                            Console.WriteLine("Wybrano szukanie");
                            FindProductInPriceList();
                            break;
                        case (int)PriceListFeatures.ChangePrice:
                            Console.WriteLine("Wybrano zmianę ceny");
                            ChangeThePriceOfProductInPriceList();
                            break;
                        case (int)PriceListFeatures.DisplayPriceListContents:
                            Console.WriteLine("Wybrano wyświetlenie zawartości cennika");
                            DisplayPriceListContents();
                            break;
                        default:
                            Console.WriteLine("Błąd!");
                            break;
                    }
                    break;
                default:
                    Console.WriteLine("Błąd!");
                    break;
            }
        }

        static void EndOfApp()
        {
            Console.Write("Naciśnij dowolny klawisz...");
            Console.ReadKey();
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Dzień dobry, z tej strony marketMate!");
            InitializePriceList();
            InitializeWarehouse();
            while (true)
            {
                ListFeatures();
                string pattern = "^[1-2]$";
                string answer = "";
                Console.WriteLine();
                try
                {
                    Console.WriteLine("Czy chcesz zakończyć program?");
                    Console.WriteLine("1. Tak");
                    Console.WriteLine("2. Nie");
                    Console.Write("Wybierz funkcjonalność podając odpowiednią liczbę: ");
                    answer = Console.ReadLine();
                    if (!Regex.IsMatch(answer, pattern))
                    {
                        throw new ArgumentException();
                    }
                }
                catch
                {
                    while (!Regex.IsMatch(answer, pattern))
                    {
                        Console.WriteLine("Podana wartość jest nieprawidłowa!");
                        Console.Write("Wybierz funkcjonalność podając odpowiednią liczbę: ");
                        answer = Console.ReadLine();
                    }
                }
               
                if (answer == "1")
                {
                    break;
                }
                Console.Clear();
            }
            EndOfApp();

        }
    }
}
