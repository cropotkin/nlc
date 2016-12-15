using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace lights
{
    public class LightStripText : ILightStrip
    {
        private readonly ILightStripInfo info;
        private readonly Action<string> renderer;

        public LightStripText(int count, string name, Action<string> renderer)
        {
            this.info = new LightStripInfo
            {
                Count = count,
                FramesPerSecond = 100,
                Name = name
            };

            this.renderer = renderer;
        }

        // ILightStrip

        public ILightStripInfo Info
        {
            get
            {
                return this.info;
            }
        }

        public Task Render(ILight[] lights)
        {
            return Task.Factory.StartNew(() =>
            {
                var sb = new StringBuilder();

                foreach (var light in lights)
                {
                    sb.Append(Render(light));
                }

                this.renderer(sb.ToString());
            });
        }

        public Task Reset()
        {
            return Task.CompletedTask;
        }

        // private functions

        private static char Render(ILight light)
        {
            if (light.Red == 0 && light.Green == 0 && light.Blue == 0)
            {
                return ' ';
            }

            var intense = light.Red + light.Green + light.Blue > 383;

            if (light.Red > light.Blue)
            {
                if (light.Red > light.Green)
                {
                    return intense ? 'R' : 'r';
                }
                else if (light.Red < light.Green)
                {
                    return intense ? 'G' : 'g';
                }

                return intense ? 'Y' : 'y';
            }
            else if (light.Red < light.Blue)
            {
                if (light.Blue > light.Green)
                {
                    return intense ? 'B' : 'b';
                }
                else if (light.Blue < light.Green)
                {
                    return intense ? 'G' : 'g';
                }

                return intense ? 'C' : 'c';
            }
            if (light.Red > light.Green)
            {
                return intense ? 'M' : 'm';
            }
            else if (light.Red < light.Green)
            {
                return intense ? 'G' : 'g';
            }

            return intense ? 'W' : 'w';
        }
    }
}
