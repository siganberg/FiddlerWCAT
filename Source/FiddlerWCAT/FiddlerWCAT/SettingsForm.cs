using System;
using System.Globalization;
using System.IO;
using System.Windows.Forms;
using FiddlerWCAT.Entities;

namespace FiddlerWCAT
{
    public partial class SettingsForm : Form
    {
        public SettingsForm()
        {
            InitializeComponent();
        }

        private void searchButton_Click(object sender, EventArgs e)
        {
            if (Directory.Exists(wcatDirectoryText.Text.Trim()))
            {
                folderBrowseDialog.SelectedPath = wcatDirectoryText.Text;
            }

            if (folderBrowseDialog.ShowDialog() == DialogResult.OK)
            {
                if (Directory.Exists(folderBrowseDialog.SelectedPath))
                {
                    wcatDirectoryText.Text = folderBrowseDialog.SelectedPath;
                }
            }
        }

        private void SettingsForm_Load(object sender, EventArgs e)
        {
            wcatDirectoryText.Text = Settings.Instance.WcatHomeDirectory;
            warmupText.Text = Settings.Instance.Warmup.ToString(CultureInfo.InvariantCulture);
            durationText.Text = Settings.Instance.Duration.ToString(CultureInfo.InvariantCulture);
            cooldownText.Text = Settings.Instance.Cooldown.ToString(CultureInfo.InvariantCulture); 
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            Settings.Instance.WcatHomeDirectory = wcatDirectoryText.Text.Trim();

            int warmup; 
            int duration;
            int cooldown;

            int.TryParse(warmupText.Text.Trim(), out warmup);
            int.TryParse(durationText.Text.Trim(), out duration);
            int.TryParse(cooldownText.Text.Trim(), out cooldown);


            Settings.Instance.Warmup = warmup;
            Settings.Instance.Duration = duration;
            Settings.Instance.Cooldown = cooldown;
            Settings.Instance.Save();

            Close();
        }
    }
}
