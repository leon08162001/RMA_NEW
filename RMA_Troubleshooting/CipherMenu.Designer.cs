namespace RMA_Troubleshooting
{
    partial class CipherMenu
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.RMA = new System.Windows.Forms.ToolStripMenuItem();
            this.Return_Quote = new System.Windows.Forms.ToolStripMenuItem();
            this.Insert_Repair_Price = new System.Windows.Forms.ToolStripMenuItem();
            this.Generate_INVOICE_AR = new System.Windows.Forms.ToolStripMenuItem();
            this.Delete_Sales_Quoted = new System.Windows.Forms.ToolStripMenuItem();
            this.Generate_Quoted = new System.Windows.Forms.ToolStripMenuItem();
            this.pACKToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.Pack_InsertSn = new System.Windows.Forms.ToolStripMenuItem();
            this.Pack_ExcelInsertSn = new System.Windows.Forms.ToolStripMenuItem();
            this.reMoClounfToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.reImportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Dock = System.Windows.Forms.DockStyle.Left;
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.RMA,
            this.pACKToolStripMenuItem,
            this.reMoClounfToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(126, 153);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // RMA
            // 
            this.RMA.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Return_Quote,
            this.Insert_Repair_Price,
            this.Generate_INVOICE_AR,
            this.Delete_Sales_Quoted,
            this.Generate_Quoted});
            this.RMA.Name = "RMA";
            this.RMA.Size = new System.Drawing.Size(113, 20);
            this.RMA.Text = "RMA";
            // 
            // Return_Quote
            // 
            this.Return_Quote.Name = "Return_Quote";
            this.Return_Quote.Size = new System.Drawing.Size(180, 22);
            this.Return_Quote.Text = "回退至已收貨";
            this.Return_Quote.Click += new System.EventHandler(this.Return_Quote_Click);
            // 
            // Insert_Repair_Price
            // 
            this.Insert_Repair_Price.Name = "Insert_Repair_Price";
            this.Insert_Repair_Price.Size = new System.Drawing.Size(180, 22);
            this.Insert_Repair_Price.Text = "匯入維修價格";
            this.Insert_Repair_Price.Click += new System.EventHandler(this.Insert_Repair_Price_Click);
            // 
            // Generate_INVOICE_AR
            // 
            this.Generate_INVOICE_AR.Name = "Generate_INVOICE_AR";
            this.Generate_INVOICE_AR.Size = new System.Drawing.Size(180, 22);
            this.Generate_INVOICE_AR.Text = "補產生AR&INVOICE";
            this.Generate_INVOICE_AR.Click += new System.EventHandler(this.Generate_INVOICE_AR_Click);
            // 
            // Delete_Sales_Quoted
            // 
            this.Delete_Sales_Quoted.Name = "Delete_Sales_Quoted";
            this.Delete_Sales_Quoted.Size = new System.Drawing.Size(180, 22);
            this.Delete_Sales_Quoted.Text = "刪除Sales Quoted";
            this.Delete_Sales_Quoted.Click += new System.EventHandler(this.Delete_Sales_Quoted_Click);
            // 
            // Generate_Quoted
            // 
            this.Generate_Quoted.Name = "Generate_Quoted";
            this.Generate_Quoted.Size = new System.Drawing.Size(180, 22);
            this.Generate_Quoted.Text = "產生報價單";
            this.Generate_Quoted.Click += new System.EventHandler(this.Generate_Quoted_Click);
            // 
            // pACKToolStripMenuItem
            // 
            this.pACKToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Pack_InsertSn,
            this.Pack_ExcelInsertSn});
            this.pACKToolStripMenuItem.Name = "pACKToolStripMenuItem";
            this.pACKToolStripMenuItem.Size = new System.Drawing.Size(113, 20);
            this.pACKToolStripMenuItem.Text = "PACK";
            // 
            // Pack_InsertSn
            // 
            this.Pack_InsertSn.Name = "Pack_InsertSn";
            this.Pack_InsertSn.Size = new System.Drawing.Size(180, 22);
            this.Pack_InsertSn.Text = "匯入序號";
            this.Pack_InsertSn.Click += new System.EventHandler(this.Pack_InsertSn_Click);
            // 
            // Pack_ExcelInsertSn
            // 
            this.Pack_ExcelInsertSn.Name = "Pack_ExcelInsertSn";
            this.Pack_ExcelInsertSn.Size = new System.Drawing.Size(180, 22);
            this.Pack_ExcelInsertSn.Text = "EXCEL匯入序號";
            this.Pack_ExcelInsertSn.Click += new System.EventHandler(this.Pack_ExcelInsertSn_Click);
            // 
            // reMoClounfToolStripMenuItem
            // 
            this.reMoClounfToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.reImportToolStripMenuItem});
            this.reMoClounfToolStripMenuItem.Name = "reMoClounfToolStripMenuItem";
            this.reMoClounfToolStripMenuItem.Size = new System.Drawing.Size(113, 20);
            this.reMoClounfToolStripMenuItem.Text = "ReMoCloud";
            // 
            // reImportToolStripMenuItem
            // 
            this.reImportToolStripMenuItem.Name = "reImportToolStripMenuItem";
            this.reImportToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.reImportToolStripMenuItem.Text = "ReImport";
            this.reImportToolStripMenuItem.Click += new System.EventHandler(this.reImportToolStripMenuItem_Click);
            // 
            // CipherMenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(397, 153);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "CipherMenu";
            this.Text = "CipherMenu";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem RMA;
        private System.Windows.Forms.ToolStripMenuItem Return_Quote;
        private System.Windows.Forms.ToolStripMenuItem Insert_Repair_Price;
        private System.Windows.Forms.ToolStripMenuItem Generate_INVOICE_AR;
        private System.Windows.Forms.ToolStripMenuItem pACKToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem Pack_InsertSn;
        private System.Windows.Forms.ToolStripMenuItem Pack_ExcelInsertSn;
        private System.Windows.Forms.ToolStripMenuItem Delete_Sales_Quoted;
        private System.Windows.Forms.ToolStripMenuItem Generate_Quoted;
        private System.Windows.Forms.ToolStripMenuItem reMoClounfToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem reImportToolStripMenuItem;
    }
}