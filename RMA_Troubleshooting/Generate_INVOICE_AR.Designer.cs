namespace RMA_Troubleshooting
{
    partial class Generate_INVOICE_AR
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            this.btn_Search = new System.Windows.Forms.Button();
            this.dgv_RMA = new System.Windows.Forms.DataGridView();
            this.dgv_Shipping = new System.Windows.Forms.DataGridView();
            this.btn_RunSP = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.tb_RMANO = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_RMA)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_Shipping)).BeginInit();
            this.SuspendLayout();
            // 
            // btn_Search
            // 
            this.btn_Search.Location = new System.Drawing.Point(402, 34);
            this.btn_Search.Name = "btn_Search";
            this.btn_Search.Size = new System.Drawing.Size(75, 23);
            this.btn_Search.TabIndex = 0;
            this.btn_Search.Text = "Search";
            this.btn_Search.UseVisualStyleBackColor = true;
            this.btn_Search.Click += new System.EventHandler(this.btn_Search_Click);
            // 
            // dgv_RMA
            // 
            this.dgv_RMA.AllowUserToAddRows = false;
            this.dgv_RMA.AllowUserToDeleteRows = false;
            this.dgv_RMA.AllowUserToOrderColumns = true;
            this.dgv_RMA.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("新細明體", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgv_RMA.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgv_RMA.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("新細明體", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgv_RMA.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgv_RMA.Location = new System.Drawing.Point(11, 78);
            this.dgv_RMA.Margin = new System.Windows.Forms.Padding(2);
            this.dgv_RMA.Name = "dgv_RMA";
            this.dgv_RMA.ReadOnly = true;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("新細明體", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgv_RMA.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dgv_RMA.RowHeadersVisible = false;
            this.dgv_RMA.RowTemplate.Height = 24;
            this.dgv_RMA.Size = new System.Drawing.Size(761, 126);
            this.dgv_RMA.TabIndex = 7;
            this.dgv_RMA.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgv_RMA_CellClick);
            // 
            // dgv_Shipping
            // 
            this.dgv_Shipping.AllowUserToAddRows = false;
            this.dgv_Shipping.AllowUserToDeleteRows = false;
            this.dgv_Shipping.AllowUserToOrderColumns = true;
            this.dgv_Shipping.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("新細明體", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgv_Shipping.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.dgv_Shipping.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("新細明體", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgv_Shipping.DefaultCellStyle = dataGridViewCellStyle5;
            this.dgv_Shipping.Location = new System.Drawing.Point(11, 224);
            this.dgv_Shipping.Margin = new System.Windows.Forms.Padding(2);
            this.dgv_Shipping.Name = "dgv_Shipping";
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("新細明體", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            dataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgv_Shipping.RowHeadersDefaultCellStyle = dataGridViewCellStyle6;
            this.dgv_Shipping.RowHeadersVisible = false;
            this.dgv_Shipping.RowTemplate.Height = 24;
            this.dgv_Shipping.Size = new System.Drawing.Size(761, 149);
            this.dgv_Shipping.TabIndex = 8;
            // 
            // btn_RunSP
            // 
            this.btn_RunSP.Location = new System.Drawing.Point(494, 34);
            this.btn_RunSP.Name = "btn_RunSP";
            this.btn_RunSP.Size = new System.Drawing.Size(75, 23);
            this.btn_RunSP.TabIndex = 9;
            this.btn_RunSP.Text = "RunSP";
            this.btn_RunSP.UseVisualStyleBackColor = true;
            this.btn_RunSP.Click += new System.EventHandler(this.btn_RunSP_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label1.Location = new System.Drawing.Point(76, 34);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(81, 20);
            this.label1.TabIndex = 11;
            this.label1.Text = "RMA單號:";
            // 
            // tb_RMANO
            // 
            this.tb_RMANO.Font = new System.Drawing.Font("微軟正黑體", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.tb_RMANO.Location = new System.Drawing.Point(173, 32);
            this.tb_RMANO.Name = "tb_RMANO";
            this.tb_RMANO.Size = new System.Drawing.Size(188, 27);
            this.tb_RMANO.TabIndex = 10;
            // 
            // Generate_INVOICE_AR
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(783, 407);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tb_RMANO);
            this.Controls.Add(this.btn_RunSP);
            this.Controls.Add(this.dgv_Shipping);
            this.Controls.Add(this.dgv_RMA);
            this.Controls.Add(this.btn_Search);
            this.Name = "Generate_INVOICE_AR";
            this.Text = "Generate_INVOICE_AR";
            ((System.ComponentModel.ISupportInitialize)(this.dgv_RMA)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_Shipping)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_Search;
        private System.Windows.Forms.DataGridView dgv_RMA;
        private System.Windows.Forms.DataGridView dgv_Shipping;
        private System.Windows.Forms.Button btn_RunSP;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tb_RMANO;
    }
}