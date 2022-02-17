using System;
using System.Windows.Forms;

namespace dvd_screensaver
{
    static class Program
    {

        static void SetupScreens()
        {
            for(int i=0; i< Screen.AllScreens.Length; i++)
            {
                screensaver screensaver = new screensaver( Screen.AllScreens[i].Bounds);
                screensaver.Show();
                
            }            
        }

        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            if(args.Length != 0)
            {
                if(args.Length == 1)
                {
                    switch (args[0])
                    {
                        case "/s":
                            SetupScreens();
                            Application.Run();
                        break;
                        case "/c":
                            Application.Run(new settings_form());
                        break;
                        default:
                            SetupScreens();
                            Application.Run();
                        break;
                    }

                }         
            }
            else
            {
                Application.Run(new settings_form());
            }            
        }
        
    }
}
