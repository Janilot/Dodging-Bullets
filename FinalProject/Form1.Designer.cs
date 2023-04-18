
namespace gameDemo
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
            this.components = new System.ComponentModel.Container();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.toolStripMenu1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1_1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuI2 = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // timer1
            // 
            this.timer1.Interval = 10;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenu1,
            this.toolStripMenuI2});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(800, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // toolStripMenu1
            // 
            this.toolStripMenu1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1_1});
            this.toolStripMenu1.Name = "toolStripMenu1";
            this.toolStripMenu1.Size = new System.Drawing.Size(65, 20);
            this.toolStripMenu1.Text = "Program";
            // 
            // toolStripMenuItem1_1
            // 
            this.toolStripMenuItem1_1.Name = "toolStripMenuItem1_1";
            this.toolStripMenuItem1_1.Size = new System.Drawing.Size(93, 22);
            this.toolStripMenuItem1_1.Text = "Exit";
            this.toolStripMenuItem1_1.Click += new System.EventHandler(this.toolStripMenuItem1_1_Click);
            // 
            // toolStripMenuI2
            // 
            this.toolStripMenuI2.Name = "toolStripMenuI2";
            this.toolStripMenuI2.Size = new System.Drawing.Size(79, 20);
            this.toolStripMenuI2.Text = "HowToPlay";
            this.toolStripMenuI2.Click += new System.EventHandler(this.toolStripMenuI2_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.menuStrip1);
            this.DoubleBuffered = true;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.Form1_Paint);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenu1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuI2;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1_1;
    }
}

