using System;
using System.IO;
using System.Windows.Forms;

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
            //-- try auto searching the wcat based on the default installation 
            var wcatDefault = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86), "wcat");
            wcatDefault = Directory.Exists(wcatDefault) ? wcatDefault : Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "wcat");
            wcatDefault = Directory.Exists(wcatDefault) ? wcatDefault : "";
            wcatDirectoryText.Text = wcatDefault; 
        }
    }
}
