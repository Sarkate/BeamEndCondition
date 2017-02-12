namespace EndRelease
{
    partial class FormSymbolSelection
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
            this.lbExConnType = new System.Windows.Forms.ListBox();
            this.butnCancel = new System.Windows.Forms.Button();
            this.butnOk = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lbExConnType
            // 
            this.lbExConnType.FormattingEnabled = true;
            this.lbExConnType.Location = new System.Drawing.Point(16, 38);
            this.lbExConnType.Name = "lbExConnType";
            this.lbExConnType.Size = new System.Drawing.Size(260, 108);
            this.lbExConnType.TabIndex = 0;
            // 
            // butnCancel
            // 
            this.butnCancel.Location = new System.Drawing.Point(201, 161);
            this.butnCancel.Name = "butnCancel";
            this.butnCancel.Size = new System.Drawing.Size(75, 23);
            this.butnCancel.TabIndex = 1;
            this.butnCancel.Text = "Cancel";
            this.butnCancel.UseVisualStyleBackColor = true;
            this.butnCancel.Click += new System.EventHandler(this.butnCancel_Click);
            // 
            // butnOk
            // 
            this.butnOk.Location = new System.Drawing.Point(120, 161);
            this.butnOk.Name = "butnOk";
            this.butnOk.Size = new System.Drawing.Size(75, 23);
            this.butnOk.TabIndex = 1;
            this.butnOk.Text = "Ok";
            this.butnOk.UseVisualStyleBackColor = true;
            this.butnOk.Click += new System.EventHandler(this.butnOk_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(96, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Connection Types:";
            // 
            // FormSymbolSelection
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 191);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.butnOk);
            this.Controls.Add(this.butnCancel);
            this.Controls.Add(this.lbExConnType);
            this.Name = "FormSymbolSelection";
            this.Text = "Conntion Type Setting";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox lbExConnType;
        private System.Windows.Forms.Button butnCancel;
        private System.Windows.Forms.Button butnOk;
        private System.Windows.Forms.Label label1;
    }
}