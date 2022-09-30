using DevExpress.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevExpress.Interface
{
    public interface IKarte2Export
    {
        void ExportToPdf(Karte2ExportModel karte2ExportModel, Stream stream);
        void ExportToPdf(Karte2ExportModel karte2ExportModel);
    }
}
