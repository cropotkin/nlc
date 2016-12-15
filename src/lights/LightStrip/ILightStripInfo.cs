namespace lights
{
    public interface ILightStripInfo
    {
        string Name { get; }
        int Count { get; }
        int FramesPerSecond { get; }
    }
}
