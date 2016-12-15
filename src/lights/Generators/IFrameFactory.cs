namespace lights
{
    using System;
    using System.Threading.Tasks;

    public interface IFrameFactory
    {
        Task<ILight[]> Create();
    }
}
