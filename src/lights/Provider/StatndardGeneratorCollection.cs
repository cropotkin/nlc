namespace lights
{
    using System.Threading.Tasks;

    public class StandardGeneratorCollection : GeneratorCollection
    {
        public StandardGeneratorCollection()
        {
            this.Add(new GeneratorSnow());
        }
    }
}
