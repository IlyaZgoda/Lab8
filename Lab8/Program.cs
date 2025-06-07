using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

namespace Lab8
{
    public class Program 
    {
        static void Main(string[] args)
        {
            var nativeWindowSettings = new NativeWindowSettings()
            {
                ClientSize = new OpenTK.Mathematics.Vector2i(800, 600),
                Title = "Solar System"
            };

            using var window = new Game(GameWindowSettings.Default, nativeWindowSettings);
            window.Run();
        }
    }
}
