﻿using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using Size = System.Drawing.Size;

namespace Hurricane.Utilities
{
   static class ImageHelper
    {
        public static Size GetMinimumSize(Size size, int maxwidth, int maxheight)
        {
            Size newSize;
            if (size.Width == size.Height)
            {
                newSize = new Size(220, 220);
            }
            else
            {
                bool widthIsHigher = size.Width > size.Height;
                double ratio = size.Width / (double)size.Height;
                double newwidth;
                double newheight;

                if (widthIsHigher)
                {
                    newwidth = maxheight * ratio;
                    if (newwidth >= maxwidth)
                    {
                        newheight = maxheight;
                    }
                    else
                    {
                        newwidth = maxwidth;
                        newheight = maxwidth / ratio;
                    }
                }
                else
                {
                    newheight = maxwidth / ratio;
                    if (newheight >= maxheight)
                    {
                        newwidth = maxwidth;
                    }
                    else
                    {
                        newwidth = maxheight * ratio;
                        newheight = maxheight;
                    }
                }
                newSize = new Size((int)Math.Round(newwidth, 0), (int)Math.Round(newheight, 0));
            }
            return newSize;
        }

        public static Bitmap ResizeImage(Bitmap imgToResize, Size size)
        {
            try
            {
                Bitmap b = new Bitmap(size.Width, size.Height);
                using (Graphics g = Graphics.FromImage(b))
                {
                    g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    g.DrawImage(imgToResize, 0, 0, size.Width, size.Height);
                }
                return b;
            }
            catch { return null; }
        }

        public static Icon GetIconFromResource(string path)
        {
            return new Icon(Application.GetResourceStream(new Uri(string.Format("pack://application:,,,/Hurricane;component/{0}", path))).Stream);
        }

        public async static Task<BitmapImage> DownloadImage(WebClient web, string url)
        {
            using (MemoryStream mr = new MemoryStream(await web.DownloadDataTaskAsync(url)))
            {
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.StreamSource = mr;
                bitmap.EndInit();
                bitmap.Freeze();
                return bitmap;
            }
        }

        public static async Task SaveImage(BitmapImage img, string filename, string directory)
        {
            await Task.Run(() =>
            {
                var encoder = new PngBitmapEncoder();
                string path = Path.Combine(directory, GeneralHelper.EscapeFilename(filename) + ".png");
                encoder.Frames.Add(BitmapFrame.Create(img));
                using (var filestream = new FileStream(path, FileMode.Create))
                    encoder.Save(filestream);
            });
        }

        public static BitmapImage ByteArrayToBitmapImage(byte[] data)
        {
            return StreamToBitmapImage(new MemoryStream(data));
        }

        public static BitmapImage StreamToBitmapImage(Stream stream)
        {
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            image.CacheOption = BitmapCacheOption.OnLoad;
            image.StreamSource = stream;
            image.EndInit();
            return image;
        }
    }
}
