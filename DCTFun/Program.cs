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
        static void Main(string[] args)
        {
            int w;
            int h;

            if (Debugger.IsAttached)
                args = new[] { "test.png" };

            if (args.Length == 0)
            {
                Console.WriteLine("No image supplied. Drag an image onto DCTFun.exe.");
                Console.WriteLine("Press any key to continue.");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("Input: {0}", args[0]);
            Console.WriteLine("What width should the matrices be? Roughly 2-32. Larger numbers take longer.");
            ReadInt(out w);
            ReadInt(out h);

            Console.WriteLine("Starting...");
            Stopwatch s = new Stopwatch();
            s.Start();
            //input bitmap
            Bitmap bitmap = new Bitmap(args[0]);
            int width = bitmap.Width;
            int height = bitmap.Height;

            //output bitmaps and graphics
            Bitmap cycleBitmap = new Bitmap(width, height);
            Graphics cycleGraphics = Graphics.FromImage(cycleBitmap);

            Bitmap coeffBitmap = new Bitmap(width, height);
            Graphics coeffGraphics = Graphics.FromImage(coeffBitmap);

            //instance of dct we will use
            DCT d = new DCT(w, h);

            for (int y = 0; y < height / h; y++)
            {
                for (int x = 0; x < width / w; x++)
                {
                    Bitmap sector = new Bitmap(w, h);
                    Graphics g = Graphics.FromImage(sector);

                    //where bitmap data will be copied
                    Rectangle dest = new Rectangle(0, 0, w, h);
                    Rectangle src = new Rectangle(x * w, y * h, w, h);

                    g.DrawImage(bitmap, dest, src, GraphicsUnit.Pixel);

                    double[][,] coeffs = d.BitmapToMatrices(sector);
                    d.DCTMatrices(coeffs);//Pass with DCT

                    coeffGraphics.DrawImage(d.MatricesToBitmap((coeffs)), src, dest, GraphicsUnit.Pixel);//Draw the coefficient table


                    d.IDCTMatrices(coeffs);//Pass with IDCT
                    cycleGraphics.DrawImage(d.MatricesToBitmap(coeffs), src, dest, GraphicsUnit.Pixel);//Draw a returned cycle


                }
                Console.Write("\r{0} of {1}", y, height / h);
            }
            Console.WriteLine();
            Console.WriteLine("{0} seconds elapsed. ", s.ElapsedMilliseconds / 1000);


            cycleBitmap.Save("cycled.png");
            Console.WriteLine("Saved cycled.png");
            coeffBitmap.Save("coeffs.png");
            Console.WriteLine("Saved coeffs.png");


            Console.WriteLine("Complete. Press any key to exit.");
            Console.ReadKey();
        }

        private static void ReadInt(out int a)
        {
            while (!int.TryParse(Console.ReadLine(), out a))
            {
                Console.WriteLine("Input should be a whole number.");
            }
        }
    }
}
