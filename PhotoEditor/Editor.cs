using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace PhotoEditor_Temp
{
    class Editor
    {
        static PictureBox Edited;
        static PictureBox Origin;
        static Form form1;
        static TrackBar BrightnessBar;
        static TrackBar SaturationBar;
        static Label filename;
        const float rwgt = 0.3086f;
        const float gwgt = 0.6094f;
        const float bwgt = 0.0820f;

        static void LoadButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Filter = "Image Files(*.jpg; *.jpeg; *.gif; *.bmp)|*.jpg; *.jpeg; *.gif; *.bmp";
            if (open.ShowDialog() == DialogResult.OK)
            {
                Origin.Image = new Bitmap(open.FileName);
            }
        }

        static void SaveButton_Click(object sender, EventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                int width = Convert.ToInt32(Edited.Width);
                int height = Convert.ToInt32(Edited.Height);
                using (Bitmap bmp = new Bitmap(width, height))
                {
                    Edited.DrawToBitmap(bmp, new Rectangle(0, 0, width, height));
                    bmp.Save(dialog.FileName, ImageFormat.Jpeg);
                }
            }
        }

        static Bitmap BS_Adjust(Image image, float brightness, float saturation)
        {
            Image temp = AdjustBrightness(image, brightness);
            temp = Saturation_Adjust(temp, saturation);
            return (Bitmap)temp;
        }

        static private Bitmap AdjustBrightness(Image image, float brightness)
        {
            float b = brightness;
            ColorMatrix cm = new ColorMatrix(new float[][]
                {
                    new float[] {b, 0, 0, 0, 0},
                    new float[] {0, b, 0, 0, 0},
                    new float[] {0, 0, b, 0, 0},
                    new float[] {0, 0, 0, 1, 0},
                    new float[] {0, 0, 0, 0, 1},
                });
            ImageAttributes attributes = new ImageAttributes();
            attributes.SetColorMatrix(cm);

            Point[] points =
            {
                new Point(0, 0),
                new Point(image.Width, 0),
                new Point(0, image.Height),
            };
            Rectangle rect = new Rectangle(0, 0, image.Width, image.Height);

            Bitmap bm = new Bitmap(image.Width, image.Height);
            using (Graphics gr = Graphics.FromImage(bm))
            {
                gr.DrawImage(image, points, rect, GraphicsUnit.Pixel, attributes);
            }

            return bm;
        }

        static Bitmap Saturation_Adjust(Image image, float saturation)
        {
            float baseSat = 1.0f - saturation;
            ColorMatrix colorMatrix = new ColorMatrix();
            colorMatrix[0, 0] = baseSat * rwgt + saturation;
            colorMatrix[0, 1] = baseSat * rwgt;
            colorMatrix[0, 2] = baseSat * rwgt;
            colorMatrix[1, 0] = baseSat * gwgt;
            colorMatrix[1, 1] = baseSat * gwgt + saturation;
            colorMatrix[1, 2] = baseSat * gwgt;
            colorMatrix[2, 0] = baseSat * bwgt;
            colorMatrix[2, 1] = baseSat * bwgt;
            colorMatrix[2, 2] = baseSat * bwgt + saturation;

            ImageAttributes attributes = new ImageAttributes();
            attributes.SetColorMatrix(colorMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);

            Point[] points =
            {
                new Point(0, 0),
                new Point(image.Width, 0),
                new Point(0, image.Height),
            };
            Rectangle rect = new Rectangle(0, 0, image.Width, image.Height);

            Bitmap bm = new Bitmap(image.Width, image.Height);
            using (Graphics gr = Graphics.FromImage(bm))
            {
                gr.DrawImage(image, points, rect, GraphicsUnit.Pixel, attributes);
            }
            return bm;
        }

        static void BrightnessBar_Scroll(object sender, EventArgs e)
        {
            Edited.Image = BS_Adjust(Origin.Image, (float)BrightnessBar.Value / 100, (float)SaturationBar.Value/100);
        }

        static void SaturationBar_Scroll(object sender, EventArgs e)
        {
            Edited.Image = BS_Adjust(Origin.Image, (float)BrightnessBar.Value / 100, (float)SaturationBar.Value / 100);
        }

        static void ResetButton_Click(object sender, EventArgs e)
        {
            Edited.Image = Origin.Image;
            filename.Text = "Filename:";
        }

        [STAThread]
        static void Main(string[] args)
        {
            form1 = new Form { Width = 1024, Height = 700, Text = "Photo Editor", StartPosition = FormStartPosition.CenterScreen };
            filename = new Label { Text = "Filename:", Top = 15, Left = 15, Font = new Font(new FontFamily("Arial"), 10) };

            Edited = new PictureBox { Top = 60, Left = 50, Width = 400, Height = 400, BackColor = Color.LightGray, SizeMode = PictureBoxSizeMode.StretchImage };
            Origin = new PictureBox { Top = 60, Left = 500, Width = 400, Height = 400, BackColor = Color.LightGray, SizeMode = PictureBoxSizeMode.StretchImage };

            BrightnessBar = new TrackBar { Width = 512, TickFrequency = 10, Top = 490, Left = 200 };
            BrightnessBar.SetRange(0, 200);
            BrightnessBar.Scroll += new EventHandler(BrightnessBar_Scroll);

            SaturationBar = new TrackBar { Width = 512, TickFrequency = 15, Top = 540, Left = 200 };
            SaturationBar.SetRange(-200, 200);
            SaturationBar.Scroll += new EventHandler(SaturationBar_Scroll);

            Label brightness = new Label { Text = "Brightness:", Top = 500, Left = 15, Font = new Font(new FontFamily("Arial"), 10) };
            Label saturation = new Label { Text = "Saturation:", Top = 550, Left = 15, Font = new Font(new FontFamily("Arial"), 10) };

            Button LoadButton = new Button { Left = 800, Top = 490, Text = "Load", Height = 50, Width = 50 };
            Button SaveButton = new Button { Left = 800, Top = 550, Text = "Save", Height = 50, Width = 50 };
            Button ResetButton = new Button { Left = 900, Top = 490, Text = "Reset", Height = 80, Width = 80 };

            LoadButton.Click += new EventHandler(LoadButton_Click);
            SaveButton.Click += new EventHandler(SaveButton_Click);
            ResetButton.Click += new EventHandler(ResetButton_Click);

            form1.Controls.Add(filename);
            form1.Controls.Add(brightness);
            form1.Controls.Add(saturation);

            form1.Controls.Add(BrightnessBar);
            form1.Controls.Add(SaturationBar);

            form1.Controls.Add(Edited);
            form1.Controls.Add(Origin);

            form1.Controls.Add(LoadButton);
            form1.Controls.Add(SaveButton);
            form1.Controls.Add(ResetButton);
            Application.Run(form1);
        }
    }
}
