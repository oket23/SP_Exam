namespace SP_Exam.Forms
{
    partial class EnterWordForm
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
            wordTB = new TextBox();
            oKBtn = new Button();
            PBLaybel = new RichTextBox();
            SuspendLayout();
            // 
            // wordTB
            // 
            wordTB.Font = new Font("Lucida Sans Unicode", 12F);
            wordTB.Location = new Point(69, 48);
            wordTB.MaxLength = 30;
            wordTB.Name = "wordTB";
            wordTB.PlaceholderText = "Enter word";
            wordTB.Size = new Size(184, 32);
            wordTB.TabIndex = 4;
            // 
            // oKBtn
            // 
            oKBtn.FlatStyle = FlatStyle.System;
            oKBtn.Font = new Font("Lucida Sans Unicode", 12F);
            oKBtn.Location = new Point(69, 98);
            oKBtn.Name = "oKBtn";
            oKBtn.Size = new Size(184, 36);
            oKBtn.TabIndex = 5;
            oKBtn.Text = "Accept";
            oKBtn.UseVisualStyleBackColor = true;
            oKBtn.Click += oKBtn_Click;
            // 
            // PBLaybel
            // 
            PBLaybel.BackColor = SystemColors.Control;
            PBLaybel.BorderStyle = BorderStyle.None;
            PBLaybel.Font = new Font("Lucida Sans Unicode", 18F, FontStyle.Regular, GraphicsUnit.Point, 204);
            PBLaybel.Location = new Point(69, 12);
            PBLaybel.Multiline = false;
            PBLaybel.Name = "PBLaybel";
            PBLaybel.ReadOnly = true;
            PBLaybel.Size = new Size(184, 30);
            PBLaybel.TabIndex = 9;
            PBLaybel.Text = "Enter new word";
            // 
            // EnterWordForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(308, 156);
            Controls.Add(PBLaybel);
            Controls.Add(oKBtn);
            Controls.Add(wordTB);
            FormBorderStyle = FormBorderStyle.Fixed3D;
            MaximizeBox = false;
            Name = "EnterWordForm";
            ShowIcon = false;
            StartPosition = FormStartPosition.CenterScreen;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox wordTB;
        private Button oKBtn;
        private RichTextBox PBLaybel;
    }
}