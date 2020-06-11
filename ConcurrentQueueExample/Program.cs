using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Threading.Tasks;

/*
 * Üç masalı bir lokantada yoğunluktan dolayı yer problemi yaşanmaktadır. 
 * Masalarda yer boşaldıkça sıradaki müşteri boşalan yere alınmakta ve bu işlem eş zamanlı 
 * olarak tüm masalar için gerçekleştirilmektedir.
 */
namespace ConcurrentQueueExample
{
    class Program
    {
        static ConcurrentQueue<string> siradakiler;

        static void Main(string[] args)
        {
            siradakiler = new ConcurrentQueue<string>();
            siradakiler.Enqueue("Ahmet");
            siradakiler.Enqueue("Ali");
            siradakiler.Enqueue("Mehmet");
            siradakiler.Enqueue("Necati");
            siradakiler.Enqueue("Ayşe");
            siradakiler.Enqueue("Mahmut");
            siradakiler.Enqueue("Hilmi");
            siradakiler.Enqueue("Hüseyin");
            siradakiler.Enqueue("Rıfkı Abi");

            Kontrol().Wait();
        }

        static async Task MasaAl(string masaAdi)
        {
            while(siradakiler.Count > 0)
            {
                await Task.Delay(100);
                siradakiler.TryDequeue(out string siradaki);
                Console.WriteLine($"{masaAdi} - {siradaki}");
            }
        }

        static async Task Kontrol()
        {
            var task1 = MasaAl("Masa 1");
            var task2 = MasaAl("Masa 2");
            var task3 = MasaAl("Masa 3");

            await Task.WhenAll(task1, task2, task3);
            Console.WriteLine("Masa sırası bitmiştir.");
        }

    }
}
