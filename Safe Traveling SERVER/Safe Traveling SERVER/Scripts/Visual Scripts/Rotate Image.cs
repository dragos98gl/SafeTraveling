using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Safe_Traveling_SERVER.Scripts.Visual_Scripts
{
    public class RotateImage
    {
        private bool On;

        public void Start(PictureBox PicBox, int Velocity, int MasuraUnghi)
        {
            Thread.Sleep(300);

            On = true;

            Image GetImg = PicBox.Image;
            PointF offset = new PointF((float)GetImg.Width / 2, (float)GetImg.Height / 2);

            int Unghi = 0;

            while (On)
            {
                Bitmap rotatedImage = new Bitmap(GetImg.Width, GetImg.Height);
                rotatedImage.SetResolution(GetImg.HorizontalResolution, GetImg.VerticalResolution);
                Graphics g = Graphics.FromImage(rotatedImage);
                g.TranslateTransform(offset.X, offset.Y);
                g.RotateTransform(Unghi);
                g.TranslateTransform(-offset.X, -offset.Y);
                try
                {
                    g.DrawImage(GetImg, new PointF(0, 0));
                }
                catch { break; }
                Unghi = Unghi + MasuraUnghi;
                Thread.Sleep(Velocity);

                PicBox.Image = rotatedImage;

                g.Dispose();
            }
        }

        public void Stop()
        {
            On = false;
        }
    }
}
