namespace TestApp
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea9 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Series series9 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea10 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Series series10 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.button1 = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.chart2 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.button2 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart2)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            resources.ApplyResources(this.button1, "button1");
            this.button1.Name = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label7
            // 
            this.label7.AllowDrop = true;
            this.label7.AutoEllipsis = true;
            resources.ApplyResources(this.label7, "label7");
            this.label7.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.label7.Name = "label7";
            // 
            // chart1
            // 
            chartArea9.AlignmentOrientation = System.Windows.Forms.DataVisualization.Charting.AreaAlignmentOrientations.Horizontal;
            chartArea9.Name = "ChartArea1";
            this.chart1.ChartAreas.Add(chartArea9);
            resources.ApplyResources(this.chart1, "chart1");
            this.chart1.Name = "chart1";
            series9.ChartArea = "ChartArea1";
            series9.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
            series9.EmptyPointStyle.IsValueShownAsLabel = true;
            series9.EmptyPointStyle.LabelBorderDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Dash;
            series9.IsXValueIndexed = true;
            series9.MarkerSize = 10;
            series9.Name = "Series1";
            this.chart1.Series.Add(series9);
            // 
            // chart2
            // 
            chartArea10.AlignmentOrientation = System.Windows.Forms.DataVisualization.Charting.AreaAlignmentOrientations.Horizontal;
            chartArea10.Name = "ChartArea1";
            this.chart2.ChartAreas.Add(chartArea10);
            resources.ApplyResources(this.chart2, "chart2");
            this.chart2.Name = "chart2";
            series10.ChartArea = "ChartArea1";
            series10.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
            series10.EmptyPointStyle.IsValueShownAsLabel = true;
            series10.EmptyPointStyle.LabelBorderDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Dash;
            series10.IsXValueIndexed = true;
            series10.MarkerSize = 10;
            series10.Name = "Series1";
            this.chart2.Series.Add(series10);
            // 
            // button2
            // 
            resources.ApplyResources(this.button2, "button2");
            this.button2.Name = "button2";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // Form1
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.button2);
            this.Controls.Add(this.chart2);
            this.Controls.Add(this.chart1);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.button1);
            this.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.Name = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
        public System.Windows.Forms.Label label7;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart2;
        private System.Windows.Forms.Button button2;
    }
}

