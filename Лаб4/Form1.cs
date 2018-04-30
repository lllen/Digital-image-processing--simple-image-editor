using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Лаб4
{
    public partial class Form1 : Form
    {
        public Bitmap img { get; set; }
        public Bitmap imgSolar { get; set; }
        public Bitmap imgWhiteBlack { get; set; }
        public Bitmap imgGistogramLightness { get; set; }
        public int height { get; set; }
        public int width { get; set; }
        private RGBcolor RGBcolor;
        private HSLconvertor HSLconvertor;


        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e) // соляризація
        {
            imgSolar = new Bitmap(img);
            HSLconvertor.toHSL(RGBcolor, height, width);
            RGBcolor rgbNew = new RGBcolor();
            rgbNew = HSLconvertor.toRGB(RGBcolor.A); // new rgb color

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    imgSolar.SetPixel(x, y, Color.FromArgb(rgbNew.A[x, y], rgbNew.R[x, y], rgbNew.G[x, y], rgbNew.B[x, y]));
                }
            }
            pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox2.Image = imgSolar;
        }

        private void button2_Click(object sender, EventArgs e) // вибрати зображення
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files(*.BMP,*.JPG,*.PNG,*.GIF)|*.BMP,*.JPG,*.PNG,*.GIF|All files(*.*)|*.*";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                startApp(openFileDialog);
            }
        }

        public void startApp(OpenFileDialog openFileDialog) // стартова точка
        {
            Initialiser(openFileDialog);
            findRGBarrays();
        }

        public void Initialiser(OpenFileDialog openFileDialog)
        {
            HSLconvertor = new HSLconvertor();
            this.img = new Bitmap(openFileDialog.FileName);
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.Image = img;

            this.width = img.Width;
            this.height = img.Height;

            RGBcolor = new RGBcolor();
            RGBcolor.A = new int[width, height];
            RGBcolor.R = new int[width, height];
            RGBcolor.G = new int[width, height];
            RGBcolor.B = new int[width, height];
        }

        public void findRGBarrays() // знаходження rgb зображення
        {
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    Color pixel = img.GetPixel(x, y);
                    RGBcolor.A[x, y] = pixel.A;
                    RGBcolor.R[x, y] = pixel.R;
                    RGBcolor.B[x, y] = pixel.B;
                    RGBcolor.G[x, y] = pixel.G;
                }
            }
        }

        private void button5_Click(object sender, EventArgs e) // чорно біле зображення
        {
            imgWhiteBlack = new Bitmap(img);
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    int grey = (RGBcolor.R[x, y] + RGBcolor.G[x, y] + RGBcolor.B[x, y]) / 3;
                    imgWhiteBlack.SetPixel(x, y, Color.FromArgb(RGBcolor.A[x, y], grey, grey, grey));
                }
            }
            pictureBox5.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox5.Image = imgWhiteBlack;
        }

        private void button4_Click(object sender, EventArgs e) // лінеаризація
        {

        }

        private void button3_Click(object sender, EventArgs e) // гістограма
        {
           
            
            int tempL;
            int[] Lcount = new int[1000];
            int N = this.height + this.width;

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    tempL = (int)(this.HSLconvertor.L[x, y]*100);
                    Lcount[tempL]++;
                }
            }
            long t = Lcount.Max();
            imgGistogramLightness = new Bitmap(256, Lcount.Max());

            for (int y = imgGistogramLightness.Width-1; y >= 0 ; y--)
            {
                for (int x = 0; x < Lcount[y]-1; x++)
                {
                   imgGistogramLightness.SetPixel(y,x,Color.White);
                   
                    
                }
            }
            imgGistogramLightness.RotateFlip(RotateFlipType.RotateNoneFlipY);
            pictureBox7.Image = imgGistogramLightness;

       
        }
    }
}
