using System.Windows.Media;
using System.Windows.Media.Imaging;
using System;

namespace img
{
    public static class Images
    {
        public readonly static ImageSource Empty = loadImg("Textures/Empty.png");
        public readonly static ImageSource Body = loadImg("Textures/Body.png");
        public readonly static ImageSource DeadBody = loadImg("Textures/DeadBody.png");
        public readonly static ImageSource Head = loadImg("Textures/Head.png");
        public readonly static ImageSource DeadHead = loadImg("Textures/DeadHead.png");
        public readonly static ImageSource Food = loadImg("Textures/Food.png");

        private static ImageSource loadImg(string fileName) {
            return new BitmapImage(new Uri($"Assets/{fileName}", UriKind.Relative));
        }
    }
}
