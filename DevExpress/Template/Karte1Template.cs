using DevExpress.XtraReports.UI;
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;

namespace DevExpress.Template
{
    public partial class Karte1Template : DevExpress.XtraReports.UI.XtraReport
    {
        public Karte1Template()
        {
            InitializeComponent();
        }
        private void groupHeaderPage2_PrintOnPage(object sender, PrintOnPageEventArgs e)
        {
            if (e.PageCount > 0 && e.PageIndex == 0)
            {
                // Cancels the control's printing.
                e.Cancel = true;
            }
            if (e.PageCount > 0 && e.PageIndex > 0)
            {
                // Cancels the control's printing.
                groupFooterPage1.Visible = false;
            }
        }

        private void groupFooterPage1_PrintOnPage(object sender, PrintOnPageEventArgs e)
        {
            if (e.PageCount > 0 && e.PageIndex > 0)
            {
                // Cancels the control's printing.
                e.Cancel = true;
            }
        }
    }
}
