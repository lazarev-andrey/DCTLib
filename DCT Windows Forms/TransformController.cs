using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CosineTransform
{
	class TransformController
	{
		public Image Transfom( Image img, int w, int h, int k )
		{
			Bitmap bitmap = new Bitmap( img );
			int width = bitmap.Width;
			int height = bitmap.Height;

			Bitmap cycleBitmap = new Bitmap( width, height );
			Graphics cycleGraphics = Graphics.FromImage( cycleBitmap );    

			Bitmap coeffBitmap = new Bitmap( width, height );
			Graphics coeffGraphics = Graphics.FromImage( coeffBitmap );

			DCT d = new DCT( w, h );

			for (int y = 0; y < height / h; y++)
            {
				for (int x = 0; x < width / w; x++)
                {
					Bitmap sector = new Bitmap( w, h );

					Graphics g = Graphics.FromImage( sector );     
                    
					Rectangle dest = new Rectangle( 0, 0, w, h );

					Rectangle src = new Rectangle( x * w, y * h, w, h );

					g.DrawImage( bitmap, dest, src, GraphicsUnit.Pixel );

					double[][,] values = d.BitmapToMatrices( sector );

					double[][,] coeffs = d.DCTMatrices( values, k );

					coeffGraphics.DrawImage( d.MatricesToBitmap( coeffs, false ), src, dest, GraphicsUnit.Pixel );

					values = d.IDCTMatrices( coeffs );

					cycleGraphics.DrawImage( d.MatricesToBitmap( values ), src, dest, GraphicsUnit.Pixel );				
				}			

			}
		return cycleBitmap;
        }
	}
}
