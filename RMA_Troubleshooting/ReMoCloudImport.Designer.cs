
namespace RMA_Troubleshooting
{
    partial class ReMoCloudImport
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
            this.btn_Qry = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btn_Qry
            // 
            this.btn_Qry.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btn_Qry.Location = new System.Drawing.Point(243, 25);
            this.btn_Qry.Name = "btn_Qry";
            this.btn_Qry.Size = new System.Drawing.Size(84, 26);
            this.btn_Qry.TabIndex = 2;
            this.btn_Qry.Text = "匯入";
            this.btn_Qry.UseVisualStyleBackColor = true;
            this.btn_Qry.Click += new System.EventHandler(this.btn_Qry_Click);
            // 
            // ReMoCloudImport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(773, 417);
            this.Controls.Add(this.btn_Qry);
            this.Name = "ReMoCloudImport";
            this.Text = "ReMoCloudImport";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btn_Qry;
    }
}