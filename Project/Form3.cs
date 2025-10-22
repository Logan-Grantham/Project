namespace Project
{
    partial class Form3
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private readonly System.ComponentModel.IContainer components = null;

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
            textBox = new TextBox();
            leaderBoard = new ListBox();
            button1 = new Button();
            SuspendLayout();
            // 
            // textBox
            // 
            textBox.Location = new Point(318, 12);
            textBox.MaxLength = 20;
            textBox.Name = "textBox";
            textBox.PlaceholderText = "Enter Your Name";
            textBox.Size = new Size(100, 23);
            textBox.TabIndex = 0;
            textBox.KeyPress += TextBox_KeyPress;
            // 
            // leaderBoard
            // 
            leaderBoard.Font = new Font("SimSun-ExtB", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            leaderBoard.FormattingEnabled = true;
            leaderBoard.ItemHeight = 15;
            leaderBoard.Location = new Point(12, 12);
            leaderBoard.Name = "leaderBoard";
            leaderBoard.Size = new Size(300, 379);
            leaderBoard.TabIndex = 1;
            // 
            // button1
            // 
            button1.Location = new Point(318, 291);
            button1.Name = "button1";
            button1.Size = new Size(100, 100);
            button1.TabIndex = 2;
            button1.Text = "update the board";
            button1.UseVisualStyleBackColor = true;
            button1.Click += Update;
            // 
            // Form3
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(430, 403);
            Controls.Add(button1);
            Controls.Add(leaderBoard);
            Controls.Add(textBox);
            FormBorderStyle = FormBorderStyle.None;
            MaximumSize = new Size(430, 403);
            MinimumSize = new Size(430, 403);
            Name = "Form3";
            Text = "Form3 Winner";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox textBox;
        private ListBox leaderBoard;
        private Button button1;
    }
}