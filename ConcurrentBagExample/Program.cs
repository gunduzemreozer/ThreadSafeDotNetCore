using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

/*
 * ConcurrentBag koleksiyonu, ekleme ve okuma işlemleri için threadler üzerinde eşit bir dağılım
 * söz konusuysa eğer çok faydalı olacaktır. Ayrıca yukarıdaki satırlarda değindiğimiz gibi eleman 
 * elde etmek istendiği vakit ya ilgili thread tarafından en son eklenen eleman ya da diğer threadlerden
 * herhangi birinin eklediği en sonucusu elimize ulaşacaktır. İşte bu mantıktan dolayı o anda hangi
 * elemanın işlendiğinin önemli olmadığı senaryolarda gönül rahatlığıyla kullanılabilecek bir koleksiyondur. 
*/
namespace ConcurrentBagExample
{
    class Program
    {
        static ConcurrentBag<int> concurrentBag = new ConcurrentBag<int>();

        static void Main2(string[] args)
        {
            AutoResetEvent autoResetEvent = new AutoResetEvent(false);
            concurrentBag.Add(0);

            #region Task 1
            Task.Run(() =>
            {
                Console.WriteLine("****");
                for(int i = 1; i < 10; i++)
                {
                    concurrentBag.Add(i);
                }

                autoResetEvent.Set();
            });
            #endregion Task 1

            #region Task 2
            Task.Run(() =>
            {
                Console.WriteLine("----");
                while (!concurrentBag.IsEmpty)
                {
                    if (concurrentBag.TryTake(out int data))
                    {
                        Console.WriteLine(data);
                    }
                }
                autoResetEvent.WaitOne();
            });
            #endregion Task 2

            Console.ReadLine();
        }

        /*
         * Yukarıdaki örnek kod bloğunu incelerseniz eğer; iki farklı taskta farklı threadlar devreye sokulmuş ve Task 1’de ConcurrentBag koleksiyonu doldurulurken Task 2’de ise ilgili koleksiyondaki değerler elde edilip ekrana yazdırılmıştır. 5. satırda koleksiyonumuza direkt olarak Main fonksiyonu içerisinden bir eleman atanmıştır. Nihayetinde bu atama işleminin Main’de olması demek Task 1’e nazaran farklı threadde olması demektir. Şimdi bu kodu derleyip çalıştırdığımızda nasıl bir sonuçla karşılaşıyoruz inceleyelim ve üzerine mantığı hep beraber istişare edelim.
         * 
         * Yukarıdaki şemada belirtildiği gibi ilk olarak Task 1 devreye girse dahi süreç asenkron olduğu için her iki yapıda birbirinden bağımsız eş zamanlı bir şekilde sürece dahil olmakta ve işlenmektedir. Burada ilk etapta “0” değerinin yazmasını beklerken Task 1 işlevi neticesinde koleksiyona dahil edilecek tüm değerlerin yazıldığını görmekteyiz. Bunun nedeni “AutoResetEvent” sınıfının “WaitOne” metodu sayesinde Task 2 faaliyeti sona erdirilmeden asenkron süreçteki diğer fonksiyonlar(Task 1) devam ettirilmekte ve nihayetinde Task 1 içerisinde yine aynı sınıfın “Set” metodu ile bekletilen thread sürece devam ettirilmektedir. Haliyle koleksiyon içerisindeki değerler değişse dahi Thread Safe koleksiyon olmasından dolayı kaynak revize edilse dahi bir hata meydana vermemekte ve yeni verileride while faaliyeti bir sonraki değeri itere ederek elde edip çıktı olarak vermektedir. Ayrıca “0” değerinin en sonda elde edilmesi ise yukarıdaki satırlarda açıklamaya çalıştığımız, her bir threadin kendi eklediğini en son olarak nitelemesi ve getirmesi olayının açık bir tezahürüdür. Neticede ilgili “0” değerini Task 1 değil Main thread’i eklemiş bulunmaktadır.
        */
    }
}
