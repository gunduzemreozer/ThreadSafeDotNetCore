using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
 * Bir başka örnek vermek gerekirse, sıradan koleksiyon kullanılan operasyonlarda kaynak değişmesi durumunda alınacak olası hataları ConcurrentBag ile rahatlıkla aşabildiğimiz üzerine verelim.
*/
namespace ConcurrentBagExample
{
    public class Ornek2Hatali_List
    {
        static List<int> sayilar = new List<int>();

        static void MainOrnek2(string[] args)
        {
            var t1 = Task.Run(() =>
            {
                for (int i = 1; i <= 10; i++)
                {
                    sayilar.Add(i);
                }
                    
                int sayac = 0;
                while (sayac < sayilar.Count)
                {
                    Console.WriteLine($"{sayilar[sayac++]}");
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
            Console.WriteLine($"sayilar.Count: {sayilar.Count}");
            Console.Read();
        }
        /*
         Yukarıdaki asenkron çalışma neticesinde her iki threadin kullandığı kaynak olan List koleksiyonu değişikliğe uğradığı esnada aşağıdaki olası hatayı meydana getirebilir.


            System.ArgumentOutOfRangeException: ‘capacity was less than the current size.
            Parameter name: value’
        */
    }
}
