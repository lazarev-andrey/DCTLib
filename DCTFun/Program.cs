using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DCTLib;

namespace DCTFun
{
    class Program
    {
        private static int size = 8;
        private const int imageSize = 1024;
        static void Main(string[] args)
        {
            Console.WriteLine("What size should the matrices be? Roughly 8-32. Larger numbers take longer.");

            size = int.Parse(Console.ReadLine());

            Console.WriteLine("Starting...");
            Stopwatch s = new Stopwatch();
            s.Start();
            //input bitmap
            Bitmap testImage = new Bitmap("test.png");

            //output bitmaps and graphics
            Bitmap cycleBitmap = new Bitmap(imageSize, imageSize);
            Graphics cycleGraphics = Graphics.FromImage(cycleBitmap);

            Bitmap coeffBitmap = new Bitmap(imageSize, imageSize);
            Graphics coeffGraphics = Graphics.FromImage(coeffBitmap);

            //instance of dct we will use
            DCT d = new DCT(size);

            for (int y = 0; y < imageSize / size; y++)
            {
                for (int x = 0; x < imageSize / size; x++)
                {
                    Bitmap sector = new Bitmap(size, size);
                    Graphics g = Graphics.FromImage(sector);

                    //where bitmap data will be copied
                    Rectangle dest = new Rectangle(0, 0, size, size);
                    Rectangle src = new Rectangle(x * size, y * size, size, size);

                    g.DrawImage(testImage, dest, src, GraphicsUnit.Pixel);

                    double[][,] coeffs = d.BitmapToMatrices(sector);
                    d.DCTMatrices(coeffs);//Pass with DCT

                    coeffGraphics.DrawImage(d.MatricesToBitmap((coeffs)), src, dest, GraphicsUnit.Pixel);//Draw the coefficient table


                    d.IDCTMatrices(coeffs);//Pass with IDCT
                    cycleGraphics.DrawImage(d.MatricesToBitmap(coeffs), src, dest, GraphicsUnit.Pixel);//Draw a returned cycle


                }
                Console.WriteLine("{0} of {1}", y, imageSize / size);
            }

            Console.WriteLine(s.ElapsedMilliseconds);
            cycleBitmap.Save("cycled.png");
            coeffBitmap.Save("coeffs.png");
            Console.WriteLine("Complete. Press any key to continue.");
            Console.ReadKey();
        }
    }
}
