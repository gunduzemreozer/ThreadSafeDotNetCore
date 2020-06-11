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
    class Ornek2_CompleteAdding
    {
        static BlockingCollection<int> blockingCollection = new BlockingCollection<int>();

        static void Main(string[] args)
        {
            int sayac = 10;
            Task.Run(() =>
            {
                Console.WriteLine("Task - 1");
                while(sayac >= 1)
                {
                    blockingCollection.Add(sayac--);
                }
                
                blockingCollection.CompleteAdding();
            });

            Task.Run(() =>
            {
                Console.WriteLine("Task - 2");
                while (true)
                {
                    try
                    {
                        Console.WriteLine(blockingCollection.Take());
                    }
                    catch (InvalidOperationException ex)
                    {
                        Console.WriteLine(ex.Message);
                        return Task.CompletedTask;
                    }
                }
            });

            Console.ReadLine();
        }
        /*
         Son olarak bir önceki örnekte (Program.cs) kod bloğunda bir hususa dikkat çekersek eğer tanımlanan tasklar global sayaç değişkeniyle döngüleri kontrol etmektedirler. Dolayısıyla bu şekilde bizler BlockingCollection kütüphanesinin yeni veri eklenmesine dair ihtiyacını hissetmemekteyiz. Eğer ki, BlockingCollection koleksiyonuna eklenecek olan verinin sonuna gelindiğine dair bilgi vermek istiyorsak ekleme işleminden sonra CompleteAdding metodunu çağırmanız yeterlidir. Bu metot çağrıldıktan sonraki ilk Take talebinde InvalidOperationException hatası fırlatılacak ve böylece yeni bir veri girilmesine dair beklenti sona erecektir. Bu sayede while(true) kullanabildik.
        */
    }
}
