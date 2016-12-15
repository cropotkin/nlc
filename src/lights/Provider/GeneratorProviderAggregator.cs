using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace lights
{
    public class GeneratorProviderAggregator : IGeneratorProvider
    {
        private readonly IGeneratorProvider[] providers;
        public GeneratorProviderAggregator(params IGeneratorProvider[] providers)
        {
            this.providers = providers;
        }

        public async Task<IGenerator[]> CollectGenerators()
        {
            var generators = new List<IGenerator>();

            foreach (var provider in this.providers)
            {
                generators.AddRange(await provider.CollectGenerators());
            }

            return generators.ToArray();
        }
    }
}
