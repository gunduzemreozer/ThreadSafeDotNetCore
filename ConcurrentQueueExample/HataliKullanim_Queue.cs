using System;
using System.Collections;
using System.Threading.Tasks;

/*
 * Üç masalı bir lokantada yoğunluktan dolayı yer problemi yaşanmaktadır. 
 * Masalarda yer boşaldıkça sıradaki müşteri boşalan yere alınmakta ve bu işlem eş zamanlı 
 * olarak tüm masalar için gerçekleştirilmektedir.
 */
namespace ConcurrentQueueExample
{
    class HataliKullanim_Queue
    {
        static Queue siradakiler;

        static void Main2(string[] args)
        {
            siradakiler = new Queue();
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
            while (siradakiler.Count > 0)
            {
                await Task.Delay(100);
                Console.WriteLine($"{masaAdi} - {siradakiler.Dequeue()}");
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
