using System;
using NUnit;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace testing.Tests
{
    [TestFixture]
    public class MainProgramTests
    {
        [Test]
        public void ConversionKilogramsToPounds_CorrectConversion()
        {
            // 2 кг -> 4.4092 фунта
            var result = MainProgram.ConversionKilogramsToPounds(2.0);
            Assert.That("2 килограмм это 4,4092 фунтов.", Is.EqualTo(result));
        }

        [Test]
        public void ConversionCentimetersToInches_CorrectConversion()
        {
            // 10 см -> 3.937 дюйма
            var result = MainProgram.ConversionCentimetersToInches(10.0);
            Assert.That("10 сантиметров это 3,937 дюймов.", Is.EqualTo(result));
        }

        [Test]
        public void FishInfo_ReturnsFormattedString()
        {
            var fish = new Fish("Лещ", 1.2, 30.0, "Озёрный лещ");
            var info = fish.FishInfo();
            Assert.That("Название: Лещ, Вес 1,2, Длина(См): 30, Описание: Озёрный лещ", Is.EqualTo(info));
        }

        [Test]
        public void ComparingNames_MatchesName_WritesFishInfo()
        {
            var fish = new Fish("Щука", 5.0, 60.0, "Хищница");
            using (var sw = new StringWriter())
            {
                Console.SetOut(sw);
                MainProgram.ComparingNames("Щука", fish);
                var output = sw.ToString().Trim();
                Assert.That(output.StartsWith("Ваша рыба: Название: Щука"));
            }
        }

        [Test]
        public void ComparingNames_NonMatchingName_WritesNotFound()
        {
            var fish = new Fish("Окунь", 0.5, 20.0, "Малый");
            using (var sw = new StringWriter())
            {
                Console.SetOut(sw);
                MainProgram.ComparingNames("Сазан", fish);
                var output = sw.ToString().Trim();
                Assert.That("Такой рыбы в списке нету", Is.EqualTo(output));
            }
        }
    }

    [TestFixture]
    public class InteractiveTests
    {
        [Test]
        public void Solve_WhenListEmpty_SelectingOption2_ShowsEmptyListMessage()
        {
            using (var input = new StringReader("2\n5\n"))
            using (var output = new StringWriter())
            {
                Console.SetIn(input);
                Console.SetOut(output);

                MainProgram.Solve();
                var text = output.ToString();
                StringAssert.Contains("Список пуст", text);
            }
        }

        [Test]
        public void Solve_CreateAndListFish()
        {
            // Создаем рыбу, затем выводим полный список
            string commands =
                "0\n" +          // Создание
                "Карп\n" +      // name
                "3,5\n" +       // weight
                "45\n" +        // length
                "Речной карп\n" + // description
                "2\n" +         // Вывести список
                "5\n";          // Остановиться

            using (var input = new StringReader(commands))
            using (var output = new StringWriter())
            {
                Console.SetIn(input);
                Console.SetOut(output);

                MainProgram.Solve();
                var text = output.ToString();
                StringAssert.Contains("Название: Карп, Вес 3,5, Длина(См): 45, Описание: Речной карп", text);
            }
        }
    }
}
