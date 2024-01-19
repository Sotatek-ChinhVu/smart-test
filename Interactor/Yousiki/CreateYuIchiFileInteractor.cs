using Domain.Models.Yousiki;
using Helper.Extension;
using System.Text;
using UseCase.Yousiki.CreateYuIchiFile;

namespace Interactor.Yousiki;

public class CreateYuIchiFileInteractor : ICreateYuIchiFileInputPort
{
    private readonly IYousikiRepository _yousikiRepository;
    private const string mInp00010 = "mInp00010";
    private const string mFree00030 = "mFree00030";
    private const string mFree00040 = "mFree00040";
    private const string confirmMessage = "confirmMessage";

    public CreateYuIchiFileInteractor(IYousikiRepository yousikiRepository)
    {
        _yousikiRepository = yousikiRepository;
    }

    public CreateYuIchiFileOutputData Handle(CreateYuIchiFileInputData inputData)
    {
        try
        {
            bool isExported = false;
            var validateData = ValidateData(inputData);
            if (validateData.Status != CreateYuIchiFileStatus.ValidateSuccessed)
            {
                return validateData;
            }
            var yousiki1InfList = _yousikiRepository.GetListYousiki1Inf(inputData.HpId, inputData.SinYm);

            // Send message confirm to FE
            if (yousiki1InfList.Any() && !inputData.ReactCreateYuIchiFile.ConfirmPatientList)
            {
                var unregistedPatients = yousiki1InfList.FindAll(p => p.Status == 0 || p.Status == 1)
                                                        .GroupBy(item => item.PtId).Select(item => item.FirstOrDefault())
                                                        .ToList();
                if (unregistedPatients.Count > 0)
                {
                    StringBuilder patientStringBuilder = new();
                    foreach (var unregistedPatient in unregistedPatients)
                    {
                        patientStringBuilder.Append($"{unregistedPatient?.SinYm / 100}/{unregistedPatient?.SinYm % 100} ID:{unregistedPatient?.PtNum.AsString(),-10} {unregistedPatient?.Name ?? string.Empty}" + Environment.NewLine);
                    }
                    var patientList = patientStringBuilder.ToString().TrimEnd(Environment.NewLine.ToCharArray());
                    return new CreateYuIchiFileOutputData(confirmMessage, patientList, CreateYuIchiFileStatus.Failed);
                }
            }

            if (inputData.IsCreateForm1File)
            {
                isExported = ExportOutpatientForm1() || isExported;
            }
            if (inputData.IsCreateEFFile || inputData.IsCreateEFile || inputData.IsCreateFFile)
            {
                isExported = ExportEFFile() || isExported;
            }
            if (inputData.IsCreateKData)
            {
                isExported = ExportForeignKCsvFile() || isExported;
            }
            if (isExported)
            {
                return new CreateYuIchiFileOutputData(mFree00040, string.Empty, CreateYuIchiFileStatus.CreateYuIchiFileSuccessed);
            }
            return new CreateYuIchiFileOutputData(mFree00040, string.Empty, CreateYuIchiFileStatus.CreateYuIchiFileFailed);
        }
        finally
        {
            _yousikiRepository.ReleaseResource();
        }
    }

    private bool ExportForeignKCsvFile()
    {
        throw new NotImplementedException();
    }

    private bool ExportEFFile()
    {
        throw new NotImplementedException();
    }

    private bool ExportOutpatientForm1()
    {
        throw new NotImplementedException();
    }

    private CreateYuIchiFileOutputData ValidateData(CreateYuIchiFileInputData data)
    {
        if (data.SinYm <= 0)
        {
            return new CreateYuIchiFileOutputData(mInp00010, string.Empty, CreateYuIchiFileStatus.InvalidCreateYuIchiFileSinYm);
        }
        if (!data.IsCreateForm1File
            && !data.IsCreateEFFile
            && !data.IsCreateEFile
            && !data.IsCreateFFile
            && !data.IsCreateKData)
        {
            return new CreateYuIchiFileOutputData(mFree00030, string.Empty, CreateYuIchiFileStatus.InvalidCreateYuIchiFileSinYm);
        }
        return new CreateYuIchiFileOutputData(string.Empty, string.Empty, CreateYuIchiFileStatus.ValidateSuccessed);
    }
}
