using System;
using System.Collections.Generic;
namespace lights
{
    public interface ILight
    {
        int Red { get; }
        int Green { get; }
        int Blue { get; }
    }
}
