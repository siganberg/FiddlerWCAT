namespace FiddlerWCAT
{
    partial class SettingsForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.wcatDirectoryText = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.searchButton = new System.Windows.Forms.Button();
            this.folderBrowseDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.label2 = new System.Windows.Forms.Label();
            this.warmupText = new System.Windows.Forms.TextBox();
            this.durationText = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cooldownText = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.saveButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // wcatDirectoryText
            // 
            this.wcatDirectoryText.Location = new System.Drawing.Point(15, 28);
            this.wcatDirectoryText.Name = "wcatDirectoryText";
            this.wcatDirectoryText.Size = new System.Drawing.Size(278, 20);
            this.wcatDirectoryText.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(165, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Specify the location of the WCAT";
            // 
            // searchButton
            // 
            this.searchButton.Location = new System.Drawing.Point(299, 26);
            this.searchButton.Name = "searchButton";
            this.searchButton.Size = new System.Drawing.Size(41, 23);
            this.searchButton.TabIndex = 2;
            this.searchButton.Text = "...";
            this.searchButton.UseVisualStyleBackColor = true;
            this.searchButton.Click += new System.EventHandler(this.searchButton_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(22, 58);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(47, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Duration";
            // 
            // warmupText
            // 
            this.warmupText.Location = new System.Drawing.Point(82, 29);
            this.warmupText.Name = "warmupText";
            this.warmupText.Size = new System.Drawing.Size(48, 20);
            this.warmupText.TabIndex = 0;
            // 
            // durationText
            // 
            this.durationText.Location = new System.Drawing.Point(82, 55);
            this.durationText.Name = "durationText";
            this.durationText.Size = new System.Drawing.Size(48, 20);
            this.durationText.TabIndex = 0;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(22, 32);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(47, 13);
            this.label3.TabIndex = 1;
            this.label3.Text = "Warmup";
            // 
            // cooldownText
            // 
            this.cooldownText.Location = new System.Drawing.Point(82, 81);
            this.cooldownText.Name = "cooldownText";
            this.cooldownText.Size = new System.Drawing.Size(48, 20);
            this.cooldownText.TabIndex = 0;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(22, 84);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(54, 13);
            this.label4.TabIndex = 1;
            this.label4.Text = "Cooldown";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.warmupText);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.durationText);
            this.groupBox1.Controls.Add(this.cooldownText);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Location = new System.Drawing.Point(12, 54);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(328, 125);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Default Value";
            // 
            // saveButton
            // 
            this.saveButton.Location = new System.Drawing.Point(99, 192);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(75, 23);
            this.saveButton.TabIndex = 4;
            this.saveButton.Text = "&Save";
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Location = new System.Drawing.Point(180, 192);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 4;
            this.cancelButton.Text = "&Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // SettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(355, 227);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.saveButton);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.searchButton);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.wcatDirectoryText);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SettingsForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "WCAT Configuration";
            this.Load += new System.EventHandler(this.SettingsForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox wcatDirectoryText;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button searchButton;
        private System.Windows.Forms.FolderBrowserDialog folderBrowseDialog;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox warmupText;
        private System.Windows.Forms.TextBox durationText;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox cooldownText;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.Button cancelButton;
    }
}