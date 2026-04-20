namespace SQLitePackItems.Forms
{
    partial class PackItemsReport
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
            this.reportViewer1 = new Microsoft.Reporting.WinForms.ReportViewer();
            this.sqlText = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // reportViewer1
            // 
            this.reportViewer1.LocalReport.ReportEmbeddedResource = "SQLitePackItems.Reports.Report1.rdlc";
            this.reportViewer1.Location = new System.Drawing.Point(4, 30);
            this.reportViewer1.Name = "reportViewer1";
            this.reportViewer1.ServerReport.BearerToken = null;
            this.reportViewer1.Size = new System.Drawing.Size(1200, 547);
            this.reportViewer1.TabIndex = 0;
            // 
            // sqlText
            // 
            this.sqlText.AutoSize = true;
            this.sqlText.Location = new System.Drawing.Point(12, 9);
            this.sqlText.Name = "sqlText";
            this.sqlText.Size = new System.Drawing.Size(12, 13);
            this.sqlText.TabIndex = 1;
            this.sqlText.Text = "x";
            // 
            // PackItemsReport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1209, 586);
            this.Controls.Add(this.sqlText);
            this.Controls.Add(this.reportViewer1);
            this.Name = "PackItemsReport";
            this.Text = "Pack Items Report";
            this.Load += new System.EventHandler(this.PackItemsReport_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Microsoft.Reporting.WinForms.ReportViewer reportViewer1;
        private System.Windows.Forms.Label sqlText;
    }
}