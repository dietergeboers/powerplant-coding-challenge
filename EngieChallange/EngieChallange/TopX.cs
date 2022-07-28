using DotNetty.Common.Utilities;

namespace Engie
{
    public class TopX<T> where T : class
    {

        private PriorityQueue<T> queue;
        private readonly IComparer<T> scheduleComparator;
        private readonly int x;

        public TopX(IComparer<T> scheduleComparator, int x)
        {
            queue = new PriorityQueue<T>(scheduleComparator);
            this.scheduleComparator = scheduleComparator;
            this.x = x;
        }
        public void Add(T item) { Enqueue(item); }
        public void Clear() { queue.Clear(); }
        public int Count { get { return queue.Count; } }
        public void Enqueue(T item)
        {
            if (queue.Count == 0)
            {
                queue.Enqueue(item);
            }
            if (queue.Count < x || scheduleComparator.Compare(queue.Min(scheduleComparator), item) < 0)
            {
                if (queue.Count >= x)
                    queue.Remove(queue.Min(scheduleComparator));
                queue.Enqueue(item);
            }
        }
        public T Dequeue() { return queue.Dequeue(); }
    }
}