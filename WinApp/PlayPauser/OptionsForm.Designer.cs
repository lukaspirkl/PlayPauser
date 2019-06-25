namespace PlayPauser
{
    partial class OptionsForm
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
            this.button1 = new System.Windows.Forms.Button();
            this.webSocketTextBox = new System.Windows.Forms.TextBox();
            this.webSocketCheckBox = new System.Windows.Forms.CheckBox();
            this.httpServerCheckBox = new System.Windows.Forms.CheckBox();
            this.httpServerTextBox = new System.Windows.Forms.TextBox();
            this.httpClientCheckBox = new System.Windows.Forms.CheckBox();
            this.httpClintTextBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(373, 118);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 41);
            this.button1.TabIndex = 0;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // webSocketTextBox
            // 
            this.webSocketTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.webSocketTextBox.Location = new System.Drawing.Point(168, 16);
            this.webSocketTextBox.Name = "webSocketTextBox";
            this.webSocketTextBox.Size = new System.Drawing.Size(280, 26);
            this.webSocketTextBox.TabIndex = 1;
            // 
            // webSocketCheckBox
            // 
            this.webSocketCheckBox.AutoSize = true;
            this.webSocketCheckBox.Location = new System.Drawing.Point(12, 18);
            this.webSocketCheckBox.Name = "webSocketCheckBox";
            this.webSocketCheckBox.Size = new System.Drawing.Size(118, 24);
            this.webSocketCheckBox.TabIndex = 3;
            this.webSocketCheckBox.Text = "WebSocket";
            this.webSocketCheckBox.UseVisualStyleBackColor = true;
            // 
            // httpServerCheckBox
            // 
            this.httpServerCheckBox.AutoSize = true;
            this.httpServerCheckBox.Location = new System.Drawing.Point(12, 50);
            this.httpServerCheckBox.Name = "httpServerCheckBox";
            this.httpServerCheckBox.Size = new System.Drawing.Size(125, 24);
            this.httpServerCheckBox.TabIndex = 5;
            this.httpServerCheckBox.Text = "HTTP Server";
            this.httpServerCheckBox.UseVisualStyleBackColor = true;
            // 
            // httpServerTextBox
            // 
            this.httpServerTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.httpServerTextBox.Location = new System.Drawing.Point(168, 48);
            this.httpServerTextBox.Name = "httpServerTextBox";
            this.httpServerTextBox.Size = new System.Drawing.Size(280, 26);
            this.httpServerTextBox.TabIndex = 4;
            // 
            // httpClientCheckBox
            // 
            this.httpClientCheckBox.AutoSize = true;
            this.httpClientCheckBox.Location = new System.Drawing.Point(12, 82);
            this.httpClientCheckBox.Name = "httpClientCheckBox";
            this.httpClientCheckBox.Size = new System.Drawing.Size(119, 24);
            this.httpClientCheckBox.TabIndex = 7;
            this.httpClientCheckBox.Text = "HTTP Client";
            this.httpClientCheckBox.UseVisualStyleBackColor = true;
            // 
            // httpClintTextBox
            // 
            this.httpClintTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.httpClintTextBox.Location = new System.Drawing.Point(168, 80);
            this.httpClintTextBox.Name = "httpClintTextBox";
            this.httpClintTextBox.Size = new System.Drawing.Size(280, 26);
            this.httpClintTextBox.TabIndex = 6;
            // 
            // Options
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(144F, 144F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(460, 171);
            this.Controls.Add(this.httpClientCheckBox);
            this.Controls.Add(this.httpClintTextBox);
            this.Controls.Add(this.httpServerCheckBox);
            this.Controls.Add(this.httpServerTextBox);
            this.Controls.Add(this.webSocketCheckBox);
            this.Controls.Add(this.webSocketTextBox);
            this.Controls.Add(this.button1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "Options";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Options";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox webSocketTextBox;
        private System.Windows.Forms.CheckBox webSocketCheckBox;
        private System.Windows.Forms.CheckBox httpServerCheckBox;
        private System.Windows.Forms.TextBox httpServerTextBox;
        private System.Windows.Forms.CheckBox httpClientCheckBox;
        private System.Windows.Forms.TextBox httpClintTextBox;
    }
}