using Microsoft.Reporting.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SQLitePackItems.Forms
{
    public partial class PackItemsReport : Form
    {
        private PackItemsManager _packItemsManager;
        private BindingSource _packItemsBindingSource = new BindingSource();
        private string sqlQuery;   
        public PackItemsReport(string query)
        {
            InitializeComponent();
            _packItemsManager = new PackItemsManager(new PackItemsRepository());
            sqlText.Text = query;
            sqlQuery = query;
        }

        private void PackItemsReport_Load(object sender, EventArgs e)
        {

            this.reportViewer1.RefreshReport();


            try {
                BindingSource _packItemsLOBindingSource = new BindingSource();
                _packItemsLOBindingSource.DataSource = _packItemsManager.PackItemsLOQuery(sqlQuery);
                _packItemsLOBindingSource.Sort = "ListOrder";

                reportViewer1.Clear();
                reportViewer1.LocalReport.DataSources.Clear();
                ReportDataSource rds = new ReportDataSource("DataSet1", _packItemsLOBindingSource);

                reportViewer1.LocalReport.DataSources.Add(rds);
                reportViewer1.RefreshReport();
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }
    }
}
