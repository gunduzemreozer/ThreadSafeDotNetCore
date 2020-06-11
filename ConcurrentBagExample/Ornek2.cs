using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
 * Bir başka örnek vermek gerekirse, sıradan koleksiyon kullanılan operasyonlarda kaynak değişmesi durumunda alınacak olası hataları ConcurrentBag ile rahatlıkla aşabildiğimiz üzerine verelim.
*/
namespace ConcurrentBagExample
{
    public class Ornek2
    {
        static ConcurrentBag<int> sayilar = new ConcurrentBag<int>();
        static int counter = 0;

        static void Main(string[] args)
        {
            var t1 = Task.Run(() =>
            {
                for (int i = 1; i <= 10; i++)
                {
                    sayilar.Add(i);
                }
                    
                int sayac = 0;
                while (!sayilar.IsEmpty)
                {
                    if (sayilar.TryTake(out int data))
                    {
                        counter++;
                        Console.WriteLine($"{data}");
                    }
                }
            });
            
            var t2 = Task.Run(() =>
            {
                for (int i = 11; i <= 20; i++)
                {
                    sayilar.Add(i);
                }
            });

            Task.WhenAll(t1, t2).Wait();
            Console.WriteLine($"sayilar counter: {counter}");
            Console.Read();
        }
    }
}
