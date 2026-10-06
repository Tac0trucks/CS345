namespace FuzzyLogicAct
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
            this.lblFuzzyDebug = new System.Windows.Forms.Label();
            this.pnlArena = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.lblPlayerStats = new System.Windows.Forms.Label();
            this.lblBossStats = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.pnl3DSurface = new FuzzyLogicAct.DoubleBufferedPanel();
            this.pnlCurves = new FuzzyLogicAct.DoubleBufferedPanel();
            this.SuspendLayout();
            // 
            // lblFuzzyDebug
            // 
            this.lblFuzzyDebug.Font = new System.Drawing.Font("Segoe Fluent Icons", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFuzzyDebug.Location = new System.Drawing.Point(78, 589);
            this.lblFuzzyDebug.Name = "lblFuzzyDebug";
            this.lblFuzzyDebug.Size = new System.Drawing.Size(666, 97);
            this.lblFuzzyDebug.TabIndex = 1;
            this.lblFuzzyDebug.Text = "Debug Here";
            this.lblFuzzyDebug.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pnlArena
            // 
            this.pnlArena.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlArena.Location = new System.Drawing.Point(202, 153);
            this.pnlArena.Name = "pnlArena";
            this.pnlArena.Size = new System.Drawing.Size(400, 400);
            this.pnlArena.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(202, 66);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(400, 40);
            this.label1.TabIndex = 3;
            this.label1.Text = "PLAYER VS. THE EMPRESS\r\nA Mamdani fuzzy logic bossfight";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblPlayerStats
            // 
            this.lblPlayerStats.Font = new System.Drawing.Font("Segoe UI", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPlayerStats.Location = new System.Drawing.Point(28, 234);
            this.lblPlayerStats.Name = "lblPlayerStats";
            this.lblPlayerStats.Size = new System.Drawing.Size(144, 190);
            this.lblPlayerStats.TabIndex = 4;
            this.lblPlayerStats.Text = "Player Stats";
            this.lblPlayerStats.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // lblBossStats
            // 
            this.lblBossStats.Font = new System.Drawing.Font("Segoe UI", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBossStats.Location = new System.Drawing.Point(627, 234);
            this.lblBossStats.Name = "lblBossStats";
            this.lblBossStats.Size = new System.Drawing.Size(146, 190);
            this.lblBossStats.TabIndex = 5;
            this.lblBossStats.Text = "Boss Stats";
            this.lblBossStats.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Crimson;
            this.panel1.Location = new System.Drawing.Point(679, 166);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(50, 50);
            this.panel1.TabIndex = 6;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.DodgerBlue;
            this.panel2.Location = new System.Drawing.Point(78, 166);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(50, 50);
            this.panel2.TabIndex = 7;
            // 
            // pnl3DSurface
            // 
            this.pnl3DSurface.Location = new System.Drawing.Point(776, 384);
            this.pnl3DSurface.Name = "pnl3DSurface";
            this.pnl3DSurface.Size = new System.Drawing.Size(1074, 622);
            this.pnl3DSurface.TabIndex = 8;
            this.pnl3DSurface.Paint += new System.Windows.Forms.PaintEventHandler(this.pnl3DSurface_Paint);
            // 
            // pnlCurves
            // 
            this.pnlCurves.Location = new System.Drawing.Point(776, 66);
            this.pnlCurves.Name = "pnlCurves";
            this.pnlCurves.Size = new System.Drawing.Size(1074, 278);
            this.pnlCurves.TabIndex = 9;
            this.pnlCurves.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlCurves_Paint);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1902, 1033);
            this.Controls.Add(this.pnlCurves);
            this.Controls.Add(this.pnl3DSurface);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.lblBossStats);
            this.Controls.Add(this.lblPlayerStats);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pnlArena);
            this.Controls.Add(this.lblFuzzyDebug);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label lblFuzzyDebug;
        private System.Windows.Forms.Panel pnlArena;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblPlayerStats;
        private System.Windows.Forms.Label lblBossStats;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private DoubleBufferedPanel pnl3DSurface;
        private DoubleBufferedPanel pnlCurves;
    }
}