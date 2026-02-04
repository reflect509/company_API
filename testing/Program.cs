using testing;

namespace testing
{
    internal class Program
    {
        static void Main(string[] args)
        {
           MainProgram.Solve();
        }        

    }
    public class MainProgram
    {
        public static void Solve()
        {
            List<Fish> FishList = new List<Fish>();

            string name;
            double weight;
            double lengthCentimeters;
            string description;

            Console.WriteLine($" - - - Номера пунктов - - - ");
            Console.WriteLine($"0 - Остановка программы\n" +
                $"1 - Перевести сантиметры нужной рыбы в дюймы\n" +
                $"2 - Вывести весь список экземпляров с названиями рыб\n" +
                $"3 - Перевести вес нужной рыбы в фунты\n" +
                $"4 - Вывести информацию о нужной рыбе/внести изменения в экземпляр\n" +
                $"5 - Создать новый экземпляр рыбы");

            bool boolValue = true;
            while (boolValue)
            {
                Console.WriteLine($"\n - - - Выберите пункт - - - ");
                Console.Write($"Ваш ввод: ");
                int userInput = Convert.ToInt32(Console.ReadLine());
                switch (userInput)
                {
                    case 0:
                        Console.WriteLine("\n - Создание нового экземпляра - ");

                        Console.Write("Введите название рыбы: ");
                        name = Console.ReadLine();
                        Console.Write("Введите вес рыбы(КГ): ");
                        weight = Convert.ToDouble(Console.ReadLine());
                        Console.Write("Введите длину рыбы(СМ): ");
                        lengthCentimeters = Convert.ToDouble(Console.ReadLine());
                        Console.Write("Введите комментарий: ");
                        description = Console.ReadLine();

                        Fish fish = new Fish(name, weight, lengthCentimeters, description);
                        FishList.Add(fish);

                        Console.WriteLine($"Вы создали рыбу {fish.FishInfo()}");

                        break;
                    case 1:
                        Console.WriteLine("\n - Вывод информации о рыбе - ");

                        Console.Write("Введите название рыбы: ");
                        string name1 = Console.ReadLine();

                        if (FishList.Count != 0)
                        {
                            foreach (Fish f in FishList)
                            {
                                ComparingNames(name1, f);
                            }
                        }
                        else
                        {
                            Console.WriteLine("Список пуст");
                        }
                        break;

                    case 2:
                        Console.WriteLine("\n - Вывод информации о рыбе - ");
                        if (FishList.Count != 0)
                        {
                            foreach (Fish f in FishList)
                            {
                                Console.WriteLine(f.FishInfo());
                            }
                        }
                        else
                        {
                            Console.WriteLine("Список пуст");
                        }
                        break;
                    case 3:
                        Console.WriteLine("\n - Перевод веса нужной рыбы в фунты - ");

                        Console.Write("Введите название рыбы: ");
                        string name3 = Console.ReadLine();

                        if (FishList.Count != 0)
                        {
                            foreach (Fish f in FishList)
                            {
                                if (name3 == f.Name)
                                {
                                    ComparingNames(name3, f);
                                    Console.WriteLine($"Перевод веса этой рыбы: {ConversionKilogramsToPounds(f.Weight)}");
                                }
                                else
                                {
                                    Console.Write($"Такой рыбы в списке нету");
                                }
                            }
                        }
                        else
                        {
                            Console.WriteLine("Список пуст");
                        }
                        break;
                    case 4:
                        Console.WriteLine("\n - Перевод веса нужной рыбы в фунты - ");

                        Console.Write("Введите название рыбы: ");
                        string name4 = Console.ReadLine();

                        if (FishList.Count != 0)
                        {
                            foreach (Fish f in FishList)
                            {
                                if (name4 == f.Name)
                                {
                                    ComparingNames(name4, f);
                                    Console.WriteLine($"Перевод длины этой рыбы: {ConversionCentimetersToInches(f.LengthCentimeters)}");
                                }
                                else
                                {
                                    Console.Write($"Такой рыбы в списке нету");
                                }
                            }
                        }
                        else
                        {
                            Console.WriteLine("Список пуст");
                        }

                        break;

                    case 5:
                        Stop(ref boolValue);
                        break;
                }
            }
        }
        static public string ConversionKilogramsToPounds(double kilograms)
        {
            double pounds = kilograms * 2.2046;

            return $"{kilograms} килограмм это {pounds} фунтов.";
        }

        static public string ConversionCentimetersToInches(double centimeters)
        {
            double inches = centimeters * 0.3937;
            return $"{centimeters} сантиметров это {inches} дюймов.";
        }

        static public void ComparingNames(string name1, Fish f)
        {
            if (name1 == f.Name)
            {
                Console.Write($"Ваша рыба: {f.FishInfo()}\n");
            }
            else
            {
                Console.Write($"Такой рыбы в списке нету");
            }
        }

        static public void Stop(ref bool boolValue)
        {
            Console.WriteLine("\n - Остановка программы - ");
            boolValue = false;
        }

    }
}
