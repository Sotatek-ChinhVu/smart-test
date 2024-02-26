using Domain.Models.HpInf;
using Domain.Models.Ka;
using Domain.Models.SetGenerationMst;
using Domain.Models.SetKbnMst;
using Domain.Models.User;
using Infrastructure.Interfaces;
using Infrastructure.Logger;
using UseCase.SetKbnMst.Upsert;

namespace Interactor.SetKbnMst
{
    public class UpsertSetKbnMstInteractor : IUpsertSetKbnMstInputPort
    {
        private readonly ISetKbnMstRepository _setKbnMstRepository;
        private readonly IKaRepository _kaRepository;
        private readonly IUserRepository _userRepository;
        private readonly ISetGenerationMstRepository _setGenerationRepository;
        private readonly IHpInfRepository _hpInfRepository;
        private readonly ILoggingHandler _loggingHandler;
        private readonly ITenantProvider _tenantProvider;
        public UpsertSetKbnMstInteractor(ITenantProvider tenantProvider, ISetKbnMstRepository setKbnMstRepository, ISetGenerationMstRepository setGenerationRepository, IKaRepository kaRepository, IUserRepository userRepository, IHpInfRepository hpInfRepository)
        {
            _setKbnMstRepository = setKbnMstRepository;
            _setGenerationRepository = setGenerationRepository;
            _kaRepository = kaRepository;
            _userRepository = userRepository;
            _hpInfRepository = hpInfRepository;
            _tenantProvider = tenantProvider;
            _loggingHandler = new LoggingHandler(_tenantProvider.CreateNewTrackingAdminDbContextOption(), tenantProvider);
        }

        public UpsertSetKbnMstOutputData Handle(UpsertSetKbnMstInputData inputData)
        {
            try
            {
                if (inputData.SetKbnMstItems.Count() == 0)
                {
                    return new UpsertSetKbnMstOutputData(UpsertSetKbnMstStatus.InvalidInputData);
                }
                if (inputData.SinDate <= 0)
                {
                    return new UpsertSetKbnMstOutputData(UpsertSetKbnMstStatus.InvalidSinDate);
                }
                foreach (var setKbnMstItem in inputData.SetKbnMstItems)
                {
                    if (setKbnMstItem.HpId <= 0)
                    {
                        return new UpsertSetKbnMstOutputData(UpsertSetKbnMstStatus.InvalidHpId);
                    }
                    if (setKbnMstItem.SetKbn < 0)
                    {
                        return new UpsertSetKbnMstOutputData(UpsertSetKbnMstStatus.InvalidSetKbn);
                    }
                    if (setKbnMstItem.SetKbnEdaNo < 0)
                    {
                        return new UpsertSetKbnMstOutputData(UpsertSetKbnMstStatus.InvalidSetKbnEdaNo);
                    }
                    if (setKbnMstItem.SetKbnName.Length > 60)
                    {
                        return new UpsertSetKbnMstOutputData(UpsertSetKbnMstStatus.InvalidSetKbnName);
                    }
                    if (setKbnMstItem.KaCd < 0)
                    {
                        return new UpsertSetKbnMstOutputData(UpsertSetKbnMstStatus.InvalidKaCd);
                    }
                    if (setKbnMstItem.DocCd < 0)
                    {
                        return new UpsertSetKbnMstOutputData(UpsertSetKbnMstStatus.InvalidDocCd);
                    }
                    if (!(setKbnMstItem.IsDeleted >= 0 && setKbnMstItem.IsDeleted <= 2))
                    {
                        return new UpsertSetKbnMstOutputData(UpsertSetKbnMstStatus.InvalidIsDelete);
                    }
                    if (setKbnMstItem.GenerationId < 0)
                    {
                        return new UpsertSetKbnMstOutputData(UpsertSetKbnMstStatus.InvalidGenerationId);
                    }
                }
                var kaIds = inputData.SetKbnMstItems.Where(s => s.KaCd > 0).Select(s => s.KaCd).Distinct().ToList();
                bool checkkaId = _kaRepository.CheckKaId(kaIds, inputData.HpId);
                var userIds = inputData.SetKbnMstItems.Where(s => s.DocCd > 0).Select(s => s.DocCd).Distinct().ToList();
                bool checkUserId = _userRepository.GetDoctorsList(inputData.HpId, userIds).Count() == inputData.SetKbnMstItems.Where(s => s.DocCd > 0).Distinct().Select(s => s.DocCd).Count();
                if (!checkkaId)
                {
                    return new UpsertSetKbnMstOutputData(UpsertSetKbnMstStatus.InvalidKaCd);
                }
                if (!checkUserId)
                {
                    return new UpsertSetKbnMstOutputData(UpsertSetKbnMstStatus.InvalidDocCd);
                }
                var hpIds = inputData.SetKbnMstItems.Select(s => s.HpId).Distinct().ToList();
                if (hpIds.Count > 1)
                {
                    return new UpsertSetKbnMstOutputData(UpsertSetKbnMstStatus.InvalidHpId);
                }
                bool checkHpId = _hpInfRepository.CheckHpId(hpIds.First());
                if (!checkHpId)
                {
                    return new UpsertSetKbnMstOutputData(UpsertSetKbnMstStatus.InvalidHpId);
                }

                var generationIds = inputData.SetKbnMstItems.Select(s => s.GenerationId).Distinct().ToList();
                if (generationIds.Count > 1)
                {
                    return new UpsertSetKbnMstOutputData(UpsertSetKbnMstStatus.InvalidGenerationId);
                }
                var generationId = _setGenerationRepository.GetGenerationId(hpIds.First(), inputData.SinDate);
                if (generationId != generationIds.First())
                {
                    return new UpsertSetKbnMstOutputData(UpsertSetKbnMstStatus.InvalidGenerationId);
                }
                var result = _setKbnMstRepository.Upsert(hpIds.First(), inputData.UserId, generationId, inputData.SetKbnMstItems.Select(
                             s => new SetKbnMstModel(
                                    s.HpId,
                                    s.SetKbn,
                                    s.SetKbnEdaNo,
                                    s.SetKbnName,
                                    s.KaCd,
                                    s.DocCd,
                                    s.IsDeleted,
                                    s.GenerationId
                                 )
                            ).ToList());
                if (!result)
                {
                    return new UpsertSetKbnMstOutputData(UpsertSetKbnMstStatus.Failed);
                }
                return new UpsertSetKbnMstOutputData(UpsertSetKbnMstStatus.Successed);
            }
            catch (Exception ex)
            {
                _loggingHandler.WriteLogExceptionAsync(ex);
                throw;
            }
            finally
            {
                _setGenerationRepository.ReleaseResource();
                _setKbnMstRepository.ReleaseResource();
                _kaRepository.ReleaseResource();
                _userRepository.ReleaseResource();
                _loggingHandler.Dispose();
            }
        }
    }
}