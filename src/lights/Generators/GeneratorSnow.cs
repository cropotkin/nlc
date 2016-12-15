namespace lights
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class GeneratorSnow : Generator
    {
        public GeneratorSnow()
            : base("snow", info => new FrameFactorySnow(info))
        {
        }
    }

    public class FrameFactorySnow : IFrameFactory
    {
        private const int SnowFlakesPerHundredLights = 8;

        private readonly ILightStripInfo info;

        private readonly Snowflake[] snowflakes;

        private int frameCount;

        public FrameFactorySnow(ILightStripInfo info)
        {
            this.info = info;
            var snowflakeCount = this.info.Count * SnowFlakesPerHundredLights / 100;
            this.snowflakes = CreateSnowflakes(snowflakeCount, info.Count).ToArray();
        }

        private IEnumerable<Snowflake> CreateSnowflakes(int snowflakes, int lights)
        {
            while (snowflakes-- > 0)
            {
                yield return new Snowflake(lights);
            }
        }

        private IEnumerable<Light> CreateLights(int count)
        {
            while (count-- > 0)
            {
                yield return new Light(0, 0, 0);
            }
        }


        public Task<ILight[]> Create()
        {
            return Task.Factory.StartNew<ILight[]>(() =>
            {
                var lights = CreateLights(this.info.Count).ToArray();

                foreach (var snowflake in this.snowflakes)
                {
                    snowflake.Render(lights);
                }

                return lights;
            });
        }
    }

    public class Snowflake
    {
        private const int StepsPerSnowflake = 32;
        private const int IntensityPerStep = 12;

        private readonly int lights;
        private readonly Random random;
        private int step;
        private int intensity;
        private int position;

        public Snowflake(int lights)
        {
            this.lights = lights;
            this.random = new Random();
            this.step = StepsPerSnowflake;
        }

        public void Render(Light[] lights)
        {
            if (this.step == StepsPerSnowflake)
            {
                this.step = 0;
                this.intensity = 0;
                this.position = this.random.Next(this.lights);
            }

            if (this.step < StepsPerSnowflake / 2)
            {
                this.intensity += IntensityPerStep;
            }
            else
            {
                this.intensity -= IntensityPerStep;
            }


            lights[position] = new Light(this.intensity, this.intensity, this.intensity);

            this.step++;
        }
    }
}
