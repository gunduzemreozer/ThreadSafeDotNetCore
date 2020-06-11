using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

/*
 * Aynı kaynağı kullanan en az iki threadın söz konusu olduğu durumlarda bir thread ile kaynağa veri eklerken bir diğeriyle silme işlemlerinin gerçekleştiği senaryolarda ConcurrentStack koleksiyonu kullanılabilir.
*/
namespace ConcurrentStatck
{
    class Program
    {
        static ConcurrentStack<int> concurrentStack = new ConcurrentStack<int>();
        static AutoResetEvent autoResetEvent1 = new AutoResetEvent(false);
        static AutoResetEvent autoResetEvent2 = new AutoResetEvent(false);

        static void Main(string[] args)
        {
            // Ekleme
            Task.Run(() =>
            {
                for (int i = 1; i <= 10; i++)
                {
                    concurrentStack.Push(i);
                    autoResetEvent2.Set();
                    autoResetEvent1.WaitOne();
                }

                autoResetEvent2.Set();
            });

            //Çıkarma           
            Task.Run(() =>
            {
                while (concurrentStack.IsEmpty)
                {
                    autoResetEvent2.WaitOne();
                    if (concurrentStack.TryPop(out int item))
                    {
                        Console.WriteLine(item);
                        autoResetEvent1.Set();
                    }
                }
            });

            Console.Read();
        }
    }
}
