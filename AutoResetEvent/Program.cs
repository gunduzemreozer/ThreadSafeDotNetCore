using System;
using System.Threading;

/*
 * Bu yapı tan olarak bir sinema giriş turnikesi gibi çalışır. Bir kişi gelip biletini okutup içeri geçmesi olayı boyunca diğer kişi bekler. Turnike bir kişi geçtikten sonra otomatik olarak kendini yeni bir kişiye hazırlar ve diğer kişi herhang bir ekstra işlem yapmadan turnikeden geçer. İşte bu özellikten dolayı Auto ön eki eklenmiştir. Kullanım şekli aşağıdaki gibi gösterilmiştir. Lütfen kodu ve yorum satırlarını detaylı olarak inceleyiniz.
*/
namespace AutoResetEventExample
{
    class Program
    {
        //static EventWaitHandle lcRS = new AutoResetEvent(false);
        //static AutoResetEvent lcRS1 = new AutoResetEvent(false);
        //static AutoResetEvent lcRS2 = new AutoResetEvent(false);
        static EventWaitHandle lcRS1 = new EventWaitHandle(false, EventResetMode.AutoReset);
        static EventWaitHandle lcRS2 = new EventWaitHandle(false, EventResetMode.AutoReset);
        // yukarıdaki iki farklı şekilde de oluştutulabilir.
        // buradaki ilk parametre hemen initalize edilip edilmeyeceğini gösterir.
        // Genellikle false olarak kullanırız.

        static void Main()
        {
            new Thread(Waiter1).Start();  // Thread başlattık.
            new Thread(Waiter2).Start();  // Thread başlattık.
            Console.ReadLine();
        }

        static void Waiter1()
        {
            lcRS1.WaitOne(); // Ancak burayı çalıştırabilmek için izin gereklidir.
            Console.WriteLine("1 Waiting..."); // Thread burasını çalıştırır.
            Console.WriteLine("1 Notified");  // İzin geldikten sonra da burayı çalıştırır.
            lcRS2.Set();
        }

        static void Waiter2()
        {
            Console.WriteLine("2 Waiting..."); // Thread burasını çalıştırır.
            Console.WriteLine("2 Notified (Şimdi 1 devam edebilir artık.)");  // İzin geldikten sonra da burayı çalıştırır.
            lcRS1.Set();                // Ancak burayı çalıştırabilmek için izin gereklidir.
            lcRS2.WaitOne();
            Console.WriteLine("Tüm işlemler tamamlandı.");  // İzin geldikten sonra da burayı çalıştırır.
        }
    }

    /*Yukarıdaki işlemlerden de anlaşılabileceği gibi, Tek Yönlü(One Way Signaling) ve Çift Yönlü(Two Way Signaling) sinyalizasyon yapılması gayet kolay, ancak burada dikkat edilmesi gereken bir nokta, eğer doğru kodlama yapılmaz ve WaitOne() konumunda açık kalırsa sistem, öylece bekleme modunda kalacaktır.*/
}
