using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace lights
{
    public interface IGenerator
    {
        string Name { get; }
        IFrameFactory Create(ILightStripInfo info);
    }
}
