using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
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
            Bitmap b = new Bitmap(size, size);
            for (int x = 0; x < size; x++)
            {
                for (int y = 0; y < size; y++)
                {
                    byte R = (byte)(array[0][x, y]);
                    byte G = (byte)(array[1][x, y]);
                    byte B = (byte)(array[2][x, y]);
                    b.SetPixel(x, y, Color.FromArgb(R, G, B));
                }
            }
            return b;
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
                    matrices[0][x, y] = b.GetPixel(x, y).R / 255d;
                    matrices[1][x, y] = b.GetPixel(x, y).G / 255d;
                    matrices[2][x, y] = b.GetPixel(x, y).B / 255d;
                }
            }
            return matrices;
        }

        //Run the DCT2D on 3-channeled group of matrices
        public void DCTMatrices(double[][,] matrices)
        {
            for (int i = 0; i < 3; i++)
            {
                matrices[i] = DCT2D(matrices[i]);
            }
        }

        //Run the inverse DCT2D on 3-channeled group of matrices
        public void IDCTMatrices(double[][,] matrices)
        {
            for (int i = 0; i < 3; i++)
            {
                matrices[i] = IDCT2D(matrices[i]);
            }
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
                    coeffs[u, v] = sum;
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
                            sum += BasisFunction(a, u, v, x, y);
                        }
                    }

                    output[x, y] = sum;
                }
            }
            return output;
        }

        //The basis function for the DCT
        //a is the value of the matrix cell in question
        public double BasisFunction(double a, double u, double v, double x, double y)
        {
            double b = Math.Cos((Math.PI * (x + 0.5d) * u) / size);
            double c = Math.Cos((Math.PI * (y + 0.5d) * v) / size);

            double d = 16d / size;//normally listed as constant '2', but this accounts for change in matrix size. not sure why.

            return a*b*c*d;
        }
    }
}
