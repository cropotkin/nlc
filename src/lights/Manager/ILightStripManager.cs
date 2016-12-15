namespace lights
{
    using System;
    using System.Threading.Tasks;

    public interface ILightStripManager : IDisposable
    {
        ILightStripInfo LightStripInfo { get; }

        Task<string[]> RefreshGenerators();

        bool SetCurrentGenerator(string name);

        string CurrentGenerator { get; }
    }
}
