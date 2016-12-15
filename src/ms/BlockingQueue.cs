namespace ms
{
    using System.Collections.Generic;
    using System.Threading;

    public class BlockingQueue<T>
    {
        private readonly Semaphore freeEntries;

        private readonly Semaphore fullEntries;

        private readonly Queue<T> queue;

        public BlockingQueue(int length)
        {
            this.freeEntries = new Semaphore(length, length);
            this.fullEntries = new Semaphore(0, length);
            this.queue = new Queue<T>();
        }

        public void Enqueue(T value)
        {
            this.freeEntries.WaitOne();

            lock (this.queue)
            {
                this.queue.Enqueue(value);
            }

            this.fullEntries.Release();
        }

        public T Dequeue()
        {
            T value;

            this.fullEntries.WaitOne();

            lock (this.queue)
            {
                value = this.queue.Dequeue();
            }

            this.freeEntries.Release();

            return value;
        }
    }
}
