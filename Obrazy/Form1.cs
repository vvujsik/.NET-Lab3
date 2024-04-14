using System.Windows.Forms;

namespace Obrazy
{
    public partial class Form1 : Form
    {
        private Bitmap? image;
        Thread[] threads;
        public Form1()
        {
            InitializeComponent();
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox3.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox4.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox5.SizeMode = PictureBoxSizeMode.StretchImage;
            threads = new Thread[4];
        }

        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
            string file = openFileDialog1.FileName;
            if (file != null)
            {
                image = new Bitmap(file);
                pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                pictureBox1.Image = image;
            }
            else
            {
                MessageBox.Show("Nie udalo sie zaladowac pliku");
            }
            pictureBox2.Image = null;
            pictureBox3.Image = null;
            pictureBox4.Image = null;
            pictureBox5.Image = null;
        }
        void ApplyNegativeFilter(Bitmap original)
        {
            Bitmap resultImage = new Bitmap(original.Width, original.Height);

            for (int x = 0; x < resultImage.Width; x++)
            {
                for (int y = 0; y < resultImage.Height; y++)
                {
                    Color originalColor = original.GetPixel(x, y);
                    Color negativeColor = Color.FromArgb(255 - originalColor.R, 255 - originalColor.G, 255 - originalColor.B);
                    resultImage.SetPixel(x, y, negativeColor);
                }
            }
            pictureBox2.Image = resultImage;
        }

        void ToGrayscale(Bitmap original)
        {
            Bitmap grayscaleImage = new Bitmap(original.Width, original.Height);

            for (int y = 0; y < original.Height; y++)
            {
                for (int x = 0; x < original.Width; x++)
                {
                    Color pixelColor = original.GetPixel(x, y);
                    int grayscaleValue = (int)(pixelColor.R * 0.3 + pixelColor.G * 0.59 + pixelColor.B * 0.11);
                    Color newColor = Color.FromArgb(grayscaleValue, grayscaleValue, grayscaleValue);
                    grayscaleImage.SetPixel(x, y, newColor);
                }
            }
            pictureBox3.Image= grayscaleImage;
        }
        private Bitmap ToGrayscale_r(Bitmap original)
        {
            Bitmap grayscaleImage = new Bitmap(original.Width, original.Height);

            for (int y = 0; y < original.Height; y++)
            {
                for (int x = 0; x < original.Width; x++)
                {
                    Color pixelColor = original.GetPixel(x, y);
                    int grayscaleValue = (int)(pixelColor.R * 0.3 + pixelColor.G * 0.59 + pixelColor.B * 0.11);
                    Color newColor = Color.FromArgb(grayscaleValue, grayscaleValue, grayscaleValue);
                    grayscaleImage.SetPixel(x, y, newColor);
                }
            }
            return grayscaleImage;
        }

        void DetectEdgesSobel(Bitmap originalImage)
        {
            Bitmap edgeImage = new Bitmap(originalImage.Width, originalImage.Height);

            // Konwersja obrazu do tablicy pikseli
            Color[,] pixels = new Color[originalImage.Width, originalImage.Height];
            for (int y = 0; y < originalImage.Height; y++)
            {
                for (int x = 0; x < originalImage.Width; x++)
                {
                    pixels[x, y] = originalImage.GetPixel(x, y);
                }
            }

            // Detekcja krawêdzi za pomoc¹ operatora Sobela
            for (int y = 1; y < originalImage.Height - 1; y++)
            {
                for (int x = 1; x < originalImage.Width - 1; x++)
                {
                    // Obliczanie gradientów Sobela dla kana³ów R, G, B
                    int gradientX_R = (-pixels[x - 1, y - 1].R + pixels[x + 1, y - 1].R
                                    - 2 * pixels[x - 1, y].R + 2 * pixels[x + 1, y].R
                                    - pixels[x - 1, y + 1].R + pixels[x + 1, y + 1].R);

                    int gradientY_R = (-pixels[x - 1, y - 1].R - 2 * pixels[x, y - 1].R - pixels[x + 1, y - 1].R
                                    + pixels[x - 1, y + 1].R + 2 * pixels[x, y + 1].R + pixels[x + 1, y + 1].R);

                    int gradient_R = Math.Abs(gradientX_R) + Math.Abs(gradientY_R);
                    gradient_R = Math.Min(255, gradient_R); // Ograniczenie wartoœci do 255


                    // Ustawienie koloru krawêdzi na obrazie wyjœciowym
                    edgeImage.SetPixel(x, y, Color.FromArgb(gradient_R, gradient_R, gradient_R));
                }
            }
            pictureBox5.Image = edgeImage;
        }

        void mirror(Bitmap originalImage)
        {
            Bitmap mirror_img = new Bitmap(originalImage.Width, originalImage.Height);
            // Iteracja po ka¿dym wierszu obrazu
            for (int y = 0; y < mirror_img.Height; y++)
            {
                // Iteracja po ka¿dym pikselu w wierszu (od prawej do lewej)
                for (int x = 0; x < mirror_img.Width; x++)
                {
                    // Odczytanie koloru piksela z oryginalnego obrazu
                    Color originalColor = originalImage.GetPixel(x, y);

                    // Obliczenie odwrotnego indeksu piksela w wierszu (odbicie lustrzane)
                    int mirroredX = mirror_img.Width - 1 - x;

                    // Ustawienie koloru odbitego piksela na nowej mapie bitowej
                    mirror_img.SetPixel(mirroredX, y, originalColor);
                }
            }
            pictureBox4.Image = mirror_img;
        }
    

        private void button2_Click(object sender, EventArgs e)
        {
            List<Bitmap> imageCopies = new List<Bitmap>();
            for (int i = 0; i < 4; i++)
            {
                imageCopies.Add(new Bitmap(image));
            }
            threads[0] = new Thread(() => ApplyNegativeFilter(imageCopies[0]));
            threads[0].Start();

            threads[1] = new Thread(() => ToGrayscale(imageCopies[1]));
            threads[1].Start();

            threads[2] = new Thread(() => mirror(imageCopies[2]));
            threads[2].Start();

            threads[3] = new Thread(() => DetectEdgesSobel(imageCopies[3]));
            threads[3].Start();

            //Bitmap image_grey = ToGrayscale(image);
            //pictureBox3.Image = image_grey;

            //Bitmap edges = DetectEdges(image);
            //pictureBox4.Image = edges;
        }
    }
}
