using Domain.CommonObject;
using Domain.Models.SpecialNote;
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
        public SpecialNoteDTO Get(PtId ptId, SinDate sinDate)
        {
            var ptInf = _tenantDataContext.PtInfs.SingleOrDefault(
                p => p.PtId == ptId.Value
                );
            var rsvInf = _tenantDataContext.RsvInfs.Where(r => r.PtId == 21144).ToList();
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
            specialNoteDTO.Comment = null;
            specialNoteDTO.PtId = ptId;
            return specialNoteDTO;
        }

        public IEnumerable<SpecialNoteDTO> GetAll()
        {
            throw new NotImplementedException();
        }
    }
}
