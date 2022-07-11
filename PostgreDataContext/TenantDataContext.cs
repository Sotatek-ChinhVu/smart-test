using Entity.Tenant;
using Microsoft.EntityFrameworkCore;

namespace PostgreDataContext
{
    public class TenantDataContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseNpgsql("host=localhost;port=5432;database=Emr;user id=postgres;password=Emr!23");

        //protected override void OnModelCreating(DbModelBuilder modelBuilder)
        //{
        //    modelBuilder.Conventions.Add(new CustomAttribute.DefaultValueAttributeConvention());
        //    modelBuilder.Conventions.Add(new CustomAttribute.DefaultValueSqlAttributeConvention());

        //    base.OnModelCreating(modelBuilder);
        //}

        public DbSet<PtInf> PtInfs { get; set; }

        public DbSet<ZUketukeSbtDayInf> ZUketukeSbtDayInfs { get; set; }

        public DbSet<PtGrpNameMst> PtGrpNameMsts { get; set; }

        public DbSet<PtGrpItem> PtGrpItems { get; set; }

        public DbSet<PtSanteiConf> PtSanteiConfs { get; set; }

        public DbSet<PtHokenInf> PtHokenInfs { get; set; }

        public DbSet<PtKohi> PtKohis { get; set; }

        public DbSet<HokenMst> HokenMsts { get; set; }

        public DbSet<PtHokenPattern> PtHokenPatterns { get; set; }

        public DbSet<PostCodeMst> PostCodeMsts { get; set; }

        public DbSet<SystemConf> SystemConfs { get; set; }

        public DbSet<PtKyusei> PtKyuseis { get; set; }

        public DbSet<KantokuMst> KantokuMsts { get; set; }

        public DbSet<RoudouMst> RoudouMsts { get; set; }

        public DbSet<UserMst> UserMsts { get; set; }

        public DbSet<UketukeSbtMst> UketukeSbtMsts { get; set; }

        public DbSet<KaMst> KaMsts { get; set; }

        public DbSet<TokkiMst> TokkiMsts { get; set; }

        public DbSet<RaiinKbnMst> RaiinKbnMsts { get; set; }

        public DbSet<RaiinKbnDetail> RaiinKbnDetails { get; set; }

        public DbSet<RaiinInf> RaiinInfs { get; set; }

        public DbSet<RaiinCmtInf> RaiinCmtInfs { get; set; }

        public DbSet<PtGrpInf> PtGrpInfs { get; set; }

        public DbSet<PtMemo> PtMemos { get; set; }

        public DbSet<RaiinKbnInf> RaiinKbnInfs { get; set; }
        public DbSet<HpInf> HpInfs { get; set; }

        public DbSet<ByomeiMstAftercare> ByomeiMstAftercares { get; set; }

        public DbSet<HokensyaMst> HokensyaMsts { get; set; }

        public DbSet<TimeZoneConf> TimeZoneConfs { get; set; }

        public DbSet<TimeZoneDayInf> TimeZoneDayInfs { get; set; }

        public DbSet<RaiinKbItem> RaiinKbItems { get; set; }

        public DbSet<PtRousaiTenki> PtRousaiTenkis { get; set; }
        public DbSet<PtHokenCheck> PtHokenChecks { get; set; }

        public DbSet<UserConf> UserConfs { get; set; }

        public DbSet<PathConf> PathConfs { get; set; }

        public DbSet<DefHokenNo> DefHokenNos { get; set; }

        public DbSet<RaiinFilterMst> RaiinFilterMsts { get; set; }

        //public DbSet<RaiinFilterMstHistory> RaiinFilterMstHistories { get; set; }

        public DbSet<RaiinFilterSort> RaiinFilterSorts { get; set; }

        public DbSet<RaiinFilterState> RaiinFilterStates { get; set; }

        public DbSet<RaiinFilterKbn> RaiinFilterKbns { get; set; }

        public DbSet<PtHokenScan> PtHokenScans { get; set; }

        public DbSet<RsvFrameMst> RsvFrameMsts { get; set; }

        public DbSet<RsvGrpMst> RsvGrpMsts { get; set; }

        public DbSet<RsvFrameInf> RsvFrameInfs { get; set; }

        public DbSet<PtByomei> PtByomeis { get; set; }

        public DbSet<OdrInf> OdrInfs { get; set; }

        public DbSet<OdrInfDetail> OdrInfDetails { get; set; }

        public DbSet<YoyakuOdrInf> YoyakuOdrInfs { get; set; }

        public DbSet<YoyakuOdrInfDetail> YoyakuOdrInfDetails { get; set; }

        public DbSet<RaiinKbnYayoku> RaiinKbnYayokus { get; set; }

        public DbSet<YoyakuSbtMst> YoyakuSbtMsts { get; set; }

        public DbSet<TenMst> TenMsts { get; set; }

        //public DbSet<PtSanteiConfHistory> PtSanteiConfHistories { get; set; }

        public DbSet<RaiinKbnKoui> RaiinKbnKouis { get; set; }

        public DbSet<KouiKbnMst> KouiKbnMsts { get; set; }

        public DbSet<ByomeiMst> ByomeiMsts { get; set; }

        public DbSet<HolidayMst> HolidayMsts { get; set; }

        public DbSet<JobMst> JobMsts { get; set; }

        public DbSet<UketukeSbtDayInf> UketukeSbtDayInfs { get; set; }

        public DbSet<KaikeiInf> KaikeiInfs { get; set; }

        public DbSet<KaikeiDetail> KaikeiDetails { get; set; }

        public DbSet<KogakuLimit> KogakuLimits { get; set; }

        public DbSet<KohiPriority> KohiPriorities { get; set; }

        public DbSet<DensiHaihanCustom> DensiHaihanCustoms { get; set; }

        public DbSet<DensiHaihanDay> DensiHaihanDays { get; set; }

        public DbSet<DensiHaihanKarte> DensiHaihanKartes { get; set; }

        public DbSet<DensiHaihanMonth> DensiHaihanMonths { get; set; }

        public DbSet<DensiHaihanWeek> DensiHaihanWeeks { get; set; }

        public DbSet<DensiHojyo> DensiHojyos { get; set; }

        public DbSet<DensiHoukatu> DensiHoukatus { get; set; }

        public DbSet<DensiHoukatuGrp> DensiHoukatuGrps { get; set; }

        public DbSet<DensiSanteiKaisu> DensiSanteiKaisus { get; set; }

        public DbSet<SinKoui> SinKouis { get; set; }

        public DbSet<SinKouiCount> SinKouiCounts { get; set; }

        public DbSet<SinKouiDetail> SinKouiDetails { get; set; }

        public DbSet<SinRpInf> SinRpInfs { get; set; }

        public DbSet<SinRpNoInf> SinRpNoInfs { get; set; }

        public DbSet<WrkSinKoui> WrkSinKouis { get; set; }

        public DbSet<WrkSinKouiDetail> WrkSinKouiDetails { get; set; }

        public DbSet<WrkSinKouiDetailDel> WrkSinKouiDetailDels { get; set; }

        public DbSet<WrkSinRpInf> WrkSinRpInfs { get; set; }

        public DbSet<OdrInfCmt> OdrInfCmts { get; set; }

        public DbSet<CalcStatus> CalcStatus { get; set; }

        public DbSet<AutoSanteiMst> AutoSanteiMsts { get; set; }

        public DbSet<PtPregnancy> PtPregnancies { get; set; }

        public DbSet<CalcLog> CalcLogs { get; set; }

        public DbSet<SetMst> SetMsts { get; set; }

        public DbSet<SetKbnMst> SetKbnMsts { get; set; }

        public DbSet<SetByomei> SetByomei { get; set; }

        public DbSet<SetOdrInf> SetOdrInf { get; set; }

        public DbSet<SetOdrInfDetail> SetOdrInfDetail { get; set; }

        public DbSet<SetOdrInfCmt> SetOdrInfCmt { get; set; }

        public DbSet<SetKarteInf> SetKarteInf { get; set; }

        public DbSet<SetKarteImgInf> SetKarteImgInf { get; set; }

        //public DbSet<PtKyuseiHistory> PtKyuseiHistory { get; set; }

        public DbSet<DosageDrug> DosageDrugs { get; set; }

        public DbSet<LimitListInf> LimitListInfs { get; set; }

        public DbSet<DosageDosage> DosageDosages { get; set; }

        public DbSet<TodoInf> TodoInfs { get; set; }

        public DbSet<TodoGrpMst> TodoGrpMsts { get; set; }

        public DbSet<TodoKbnMst> TodoKbnMsts { get; set; }

        public DbSet<IpnNameMst> IpnNameMsts { get; set; }
        public DbSet<IpnMinYakkaMst> IpnMinYakkaMsts { get; set; }
        public DbSet<IpnKasanMst> IpnKasanMsts { get; set; }

        //public DbSet<TodoGrpMstHistory> TodoGrpMstsMstHistories { get; set; }

        //public DbSet<TodoInfHistory> TodoInfHistories { get; set; }

        //public DbSet<TodoKbnMstHistory> TodoKbnMstHistories { get; set; }
        public DbSet<ListSetMst> ListSetMsts { get; set; }

        public DbSet<RousaiGoseiMst> RousaiGoseiMsts { get; set; }

        public DbSet<RsvkrtMst> RsvkrtMsts { get; set; }

        public DbSet<RsvkrtOdrInf> RsvkrtOdrInfs { get; set; }

        public DbSet<RsvkrtOdrInfDetail> RsvkrtOdrInfDetails { get; set; }

        public DbSet<RsvkrtOdrInfCmt> RsvkrtOdrInfCmts { get; set; }

        public DbSet<KarteKbnMst> KarteKbnMst { get; set; }

        public DbSet<KarteInf> KarteInfs { get; set; }

        public DbSet<KarteImgInf> KarteImgInfs { get; set; }

        public DbSet<SystemGenerationConf> SystemGenerationConfs { get; set; }

        public DbSet<SchemaCmtMst> SchemaCmtMsts { get; set; }

        public DbSet<RsvkrtKarteInf> RsvkrtKarteInfs { get; set; }

        public DbSet<RsvkrtKarteImgInf> RsvkrtKarteImgInfs { get; set; }

        public DbSet<SummaryInf> SummaryInfs { get; set; }

        public DbSet<SeikaturekiInf> SeikaturekiInfs { get; set; }

        public DbSet<SanteiInf> SanteiInfs { get; set; }

        public DbSet<SanteiInfDetail> SanteiInfDetails { get; set; }

        public DbSet<KarteFilterMst> KarteFilterMsts { get; set; }

        public DbSet<KensaMst> KensaMsts { get; set; }

        public DbSet<YakkaSyusaiMst> yakkaSyusaiMsts { get; set; }

        public DbSet<KarteFilterDetail> KarteFilterDetails { get; set; }

        public DbSet<SentenceList> SentenceLists { get; set; }

        public DbSet<MonshinInfo> MonshinInfo { get; set; }

        public DbSet<LimitCntListInf> LimitCntListInfs { get; set; }

        public DbSet<ExceptHokensya> ExceptHokensyas { get; set; }

        public DbSet<RaiinListTag> RaiinListTags { get; set; }

        public DbSet<IpnKasanExclude> ipnKasanExcludes { get; set; }

        public DbSet<IpnKasanExcludeItem> ipnKasanExcludeItems { get; set; }

        public DbSet<PtFamily> PtFamilys { get; set; }

        public DbSet<PtFamilyReki> PtFamilyRekis { get; set; }

        public DbSet<RaiinListMst> RaiinListMsts { get; set; }

        public DbSet<RaiinListDetail> RaiinListDetails { get; set; }

        public DbSet<RaiinListInf> RaiinListInfs { get; set; }

        public DbSet<RaiinListItem> RaiinListItems { get; set; }

        public DbSet<RaiinListKoui> RaiinListKouis { get; set; }

        public DbSet<TemplateMst> TemplateMsts { get; set; }

        public DbSet<TemplateDetail> TemplateDetails { get; set; }

        public DbSet<TemplateDspConf> TemplateDspConfs { get; set; }

        public DbSet<TemplateMenuMst> TemplateMenuMsts { get; set; }

        public DbSet<TemplateMenuDetail> TemplateMenuDetails { get; set; }

        public DbSet<M41SuppleIndexcode> M41SuppleIndexcodes { get; set; }

        public DbSet<M41SuppleIndexdef> M41SuppleIndexdefs { get; set; }

        public DbSet<M41SuppleIngre> M41SuppleIngres { get; set; }

        public DbSet<PtCmtInf> PtCmtInfs { get; set; }

        public DbSet<PtLastVisitDate> PtLastVisitDates { get; set; }

        public DbSet<SyunoSeikyu> SyunoSeikyus { get; set; }

        public DbSet<KensaInf> KensaInfs { get; set; }

        public DbSet<KensaInfDetail> KensaInfDetails { get; set; }

        public DbSet<M38OtcFormCode> M38OtcFormCode { get; set; }

        public DbSet<M38OtcMain> M38OtcMain { get; set; }

        public DbSet<M38MajorDivCode> M38MajorDivCode { get; set; }

        public DbSet<M56UsageCode> M56UsageCode { get; set; }

        public DbSet<M38ClassCode> M38ClassCode { get; set; }

        public DbSet<PtTag> PtTag { get; set; }

        public DbSet<M38OtcMakerCode> M38OtcMakerCode { get; set; }

        public DbSet<M01Kinki> M01Kinki { get; set; }

        public DbSet<M01KinkiCmt> M01KinkiCmt { get; set; }

        public DbSet<M01KijyoCmt> M01KijyoCmt { get; set; }

        public DbSet<M12FoodAlrgyKbn> M12FoodAlrgyKbn { get; set; }

        public DbSet<M12FoodAlrgy> M12FoodAlrgy { get; set; }

        public DbSet<PtAlrgyElse> PtAlrgyElses { get; set; }

        public DbSet<PtAlrgyFood> PtAlrgyFoods { get; set; }

        public DbSet<PtAlrgyDrug> PtAlrgyDrugs { get; set; }

        public DbSet<M28DrugMst> M28DrugMst { get; set; }

        public DbSet<M10DayLimit> M10DayLimit { get; set; }

        public DbSet<M14AgeCheck> M14AgeCheck { get; set; }

        public DbSet<M14CmtCode> M14CmtCode { get; set; }

        public DbSet<FilingCategoryMst> FilingCategoryMst { get; set; }

        public DbSet<FilingInf> FilingInf { get; set; }

        public DbSet<M42ContraCmt> M42ContraCmt { get; set; }

        public DbSet<M42ContraindiDisBc> M42ContraindiDisBc { get; set; }

        public DbSet<M42ContraindiDisClass> M42ContraindiDisClass { get; set; }

        public DbSet<M42ContraindiDisCon> M42ContraindiDisCon { get; set; }

        public DbSet<M42ContraindiDrugMainEx> M42ContraindiDrugMainEx { get; set; }

        public DbSet<M56ExIngCode> M56ExIngCode { get; set; }

        public DbSet<M56ExEdIngredients> M56ExEdIngredients { get; set; }

        public DbSet<M56ProdrugCd> M56ProdrugCd { get; set; }

        public DbSet<M56DrvalrgyCode> M56DrvalrgyCode { get; set; }

        public DbSet<M56AnalogueCd> M56AnalogueCd { get; set; }

        public DbSet<SyunoNyukin> SyunoNyukin { get; set; }

        public DbSet<ReceInf> ReceInfs { get; set; }

        public DbSet<ReceSeikyu> ReceSeikyus { get; set; }

        public DbSet<GcStdMst> GcStdMsts { get; set; }

        public DbSet<PtOtcDrug> PtOtcDrug { get; set; }

        public DbSet<M56ExAnalogue> M56ExAnalogue { get; set; }

        public DbSet<M56AlrgyDerivatives> M56AlrgyDerivatives { get; set; }

        public DbSet<PriorityHaihanMst> PriorityHaihanMsts { get; set; }

        public DbSet<PtOtherDrug> PtOtherDrug { get; set; }

        public DbSet<PtInfection> PtInfection { get; set; }

        public DbSet<PtSupple> PtSupples { get; set; }

        public DbSet<PtKioReki> PtKioRekis { get; set; }

        public DbSet<PaymentMethodMst> PaymentMethodMsts { get; set; }

        public DbSet<RaiinListFile> RaiinListFile { get; set; }

        public DbSet<FilingAutoImp> FilingAutoImp { get; set; }

        public DbSet<ReceCmt> ReceCmts { get; set; }

        public DbSet<SyoukiKbnMst> SyoukiKbnMsts { get; set; }

        public DbSet<SyoukiInf> SyoukiInfs { get; set; }

        public DbSet<M38IngCode> M38IngCode { get; set; }

        public DbSet<UnitMst> UnitMsts { get; set; }

        public DbSet<M38Ingredients> M38Ingredients { get; set; }

        public DbSet<PhysicalAverage> PhysicalAverage { get; set; }

        public DbSet<RsvFrameWeekPtn> RsvFrameWeekPtn { get; set; }

        public DbSet<ReceStatus> ReceStatuses { get; set; }

        public DbSet<ReceCheckCmt> ReceCheckCmts { get; set; }

        public DbSet<ReceCheckErr> ReceCheckErrs { get; set; }

        public DbSet<ReceCheckOpt> ReceCheckOpts { get; set; }

        public DbSet<RsvFrameWith> RsvFrameWiths { get; set; }

        public DbSet<SyobyoKeika> SyobyoKeikas { get; set; }

        public DbSet<ReceInfEdit> ReceInfEdits { get; set; }

        public DbSet<ReceInfPreEdit> ReceInfPreEdits { get; set; }
        public DbSet<ApprovalInf> ApprovalInfs { get; set; }

        public DbSet<RecedenHenJiyuu> RecedenHenJiyuus { get; set; }
        public DbSet<RecedenRirekiInf> RecedenRirekiInfs { get; set; }

        public DbSet<ReceFutanKbn> ReceFutanKbns { get; set; }

        public DbSet<DocComment> DocComments { get; set; }

        public DbSet<DocCommentDetail> DocCommentDetails { get; set; }

        public DbSet<PiProductInf> PiProductInfs { get; set; }

        public DbSet<M34DrugInfoMain> M34DrugInfoMains { get; set; }

        public DbSet<RaiinListCmt> RaiinListCmts { get; set; }

        public DbSet<ByomeiSetMst> ByomeiSetMsts { get; set; }

        public DbSet<UserPermission> UserPermissions { get; set; }

        public DbSet<TekiouByomeiMst> TekiouByomeiMsts { get; set; }
        public DbSet<M34PrecautionCode> M34PrecautionCodes { get; set; }
        public DbSet<M34Precaution> M34Precautions { get; set; }
        public DbSet<M34InteractionPatCode> M34InteractionPatCodes { get; set; }
        public DbSet<M34InteractionPat> M34InteractionPats { get; set; }
        public DbSet<M34SarSymptomCode> M34SarSymptomCodes { get; set; }
        public DbSet<M34ArDisconCode> M34ArDisconCodes { get; set; }
        public DbSet<M34ArDiscon> M34ArDiscons { get; set; }
        public DbSet<M34IndicationCode> M34IndicationCodes { get; set; }
        public DbSet<M34FormCode> M34FormCodes { get; set; }
        public DbSet<PiInf> PiInfs { get; set; }
        public DbSet<PiInfDetail> PiInfDetails { get; set; }
        public DbSet<PiImage> PiImages { get; set; }
        public DbSet<M34ArCode> M34ArCodes { get; set; }
        public DbSet<M34PropertyCode> M34PropertyCodes { get; set; }
        public DbSet<CmtCheckMst> CmtCheckMsts { get; set; }
        public DbSet<RsvInf> RsvInfs { get; set; }
        public DbSet<DocCategoryMst> DocCategoryMsts { get; set; }
        public DbSet<DocInf> DocInfs { get; set; }

        public DbSet<SystemConfItem> SystemConfItem { get; set; }

        public DbSet<SystemConfMenu> SystemConfMenu { get; set; }

        public DbSet<JihiSbtMst> JihiSbtMsts { get; set; }

        public DbSet<SinrekiFilterMst> SinrekiFilterMsts { get; set; }

        public DbSet<SinrekiFilterMstDetail> SinrekiFilterMstDetails { get; set; }

        public DbSet<KinkiMst> KinkiMsts { get; set; }

        public DbSet<RsvFrameDayPtn> RsvFrameDayPtns { get; set; }

        public DbSet<TekiouByomeiMstExcluded> TekiouByomeiMstExcludeds { get; set; }

        public DbSet<ByomeiSetGenerationMst> ByomeiSetGenerationMsts { get; set; }

        public DbSet<ListSetGenerationMst> ListSetGenerationMsts { get; set; }

        public DbSet<DrugInf> DrugInfs { get; set; }

        public DbSet<MaterialMst> MaterialMsts { get; set; }
        public DbSet<ContainerMst> ContainerMsts { get; set; }

        public DbSet<DrugDayLimit> DrugDayLimits { get; set; }
        public DbSet<DosageMst> DosageMsts { get; set; }
        public DbSet<FunctionMst> FunctionMsts { get; set; }
        public DbSet<PermissionMst> PermissionMsts { get; set; }
        public DbSet<KensaStdMst> KensaStdMsts { get; set; }
        public DbSet<KensaCenterMst> KensaCenterMsts { get; set; }

        public DbSet<ReleasenoteRead> ReleasenoteReads { get; set; }

        public DbSet<SetGenerationMst> SetGenerationMsts { get; set; }

        public DbSet<RecedenCmtSelect> RecedenCmtSelects { get; set; }

        public DbSet<StaMst> StaMsts { get; set; }

        public DbSet<StaGrp> StaGrps { get; set; }

        public DbSet<StaMenu> StaMenus { get; set; }

        public DbSet<StaConf> StaConfs { get; set; }

        public DbSet<SokatuMst> SokatuMsts { get; set; }

        public DbSet<SingleDoseMst> SingleDoseMsts { get; set; }

        public DbSet<RenkeiConf> RenkeiConfs { get; set; }
        public DbSet<RenkeiMst> RenkeiMsts { get; set; }
        public DbSet<RenkeiPathConf> RenkeiPathConfs { get; set; }
        public DbSet<RenkeiTemplateMst> RenkeiTemplateMsts { get; set; }
        public DbSet<RenkeiTimingConf> RenkeiTimingConfs { get; set; }
        public DbSet<RenkeiTimingMst> RenkeiTimingMsts { get; set; }

        public DbSet<LockInf> LockInfs { get; set; }

        public DbSet<LockMst> LockMsts { get; set; }

        public DbSet<YohoSetMst> YohoSetMsts { get; set; }

        public DbSet<AuditTrailLog> AuditTrailLogs { get; set; }
        public DbSet<AuditTrailLogDetail> AuditTrailLogDetails { get; set; }

        public DbSet<SessionInf> SessionInfs { get; set; }

        public DbSet<RsvRenkeiInf> RsvRenkeiInfs { get; set; }

        public DbSet<EventMst> EventMsts { get; set; }

        public DbSet<KacodeMst> KacodeMsts { get; set; }

        public DbSet<SanteiAutoOrder> SanteiAutoOrders { get; set; }

        public DbSet<SanteiAutoOrderDetail> SanteiAutoOrderDetails { get; set; }

        public DbSet<SanteiCntCheck> SanteiCntChecks { get; set; }

        public DbSet<SanteiGrpMst> SanteiGrpMsts { get; set; }

        public DbSet<SanteiGrpDetail> SanteiGrpDetails { get; set; }

        public DbSet<RenkeiReq> RenkeiReqs { get; set; }

        public DbSet<AccountingFormMst> AccountingFormMsts { get; set; }

        public DbSet<RsvDayComment> RsvDayComments { get; set; }

        public DbSet<KensaIraiLog> KensaIraiLogs { get; set; }

        public DbSet<RaiinListDoc> RaiinListDocs { get; set; }

        public DbSet<PtJibaiDoc> PtJibaiDocs { get; set; }

        public DbSet<StaCsv> StaCsvs { get; set; }

        public DbSet<ConversionItemInf> ConversionItemInfs { get; set; }

        public DbSet<BackupReq> BackupReqs { get; set; }

        public DbSet<M56ExIngrdtMain> M56ExIngrdtMain { get; set; }

        public DbSet<M56DrugClass> M56DrugClass { get; set; }

        public DbSet<M56YjDrugClass> M56YjDrugClass { get; set; }

        public DbSet<SystemChangeLog> SystemChangeLogs { get; set; }

        public DbSet<ZPtInf> ZPtInfs { get; set; }

        public DbSet<TenMstMother> TenMstMothers { get; set; }

        public DbSet<ZDocInf> ZDocInfs { get; set; }

        public DbSet<ZFilingInf> ZFilingInfs { get; set; }

        public DbSet<ZKensaInf> ZKensaInfs { get; set; }

        public DbSet<ZKensaInfDetail> ZKensaInfDetails { get; set; }

        public DbSet<ZLimitCntListInf> ZLimitCntListInfs { get; set; }

        public DbSet<ZLimitListInf> ZLimitListInfs { get; set; }

        public DbSet<ZMonshinInf> ZMonshinInfs { get; set; }

        public DbSet<ZPtAlrgyDrug> ZPtAlrgyDrugs { get; set; }

        public DbSet<ZPtAlrgyElse> ZPtAlrgyElses { get; set; }

        public DbSet<ZPtAlrgyFood> ZPtAlrgyFoods { get; set; }

        public DbSet<ZPtCmtInf> ZPtCmtInfs { get; set; }

        public DbSet<ZPtFamily> ZPtFamilys { get; set; }

        public DbSet<ZPtFamilyReki> ZPtFamilyRekis { get; set; }

        public DbSet<ZPtGrpInf> ZPtGrpInfs { get; set; }

        public DbSet<ZPtHokenCheck> ZPtHokenChecks { get; set; }

        public DbSet<ZPtHokenInf> ZPtHokenInfs { get; set; }

        public DbSet<ZPtHokenPattern> ZPtHokenPatterns { get; set; }

        public DbSet<ZPtHokenScan> ZPtHokenScans { get; set; }

        public DbSet<ZPtInfection> ZPtInfections { get; set; }

        public DbSet<ZPtKioReki> ZPtKioRekis { get; set; }

        public DbSet<ZPtKohi> ZPtKohis { get; set; }

        public DbSet<ZPtKyusei> ZPtKyuseis { get; set; }

        public DbSet<ZPtMemo> ZPtMemos { get; set; }

        public DbSet<ZPtOtcDrug> ZPtOtcDrugs { get; set; }

        public DbSet<ZPtOtherDrug> ZPtOtherDrugs { get; set; }

        public DbSet<ZPtPregnancy> ZPtPregnancys { get; set; }

        public DbSet<ZPtRousaiTenki> ZPtRousaiTenkis { get; set; }

        public DbSet<ZPtSanteiConf> ZPtSanteiConfs { get; set; }

        public DbSet<ZPtSupple> ZPtSupples { get; set; }

        public DbSet<ZPtTag> ZPtTags { get; set; }

        public DbSet<ZRaiinCmtInf> ZRaiinCmtInfs { get; set; }

        public DbSet<ZRaiinKbnInf> ZRaiinKbnInfs { get; set; }

        public DbSet<ZRaiinListCmt> ZRaiinListCmts { get; set; }

        public DbSet<ZRaiinListTag> ZRaiinListTags { get; set; }

        public DbSet<ZReceCheckCmt> ZReceCheckCmts { get; set; }

        public DbSet<ZReceCmt> ZReceCmts { get; set; }

        public DbSet<ZReceInfEdit> ZReceInfEdits { get; set; }

        public DbSet<ZReceSeikyu> ZReceSeikyus { get; set; }

        public DbSet<ZSanteiInfDetail> ZSanteiInfDetails { get; set; }

        public DbSet<ZSeikaturekiInf> ZSeikaturekiInfs { get; set; }

        public DbSet<ZSummaryInf> ZSummaryInfs { get; set; }

        public DbSet<ZSyobyoKeika> ZSyobyoKeikas { get; set; }

        public DbSet<ZSyoukiInf> ZSyoukiInfs { get; set; }

        public DbSet<ZSyunoNyukin> ZSyunoNyukins { get; set; }

        public DbSet<ZTodoInf> ZTodoInfs { get; set; }

        public DbSet<ZRsvDayComment> ZRsvDayComments { get; set; }

        public DbSet<ZRsvInf> ZRsvInfs { get; set; }

        public DbSet<CmtKbnMst> CmtKbnMsts { get; set; }

        public DbSet<ZRaiinInf> ZRaiinInfs { get; set; }

        public DbSet<DrugUnitConv> DrugUnitConvs { get; set; }

        public DbSet<OnlineConfirmation> OnlineConfirmations { get; set; }

        public DbSet<OnlineConfirmationHistory> OnlineConfirmationHistories { get; set; }

        public DbSet<TagGrpMst> TagGrpMsts { get; set; }

        public DbSet<RsvRenkeiInfTk> RsvRenkeiInfTks { get; set; }

        public DbSet<OdrDateInf> OdrDateInfs { get; set; }
        public DbSet<OdrDateDetail> OdrDateDetails { get; set; }

        public DbSet<ReceInfJd> ReceInfJds { get; set; }

        public DbSet<YohoInfMst> YohoInfMsts { get; set; }

        public DbSet<PtJibkar> PtJibkars { get; set; }

        public DbSet<ZPtJibkar> ZPtJibkars { get; set; }

        public DbSet<ItemGrpMst> itemGrpMsts { get; set; }

        public DbSet<BuiOdrMst> BuiOdrMsts { get; set; }

        public DbSet<BuiOdrByomeiMst> BuiOdrByomeiMsts { get; set; }

        public DbSet<BuiOdrItemMst> BuiOdrItemMsts { get; set; }

        public DbSet<BuiOdrItemByomeiMst> BuiOdrItemByomeiMsts { get; set; }

        public DbSet<MallMessageInf> MallMessageInfs { get; set; }

        public DbSet<MallRenkeiInf> MallRenkeiInfs { get; set; }
    }
}
