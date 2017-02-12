namespace EndRelease
{
    partial class FormUserSetting
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.tbCantilever = new System.Windows.Forms.TextBox();
            this.tbMoment = new System.Windows.Forms.TextBox();
            this.SelectCan = new System.Windows.Forms.Button();
            this.butnSelectMom = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(85, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Moment Symbol:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 72);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(94, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Cantilever Symbol:";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(200, 116);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "Ok";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.butnOk_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(281, 116);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 2;
            this.button2.Text = "Cancel";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.butnCancle_Click);
            // 
            // tbCantilever
            // 
            this.tbCantilever.Location = new System.Drawing.Point(103, 69);
            this.tbCantilever.Name = "tbCantilever";
            this.tbCantilever.Size = new System.Drawing.Size(172, 20);
            this.tbCantilever.TabIndex = 12;
            // 
            // tbMoment
            // 
            this.tbMoment.Location = new System.Drawing.Point(103, 18);
            this.tbMoment.Name = "tbMoment";
            this.tbMoment.Size = new System.Drawing.Size(172, 20);
            this.tbMoment.TabIndex = 13;
            // 
            // SelectCan
            // 
            this.SelectCan.Location = new System.Drawing.Point(281, 67);
            this.SelectCan.Name = "SelectCan";
            this.SelectCan.Size = new System.Drawing.Size(75, 23);
            this.SelectCan.TabIndex = 10;
            this.SelectCan.Text = "Select";
            this.SelectCan.UseVisualStyleBackColor = true;
            this.SelectCan.Click += new System.EventHandler(this.butnSelCan_Click);
            // 
            // butnSelectMom
            // 
            this.butnSelectMom.Location = new System.Drawing.Point(281, 16);
            this.butnSelectMom.Name = "butnSelectMom";
            this.butnSelectMom.Size = new System.Drawing.Size(75, 23);
            this.butnSelectMom.TabIndex = 11;
            this.butnSelectMom.Text = "Select";
            this.butnSelectMom.UseVisualStyleBackColor = true;
            this.butnSelectMom.Click += new System.EventHandler(this.butnSelMom_Click);
            // 
            // FormUserSetting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(364, 161);
            this.Controls.Add(this.tbCantilever);
            this.Controls.Add(this.tbMoment);
            this.Controls.Add(this.SelectCan);
            this.Controls.Add(this.butnSelectMom);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "FormUserSetting";
            this.Text = "Connection Symbol Setting";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TextBox tbCantilever;
        private System.Windows.Forms.TextBox tbMoment;
        private System.Windows.Forms.Button SelectCan;
        private System.Windows.Forms.Button butnSelectMom;
    }
}