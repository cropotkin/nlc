using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace lights
{
    public interface IGeneratorProvider
    {
        Task<IGenerator[]> CollectGenerators();
    }
}
