namespace RMA_Troubleshooting
{
    partial class Generate_Quoted
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
            this.btn_quoted = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.tb_RMANO = new System.Windows.Forms.TextBox();
            this.CrystalReportViewer1 = new CrystalDecisions.Windows.Forms.CrystalReportViewer();
            this.SuspendLayout();
            // 
            // btn_quoted
            // 
            this.btn_quoted.Location = new System.Drawing.Point(166, 89);
            this.btn_quoted.Name = "btn_quoted";
            this.btn_quoted.Size = new System.Drawing.Size(94, 36);
            this.btn_quoted.TabIndex = 0;
            this.btn_quoted.Text = "產生報價單";
            this.btn_quoted.UseVisualStyleBackColor = true;
            this.btn_quoted.Click += new System.EventHandler(this.btn_quoted_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label1.Location = new System.Drawing.Point(69, 41);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(81, 20);
            this.label1.TabIndex = 13;
            this.label1.Text = "RMA單號:";
            // 
            // tb_RMANO
            // 
            this.tb_RMANO.Font = new System.Drawing.Font("微軟正黑體", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.tb_RMANO.Location = new System.Drawing.Point(166, 39);
            this.tb_RMANO.Name = "tb_RMANO";
            this.tb_RMANO.Size = new System.Drawing.Size(188, 27);
            this.tb_RMANO.TabIndex = 12;
            // 
            // CrystalReportViewer1
            // 
            this.CrystalReportViewer1.ActiveViewIndex = -1;
            this.CrystalReportViewer1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.CrystalReportViewer1.Cursor = System.Windows.Forms.Cursors.Default;
            this.CrystalReportViewer1.Location = new System.Drawing.Point(38, 131);
            this.CrystalReportViewer1.Name = "CrystalReportViewer1";
            this.CrystalReportViewer1.Size = new System.Drawing.Size(150, 150);
            this.CrystalReportViewer1.TabIndex = 14;
            this.CrystalReportViewer1.Visible = false;
            // 
            // Generate_Quoted
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(506, 217);
            this.Controls.Add(this.CrystalReportViewer1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tb_RMANO);
            this.Controls.Add(this.btn_quoted);
            this.Name = "Generate_Quoted";
            this.Text = "Generate_Quoted";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_quoted;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tb_RMANO;
        private CrystalDecisions.Windows.Forms.CrystalReportViewer CrystalReportViewer1;
    }
}