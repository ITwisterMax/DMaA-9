using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Media.Imaging;

namespace ImageLoader
{
    /// <summary>
    ///     Bitmap converter
    /// </summary>
    public static class BitmapConverter
    {
        /// <summary>
        ///     From bitmap to list of bytes
        /// </summary>
        ///
        /// <param name="bitmap">Bitmap</param>
        ///
        /// <returns>List<int></returns>
        public static List<int> ToInt32List(Bitmap bitmap)
        {
            var result = new List<int>();
            for (var i = 0; i < bitmap.Width; i++)
            {
                for (var j = 0; j < bitmap.Height; j++)
                {
                    var pixelColor = bitmap.GetPixel(i, j);
                    result.Add(255 - (pixelColor.R + pixelColor.G + pixelColor.B) / 3);
                }
            }

            return result;
        }

        /// <summary>
        ///     Load image
        /// </summary>
        ///
        /// <param name="path">Path</param>
        /// <param name="size">Size</param>
        ///
        /// <returns>Bitmap</returns>
        public static Bitmap Load(String path, int size)
        {
            var result = new Bitmap(size, size);

            using (var loaded = new Bitmap(path))
            using (var graphics = Graphics.FromImage(result))
            {
                graphics.DrawImage(loaded, new Rectangle(0, 0, loaded.Width, loaded.Height), 0, 0, size, size, GraphicsUnit.Pixel);
            }

            return result;
        }

        /// <summary>
        ///     Convert to BitmapImage
        /// </summary>
        ///
        /// <param name="bitmap">Bitmap</param>
        ///
        /// <returns>BitmapImage</returns>
        public static BitmapImage ToBitmapImage(Bitmap bitmap)
        {
            var result = new BitmapImage();

            using (var stream = new MemoryStream())
            {
                bitmap.Save(stream, ImageFormat.Png);

                result.BeginInit();
                result.StreamSource = new MemoryStream(stream.ToArray());
                result.EndInit();
            }

            return result;
        }
    }
}