using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Reporting.WinForms;

namespace SQLitePackItems.Forms
{
    public partial class BackItemsReport : Form
    {
        private PackItemsManager _packItemsManager;
        private BindingSource _packItemsBindingSource = new BindingSource();
        public BackItemsReport()
        {
            InitializeComponent();
            _packItemsManager = new PackItemsManager(new PackItemsRepository());
        }

        private void BackItemsReport_Load(object sender, EventArgs e)
        {

            this.reportViewer1.RefreshReport();

            /*
            try {
            string query = "SELECT * FROM PackItems";
            BindingSource _packItemsLOBindingSource = new BindingSource();
            _packItemsLOBindingSource.DataSource = _packItemsManager.PackItemsLO(query);
            _packItemsLOBindingSource.Sort = "ListOrder";

            reportViewer1.Clear();
            reportViewer1.LocalReport.DataSources.Clear();
            ReportDataSource rds = new ReportDataSource("DataSet1", _packItemsLOBindingSource);

            reportViewer1.LocalReport.DataSources.Add(rds);
            this.reportViewer1.RefreshReport();
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            */
        }
    }
}
