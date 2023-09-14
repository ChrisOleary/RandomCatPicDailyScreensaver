using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace RandomCatPicDailyScreensaver
{
    class Wallpaper
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int SystemParametersInfo(int uAction, int uParam, string lpvParam, int fuWinIni);

        const int SPI_SETDESKWALLPAPER = 0x0014;
        const int SPIF_UPDATEINIFILE = 0x01;
        const int SPIF_SENDCHANGE = 0x02;

        public static void SetWallpaper(byte[] imageBytes)
        {
            // Save the image to a temporary file
            string tempImagePath = "tempImage.jpg"; // You can choose a different path
            File.WriteAllBytes(tempImagePath, imageBytes);

            // Set the desktop background
            SystemParametersInfo(SPI_SETDESKWALLPAPER, 0, tempImagePath, SPIF_UPDATEINIFILE | SPIF_SENDCHANGE);
        }
    }
}
