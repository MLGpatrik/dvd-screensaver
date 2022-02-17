using System;
using System.Windows.Forms;
using Microsoft.Win32;

namespace dvd_screensaver
{
    public partial class settings_form : Form
    {
        public settings_form()
        {
            InitializeComponent();
            LoadSettings();
        }


        private void ChangeSingleSelect(bool rgb)
        {
            ColorComboBox.Enabled = !rgb;
            label2.Enabled = !rgb;
        }

        private void LoadSettings()
        {
            RegistryKey key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\DVD Screensaver");
            if (key == null)
            {
                Brgb.Checked = false;
            }
            else
            {
                Brgb.Checked = Convert.ToBoolean(key.GetValue("rgb"));
                ChangeSingleSelect(Brgb.Checked);
            }
        }

        private void SaveSettings()
        {
            RegistryKey key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\DVD Screensaver");
            key.SetValue("SingleColor", ColorComboBox.SelectedIndex);
            key.SetValue("rgb", Brgb.Checked);
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            SaveSettings();
            Close();
        }

        private void ColorComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void Brgb_CheckedChanged(object sender, EventArgs e)
        {
            ChangeSingleSelect(Brgb.Checked);
        }
    }
}
