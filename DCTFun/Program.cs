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
        private static int size;
        static void Main(string[] args)
        {
            if (Debugger.IsAttached)
                args = new[] {"test.png"};

            Console.WriteLine("Input: {0}", args[0]);
            Console.WriteLine("What size should the matrices be? Roughly 2-32. Larger numbers take longer.");
            Console.WriteLine("Best results are multiples of the input image's width and height.");

            while (!int.TryParse(Console.ReadLine(), out size))
            {
                Console.WriteLine("Input should be a whole positive number.");
            }

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
            DCT d = new DCT(size);

            for (int y = 0; y < height / size; y++)
            {
                for (int x = 0; x < width / size; x++)
                {
                    Bitmap sector = new Bitmap(size, size);
                    Graphics g = Graphics.FromImage(sector);

                    //where bitmap data will be copied
                    Rectangle dest = new Rectangle(0, 0, size, size);
                    Rectangle src = new Rectangle(x * size, y * size, size, size);

                    g.DrawImage(bitmap, dest, src, GraphicsUnit.Pixel);

                    double[][,] coeffs = d.BitmapToMatrices(sector);
                    d.DCTMatrices(coeffs);//Pass with DCT

                    coeffGraphics.DrawImage(d.MatricesToBitmap((coeffs)), src, dest, GraphicsUnit.Pixel);//Draw the coefficient table


                    d.IDCTMatrices(coeffs);//Pass with IDCT
                    cycleGraphics.DrawImage(d.MatricesToBitmap(coeffs), src, dest, GraphicsUnit.Pixel);//Draw a returned cycle


                }
                Console.Write("\r{0} of {1}", y, height / size);
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
    }
}
