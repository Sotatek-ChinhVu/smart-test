using Domain.Models.Insurance;
using Domain.Models.PatientInfor;
using Helper.Constants;
using Infrastructure.Interfaces;
using Schema.Insurance.SaveInsuranceScan;

namespace Interactor.Schema
{
    public class SaveInsuranceScanInteractor : ISaveInsuranceScanInputPort
    {
        private readonly IAmazonS3Service _amazonS3Service;
        private readonly IInsuranceRepository _insuranceRepository;
        private readonly IPatientInforRepository _patientInforRepository;

        public SaveInsuranceScanInteractor(IAmazonS3Service amazonS3Service, IInsuranceRepository insuranceRepository, IPatientInforRepository patientInforRepository )
        {
            _amazonS3Service = amazonS3Service;
            _insuranceRepository = insuranceRepository;
            _patientInforRepository = patientInforRepository;
        }

        public SaveInsuranceScanOutputData Handle(SaveInsuranceScanInputData inputData)
        {
            var filePathCreated = new List<string>();
            try
            {
                if (inputData.InsuranceScans == null || !inputData.InsuranceScans.Any())
                    return new SaveInsuranceScanOutputData(SaveInsuranceScanStatus.InvalidNoDataSave, filePathCreated);

                var ptInf = _patientInforRepository.GetById(inputData.HpId, inputData.InsuranceScans.FirstOrDefault()?.PtId ?? 0, 0, 0);
                if (ptInf == null)
                    return new SaveInsuranceScanOutputData(SaveInsuranceScanStatus.InvalidPtId, filePathCreated);

                var listFolders = new List<string>() { CommonConstants.Store, CommonConstants.InsuranceScan };

                string path = string.Empty;
                foreach (var item in inputData.InsuranceScans)
                {
                    if (item.IsDeleted == DeleteTypes.Deleted) // Delete
                    {
                        if (!string.IsNullOrEmpty(item.FileName))
                        {
                            _amazonS3Service.DeleteObjectAsync(item.FileName);
                        }
                        _insuranceRepository.DeleteInsuranceScan(inputData.HpId, item.SeqNo, inputData.UserId);
                    }
                    else
                    {
                        if (item.File.Length > 0) //File is existings
                        {
                            path = _amazonS3Service.GetFolderUploadToPtNum(listFolders, ptInf.PtNum);

                            string fileName = $"{ptInf.PtId.ToString().PadLeft(10, '0')}{item.HokenId}.png";
                            fileName = _amazonS3Service.GetUniqueFileNameKey(fileName);

                            string pathScan = _amazonS3Service.UploadObjectAsync(path, fileName, item.File, true).Result;
                            filePathCreated.Add(pathScan);
                            //Create or update
                            _insuranceRepository.SaveInsuraneScan(new InsuranceScanModel(inputData.HpId,
                                                                                ptInf.PtId,
                                                                                item.SeqNo,
                                                                                item.HokenGrp,
                                                                                item.HokenId,
                                                                                pathScan,
                                                                                Stream.Null,
                                                                                0), inputData.UserId);

                            if (item.SeqNo > 0 && !string.IsNullOrEmpty(item.FileName)) //case udpate && file exists on s3 do not need to use
                            {
                                _amazonS3Service.DeleteObjectAsync(item.FileName);
                            }
                        }
                        else
                        {
                            continue;
                        }
                    }
                }
                return new SaveInsuranceScanOutputData(SaveInsuranceScanStatus.Successful, filePathCreated);
            }
            catch
            {
                return new SaveInsuranceScanOutputData(SaveInsuranceScanStatus.Error, filePathCreated);
            }
            finally
            {
                _patientInforRepository.ReleaseResource();
                _insuranceRepository.ReleaseResource();
            }
        }
    }
}
