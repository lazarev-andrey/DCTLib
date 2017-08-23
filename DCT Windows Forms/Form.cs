using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CosineTransform
{
	public partial class Form: System.Windows.Forms.Form
	{
		public Form()
		{
			InitializeComponent();
		}

		private void button1_Click( object sender, EventArgs e )
		{
			openFileDialog1.ShowDialog();
		}

		private void button3_Click( object sender, EventArgs e )
		{
			if (pictureBox2.Image == null)
			{
				MessageBox.Show( "Processed image is not created yet.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error );
				return;
			}

			saveFileDialog1.ShowDialog();
		}

		TransformController _controller = new TransformController();

		private void button2_Click( object sender, EventArgs e )
		{
			if (pictureBox1.Image == null)
			{
				MessageBox.Show( "Input image was not selected yet.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error );
				return;
			}

			//transform using matrix trackBar1.Value x trackBar1.Value	
			pictureBox2.Image = _controller.Transfom( pictureBox1.Image, trackBar1.Value, trackBar1.Value, trackBar2.Value );		
		}

		private void openFileDialog1_FileOk( object sender, CancelEventArgs e )
		{
			pictureBox1.Image = Image.FromFile( openFileDialog1.FileName );
		}

		private void saveFileDialog1_FileOk( object sender, CancelEventArgs e )
		{
			//save as is (quality 100%)
			SaveJpeg( saveFileDialog1.FileName, new Bitmap(pictureBox2.Image), 100 );			
		}

		
		public static void SaveJpeg( string path, Bitmap image, long quality )
		{
			using (EncoderParameters encoderParameters = new EncoderParameters( 1 ))
			using (EncoderParameter encoderParameter = new EncoderParameter( System.Drawing.Imaging.Encoder.Quality, quality ))
			{
				ImageCodecInfo codecInfo = ImageCodecInfo.GetImageDecoders().First( codec => codec.FormatID == ImageFormat.Jpeg.Guid );
				encoderParameters.Param[0] = encoderParameter;
				image.Save( path, codecInfo, encoderParameters );
			}
		}

		private void trackBar1_ValueChanged( object sender, EventArgs e )
		{
			label1.Text = trackBar1.Value.ToString() + "x" + trackBar1.Value.ToString();
		}

		private void trackBar2_ValueChanged( object sender, EventArgs e )
		{
			label2.Text = trackBar2.Value.ToString();
		}

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}
