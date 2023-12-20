using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace ConvolutionNeuralNetwork
{
    public class InputData
    {
        private int Count { get; }
        private Image[] RawData { get; }
        private Bitmap[] WetData { get; }
        private int[][][,] Data { get; }
        private int Width { get; }
        private int Hight { get; }
        private bool Colored { get; }

        public InputData(Image[] images, int width = 256, int hight = 64, bool colored = false)
        {
            RawData = new Image[images.Length];
            images.CopyTo(RawData, 0);

            Count = images.Length;
            Colored = colored;
            Width = width;
            Hight = hight;

            WetData = new Bitmap[images.Length];
            Data = new int[Count][][,];

            ProcessImages(images);
        }

        private void ProcessImages(Image[] images)
        {
            for (int i = 0; i < Count; i++)
            {
                var wetImage = ResizeImage(images[i], Width, Hight);

                if (!Colored)
                {
                    wetImage = SauvolBinarize.Sauvola(wetImage);

                    WetData[i] = wetImage;

                    Data[i] = new int[1][,];
                    Data[i][0] = new int[Width, Hight];

                    for (int n = 0; n < wetImage.Height; n++)
                    {
                        for (int m = 0; m < wetImage.Width; m++)
                        {
                            if (wetImage.GetPixel(m, n).IsNamedColor)
                            {
                                switch (wetImage.GetPixel(m, n).Name)
                                {
                                    case "Black":
                                        Data[i][0][n, m] = 0;
                                        break;
                                    case "White":
                                        Data[i][0][n, m] = 1;
                                        break;
                                    default:
                                        throw new Exception();
                                }
                            }
                        }
                    }
                }
                else
                {
                    WetData[i] = wetImage;

                    Data[i] = new int[3][,];

                    Data[i][0] = new int[Width, Hight];
                    Data[i][1] = new int[Width, Hight];
                    Data[i][2] = new int[Width, Hight];

                    for (int n = 0; n < wetImage.Height; n++)
                    {
                        for (int m = 0; m < wetImage.Width; m++)
                        {
                            Data[i][0][n, m] = wetImage.GetPixel(m, n).R / 255;
                            Data[i][1][n, m] = wetImage.GetPixel(m, n).G / 255;
                            Data[i][2][n, m] = wetImage.GetPixel(m, n).B / 255;
                        }
                    }
                }
            }
        }

        private static Bitmap ResizeImage(Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }
    }
}
