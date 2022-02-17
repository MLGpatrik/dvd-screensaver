using System;
using System.Drawing;

namespace dvd_screensaver
{
    public delegate void HitBoundarieDelegate();
    public class MovingObject 
    {
        private Bitmap bmp;
        private Point location;
        private int width, height;
        public event HitBoundarieDelegate BoundarieHitEvent;
        public int Bound_X, Bound_Y;
        private Velocity velocity;
        public Bitmap BMP { get { return bmp; } set { return; } }       
        public bool RGB { get; set; }
        public int Width { get { return width; } set { width = value; } }
        public int Height { get { return height; } set { height = value; } }
        public Point Location { get { return location; } set { return; } }

        public MovingObject(String path,int Bound_X,int Bound_Y)
        {
            velocity = new Velocity();
            bmp = (Bitmap)Bitmap.FromFile(path);
            this.Bound_X = Bound_X;
            this.Bound_Y = Bound_Y;
            location = new Point(0, 0);
            velocity.X = -5;
            velocity.Y = 5;
            width = 300;
            height = 150;
            BoundarieHitEvent += ChangeDirection;
        }

        public MovingObject(int Bound_X, int Bound_Y)
        {
            velocity = new Velocity();
            Random rnd = new Random();
            bmp = dvd_screensaver.Properties.Resources.logo;
            width = 300;
            height = 150;
            this.Bound_X = Bound_X;
            this.Bound_Y = Bound_Y;
            location = new Point(rnd.Next() % (Bound_X -width), rnd.Next() % (Bound_Y -height));
            if ((rnd.Next() % 101) <= 50)
                velocity.X = 5;
            else
                velocity.X = -5;

            if ((rnd.Next() % 101) <= 50)
                velocity.Y = 5;
            else
                velocity.Y = -5;
            BoundarieHitEvent += ChangeDirection;
        }

        public void Move()
        {
            try
            {
                Step();
            }catch(ArgumentOutOfRangeException ex)
            {
                BoundarieHitEvent?.Invoke();                
            }
        }

        public Color GetColorSample()
        {
            return bmp.GetPixel(100, 20);
        }

        private void ChangeDirection()
        {
            if (location.X + velocity.X + width > Bound_X || location.X + velocity.X < 0)
                velocity.Invert_X();
            if (location.Y + velocity.Y + height > Bound_Y || location.Y + velocity.Y < 0)
                velocity.Invert_Y();
        }

        private void Step()
        {
            if (location.X + velocity.X + width > Bound_X || location.X + velocity.X < 0 
                || location.Y + velocity.Y + height > Bound_Y || location.Y + velocity.Y < 0)
                throw new ArgumentOutOfRangeException();
            location.X += velocity.X;
            location.Y += velocity.Y;
        }
    }
}
