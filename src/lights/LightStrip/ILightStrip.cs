namespace lights
{
    using System.Threading.Tasks;

    public interface ILightStrip
    {
        ILightStripInfo Info { get; }
        Task Reset();
        Task Render(ILight[] lights);
    }
}
