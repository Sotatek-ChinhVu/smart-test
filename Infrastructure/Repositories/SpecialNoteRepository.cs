using Domain.Models.SpecialNote;
using Entity.Tenant;
using Infrastructure.Interfaces;
using PostgreDataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class SpecialNoteRepository : ISpecialNoteRepository
    {
        private readonly TenantDataContext _tenantDataContext;
        public SpecialNoteRepository(ITenantProvider tenantProvider)
        {
            _tenantDataContext = tenantProvider.GetDataContext();
        }
        public SpecialNoteDTO Get(int ptId, int sinDate)
        {
            var ptInf = _tenantDataContext.PtInfs.SingleOrDefault(
                p => p.PtId == ptId
                );
            var cmtInf = _tenantDataContext.PtCmtInfs.SingleOrDefault(
                p => p.PtId == ptId);
            var kensaInfDetails = _tenantDataContext.KensaInfDetails.Where(p => p.PtId == ptId && p.IraiDate <= sinDate).OrderByDescending(p => p.IraiDate).Take(3).ToList();
            var rsvInf = _tenantDataContext.RsvInfs.Where(r => r.PtId == ptId).ToList();
            SpecialNoteDTO specialNoteDTO = new SpecialNoteDTO();
            System.Collections.IList list = rsvInf;
            for (int i = 0; i < list.Count; i++) {
                Entity.Tenant.RsvInf rsv = (Entity.Tenant.RsvInf)list[i];
                MedicalSchedule medicalSchedule = new MedicalSchedule();
                medicalSchedule.ConvertDate("" + rsv.SinDate);
                medicalSchedule.ConvertTime("" + rsv.StartTime);
                specialNoteDTO.MedicalSchedule.Add(medicalSchedule);
            }
            specialNoteDTO.Address = ptInf.HomeAddress1 != null ? ptInf.HomeAddress1 : (ptInf.HomeAddress2 ?? null);
            specialNoteDTO.Tel = ptInf.Tel1 != null ? ptInf.Tel1 : (ptInf.Tel2 ?? null);
            specialNoteDTO.Comment = cmtInf == null  ? null : cmtInf.Text == null ? null : cmtInf.Text;
            specialNoteDTO.PtId = ptId;
            specialNoteDTO.BodyIndex = ConvertKensaInf(kensaInfDetails);
            return specialNoteDTO;
        }

        private string? ConvertKensaInf(List<KensaInfDetail> kensaInDetails)
        {
            if (kensaInDetails == null || kensaInDetails.Count < 1) { return null; }
            string? result = null;
            double? rsVal1 = null;
            double? rsVal2 = null;
            int iraiDate = 0;
            foreach (KensaInfDetail kensaInfDetail in kensaInDetails)
            {
                if (kensaInfDetail.KensaItemCd == "V0001" && kensaInfDetail.ResultVal != null)
                {
                    result += kensaInfDetail.ResultVal + "cm" + " (" + ConvertDate(kensaInfDetail.IraiDate) + ") /";
                    rsVal1 = ConvertStringToDouble(kensaInfDetail.ResultVal);
                    continue;
                }
                if (kensaInfDetail.KensaItemCd == "V0002" && kensaInfDetail.ResultVal != null)
                {
                    result += kensaInfDetail.ResultVal + "kg" + " (" + ConvertDate(kensaInfDetail.IraiDate) + ") /";
                    rsVal2 = ConvertStringToDouble(kensaInfDetail.ResultVal);
                    continue;
                }
                iraiDate = kensaInfDetail.IraiDate;
            }
            if (rsVal1 != null && rsVal2 != null)
            {
                result += "BMI:" + Math.Round((double)(rsVal2 / (Math.Pow((double)(rsVal1 / 100), 2))), 1) + " (" + ConvertDate(iraiDate) + ")";
            }
            return result;
        }

        private double? ConvertStringToDouble(string data)
        {
            if (string.IsNullOrEmpty(data)) return null;
            double res;
            double.TryParse(data, out res);
            if (double.IsNaN(res) || double.IsInfinity(res))
            {
                return null;
            }
            return res;
        }

        private string? ConvertDate(int date)
        {
            if (date == null) return null;
            string rs = "" + date;
            return rs.Substring(0, 4) + "/" + rs.Substring(4, 2) + "/" + rs.Substring(6, 2);
        }

        public IEnumerable<SpecialNoteDTO> GetAll()
        {
            throw new NotImplementedException();
        }
    }
}
