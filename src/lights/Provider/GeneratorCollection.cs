namespace lights
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class GeneratorCollection : IGeneratorProvider
    {
        private readonly List<IGenerator> generators;

        protected GeneratorCollection()
        {
            this.generators = new List<IGenerator>();
        }

        protected void Add(IGenerator generator)
        {
            lock (this.generators)
            {
                this.generators.Add(generator);
            }
        }

        public async Task<IGenerator[]> CollectGenerators()
        {
            IGenerator[] result;

            lock (this.generators)
            {
                result = this.generators.ToArray();
            }

            return await Task.FromResult(result);
        }
    }
}
