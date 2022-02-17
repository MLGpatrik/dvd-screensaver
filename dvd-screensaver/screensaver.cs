using System;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using Microsoft.Win32;
using System.Drawing.Imaging;

namespace dvd_screensaver
{
    public partial class screensaver : Form
    {

        [DllImport("user32.dll")]
        static extern int SetWindowLong(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

        [DllImport("user32.dll", SetLastError = true)]
        static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll")]
        static extern bool GetClientRect(IntPtr hWnd, out Rectangle lpRect);



        private Point mouseLocation;
        private MovingObject dvd_obj;
        private ImageAttributes attr;
        private Color[] colorPalette = new Color[7] { Color.FromArgb(1, 255, 131, 0), Color.Cyan, Color.Pink, Color.Blue, Color.Green, Color.Red, Color.Magenta };
        int RGBcounter = 0;

        public screensaver()
        {
            InitializeComponent();
        }

        public screensaver(Rectangle Bounds)
        {
            InitializeComponent();
            this.Bounds = Bounds;
            dvd_obj = new MovingObject(Bounds.Width, Bounds.Height);
            dvd_obj.Bound_X = this.Bounds.Width;
            dvd_obj.Bound_Y = this.Bounds.Height;
            DoubleBuffered = true;
            colorPalette[0] = dvd_obj.GetColorSample();
        }

        private void ScreenSaverForm_Load(object sender, EventArgs e)
        {
            LoadSettings();
            Cursor.Hide();
            TopMost = true;
            moveTimer.Interval = 30;
            moveTimer.Tick += new EventHandler(moveTimer_Tick);
            moveTimer.Start();
        }

        public void SetSingleColor()
        {
            ColorMap[] colorMap = new ColorMap[1];
            colorMap[0] = new ColorMap();
            colorMap[0].OldColor = dvd_obj.GetColorSample();
            colorMap[0].NewColor = colorPalette[RGBcounter];
            attr = new ImageAttributes();
            attr.SetRemapTable(colorMap);
        }

        public void ChangeColor()
        {
            ColorMap[] colorMap = new ColorMap[1];
            colorMap[0] = new ColorMap();
            colorMap[0].OldColor = dvd_obj.GetColorSample();
            RGBcounter++;
            if (RGBcounter >= colorPalette.Length)
                RGBcounter = 0;
            colorMap[0].NewColor = colorPalette[RGBcounter];
            attr = new ImageAttributes();
            attr.SetRemapTable(colorMap);
        }


        private void moveTimer_Tick(object sender, System.EventArgs e)
        {
            dvd_obj.Move();
            this.Invalidate();
        }

        private void LoadSettings()
        {
            RegistryKey key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\DVD Screensaver");
            if (key != null && Convert.ToBoolean(key.GetValue("rgb")))
                dvd_obj.BoundarieHitEvent += ChangeColor;
            else
                if (key != null && (key.GetValue("SingleColor") != null))
            {
                RGBcounter = Convert.ToInt32(key.GetValue("SingleColor"));
                SetSingleColor();
            }
        }

        private void ScreenSaverForm_MouseMove(object sender, MouseEventArgs e)
        {
                if (!mouseLocation.IsEmpty)
                {
                    if (Math.Abs(mouseLocation.X - e.X) > 5 ||
                        Math.Abs(mouseLocation.Y - e.Y) > 5)
                        Application.Exit();
                }
                mouseLocation = e.Location;
        }

        private void ScreenSaverForm_KeyPress(object sender, KeyPressEventArgs e)
        {
                Application.Exit();
        }

        private void ScreenSaverForm_MouseClick(object sender, MouseEventArgs e)
        {
                Application.Exit();
        }

        private void ScreenSaverForm_Paint(object sender, PaintEventArgs e)
        {
            Graphics graphics = e.Graphics;
            graphics.Clear(Color.Black);
            if (dvd_obj != null)
                graphics.DrawImage(dvd_obj.BMP, new Rectangle(dvd_obj.Location.X, dvd_obj.Location.Y, dvd_obj.Width, dvd_obj.Height), 0, 0, dvd_obj.BMP.Width, dvd_obj.BMP.Height, GraphicsUnit.Pixel, attr);
        }
    }
}
