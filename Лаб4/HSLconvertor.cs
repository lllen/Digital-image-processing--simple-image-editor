using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Лаб4
{
    class HSLconvertor
    {
        public float[,] H { get; set; } // hue
        public float[,] S { get; set; } // saturation
        public float[,] L { get; set; } // lightness
        public float[,] Cmax { get; set; }
        public float[,] Cmin { get; set; }
        public float[,] delta { get; set; }
        public int height;
        public int width;
        public float h;
        public float s;
        public float l;

        public HSLconvertor() { }

        public HSLconvertor(float[,] h, float[,]s, float[,] l)
        {
            this.H = h;
            this.S = s;
            this.L = l;
        }

        public HSLconvertor toHSL(RGBcolor color, int height, int width)
        {
            this.height = height;
            this.width = width;
            float[,] R = new float[width, height];
            float[,] G = new float[width, height];
            float[,] B = new float[width, height];
            this.Cmax = new float[width, height];
            this.Cmin = new float[width, height];
            this.delta = new float[width, height];
            this.H = new float[width,height];
            this.S = new float[width, height];
            this.L = new float[width, height];

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    R[x, y] = (float)color.R[x, y] / 255; // R'
                    G[x, y] = (float)color.G[x, y] / 255; // G'
                    B[x, y] = (float)color.B[x, y] / 255; // B'
                    this.Cmax[x, y] = (float)Math.Max(R[x, y], Math.Max(G[x, y], B[x, y]));
                    this.Cmin[x, y] = (float)Math.Min(R[x, y], Math.Min(G[x, y], B[x, y]));
                    this.delta[x, y] = (float)this.Cmax[x, y] - this.Cmin[x, y];

                    // hue calculation
                    if (this.delta[x, y] == 0)
                    {
                        this.H[x, y] = 0;
                    }
                    else
                    {
                        if (this.Cmax[x, y] == R[x, y])
                        {
                            this.H[x, y] = (G[x, y] - B[x, y] / this.delta[x, y]) % 6;
                        }
                        else if (this.Cmax[x, y] == G[x, y])
                        {
                            this.H[x, y] = ((B[x, y] - R[x, y]) / this.delta[x, y]) + 2f;
                        }
                        else if (this.Cmax[x, y] == B[x, y])
                        {
                            this.H[x, y] = ((R[x, y] - G[x, y]) / this.delta[x, y]) + 4f;
                        }

                        // convert to degrees
                        this.H[x, y] *= 60f;
                        if (this.H[x, y] < 0)
                        {
                            this.H[x, y] += 360;
                        }
                    }

                    // lightness calculation
                    this.L[x, y] = ((this.Cmax[x, y] + this.Cmin[x, y]) / 2);

                    //saturation calculation
                    if (this.delta[x, y] == 0)
                    {
                        this.S[x, y] = 0;
                    }
                    else
                    {
                        this.S[x, y] = this.delta[x, y] / (1 - Math.Abs(2 * this.L[x, y] - 1));
                    }
                }
            }
              

              
            return new HSLconvertor(this.H, this.S, this.L);

        }

        public RGBcolor toRGB(int[,]A)
        {
            double R = 0;
            double G = 0;
            double B = 0;

            double C;
            double X;
            double m;
            double lnew;  

            RGBcolor color = new RGBcolor();
            color.R = new int[width, height];
            color.G = new int[width, height];
            color.B = new int[width, height];
            color.A = new int[width, height];
           
            double Lmax = this.L.Cast<float>().Max();

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    lnew = 4 / Lmax * this.L[x, y] * (Lmax - this.L[x, y]);
                    C = (1 - Math.Abs(2 * lnew - 1)) * this.S[x, y];
                    X = C * (1 - Math.Abs((this.H[x, y] / 60) % 2 - 1));
                    m = lnew - C / 2;

                    if (this.H[x,y]>=0 && this.H[x, y] < 60)
                    {
                        R = C;
                        G = X;
                        B = 0;
                    }
                    else if (this.H[x, y] >= 60 && this.H[x, y] < 120)
                    {
                        R = X;
                        G = C;
                        B = 0;
                    }
                    else if (this.H[x, y] >= 120 && this.H[x, y] < 180)
                    {
                        R = 0;
                        G = C;
                        B = X;
                    }
                    else if (this.H[x, y] >= 180 && this.H[x, y] < 240)
                    {
                        R = 0;
                        G = X; 
                        B = C;
                    }
                    else if (this.H[x, y] >= 240 && this.H[x, y] < 300)
                    {
                        R = X;
                        G = 0;
                        B = C;
                    }
                    else if (this.H[x, y] >= 300 && this.H[x, y] < 360)
                    {
                        R = C;
                        G = 0;
                        B = X;
                    }
                    color.A[x, y] = A[x, y];
                    color.R[x, y] = (int)((R + m) * 255);
                    color.G[x, y] = (int)((G + m) * 255);
                    color.B[x, y] = (int)((B + m) * 255);
                }
            }
                    return color;
        }


        
     
    }
}
