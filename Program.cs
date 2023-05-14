using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace final_project_pysachuk
{
    class Program
    {
        static void Main(string[] args)
        {
            // Зчитуємо дані з файлу
            var stores = new List<JewelryStore>();
            using (var reader = new StreamReader("data.txt"))
            {
                while (!reader.EndOfStream)
                {
                    var store = new JewelryStore();
                    store.Address = reader.ReadLine();
                    store.Items = new List<JewelryItem>();

                    var count = int.Parse(reader.ReadLine());
                    for (int i = 0; i < count; i++)
                    {
                        var line = reader.ReadLine().Split(',');
                        var item = new JewelryItem();
                        item.Name = line[0];
                        item.Metal = line[1];
                        item.Weight = int.Parse(line[2]);
                        if (int.TryParse(line[2], out int weight))
                        {
                            item.Weight = weight;
                        }
                        else
                        {
                            // Обробити помилку перетворення рядка на тип double
                        }

                        item.Price = decimal.Parse(line[3]);
                        store.Items.Add(item);
                    }

                    stores.Add(store);
                }
            }

            // Формуємо список металів та кількість виробів з кожного магазину
            var metals = new Dictionary<string, int>();
            foreach (var store in stores)
            {
                foreach (var item in store.Items)
                {
                    if (!metals.ContainsKey(item.Metal))
                        metals[item.Metal] = 0;
                    metals[item.Metal]++;
                }
            }

            // Записуємо список металів у файл
            using (var writer = new StreamWriter("metals.txt"))
            {
                foreach (var metal in metals)
                {
                    writer.WriteLine($"{metal.Key}: {metal.Value}");
                }
            }

            // Фільтруємо магазини за загальною сумою виробів
            var filteredStores = stores.Where(store => store.Items.Sum(item => item.Price) >= 500);

            // Формуємо список всіх прикрас з відфільтрованих магазинів
            var allItems = new List<JewelryItem>();
            foreach (var store in filteredStores)
            {
                allItems.AddRange(store.Items);
            }

            // Сортуємо список за назвою прикраси та записуємо у файл
            allItems.Sort((x, y) => x.Name.CompareTo(y.Name));
            using (var writer = new StreamWriter("items.txt"))
            {
                foreach (var item in allItems)
                {
                    writer.WriteLine($"{item.Name}, {item.Metal}, {item.Weight}, {item.Price}");
                }
            }
        }
    }
}
