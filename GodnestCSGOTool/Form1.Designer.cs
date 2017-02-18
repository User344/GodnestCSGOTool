namespace GodnestCSGOTool
{
    partial class Form1
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
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.materialRaisedButton20 = new MaterialSkin.Controls.MaterialRaisedButton();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(12, 74);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(610, 237);
            this.textBox1.TabIndex = 0;
            // 
            // materialRaisedButton20
            // 
            this.materialRaisedButton20.Depth = 0;
            this.materialRaisedButton20.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.materialRaisedButton20.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.materialRaisedButton20.Location = new System.Drawing.Point(12, 317);
            this.materialRaisedButton20.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialRaisedButton20.Name = "materialRaisedButton20";
            this.materialRaisedButton20.Primary = true;
            this.materialRaisedButton20.Size = new System.Drawing.Size(610, 39);
            this.materialRaisedButton20.TabIndex = 37;
            this.materialRaisedButton20.Text = "Заспамить";
            this.materialRaisedButton20.UseVisualStyleBackColor = true;
            this.materialRaisedButton20.Click += new System.EventHandler(this.materialRaisedButton20_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(634, 364);
            this.Controls.Add(this.materialRaisedButton20);
            this.Controls.Add(this.textBox1);
            this.Name = "Form1";
            this.Text = "Invite Spam";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox1;
        private MaterialSkin.Controls.MaterialRaisedButton materialRaisedButton20;
    }
}