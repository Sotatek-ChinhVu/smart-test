using Domain.Constant;
using Entity.Tenant;
using Helper.Constants;
using Helper.Extension;
using Infrastructure.Base;
using Infrastructure.Interfaces;
using Reporting.PatientManagement.Models;

namespace Reporting.PatientManagement.DB;

public class PatientManagementFinder : RepositoryBase, IPatientManagementFinder
{
    public PatientManagementFinder(ITenantProvider tenantProvider) : base(tenantProvider)
    {
    }

    public void ReleaseResource()
    {
        DisposeDataContext();
    }

    public List<GroupInfoSearchPatientModel> GetListGroupInfo(int hpId)
    {
        List<GroupInfoSearchPatientModel> result;
        var grs = NoTrackingDataContext.PtGrpNameMsts.Where(item => item.HpId == hpId && item.IsDeleted != DeleteStatus.DeleteFlag).OrderBy(gr => gr.SortNo).ToList();

        var ptGrpList = NoTrackingDataContext.PtGrpItems.Where(item => item.HpId == hpId && item.IsDeleted != DeleteStatus.DeleteFlag)
                                                        .ToList();

        var list = ptGrpList.Select(item => new GroupItemModel(item))
                            .OrderBy(item => item.GroupSortNo)
                            .ToList();

        result = grs.Select(gr => new GroupInfoSearchPatientModel(gr, null)
        {
            GroupItemSelected = null,
            ListItem = list.Where(li => li.GroupId == gr.GrpId).ToList()
        })
        .OrderBy(item => item.GroupSortNo)
        .ToList();
        result.ForEach(grItem =>
        {
            grItem.ListItem.Insert(0, new GroupItemModel(new PtGrpItem()));
        });

        return result;
    }

    public PatientManagementModel GetPatientManagement(int hpId, int menuId)
    {
        PatientManagementModel result = new();
        var staConfList = NoTrackingDataContext.StaConfs.Where(x => x.HpId == hpId && x.MenuId == menuId).ToList();

        string outputOrder = staConfList.FirstOrDefault(x => x.ConfId == StaConfId.OutputOrder)?.Val ?? string.Empty;
        result.OutputOrder = outputOrder.AsInteger();
        string outputOrder2 = staConfList.FirstOrDefault(x => x.ConfId == StaConfId.OutputOrder2)?.Val ?? string.Empty;
        if (!string.IsNullOrEmpty(outputOrder2)) result.OutputOrder2 = outputOrder2.AsInteger();
        string outputOrder3 = staConfList.FirstOrDefault(x => x.ConfId == StaConfId.OutputOrder3)?.Val ?? string.Empty;
        if (!string.IsNullOrEmpty(outputOrder3)) result.OutputOrder3 = outputOrder3.AsInteger();
        string reportType = staConfList.FirstOrDefault(x => x.ConfId == StaConfId.ReportType)?.Val ?? string.Empty;
        result.ReportType = reportType.AsInteger();
        string ptNumFrom = staConfList.FirstOrDefault(x => x.ConfId == StaConfId.PtNumFrom)?.Val ?? string.Empty;
        result.PtNumFrom = ptNumFrom.AsInteger();
        string ptNumTo = staConfList.FirstOrDefault(x => x.ConfId == StaConfId.PtNumTo)?.Val ?? string.Empty;
        result.PtNumTo = ptNumTo.AsInteger();
        string kanaName = staConfList.FirstOrDefault(x => x.ConfId == StaConfId.KanaName)?.Val ?? string.Empty;
        result.KanaName = kanaName.AsString();
        string name = staConfList.FirstOrDefault(x => x.ConfId == StaConfId.Name)?.Val ?? string.Empty;
        result.Name = name.AsString();
        string birthDayFrom = staConfList.FirstOrDefault(x => x.ConfId == StaConfId.BirthDayFrom)?.Val ?? string.Empty;
        result.BirthDayFrom = birthDayFrom.AsInteger();
        string birthDayTo = staConfList.FirstOrDefault(x => x.ConfId == StaConfId.BirthDayTo)?.Val ?? string.Empty;
        result.BirthDayTo = birthDayTo.AsInteger();
        string ageFrom = staConfList.FirstOrDefault(x => x.ConfId == StaConfId.AgeFrom)?.Val ?? string.Empty;
        result.AgeFrom = ageFrom;
        string ageTo = staConfList.FirstOrDefault(x => x.ConfId == StaConfId.AgeTo)?.Val ?? string.Empty;
        result.AgeTo = ageTo;
        string ageRefDate = staConfList.FirstOrDefault(x => x.ConfId == StaConfId.AgeRefDate)?.Val ?? string.Empty;
        result.AgeRefDate = ageRefDate.AsInteger();
        string sex = staConfList.FirstOrDefault(x => x.ConfId == StaConfId.Sex)?.Val ?? string.Empty;
        result.Sex = sex.AsInteger();
        string homePost = staConfList.FirstOrDefault(x => x.ConfId == StaConfId.HomePost)?.Val ?? string.Empty;
        if (!string.IsNullOrEmpty(homePost))
        {
            homePost = homePost.Replace("-", string.Empty);
            if (homePost.Length > 3)
            {
                result.ZipCD1 = homePost.Substring(0, 3);
                result.ZipCD2 = homePost.Substring(3);
            }
        }
        string address = staConfList.FirstOrDefault(x => x.ConfId == StaConfId.Address)?.Val ?? string.Empty;
        result.Address = address.AsString();
        string phoneNumber = staConfList.FirstOrDefault(x => x.ConfId == StaConfId.PhoneNumber)?.Val ?? string.Empty;
        result.PhoneNumber = phoneNumber.AsString();
        string registrationDateFrom = staConfList.FirstOrDefault(x => x.ConfId == StaConfId.RegistrationDateFrom)?.Val ?? string.Empty;
        result.RegistrationDateFrom = registrationDateFrom.AsInteger();
        string registrationDateTo = staConfList.FirstOrDefault(x => x.ConfId == StaConfId.RegistrationDateTo)?.Val ?? string.Empty;
        result.RegistrationDateTo = registrationDateTo.AsInteger();
        string includeTestPt = staConfList.FirstOrDefault(x => x.ConfId == StaConfId.IncludeTestPt)?.Val ?? string.Empty;
        result.IncludeTestPt = includeTestPt.AsInteger();
        string groupSelected = staConfList.FirstOrDefault(x => x.ConfId == StaConfId.GroupSelected)?.Val ?? string.Empty;
        result.GroupSelected = groupSelected.AsString();
        string hokensyaNoFrom = staConfList.FirstOrDefault(x => x.ConfId == StaConfId.HokensyaNoFrom)?.Val ?? string.Empty;
        result.HokensyaNoFrom = hokensyaNoFrom.AsString();
        string hokensyaNoTo = staConfList.FirstOrDefault(x => x.ConfId == StaConfId.HokensyaNoTo)?.Val ?? string.Empty;
        result.HokensyaNoTo = hokensyaNoTo.AsString();
        string kigo = staConfList.FirstOrDefault(x => x.ConfId == StaConfId.Kigo)?.Val ?? string.Empty;
        result.Kigo = kigo.AsString();
        string bango = staConfList.FirstOrDefault(x => x.ConfId == StaConfId.Bango)?.Val ?? string.Empty;
        result.Bango = bango.AsString();
        string hokenKbn = staConfList.FirstOrDefault(x => x.ConfId == StaConfId.HokenKbn)?.Val ?? string.Empty;
        result.HokenKbn = hokenKbn.AsInteger();
        string kohiFutansyaNoFrom = staConfList.FirstOrDefault(x => x.ConfId == StaConfId.KohiFutansyaNoFrom)?.Val ?? string.Empty;
        result.KohiFutansyaNoFrom = kohiFutansyaNoFrom.AsString();
        string kohiFutansyaNoTo = staConfList.FirstOrDefault(x => x.ConfId == StaConfId.KohiFutansyaNoTo)?.Val ?? string.Empty;
        result.KohiFutansyaNoTo = kohiFutansyaNoTo.AsString();
        string kohiTokusyuNoFrom = staConfList.FirstOrDefault(x => x.ConfId == StaConfId.KohiTokusyuNoFrom)?.Val ?? string.Empty;
        result.KohiTokusyuNoFrom = kohiTokusyuNoFrom.AsString();
        string kohiTokusyuNoTo = staConfList.FirstOrDefault(x => x.ConfId == StaConfId.KohiTokusyuNoTo)?.Val ?? string.Empty;
        result.KohiTokusyuNoTo = kohiTokusyuNoTo.AsString();
        string expireDateFrom = staConfList.FirstOrDefault(x => x.ConfId == StaConfId.ExpireDateFrom)?.Val ?? string.Empty;
        result.ExpireDateFrom = expireDateFrom.AsInteger();
        string expireDateTo = staConfList.FirstOrDefault(x => x.ConfId == StaConfId.ExpireDateTo)?.Val ?? string.Empty;
        result.ExpireDateTo = expireDateTo.AsInteger();
        string hokenSbt = staConfList.FirstOrDefault(x => x.ConfId == StaConfId.HokenSbt)?.Val ?? string.Empty;
        result.HokenSbt = string.IsNullOrEmpty(hokenSbt) ? new List<int>() : hokenSbt.Split(',').Select(x => x.AsInteger()).ToList();
        string houbetu1 = staConfList.FirstOrDefault(x => x.ConfId == StaConfId.Houbetu1)?.Val ?? string.Empty;
        result.Houbetu1 = houbetu1;
        string houbetu2 = staConfList.FirstOrDefault(x => x.ConfId == StaConfId.Houbetu2)?.Val ?? string.Empty;
        result.Houbetu2 = houbetu2;
        string houbetu3 = staConfList.FirstOrDefault(x => x.ConfId == StaConfId.Houbetu3)?.Val ?? string.Empty;
        result.Houbetu3 = houbetu3;
        string houbetu4 = staConfList.FirstOrDefault(x => x.ConfId == StaConfId.Houbetu4)?.Val ?? string.Empty;
        result.Houbetu4 = houbetu4;
        string houbetu5 = staConfList.FirstOrDefault(x => x.ConfId == StaConfId.Houbetu5)?.Val ?? string.Empty;
        result.Houbetu5 = houbetu5;
        string kogaku = staConfList.FirstOrDefault(x => x.ConfId == StaConfId.Kogaku)?.Val ?? string.Empty;
        result.Kogaku = kogaku.AsString();
        string kohiHokenNoFrom = staConfList.FirstOrDefault(x => x.ConfId == StaConfId.KohiHokenNoFrom)?.Val ?? string.Empty;
        result.KohiHokenNoFrom = kohiHokenNoFrom.AsInteger();
        string kohiHokenEdaNoFrom = staConfList.FirstOrDefault(x => x.ConfId == StaConfId.KohiHokenEdaNoFrom)?.Val ?? string.Empty;
        result.KohiHokenEdaNoFrom = kohiHokenEdaNoFrom.AsInteger();
        string kohiHokenNoTo = staConfList.FirstOrDefault(x => x.ConfId == StaConfId.KohiHokenNoTo)?.Val ?? string.Empty;
        result.KohiHokenNoTo = kohiHokenNoTo.AsInteger();
        string kohiHokenEdaNoTo = staConfList.FirstOrDefault(x => x.ConfId == StaConfId.KohiHokenEdaNoTo)?.Val ?? string.Empty;
        result.KohiHokenEdaNoTo = kohiHokenEdaNoTo.AsInteger();
        string startDateFrom = staConfList.FirstOrDefault(x => x.ConfId == StaConfId.StartDateFrom)?.Val ?? string.Empty;
        result.StartDateFrom = startDateFrom.AsInteger();
        string startDateTo = staConfList.FirstOrDefault(x => x.ConfId == StaConfId.StartDateTo)?.Val ?? string.Empty;
        result.StartDateTo = startDateTo.AsInteger();
        string tenkiDateFrom = staConfList.FirstOrDefault(x => x.ConfId == StaConfId.TenkiDateFrom)?.Val ?? string.Empty;
        result.TenkiDateFrom = tenkiDateFrom.AsInteger();
        string tenkiDateTo = staConfList.FirstOrDefault(x => x.ConfId == StaConfId.TenkiDateTo)?.Val ?? string.Empty;
        result.TenkiDateTo = tenkiDateTo.AsInteger();
        string tenkiKbnStr = staConfList.FirstOrDefault(x => x.ConfId == StaConfId.TenkiKbn)?.Val ?? string.Empty;
        result.TenkiKbns = string.IsNullOrEmpty(tenkiKbnStr) ? new List<int>() : tenkiKbnStr.Split(',').Select(x => x.AsInteger()).ToList();
        string sikkanKbnStr = staConfList.FirstOrDefault(x => x.ConfId == StaConfId.SikkanKbn)?.Val ?? string.Empty;
        result.SikkanKbns = string.IsNullOrEmpty(sikkanKbnStr) ? new List<int>() : sikkanKbnStr.Split(',').Select(x => x.AsInteger()).ToList();
        string nanbyoCdStr = staConfList.FirstOrDefault(x => x.ConfId == StaConfId.NanbyoCds)?.Val ?? string.Empty;
        result.NanbyoCds = string.IsNullOrEmpty(nanbyoCdStr) ? new List<int>() : nanbyoCdStr.Split(',').Select(x => x.AsInteger()).ToList();
        string isDoubt = staConfList.FirstOrDefault(x => x.ConfId == StaConfId.IsDoubt)?.Val ?? string.Empty;
        result.IsDoubt = isDoubt.AsInteger();
        string syubyo = staConfList.FirstOrDefault(x => x.ConfId == StaConfId.Syubyo && x.MenuId == menuId)?.Val ?? string.Empty;
        result.Syubyo = syubyo.AsInteger();
        string searchWord = staConfList.FirstOrDefault(x => x.ConfId == StaConfId.SearchWord)?.Val ?? string.Empty;
        result.SearchWord = searchWord.AsString();
        string searchWordMode = staConfList.FirstOrDefault(x => x.ConfId == StaConfId.SearchWordMode)?.Val ?? string.Empty;
        result.SearchWordMode = searchWordMode.AsInteger();
        string byomeiCdStr = staConfList.FirstOrDefault(x => x.ConfId == StaConfId.ByomeiCd)?.Val ?? string.Empty;
        result.ByomeiCds = string.IsNullOrEmpty(byomeiCdStr) ? new List<string>() : byomeiCdStr.Split(',').ToList();
        string byomeiCdOpt = staConfList.FirstOrDefault(x => x.ConfId == StaConfId.ByomeiCdOpt)?.Val ?? string.Empty;
        result.ByomeiCdOpt = byomeiCdOpt.AsInteger();
        string freeByomeiStr = staConfList.FirstOrDefault(x => x.ConfId == StaConfId.FreeByomei)?.Val ?? string.Empty;
        result.FreeByomeis = string.IsNullOrEmpty(freeByomeiStr) ? new List<string>() : freeByomeiStr.Split(',').ToList();
        string sindateFrom = staConfList.FirstOrDefault(x => x.ConfId == StaConfId.SindateFrom)?.Val ?? string.Empty;
        result.SindateFrom = sindateFrom.AsInteger();
        string sindateTo = staConfList.FirstOrDefault(x => x.ConfId == StaConfId.SindateTo)?.Val ?? string.Empty;
        result.SindateTo = sindateTo.AsInteger();
        string lastVisitDateFrom = staConfList.FirstOrDefault(x => x.ConfId == StaConfId.LastVisitDateFrom)?.Val ?? string.Empty;
        result.LastVisitDateFrom = lastVisitDateFrom.AsInteger();
        string lastVisitDateTo = staConfList.FirstOrDefault(x => x.ConfId == StaConfId.LastVisitDateTo)?.Val ?? string.Empty;
        result.LastVisitDateTo = lastVisitDateTo.AsInteger();
        string statuses = staConfList.FirstOrDefault(x => x.ConfId == StaConfId.Status)?.Val ?? string.Empty;
        result.Statuses = string.IsNullOrEmpty(statuses) ? new List<int>() : statuses.Split(',').Select(x => x.AsInteger()).ToList();
        string uketukeSbts = staConfList.FirstOrDefault(x => x.ConfId == StaConfId.UketukeSbtId)?.Val ?? string.Empty;
        result.UketukeSbtId = string.IsNullOrEmpty(uketukeSbts) ? new List<int>() : uketukeSbts.Split(',').Select(x => x.AsInteger()).ToList();
        string kaMsts = staConfList.FirstOrDefault(x => x.ConfId == StaConfId.KaMstId)?.Val ?? string.Empty;
        result.KaMstId = string.IsNullOrEmpty(kaMsts) ? new List<int>() : kaMsts.Split(',').Select(x => x.AsInteger()).ToList();
        string userMsts = staConfList.FirstOrDefault(x => x.ConfId == StaConfId.UserMstId)?.Val ?? string.Empty;
        result.UserMstId = string.IsNullOrEmpty(userMsts) ? new List<int>() : userMsts.Split(',').Select(x => x.AsInteger()).ToList();
        string isSinkan = staConfList.FirstOrDefault(x => x.ConfId == StaConfId.IsSinkan)?.Val ?? string.Empty;
        result.IsSinkan = isSinkan.AsInteger();
        string raiinAgeFrom = staConfList.FirstOrDefault(x => x.ConfId == StaConfId.RaiinAgeFrom)?.Val ?? string.Empty;
        result.RaiinAgeFrom = raiinAgeFrom;
        string raiinAgeTo = staConfList.FirstOrDefault(x => x.ConfId == StaConfId.RaiinAgeTo)?.Val ?? string.Empty;
        result.RaiinAgeTo = raiinAgeTo;
        string jikbanKbns = staConfList.FirstOrDefault(x => x.ConfId == StaConfId.JikanKbn)?.Val ?? string.Empty;
        result.JikanKbns = string.IsNullOrEmpty(jikbanKbns) ? new List<int>() : jikbanKbns.Split(',').Select(x => x.AsInteger()).ToList();
        string dataKind = staConfList.FirstOrDefault(x => x.ConfId == StaConfId.DataKind)?.Val ?? string.Empty;
        result.DataKind = dataKind.AsInteger();
        string itemCds = staConfList.FirstOrDefault(x => x.ConfId == StaConfId.ItemCds)?.Val ?? string.Empty;
        result.ItemCds = string.IsNullOrEmpty(itemCds) ? new List<string>() : itemCds.Split(',').ToList();
        string itemCdOpt = staConfList.FirstOrDefault(x => x.ConfId == StaConfId.ItemCdOpt)?.Val ?? string.Empty;
        result.ItemCdOpt = itemCdOpt.AsInteger();
        string itemCmtStr = staConfList.FirstOrDefault(x => x.ConfId == StaConfId.ItemCmt)?.Val ?? string.Empty;
        result.ItemCmts = string.IsNullOrEmpty(itemCmtStr) ? new List<string>() : itemCmtStr.Split(',').ToList();
        string medicalSearchWord = staConfList.FirstOrDefault(x => x.ConfId == StaConfId.MedicalSearchWord)?.Val ?? string.Empty;
        result.MedicalSearchWord = medicalSearchWord;
        string wordOpt = staConfList.FirstOrDefault(x => x.ConfId == StaConfId.WordOpt)?.Val ?? string.Empty;
        result.WordOpt = wordOpt.AsInteger();
        string kartekbn = staConfList.FirstOrDefault(x => x.ConfId == StaConfId.KarteKbns)?.Val ?? string.Empty;
        result.KarteKbns = string.IsNullOrEmpty(kartekbn) ? new List<int>() : kartekbn.Split(',').Select(x => x.AsInteger()).ToList();
        string karteSearchWords = staConfList.FirstOrDefault(x => x.ConfId == StaConfId.KarteSearchWords)?.Val ?? string.Empty;
        result.KarteSearchWords = string.IsNullOrEmpty(karteSearchWords) ? new List<string>() : karteSearchWords.Split(',').ToList();
        string karteWordOpt = staConfList.FirstOrDefault(x => x.ConfId == StaConfId.KarteWordOpt)?.Val ?? string.Empty;
        result.KarteWordOpt = karteWordOpt.AsInteger();
        string startIraiDate = staConfList.FirstOrDefault(x => x.ConfId == StaConfId.StartIraiDate)?.Val ?? string.Empty;
        result.StartIraiDate = startIraiDate.AsInteger();
        string endIraiDate = staConfList.FirstOrDefault(x => x.ConfId == StaConfId.EndIraiDate)?.Val ?? string.Empty;
        result.EndIraiDate = endIraiDate.AsInteger();
        string kensaItemCds = staConfList.FirstOrDefault(x => x.ConfId == StaConfId.KensaItemCds)?.Val ?? string.Empty;
        result.KensaItemCds = string.IsNullOrEmpty(kensaItemCds) ? new List<string>() : kensaItemCds.Split(',').ToList();
        string kensaItemCdOpt = staConfList.FirstOrDefault(x => x.ConfId == StaConfId.KensaItemCdOpt)?.Val ?? string.Empty;
        result.KensaItemCdOpt = kensaItemCdOpt.AsInteger();
        return result;
    }
}
