using System.Drawing;

namespace open_gl
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Game game = new Game(Console.WindowWidth, Console.WindowHeight, "LearnOpenTK");
            while (true)
            {
                Console.SetCursorPosition(0, 0);
                Console.CursorVisible = false;
                game.Paint();
            }
        }

    }
    class Game
    {
        const double FOV = 90;
        static int POV = 180;
        static int PX = 0;
        static int PY = 128;
        int Heightpic;
        int Widthpic;
        int Hei05;
        int Wid05;
        double Fovwid;
        double Rprrl;
        string Title;
        vec2[] distances;
        char[][] map;
        Bitmap Image;
        char[] pixs;

        public Game(int width, int height, string title)
        {
            Image = new Bitmap("C:\\Users\\admin\\source\\repos\\open gl\\open gl\\Bitmap1.bmp");
            Heightpic = height;
            Widthpic = width;
            Fovwid = FOV / Widthpic;
            Hei05 = Heightpic / 2;
            Wid05 = Widthpic / 2;
            Rprrl = Wid05 / Math.Tan(Math.PI * FOV / 360);
            pixs = " .:!/r(l1Z4H9W8$@".ToCharArray();
            Map();
            Thread thread = new Thread(Imp);
            thread.Start();
        }
        private static void Imp()
        {
            while (true)
            {
                if (POV > 360)
                {
                    POV -= 360;
                }
                if (POV < 0)
                {
                    POV += 360;
                }
                switch (Console.ReadKey().Key)
                {
                    case ConsoleKey.W:
                        PY -= (int)(Math.Sin(POV * Math.PI / 180) * 10);
                        PX += (int)(Math.Cos(POV * Math.PI / 180) * 10);
                        break;
                    case ConsoleKey.S:
                        PY += (int)(Math.Sin(POV * Math.PI / 180) *10);
                        PX -= (int)(Math.Cos(POV * Math.PI / 180) * 10);
                        break;
                    case ConsoleKey.A:
                        POV += 2;
                        break;
                    case ConsoleKey.D:
                        POV -= 2;
                        break;
                    default:
                        break;
                }
            }
        }
        void Map()
        {
            string smap =
            "################\n" +
            "#..............#\n" +
            "#..............#\n" +
            "#..............#\n" +
            "#.......#......#\n" +
            "#..............#\n" +
            "#..............#\n" +
            "#..............#\n" +
            "#..............#\n" +
            "#..............#\n" +
            "#..............#\n" +
            "#..............#\n" +
            "#..............#\n" +
            "#..............#\n" +
            "#..............#\n" +
            "################";
            string[] mmap = smap.Split('\n');
            map = new char[mmap.Length][];
            for (int i = 0; i < mmap.Length; i++)
            {
                map[i] = mmap[i].ToCharArray();
            }
        }
        vec2[] Renddis()
        {
            double angle = POV + FOV / 2;
            vec2[] distances = new vec2[Widthpic];
            for (int i = 0; i < Widthpic; i++)
            {
                distances[i] = Ray(angle, PX, PY);
                angle -= Fovwid;
            }
            return distances;
        }
        vec2 Ray(double Pov, int PX, int PY)
        {
            int Ey = 10000;
            int Ex = 10000;
            int xCoordinat1 = 0;
            int xCoordinat2 = 0;
            double An = Math.PI / 180;
            double Cos = Math.Cos(An * (Pov - POV));
            double Tan = Math.Tan(An * Pov);
            {
                bool up = Pov < 180 && Pov > 0;
                double Ay, Ax, Ya, Xa;
                if (up)
                    Ay = (PY / 128) * 128 - 128;
                else
                    Ay = (PY / 128) * 128 + 128;
                Ax = PX + (PY - Ay) / Tan;
                if (up)
                    Ya = -128;
                else
                    Ya = 128;
                Xa = 128 / Tan;
                for (int i = 0; i < 30; i++)
                {
                    int y = (int)(Ay / 128) + (map.Length / 2);
                    int x = (int)(Ax / 128) + (map[0].Length / 2);
                    if ((x < 0 || x > map.Length - 1) || (y < 0 || y > map[0].Length - 1))
                        break;
                    if (map[y][x] == '#')
                    {
                        int A = 256;
                        double B = Ax;
                        xCoordinat1 = (int)(128 * ((A - B) / A));
                        Ex = (int)Math.Sqrt(Math.Pow(PX - Ax, 2) + Math.Pow(PY - Ay, 2));
                        break;
                    }
                    Ax += Xa;
                    Ay += Ya;
                }
            }
            {
                bool right = Pov < 90 || Pov > 270;
                double By, Bx, Yb, Xb;
                if (right)
                    Bx = (PX / 128) * 128 + 128;
                else
                    Bx = (PX / 128) * 128 - 1;
                By = PY + (PX - Bx) * Tan;
                if (right)
                    Xb = 128;
                else
                    Xb = -128;
                Yb = 128 * Tan;
                for (int j = 0; j < 30; j++)
                {
                    int y = (int)(By / 128) + (map.Length / 2);
                    int x = (int)(Bx / 128) + (map[0].Length / 2);
                    if ((x < 0 || x > map.Length - 1) || (y < 0 || y > map[0].Length - 1))
                        break;
                    if (map[y][x] == '#')
                    {
                        Ey = (int)Math.Sqrt(Math.Pow(PX - Bx, 2) + Math.Pow(PY - By, 2));
                        int A = 256;
                        double B = By;
                        xCoordinat2 = (int)(128 * ((A - B) / A));
                        
                        break;
                    }
                    Bx += Xb;
                    By += Yb;
                }
            }
            if (Ex < Ey)
            {
                Ex = (int)Math.Round(Ex * Cos);
                return new vec2(xCoordinat1, Ex);
            }

            else
            {
                Ey = (int)Math.Round(Ey * Cos);
                return new vec2(xCoordinat2, Ey);
            }
        }
        public void Paint()
        {
            char[] rend = new char[Widthpic * Heightpic];
            Array.Fill(rend, ' ');
            distances = Renddis();
            for (int i = 0; i < Widthpic; i++)
            {
                int x = distances[i].X;
                double H = 128 / (double)distances[i].L * Rprrl;
                double РВС = H;
                if (H > Heightpic)
                {
                    H = Heightpic;
                }
                x = Math.Abs(x);
                x -= 128 * (int)(x / 128.0);
                for (int j = 0; j < H; j++)
                {
                    int yCoordinate = (int)(128 * ((((int)(H + РВС) >> 1) - j)) / РВС);
                    char pix = '№';
                    double color = Image.GetPixel(x, yCoordinate).R / 10;
                    color = clamp(color, 0, pixs.Length - 1);
                    pix = pixs[(int)color];
                    rend[i + ((int)(Hei05 - H / 2) + j) * Widthpic] = pix;
                }
            }
            Console.Write(rend);
        }
        double clamp(double value, int min, int max) { return Math.Max (Math.Min(value, max), min); }
    }
    struct vec2
    {
        public int X;
        public int L;
        public vec2(int X, int L) { this.X = X; this.L = L; }
    }
}