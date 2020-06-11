using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

/*
 Veri kaynağının boş olduğu durumlarda eleman talep edildiği vakit bir eleman ekleninceye kadar metot akışını bekleten ve başka bir thread tarafından ilgili kaynağa eleman eklenince bu elemanı alarak geriye döndüren bir koleksiyondur. Ayriyetten bunların yanında belirlediğimiz kotayı aşamaması için boyutu limitlenebilen bir koleksiyondur. BlockingCollection koleksiyonu farklı overloadlarında yukarıda ele aldığımız ConcurrentQueue, ConcurrentStack ve ConcurrentBag koleksiyonlarıyla beraber çalışmaktadır.

BlockingCollection<T>; bu constructer kullanılıyorsa eğer arka planda bir ConcurrentQueue koleksiyonu oluşturulmaktadır ve o noktadan itibaren ilgili koleksiyon mantığında çalışmasına devam edecektir.

BlockingCollection<T>(Int32); bu constructer ise arka planda yukarıdaki gibi bir ConcurrentQueue koleksiyonu oluşturmaktadır lakin aldığı int parametresi sayesinde koleksiyonun boyutunu sınırlandıracaktır. Eğer ki, koleksiyonun limiti sınıra dayanmış ise süreçteki herhangi bir yeni eleman ekleme durumunda ilgili koleksiyon yer boşalana kadar bekleyecek ve ilk fırsatta eklemeyi gerçekleştirecektir.

BlockingCollection<T>(IProducerConsumerCollection<T>); ilgili koleksiyonu bir ConcurrentStack yahut ConcurrentBag mantığında değerlendirecektir.

BlockingCollection<T>(IProducerConsumerCollection<T>, Int32); bu constructer ise ilgili koleksiyonu ConcurrentStack yahut ConcurrentBag mantığında çalıştıracak ve bir yandan da boyut eşiğini belirlemiş olacaktır.
*/
namespace BlockingCollectionExample
{
    class Program
    {
        static BlockingCollection<int> blockingCollection = new BlockingCollection<int>();

        static void Main2(string[] args)
        {
            int sayac = 10;
            Task.Run(() =>
            {
                Console.WriteLine("Task - 1");
                while(sayac >= 1)
                {
                    blockingCollection.Add(sayac--);
                }
            });

            Task.Run(() =>
            {
                Console.WriteLine("Task - 2");
                while (sayac != 1)
                {
                    Console.WriteLine(blockingCollection.Take());
                }
            });

            Console.ReadLine();
        }
        /*
         * Eğer ilk devreye 2. task girerse koleksiyondaki değerleri okumaya çalışacak lakin ilk etapta koleksiyon içerisinde eleman olmayacağından dolayı eklenmesini bekleyecek ve böylece 1. task devreye girdiğinde gerekli veriler koleksiyona set edilmiş olacaktır. Netice olarak 2. task koleksiyon içerisindeki elemanları tek tek elde edebilecektir.
        */
    }
}
