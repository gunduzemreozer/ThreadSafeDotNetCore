using System;
using System.Collections.Generic;
using System.Threading.Tasks;

/*
 * TSK bünyesinde 100 kişilik bir grubu dağılım dengesini gözetmeksizin rastgele bir şekilde 
 * iki bölüğe ayırmak istiyoruz.
 */
namespace ConcurrentDictionaryExample
{
    class HataliKukkanim_Dictionary
    {
        static Dictionary<int, string> askerler;

        static void Main2(string[] args)
        {
            askerler = new Dictionary<int, string>();
            AtamaGerceklestir().Wait();

            foreach(var asker in askerler)
            {
                Console.WriteLine($"{asker.Key} - {asker.Value}");
            }

            Console.WriteLine(askerler.Count);
            Console.ReadLine();
        }

        static async Task Ata(string bolukAdi)
        {
            for(int i = 1; i <= 100; i++)
            {
                await Task.Delay(10);
                if (!askerler.ContainsKey(i))
                {
                    askerler.TryAdd(i, bolukAdi);
                }
            }
        }

        static async Task AtamaGerceklestir()
        {
            var task1 = Ata("1. Bölük");
            var task2 = Ata("2. Bölük");

            await Task.WhenAll(task1, task2);
        }
    }
}
