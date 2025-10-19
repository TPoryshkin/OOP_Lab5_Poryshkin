﻿using System;
using System.Collections.Generic;
using System.Linq;

class Program
{
    static List<Plant> plants = new List<Plant>();
    static int maxPlants;
    static Random random = new Random();

    static void Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        InitializeMaxPlants();
        ShowMenu();
    }

    static void InitializeMaxPlants()
    {
        while (true)
        {
            Console.Write("Введіть максимальну кількість рослин (N > 0): ");
            if (int.TryParse(Console.ReadLine(), out maxPlants) && maxPlants > 0)
                break;
            Console.WriteLine("Помилка: Введіть коректне ціле число більше 0.");
        }
    }

    static void ShowMenu()
    {
        while (true)
        {
            Console.WriteLine("\n--- Меню управління колекцією рослин ---");
            Console.WriteLine("1 - Додати рослину");
            Console.WriteLine("2 - Переглянути всі рослини");
            Console.WriteLine("3 - Знайти рослину");
            Console.WriteLine("4 - Продемонструвати поведінку");
            Console.WriteLine("5 - Видалити рослину");
            Console.WriteLine("6 - Продемонструвати static-методи");
            Console.WriteLine("0 - Вийти з програми");
            Console.Write("Ваш вибір: ");

            string choice = Console.ReadLine();
            switch (choice)
            {
                case "1": AddPlant(); break;
                case "2": ViewAllPlants(); break;
                case "3": FindPlant(); break;
                case "4": DemonstrateBehavior(); break;
                case "5": DeletePlant(); break;
                case "6": DemonstrateStaticMethods(); break;
                case "0": return;
                default: Console.WriteLine("Некоректний вибір. Спробуйте ще."); break;
            }
        }
    }

    static void AddPlant()
    {
        if (plants.Count >= maxPlants)
        {
            Console.WriteLine($"Досягнуто максимум ({maxPlants}) рослин. Додавання неможливе.");
            return;
        }

        Console.WriteLine("\n--- Додавання нової рослини ---");
        Console.WriteLine("Оберіть спосіб створення:");
        Console.WriteLine("1 - Вручну (основний конструктор)");
        Console.WriteLine("2 - Випадковий конструктор");
        Console.WriteLine("3 - Через рядок (парсинг)");
        Console.Write("Ваш вибір: ");

        string choice = Console.ReadLine();
        try
        {
            Plant newPlant;
            if (choice == "1")
            {
                newPlant = CreatePlantManually();
            }
            else if (choice == "2")
            {
                newPlant = CreatePlantRandom();
            }
            else if (choice == "3")
            {
                newPlant = CreatePlantFromString();
            }
            else
            {
                Console.WriteLine("Некоректний вибір.");
                return;
            }

            plants.Add(newPlant);
            Plant.AddPlantToCollection(newPlant);
            Console.WriteLine($"Рослина '{newPlant.Name}' успішно додана!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Помилка при додаванні: {ex.Message}");
        }
    }

    static Plant CreatePlantFromString()
    {
        Console.Write("Введіть дані рослини у форматі: Назва,Тип,Вік,Висота,Дата посадки (рррр-мм-дд),Цвітуча (True/False): ");
        Console.WriteLine("\nПриклад: Троянда,Flower,2,0.5,2023-05-15,True");
        Console.Write("Введіть дані: ");
        string input = Console.ReadLine();

        if (Plant.TryParse(input, out Plant plant))
        {
            return plant;
        }
        else
        {
            throw new Exception("Не вдалося створити рослину з введеного рядка.");
        }
    }

    static Plant CreatePlantManually()
    {
        Console.Write("Назва: ");
        string name = Console.ReadLine();

        Console.WriteLine("Тип (0-Дерево, 1-Кущ, 2-Квітка, 3-Трава, 4-Папороть, 5-Кактус, 6-Ліана): ");
        PlantType type = (PlantType)GetValidatedInput("тип", 0, 6);

        Console.Write("Вік (роки): ");
        int age = GetValidatedInput("вік", 0, 5000);

        Console.Write("Висота (метри): ");
        double height = GetValidatedDoubleInput("висота", 0.01, 115.7);

        Console.Write("Дата посадки (рррр-мм-дд): ");
        DateTime plantingDate = GetValidatedDate();

        Console.Write("Цвітуча рослина (так/ні): ");
        bool isFlowering = Console.ReadLine().ToLower() == "так";

        Plant plant = new Plant(name, type, age, height, plantingDate);
        plant.IsFlowering = isFlowering;

        return plant;
    }

    static Plant CreatePlantRandom()
    {
        int constructorType = random.Next(0, 4);
        Plant newPlant;

        string[] plantNames = { "Дуб", "Троянда", "Кактус", "Береза", "Лаванда", "Папороть", "Плющ", "Ялина" };
        string randomName = plantNames[random.Next(plantNames.Length)];

        switch (constructorType)
        {
            case 0:
                Console.WriteLine("Створення за допомогою конструктора без параметрів...");
                newPlant = new Plant();
                newPlant.Name = randomName;
                break;

            case 1:
                Console.WriteLine("Створення за допомогою конструктора з 2 параметрами (ім'я та тип)...");
                PlantType randomType = (PlantType)random.Next(0, 7);
                newPlant = new Plant(randomName, randomType);
                break;

            case 2:
                Console.WriteLine("Створення за допомогою конструктора з 2 параметрами (ім'я та вік)...");
                int randomAge = random.Next(1, 20);
                newPlant = new Plant(randomName, randomAge);
                break;

            case 3:
                Console.WriteLine("Створення за допомогою основного конструктора...");
                PlantType type = (PlantType)random.Next(0, 7);
                int age = random.Next(1, 50);
                double height = Math.Round(random.NextDouble() * 10 + 0.1, 2);
                DateTime plantingDate = DateTime.Now.AddYears(-age).AddDays(-random.Next(0, 365));
                newPlant = new Plant(randomName, type, age, height, plantingDate);
                break;

            default:
                newPlant = new Plant();
                break;
        }

        newPlant.IsFlowering = random.Next(0, 2) == 1;
        return newPlant;
    }

    static int GetValidatedInput(string fieldName, int min, int max)
    {
        while (true)
        {
            if (int.TryParse(Console.ReadLine(), out int value) && value >= min && value <= max)
                return value;
            Console.Write($"Помилка: {fieldName} повинен бути від {min} до {max}. Спробуйте ще: ");
        }
    }

    static double GetValidatedDoubleInput(string fieldName, double min, double max)
    {
        while (true)
        {
            if (double.TryParse(Console.ReadLine(), out double value) && value >= min && value <= max)
                return value;
            Console.Write($"Помилка: {fieldName} повинна бути від {min} до {max}. Спробуйте ще: ");
        }
    }

    static DateTime GetValidatedDate()
    {
        while (true)
        {
            if (DateTime.TryParse(Console.ReadLine(), out DateTime date))
            {
                try
                {
                    Plant testPlant = new Plant("Test", PlantType.Tree, 1, 1.0, date);
                    return date;
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine($"Помилка: {ex.Message}");
                }
            }
            else
            {
                Console.Write("Помилка: Некоректний формат дати. Використовуйте рррр-мм-дд: ");
            }
        }
    }

    static void ViewAllPlants()
    {
        if (plants.Count == 0)
        {
            Console.WriteLine("Колекція рослин порожня.");
            return;
        }

        Console.WriteLine("\n--- Всі рослини ---");
        Console.WriteLine("{0,-3} {1,-20} {2,-10} {3,-8} {4,-12} {5,-12} {6,-10} {7,-15}",
                          "#", "Назва", "Тип", "Вік", "Висота", "Посаджено", "Цвітуча", "Категорія");
        for (int i = 0; i < plants.Count; i++)
        {
            var p = plants[i];
            Console.WriteLine("{0,-3} {1,-20} {2,-10} {3,-8} {4,-12:F2} {5,-12:yyyy-MM-dd} {6,-10} {7,-15}",
                              i + 1, p.Name, p.Type, p.Age, p.Height, p.PlantingDate,
                              p.IsFlowering ? "Так" : "Ні", p.AgeCategory);
        }

        Console.WriteLine($"\nЗагальна кількість рослин: {Plant.Count}");
        Console.WriteLine($"Середня висота рослин: {Plant.AverageHeight:F2} м");
    }

    static void FindPlant()
    {
        if (plants.Count == 0)
        {
            Console.WriteLine("Колекція рослин порожня.");
            return;
        }

        Console.WriteLine("\n--- Пошук рослин ---");
        Console.WriteLine("1 - По назві");
        Console.WriteLine("2 - По типу");
        Console.WriteLine("3 - По категорії віку");
        Console.Write("Виберіть критерій пошуку: ");

        string choice = Console.ReadLine();
        List<Plant> results = new List<Plant>();

        switch (choice)
        {
            case "1":
                Console.Write("Введіть назву для пошуку: ");
                string nameQuery = Console.ReadLine();
                results = plants.Where(p => p.Name.Contains(nameQuery, StringComparison.OrdinalIgnoreCase)).ToList();
                break;

            case "2":
                Console.WriteLine("Тип (0-Дерево, 1-Кущ, 2-Квітка, 3-Трава, 4-Папороть, 5-Кактус, 6-Ліана): ");
                PlantType typeQuery = (PlantType)GetValidatedInput("тип", 0, 6);
                results = plants.Where(p => p.Type == typeQuery).ToList();
                break;

            case "3":
                Console.WriteLine("Категорія віку (1-Молода, 2-Доросла, 3-Стара): ");
                int categoryChoice = GetValidatedInput("категорія", 1, 3);
                string category = categoryChoice == 1 ? "Молода" : (categoryChoice == 2 ? "Доросла" : "Стара");
                results = plants.Where(p => p.AgeCategory == category).ToList();
                break;

            default:
                Console.WriteLine("Некоректний вибір.");
                return;
        }

        DisplaySearchResults(results);
    }

    static void DisplaySearchResults(List<Plant> results)
    {
        if (results.Count == 0)
        {
            Console.WriteLine("Рослин не знайдено.");
            return;
        }

        Console.WriteLine("\n--- Результати пошуку ---");
        Console.WriteLine("{0,-3} {1,-20} {2,-10} {3,-8} {4,-12} {5,-12} {6,-10} {7,-15}",
                          "#", "Назва", "Тип", "Вік", "Висота", "Посаджено", "Цвітуча", "Категорія");
        for (int i = 0; i < results.Count; i++)
        {
            var p = results[i];
            Console.WriteLine("{0,-3} {1,-20} {2,-10} {3,-8} {4,-12:F2} {5,-12:yyyy-MM-dd} {6,-10} {7,-15}",
                              i + 1, p.Name, p.Type, p.Age, p.Height, p.PlantingDate,
                              p.IsFlowering ? "Так" : "Ні", p.AgeCategory);
        }
    }

    static void DemonstrateBehavior()
    {
        if (plants.Count == 0)
        {
            Console.WriteLine("Колекція рослин порожня.");
            return;
        }

        ViewAllPlants();
        Console.Write("Виберіть номер рослини для демонстрації: ");
        if (int.TryParse(Console.ReadLine(), out int index) && index > 0 && index <= plants.Count)
        {
            Plant plant = plants[index - 1];
            Console.WriteLine($"\n--- Демонстрація поведінки для {plant.Name} ---");

            Console.WriteLine(plant.GetDescription());
            Console.WriteLine(plant.GetPlantingInfo());
            Console.WriteLine($"Доросла рослина: {(plant.IsMature() ? "Так" : "Ні")}");
            Console.WriteLine($"Категорія за віком: {plant.AgeCategory}");
            Console.WriteLine($"Цвітуча рослина: {(plant.IsFlowering ? "Так" : "Ні")}");
            Console.WriteLine($"Останній полив: {plant.LastWatered}");

            Console.WriteLine("\n--- Демонстрація перевантажених методів ---");

            Console.WriteLine("\n1. Демонстрація перевантажених методів Grow:");
            Console.WriteLine("Базовий метод Grow:");
            plant.Grow(0.5);

            Console.WriteLine("\nПеревантажений метод Grow() без параметрів:");
            plant.Grow();

            Console.WriteLine("\nПеревантажений метод Grow(int years):");
            plant.Grow(2);

            Console.WriteLine("\n2. Демонстрація перевантажених методів WaterPlant:");
            Console.WriteLine("Базовий метод WaterPlant():");
            plant.WaterPlant();

            Console.WriteLine("\nПеревантажений метод WaterPlant(string waterType):");
            plant.WaterPlant("дощова");

            Console.WriteLine("\nПеревантажений метод WaterPlant(int milliliters):");
            plant.WaterPlant(500);

            Console.WriteLine("\nПеревантажений метод WaterPlant(string waterType, int milliliters):");
            plant.WaterPlant("мінеральна", 300);
        }
        else
        {
            Console.WriteLine("Некоректний номер рослини.");
        }
    }

    static void DeletePlant()
    {
        if (plants.Count == 0)
        {
            Console.WriteLine("Колекція рослин порожня.");
            return;
        }

        Console.WriteLine("\n--- Видалення рослин ---");
        Console.WriteLine("1 - За порядковим номером");
        Console.WriteLine("2 - За назвою");
        Console.Write("Виберіть спосіб видалення: ");

        string choice = Console.ReadLine();
        switch (choice)
        {
            case "1":
                ViewAllPlants();
                Console.Write("Введіть номер рослини для видалення: ");
                if (int.TryParse(Console.ReadLine(), out int index) && index > 0 && index <= plants.Count)
                {
                    Plant removed = plants[index - 1];
                    plants.RemoveAt(index - 1);
                    Plant.RemovePlantFromCollection(removed);
                    Console.WriteLine($"Рослина '{removed.Name}' була успішно видалена.");
                }
                else
                {
                    Console.WriteLine("Некоректний номер.");
                }
                break;

            case "2":
                Console.Write("Введіть назву для видалення: ");
                string nameToDelete = Console.ReadLine();
                var toRemove = plants.Where(p => p.Name.Equals(nameToDelete, StringComparison.OrdinalIgnoreCase)).ToList();

                if (toRemove.Count > 0)
                {
                    foreach (var plant in toRemove)
                    {
                        Plant.RemovePlantFromCollection(plant);
                    }
                    plants.RemoveAll(p => p.Name.Equals(nameToDelete, StringComparison.OrdinalIgnoreCase));
                    Console.WriteLine($"Видалено {toRemove.Count} рослин з назвою '{nameToDelete}'.");
                }
                else
                {
                    Console.WriteLine("Рослин з такою назвою не знайдено.");
                }
                break;

            default:
                Console.WriteLine("Некоректний вибір.");
                break;
        }
    }

    static void DemonstrateStaticMethods()
    {
        Console.WriteLine("\n--- Демонстрація static-методів ---");

        Console.WriteLine($"\n1. Статичні властивості:");
        Console.WriteLine($"Загальна кількість рослин: {Plant.Count}");
        Console.WriteLine($"Середня висота рослин: {Plant.AverageHeight:F2} м");

        Console.WriteLine($"\n2. Статичний метод GetGlobalPlantInfo():");
        Console.WriteLine(Plant.GetGlobalPlantInfo());

        Console.WriteLine($"\n3. Демонстрація методів Parse та TryParse:");

        string correctString = "Троянда,Flower,2,0.5,2023-05-15,True";
        Console.WriteLine($"Коректний рядок: {correctString}");

        try
        {
            Plant parsedPlant = Plant.Parse(correctString);
            Console.WriteLine("Метод Parse успішно створив рослину:");
            Console.WriteLine($"Назва: {parsedPlant.Name}, Тип: {parsedPlant.Type}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Помилка Parse: {ex.Message}");
        }

        Console.WriteLine($"\nДемонстрація TryParse з коректним рядком:");
        if (Plant.TryParse(correctString, out Plant tryParsedPlant))
        {
            Console.WriteLine("TryParse успішно створив рослину:");
            Console.WriteLine($"Назва: {tryParsedPlant.Name}, Вік: {tryParsedPlant.Age}");
        }
        else
        {
            Console.WriteLine("TryParse не вдалось створити рослину");
        }

        string incorrectString = "Неправильний,рядок";
        Console.WriteLine($"\nДемонстрація TryParse з некоректним рядком: {incorrectString}");
        if (Plant.TryParse(incorrectString, out Plant invalidPlant))
        {
            Console.WriteLine("TryParse успішно створив рослину (неочікувано)");
        }
        else
        {
            Console.WriteLine("TryParse не вдалось створити рослину (очікувана поведінка)");
        }

        Console.WriteLine($"\n4. Демонстрація методу ToString():");
        if (plants.Count > 0)
        {
            Plant samplePlant = plants[0];
            string plantString = samplePlant.ToString();
            Console.WriteLine($"Результат ToString(): {plantString}");

            if (Plant.TryParse(plantString, out Plant recreatedPlant))
            {
                Console.WriteLine("Рядок з ToString() успішно перетворено назад у рослину!");
                Console.WriteLine($"Перевірка: {recreatedPlant.Name}, {recreatedPlant.Type}, {recreatedPlant.Age} років");
            }
        }

        Console.WriteLine("\nДемонстрація завершена. Лічильник не змінився, оскільки демонстраційні рослини не додавались до колекції.");
    }
}
