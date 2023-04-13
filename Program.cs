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
        enum features
        {
            Warehouse = 1,
            priceList = 2
        }
        enum warehouseFeatures
        {
            Adding = 1,
            Subtraction = 2,
            Find = 3,
            Value = 4,
            ChangeQuantity = 5
        }
        enum priceListFeatures
        {
            Adding = 1,
            Subtraction = 2,
            Find = 3,
            ChangePrice = 4
        }
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

        static void findProductInWarehouse()
        {
            Console.Write("Podaj id produktu: ");
            string id = Console.ReadLine();
            StreamReader sr = streamReaderWarehouse();
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
        static int findProductIndexInWarehouse()
        {

            Console.Write("Podaj id produktu: ");
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
            Console.WriteLine("Nie znaleziono takiego produktu");
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
            Console.Write("Podaj id produktu: ");
            string id = Console.ReadLine();
            if (findProductIndexInWarehouse(id) >= 0)
            {
                Console.WriteLine("Taki produkt już istnieje w magazynie!");
                return;
            }
            Console.Write("Podaj ilość produktu: ");
            string quantity = Console.ReadLine();
            StreamWriter sw = streamWriterWarehouse();
            sw.WriteLine(id + " " + quantity);
            sw.Close();
        }


        static void removeProductFromWarehouse()
        {
            string[] lines = File.ReadAllLines("warehouse.txt");
            Console.Write("Podaj id produktu: ");
            string id = Console.ReadLine();
            if (findProductIndexInWarehouse(id) < 0)
            {
                Console.WriteLine("Nie ma produktu z podanym id");
                return;
            }
            lines = lines.Where((line, index) => index != findProductIndexInWarehouse(id)).ToArray();

            File.WriteAllLines("warehouse.txt", lines);
        }
        static void changeTheQuantityOfProductInWarehouse()
        {
            string[] lines = File.ReadAllLines("warehouse.txt");
            int quantityToChange;
            string pattern = "^[1-2]$";
            string[] features = { "1. Dodawanie ilości produktu", "2. Odejmowanie ilości produktu"};
            string selectedFeature = "";

            Console.Write("Podaj id produktu: ");
            string id = Console.ReadLine();
            int productIndex = findProductIndexInWarehouse(id);

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

        static string warehouseValue()
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

        static void findProductInPriceList()
        {
            Console.Write("Podaj id produktu: ");
            string id = Console.ReadLine();
            StreamReader sr = streamReaderPriceList();
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
        static int findProductIndexInPriceList()
        {

            Console.Write("Podaj id produktu: ");
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
            Console.WriteLine("Nie znaleziono takiego produktu");
            return -1;
        }

        static int findProductIndexInPriceList(string id)
        {
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
            if (findProductIndexInPriceList(id) >= 0)
            {
                Console.WriteLine("Taki produkt już istnieje w cenniku!");
                return;
            }
            Console.WriteLine("Podaj cenę produktu: ");
            string price = Console.ReadLine();
            StreamWriter sw = streamWriterPriceList();
            sw.WriteLine(id + " " + price);
            sw.Close();
        }

        static void removeProductFromPriceList()
        {
            string[] lines = File.ReadAllLines("priceList.txt");
            Console.Write("Podaj id produktu: ");
            string id = Console.ReadLine();
            if (findProductIndexInPriceList(id)<0)
            {
                Console.WriteLine("Nie znaleziono takiego produktu");
                return;
            }
            lines = lines.Where((line, index) => index != findProductIndexInWarehouse(id)).ToArray();

            File.WriteAllLines("priceList.txt", lines);
        }

        static void changeThePriceOfProductInPriceList()
        {
            string[] lines = File.ReadAllLines("priceList.txt");
            List<string> linesList = new List<string>();
            foreach (var line in lines)
            {
                linesList.Add(line);
            }
            Console.Write("Podaj id produktu: ");
            string id = Console.ReadLine();
            if (findProductIndexInPriceList(id)<0)
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

            if (!File.Exists(path))
            {
                createWarehouse();
            }
        }

        static void initializePriceList()
        {
            string path = @"priceList.txt";

            if (!File.Exists(path))
            {
                createPriceList();
            }

        }

        static void listFeatures()
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
                case (int)features.Warehouse:
                    pattern = "^[1-5]$";
                    selectedFeature = 0;
                    Console.WriteLine("Wybrano magazyn");
                    foreach (var feature in new string[]{"1. Dodawanie", "2. Usuwanie", "3. Szukanie", "4. Wartość magazynu", "5. Zmiana ilości produktu"})
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
                        case (int)warehouseFeatures.Adding:
                            Console.WriteLine("Wybrano dodawanie");
                            addNewProductToWarehouse();
                            break;
                        case (int)warehouseFeatures.Subtraction:
                            Console.WriteLine("Wybrano usuwanie");
                            removeProductFromWarehouse();
                            break;
                        case (int)warehouseFeatures.Find:
                            Console.WriteLine("Wybrano szukanie");
                            findProductInWarehouse();
                            break;
                        case (int)warehouseFeatures.Value:
                            Console.WriteLine("Wybrano wartość magazynu");
                            Console.WriteLine("Wartość magazynu: " + warehouseValue() + " zł");
                            break;
                        case (int)warehouseFeatures.ChangeQuantity:
                            Console.WriteLine("Wybrano zmianę ilości produktu");
                            changeTheQuantityOfProductInWarehouse();
                            break;
                        default:
                            Console.WriteLine("Błąd!");
                            break;
                    }
                    break;
                case (int)features.priceList:
                    pattern = "^[1-4]$";
                    selectedFeature = 0;
                    Console.WriteLine("Wybrano cennik");
                    foreach (var feature in new string[]{"1. Dodawanie", "2. Usuwanie", "3. Szukanie", "4. Zmiana ceny"})
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
                        case (int)priceListFeatures.Adding:
                            Console.WriteLine("Wybrano dodawanie");
                            addNewProductToPriceList();
                            break;
                        case (int)priceListFeatures.Subtraction:
                            Console.WriteLine("Wybrano usuwanie");
                            removeProductFromPriceList();
                            break;
                        case (int)priceListFeatures.Find:
                            Console.WriteLine("Wybrano szukanie");
                            findProductInPriceList();
                            break;
                        case (int)priceListFeatures.ChangePrice:
                            Console.WriteLine("Wybrano zmianę ceny");
                            changeThePriceOfProductInPriceList();
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

        static void endOfApp()
        {
            Console.Write("Naciśnij dowolny klawisz...");
            Console.ReadKey();
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Dzień dobry, z tej strony marketMate!");
            initializePriceList();
            initializeWarehouse();
            while (true)
            {
                listFeatures();
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
            endOfApp();

        }
    }
}
