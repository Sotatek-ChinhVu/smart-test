using DevExpress.XtraReports.UI;
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;

namespace DevExpress.Template
{
    public partial class Karte1Template_page1 : DevExpress.XtraReports.UI.XtraReport
    {
        public Karte1Template_page1()
        {
            InitializeComponent();
        }

        private void groupFooterPage1_PrintOnPage(object sender, PrintOnPageEventArgs e)
        {
            if (e.PageCount > 0 && e.PageIndex > 0)
            {
                // Cancels the control's printing.
                e.Cancel = true;
                groupFooterPage1.SizeF = new SizeF(0, 0);
                groupFooterPage1.Size = new Size(0, 0);
            }
        }
    }
}
