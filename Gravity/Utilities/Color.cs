using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gravity.Utilities.ColorUtilities
{
    public struct RGB
    {
        public byte R, G, B, A;

        public RGB(byte r, byte g, byte b, byte a = 255)
        {
            R = r;
            G = g;
            B = b;
            A = a;
        }

        public RGB(Color color)
        {
            R = color.R;
            G = color.G;
            B = color.B;
            A = color.A;
        }
        public static explicit operator RGB(Color color) => new RGB(color);

        public Color ToColor()
        {
            return new Color(R, G, B, A);
        }
        public static implicit operator Color(RGB rgb) => rgb.ToColor();

        public RGB(HSV hsv)
        {
            hsv = hsv.Clamp();
            float C = hsv.S * hsv.V; // Chroma
            float HPrime = (hsv.H / 60) % 6.0f;
            float X = C * (1 - Math.Abs(HPrime % 2.0f - 1));
            float M = hsv.V - C;

            float Rf = 0.0f;
            float Gf = 0.0f;
            float Bf = 0.0f;

            switch ((int)HPrime)
            {
                case 0: Rf = C; Gf = X;         break; // [0, 1)
                case 1: Rf = X; Gf = C;         break; // [1, 2)
                case 2:         Gf = C; Bf = X; break; // [2, 3)
                case 3:         Gf = X; Bf = C; break; // [3, 4)
                case 4: Rf = X;         Bf = C; break; // [4, 5)
                case 5: Rf = C;         Bf = X; break; // [5, 6)
            }

            Rf += M;
            Gf += M;
            Bf += M;
            
            R = (byte)Math.Round(Rf * 255);
            G = (byte)Math.Round(Gf * 255);
            B = (byte)Math.Round(Bf * 255);
            A = hsv.A;
        }
        public static explicit operator RGB(HSV hsv) => new RGB(hsv);

        public HSV ToHSV()
        {
            return new HSV(this);
        }
        public static implicit operator HSV(RGB rgb) => rgb.ToHSV();

        public RGB(HSL hsl)
        {
            hsl = hsl.Clamp();
            float C = (1 - Math.Abs(2 * hsl.L - 1)) * hsl.S; // Chroma
            float HPrime = hsl.H / 60; // H'
            float X = C * (1 - Math.Abs(HPrime % 2.0f) - 1);
            float M = hsl.L - C / 2;

            float Rf = 0.0f;
            float Gf = 0.0f;
            float Bf = 0.0f;

            switch ((int)HPrime)
            {
                case 0: Rf = C; Gf = X;         break; // [0, 1)
                case 1: Rf = X; Gf = C;         break; // [1, 2)
                case 2:         Gf = C; Bf = X; break; // [2, 3)
                case 3:         Gf = X; Bf = C; break; // [3, 4)
                case 4: Rf = X;         Bf = C; break; // [4, 5)
                case 5: Rf = C;         Bf = X; break; // [5, 6)
            }

            Rf += M;
            Gf += M;
            Bf += M;

            R = (byte)Math.Round(Rf * 255);
            G = (byte)Math.Round(Gf * 255);
            B = (byte)Math.Round(Bf * 255);
            A = hsl.A;
        }
        public static explicit operator RGB(HSL hsl) => new RGB(hsl);

        public HSL ToHSL()
        {
            return new HSL(this);
        }
        public static implicit operator HSL(RGB rgb) => rgb.ToHSL();
    }

    public struct HSV
    {
        public float H, S, V;
        public byte A;

        public HSV(float h, float s, float v, byte a = 255)
        {
            H = h;
            S = s;
            V = v;
            A = a;
        }

        public HSV(Color color)
        {
            this = (RGB)color;
        }
        public static explicit operator HSV(Color color) => new HSV(color);

        public Color ToColor()
        {
            return new RGB(this);
        }
        public static implicit operator Color(HSV hsv) => hsv.ToColor();

        public HSV(RGB rgb)
        {
            H = 0;
            S = 0;
            V = 0;
            A = rgb.A;

            float R = rgb.R / 255.0f;
            float G = rgb.G / 255.0f;
            float B = rgb.B / 255.0f;

            float M = MathUtilities.Max(R, G, B);
            float m = MathUtilities.Min(R, G, B);
            float C = M - m; // Chroma

            if (C != 0.0f)
            {
                if (M == rgb.R)
                    H = ((rgb.G - rgb.B) / C) % 6.0f;
                else if (M == rgb.G)
                    H = ((rgb.B - rgb.R) / C) + 2;
                else if (M == rgb.B)
                    H = ((rgb.R - rgb.G) / C) + 4;

                H *= 60;
            }

            if (H < 0.0f)
                H += 360;

            V = M;

            if (V != 0.0f)
                S = C / V;
        }
        public static explicit operator HSV(RGB rgb) => new HSV(rgb);

        public RGB ToRGB()
        {
            return new RGB(this);
        }
        public static implicit operator RGB(HSV hsv) => hsv.ToRGB();

        public HSV(HSL hsl)
        {
            hsl = hsl.Clamp();
            float sat = hsl.S * hsl.L < 0.5 ? hsl.L : 1 - hsl.L;

            H = hsl.H;
            S = 2 * sat / (hsl.L + sat);
            V = hsl.L + sat;
            A = hsl.A;
        }
        public static explicit operator HSV(HSL hsl) => new HSV(hsl);

        public HSL ToHSL()
        {
            return new HSL(this);
        }
        public static implicit operator HSL(HSV hsv) => hsv.ToHSL();

        public HSV Clamp()
        {
            return new HSV
            {
                H = Math.Clamp(H, 0, 360),
                S = Math.Clamp(S, 0, 1),
                V = Math.Clamp(V, 0, 1),
                A = A
            };
        }
    }

    public struct HSL
    {
        public float H, S, L;
        public byte A;

        public HSL(float h, float s, float l, byte a = 255)
        {
            H = h;
            S = s;
            L = l;
            A = a;
        }

        public HSL(Color color)
        {
            this = (RGB)color;
        }
        public static explicit operator HSL(Color color) => new HSL(color);

        public Color ToColor()
        {
            return new RGB(this);
        }
        public static implicit operator Color(HSL hsl) => hsl.ToColor();

        public HSL(RGB rgb)
        {
            H = 0;
            S = 0;
            L = 0;
            A = rgb.A;

            float R = rgb.R / 255.0f;
            float G = rgb.G / 255.0f;
            float B = rgb.B / 255.0f;

            float M = MathUtilities.Max(R, G, B);
            float m = MathUtilities.Min(R, G, B);
            float C = M - m; // Chroma

            if (C != 0.0f)
            {
                if (M == R)
                    H = ((G - B) / C) % 6.0f;
                else if (M == G)
                    H = ((B - R) / C) + 2;
                else if (M == B)
                    H = ((R - G) / C) + 4;

                H *= 60;
            }

            if (H < 0.0f)
                H += 360;

            L += (M + m) / 2;

            if (L != 1.0f && L != 0.0f)
                S = C / (1 - Math.Abs(2 * L - 1));
        }
        public static explicit operator HSL(RGB rgb) => new HSL(rgb);

        public RGB ToRGB()
        {
            return new RGB(this);
        }
        public static implicit operator RGB(HSL hsl) => hsl.ToRGB();

        public HSL(HSV hsv)
        {
            hsv = hsv.Clamp();
            float tmp = (2 - hsv.S) * hsv.V;

            H = hsv.H;
            S = hsv.S * hsv.V / (tmp < 1 ? hsv.H : 2 - hsv.H);
            L = hsv.H / 2;
            A = hsv.A;
        }
        public static explicit operator HSL(HSV hsv) => new HSL(hsv);

        public HSV ToHSV()
        {
            return new HSV(this);
        }
        public static implicit operator HSV(HSL hsl) => hsl.ToHSV();

        public HSL Clamp()
        {
            return new HSL
            {
                H = Math.Clamp(H, 0, 360),
                S = Math.Clamp(S, 0, 1),
                L = Math.Clamp(L, 0, 1),
                A = A
            };
        }
    }
}
