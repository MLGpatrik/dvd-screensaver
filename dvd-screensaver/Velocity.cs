namespace dvd_screensaver
{
    class Velocity
    {
        public int X { get; set; }
        public int Y { get; set; }

        public void Invert_X()
        {
            X *= -1;
        }
        public void Invert_Y()
        {
            Y *= -1;
        }
        public override string ToString()
        {
            return "X: "+X+" Y: "+Y;
        }
    }
}
