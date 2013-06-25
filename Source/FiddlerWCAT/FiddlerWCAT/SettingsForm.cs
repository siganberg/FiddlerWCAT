// <copyright file=" " company="Digitrish">
// Copyright (c) 2013 All Right Reserved, http://www.digitrish.com
//
// THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY 
// KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
// IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
// </copyright>
//
// <author>Francis Marasigan</author>
// <email>francis.marasigan@live.com</email>
// <date>2013-06-23</date>
      
      
      
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
            virtualClientText.Text = Settings.Instance.VirtualClient.ToString(CultureInfo.InvariantCulture);
            rpsText.Text = Settings.Instance.ThrottleRps.ToString(CultureInfo.InvariantCulture);
            clientTextBox.Text = Settings.Instance.Clients;
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            Settings.Instance.WcatHomeDirectory = wcatDirectoryText.Text.Trim();

            int warmup, duration, cooldown, virtualClient, throttlerps;


            int.TryParse(warmupText.Text.Trim(), out warmup);
            int.TryParse(durationText.Text.Trim(), out duration);
            int.TryParse(cooldownText.Text.Trim(), out cooldown);
            int.TryParse(virtualClientText.Text.Trim(), out virtualClient);
            int.TryParse(rpsText.Text.Trim(), out throttlerps);


            Settings.Instance.Warmup = warmup;
            Settings.Instance.Duration = duration;
            Settings.Instance.Cooldown = cooldown;
            Settings.Instance.VirtualClient = virtualClient;
            Settings.Instance.ThrottleRps = throttlerps;
            Settings.Instance.Clients = clientTextBox.Text.Trim();
            Settings.Instance.Save();

            Close();
        }
    }
}
