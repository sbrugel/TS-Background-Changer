using Microsoft.Win32;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;

namespace TS_Background_Changer
{
    public partial class Form1 : Form
    {
        string sourcePic;
        string folderOfPic;
        string gameDirectory;
        bool deleteSourcePic;
        Image temp; //for png's
        readonly string startupPath = System.IO.Directory.GetCurrentDirectory();
        readonly string dirfile;

        public Form1()
        {
            InitializeComponent();
            // get TS path from registry
            try
            {
                RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\WOW6432Node\Railsimulator.com\RailWorks");
                object objRegisteredValue = key.GetValue("EXE_Path");
                gameDirectory = objRegisteredValue.ToString().Substring(0, objRegisteredValue.ToString().Length - 13); //remove executable name
            }
            catch (Exception ex)
            {
                gameDirectory = @"C:\Program Files (x86)\Steam\steamapps\common\RailWorks";
            }
            textBox2.Text = gameDirectory;
        }

        private void Button1_Click(object sender, EventArgs e) // open image
        {
            openFileDialog1.Filter = "Image Files (JPG/PNG)|*.JPG;*.PNG;";
            DialogResult result = openFileDialog1.ShowDialog(); // Show the dialog.
            if (result == DialogResult.OK)
            {
                // set picture, directory info, and picture info
                sourcePic = @openFileDialog1.FileName;
                textBox1.Text = sourcePic;
                pictureBox1.ImageLocation = sourcePic;

                // convert picture to an Image to get dimensions
                System.Drawing.Image img = System.Drawing.Image.FromFile(@sourcePic);
                if (img.Width == 1920 && img.Height == 1080)
                {
                    ChangeButton.BackColor = Color.LightGreen;
                } else if ((img.Width % 16 == 0) && (img.Height % 9 == 0))
                {
                    ChangeButton.BackColor = Color.LightYellow;
                } else
                {
                    ChangeButton.BackColor = Color.LightPink;
                }
                folderOfPic = Path.GetDirectoryName(sourcePic);
                label4.Text = "";
            }
        }

        private void Button2_Click(object sender, EventArgs e) // set RW directory
        {
            DialogResult result = folderBrowserDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                gameDirectory = @folderBrowserDialog1.SelectedPath;
                textBox2.Text = gameDirectory;
            }
        }

        private void Button3_Click(object sender, EventArgs e) // change image
        {
            if (File.Exists(sourcePic))
            {
                // copy the original file over
                string fileName = Path.GetFileName(sourcePic);
                string bgFile = gameDirectory + "\\data\\textures\\frontend\\background.jpg";

                ImageCodecInfo myImageCodecInfo = GetEncoderInfo("image/jpeg");
                EncoderParameter myEncoderParameter;
                EncoderParameters myEncoderParameters = new EncoderParameters(1);

                // resize the selected image
                Image i = Image.FromFile(sourcePic);
                i = ResizeImage(i, 1920, 1080);
                Encoder myEncoder = Encoder.Quality;
                myEncoderParameter = new EncoderParameter(myEncoder, 75L);
                myEncoderParameters.Param[0] = myEncoderParameter;
                i.Save(folderOfPic + "\\temp.jpg", myImageCodecInfo, myEncoderParameters);

                // replace background with the image
                if (File.Exists(bgFile))
                {
                    // there is already a background.jpg file so overwrite
                    FileAttributes attributes = File.GetAttributes(bgFile);
                    attributes = RemoveAttribute(attributes, FileAttributes.ReadOnly);
                    File.SetAttributes(bgFile, attributes);
                    File.Delete(bgFile);
                    File.Copy(sourcePic, bgFile);
                    attributes = AddAttribute(attributes, FileAttributes.ReadOnly);
                    File.SetAttributes(bgFile, attributes);
                }
                else
                {
                    // the background.jpg file does not exist so copy the resized image over and rename it
                    System.IO.File.Copy(sourcePic, bgFile);
                    FileAttributes attributes = File.GetAttributes(bgFile);
                    attributes = AddAttribute(attributes, FileAttributes.ReadOnly);
                    File.SetAttributes(bgFile, attributes);
                }

                // cleanup unneeded files
                File.Delete(folderOfPic + "\\temp.jpg");
                label4.Text = "Background successfully changed!";
            }
        }

        public static Bitmap ResizeImage(Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }

        private static ImageCodecInfo GetEncoderInfo(String mimeType)
        {
            int j;
            ImageCodecInfo[] encoders;
            encoders = ImageCodecInfo.GetImageEncoders();
            for (j = 0; j < encoders.Length; ++j)
            {
                if (encoders[j].MimeType == mimeType)
                    return encoders[j];
            }
            return null;
        }

        private static FileAttributes RemoveAttribute(FileAttributes attributes, FileAttributes attributesToRemove)
        {
            return attributes & ~attributesToRemove;
        }

        private static FileAttributes AddAttribute(FileAttributes attributes, FileAttributes attributesToAdd)
        {
            return attributes | attributesToAdd;
        }

        private void TextBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }
    }
}