namespace nlc
{
    using lights;
    using System;

    public class Program
    {
        public static void Main(string[] args)
        {
            var lightStrip = new LightStripText(80, "test", v => Console.WriteLine(v));
            var generatorProvider = new StandardGeneratorCollection();

            using (var lsm = new LightStripManager(lightStrip, generatorProvider))
            {
                lsm.RefreshGenerators().Wait();
                lsm.SetCurrentGenerator("snow");
                Console.ReadLine();
            }
        }
    }
}
