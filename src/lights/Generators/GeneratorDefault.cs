namespace lights
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class GeneratorDefault : Generator
    {
        public GeneratorDefault()
            : base("default", info => new FrameFactoryDefault(info))
        {
        }
    }

    public class FrameFactoryDefault : IFrameFactory
    {
        private const int LightIncrement = 1;
        private readonly Queue<ILight> queue;
        private ILight next;

        public FrameFactoryDefault(ILightStripInfo info)
        {
            this.queue = new Queue<ILight>();
            this.next = new Light(0, 0, 0);

            for (int i = 0; i < info.Count; i++)
            {
                PopulateQueue();
            }
        }

        public Task<ILight[]> Create()
        {
            return Task.Factory.StartNew<ILight[]>(() =>
            {
                var lights = this.queue.ToArray();
                this.queue.Dequeue();
                this.PopulateQueue();
                return lights;
            });
        }

        private void PopulateQueue()
        {
            this.queue.Enqueue(this.next);
            this.next = Light.Add(this.next, LightIncrement);
        }
    }
}
