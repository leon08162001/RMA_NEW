namespace RMA_Troubleshooting
{
    partial class Return_Quote
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

        #region Windows Form 設計工具產生的程式碼

        /// <summary>
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器
        /// 修改這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            this.tb_RMANO = new System.Windows.Forms.TextBox();
            this.btn_Qry = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.dgv_RMADetail = new System.Windows.Forms.DataGridView();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btn_return30 = new System.Windows.Forms.Button();
            this.dgv_RMA = new System.Windows.Forms.DataGridView();
            this.btn_return20 = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.lbl_msg = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_RMADetail)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_RMA)).BeginInit();
            this.SuspendLayout();
            // 
            // tb_RMANO
            // 
            this.tb_RMANO.Font = new System.Drawing.Font("微軟正黑體", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.tb_RMANO.Location = new System.Drawing.Point(103, 20);
            this.tb_RMANO.Name = "tb_RMANO";
            this.tb_RMANO.Size = new System.Drawing.Size(188, 27);
            this.tb_RMANO.TabIndex = 0;
            // 
            // btn_Qry
            // 
            this.btn_Qry.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btn_Qry.Location = new System.Drawing.Point(318, 20);
            this.btn_Qry.Name = "btn_Qry";
            this.btn_Qry.Size = new System.Drawing.Size(84, 26);
            this.btn_Qry.TabIndex = 1;
            this.btn_Qry.Text = "查詢";
            this.btn_Qry.UseVisualStyleBackColor = true;
            this.btn_Qry.Click += new System.EventHandler(this.btn_Qry_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label1.Location = new System.Drawing.Point(6, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(81, 20);
            this.label1.TabIndex = 2;
            this.label1.Text = "RMA單號:";
            // 
            // dgv_RMADetail
            // 
            this.dgv_RMADetail.AllowUserToAddRows = false;
            this.dgv_RMADetail.AllowUserToDeleteRows = false;
            this.dgv_RMADetail.AllowUserToOrderColumns = true;
            this.dgv_RMADetail.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_RMADetail.Location = new System.Drawing.Point(5, 218);
            this.dgv_RMADetail.Margin = new System.Windows.Forms.Padding(2);
            this.dgv_RMADetail.Name = "dgv_RMADetail";
            this.dgv_RMADetail.RowHeadersVisible = false;
            this.dgv_RMADetail.RowTemplate.Height = 24;
            this.dgv_RMADetail.Size = new System.Drawing.Size(804, 144);
            this.dgv_RMADetail.TabIndex = 3;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btn_return30);
            this.groupBox1.Controls.Add(this.dgv_RMA);
            this.groupBox1.Controls.Add(this.btn_return20);
            this.groupBox1.Controls.Add(this.dgv_RMADetail);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.btn_Qry);
            this.groupBox1.Controls.Add(this.tb_RMANO);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(814, 381);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            // 
            // btn_return30
            // 
            this.btn_return30.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btn_return30.Location = new System.Drawing.Point(536, 20);
            this.btn_return30.Name = "btn_return30";
            this.btn_return30.Size = new System.Drawing.Size(146, 26);
            this.btn_return30.TabIndex = 7;
            this.btn_return30.Text = "回退保留報價資訊(30)";
            this.btn_return30.UseVisualStyleBackColor = true;
            this.btn_return30.Click += new System.EventHandler(this.btn_return30_Click);
            // 
            // dgv_RMA
            // 
            this.dgv_RMA.AllowUserToAddRows = false;
            this.dgv_RMA.AllowUserToDeleteRows = false;
            this.dgv_RMA.AllowUserToOrderColumns = true;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("新細明體", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgv_RMA.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.dgv_RMA.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("新細明體", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgv_RMA.DefaultCellStyle = dataGridViewCellStyle5;
            this.dgv_RMA.Location = new System.Drawing.Point(5, 64);
            this.dgv_RMA.Margin = new System.Windows.Forms.Padding(2);
            this.dgv_RMA.Name = "dgv_RMA";
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("新細明體", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            dataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgv_RMA.RowHeadersDefaultCellStyle = dataGridViewCellStyle6;
            this.dgv_RMA.RowHeadersVisible = false;
            this.dgv_RMA.RowTemplate.Height = 24;
            this.dgv_RMA.Size = new System.Drawing.Size(804, 144);
            this.dgv_RMA.TabIndex = 6;
            this.dgv_RMA.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgv_RMA_CellClick);
            // 
            // btn_return20
            // 
            this.btn_return20.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btn_return20.Location = new System.Drawing.Point(408, 20);
            this.btn_return20.Name = "btn_return20";
            this.btn_return20.Size = new System.Drawing.Size(122, 26);
            this.btn_return20.TabIndex = 4;
            this.btn_return20.Text = "回退收貨(20)";
            this.btn_return20.UseVisualStyleBackColor = true;
            this.btn_return20.Click += new System.EventHandler(this.btn_return20_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label2.Location = new System.Drawing.Point(18, 402);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 20);
            this.label2.TabIndex = 5;
            this.label2.Text = "系統訊息:";
            // 
            // lbl_msg
            // 
            this.lbl_msg.AutoSize = true;
            this.lbl_msg.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lbl_msg.ForeColor = System.Drawing.Color.Red;
            this.lbl_msg.Location = new System.Drawing.Point(111, 402);
            this.lbl_msg.Name = "lbl_msg";
            this.lbl_msg.Size = new System.Drawing.Size(46, 20);
            this.lbl_msg.TabIndex = 6;
            this.lbl_msg.Text = "Error";
            // 
            // Return_Quote
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(838, 435);
            this.Controls.Add(this.lbl_msg);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.groupBox1);
            this.Font = new System.Drawing.Font("新細明體", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.Name = "Return_Quote";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Return Quote";
            ((System.ComponentModel.ISupportInitialize)(this.dgv_RMADetail)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_RMA)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tb_RMANO;
        private System.Windows.Forms.Button btn_Qry;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView dgv_RMADetail;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btn_return20;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lbl_msg;
        private System.Windows.Forms.DataGridView dgv_RMA;
        private System.Windows.Forms.Button btn_return30;
    }
}