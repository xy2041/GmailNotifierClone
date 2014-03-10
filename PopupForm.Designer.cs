namespace GmailNotifierClone
{
    partial class PopupForm
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
            this.lblCountAdndDate = new System.Windows.Forms.Label();
            this.lblFrom = new System.Windows.Forms.Label();
            this.lblHeader = new System.Windows.Forms.Label();
            this.imgIco = new System.Windows.Forms.PictureBox();
            this.lblText = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.imgIco)).BeginInit();
            this.SuspendLayout();
            // 
            // lblCountAdndDate
            // 
            this.lblCountAdndDate.AutoSize = true;
            this.lblCountAdndDate.Location = new System.Drawing.Point(50, 3);
            this.lblCountAdndDate.Name = "lblCountAdndDate";
            this.lblCountAdndDate.Size = new System.Drawing.Size(87, 13);
            this.lblCountAdndDate.TabIndex = 0;
            this.lblCountAdndDate.Text = "6 of 103 - sep 13";
            // 
            // lblFrom
            // 
            this.lblFrom.AutoSize = true;
            this.lblFrom.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblFrom.Location = new System.Drawing.Point(143, 3);
            this.lblFrom.Name = "lblFrom";
            this.lblFrom.Size = new System.Drawing.Size(93, 13);
            this.lblFrom.TabIndex = 1;
            this.lblFrom.Text = "Adobe Systems";
            // 
            // lblHeader
            // 
            this.lblHeader.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHeader.Location = new System.Drawing.Point(50, 19);
            this.lblHeader.Name = "lblHeader";
            this.lblHeader.Size = new System.Drawing.Size(299, 29);
            this.lblHeader.TabIndex = 2;
            this.lblHeader.Text = "Creative Freedom! Tre forskeilige sadk fj dfaskf jasd ;fkja sd;lfkj saddafsdfasdf" +
    " ";
            // 
            // imgIco
            // 
            this.imgIco.Image = global::GmailNotifierClone.Properties.Resources.PopupIcon;
            this.imgIco.Location = new System.Drawing.Point(6, 8);
            this.imgIco.Name = "imgIco";
            this.imgIco.Size = new System.Drawing.Size(35, 29);
            this.imgIco.TabIndex = 4;
            this.imgIco.TabStop = false;
            // 
            // lblText
            // 
            this.lblText.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblText.Location = new System.Drawing.Point(49, 48);
            this.lblText.Name = "lblText";
            this.lblText.Size = new System.Drawing.Size(299, 29);
            this.lblText.TabIndex = 5;
            this.lblText.Text = "Some unreal cool text fgdsj gsdkfg jsdfgj sdfg sdfg sdfg sdfg sd asdf asdf dddddd" +
    "dddddd";
            // 
            // PopupForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(360, 76);
            this.ControlBox = false;
            this.Controls.Add(this.lblText);
            this.Controls.Add(this.imgIco);
            this.Controls.Add(this.lblHeader);
            this.Controls.Add(this.lblFrom);
            this.Controls.Add(this.lblCountAdndDate);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "PopupForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "PopupForm";
            this.TopMost = true;
            ((System.ComponentModel.ISupportInitialize)(this.imgIco)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblCountAdndDate;
        private System.Windows.Forms.Label lblFrom;
        private System.Windows.Forms.Label lblHeader;
        private System.Windows.Forms.PictureBox imgIco;
        private System.Windows.Forms.Label lblText;
    }
}