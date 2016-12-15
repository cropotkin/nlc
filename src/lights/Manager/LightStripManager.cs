namespace lights
{
    using System;
    using System.Linq;
    using System.Threading;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using ms;

    public class LightStripManager : ILightStripManager
    {
        private const int MaxFrameQueueOccupancy = 50;

        private readonly ILightStrip lightStrip;
        private readonly IGeneratorProvider generatorProvider;

        private object sync;

        private IGenerator currentGenerator;
        private IFrameFactory currentFrameFactory;
        private IGenerator[] generators;

        private CancellationTokenSource cancellationTokenSource;
        private BlockingQueue<ILight[]> frameQueue;
        private Thread consumerThread;
        private Thread producerThread;

        public LightStripManager(ILightStrip lightStrip, IGeneratorProvider generatorProvider)
        {
            this.lightStrip = lightStrip;
            this.generatorProvider = generatorProvider;
            this.sync = new object();
            this.currentGenerator = new GeneratorDefault();
            this.currentFrameFactory = this.currentGenerator.Create(this.lightStrip.Info);
            this.generators = new[] { this.currentGenerator };
            this.cancellationTokenSource = new CancellationTokenSource();
            this.frameQueue = new BlockingQueue<ILight[]>(MaxFrameQueueOccupancy);
            this.consumerThread = new Thread(ConsumerThread);
            this.producerThread = new Thread(ProducerThread);
            this.consumerThread.Start();
            this.producerThread.Start();
        }

        // ILightStripManager

        public ILightStripInfo LightStripInfo
        {
            get
            {
                return this.lightStrip.Info;
            }
        }

        public async Task<string[]> RefreshGenerators()
        {
            var aggregatedGenerators = new List<IGenerator>();

            var currentGenerators = EnumerateCurrentGenerators();

            var latestGenerators = await this.generatorProvider.CollectGenerators();

            foreach (var generator in latestGenerators)
            {
                aggregatedGenerators.Add(generator);

                if (currentGenerators.Any(v => v.Name == generator.Name))
                {
                    currentGenerators  = currentGenerators.Where(v => v.Name != generator.Name);
                }
            }

            aggregatedGenerators.AddRange(currentGenerators);

            lock (sync)
            {
                this.generators = aggregatedGenerators.OrderBy(v => v.Name).ToArray();
                return this.generators.Select(v => v.Name).ToArray();
            }
        }

        public string CurrentGenerator
        {
            get
            {
                lock (this.sync)
                {
                    return this.currentGenerator.Name;
                }
            }
        }

        public bool SetCurrentGenerator(string name)
        {
            lock (this.sync)
            {
                foreach (var generator in this.generators)
                {
                    if (generator.Name == name)
                    {
                        this.currentGenerator = generator;
                        this.currentFrameFactory = this.currentGenerator.Create(this.lightStrip.Info);
                        return true;
                    }
                }

                return false;
            }
        }

        // IDisposable

        public void Dispose()
        {
            this.cancellationTokenSource.Cancel();
            this.producerThread.Join();
            this.consumerThread.Join();
            this.lightStrip.Reset();
        }

        // private functions

        private IEnumerable<IGenerator> EnumerateCurrentGenerators()
        {
            lock (this.sync)
            {
                return this.generators;
            }
        }

        private IFrameFactory GetCurrentFrameFactory()
        {
            lock (this.sync)
            {
                return this.currentFrameFactory;
            }
        }

        private async void ProducerThread()
        {
            while (true)
            {
                this.cancellationTokenSource.Token.ThrowIfCancellationRequested();
                this.frameQueue.Enqueue(await this.GetCurrentFrameFactory().Create());
            }
        }

        private async void ConsumerThread()
        {
            await this.lightStrip.Reset();

            while (true)
            {
                this.cancellationTokenSource.Token.ThrowIfCancellationRequested();
                await this.lightStrip.Render(this.frameQueue.Dequeue());
            }
        }
    }
}
