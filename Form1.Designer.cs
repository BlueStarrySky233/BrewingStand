namespace ModVersionConvertor
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            btnStart = new Button();
            rtbLog = new RichTextBox();
            progressBar = new ProgressBar();
            sha1Label = new Label();
            groupBox1 = new GroupBox();
            label4 = new Label();
            btnBrowseOutput = new Button();
            txtOutput = new TextBox();
            label3 = new Label();
            label2 = new Label();
            label1 = new Label();
            checkBox2 = new CheckBox();
            checkBox1 = new CheckBox();
            btnBrowse = new Button();
            txtModsPath = new TextBox();
            versionCombo = new ComboBox();
            loaderCombo = new ComboBox();
            groupBox1.SuspendLayout();
            SuspendLayout();
            // 
            // btnStart
            // 
            btnStart.Location = new Point(1024, 625);
            btnStart.Name = "btnStart";
            btnStart.Size = new Size(116, 47);
            btnStart.TabIndex = 4;
            btnStart.Text = "Migrate";
            btnStart.UseVisualStyleBackColor = true;
            btnStart.Click += btnStart_Click;
            // 
            // rtbLog
            // 
            rtbLog.Location = new Point(12, 218);
            rtbLog.Name = "rtbLog";
            rtbLog.Size = new Size(1128, 391);
            rtbLog.TabIndex = 5;
            rtbLog.Text = "";
            // 
            // progressBar
            // 
            progressBar.Location = new Point(12, 625);
            progressBar.Name = "progressBar";
            progressBar.Size = new Size(997, 47);
            progressBar.TabIndex = 6;
            // 
            // sha1Label
            // 
            sha1Label.AutoSize = true;
            sha1Label.BackColor = Color.Aqua;
            sha1Label.Location = new Point(23, 661);
            sha1Label.Name = "sha1Label";
            sha1Label.Size = new Size(0, 24);
            sha1Label.TabIndex = 7;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(label4);
            groupBox1.Controls.Add(btnBrowseOutput);
            groupBox1.Controls.Add(txtOutput);
            groupBox1.Controls.Add(label3);
            groupBox1.Controls.Add(label2);
            groupBox1.Controls.Add(label1);
            groupBox1.Controls.Add(checkBox2);
            groupBox1.Controls.Add(checkBox1);
            groupBox1.Controls.Add(btnBrowse);
            groupBox1.Controls.Add(txtModsPath);
            groupBox1.Controls.Add(versionCombo);
            groupBox1.Controls.Add(loaderCombo);
            groupBox1.Location = new Point(12, 11);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(1132, 201);
            groupBox1.TabIndex = 10;
            groupBox1.TabStop = false;
            groupBox1.Text = "Migration Settings";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(414, 79);
            label4.Name = "label4";
            label4.Size = new Size(137, 24);
            label4.TabIndex = 21;
            label4.Text = "Output Folder:";
            // 
            // btnBrowseOutput
            // 
            btnBrowseOutput.Location = new Point(1080, 79);
            btnBrowseOutput.Name = "btnBrowseOutput";
            btnBrowseOutput.Size = new Size(39, 30);
            btnBrowseOutput.TabIndex = 20;
            btnBrowseOutput.Text = "...";
            btnBrowseOutput.UseVisualStyleBackColor = true;
            btnBrowseOutput.Click += btnBrowseOutput_Click;
            // 
            // txtOutput
            // 
            txtOutput.Location = new Point(560, 79);
            txtOutput.Name = "txtOutput";
            txtOutput.Size = new Size(514, 30);
            txtOutput.TabIndex = 19;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(436, 40);
            label3.Name = "label3";
            label3.Size = new Size(115, 24);
            label3.TabIndex = 18;
            label3.Text = "Mod Folder:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(65, 79);
            label2.Name = "label2";
            label2.Size = new Size(119, 24);
            label2.TabIndex = 17;
            label2.Text = "Mod Loader:";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(18, 37);
            label1.Name = "label1";
            label1.Size = new Size(166, 24);
            label1.TabIndex = 16;
            label1.Text = "Minecraft Version:";
            // 
            // checkBox2
            // 
            checkBox2.AutoSize = true;
            checkBox2.CheckAlign = ContentAlignment.MiddleRight;
            checkBox2.Location = new Point(18, 155);
            checkBox2.Name = "checkBox2";
            checkBox2.Size = new Size(197, 28);
            checkBox2.TabIndex = 15;
            checkBox2.Text = "Auto Update Mod:";
            checkBox2.UseVisualStyleBackColor = true;
            // 
            // checkBox1
            // 
            checkBox1.AutoSize = true;
            checkBox1.CheckAlign = ContentAlignment.MiddleRight;
            checkBox1.Enabled = false;
            checkBox1.Location = new Point(68, 121);
            checkBox1.Name = "checkBox1";
            checkBox1.Size = new Size(147, 28);
            checkBox1.TabIndex = 14;
            checkBox1.Text = "Auto Sinytra:";
            checkBox1.UseVisualStyleBackColor = true;
            // 
            // btnBrowse
            // 
            btnBrowse.Location = new Point(1080, 37);
            btnBrowse.Name = "btnBrowse";
            btnBrowse.Size = new Size(39, 30);
            btnBrowse.TabIndex = 13;
            btnBrowse.Text = "...";
            btnBrowse.UseVisualStyleBackColor = true;
            btnBrowse.Click += btnBrowse_Click;
            // 
            // txtModsPath
            // 
            txtModsPath.Location = new Point(560, 37);
            txtModsPath.Name = "txtModsPath";
            txtModsPath.Size = new Size(514, 30);
            txtModsPath.TabIndex = 12;
            // 
            // versionCombo
            // 
            versionCombo.FormattingEnabled = true;
            versionCombo.Location = new Point(195, 34);
            versionCombo.Name = "versionCombo";
            versionCombo.Size = new Size(197, 32);
            versionCombo.TabIndex = 11;
            versionCombo.SelectedIndexChanged += versionCombo_SelectedIndexChanged;
            // 
            // loaderCombo
            // 
            loaderCombo.Enabled = false;
            loaderCombo.FormattingEnabled = true;
            loaderCombo.Location = new Point(195, 76);
            loaderCombo.Name = "loaderCombo";
            loaderCombo.Size = new Size(197, 32);
            loaderCombo.TabIndex = 10;
            loaderCombo.SelectedIndexChanged += loaderCombo_SelectedIndexChanged;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(11F, 24F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1154, 692);
            Controls.Add(groupBox1);
            Controls.Add(sha1Label);
            Controls.Add(progressBar);
            Controls.Add(rtbLog);
            Controls.Add(btnStart);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Brewing Stand";
            Load += Form1_Load;
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Button btnStart;
        private RichTextBox rtbLog;
        private ProgressBar progressBar;
        private Label sha1Label;
        private GroupBox groupBox1;
        private Label label2;
        private Label label1;
        private CheckBox checkBox2;
        private CheckBox checkBox1;
        private Button btnBrowse;
        private TextBox txtModsPath;
        private ComboBox versionCombo;
        private ComboBox loaderCombo;
        private Label label4;
        private Button btnBrowseOutput;
        private TextBox txtOutput;
        private Label label3;
    }
}
