using Interactor.ExportPDF.Karte2;

namespace DevExpress.Inteface;

public interface IKarte2Export
{
    MemoryStream ExportToPdf(Karte2ExportInput inputData);
}
