namespace ProCPTestAppTiles
{
    partial class Graphs
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
            this.components = new System.ComponentModel.Container();
            this.angularGauge1 = new LiveCharts.WinForms.AngularGauge();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.solidGauge1 = new LiveCharts.WinForms.SolidGauge();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.lblTotalTime = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.lblCarCrash = new System.Windows.Forms.Label();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.lblTileCrash = new System.Windows.Forms.Label();
            this.groupBox8 = new System.Windows.Forms.GroupBox();
            this.lblTileStopped = new System.Windows.Forms.Label();
            this.groupBox9 = new System.Windows.Forms.GroupBox();
            this.lblHighAvgSpeedTile = new System.Windows.Forms.Label();
            this.tltldistance = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox7.SuspendLayout();
            this.groupBox8.SuspendLayout();
            this.groupBox9.SuspendLayout();
            this.SuspendLayout();
            this.angularGauge1.Location = new System.Drawing.Point(35, 41);
            this.angularGauge1.Name = "angularGauge1";
            this.angularGauge1.Size = new System.Drawing.Size(170, 170);
            this.angularGauge1.TabIndex = 0;
            this.angularGauge1.Text = "angularGauge1";
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            this.solidGauge1.Location = new System.Drawing.Point(35, 37);
            this.solidGauge1.Name = "solidGauge1";
            this.solidGauge1.Size = new System.Drawing.Size(170, 159);
            this.solidGauge1.TabIndex = 2;
            this.solidGauge1.Text = "solidGauge1";
            this.groupBox1.Controls.Add(this.groupBox6);
            this.groupBox1.Controls.Add(this.groupBox5);
            this.groupBox1.Controls.Add(this.angularGauge1);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F);
            this.groupBox1.Location = new System.Drawing.Point(6, 257);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(241, 234);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Cars average speed";
            this.groupBox6.Controls.Add(this.label2);
            this.groupBox6.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F);
            this.groupBox6.Location = new System.Drawing.Point(247, 127);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(237, 107);
            this.groupBox6.TabIndex = 9;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Amount of cars crashing";
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Stencil", 18F, System.Drawing.FontStyle.Bold,
                System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.label2.ForeColor = System.Drawing.SystemColors.WindowText;
            this.label2.Location = new System.Drawing.Point(65, 59);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(28, 29);
            this.label2.TabIndex = 6;
            this.label2.Text = "0";
            this.groupBox5.Controls.Add(this.label1);
            this.groupBox5.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F);
            this.groupBox5.Location = new System.Drawing.Point(247, 0);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(237, 107);
            this.groupBox5.TabIndex = 9;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Amount of cars crashing";
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Stencil", 18F, System.Drawing.FontStyle.Bold,
                System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.label1.ForeColor = System.Drawing.SystemColors.WindowText;
            this.label1.Location = new System.Drawing.Point(65, 59);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(28, 29);
            this.label1.TabIndex = 6;
            this.label1.Text = "0";
            this.groupBox2.Controls.Add(this.solidGauge1);
            this.groupBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F,
                System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.groupBox2.Location = new System.Drawing.Point(6, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(241, 211);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Cars in simulation";
            this.lblTotalTime.AutoSize = true;
            this.lblTotalTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold,
                System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.lblTotalTime.ForeColor = System.Drawing.SystemColors.WindowText;
            this.lblTotalTime.Location = new System.Drawing.Point(65, 63);
            this.lblTotalTime.Name = "lblTotalTime";
            this.lblTotalTime.Size = new System.Drawing.Size(27, 29);
            this.lblTotalTime.TabIndex = 6;
            this.lblTotalTime.Text = "0";
            this.groupBox3.Controls.Add(this.lblTotalTime);
            this.groupBox3.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F);
            this.groupBox3.Location = new System.Drawing.Point(253, 12);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(235, 107);
            this.groupBox3.TabIndex = 7;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Total Simulation Time";
            this.groupBox4.Controls.Add(this.lblCarCrash);
            this.groupBox4.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F);
            this.groupBox4.Location = new System.Drawing.Point(253, 134);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(237, 107);
            this.groupBox4.TabIndex = 8;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Amount of cars crashing";
            this.groupBox4.Enter += new System.EventHandler(this.groupBox4_Enter);
            this.lblCarCrash.AutoSize = true;
            this.lblCarCrash.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold,
                System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.lblCarCrash.ForeColor = System.Drawing.SystemColors.WindowText;
            this.lblCarCrash.Location = new System.Drawing.Point(65, 59);
            this.lblCarCrash.Name = "lblCarCrash";
            this.lblCarCrash.Size = new System.Drawing.Size(27, 29);
            this.lblCarCrash.TabIndex = 6;
            this.lblCarCrash.Text = "0";
            this.groupBox7.Controls.Add(this.lblTileCrash);
            this.groupBox7.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F,
                System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.groupBox7.Location = new System.Drawing.Point(259, 258);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(231, 66);
            this.groupBox7.TabIndex = 9;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "Tile with Most crashes";
            this.lblTileCrash.AutoSize = true;
            this.lblTileCrash.Location = new System.Drawing.Point(47, 34);
            this.lblTileCrash.Name = "lblTileCrash";
            this.lblTileCrash.Size = new System.Drawing.Size(47, 24);
            this.lblTileCrash.TabIndex = 0;
            this.lblTileCrash.Text = "(0,0)";
            this.groupBox8.Controls.Add(this.lblTileStopped);
            this.groupBox8.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular,
                System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.groupBox8.Location = new System.Drawing.Point(260, 331);
            this.groupBox8.Name = "groupBox8";
            this.groupBox8.Size = new System.Drawing.Size(230, 77);
            this.groupBox8.TabIndex = 10;
            this.groupBox8.TabStop = false;
            this.groupBox8.Text = "Tile with Most cars Stopped";
            this.lblTileStopped.AutoSize = true;
            this.lblTileStopped.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F,
                System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.lblTileStopped.Location = new System.Drawing.Point(46, 36);
            this.lblTileStopped.Name = "lblTileStopped";
            this.lblTileStopped.Size = new System.Drawing.Size(47, 24);
            this.lblTileStopped.TabIndex = 0;
            this.lblTileStopped.Text = "(0,0)";
            this.groupBox9.Controls.Add(this.lblHighAvgSpeedTile);
            this.groupBox9.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular,
                System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.groupBox9.Location = new System.Drawing.Point(260, 415);
            this.groupBox9.Name = "groupBox9";
            this.groupBox9.Size = new System.Drawing.Size(230, 70);
            this.groupBox9.TabIndex = 11;
            this.groupBox9.TabStop = false;
            this.groupBox9.Text = "Tile with Highest AVG speed";
            this.lblHighAvgSpeedTile.AutoSize = true;
            this.lblHighAvgSpeedTile.Location = new System.Drawing.Point(46, 33);
            this.lblHighAvgSpeedTile.Name = "lblHighAvgSpeedTile";
            this.lblHighAvgSpeedTile.Size = new System.Drawing.Size(41, 20);
            this.lblHighAvgSpeedTile.TabIndex = 0;
            this.lblHighAvgSpeedTile.Text = "(0,0)";
            this.tltldistance.AutoSize = true;
            this.tltldistance.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F,
                System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.tltldistance.Location = new System.Drawing.Point(6, 498);
            this.tltldistance.Name = "tltldistance";
            this.tltldistance.Size = new System.Drawing.Size(239, 24);
            this.tltldistance.TabIndex = 12;
            this.tltldistance.Text = "Total distance Travelled:";
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(496, 552);
            this.Controls.Add(this.tltldistance);
            this.Controls.Add(this.groupBox9);
            this.Controls.Add(this.groupBox8);
            this.Controls.Add(this.groupBox7);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "Graphs";
            this.Text = "test1";
            this.Load += new System.EventHandler(this.Graphs_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox7.ResumeLayout(false);
            this.groupBox7.PerformLayout();
            this.groupBox8.ResumeLayout(false);
            this.groupBox8.PerformLayout();
            this.groupBox9.ResumeLayout(false);
            this.groupBox9.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private LiveCharts.WinForms.AngularGauge angularGauge1;
        private System.Windows.Forms.Timer timer1;
        private LiveCharts.WinForms.SolidGauge solidGauge1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label lblTotalTime;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Label lblCarCrash;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.GroupBox groupBox8;
        private System.Windows.Forms.Label lblTileCrash;
        private System.Windows.Forms.Label lblTileStopped;
        private System.Windows.Forms.GroupBox groupBox9;
        private System.Windows.Forms.Label lblHighAvgSpeedTile;
        private System.Windows.Forms.Label tltldistance;
    }
}