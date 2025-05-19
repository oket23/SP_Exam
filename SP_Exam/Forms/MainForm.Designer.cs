namespace SP_Exam.Forms
{
    partial class MainForm
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
            mainRTB = new RichTextBox();
            wordTB = new TextBox();
            searchWordBtn = new Button();
            fCaRWBtn = new Button();
            classFindBtn = new Button();
            CancelBtn = new Button();
            panel1 = new Panel();
            mainPB = new ProgressBar();
            PBLaybel = new RichTextBox();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // mainRTB
            // 
            mainRTB.BackColor = Color.White;
            mainRTB.BorderStyle = BorderStyle.None;
            mainRTB.Font = new Font("Lucida Sans Unicode", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 204);
            mainRTB.Location = new Point(-1, -1);
            mainRTB.Name = "mainRTB";
            mainRTB.ReadOnly = true;
            mainRTB.Size = new Size(279, 300);
            mainRTB.TabIndex = 2;
            mainRTB.Text = "";
            // 
            // wordTB
            // 
            wordTB.Font = new Font("Lucida Sans Unicode", 12F);
            wordTB.Location = new Point(23, 27);
            wordTB.MaxLength = 30;
            wordTB.Name = "wordTB";
            wordTB.PlaceholderText = "Enter word";
            wordTB.Size = new Size(184, 32);
            wordTB.TabIndex = 3;
            // 
            // searchWordBtn
            // 
            searchWordBtn.FlatStyle = FlatStyle.System;
            searchWordBtn.Font = new Font("Lucida Sans Unicode", 12F);
            searchWordBtn.Location = new Point(23, 75);
            searchWordBtn.Name = "searchWordBtn";
            searchWordBtn.Size = new Size(184, 56);
            searchWordBtn.TabIndex = 4;
            searchWordBtn.Text = "Find Word in Folder";
            searchWordBtn.UseVisualStyleBackColor = true;
            searchWordBtn.Click += searchWordBtn_Click;
            // 
            // fCaRWBtn
            // 
            fCaRWBtn.FlatStyle = FlatStyle.System;
            fCaRWBtn.Font = new Font("Lucida Sans Unicode", 12F);
            fCaRWBtn.Location = new Point(23, 146);
            fCaRWBtn.Name = "fCaRWBtn";
            fCaRWBtn.Size = new Size(184, 57);
            fCaRWBtn.TabIndex = 4;
            fCaRWBtn.Text = "Find, Copy, and Replace Word";
            fCaRWBtn.UseVisualStyleBackColor = true;
            fCaRWBtn.Click += fCaRWBtn_Click;
            // 
            // classFindBtn
            // 
            classFindBtn.FlatStyle = FlatStyle.System;
            classFindBtn.Font = new Font("Lucida Sans Unicode", 12F);
            classFindBtn.Location = new Point(23, 219);
            classFindBtn.Name = "classFindBtn";
            classFindBtn.Size = new Size(184, 56);
            classFindBtn.TabIndex = 4;
            classFindBtn.Text = "Find C# Classes and Interfaces";
            classFindBtn.UseVisualStyleBackColor = true;
            classFindBtn.Click += classFindBtn_Click;
            // 
            // CancelBtn
            // 
            CancelBtn.FlatStyle = FlatStyle.System;
            CancelBtn.Font = new Font("Lucida Sans Unicode", 12F);
            CancelBtn.Location = new Point(23, 291);
            CancelBtn.Name = "CancelBtn";
            CancelBtn.Size = new Size(184, 36);
            CancelBtn.TabIndex = 4;
            CancelBtn.Text = "Cancel";
            CancelBtn.UseVisualStyleBackColor = true;
            CancelBtn.Click += CancelBtn_Click;
            // 
            // panel1
            // 
            panel1.BorderStyle = BorderStyle.FixedSingle;
            panel1.Controls.Add(mainRTB);
            panel1.Location = new Point(235, 27);
            panel1.Name = "panel1";
            panel1.Size = new Size(279, 300);
            panel1.TabIndex = 5;
            // 
            // mainPB
            // 
            mainPB.BackColor = Color.White;
            mainPB.Location = new Point(23, 369);
            mainPB.Name = "mainPB";
            mainPB.Size = new Size(491, 38);
            mainPB.Style = ProgressBarStyle.Continuous;
            mainPB.TabIndex = 6;
            // 
            // PBLaybel
            // 
            PBLaybel.BackColor = SystemColors.Control;
            PBLaybel.BorderStyle = BorderStyle.None;
            PBLaybel.Font = new Font("Lucida Sans Unicode", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 204);
            PBLaybel.Location = new Point(23, 345);
            PBLaybel.Multiline = false;
            PBLaybel.Name = "PBLaybel";
            PBLaybel.ReadOnly = true;
            PBLaybel.Size = new Size(491, 18);
            PBLaybel.TabIndex = 8;
            PBLaybel.Text = "";
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.Control;
            ClientSize = new Size(534, 429);
            Controls.Add(PBLaybel);
            Controls.Add(mainPB);
            Controls.Add(panel1);
            Controls.Add(CancelBtn);
            Controls.Add(classFindBtn);
            Controls.Add(fCaRWBtn);
            Controls.Add(searchWordBtn);
            Controls.Add(wordTB);
            FormBorderStyle = FormBorderStyle.Fixed3D;
            MaximizeBox = false;
            Name = "MainForm";
            ShowIcon = false;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "SP_Exam";
            panel1.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private RichTextBox mainRTB;
        private TextBox wordTB;
        private Button searchWordBtn;
        private Button fCaRWBtn;
        private Button classFindBtn;
        private Button CancelBtn;
        private Panel panel1;
        private ProgressBar mainPB;
        private RichTextBox PBLaybel;
    }
}