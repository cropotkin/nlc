namespace lights
{
    using System;

    public class Generator : IGenerator
    {
        private readonly Func<ILightStripInfo, IFrameFactory> factory;

        protected Generator(string name, Func<ILightStripInfo, IFrameFactory> factory)
        {
            this.Name = name;
            this.factory = factory;
        }

        public string Name { get; }

        public IFrameFactory Create(ILightStripInfo info)
        {
            return this.factory(info);
        }
    }
}
