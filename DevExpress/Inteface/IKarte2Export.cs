using DevExpress.Models.Karte2;
using Interactor.ExportPDF.Karte2;

namespace DevExpress.Inteface;

public interface IKarte2Export
{
    Karte2Output ExportToPdf(Karte2ExportInput inputData);
}
