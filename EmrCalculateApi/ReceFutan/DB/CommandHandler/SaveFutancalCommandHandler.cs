using EmrCalculateApi.ReceFutan.Models;
using EmrCalculateApi.Constants;
using EmrCalculateApi.Extensions;
using EmrCalculateApi.Interface;
using Entity.Tenant;
using PostgreDataContext;


namespace EmrCalculateApi.ReceFutan.DB.CommandHandler
{
    public class SaveFutancalCommandHandler
    {
        private readonly TenantDataContext _tenantDataContext;
        private readonly IEmrLogger _emrLogger;
        public SaveFutancalCommandHandler(TenantDataContext tenantDataContext, IEmrLogger emrLogger)
        {
            _tenantDataContext = tenantDataContext;
            _emrLogger = emrLogger;
        }

        public void AddReceInf(List<ReceInfModel> receInfModels)
        {
            const string conFncName = nameof(AddReceInf);
            try
            {
                List<ReceInf> receInfs = receInfModels.Select(x => x.ReceInf).ToList();

                receInfs.ForEach(x =>
                {
                    x.CreateDate = DateTime.Now;
                    x.CreateId = Hardcode.UserID;
                    x.CreateMachine = Hardcode.ComputerName;
                }
                );

                _tenantDataContext.ReceInfs.AddRange(receInfs);
            }
            catch (Exception E)
            {
                _emrLogger.WriteLogError(this, conFncName, E);
            }
        }

        public void UpdReceInfEdit(List<ReceInfEditModel> receInfEditModels, List<ReceInfPreEditModel> receInfPreEditModels)
        {
            const string conFncName = nameof(UpdReceInfEdit);
            try
            {
                List<ReceInfEdit> receInfEdits = receInfEditModels.Select(r => r.ReceInfEdit).Where(
                    r =>
                        !receInfPreEditModels.Any(p =>
                            p.PtId == r.PtId &&
                            p.SinYm == r.SinYm &&
                            p.HokenId == r.HokenId &&
                            p.ReceSbt == r.ReceSbt &&
                            p.Houbetu == r.Houbetu &&
                            p.Kohi1Houbetu == (r.Kohi1Houbetu ?? string.Empty) &&
                            p.Kohi2Houbetu == (r.Kohi2Houbetu ?? string.Empty) &&
                            p.Kohi3Houbetu == (r.Kohi3Houbetu ?? string.Empty) &&
                            p.Kohi4Houbetu == (r.Kohi4Houbetu ?? string.Empty)
                        )
                ).ToList();

                receInfEdits.ForEach(x =>
                    {
                        x.UpdateDate = DateTime.Now;
                        x.UpdateId = 0;  //計算による自動削除のため未指定
                        x.UpdateMachine = Hardcode.ComputerName;
                        x.IsDeleted = DeleteStatus.DeleteFlag;
                    }
                );

                _tenantDataContext.ReceInfEdits.UpdateRange(receInfEdits);
            }
            catch (Exception E)
            {
                _emrLogger.WriteLogError(this, conFncName, E);
            }
        }
        public void AddReceInfPreEdit(List<ReceInfPreEditModel> receInfPreEditModels)
        {
            const string conFncName = nameof(AddReceInfPreEdit);
            try
            {
                List<ReceInfPreEdit> receInfPreEdits = receInfPreEditModels.Select(x => x.ReceInfPreEdit).ToList();

                receInfPreEdits.ForEach(x =>
                {
                    x.CreateDate = DateTime.Now;
                    x.CreateId = Hardcode.UserID;
                    x.CreateMachine = Hardcode.ComputerName;
                }
                );

                _tenantDataContext.ReceInfPreEdits.AddRange(receInfPreEdits);
            }
            catch (Exception E)
            {
                _emrLogger.WriteLogError(this, conFncName, E);
            }
        }

        public void AddReceFutanKbn(List<ReceFutanKbnModel> receFutanKbnModels)
        {
            const string conFncName = nameof(AddReceFutanKbn);
            try
            {
                List<ReceFutanKbn> receFutanKbns = receFutanKbnModels.Select(x => x.ReceFutanKbn).ToList();

                receFutanKbns.ForEach(x =>
                {
                    x.CreateDate = DateTime.Now;
                    x.CreateId = Hardcode.UserID;
                    x.CreateMachine = Hardcode.ComputerName;
                }
                );

                _tenantDataContext.ReceFutanKbns.AddRange(receFutanKbns);
            }
            catch (Exception E)
            {
                _emrLogger.WriteLogError(this, conFncName, E);
            }
        }

        public void AddReceInfJd(List<ReceInfJdModel> receInfJdModels)
        {
            const string conFncName = nameof(AddReceInfJd);
            try
            {
                List<ReceInfJd> receInfJds = receInfJdModels.Select(x => x.ReceInfJd).ToList();

                receInfJds.ForEach(x =>
                {
                    x.CreateDate = DateTime.Now;
                    x.CreateId = Hardcode.UserID;
                    x.CreateMachine = Hardcode.ComputerName;
                }
                );

                _tenantDataContext.ReceInfJds.AddRange(receInfJds);
            }
            catch (Exception E)
            {
                _emrLogger.WriteLogError(this, conFncName, E);
            }
        }
    }
}
