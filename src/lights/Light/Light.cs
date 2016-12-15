using System.Collections.Generic;

namespace lights
{
    public class Light : ILight
    {
        public Light(int red, int green, int blue)
        {
            this.Red = red;
            this.Green = green;
            this.Blue = blue;
        }

        public int Red { get; }
        public int Green { get; }
        public int Blue { get; }

        public static ILight[] Fill(int count, ILight light)
        {
            var lights = new Queue<ILight>();

            while (count-- > 0)
            {
                lights.Enqueue(light);
            }

            return lights.ToArray();
        }

        public static ILight[] Fill(int count)
        {
            return Fill(count, new Light(0, 0, 0));
        }

        public static ILight Add(ILight light, int value)
        {
            var oldValue = light.Blue + (light.Green << 8) + (light.Red << 16);
            var newValue = oldValue + value;
            return new Light((newValue & 0xff000) >> 16, (newValue & 0xff00) >> 8, newValue & 0xff);
        }
    }
}
