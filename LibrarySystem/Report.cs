using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CrystalDecisions.CrystalReports.Engine;

namespace LibrarySystem
{
    public partial class Report : Form
    {
        private ReportDocument rd;
        private DataTablesForReport reporttables;
        private DataTablesForReportTableAdapters.bookTableAdapter booktableadapter;

        public Report()
        {
            InitializeComponent();
        }

        private void Report_Load(object sender, EventArgs e)
        {
            comboBox1.DataSource = List.DiffReport;
            loadReport(comboBox1.SelectedIndex);
        }

        private void loadReport(int reportkind)
        {
            rd = new ReportDocument();

            switch (reportkind)
            {
                case 0:
                    reporttables = new DataTablesForReport(); // .xsd file name

                    // using the table adapter for custom query created from dataset
                    booktableadapter = new DataTablesForReportTableAdapters.bookTableAdapter();

                    reporttables.Tables[0].Merge(booktableadapter.GetData());
                    rd.Load(@"CrystalReport1.rpt");
                    rd.SetDataSource(reporttables);
                    break;
            }
            
            crystalReportViewer1.ReportSource = rd;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            loadReport(comboBox1.SelectedIndex);
        }
    }
}
