using Entity.Tenant;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace PostgreDataContext
{
    public class TenantDataContext : DbContext
    {
        public TenantDataContext(DbContextOptions options)
        : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AccountingFormMst>().HasKey(a => new { a.HpId, a.FormNo });
            modelBuilder.Entity<AuditTrailLog>().HasKey(a => new { a.LogId });
            modelBuilder.Entity<AuditTrailLogDetail>().HasKey(a => new { a.LogId });
            modelBuilder.Entity<ApprovalInf>().HasKey(a => new { a.Id, a.HpId, a.RaiinNo });
            modelBuilder.Entity<AutoSanteiMst>().HasKey(a => new { a.Id, a.HpId, a.ItemCd });
            modelBuilder.Entity<BuiOdrByomeiMst>().HasKey(b => new { b.HpId, b.BuiId, b.ByomeiBui });
            modelBuilder.Entity<BuiOdrItemByomeiMst>().HasKey(b => new { b.HpId, b.ItemCd, b.ByomeiBui });
            modelBuilder.Entity<BuiOdrItemMst>().HasKey(b => new { b.HpId, b.ItemCd });
            modelBuilder.Entity<BuiOdrMst>().HasKey(b => new { b.HpId, b.BuiId });
            modelBuilder.Entity<ByomeiMst>().HasKey(b => new { b.HpId, b.ByomeiCd });
            modelBuilder.Entity<ByomeiMstAftercare>().HasKey(b => new { b.ByomeiCd, b.Byomei, b.StartDate });
            modelBuilder.Entity<ByomeiSetGenerationMst>().HasKey(b => new { b.HpId, b.GenerationId });
            modelBuilder.Entity<ByomeiSetMst>().HasKey(b => new { b.HpId, b.GenerationId, b.SeqNo });
            modelBuilder.Entity<CalcLog>().HasKey(b => new { b.HpId, b.PtId, b.RaiinNo, b.SeqNo });
            modelBuilder.Entity<CmtCheckMst>().HasKey(b => new { b.HpId, b.ItemCd, b.SeqNo });
            modelBuilder.Entity<ContainerMst>().HasKey(b => new { b.HpId, b.ContainerCd });
            modelBuilder.Entity<ConversionItemInf>().HasKey(b => new { b.HpId, b.SourceItemCd, b.DestItemCd });
            modelBuilder.Entity<DefHokenNo>().HasKey(b => new { b.HpId, b.Digit1, b.Digit2, b.SeqNo });
            modelBuilder.Entity<DensiHaihanCustom>().HasKey(b => new { b.Id, b.HpId, b.ItemCd1, b.SeqNo, b.UserSetting });
            modelBuilder.Entity<DensiHaihanDay>().HasKey(b => new { b.Id, b.HpId, b.ItemCd1, b.SeqNo, b.UserSetting });
            modelBuilder.Entity<DensiHaihanKarte>().HasKey(b => new { b.Id, b.HpId, b.ItemCd1, b.SeqNo, b.UserSetting });
            modelBuilder.Entity<DensiHaihanMonth>().HasKey(b => new { b.Id, b.HpId, b.ItemCd1, b.SeqNo, b.UserSetting });
            modelBuilder.Entity<DensiHaihanWeek>().HasKey(b => new { b.Id, b.HpId, b.ItemCd1, b.SeqNo, b.UserSetting });
            modelBuilder.Entity<DensiHojyo>().HasKey(b => new { b.HpId, b.ItemCd, b.StartDate });
            modelBuilder.Entity<DensiHoukatu>().HasKey(b => new { b.StartDate, b.HpId, b.ItemCd, b.SeqNo, b.UserSetting });
            modelBuilder.Entity<DensiHoukatuGrp>().HasKey(b => new { b.HpId, b.HoukatuGrpNo, b.ItemCd, b.SeqNo, b.UserSetting, b.StartDate });
            modelBuilder.Entity<DensiSanteiKaisu>().HasKey(e => new { e.HpId, e.Id, e.ItemCd, e.SeqNo, e.UserSetting });
            modelBuilder.Entity<DocCategoryMst>().HasKey(e => new { e.HpId, e.CategoryCd });
            modelBuilder.Entity<DocCommentDetail>().HasKey(e => new { e.CategoryId, e.EdaNo });
            modelBuilder.Entity<DocInf>().HasKey(e => new { e.HpId, e.PtId, e.SinDate, e.RaiinNo, e.SeqNo });
            modelBuilder.Entity<DosageDosage>().HasKey(e => new { e.DoeiCd, e.DoeiSeqNo });
            modelBuilder.Entity<DosageDrug>().HasKey(e => new { e.DoeiCd, e.YjCd });
            modelBuilder.Entity<DosageMst>().HasKey(e => new { e.Id, e.HpId, e.ItemCd, e.SeqNo });
            modelBuilder.Entity<DrugDayLimit>().HasKey(e => new { e.Id, e.HpId, e.ItemCd, e.SeqNo });
            modelBuilder.Entity<DrugInf>().HasKey(e => new { e.InfKbn, e.HpId, e.ItemCd, e.SeqNo });
            modelBuilder.Entity<DrugUnitConv>().HasKey(e => new { e.ItemCd, e.StartDate });
            modelBuilder.Entity<ExceptHokensya>().HasKey(e => new { e.Id, e.HpId, e.PrefNo, e.HokenNo, e.HokenEdaNo, e.StartDate });
            modelBuilder.Entity<FilingAutoImp>().HasKey(e => new { e.SeqNo, e.HpId });
            modelBuilder.Entity<FilingCategoryMst>().HasKey(e => new { e.CategoryCd, e.HpId });
            modelBuilder.Entity<FilingInf>().HasKey(e => new { e.HpId, e.FileId, e.PtId, e.GetDate, e.FileNo });
            modelBuilder.Entity<GcStdMst>().HasKey(e => new { e.HpId, e.StdKbn, e.Sex, e.Point });
            modelBuilder.Entity<PtByomei>().HasKey(table => new { table.HpId, table.PtId, table.Id });
            modelBuilder.Entity<OdrInf>().HasKey(o => new { o.HpId, o.RaiinNo, o.RpNo, o.RpEdaNo, o.Id });
            modelBuilder.Entity<OdrInfDetail>().HasKey(o => new { o.HpId, o.RaiinNo, o.RpNo, o.RpEdaNo, o.RowNo });
            modelBuilder.Entity<RaiinKbnMst>().HasKey(r => new { r.HpId, r.GrpCd });
            modelBuilder.Entity<RaiinKbnInf>().HasKey(r => new { r.HpId, r.PtId, r.RaiinNo, r.GrpId, r.SeqNo });
            modelBuilder.Entity<RaiinKbnDetail>().HasKey(r => new { r.HpId, r.GrpCd, r.KbnCd });
            modelBuilder.Entity<SetMst>().HasKey(o => new { o.HpId, o.SetCd });
            modelBuilder.Entity<SetKbnMst>().HasKey(o => new { o.HpId, o.SetKbn, o.SetKbnEdaNo, o.GenerationId });
            modelBuilder.Entity<SetGenerationMst>().HasKey(o => new { o.HpId, o.GenerationId });
            modelBuilder.Entity<PtHokenPattern>().HasKey(r => new { r.HpId, r.PtId, r.SeqNo, r.HokenPid });
            modelBuilder.Entity<RaiinInf>().HasKey(r => new { r.HpId, r.RaiinNo });
            modelBuilder.Entity<RaiinCmtInf>().HasKey(e => new { e.HpId, e.RaiinNo, e.CmtKbn, e.SeqNo });
            modelBuilder.Entity<PtInf>().HasKey(r => new { r.HpId, r.PtId, r.SeqNo });
            modelBuilder.Entity<PtCmtInf>().HasKey(e => new { e.Id, e.HpId, e.PtId, e.SeqNo });
            modelBuilder.Entity<RsvInf>().HasKey(e => new { e.HpId, e.RsvFrameId, e.SinDate, e.StartTime, e.RaiinNo });
            modelBuilder.Entity<RsvFrameMst>().HasKey(e => new { e.HpId, e.RsvFrameId });
            modelBuilder.Entity<UserMst>().HasKey(e => new { e.Id, e.HpId });
            modelBuilder.Entity<JobMst>().HasKey(e => new { e.JobCd, e.HpId });
            modelBuilder.Entity<KaMst>().HasKey(e => new { e.Id, e.HpId });
            modelBuilder.Entity<LockInf>().HasKey(e => new { e.HpId, e.PtId, e.FunctionCd, e.SinDate, e.RaiinNo, e.OyaRaiinNo });
            modelBuilder.Entity<UketukeSbtMst>().HasKey(e => new { e.HpId, e.KbnId });
            modelBuilder.Entity<UketukeSbtDayInf>().HasKey(e => new { e.HpId, e.SinDate, e.SeqNo });
            modelBuilder.Entity<PtGrpNameMst>().HasKey(r => new { r.HpId, r.GrpId });
            modelBuilder.Entity<PtGrpItem>().HasKey(r => new { r.HpId, r.GrpId, r.GrpCode, r.SeqNo });
            modelBuilder.Entity<PtHokenInf>().HasKey(r => new { r.HpId, r.PtId, r.HokenId, r.SeqNo });
            modelBuilder.Entity<PtHokenCheck>().HasKey(r => new { r.HpId, r.PtID, r.HokenGrp, r.HokenId, r.SeqNo });
            modelBuilder.Entity<PtKohi>().HasKey(r => new { r.HpId, r.PtId, r.HokenId, r.SeqNo });
            modelBuilder.Entity<KarteKbnMst>().HasKey(o => new { o.HpId, o.KarteKbn });
            modelBuilder.Entity<PtGrpInf>().HasKey(r => new { r.HpId, r.GroupId, r.GroupCode, r.SeqNo });
            modelBuilder.Entity<PtSanteiConf>().HasKey(r => new { r.HpId, r.PtId, r.SeqNo });
            modelBuilder.Entity<KensaInfDetail>().HasKey(r => new { r.HpId, r.PtId, r.IraiCd, r.SeqNo });
            modelBuilder.Entity<KensaMst>().HasKey(r => new { r.HpId, r.KensaItemCd, r.KensaItemSeqNo });
            modelBuilder.Entity<PtAlrgyDrug>().HasKey(r => new { r.HpId, r.PtId, r.SeqNo });
            modelBuilder.Entity<PtAlrgyElse>().HasKey(r => new { r.HpId, r.PtId, r.SeqNo });
            modelBuilder.Entity<PtAlrgyFood>().HasKey(r => new { r.HpId, r.PtId, r.SeqNo });
            modelBuilder.Entity<PtInfection>().HasKey(r => new { r.HpId, r.PtId, r.SeqNo });
            modelBuilder.Entity<PtKioReki>().HasKey(r => new { r.HpId, r.PtId, r.SeqNo });
            modelBuilder.Entity<PtOtcDrug>().HasKey(r => new { r.HpId, r.PtId, r.SeqNo });
            modelBuilder.Entity<PtOtherDrug>().HasKey(r => new { r.HpId, r.PtId, r.SeqNo });
            modelBuilder.Entity<PtPregnancy>().HasKey(r => new { r.Id, r.HpId, r.PtId, r.SeqNo });
            modelBuilder.Entity<PtPregnancy>().Property(r => r.HpId).HasColumnOrder(2);
            modelBuilder.Entity<PtSupple>().HasKey(r => new { r.HpId, r.PtId, r.SeqNo });
            modelBuilder.Entity<RsvFrameMst>().HasKey(r => new { r.HpId, r.RsvFrameId });
            modelBuilder.Entity<RsvGrpMst>().HasKey(r => new { r.HpId, r.RsvGrpId });
            modelBuilder.Entity<RsvInf>().HasKey(r => new { r.HpId, r.RsvFrameId, r.SinDate, r.StartTime, r.RaiinNo });
            modelBuilder.Entity<TenMst>().HasKey(r => new { r.HpId, r.ItemCd, r.StartDate });
            modelBuilder.Entity<RaiinListCmt>().HasKey(r => new { r.HpId, r.RaiinNo, r.CmtKbn });
            modelBuilder.Entity<RaiinListTag>().HasKey(r => new { r.HpId, r.RaiinNo, r.SeqNo });
            modelBuilder.Entity<RaiinListInf>().HasKey(r => new { r.HpId, r.PtId, r.SinDate, r.RaiinNo, r.GrpId, r.RaiinListKbn });
            modelBuilder.Entity<RaiinListDetail>().HasKey(r => new { r.HpId, r.GrpId, r.KbnCd });
            modelBuilder.Entity<RaiinInf>().HasKey(r => new { r.HpId, r.RaiinNo });
            modelBuilder.Entity<RaiinListMst>().HasKey(r => new { r.HpId, r.GrpId });
            modelBuilder.Entity<RoudouMst>().HasKey(r => new { r.RoudouCd });
            modelBuilder.Entity<KantokuMst>().HasKey(r => new { r.RoudouCd, r.KantokuCd });
            modelBuilder.Entity<HokenMst>().HasKey(r => new { r.HpId, r.PrefNo, r.HokenNo, r.HokenEdaNo, r.StartDate });
            modelBuilder.Entity<HokensyaMst>().HasKey(r => new { r.HpId, r.HokensyaNo });
            modelBuilder.Entity<UserConf>().HasKey(e => new { e.HpId, e.UserId, e.GrpCd, e.GrpItemCd, e.GrpItemEdaNo });
            modelBuilder.Entity<SystemConf>().HasKey(e => new { e.HpId, e.GrpCd, e.GrpEdaNo });
            modelBuilder.Entity<KarteFilterDetail>().HasKey(e => new { e.HpId, e.UserId, e.FilterId, e.FilterItemCd, e.FilterEdaNo });
            modelBuilder.Entity<KarteFilterMst>().HasKey(e => new { e.HpId, e.UserId, e.FilterId });
            modelBuilder.Entity<ColumnSetting>().HasKey(e => new { e.UserId, e.TableName, e.ColumnName });
            modelBuilder.Entity<JsonSetting>().HasKey(e => new { e.UserId, e.Key });
            modelBuilder.Entity<YakkaSyusaiMst>().HasKey(e => new { e.HpId, e.YakkaCd, e.ItemCd, e.StartDate });
            modelBuilder.Entity<SetOdrInf>().HasKey(e => new { e.HpId, e.SetCd, e.RpNo, e.RpEdaNo, e.Id });
            modelBuilder.Entity<SetOdrInfDetail>().HasKey(e => new { e.HpId, e.SetCd, e.RpNo, e.RpEdaNo, e.RowNo });
            modelBuilder.Entity<SetKarteInf>().HasKey(e => new { e.HpId, e.SetCd, e.KarteKbn, e.SeqNo });
            modelBuilder.Entity<SetByomei>().HasKey(e => new { e.Id, e.HpId, e.SetCd, e.SeqNo });
            modelBuilder.Entity<PiProductInf>().HasKey(e => new { e.PiIdFull, e.PiId, e.Branch, e.Jpn });
            modelBuilder.Entity<M28DrugMst>().HasKey(e => new { e.YjCd });
            modelBuilder.Entity<M34DrugInfoMain>().HasKey(e => new { e.YjCd });
            modelBuilder.Entity<PathConf>().HasKey(e => new { e.HpId, e.GrpCd, e.GrpEdaNo, e.SeqNo });
            modelBuilder.Entity<PiInfDetail>().HasKey(e => new { e.PiId, e.Branch, e.Jpn, e.SeqNo });
            modelBuilder.Entity<M34FormCode>().HasKey(e => new { e.FormCd });
            modelBuilder.Entity<M34IndicationCode>().HasKey(e => new { e.KonoCd });
            modelBuilder.Entity<M34ArCode>().HasKey(e => new { e.FukusayoCd });
            modelBuilder.Entity<SystemGenerationConf>().HasKey(e => new { e.HpId, e.GrpEdaNo, e.GrpCd, e.Id });
            modelBuilder.Entity<IpnNameMst>().HasKey(e => new { e.HpId, e.IpnNameCd, e.StartDate, e.SeqNo });
            modelBuilder.Entity<IpnMinYakkaMst>().HasKey(e => new { e.HpId, e.Id, e.IpnNameCd, e.SeqNo });
            modelBuilder.Entity<IpnKasanExclude>().HasKey(e => new { e.HpId, e.StartDate, e.IpnNameCd, e.SeqNo });
            modelBuilder.Entity<IpnKasanExclude>().HasKey(e => new { e.HpId, e.StartDate, e.IpnNameCd, e.SeqNo });
            modelBuilder.Entity<IpnKasanExcludeItem>().HasKey(e => new { e.HpId, e.StartDate, e.ItemCd });
            modelBuilder.Entity<KouiKbnMst>().HasKey(e => new { e.HpId, e.KouiKbnId });
            modelBuilder.Entity<RaiinListKoui>().HasKey(e => new { e.HpId, e.KbnCd, e.SeqNo, e.GrpId });
            modelBuilder.Entity<RaiinListItem>().HasKey(e => new { e.HpId, e.KbnCd, e.SeqNo, e.GrpId });
            modelBuilder.Entity<KarteInf>().HasKey(e => new { e.HpId, e.RaiinNo, e.SeqNo, e.KarteKbn });
            modelBuilder.Entity<MonshinInfo>().HasKey(r => new { r.HpId, r.PtId, r.RaiinNo, r.SeqNo });
            modelBuilder.Entity<RsvkrtOdrInf>().HasKey(e => new { e.HpId, e.PtId, e.RsvkrtNo, e.RpNo, e.RpEdaNo, e.Id });
            modelBuilder.Entity<RsvkrtOdrInfDetail>().HasKey(e => new { e.HpId, e.PtId, e.RsvkrtNo, e.RpNo, e.RpEdaNo, e.RowNo });
            modelBuilder.Entity<PtTag>().HasKey(e => new { e.HpId, e.PtId, e.SeqNo });
            modelBuilder.Entity<PtKyusei>().HasKey(e => new { e.HpId, e.PtId, e.SeqNo });
            modelBuilder.Entity<RaiinKbnKoui>().HasKey(e => new { e.HpId, e.GrpId, e.KbnCd, e.SeqNo });
            modelBuilder.Entity<RaiinKbItem>().HasKey(e => new { e.HpId, e.GrpCd, e.KbnCd, e.SeqNo });
            modelBuilder.Entity<RaiinKbnMst>().HasKey(e => new { e.HpId, e.GrpCd });
            modelBuilder.Entity<RaiinKbnYayoku>().HasKey(e => new { e.HpId, e.GrpId, e.KbnCd, e.SeqNo });
            modelBuilder.Entity<RaiinKbnDetail>().HasKey(e => new { e.HpId, e.GrpCd, e.KbnCd });
            modelBuilder.Entity<ItemGrpMst>().HasKey(e => new { e.HpId, e.GrpSbt, e.ItemGrpCd, e.SeqNo, e.StartDate });
            modelBuilder.Entity<SinRpInf>().HasKey(e => new { e.HpId, e.PtId, e.SinYm, e.RpNo });
            modelBuilder.Entity<SinKouiCount>().HasKey(e => new { e.HpId, e.PtId, e.SinYm, e.SinDay, e.RaiinNo, e.RpNo, e.SeqNo });
            modelBuilder.Entity<SinKouiDetail>().HasKey(e => new { e.HpId, e.PtId, e.SinYm, e.RpNo, e.SeqNo, e.RowNo });
            modelBuilder.Entity<HolidayMst>().HasKey(e => new { e.HpId, e.SinDate, e.SeqNo });
            modelBuilder.Entity<SyunoNyukin>().HasKey(e => new { e.HpId, e.RaiinNo, e.SeqNo });
            modelBuilder.Entity<SyunoSeikyu>().HasKey(e => new { e.HpId, e.RaiinNo, e.PtId, e.SinDate });
            modelBuilder.Entity<PtRousaiTenki>().HasKey(e => new { e.HpId, e.PtId, e.HokenId, e.SeqNo });
            modelBuilder.Entity<TimeZoneDayInf>().HasKey(e => new { e.HpId, e.Id, e.SinDate });
            modelBuilder.Entity<TimeZoneConf>().HasKey(e => new { e.HpId, e.YoubiKbn, e.SeqNo });
            modelBuilder.Entity<TekiouByomeiMst>().HasKey(e => new { e.HpId, e.ItemCd, e.ByomeiCd, e.SystemData });
            modelBuilder.Entity<PtInf>().HasKey(e => new { e.HpId, e.PtId, e.SeqNo });
            modelBuilder.Entity<RsvkrtByomei>().HasKey(o => new { o.HpId, o.PtId, o.RsvkrtNo, o.SeqNo, o.Id });
            modelBuilder.Entity<RsvkrtKarteInf>().HasKey(e => new { e.HpId, e.PtId, e.RsvkrtNo, e.KarteKbn, e.SeqNo });
            modelBuilder.Entity<OnlineConsent>().HasKey(e => new { e.PtId, e.ConsKbn });
            modelBuilder.Entity<KouiHoukatuMst>().HasKey(e => new { e.HpId, e.ItemCd, e.StartDate });
            modelBuilder.Entity<JihiSbtMst>().HasKey(e => new { e.HpId, e.JihiSbt });
            modelBuilder.Entity<KaikeiDetail>().HasKey(e => new { e.HpId, e.PtId, e.SinDate, e.RaiinNo, e.HokenPid, e.AdjustPid });
            modelBuilder.Entity<KaikeiInf>().HasKey(e => new { e.HpId, e.PtId, e.SinDate, e.RaiinNo, e.HokenId });
            modelBuilder.Entity<KensaCenterMst>().HasKey(e => new { e.HpId, e.Id });
            modelBuilder.Entity<KensaInf>().HasKey(e => new { e.HpId, e.PtId, e.IraiCd });
            modelBuilder.Entity<KensaIraiLog>().HasKey(e => new { e.HpId, e.CenterCd, e.CreateDate });
            modelBuilder.Entity<KensaStdMst>().HasKey(e => new { e.HpId, e.KensaItemCd, e.StartDate });
            modelBuilder.Entity<KinkiMst>().HasKey(e => new { e.HpId, e.Id, e.ACd, e.SeqNo });
            modelBuilder.Entity<KogakuLimit>().HasKey(e => new { e.HpId, e.AgeKbn, e.KogakuKbn, e.StartDate });
            modelBuilder.Entity<KohiPriority>().HasKey(e => new { e.PrefNo, e.Houbetu, e.PriorityNo });
            modelBuilder.Entity<LimitCntListInf>().HasKey(e => new { e.HpId, e.PtId, e.KohiId, e.SinDate, e.SeqNo });
            modelBuilder.Entity<ListSetGenerationMst>().HasKey(e => new { e.HpId, e.GenerationId });
            modelBuilder.Entity<ListSetMst>().HasKey(e => new { e.HpId, e.GenerationId, e.SetId });
            modelBuilder.Entity<LockMst>().HasKey(e => new { e.FunctionCdA, e.FunctionCdB });
            modelBuilder.Entity<M01Kinki>().HasKey(e => new { e.ACd, e.BCd, e.CmtCd, e.SayokijyoCd });
            modelBuilder.Entity<M10DayLimit>().HasKey(e => new { e.YjCd, e.SeqNo });
            modelBuilder.Entity<M12FoodAlrgy>().HasKey(e => new { e.YjCd, e.FoodKbn, e.TenpuLevel });
            modelBuilder.Entity<M14AgeCheck>().HasKey(e => new { e.YjCd, e.AttentionCmtCd });
            modelBuilder.Entity<M34ArDiscon>().HasKey(e => new { e.YjCd, e.SeqNo });
            modelBuilder.Entity<M34InteractionPat>().HasKey(e => new { e.YjCd, e.SeqNo });
            modelBuilder.Entity<M34Precaution>().HasKey(e => new { e.YjCd, e.SeqNo });
            modelBuilder.Entity<M34PrecautionCode>().HasKey(e => new { e.PrecautionCd, e.ExtendCd });
            modelBuilder.Entity<M38Ingredients>().HasKey(e => new { e.SeibunCd, e.SerialNum, e.Sbt });
            modelBuilder.Entity<M41SuppleIndexcode>().HasKey(e => new { e.SeibunCd, e.IndexCd });
            modelBuilder.Entity<M42ContraindiDisBc>().HasKey(e => new { e.ByotaiCd, e.ByotaiClassCd });
            modelBuilder.Entity<M42ContraindiDrugMainEx>().HasKey(e => new { e.YjCd, e.TenpuLevel, e.ByotaiCd, e.CmtCd });
            modelBuilder.Entity<M56AlrgyDerivatives>().HasKey(e => new { e.SeibunCd, e.DrvalrgyCd, e.YjCd });
            modelBuilder.Entity<M56ExAnalogue>().HasKey(e => new { e.SeibunCd, e.SeqNo });
            modelBuilder.Entity<M56ExEdIngredients>().HasKey(e => new { e.YjCd, e.SeqNo });
            modelBuilder.Entity<M56ExIngCode>().HasKey(e => new { e.SeibunCd, e.SeibunIndexCd });
            modelBuilder.Entity<M56ProdrugCd>().HasKey(e => new { e.SeqNo, e.SeibunCd });
            modelBuilder.Entity<M56YjDrugClass>().HasKey(e => new { e.YjCd, e.ClassCd });
            modelBuilder.Entity<MallRenkeiInf>().HasKey(e => new { e.HpId, e.RaiinNo });
            modelBuilder.Entity<MaterialMst>().HasKey(e => new { e.HpId, e.MaterialCd });
            modelBuilder.Entity<OdrDateDetail>().HasKey(e => new { e.HpId, e.GrpId, e.SeqNo });
            modelBuilder.Entity<OdrDateInf>().HasKey(e => new { e.HpId, e.GrpId });
            modelBuilder.Entity<OdrInfCmt>().HasKey(e => new { e.HpId, e.RaiinNo, e.RpNo, e.RpEdaNo, e.RowNo, e.EdaNo });
            modelBuilder.Entity<PaymentMethodMst>().HasKey(e => new { e.HpId, e.PaymentMethodCd });
            modelBuilder.Entity<PermissionMst>().HasKey(e => new { e.FunctionCd, e.Permission });
            modelBuilder.Entity<PiImage>().HasKey(e => new { e.HpId, e.ImageType, e.ItemCd });
            modelBuilder.Entity<PriorityHaihanMst>().HasKey(e => new { e.HpId, e.HaihanGrp, e.StartDate, e.UserSetting });
            modelBuilder.Entity<PtGrpItem>().HasKey(e => new { e.HpId, e.GrpId, e.GrpCode, e.SeqNo });
            modelBuilder.Entity<PtHokenScan>().HasKey(e => new { e.HpId, e.PtId, e.HokenGrp, e.HokenId, e.SeqNo });
            modelBuilder.Entity<PtJibaiDoc>().HasKey(e => new { e.HpId, e.PtId, e.SinYm, e.HokenId, e.SeqNo });
            modelBuilder.Entity<PtJibkar>().HasKey(e => new { e.HpId, e.WebId });
            modelBuilder.Entity<PtLastVisitDate>().HasKey(e => new { e.HpId, e.PtId });
            modelBuilder.Entity<PtMemo>().HasKey(e => new { e.HpId, e.PtId, e.SeqNo });
            modelBuilder.Entity<PhysicalAverage>().HasKey(e => new { e.JissiYear, e.AgeYear, e.AgeMonth, e.AgeDay });
            modelBuilder.Entity<RaiinFilterKbn>().HasKey(e => new { e.HpId, e.FilterId, e.SeqNo });
            modelBuilder.Entity<RaiinFilterState>().HasKey(e => new { e.HpId, e.FilterId, e.SeqNo });
            modelBuilder.Entity<RaiinListDoc>().HasKey(e => new { e.HpId, e.GrpId, e.KbnCd, e.SeqNo });
            modelBuilder.Entity<RaiinListFile>().HasKey(e => new { e.HpId, e.GrpId, e.KbnCd, e.SeqNo });
            modelBuilder.Entity<ReceCmt>().HasKey(e => new { e.HpId, e.PtId, e.SinYm, e.HokenId, e.CmtKbn, e.CmtSbt, e.Id });
            modelBuilder.Entity<ReceCheckCmt>().HasKey(e => new { e.HpId, e.PtId, e.SinYm, e.HokenId, e.SeqNo });
            modelBuilder.Entity<ReceCheckErr>().HasKey(e => new { e.HpId, e.PtId, e.SinYm, e.HokenId, e.ErrCd, e.SinDate, e.ACd, e.BCd });
            modelBuilder.Entity<ReceCheckOpt>().HasKey(e => new { e.HpId, e.ErrCd });
            modelBuilder.Entity<RecedenCmtSelect>().HasKey(e => new { e.HpId, e.ItemNo, e.EdaNo, e.ItemCd, e.StartDate, e.CommentCd });
            modelBuilder.Entity<RecedenHenJiyuu>().HasKey(e => new { e.HpId, e.PtId, e.HokenId, e.SinYm, e.SeqNo });
            modelBuilder.Entity<RecedenRirekiInf>().HasKey(e => new { e.HpId, e.PtId, e.HokenId, e.SinYm, e.SeqNo });
            modelBuilder.Entity<ReceFutanKbn>().HasKey(e => new { e.HpId, e.SeikyuYm, e.PtId, e.SinYm, e.HokenId, e.HokenPid });
            modelBuilder.Entity<ReceInf>().HasKey(e => new { e.HpId, e.SeikyuYm, e.PtId, e.SinYm, e.HokenId });
            modelBuilder.Entity<ReceInfEdit>().HasKey(e => new { e.HpId, e.SeikyuYm, e.PtId, e.SinYm, e.HokenId, e.SeqNo });
            modelBuilder.Entity<ReceInfJd>().HasKey(e => new { e.HpId, e.SeikyuYm, e.PtId, e.SinYm, e.HokenId, e.KohiId });
            modelBuilder.Entity<ReceInfPreEdit>().HasKey(e => new { e.HpId, e.SeikyuYm, e.PtId, e.SinYm, e.HokenId });
            modelBuilder.Entity<ReceSeikyu>().HasKey(e => new { e.HpId, e.SinYm, e.SeqNo });
            modelBuilder.Entity<ReceStatus>().HasKey(e => new { e.HpId, e.PtId, e.SeikyuYm, e.HokenId, e.SinYm });
            modelBuilder.Entity<ReleasenoteRead>().HasKey(e => new { e.HpId, e.UserId, e.Version });
            modelBuilder.Entity<RenkeiConf>().HasKey(e => new { e.HpId, e.Id });
            modelBuilder.Entity<RenkeiMst>().HasKey(e => new { e.HpId, e.RenkeiId });
            modelBuilder.Entity<RenkeiPathConf>().HasKey(e => new { e.HpId, e.EdaNo, e.Id });
            modelBuilder.Entity<RenkeiTemplateMst>().HasKey(e => new { e.HpId, e.TemplateId });
            modelBuilder.Entity<RenkeiTimingConf>().HasKey(e => new { e.HpId, e.EventCd, e.Id });
            modelBuilder.Entity<RenkeiTimingMst>().HasKey(e => new { e.HpId, e.RenkeiId, e.EventCd });
            modelBuilder.Entity<RousaiGoseiMst>().HasKey(e => new { e.HpId, e.GoseiGrp, e.GoseiItemCd, e.ItemCd, e.SisiKbn, e.StartDate });
            modelBuilder.Entity<RsvDayComment>().HasKey(e => new { e.HpId, e.SinDate, e.SeqNo });
            modelBuilder.Entity<RsvFrameDayPtn>().HasKey(e => new { e.HpId, e.RsvFrameId, e.SinDate, e.SeqNo });
            modelBuilder.Entity<RsvFrameInf>().HasKey(e => new { e.HpId, e.RsvFrameId, e.SinDate, e.StartTime, e.Id });
            modelBuilder.Entity<RsvFrameWeekPtn>().HasKey(e => new { e.Id, e.HpId, e.RsvFrameId, e.Week, e.SeqNo });
            modelBuilder.Entity<RsvFrameWith>().HasKey(e => new { e.Id, e.HpId, e.RsvFrameId });
            modelBuilder.Entity<RsvkrtMst>().HasKey(e => new { e.HpId, e.PtId, e.RsvkrtNo });
            modelBuilder.Entity<RsvkrtOdrInfCmt>().HasKey(e => new { e.HpId, e.PtId, e.RsvkrtNo, e.RpEdaNo, e.RpNo, e.RowNo, e.EdaNo });
            modelBuilder.Entity<RsvRenkeiInf>().HasKey(e => new { e.HpId, e.RaiinNo });
            modelBuilder.Entity<RsvRenkeiInfTk>().HasKey(e => new { e.HpId, e.RaiinNo, e.SystemKbn });
            modelBuilder.Entity<SanteiAutoOrder>().HasKey(e => new { e.Id, e.HpId, e.SanteiGrpCd, e.SeqNo });
            modelBuilder.Entity<SanteiAutoOrderDetail>().HasKey(e => new { e.Id, e.HpId, e.SanteiGrpCd, e.SeqNo, e.ItemCd });
            modelBuilder.Entity<SanteiCntCheck>().HasKey(e => new { e.HpId, e.SanteiGrpCd, e.SeqNo });
            modelBuilder.Entity<SanteiGrpDetail>().HasKey(e => new { e.HpId, e.SanteiGrpCd, e.ItemCd });
            modelBuilder.Entity<SanteiGrpMst>().HasKey(e => new { e.HpId, e.SanteiGrpCd });
            modelBuilder.Entity<SchemaCmtMst>().HasKey(e => new { e.HpId, e.CommentCd });
            modelBuilder.Entity<SentenceList>().HasKey(e => new { e.HpId, e.Sentence });
            modelBuilder.Entity<SessionInf>().HasKey(e => new { e.HpId, e.Machine });
            modelBuilder.Entity<SetOdrInfCmt>().HasKey(e => new { e.HpId, e.SetCd, e.RpNo, e.RpEdaNo, e.RowNo, e.EdaNo });
            modelBuilder.Entity<SinKoui>().HasKey(e => new { e.HpId, e.PtId, e.SinYm, e.RpNo, e.SeqNo });
            modelBuilder.Entity<SinKouiCount>().HasKey(e => new { e.HpId, e.PtId, e.SinYm, e.SinDay, e.RaiinNo, e.RpNo, e.SeqNo });
            modelBuilder.Entity<SinrekiFilterMst>().HasKey(e => new { e.HpId, e.GrpCd });
            modelBuilder.Entity<SinrekiFilterMstDetail>().HasKey(e => new { e.HpId, e.GrpCd, e.Id });
            modelBuilder.Entity<SinRpNoInf>().HasKey(e => new { e.HpId, e.PtId, e.SinYm, e.SinDay, e.RaiinNo, e.RpNo });
            modelBuilder.Entity<SingleDoseMst>().HasKey(e => new { e.HpId, e.Id });
            modelBuilder.Entity<StaConf>().HasKey(e => new { e.HpId, e.MenuId, e.ConfId });
            modelBuilder.Entity<StaGrp>().HasKey(e => new { e.HpId, e.GrpId, e.ReportId });
            modelBuilder.Entity<StaMst>().HasKey(e => new { e.HpId, e.ReportId });
            modelBuilder.Entity<SyobyoKeika>().HasKey(e => new { e.HpId, e.PtId, e.SinYm, e.SinDay, e.HokenId, e.SeqNo });
            modelBuilder.Entity<SyoukiInf>().HasKey(e => new { e.HpId, e.PtId, e.SinYm, e.HokenId, e.SeqNo });
            modelBuilder.Entity<SyoukiKbnMst>().HasKey(e => new { e.SyoukiKbn, e.StartYm });
            modelBuilder.Entity<SystemConfItem>().HasKey(e => new { e.HpId, e.MenuId, e.SeqNo });
            modelBuilder.Entity<SystemConfMenu>().HasKey(e => new { e.HpId, e.MenuId });
            modelBuilder.Entity<TagGrpMst>().HasKey(e => new { e.HpId, e.TagGrpNo });
            modelBuilder.Entity<TekiouByomeiMstExcluded>().HasKey(e => new { e.HpId, e.ItemCd, e.SeqNo });
            modelBuilder.Entity<TemplateDetail>().HasKey(e => new { e.HpId, e.TemplateCd, e.SeqNo, e.ControlId });
            modelBuilder.Entity<TemplateDspConf>().HasKey(e => new { e.HpId, e.TemplateCd, e.SeqNo, e.DspKbn });
            modelBuilder.Entity<TemplateMenuDetail>().HasKey(e => new { e.HpId, e.MenuKbn, e.SeqNo });
            modelBuilder.Entity<TemplateMenuMst>().HasKey(e => new { e.HpId, e.MenuKbn, e.SeqNo });
            modelBuilder.Entity<TenMstMother>().HasKey(e => new { e.HpId, e.ItemCd, e.StartDate });
            modelBuilder.Entity<TimeZoneDayInf>().HasKey(e => new { e.HpId, e.Id, e.SinDate });
            modelBuilder.Entity<TodoGrpMst>().HasKey(e => new { e.HpId, e.TodoGrpNo });
            modelBuilder.Entity<TodoInf>().HasKey(e => new { e.HpId, e.TodoNo, e.TodoEdaNo, e.PtId });
            modelBuilder.Entity<TodoKbnMst>().HasKey(e => new { e.HpId, e.TodoKbnNo });
            modelBuilder.Entity<TokkiMst>().HasKey(e => new { e.HpId, e.TokkiCd });
            modelBuilder.Entity<UserPermission>().HasKey(e => new { e.HpId, e.UserId, e.FunctionCd });
            modelBuilder.Entity<WrkSinKoui>().HasKey(e => new { e.HpId, e.RaiinNo, e.HokenKbn, e.RpNo, e.SeqNo });
            modelBuilder.Entity<WrkSinKouiDetail>().HasKey(e => new { e.HpId, e.RaiinNo, e.HokenKbn, e.RpNo, e.SeqNo, e.RowNo });
            modelBuilder.Entity<WrkSinKouiDetailDel>().HasKey(e => new { e.HpId, e.RaiinNo, e.HokenKbn, e.RpNo, e.SeqNo, e.RowNo, e.ItemSeqNo });
            modelBuilder.Entity<WrkSinRpInf>().HasKey(e => new { e.HpId, e.RaiinNo, e.HokenKbn, e.RpNo });
            modelBuilder.Entity<YohoInfMst>().HasKey(e => new { e.HpId, e.ItemCd });
            modelBuilder.Entity<YoyakuOdrInf>().HasKey(e => new { e.HpId, e.PtId, e.YoyakuKarteNo, e.RpNo, e.RpEdaNo });
            modelBuilder.Entity<YoyakuOdrInfDetail>().HasKey(e => new { e.HpId, e.PtId, e.YoyakuKarteNo, e.RpNo, e.RpEdaNo, e.RowNo });
            modelBuilder.Entity<YoyakuSbtMst>().HasKey(e => new { e.HpId, e.YoyakuSbt });
            modelBuilder.Entity<BackupReq>().HasKey(e => new { e.Id });
            modelBuilder.Entity<CalcStatus>().HasKey(e => new { e.CalcId });
            modelBuilder.Entity<CmtKbnMst>().HasKey(e => new { e.Id });
            modelBuilder.Entity<DocComment>().HasKey(e => new { e.CategoryId });
            modelBuilder.Entity<EventMst>().HasKey(e => new { e.EventCd });
            modelBuilder.Entity<FunctionMst>().HasKey(e => new { e.FunctionCd });
            modelBuilder.Entity<KacodeMst>().HasKey(e => new { e.ReceKaCd });
            modelBuilder.Entity<KarteImgInf>().HasKey(e => new { e.Id });
            modelBuilder.Entity<LimitListInf>().HasKey(e => new { e.Id });
            modelBuilder.Entity<M01KijyoCmt>().HasKey(e => new { e.CmtCd });
            modelBuilder.Entity<M01KinkiCmt>().HasKey(e => new { e.CmtCd });
            modelBuilder.Entity<M12FoodAlrgyKbn>().HasKey(e => new { e.FoodKbn });
            modelBuilder.Entity<M14CmtCode>().HasKey(e => new { e.AttentionCmtCd });
            modelBuilder.Entity<M28DrugMst>().HasKey(e => new { e.YjCd });
            modelBuilder.Entity<M34ArCode>().HasKey(e => new { e.FukusayoCd });
            modelBuilder.Entity<M34ArDisconCode>().HasKey(e => new { e.FukusayoCd });
            modelBuilder.Entity<M34DrugInfoMain>().HasKey(e => new { e.YjCd });
            modelBuilder.Entity<M34FormCode>().HasKey(e => new { e.FormCd });
            modelBuilder.Entity<M34IndicationCode>().HasKey(e => new { e.KonoCd });
            modelBuilder.Entity<M34InteractionPatCode>().HasKey(e => new { e.InteractionPatCd });
            modelBuilder.Entity<M34PropertyCode>().HasKey(e => new { e.PropertyCd });
            modelBuilder.Entity<M34SarSymptomCode>().HasKey(e => new { e.FukusayoInitCd });
            modelBuilder.Entity<M38ClassCode>().HasKey(e => new { e.ClassCd });
            modelBuilder.Entity<M38IngCode>().HasKey(e => new { e.SeibunCd });
            modelBuilder.Entity<M38MajorDivCode>().HasKey(e => new { e.MajorDivCd });
            modelBuilder.Entity<M38OtcFormCode>().HasKey(e => new { e.FormCd });
            modelBuilder.Entity<M38OtcMain>().HasKey(e => new { e.SerialNum });
            modelBuilder.Entity<M38OtcMakerCode>().HasKey(e => new { e.MakerCd });
            modelBuilder.Entity<M41SuppleIndexdef>().HasKey(e => new { e.SeibunCd });
            modelBuilder.Entity<M41SuppleIngre>().HasKey(e => new { e.SeibunCd });
            modelBuilder.Entity<M42ContraCmt>().HasKey(e => new { e.CmtCd });
            modelBuilder.Entity<M42ContraindiDisClass>().HasKey(e => new { e.ByotaiClassCd });
            modelBuilder.Entity<M42ContraindiDisCon>().HasKey(e => new { e.ByotaiCd });
            modelBuilder.Entity<M56DrugClass>().HasKey(e => new { e.ClassCd });
            modelBuilder.Entity<M56DrvalrgyCode>().HasKey(e => new { e.DrvalrgyCd });
            modelBuilder.Entity<M56ExIngrdtMain>().HasKey(e => new { e.YjCd });
            modelBuilder.Entity<M56UsageCode>().HasKey(e => new { e.YohoCd });
            modelBuilder.Entity<MallMessageInf>().HasKey(e => new { e.Id });
            modelBuilder.Entity<OnlineConfirmation>().HasKey(e => new { e.ReceptionNo });
            modelBuilder.Entity<OnlineConfirmationHistory>().HasKey(e => new { e.ID });
            modelBuilder.Entity<PiInf>().HasKey(e => new { e.PiId });
            modelBuilder.Entity<PostCodeMst>().HasKey(e => new { e.Id });
            modelBuilder.Entity<PtFamilyReki>().HasKey(e => new { e.Id });
            modelBuilder.Entity<RaiinFilterMst>().HasKey(e => new { e.FilterId });
            modelBuilder.Entity<RaiinFilterSort>().HasKey(e => new { e.Id });
            modelBuilder.Entity<RenkeiReq>().HasKey(e => new { e.ReqId });
            modelBuilder.Entity<RoudouMst>().HasKey(e => new { e.RoudouCd });
            modelBuilder.Entity<RsvkrtKarteImgInf>().HasKey(e => new { e.Id });
            modelBuilder.Entity<SanteiInf>().HasKey(e => new { e.Id });
            modelBuilder.Entity<SanteiInfDetail>().HasKey(e => new { e.Id });
            modelBuilder.Entity<SeikaturekiInf>().HasKey(e => new { e.Id });
            modelBuilder.Entity<SetKarteImgInf>().HasKey(e => new { e.Id });
            modelBuilder.Entity<StaCsv>().HasKey(e => new { e.Id });
            modelBuilder.Entity<StaMenu>().HasKey(e => new { e.MenuId });
            modelBuilder.Entity<SummaryInf>().HasKey(e => new { e.Id });
            modelBuilder.Entity<SystemChangeLog>().HasKey(e => new { e.Id });
            modelBuilder.Entity<UnitMst>().HasKey(e => new { e.UnitCd });
            modelBuilder.Entity<YohoSetMst>().HasKey(e => new { e.SetId });
            modelBuilder.Entity<ZDocInf>().HasKey(e => new { e.OpId });
            modelBuilder.Entity<ZFilingInf>().HasKey(e => new { e.OpId });
            modelBuilder.Entity<ZKensaInf>().HasKey(e => new { e.OpId });
            modelBuilder.Entity<ZKensaInfDetail>().HasKey(e => new { e.OpId });
            modelBuilder.Entity<ZLimitCntListInf>().HasKey(e => new { e.OpId });
            modelBuilder.Entity<ZLimitListInf>().HasKey(e => new { e.OpId });
            modelBuilder.Entity<ZMonshinInf>().HasKey(e => new { e.OpId });
            modelBuilder.Entity<ZPtAlrgyDrug>().HasKey(e => new { e.OpId });
            modelBuilder.Entity<ZPtAlrgyElse>().HasKey(e => new { e.OpId });
            modelBuilder.Entity<ZPtAlrgyFood>().HasKey(e => new { e.OpId });
            modelBuilder.Entity<ZPtCmtInf>().HasKey(e => new { e.OpId });
            modelBuilder.Entity<ZPtFamily>().HasKey(e => new { e.OpId });
            modelBuilder.Entity<ZPtFamilyReki>().HasKey(e => new { e.OpId });
            modelBuilder.Entity<ZPtGrpInf>().HasKey(e => new { e.OpId });
            modelBuilder.Entity<ZPtHokenCheck>().HasKey(e => new { e.OpId });
            modelBuilder.Entity<ZPtHokenInf>().HasKey(e => new { e.OpId });
            modelBuilder.Entity<ZPtHokenPattern>().HasKey(e => new { e.OpId });
            modelBuilder.Entity<ZPtHokenScan>().HasKey(e => new { e.OpId });
            modelBuilder.Entity<ZPtInf>().HasKey(e => new { e.OpId });
            modelBuilder.Entity<ZPtInfection>().HasKey(e => new { e.OpId });
            modelBuilder.Entity<ZPtJibkar>().HasKey(e => new { e.OpId });
            modelBuilder.Entity<ZPtKioReki>().HasKey(e => new { e.OpId });
            modelBuilder.Entity<ZPtKohi>().HasKey(e => new { e.OpId });
            modelBuilder.Entity<ZPtKyusei>().HasKey(e => new { e.OpId });
            modelBuilder.Entity<ZPtMemo>().HasKey(e => new { e.OpId });
            modelBuilder.Entity<ZPtOtcDrug>().HasKey(e => new { e.OpId });
            modelBuilder.Entity<ZPtOtherDrug>().HasKey(e => new { e.OpId });
            modelBuilder.Entity<ZPtPregnancy>().HasKey(e => new { e.OpId });
            modelBuilder.Entity<ZPtRousaiTenki>().HasKey(e => new { e.OpId });
            modelBuilder.Entity<ZPtSanteiConf>().HasKey(e => new { e.OpId });
            modelBuilder.Entity<ZPtSupple>().HasKey(e => new { e.OpId });
            modelBuilder.Entity<ZPtTag>().HasKey(e => new { e.OpId });
            modelBuilder.Entity<ZRaiinCmtInf>().HasKey(e => new { e.OpId });
            modelBuilder.Entity<ZRaiinInf>().HasKey(e => new { e.OpId });
            modelBuilder.Entity<ZRaiinKbnInf>().HasKey(e => new { e.OpId });
            modelBuilder.Entity<ZRaiinListCmt>().HasKey(e => new { e.OpId });
            modelBuilder.Entity<ZRaiinListTag>().HasKey(e => new { e.OpId });
            modelBuilder.Entity<ZReceCmt>().HasKey(e => new { e.OpId });
            modelBuilder.Entity<ZReceCheckCmt>().HasKey(e => new { e.OpId });
            modelBuilder.Entity<ZReceInfEdit>().HasKey(e => new { e.OpId });
            modelBuilder.Entity<ZReceSeikyu>().HasKey(e => new { e.OpId });
            modelBuilder.Entity<ZRsvDayComment>().HasKey(e => new { e.OpId });
            modelBuilder.Entity<ZRsvInf>().HasKey(e => new { e.OpId });
            modelBuilder.Entity<ZSanteiInfDetail>().HasKey(e => new { e.OpId });
            modelBuilder.Entity<ZSeikaturekiInf>().HasKey(e => new { e.OpId });
            modelBuilder.Entity<ZSummaryInf>().HasKey(e => new { e.OpId });
            modelBuilder.Entity<ZSyobyoKeika>().HasKey(e => new { e.OpId });
            modelBuilder.Entity<ZSyoukiInf>().HasKey(e => new { e.OpId });
            modelBuilder.Entity<ZSyunoNyukin>().HasKey(e => new { e.OpId });
            modelBuilder.Entity<ZTodoInf>().HasKey(e => new { e.OpId });
            modelBuilder.Entity<ZUketukeSbtDayInf>().HasKey(e => new { e.OpId });
            modelBuilder.Entity<HpInf>().HasKey(h => new { h.HpId, h.StartDate });
            modelBuilder.Entity<IpnKasanMst>().HasKey(i => new { i.HpId, i.StartDate, i.IpnNameCd, i.SeqNo });
            modelBuilder.Entity<M56AnalogueCd>().HasKey(i => new { i.AnalogueCd });
            modelBuilder.Entity<PtFamily>().HasKey(p => new { p.PtId });
            modelBuilder.Entity<SokatuMst>().HasKey(s => new { s.HpId, s.PrefNo, s.StartYm, s.ReportEdaNo, s.ReportId });
            modelBuilder.Entity<TemplateMst>().HasKey(s => new { s.HpId, s.TemplateCd, s.SeqNo });
        }

        public DbSet<JsonSetting> JsonSettings { get; set; } = default!;

        public DbSet<ColumnSetting> ColumnSettings { get; set; } = default!;

        public DbSet<PtInf> PtInfs { get; set; } = default!;

        public DbSet<ZUketukeSbtDayInf> ZUketukeSbtDayInfs { get; set; } = default!;

        public DbSet<PtGrpNameMst> PtGrpNameMsts { get; set; } = default!;

        public DbSet<PtGrpItem> PtGrpItems { get; set; } = default!;

        public DbSet<PtSanteiConf> PtSanteiConfs { get; set; } = default!;

        public DbSet<PtHokenInf> PtHokenInfs { get; set; } = default!;

        public DbSet<PtKohi> PtKohis { get; set; } = default!;

        public DbSet<HokenMst> HokenMsts { get; set; } = default!;

        public DbSet<PtHokenPattern> PtHokenPatterns { get; set; } = default!;

        public DbSet<PostCodeMst> PostCodeMsts { get; set; } = default!;

        public DbSet<SystemConf> SystemConfs { get; set; } = default!;

        public DbSet<PtKyusei> PtKyuseis { get; set; } = default!;

        public DbSet<KantokuMst> KantokuMsts { get; set; } = default!;

        public DbSet<RoudouMst> RoudouMsts { get; set; } = default!;

        public DbSet<UserMst> UserMsts { get; set; } = default!;

        public DbSet<UketukeSbtMst> UketukeSbtMsts { get; set; } = default!;

        public DbSet<KaMst> KaMsts { get; set; } = default!;

        public DbSet<TokkiMst> TokkiMsts { get; set; } = default!;

        public DbSet<RaiinKbnMst> RaiinKbnMsts { get; set; } = default!;

        public DbSet<RaiinKbnDetail> RaiinKbnDetails { get; set; } = default!;

        public DbSet<RaiinInf> RaiinInfs { get; set; } = default!;

        public DbSet<RaiinCmtInf> RaiinCmtInfs { get; set; } = default!;

        public DbSet<PtGrpInf> PtGrpInfs { get; set; } = default!;

        public DbSet<PtMemo> PtMemos { get; set; } = default!;

        public DbSet<RaiinKbnInf> RaiinKbnInfs { get; set; } = default!;

        public DbSet<HpInf> HpInfs { get; set; } = default!;

        public DbSet<ByomeiMstAftercare> ByomeiMstAftercares { get; set; } = default!;

        public DbSet<HokensyaMst> HokensyaMsts { get; set; } = default!;

        public DbSet<TimeZoneConf> TimeZoneConfs { get; set; } = default!;

        public DbSet<TimeZoneDayInf> TimeZoneDayInfs { get; set; } = default!;

        public DbSet<RaiinKbItem> RaiinKbItems { get; set; } = default!;

        public DbSet<PtRousaiTenki> PtRousaiTenkis { get; set; } = default!;

        public DbSet<PtHokenCheck> PtHokenChecks { get; set; } = default!;

        public DbSet<UserConf> UserConfs { get; set; } = default!;

        public DbSet<PathConf> PathConfs { get; set; } = default!;

        public DbSet<DefHokenNo> DefHokenNos { get; set; } = default!;

        public DbSet<RaiinFilterMst> RaiinFilterMsts { get; set; } = default!;

        public DbSet<RaiinFilterSort> RaiinFilterSorts { get; set; } = default!;

        public DbSet<RaiinFilterState> RaiinFilterStates { get; set; } = default!;

        public DbSet<RaiinFilterKbn> RaiinFilterKbns { get; set; } = default!;

        public DbSet<PtHokenScan> PtHokenScans { get; set; } = default!;

        public DbSet<RsvFrameMst> RsvFrameMsts { get; set; } = default!;

        public DbSet<RsvGrpMst> RsvGrpMsts { get; set; } = default!;

        public DbSet<RsvFrameInf> RsvFrameInfs { get; set; } = default!;

        public DbSet<PtByomei> PtByomeis { get; set; } = default!;

        public DbSet<OdrInf> OdrInfs { get; set; } = default!;

        public DbSet<OdrInfDetail> OdrInfDetails { get; set; } = default!;

        public DbSet<YoyakuOdrInf> YoyakuOdrInfs { get; set; } = default!;

        public DbSet<YoyakuOdrInfDetail> YoyakuOdrInfDetails { get; set; } = default!;

        public DbSet<RaiinKbnYayoku> RaiinKbnYayokus { get; set; } = default!;

        public DbSet<YoyakuSbtMst> YoyakuSbtMsts { get; set; } = default!;

        public DbSet<TenMst> TenMsts { get; set; } = default!;

        public DbSet<RaiinKbnKoui> RaiinKbnKouis { get; set; } = default!;

        public DbSet<KouiKbnMst> KouiKbnMsts { get; set; } = default!;

        public DbSet<ByomeiMst> ByomeiMsts { get; set; } = default!;

        public DbSet<HolidayMst> HolidayMsts { get; set; } = default!;

        public DbSet<JobMst> JobMsts { get; set; } = default!;

        public DbSet<UketukeSbtDayInf> UketukeSbtDayInfs { get; set; } = default!;

        public DbSet<KaikeiInf> KaikeiInfs { get; set; } = default!;

        public DbSet<KaikeiDetail> KaikeiDetails { get; set; } = default!;

        public DbSet<KogakuLimit> KogakuLimits { get; set; } = default!;

        public DbSet<KohiPriority> KohiPriorities { get; set; } = default!;

        public DbSet<DensiHaihanCustom> DensiHaihanCustoms { get; set; } = default!;

        public DbSet<DensiHaihanDay> DensiHaihanDays { get; set; } = default!;

        public DbSet<DensiHaihanKarte> DensiHaihanKartes { get; set; } = default!;

        public DbSet<DensiHaihanMonth> DensiHaihanMonths { get; set; } = default!;

        public DbSet<DensiHaihanWeek> DensiHaihanWeeks { get; set; } = default!;

        public DbSet<DensiHojyo> DensiHojyos { get; set; } = default!;

        public DbSet<DensiHoukatu> DensiHoukatus { get; set; } = default!;

        public DbSet<DensiHoukatuGrp> DensiHoukatuGrps { get; set; } = default!;

        public DbSet<DensiSanteiKaisu> DensiSanteiKaisus { get; set; } = default!;

        public DbSet<SinKoui> SinKouis { get; set; } = default!;

        public DbSet<SinKouiCount> SinKouiCounts { get; set; } = default!;

        public DbSet<SinKouiDetail> SinKouiDetails { get; set; } = default!;

        public DbSet<SinRpInf> SinRpInfs { get; set; } = default!;

        public DbSet<SinRpNoInf> SinRpNoInfs { get; set; } = default!;

        public DbSet<WrkSinKoui> WrkSinKouis { get; set; } = default!;

        public DbSet<WrkSinKouiDetail> WrkSinKouiDetails { get; set; } = default!;

        public DbSet<WrkSinKouiDetailDel> WrkSinKouiDetailDels { get; set; } = default!;

        public DbSet<WrkSinRpInf> WrkSinRpInfs { get; set; } = default!;

        public DbSet<OdrInfCmt> OdrInfCmts { get; set; } = default!;

        public DbSet<CalcStatus> CalcStatus { get; set; } = default!;

        public DbSet<AutoSanteiMst> AutoSanteiMsts { get; set; } = default!;

        public DbSet<PtPregnancy> PtPregnancies { get; set; } = default!;

        public DbSet<CalcLog> CalcLogs { get; set; } = default!;

        public DbSet<SetMst> SetMsts { get; set; } = default!;

        public DbSet<SetKbnMst> SetKbnMsts { get; set; } = default!;

        public DbSet<SetByomei> SetByomei { get; set; } = default!;

        public DbSet<SetOdrInf> SetOdrInf { get; set; } = default!;

        public DbSet<SetOdrInfDetail> SetOdrInfDetail { get; set; } = default!;

        public DbSet<SetOdrInfCmt> SetOdrInfCmt { get; set; } = default!;

        public DbSet<SetKarteInf> SetKarteInf { get; set; } = default!;

        public DbSet<SetKarteImgInf> SetKarteImgInf { get; set; } = default!;

        public DbSet<DosageDrug> DosageDrugs { get; set; } = default!;

        public DbSet<LimitListInf> LimitListInfs { get; set; } = default!;

        public DbSet<DosageDosage> DosageDosages { get; set; } = default!;

        public DbSet<TodoInf> TodoInfs { get; set; } = default!;

        public DbSet<TodoGrpMst> TodoGrpMsts { get; set; } = default!;

        public DbSet<TodoKbnMst> TodoKbnMsts { get; set; } = default!;

        public DbSet<IpnNameMst> IpnNameMsts { get; set; } = default!;
        public DbSet<IpnMinYakkaMst> IpnMinYakkaMsts { get; set; } = default!;
        public DbSet<IpnKasanMst> IpnKasanMsts { get; set; } = default!;

        public DbSet<ListSetMst> ListSetMsts { get; set; } = default!;

        public DbSet<RousaiGoseiMst> RousaiGoseiMsts { get; set; } = default!;

        public DbSet<RsvkrtMst> RsvkrtMsts { get; set; } = default!;

        public DbSet<RsvkrtOdrInf> RsvkrtOdrInfs { get; set; } = default!;

        public DbSet<RsvkrtOdrInfDetail> RsvkrtOdrInfDetails { get; set; } = default!;

        public DbSet<RsvkrtOdrInfCmt> RsvkrtOdrInfCmts { get; set; } = default!;

        public DbSet<KarteKbnMst> KarteKbnMst { get; set; } = default!;

        public DbSet<KarteInf> KarteInfs { get; set; } = default!;

        public DbSet<KarteImgInf> KarteImgInfs { get; set; } = default!;

        public DbSet<SystemGenerationConf> SystemGenerationConfs { get; set; } = default!;

        public DbSet<SchemaCmtMst> SchemaCmtMsts { get; set; } = default!;

        public DbSet<RsvkrtKarteInf> RsvkrtKarteInfs { get; set; } = default!;

        public DbSet<RsvkrtKarteImgInf> RsvkrtKarteImgInfs { get; set; } = default!;

        public DbSet<SummaryInf> SummaryInfs { get; set; } = default!;

        public DbSet<SeikaturekiInf> SeikaturekiInfs { get; set; } = default!;

        public DbSet<SanteiInf> SanteiInfs { get; set; } = default!;

        public DbSet<SanteiInfDetail> SanteiInfDetails { get; set; } = default!;

        public DbSet<KarteFilterMst> KarteFilterMsts { get; set; } = default!;

        public DbSet<KensaMst> KensaMsts { get; set; } = default!;

        public DbSet<YakkaSyusaiMst> YakkaSyusaiMsts { get; set; } = default!;

        public DbSet<KarteFilterDetail> KarteFilterDetails { get; set; } = default!;

        public DbSet<SentenceList> SentenceLists { get; set; } = default!;

        public DbSet<MonshinInfo> MonshinInfo { get; set; } = default!;

        public DbSet<LimitCntListInf> LimitCntListInfs { get; set; } = default!;

        public DbSet<ExceptHokensya> ExceptHokensyas { get; set; } = default!;

        public DbSet<RaiinListTag> RaiinListTags { get; set; } = default!;

        public DbSet<IpnKasanExclude> ipnKasanExcludes { get; set; } = default!;

        public DbSet<IpnKasanExcludeItem> ipnKasanExcludeItems { get; set; } = default!;

        public DbSet<PtFamily> PtFamilys { get; set; } = default!;

        public DbSet<PtFamilyReki> PtFamilyRekis { get; set; } = default!;

        public DbSet<RaiinListMst> RaiinListMsts { get; set; } = default!;

        public DbSet<RaiinListDetail> RaiinListDetails { get; set; } = default!;

        public DbSet<RaiinListInf> RaiinListInfs { get; set; } = default!;

        public DbSet<RaiinListItem> RaiinListItems { get; set; } = default!;

        public DbSet<RaiinListKoui> RaiinListKouis { get; set; } = default!;

        public DbSet<TemplateMst> TemplateMsts { get; set; } = default!;

        public DbSet<TemplateDetail> TemplateDetails { get; set; } = default!;

        public DbSet<TemplateDspConf> TemplateDspConfs { get; set; } = default!;

        public DbSet<TemplateMenuMst> TemplateMenuMsts { get; set; } = default!;

        public DbSet<TemplateMenuDetail> TemplateMenuDetails { get; set; } = default!;

        public DbSet<M41SuppleIndexcode> M41SuppleIndexcodes { get; set; } = default!;

        public DbSet<M41SuppleIndexdef> M41SuppleIndexdefs { get; set; } = default!;

        public DbSet<M41SuppleIngre> M41SuppleIngres { get; set; } = default!;

        public DbSet<PtCmtInf> PtCmtInfs { get; set; } = default!;

        public DbSet<PtLastVisitDate> PtLastVisitDates { get; set; } = default!;

        public DbSet<SyunoSeikyu> SyunoSeikyus { get; set; } = default!;

        public DbSet<KensaInf> KensaInfs { get; set; } = default!;

        public DbSet<KensaInfDetail> KensaInfDetails { get; set; } = default!;

        public DbSet<M38OtcFormCode> M38OtcFormCode { get; set; } = default!;

        public DbSet<M38OtcMain> M38OtcMain { get; set; } = default!;

        public DbSet<M38MajorDivCode> M38MajorDivCode { get; set; } = default!;

        public DbSet<M56UsageCode> M56UsageCode { get; set; } = default!;

        public DbSet<M38ClassCode> M38ClassCode { get; set; } = default!;

        public DbSet<PtTag> PtTag { get; set; } = default!;

        public DbSet<M38OtcMakerCode> M38OtcMakerCode { get; set; } = default!;

        public DbSet<M01Kinki> M01Kinki { get; set; } = default!;

        public DbSet<M01KinkiCmt> M01KinkiCmt { get; set; } = default!;

        public DbSet<M01KijyoCmt> M01KijyoCmt { get; set; } = default!;

        public DbSet<M12FoodAlrgyKbn> M12FoodAlrgyKbn { get; set; } = default!;

        public DbSet<M12FoodAlrgy> M12FoodAlrgy { get; set; } = default!;

        public DbSet<PtAlrgyElse> PtAlrgyElses { get; set; } = default!;

        public DbSet<PtAlrgyFood> PtAlrgyFoods { get; set; } = default!;

        public DbSet<PtAlrgyDrug> PtAlrgyDrugs { get; set; } = default!;

        public DbSet<M28DrugMst> M28DrugMst { get; set; } = default!;

        public DbSet<M10DayLimit> M10DayLimit { get; set; } = default!;

        public DbSet<M14AgeCheck> M14AgeCheck { get; set; } = default!;

        public DbSet<M14CmtCode> M14CmtCode { get; set; } = default!;

        public DbSet<FilingCategoryMst> FilingCategoryMst { get; set; } = default!;

        public DbSet<FilingInf> FilingInf { get; set; } = default!;

        public DbSet<M42ContraCmt> M42ContraCmt { get; set; } = default!;

        public DbSet<M42ContraindiDisBc> M42ContraindiDisBc { get; set; } = default!;

        public DbSet<M42ContraindiDisClass> M42ContraindiDisClass { get; set; } = default!;

        public DbSet<M42ContraindiDisCon> M42ContraindiDisCon { get; set; } = default!;

        public DbSet<M42ContraindiDrugMainEx> M42ContraindiDrugMainEx { get; set; } = default!;

        public DbSet<M56ExIngCode> M56ExIngCode { get; set; } = default!;

        public DbSet<M56ExEdIngredients> M56ExEdIngredients { get; set; } = default!;

        public DbSet<M56ProdrugCd> M56ProdrugCd { get; set; } = default!;

        public DbSet<M56DrvalrgyCode> M56DrvalrgyCode { get; set; } = default!;

        public DbSet<M56AnalogueCd> M56AnalogueCd { get; set; } = default!;

        public DbSet<SyunoNyukin> SyunoNyukin { get; set; } = default!;

        public DbSet<ReceInf> ReceInfs { get; set; } = default!;

        public DbSet<ReceSeikyu> ReceSeikyus { get; set; } = default!;

        public DbSet<GcStdMst> GcStdMsts { get; set; } = default!;

        public DbSet<PtOtcDrug> PtOtcDrug { get; set; } = default!;

        public DbSet<M56ExAnalogue> M56ExAnalogue { get; set; } = default!;

        public DbSet<M56AlrgyDerivatives> M56AlrgyDerivatives { get; set; } = default!;

        public DbSet<PriorityHaihanMst> PriorityHaihanMsts { get; set; } = default!;

        public DbSet<PtOtherDrug> PtOtherDrug { get; set; } = default!;

        public DbSet<PtInfection> PtInfection { get; set; } = default!;

        public DbSet<PtSupple> PtSupples { get; set; } = default!;

        public DbSet<PtKioReki> PtKioRekis { get; set; } = default!;

        public DbSet<PaymentMethodMst> PaymentMethodMsts { get; set; } = default!;

        public DbSet<RaiinListFile> RaiinListFile { get; set; } = default!;

        public DbSet<FilingAutoImp> FilingAutoImp { get; set; } = default!;

        public DbSet<ReceCmt> ReceCmts { get; set; } = default!;

        public DbSet<SyoukiKbnMst> SyoukiKbnMsts { get; set; } = default!;

        public DbSet<SyoukiInf> SyoukiInfs { get; set; } = default!;

        public DbSet<M38IngCode> M38IngCode { get; set; } = default!;

        public DbSet<UnitMst> UnitMsts { get; set; } = default!;

        public DbSet<M38Ingredients> M38Ingredients { get; set; } = default!;

        public DbSet<PhysicalAverage> PhysicalAverage { get; set; } = default!;

        public DbSet<RsvFrameWeekPtn> RsvFrameWeekPtn { get; set; } = default!;

        public DbSet<ReceStatus> ReceStatuses { get; set; } = default!;

        public DbSet<ReceCheckCmt> ReceCheckCmts { get; set; } = default!;

        public DbSet<ReceCheckErr> ReceCheckErrs { get; set; } = default!;

        public DbSet<ReceCheckOpt> ReceCheckOpts { get; set; } = default!;

        public DbSet<RsvFrameWith> RsvFrameWiths { get; set; } = default!;

        public DbSet<SyobyoKeika> SyobyoKeikas { get; set; } = default!;

        public DbSet<ReceInfEdit> ReceInfEdits { get; set; } = default!;

        public DbSet<ReceInfPreEdit> ReceInfPreEdits { get; set; } = default!;
        public DbSet<ApprovalInf> ApprovalInfs { get; set; } = default!;

        public DbSet<RecedenHenJiyuu> RecedenHenJiyuus { get; set; } = default!;
        public DbSet<RecedenRirekiInf> RecedenRirekiInfs { get; set; } = default!;

        public DbSet<ReceFutanKbn> ReceFutanKbns { get; set; } = default!;

        public DbSet<DocComment> DocComments { get; set; } = default!;

        public DbSet<DocCommentDetail> DocCommentDetails { get; set; } = default!;

        public DbSet<PiProductInf> PiProductInfs { get; set; } = default!;

        public DbSet<M34DrugInfoMain> M34DrugInfoMains { get; set; } = default!;

        public DbSet<RaiinListCmt> RaiinListCmts { get; set; } = default!;

        public DbSet<ByomeiSetMst> ByomeiSetMsts { get; set; } = default!;

        public DbSet<UserPermission> UserPermissions { get; set; } = default!;

        public DbSet<TekiouByomeiMst> TekiouByomeiMsts { get; set; } = default!;
        public DbSet<M34PrecautionCode> M34PrecautionCodes { get; set; } = default!;
        public DbSet<M34Precaution> M34Precautions { get; set; } = default!;
        public DbSet<M34InteractionPatCode> M34InteractionPatCodes { get; set; } = default!;
        public DbSet<M34InteractionPat> M34InteractionPats { get; set; } = default!;
        public DbSet<M34SarSymptomCode> M34SarSymptomCodes { get; set; } = default!;
        public DbSet<M34ArDisconCode> M34ArDisconCodes { get; set; } = default!;
        public DbSet<M34ArDiscon> M34ArDiscons { get; set; } = default!;
        public DbSet<M34IndicationCode> M34IndicationCodes { get; set; } = default!;
        public DbSet<M34FormCode> M34FormCodes { get; set; } = default!;
        public DbSet<PiInf> PiInfs { get; set; } = default!;
        public DbSet<PiInfDetail> PiInfDetails { get; set; } = default!;
        public DbSet<PiImage> PiImages { get; set; } = default!;
        public DbSet<M34ArCode> M34ArCodes { get; set; } = default!;
        public DbSet<M34PropertyCode> M34PropertyCodes { get; set; } = default!;
        public DbSet<CmtCheckMst> CmtCheckMsts { get; set; } = default!;
        public DbSet<RsvInf> RsvInfs { get; set; } = default!;
        public DbSet<DocCategoryMst> DocCategoryMsts { get; set; } = default!;
        public DbSet<DocInf> DocInfs { get; set; } = default!;

        public DbSet<SystemConfItem> SystemConfItem { get; set; } = default!;

        public DbSet<SystemConfMenu> SystemConfMenu { get; set; } = default!;

        public DbSet<JihiSbtMst> JihiSbtMsts { get; set; } = default!;

        public DbSet<SinrekiFilterMst> SinrekiFilterMsts { get; set; } = default!;

        public DbSet<SinrekiFilterMstDetail> SinrekiFilterMstDetails { get; set; } = default!;

        public DbSet<KinkiMst> KinkiMsts { get; set; } = default!;

        public DbSet<RsvFrameDayPtn> RsvFrameDayPtns { get; set; } = default!;

        public DbSet<TekiouByomeiMstExcluded> TekiouByomeiMstExcludeds { get; set; } = default!;

        public DbSet<ByomeiSetGenerationMst> ByomeiSetGenerationMsts { get; set; } = default!;

        public DbSet<ListSetGenerationMst> ListSetGenerationMsts { get; set; } = default!;

        public DbSet<DrugInf> DrugInfs { get; set; } = default!;

        public DbSet<MaterialMst> MaterialMsts { get; set; } = default!;
        public DbSet<ContainerMst> ContainerMsts { get; set; } = default!;

        public DbSet<DrugDayLimit> DrugDayLimits { get; set; } = default!;
        public DbSet<DosageMst> DosageMsts { get; set; } = default!;
        public DbSet<FunctionMst> FunctionMsts { get; set; } = default!;
        public DbSet<PermissionMst> PermissionMsts { get; set; } = default!;
        public DbSet<KensaStdMst> KensaStdMsts { get; set; } = default!;
        public DbSet<KensaCenterMst> KensaCenterMsts { get; set; } = default!;

        public DbSet<ReleasenoteRead> ReleasenoteReads { get; set; } = default!;

        public DbSet<SetGenerationMst> SetGenerationMsts { get; set; } = default!;

        public DbSet<RecedenCmtSelect> RecedenCmtSelects { get; set; } = default!;

        public DbSet<StaMst> StaMsts { get; set; } = default!;

        public DbSet<StaGrp> StaGrps { get; set; } = default!;

        public DbSet<StaMenu> StaMenus { get; set; } = default!;

        public DbSet<StaConf> StaConfs { get; set; } = default!;

        public DbSet<SokatuMst> SokatuMsts { get; set; } = default!;

        public DbSet<SingleDoseMst> SingleDoseMsts { get; set; } = default!;

        public DbSet<RenkeiConf> RenkeiConfs { get; set; } = default!;

        public DbSet<RenkeiMst> RenkeiMsts { get; set; } = default!;

        public DbSet<RenkeiPathConf> RenkeiPathConfs { get; set; } = default!;

        public DbSet<RenkeiTemplateMst> RenkeiTemplateMsts { get; set; } = default!;

        public DbSet<RenkeiTimingConf> RenkeiTimingConfs { get; set; } = default!;

        public DbSet<RenkeiTimingMst> RenkeiTimingMsts { get; set; } = default!;

        public DbSet<LockInf> LockInfs { get; set; } = default!;

        public DbSet<LockMst> LockMsts { get; set; } = default!;

        public DbSet<YohoSetMst> YohoSetMsts { get; set; } = default!;

        public DbSet<AuditTrailLog> AuditTrailLogs { get; set; } = default!;

        public DbSet<AuditTrailLogDetail> AuditTrailLogDetails { get; set; } = default!;

        public DbSet<SessionInf> SessionInfs { get; set; } = default!;

        public DbSet<RsvRenkeiInf> RsvRenkeiInfs { get; set; } = default!;

        public DbSet<EventMst> EventMsts { get; set; } = default!;

        public DbSet<KacodeMst> KacodeMsts { get; set; } = default!;

        public DbSet<SanteiAutoOrder> SanteiAutoOrders { get; set; } = default!;

        public DbSet<SanteiAutoOrderDetail> SanteiAutoOrderDetails { get; set; } = default!;

        public DbSet<SanteiCntCheck> SanteiCntChecks { get; set; } = default!;

        public DbSet<SanteiGrpMst> SanteiGrpMsts { get; set; } = default!;

        public DbSet<SanteiGrpDetail> SanteiGrpDetails { get; set; } = default!;

        public DbSet<RenkeiReq> RenkeiReqs { get; set; } = default!;

        public DbSet<AccountingFormMst> AccountingFormMsts { get; set; } = default!;

        public DbSet<RsvDayComment> RsvDayComments { get; set; } = default!;

        public DbSet<KensaIraiLog> KensaIraiLogs { get; set; } = default!;

        public DbSet<RaiinListDoc> RaiinListDocs { get; set; } = default!;

        public DbSet<PtJibaiDoc> PtJibaiDocs { get; set; } = default!;

        public DbSet<StaCsv> StaCsvs { get; set; } = default!;

        public DbSet<ConversionItemInf> ConversionItemInfs { get; set; } = default!;

        public DbSet<BackupReq> BackupReqs { get; set; } = default!;

        public DbSet<M56ExIngrdtMain> M56ExIngrdtMain { get; set; } = default!;

        public DbSet<M56DrugClass> M56DrugClass { get; set; } = default!;

        public DbSet<M56YjDrugClass> M56YjDrugClass { get; set; } = default!;

        public DbSet<SystemChangeLog> SystemChangeLogs { get; set; } = default!;

        public DbSet<ZPtInf> ZPtInfs { get; set; } = default!;

        public DbSet<TenMstMother> TenMstMothers { get; set; } = default!;

        public DbSet<ZDocInf> ZDocInfs { get; set; } = default!;

        public DbSet<ZFilingInf> ZFilingInfs { get; set; } = default!;

        public DbSet<ZKensaInf> ZKensaInfs { get; set; } = default!;

        public DbSet<ZKensaInfDetail> ZKensaInfDetails { get; set; } = default!;

        public DbSet<ZLimitCntListInf> ZLimitCntListInfs { get; set; } = default!;

        public DbSet<ZLimitListInf> ZLimitListInfs { get; set; } = default!;

        public DbSet<ZMonshinInf> ZMonshinInfs { get; set; } = default!;

        public DbSet<ZPtAlrgyDrug> ZPtAlrgyDrugs { get; set; } = default!;

        public DbSet<ZPtAlrgyElse> ZPtAlrgyElses { get; set; } = default!;

        public DbSet<ZPtAlrgyFood> ZPtAlrgyFoods { get; set; } = default!;

        public DbSet<ZPtCmtInf> ZPtCmtInfs { get; set; } = default!;

        public DbSet<ZPtFamily> ZPtFamilys { get; set; } = default!;

        public DbSet<ZPtFamilyReki> ZPtFamilyRekis { get; set; } = default!;

        public DbSet<ZPtGrpInf> ZPtGrpInfs { get; set; } = default!;

        public DbSet<ZPtHokenCheck> ZPtHokenChecks { get; set; } = default!;

        public DbSet<ZPtHokenInf> ZPtHokenInfs { get; set; } = default!;

        public DbSet<ZPtHokenPattern> ZPtHokenPatterns { get; set; } = default!;

        public DbSet<ZPtHokenScan> ZPtHokenScans { get; set; } = default!;

        public DbSet<ZPtInfection> ZPtInfections { get; set; } = default!;

        public DbSet<ZPtKioReki> ZPtKioRekis { get; set; } = default!;

        public DbSet<ZPtKohi> ZPtKohis { get; set; } = default!;

        public DbSet<ZPtKyusei> ZPtKyuseis { get; set; } = default!;

        public DbSet<ZPtMemo> ZPtMemos { get; set; } = default!;

        public DbSet<ZPtOtcDrug> ZPtOtcDrugs { get; set; } = default!;

        public DbSet<ZPtOtherDrug> ZPtOtherDrugs { get; set; } = default!;

        public DbSet<ZPtPregnancy> ZPtPregnancys { get; set; } = default!;

        public DbSet<ZPtRousaiTenki> ZPtRousaiTenkis { get; set; } = default!;

        public DbSet<ZPtSanteiConf> ZPtSanteiConfs { get; set; } = default!;

        public DbSet<ZPtSupple> ZPtSupples { get; set; } = default!;

        public DbSet<ZPtTag> ZPtTags { get; set; } = default!;

        public DbSet<ZRaiinCmtInf> ZRaiinCmtInfs { get; set; } = default!;

        public DbSet<ZRaiinKbnInf> ZRaiinKbnInfs { get; set; } = default!;

        public DbSet<ZRaiinListCmt> ZRaiinListCmts { get; set; } = default!;

        public DbSet<ZRaiinListTag> ZRaiinListTags { get; set; } = default!;

        public DbSet<ZReceCheckCmt> ZReceCheckCmts { get; set; } = default!;

        public DbSet<ZReceCmt> ZReceCmts { get; set; } = default!;

        public DbSet<ZReceInfEdit> ZReceInfEdits { get; set; } = default!;

        public DbSet<ZReceSeikyu> ZReceSeikyus { get; set; } = default!;

        public DbSet<ZSanteiInfDetail> ZSanteiInfDetails { get; set; } = default!;

        public DbSet<ZSeikaturekiInf> ZSeikaturekiInfs { get; set; } = default!;

        public DbSet<ZSummaryInf> ZSummaryInfs { get; set; } = default!;

        public DbSet<ZSyobyoKeika> ZSyobyoKeikas { get; set; } = default!;

        public DbSet<ZSyoukiInf> ZSyoukiInfs { get; set; } = default!;

        public DbSet<ZSyunoNyukin> ZSyunoNyukins { get; set; } = default!;

        public DbSet<ZTodoInf> ZTodoInfs { get; set; } = default!;

        public DbSet<ZRsvDayComment> ZRsvDayComments { get; set; } = default!;

        public DbSet<ZRsvInf> ZRsvInfs { get; set; } = default!;

        public DbSet<CmtKbnMst> CmtKbnMsts { get; set; } = default!;

        public DbSet<ZRaiinInf> ZRaiinInfs { get; set; } = default!;

        public DbSet<DrugUnitConv> DrugUnitConvs { get; set; } = default!;

        public DbSet<OnlineConfirmation> OnlineConfirmations { get; set; } = default!;

        public DbSet<OnlineConfirmationHistory> OnlineConfirmationHistories { get; set; } = default!;

        public DbSet<TagGrpMst> TagGrpMsts { get; set; } = default!;

        public DbSet<RsvRenkeiInfTk> RsvRenkeiInfTks { get; set; } = default!;

        public DbSet<OdrDateInf> OdrDateInfs { get; set; } = default!;
        public DbSet<OdrDateDetail> OdrDateDetails { get; set; } = default!;

        public DbSet<ReceInfJd> ReceInfJds { get; set; } = default!;

        public DbSet<YohoInfMst> YohoInfMsts { get; set; } = default!;

        public DbSet<PtJibkar> PtJibkars { get; set; } = default!;

        public DbSet<ZPtJibkar> ZPtJibkars { get; set; } = default!;

        public DbSet<ItemGrpMst> itemGrpMsts { get; set; } = default!;

        public DbSet<BuiOdrMst> BuiOdrMsts { get; set; } = default!;

        public DbSet<BuiOdrByomeiMst> BuiOdrByomeiMsts { get; set; } = default!;

        public DbSet<BuiOdrItemMst> BuiOdrItemMsts { get; set; } = default!;

        public DbSet<BuiOdrItemByomeiMst> BuiOdrItemByomeiMsts { get; set; } = default!;

        public DbSet<MallMessageInf> MallMessageInfs { get; set; } = default!;

        public DbSet<MallRenkeiInf> MallRenkeiInfs { get; set; } = default!;

        public DbSet<RsvkrtByomei> RsvkrtByomeis { get; set; } = default!;

        public DbSet<OnlineConsent> OnlineConsents { get; set; } = default!;

        public DbSet<KouiHoukatuMst> KouiHoukatuMsts { get; set; } = default!;
    }
}
