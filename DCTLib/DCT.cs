using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DCTLib
{
    public class DCT
    {
        public DCT(int matrixSize)
        {
            size = matrixSize;
        }

        //size of all matrices, width and height.
        private readonly int size;

        //Turn DCT matrices into an RGB bitmap for output
        public Bitmap MatricesToBitmap(double[][,] array)
        {
            PrintMatrix(array[0]);
            Bitmap b = new Bitmap(size, size);
            for (int x = 0; x < size; x++)
            {
                for (int y = 0; y < size; y++)
                {
                    byte R = (byte)(normOut(array[0][x, y] + 128));
                    byte G = (byte)(normOut(array[1][x, y] + 128));
                    byte B = (byte)(normOut(array[2][x, y] + 128));
                    b.SetPixel(x, y, Color.FromArgb(R, G, B));
                }
            }
            return b;
        }

        private double normOut(double a)
        {
            return Math.Min(Math.Max(a, 0), 255);
        }

        //Create matrices from an RGB bitmap
        public double[][,] BitmapToMatrices(Bitmap b)
        {
            double[][,] matrices = new double[3][,];

            for (int i = 0; i < 3; i++)
            {
                matrices[i] = new double[size, size];
            }

            for (int x = 0; x < size; x++)
            {
                for (int y = 0; y < size; y++)
                {
                    matrices[0][x, y] = b.GetPixel(x, y).R - 128;
                    matrices[1][x, y] = b.GetPixel(x, y).G - 128;
                    matrices[2][x, y] = b.GetPixel(x, y).B - 128;
                }
            }
            PrintMatrix(matrices[0]);
            return matrices;
        }

        //Run the DCT2D on 3-channeled group of matrices
        public void DCTMatrices(double[][,] matrices)
        {
            Parallel.For(0, 3, i =>
            {
                matrices[i] = DCT2D(matrices[i]);
            });
        }

        //Run the inverse DCT2D on 3-channeled group of matrices
        public void IDCTMatrices(double[][,] matrices)
        {
            Parallel.For(0, 3, i =>
            {
                matrices[i] = IDCT2D(matrices[i]);
            });
        }

        //Run a DCT2D on a single matrix
        public double[,] DCT2D(double[,] input)
        {
            double[,] coeffs = new double[size, size];

            //To initialise every [u,v] value in the coefficient table...
            for (int u = 0; u < size; u++)
            {
                for (int v = 0; v < size; v++)
                {
                    //...sum the basisfunction for every [x,y] value in the bitmap input
                    double sum = 0d;



                    for (int x = 0; x < size; x++)
                    {
                        for (int y = 0; y < size; y++)
                        {
                            double a = input[x, y];
                            sum += BasisFunction(a, u, v, x, y);
                        }
                    }
                    coeffs[u, v] = sum * 0.25d * alpha(u) * alpha(v);
                }
            }
            return coeffs;
        }

        //Run an inverse DCT2D on a single matrix
        public double[,] IDCT2D(double[,] coeffs)
        {
            double[,] output = new double[size, size];

            //To initialise every [x,y] value in the bitmap output...
            for (int x = 0; x < size; x++)
            {
                for (int y = 0; y < size; y++)
                {
                    //...sum the basisfunction for every [u,v] value in the coefficient table
                    double sum = 0d;

                    for (int u = 0; u < size; u++)
                    {
                        for (int v = 0; v < size; v++)
                        {
                            double a = coeffs[u, v];
                            sum += BasisFunction(a, u, v, x, y) * alpha(u) * alpha(v);
                        }
                    }

                    output[x, y] = sum * 0.25d;
                }
            }
            return output;
        }

        public double BasisFunction(double a, double u, double v, double x, double y)
        {
            double b = Math.Cos(((2d * x + 1d) * u * Math.PI) / 16d);
            double c = Math.Cos(((2d * y + 1d) * v * Math.PI) / 16d);

            return a * b * c;
        }


        private void PrintMatrix(double[,] matrix)
        {
            return;
            for (int u = 0; u < size; u++)
            {
                for (int v = 0; v < size; v++)
                {
                    Console.Write(Math.Round(matrix[u, v], 2) + " ");
                }
                Console.WriteLine();
            }
        }

        private double alpha(int u)
        {
            if (u == 0)
                return 1 / Math.Sqrt(2);
            return 1;
        }

    }
}
