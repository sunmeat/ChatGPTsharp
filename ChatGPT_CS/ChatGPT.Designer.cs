namespace ChatGPTCS
{
    partial class ChatGPT
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
            this.txtAnswer = new System.Windows.Forms.TextBox();
            this.txtQuestion = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // txtAnswer
            // 
            this.txtAnswer.AcceptsReturn = true;
            this.txtAnswer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtAnswer.Location = new System.Drawing.Point(8, 8);
            this.txtAnswer.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtAnswer.Multiline = true;
            this.txtAnswer.Name = "txtAnswer";
            this.txtAnswer.ReadOnly = true;
            this.txtAnswer.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtAnswer.Size = new System.Drawing.Size(784, 223);
            this.txtAnswer.TabIndex = 11;
            // 
            // txtQuestion
            // 
            this.txtQuestion.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtQuestion.Location = new System.Drawing.Point(8, 235);
            this.txtQuestion.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtQuestion.Multiline = true;
            this.txtQuestion.Name = "txtQuestion";
            this.txtQuestion.Size = new System.Drawing.Size(784, 155);
            this.txtQuestion.TabIndex = 10;
            this.txtQuestion.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtQuestion_KeyDown);
            // 
            // ChatGPT
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(799, 398);
            this.Controls.Add(this.txtAnswer);
            this.Controls.Add(this.txtQuestion);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "ChatGPT";
            this.Text = "тест апи чата гпт";
            this.Load += new System.EventHandler(this.ChatGPT_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        internal System.Windows.Forms.TextBox txtAnswer;
        internal System.Windows.Forms.TextBox txtQuestion;
    }
}

