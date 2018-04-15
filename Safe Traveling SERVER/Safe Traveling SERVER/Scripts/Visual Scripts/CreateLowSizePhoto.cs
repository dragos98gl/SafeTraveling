using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Safe_Traveling_SERVER.Scripts.Visual_Scripts
{
    class CreateLowSizePhoto
    {
        public static string GetLowSizePhotoBase64(string URL)
        {
            Image temp = Image.FromFile(URL);
            Bitmap img = new Bitmap(temp,new Size(50,50));

            ImageConverter Converter = new ImageConverter();
            byte[] imgByteArray = (byte[])Converter.ConvertTo(img,typeof(byte[]));

            return Convert.ToBase64String(imgByteArray);
        }
    }
}
