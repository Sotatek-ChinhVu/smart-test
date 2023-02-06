namespace EmrCloudApi.Constants
{
    public static class ResponseMessage
    {
        //Invalid parameter
        public static readonly string InvalidKeyword = "Invalid keyword";
        public static readonly string InvalidHpId = "Invalid HpId";
        public static readonly string InvalidPtId = "Invalid PtId";
        public static readonly string InvalidRaiinNo = "Invalid RaiinNo";
        public static readonly string InvalidSinDate = "Invalid SinDate";
        public static readonly string InvalidHokenId = "Invalid HokenId";
        public static readonly string InvalidPageIndex = "Invalid PageIndex";
        public static readonly string InvalidPageCount = "Invalid PageCount";
        public static readonly string InvalidPageSize = "Invalid PageSize";
        public static readonly string InvalidStartIndex = "Invalid StartIndex";
        public static readonly string InvalidKouiKbn = "Invalid KouiKbn";
        public static readonly string InvalidValueAdopted = "Invalid Value Adopted";
        public static readonly string InvalidItemCd = "Invalid ItemCd";
        public static readonly string InvalidFutansyaNo = "Invalid FutansyaNo";
        public static readonly string InvalidUsageKbn = "Invalid UsageKbn";
        public static readonly string InvalidKohiId = "Invalid HokenKohiId";
        public static readonly string InvalidGrpCd = "Invalid GrpCd";
        public static readonly string InvalidGrpEdaNo = "Invalid GrpEdaNo";
        public static readonly string InvalidDefaultValue = "Invalid DefaultValue";
        public static readonly string InvalidPresentDate = "Invalid PresentDate";
        public static readonly string InvalidHpIdNotExist = "HpId not exist";
        public static readonly string InvalidStartDate = "Invalid startDate";
        public static readonly string InvalidYJCode = "Invalid YJCode";
        public static readonly string InvalidIsDeleted = "Invalid isDeleted";
        public static readonly string InvalidSeqNo = "Invalid SeqNo";
        public static readonly string InvalidDate = "Invalid Date";
        public static readonly string InvalidValue = "Invalid Value ";
        public static readonly string InvalidHokenEdraNo = "Invalid HokenEdraNo";
        public static readonly string InvalidTantoId = "Invalid TantoId";
        public static readonly string InvalidAdoptedValue = "Invalid Adopted Value";
        public static readonly string InvalidCurrentIndex = "Invalid CurrentIndex";
        public static readonly string InvalidWindowType = "Invalid WindowType";
        public static readonly string InvalidFrameId = "Invalid FrameId";
        public static readonly string InvalidOyaRaiinNo = "Invalid OyaRaiinNo";
        public static readonly string InvalidPrimaryDoctor = "Invalid PrimaryDoctor";

        //Common
        public static readonly string NotFound = "Not found";
        public static readonly string Success = "Success";
        public static readonly string NoData = "No data";
        public static readonly string Failed = "Failed";
        public static readonly string DuplicateId = "DuplicateId";
        public static readonly string ExistedId = "ExistedId";
        public static readonly string InputDataNull = "Input data is null";
        public static readonly string Valid = "Valid";

        public static readonly string CreateUserInvalidName = "Please input user name";
        public static readonly string CreateUserSuccessed = "User created!!!";
        public static readonly string Error = "Error";

        //Patient Infor
        public static readonly string InvalidPtNum = "Invalid PtNum";

        // RousaiJibai
        public static readonly string InvalidHokenKbn = "Invalid HokenKbn";
        public static readonly string InvalidSelectedHokenInfRousaiSaigaiKbn = "Invalid SelectedHokenInf RousaiSaigaiKbn";
        public static readonly string InvalidSelectedHokenInfRousaiSyobyoDate = "Invalid SelectedHokenInf RousaiSyobyoDate";
        public static readonly string InvalidSelectedHokenInfRyoyoStartDate = "Invalid SelectedHokenInf RyoyoStartDate";
        public static readonly string InvalidSelectedHokenInfRyoyoEndDate = "Invalid SelectedHokenInf RyoyoEndDate";

        //Group Infor
        public static readonly string DuplicateGroupId = "Can not duplicate GroupId";
        public static readonly string DuplicateGroupName = "Can not duplicate GroupName";
        public static readonly string DuplicateGroupDetailCode = "Can not Duplicate GroupDetail Code";
        public static readonly string DuplicateGroupDetailSeqNo = "Can not Duplicate GroupDetail SeqNo";
        public static readonly string DuplicateGroupDetailName = "Can not Duplicate GroupDetail Name";
        public static readonly string InvalidGroupId = "Invalid GroupId, GroupId > 0";
        public static readonly string InvalidGroupName = "Invalid GroupName, GroupName is required and length must be less than or equal 20";
        public static readonly string InvalidDetailGroupCode = "Invalid GroupCode, GroupCode is required and length must be less than or equal 2";
        public static readonly string InvalidGroupDetailName = "Invalid GroupDetailName, GroupDetailName is required and length must be less than or equal 30";

        //Reception controller
        public static readonly string ReceptionInvalidUserId = "Invalid UserId";
        public static readonly string InValidHpId = "InValidHpId";
        public static readonly string InValidPtId = "InValidPtId";

        //PtDisease controller
        public static readonly string PtDiseaseUpsertSuccess = "更新が成功しました";
        public static readonly string PtDiseaseUpsertFail = "更新に失敗しました。";
        public static readonly string PtDiseaseUpsertInputNoData = "無効なデータを受信しました。";

        //User controller
        public static readonly string UpsertInputNoData = "Input No Data";
        public static readonly string UpsertInvalidExistedLoginId = "Existed LoginId";
        public static readonly string UpsertInvalidNoExistedId = "No Exist Id";
        public static readonly string UpsertInvalidExistedUserId = "Existed UserId";
        public static readonly string UpsertInvalidId = "Invalid Id";
        public static readonly string UpsertInvalidUserId = "Invalid UserId";
        public static readonly string UpsertInvalidJobCd = "Invalid JobCd";
        public static readonly string UpsertInvalidManagerKbn = "Invalid ManagerKbn";
        public static readonly string UpsertInvalidKaId = "Invalid KaId";
        public static readonly string UpsertInvalidKanaName = "Invalid KanaName";
        public static readonly string UpsertInvalidName = "Invalid Name";
        public static readonly string UpsertInvalidSname = "Invalid Sname";
        public static readonly string UpsertInvalidLoginId = "Invalid LoginId";
        public static readonly string UpsertInvalidLoginPass = "Invalid LoginPass";
        public static readonly string UpsertInvalidStartDate = "Invalid StartDate";
        public static readonly string UpsertInvalidEndDate = "Invalid EndDate";
        public static readonly string UpsertInvalidSortNo = "Invalid SortNo";
        public static readonly string UpsertInvalidIsDeleted = "Invalid IsDeleted";
        public static readonly string UpsertInvalidRenkeiCd1 = "Invalid RenkeiCd1";
        public static readonly string UpsertInvalidDrName = "Invalid DrName";
        public static readonly string UpsertUserListSuccess = "UpsertUserListSuccess";
        public static readonly string UpsertKaIdNoExist = "No Exist KaId";
        public static readonly string UpsertJobCdNoExist = "No Exist JobCd";
        public static readonly string UpsertIdNoExist = "No Exist Id";
        public static readonly string UserListExistedInputData = "UserListExistedInputData";

        //ApprovalInfo
        public static readonly string InvalidStarDate = "InvalidStarDate";
        public static readonly string InvalidEndDate = "InvalidEndDate";
        public static readonly string InvalidId = "InvalidId";
        public static readonly string ApprovalInfoListInputNoData = "ApprovalInfoListInputNoData";
        public static readonly string ApprovalInfListExistedInputData = "ApprovalInfListExistedInputData";
        public static readonly string ApprovalInfListInvalidNoExistedId = "ApprovalInfListInvalidNoExistedId";
        public static readonly string ApprovalInfListInvalidNoExistedRaiinNo = "ApprovalInfListInvalidNoExistedRaiinNo";

        //Insurance
        // Validate Main Insurance
        public static readonly string InvalidPtBirthday = "Invalid PtBirthday";
        public static readonly string InvalidSelectedHokenInfHokenNo = "Invalid SelectedHokenInf HokenNo";
        public static readonly string InvalidSelectedHokenInfStartDate = "Invalid SelectedHokenInf StartDate";
        public static readonly string InvalidSelectedHokenInfEndDate = "Invalid SelectedHokenInf EndDate";
        public static readonly string InvaliSelectedHokenInfHokensyaMstIsKigoNa = "Invalid SelectedHokenInf HokensyaMst IsKigoNa";
        public static readonly string InvalidSelectedHokenInfHonkeKbn = "Invalid SelectedHokenInf HonkeKbn";
        public static readonly string InvalidSelectedHokenInfTokureiYm1 = "Invalid SelectedHokenInf TokureiYm1";
        public static readonly string InvalidSelectedHokenInfTokureiYm2 = "Invalid SelectedHokenInf TokureiYm2";
        public static readonly string InvalidSelectedHokenInfConfirmDate = "Invalid SelectedHokenInf ConfirmDate";
        public static readonly string InvalidSelectedHokenMstHokenNo = "Invalid SelectedHokenMst HokenNo";
        public static readonly string InvalidSelectedHokenMstCheckDegit = "Invalid SelectedHokenMst CheckDegit";
        public static readonly string InvalidSelectedHokenMstAgeStart = "Invalid SelectedHokenMst AgeStart";
        public static readonly string InvalidSelectedHokenMstAgeEnd = "Invalid SelectedHokenMst AgeEnd";
        public static readonly string InvalidSelectedHokenMstStartDate = "Invalid SelectedHokenMst StartDate";
        public static readonly string InvalidSelectedHokenMstEndDate = "Invalid SelectedHokenMst EndDate";
        public static readonly string InvalidHistoryPid = "Invalid HistoryPid";
        public static readonly string InvalidSelectedHokenPid = "Invalid SelectedHokenPid";
        public static readonly string InvalidException = "Invalid Exception";

        // Validate Pattern
        public static readonly string InvalidPatternJihiSelectedHokenInfHokenNoEquals0 = "Invalid SelectedHokenInf HokenNo Equals 0";
        public static readonly string InvalidPatternEmptyHoken = "Invalid Empty Hoken";
        public static readonly string InvalidPatternHokenNashiOnly = "Invalid Hoken Nashi Only";
        public static readonly string InvalidPatternTokkiValue1 = "Invalid TokkiValue1";
        public static readonly string InvalidPatternTokkiValue21 = "Invalid TokkiValue21";
        public static readonly string InvalidPatternTokkiValue31 = "Invalid TokkiValue31";
        public static readonly string InvalidPatternTokkiValue41 = "Invalid TokkiValue41";
        public static readonly string InvalidPatternTokkiValue51 = "Invalid TokkiValue51";
        public static readonly string InvalidPatternTokkiValue2 = "Invalid TokkiValue2";
        public static readonly string InvalidPatternTokkiValue23 = "Invalid TokkiValue23";
        public static readonly string InvalidPatternTokkiValue24 = "Invalid TokkiValue24";
        public static readonly string InvalidPatternTokkiValue25 = "Invalid TokkiValue25";
        public static readonly string InvalidPatternTokkiValue3 = "Invalid TokkiValue3";
        public static readonly string InvalidPatternTokkiValue34 = "Invalid TokkiValue34";
        public static readonly string InvalidPatternTokkiValue35 = "Invalid TokkiValue35";
        public static readonly string InvalidPatternTokkiValue4 = "Invalid TokkiValue4";
        public static readonly string InvalidPatternTokkiValue45 = "Invalid TokkiValue45";
        public static readonly string InvalidPatternTokkiValue5 = "Invalid TokkiValue5";
        public static readonly string InvalidPatternYukoKigen = "Invalid YukoKigen";
        public static readonly string InvalidPatternHokenSyaNoNullAndHokenNoNotEquals0 = "Invalid HokenSyaNo Null And HokenNo Not Equals 0 ";
        public static readonly string InvalidPatternHokenNoEquals0 = "Invalid HokenNo Equals 0";
        public static readonly string InvalidPatternHokenNoHaveHokenMst = "Invalid Hoken No Have HokenMst";
        public static readonly string InvalidPatternHoubetu = "Invalid Houbetu";
        public static readonly string InvalidPatternCheckDigitEquals1 = "Invalid CheckDigit Equals 1";
        public static readonly string InvalidPatternCheckAgeHokenMst = "Invalid CheckAgeHokenMst ";
        public static readonly string InvalidPatternHokensyaNoNull = "Invalid CheckAgeHokenMst ";
        public static readonly string InvalidPatternHokensyaNoEquals0 = "Invalid CheckAgeHokenMst ";
        public static readonly string InvalidPatternHokensyaNoLength8StartWith39 = "Invalid HokensyaNoLength8StartWith39";
        public static readonly string InvalidPatternKigoNull = "Invalid KigoNull";
        public static readonly string InvalidPatternBangoNull = "Invalid BangoNull";
        public static readonly string InvalidPatternHokenKbnEquals0 = "Invalid HokenKbnEquals0";
        public static readonly string InvalidPatternTokkurei = "Invalid Tokkurei";
        public static readonly string InvalidPatternConfirmDateAgeCheck = "Invalid ConfirmDateAgeCheck";
        public static readonly string InvalidPatternHokenMstStartDate = "Invalid HokenMstStartDate";
        public static readonly string InvalidPatternHokenMstEndDate = "Invalid HokenMstEndDate";
        public static readonly string InvalidPatternHokenRodoBangoNull = "Invalid RodoBangoNull";
        public static readonly string InvalidPatternRodoBangoLengthNotEquals14 = "Invalid RodoBangoLengthNotEquals14";
        public static readonly string InvalidPatternRousaiTenkiDefaultRow = "Invalid RousaiTenkiDefaultRow";
        public static readonly string InvalidPatternRousaiTenkiData = "Invalid RousaiTenkiData";
        public static readonly string InvalidPatternRousaiSaigaiKbn = "Invalid RousaiSaigaiKbn";
        public static readonly string InvalidPatternRousaiSyobyoDateEquals0 = "Invalid RousaiSyobyoDate Equals 0";
        public static readonly string InvalidPatternRousaiSyobyoCdNull = "Invalid RousaiSyobyoCdNull";
        public static readonly string InvalidPatternRousaiRyoyoDate = "Invalid RousaiRyoyoDate";
        public static readonly string InvalidPatternRosaiYukoDate = "Invalid RosaiYukoDate";
        public static readonly string InvalidPatternCheckHokenInfDate = "Invalid CheckHokenInfDate";
        public static readonly string InvalidPatternNenkinBangoNull = "Invalid NenkinBangoNull";
        public static readonly string InvalidPatternNenkinBangoLengthNotEquals9 = "Invalid NenkinBango Length Not Equals 9";
        public static readonly string InvalidPatternKenkoKanriBangoNull = "Invalid KenkoKanriBangoNull";
        public static readonly string InvalidPatternKenkoKanriBangoLengthNotEquals13 = "Invalid KenkoKanriBango Length Not Equals 13";
        public static readonly string InvalidPatternKohiEmptyModel1 = "Invalid Kohi1 Empty Model";
        public static readonly string InvalidPatternKohiHokenMstEmpty1 = "Invalid Kohi1 HokenMst Empty";
        public static readonly string InvalidPatternFutansyaNoEmpty1 = "Invalid Kohi1 FutansyaNo Empty";
        public static readonly string InvalidPatternJyukyusyaNo1 = "Invalid Kohi1 JyukyusyaNo";
        public static readonly string InvalidPatternTokusyuNo1 = "Invalid Kohi1 TokusyuNo";
        public static readonly string InvalidPatternFutansyaNo01 = "Invalid Kohi1 FutansyaNo Equals 0";
        public static readonly string InvalidPatternKohiYukoDate1 = "Invalid Kohi1 YukoDate";
        public static readonly string InvalidPatternKohiHokenMstStartDate1 = "Invalid Kohi1 HokenMst StartDate";
        public static readonly string InvalidPatternKohiHokenMstEndDate1 = "Invalid Kohi1 HokenMst EndDate";
        public static readonly string InvalidPatternKohiConfirmDate1 = "Invalid Kohi1 ConfirmDate";
        public static readonly string InvalidPatternKohiMstCheckHBT1 = "Invalid Kohi1 HokenMst CheckHBT";
        public static readonly string InvalidPatternKohiMstCheckDigitFutansyaNo1 = "Invalid Kohi1 HokenMst CheckDigitFutansyaNo";
        public static readonly string InvalidPatternKohiHokenMstCheckDigitJyukyusyaNo1 = "Invalid Kohi1 HokenMst CheckDigitJyukyusyaNo";
        public static readonly string InvalidPatternKohiHokenMstCheckAge1 = "Invalid Kohi1 HokenMst CheckAge";
        public static readonly string InvalidPatternKohiHokenMstFutanJyoTokuNull1 = "Invalid Kohi1 HokenMst FutanJyoToku Is Null";
        public static readonly string InvalidPatternKohiEmptyModel2 = "Invalid Kohi2 Empty Model";
        public static readonly string InvalidPatternKohiHokenMstEmpty2 = "Invalid Kohi2 HokenMst Empty";
        public static readonly string InvalidPatternFutansyaNoEmpty2 = "Invalid Kohi2 FutansyaNo Empty";
        public static readonly string InvalidPatternJyukyusyaNo2 = "Invalid Kohi2 JyukyusyaNo";
        public static readonly string InvalidPatternTokusyuNo2 = "Invalid Kohi2 TokusyuNo";
        public static readonly string InvalidPatternFutansyaNo02 = "Invalid Kohi2 FutansyaNo Equals 0";
        public static readonly string InvalidPatternKohiYukoDate2 = "Invalid Kohi2 YukoDate";
        public static readonly string InvalidPatternKohiHokenMstStartDate2 = "Invalid Kohi2 HokenMst StartDate";
        public static readonly string InvalidPatternKohiHokenMstEndDate2 = "Invalid Kohi2 HokenMst EndDate";
        public static readonly string InvalidPatternKohiConfirmDate2 = "Invalid Kohi2 ConfirmDate";
        public static readonly string InvalidPatternKohiMstCheckHBT2 = "Invalid Kohi2 HokenMst CheckHBT";
        public static readonly string InvalidPatternKohiMstCheckDigitFutansyaNo2 = "Invalid Kohi2 HokenMst CheckDigitFutansyaNo";
        public static readonly string InvalidPatternKohiHokenMstCheckDigitJyukyusyaNo2 = "Invalid Kohi2 HokenMst CheckDigitJyukyusyaNo";
        public static readonly string InvalidPatternKohiHokenMstCheckAge2 = "Invalid Kohi2 HokenMst CheckAge";
        public static readonly string InvalidPatternKohiHokenMstFutanJyoTokuNull2 = "Invalid Kohi2 HokenMst FutanJyoToku Is Null";
        public static readonly string InvalidPatternKohiEmptyModel3 = "Invalid Kohi3 Empty Model";
        public static readonly string InvalidPatternKohiHokenMstEmpty3 = "Invalid Kohi3 HokenMst Empty";
        public static readonly string InvalidPatternFutansyaNoEmpty3 = "Invalid Kohi3 FutansyaNo Empty";
        public static readonly string InvalidPatternJyukyusyaNo3 = "Invalid Kohi3 JyukyusyaNo";
        public static readonly string InvalidPatternTokusyuNo3 = "Invalid Kohi3 TokusyuNo";
        public static readonly string InvalidPatternFutansyaNo03 = "Invalid Kohi3 FutansyaNo Equals 0";
        public static readonly string InvalidPatternKohiYukoDate3 = "Invalid Kohi3 YukoDate";
        public static readonly string InvalidPatternKohiHokenMstStartDate3 = "Invalid Kohi3 HokenMst StartDate";
        public static readonly string InvalidPatternKohiHokenMstEndDate3 = "Invalid Kohi3 HokenMst EndDate";
        public static readonly string InvalidPatternKohiConfirmDate3 = "Invalid Kohi3 ConfirmDate";
        public static readonly string InvalidPatternKohiMstCheckHBT3 = "Invalid Kohi3 HokenMst CheckHBT";
        public static readonly string InvalidPatternKohiMstCheckDigitFutansyaNo3 = "Invalid Kohi3 HokenMst CheckDigitFutansyaNo";
        public static readonly string InvalidPatternKohiHokenMstCheckDigitJyukyusyaNo3 = "Invalid Kohi3 HokenMst CheckDigitJyukyusyaNo";
        public static readonly string InvalidPatternKohiHokenMstCheckAge3 = "Invalid Kohi3 HokenMst CheckAge";
        public static readonly string InvalidPatternKohiHokenMstFutanJyoTokuNull3 = "Invalid Kohi3 HokenMst FutanJyoToku Is Null";
        public static readonly string InvalidPatternKohiEmptyModel4 = "Invalid Kohi4 Empty Model";
        public static readonly string InvalidPatternKohiHokenMstEmpty4 = "Invalid Kohi4 HokenMst Empty";
        public static readonly string InvalidPatternFutansyaNoEmpty4 = "Invalid Kohi4 FutansyaNo Empty";
        public static readonly string InvalidPatternJyukyusyaNo4 = "Invalid Kohi4 JyukyusyaNo";
        public static readonly string InvalidPatternTokusyuNo4 = "Invalid Kohi4 TokusyuNo";
        public static readonly string InvalidPatternFutansyaNo04 = "Invalid Kohi4 FutansyaNo Equals 0";
        public static readonly string InvalidPatternKohiYukoDate4 = "Invalid Kohi4 YukoDate";
        public static readonly string InvalidPatternKohiHokenMstStartDate4 = "Invalid Kohi4 HokenMst StartDate";
        public static readonly string InvalidPatternKohiHokenMstEndDate4 = "Invalid Kohi4 HokenMst EndDate";
        public static readonly string InvalidPatternKohiConfirmDate4 = "Invalid Kohi4 ConfirmDate";
        public static readonly string InvalidPatternKohiMstCheckHBT4 = "Invalid Kohi4 HokenMst CheckHBT";
        public static readonly string InvalidPatternKohiMstCheckDigitFutansyaNo4 = "Invalid Kohi4 HokenMst CheckDigitFutansyaNo";
        public static readonly string InvalidPatternKohiHokenMstCheckDigitJyukyusyaNo4 = "Invalid Kohi4 HokenMst CheckDigitJyukyusyaNo";
        public static readonly string InvalidPatternKohiHokenMstCheckAge4 = "Invalid Kohi4 HokenMst CheckAge";
        public static readonly string InvalidPatternKohiHokenMstFutanJyoTokuNull4 = "Invalid Kohi4 HokenMst FutanJyoToku Is Null";
        public static readonly string InvalidPatternKohiEmpty21 = "Invalid Kohi1 Kohi2 Empty";
        public static readonly string InvalidPatternKohiEmpty31 = "Invalid Kohi3 Kohi1 Empty";
        public static readonly string InvalidPatternKohiEmpty32 = "Invalid Kohi3 Kohi2 Empty";
        public static readonly string InvalidPatternKohiEmpty41 = "Invalid Kohi4 Kohi1 Empty";
        public static readonly string InvalidPatternKohiEmpty42 = "Invalid Kohi4 Kohi2 Empty";
        public static readonly string InvalidPatternKohiEmpty43 = "Invalid Kohi4 Kohi3 Empty";
        public static readonly string InvalidPatternDuplicateKohi1 = "Invalid DuplicateKohi1";
        public static readonly string InvalidPatternDuplicateKohi2 = "Invalid DuplicateKohi2";
        public static readonly string InvalidPatternDuplicateKohi3 = "Invalid DuplicateKohi3";
        public static readonly string InvalidPatternDuplicateKohi4 = "Invalid DuplicateKohi4";
        public static readonly string InvalidPatternWarningDuplicatePattern = "Invalid Warning Duplicate Pattern";
        public static readonly string InvalidPatternWarningAge75 = "Invalid Warning Age75";
        public static readonly string InvalidPatternWarningAge65 = "Invalid Warning Age65";
        public static readonly string InvalidPatternMaruchoOnly = "Invalid Marucho Only";

        //KarteInf controller
        public static readonly string GetKarteInfInvalidRaiinNo = "Invalid RaiinNo";
        public static readonly string GetKarteInfInvalidPtId = "Invalid PtId";
        public static readonly string GetKarteInfInvalidSinDate = "Invalid SinDate";
        public static readonly string GetKarteInfNoData = "No Data";
        public static readonly string GetKarteInfSuccessed = "Successed";
        public static readonly string UpsertKarteInfInvalidHpId = "Invalid HpId";
        public static readonly string UpsertKarteInfInvalidRaiinNo = "Invalid RaiinNo";
        public static readonly string UpsertKarteInfInvalidKarteKbn = "Invalid KarteKbn";
        public static readonly string UpsertKarteInfInvalidPtId = "Invalid PtId";
        public static readonly string UpsertKarteInfInvalidSinDate = "Invalid SinDate";
        public static readonly string UpsertKarteInfInvalidIsDeleted = "Invalid IsDeleted";
        public static readonly string UpsertKarteInfRaiinNoNoExist = "RaiinNo No Exist";
        public static readonly string UpsertKarteInfPtIdNoExist = "PtId No Exist";
        public static readonly string UpsertKarteInfKarteKbnNoExist = "KarteKbn No Exist";

        //OrdInf controller
        public static readonly string GetOrdInfInvalidRaiinNo = "Invalid RaiinNo";
        public static readonly string GetOrdInfInvalidHpId = "Invalid HpId";
        public static readonly string GetOrdInfInvalidPtId = "Invalid PtId";
        public static readonly string GetOrdInfInvalidSinDate = "Invalid SinDate";
        public static readonly string GetOrdInfNoData = "No Data";
        public static readonly string GetOrdInfSuccessed = "Successed";

        //RaiinKubun controller
        public static readonly string RaiinKubunInvalidKbnInf = "Invalid KbnInf";

        //Calculation Inf

        //Medical examination controller
        public static readonly string GetMedicalExaminationInvalidPtId = "Invalid PtId";
        public static readonly string GetMedicalExaminationInvalidHpId = "Invalid HpId";
        public static readonly string GetMedicalExaminationInvalidStartIndex = "Invalid Start Index Of Page";
        public static readonly string GetMedicalExaminationInvalidSinDate = "Invalid SinDate";
        public static readonly string GetMedicalExaminationNoData = "No Data";
        public static readonly string GetMedicalExaminationSuccessed = "Successed";
        public static readonly string GetMedicalExaminationInvalidDeleteCondition = "Invalid Delete Condition";
        public static readonly string GetMedicalExaminationInvalidFilterId = "Invalid FilterId";
        public static readonly string GetMedicalExaminationInvalidPageSize = "Invalid PageSize";
        public static readonly string GetMedicalExaminationInvalidSearchType = "Invalid Search Type";
        public static readonly string GetMedicalExaminationInvalidSearchCategory = "Invalid Search Category";
        public static readonly string GetMedicalExaminationInvalidSearchText = "Invalid Search Text";
        public static readonly string GetMedicalExaminationInvalidUserId = "Invalid UserId";
        public static readonly string GetMedicalExaminationInvalidDrugOrByomei = "DrugOrders Or Byomeis Is Null";
        public static readonly string GetMedicalExaminationInvalidByomei = "Byomeis Is Null";

        //OrdInf controller

        //RaiinKubun controller

        //TimeZone
        public static readonly string InvalidBirthDay = "Invalid BirthDay";
        public static readonly string InvalidCurrentTimeKbn = "Invalid CurrentTimeKbn, CurrentTimeKbn > 0";
        public static readonly string InvalidBeforeTimeKbn = "Invalid BeforeTimeKbn, BeforeTimeKbn >= 0";
        public static readonly string InvalidUketukeTime = "Invalid UketukeTime";
        public static readonly string CanNotUpdateTimeZoneInf = "CurrentTimeKbn = BeforeTimeKbn, Can Not Update TimeZoneInf";

        //SetMst
        public static readonly string GetSetListInvalidHpId = "Invalid HpId";
        public static readonly string GetSetListSinDate = "Invalid SinDate";
        public static readonly string GetSetListInvalidSetKbn = "Invalid SetKbn";
        public static readonly string GetSetListInvalidSetKbnEdaNo = "Invalid SetKbnEdaNo";
        public static readonly string GetSetListNoData = "No Data";
        public static readonly string GetSetListSuccessed = "Successed";
        public static readonly string InvalidLevel = "Invalid Level, can't move";
        public static readonly string InvalidUserId = "Invalid UserId, can't move";
        public static readonly string InvalidCopySetCd = "Invalid CopySetCd, CopySetCd >= 0";
        public static readonly string InvalidPasteSetCd = "Invalid PasteSetCd, PasteSetCd > 0";
        public static readonly string InvalidDragSetCd = "Invalid DragSetCd, DragSetCd >= 0";
        public static readonly string InvalidDropSetCd = "Invalid DropSetCd, DropSetCd > 0";
        public static readonly string InvalidSetCd = "Invalid SetCd, this SetCd is not exist.";
        public static readonly string InvalidSetKbn = "Invalid SetKbn, SetKbn >= 1 and SetKbn <= 10";
        public static readonly string InvalidSetKbnEdaNo = "Invalid SetKbnEdaNo, SetKbnEdaNo >= 1 and SetKbnEdaNo <= 6";
        public static readonly string InvalidGenarationId = "Invalid GenarationId, GenarationId >= 0";
        public static readonly string InvalidLevel1 = "Invalid Level1, Level1 > 0";
        public static readonly string InvalidLevel2 = "Invalid Level2, Level2 >= 0";
        public static readonly string InvalidLevel3 = "Invalid Level3, Level3 >= 0";
        public static readonly string InvalidSetName = "Invalid SetName, SetName maxlength is 60";
        public static readonly string InvalidWeightKbn = "Invalid WeightKbn, WeightKbn >= 0";
        public static readonly string InvalidColor = "Invalid Color, Color >= 0";
        public static readonly string InvalidMemo = "Invalid Memo";

        //Set
        public static readonly string GetSetKbnListInvalidHpId = "Invalid HpId";
        public static readonly string GetSetKbnListSinDate = "Invalid SinDate";
        public static readonly string GetSetKbnListInvalidSetKbnFrom = "Invalid SetKbnFrom";
        public static readonly string GetSetKbnListInvalidSetKbnTo = "Invalid SetKbnTo";
        public static readonly string GetSetKbnListInvalidSetKbn = "SetKbnTo must more than SetKbnFrom";
        public static readonly string GetSetKbnListNoData = "No Data";
        public static readonly string GetSetKbntListSuccessed = "Successed";
        public static readonly string SaveSetByomeiFailed = "Save SetByomei Failed.";
        public static readonly string SaveSetOrderInfFailed = "Save SetOrderInf Failed.";
        public static readonly string SaveSetKarteInfFailed = "Save SetKarteInf Failed.";
        //Calculation Inf


        // Visiting controller
        //  - UpdateStaticCell
        public static readonly string UpdateReceptionStaticCellUnknownError = "Failed to update cell value.";
        public static readonly string UpdateReceptionStaticCellSuccess = "Cell value updated successfully.";
        public static readonly string UpdateReceptionStaticCellInvalidHpId = "HpId must be greater than 0.";
        public static readonly string UpdateReceptionStaticCellInvalidSinDate = "SinDate must be greater than 0.";
        public static readonly string UpdateReceptionStaticCellInvalidRaiinNo = "RaiinNo must be greater than 0.";
        public static readonly string UpdateReceptionStaticCellInvalidPtId = "PtId must be greater than 0.";
        //  - UpdateDynamicCell
        public static readonly string UpdateReceptionDynamicCellSuccess = "Cell value updated successfully.";
        public static readonly string UpdateReceptionDynamicCellInvalidHpId = "HpId must be greater than 0.";
        public static readonly string UpdateReceptionDynamicCellInvalidSinDate = "SinDate must be greater than 0.";
        public static readonly string UpdateReceptionDynamicCellInvalidRaiinNo = "RaiinNo must be greater than 0.";
        public static readonly string UpdateReceptionDynamicCellInvalidPtId = "PtId must be greater than 0.";
        public static readonly string UpdateReceptionDynamicCellInvalidGrpId = "GrpId cannot be negative.";

        //Flowsheet
        public static readonly string UpsertFlowSheetSuccess = "Upsert value successfully.";
        public static readonly string UpsertFlowSheetInvalidPtId = "PtId must be greater than 0.";
        public static readonly string UpsertFlowSheetInvalidSinDate = "SinDate is no valid.";
        public static readonly string UpsertFlowSheetInvalidRaiinNo = "RaiinNo must be greater than 0.";
        public static readonly string UpsertFlowSheetInvalidTagNo = "TagNo is no valid";
        public static readonly string UpsertFlowSheetUpdateNoSuccess = "Update is no successful.";
        public static readonly string UpsertFlowSheetInputDataNoValid = "Input data no valid.";
        public static readonly string UpsertFlowSheetRainNoNoExist = "RainNo No Exist.";
        public static readonly string UpsertFlowSheetPtIdNoExist = "PtId No Exist.";

        // Schema
        public static readonly string InvalidOldImage = "Invalid old image.";
        public static readonly string DeleteSuccessed = "Delete image successed.";
        public static readonly string InvalidFileImage = "File image is not null.";
        public static readonly string InvalidTypeUpload = "Invalid type upload.";

        // Today Validate Order
        public static readonly string TodayOrdInvalidSpecialItem = "Special item doesn't contain drug, injection and other";
        public static readonly string TodayOrdIInvalidSpecialStadardUsage = "Special item doesn't contain standard usage";
        public static readonly string TodayOrdInvalidOdrKouiKbn = "Value of OdrKouiKbn is invalid ";
        public static readonly string TodayOrdInvalidSpecialSuppUsage = "Special item doesn't contain supply usage";
        public static readonly string TodayOrdInvalidHasUsageButNotDrug = "Item which differs drug item, it doesn't have drug usage";
        public static readonly string TodayOrdInvalidHasUsageButNotInjectionOrDrug = "Item which differs drug item or injection item, it doesn't have injection usage";
        public static readonly string TodayOrdInvalidHasDrugButNotUsage = "Drug item doesn't have usage";
        public static readonly string TodayOrdInvalidHasInjectionButNotUsage = "Injection item doesn't have usage";
        public static readonly string TodayOrdInvalidHasNotBothInjectionAndUsageOf28 = "Self Injection doesn't have self injection detail and usage";
        public static readonly string TodayOrdInvalidStandardUsageOfDrugOrInjection = "Standard usage of drug item or usage of injection item don't more than 1";
        public static readonly string TodayOrdInvalidSuppUsageOfDrugOrInjection = "Supply usage of drug item or usage of injection item don't more than 1";
        public static readonly string TodayOrdInvalidBunkatu = "Bunkatu item of drug item doesn't more than 1";
        public static readonly string TodayOrdInvalidUsageWhenBuntakuNull = "Bunkatu item doesn't have usage";
        public static readonly string TodayOrdInvalidSumBunkatuDifferentSuryo = "Bunkatu item has sum of suryo not equal bunkatu";
        public static readonly string TodayOrdInvalidQuantityUnit = "Has unit but doesn't have quantity";
        public static readonly string TodayOrdInvalidSuryoAndYohoKbnWhenDisplayedUnitNotNull = "Has unit but yohoKbn and Suryo don't invalid (YohoKbn != 1 and Suryo > 999)";
        public static readonly string TodayOrdInvalidSuryoBunkatuWhenIsCon_TouyakuOrSiBunkatu = "Bunkatu item doesn't have suryo and bunkatu";
        public static readonly string TodayOrdInvalidPrice = "Price must more than 0 and (suryo * price) <= 999999999";
        public static readonly string TodayOrdInvalidCmt840 = "CmtOpt is not null and CmtName is not null when CmtCol1 of Cmt840 > 0";
        public static readonly string TodayOrdInvalidCmt842 = "CmtOpt of Cmt842 is not null and CmtName is not null";
        public static readonly string TodayOrdInvalidCmt842CmtOptMoreThan38 = "CmtOpt of Cmt842 is not null and has length less than or equal 38";
        public static readonly string TodayOrdInvalidCmt830CmtOpt = "CmtOpt of Cmt830 is not null and not white space";
        public static readonly string TodayOrdInvalidCmt830CmtOptMoreThan38 = "CmtOpt of Cmt830 is not null and has length less than or equal 38";
        public static readonly string TodayOrdInvalidCmt831 = "CmtOpt of Cmt831 is not null and CmtName is not null";
        public static readonly string TodayOrdInvalidCmt850Date = "CmtOpt of Cmt850 is not map format and CmtName is not null when CmtName contain day";
        public static readonly string TodayOrdInvalidCmt850OtherDate = "CmtOpt of Cmt850 is not map format and CmtName is not null when CmtName doesn't contain day";
        public static readonly string TodayOrdInvalidCmt851 = "CmtOpt of Cmt851 is not map format and CmtName is not null";
        public static readonly string TodayOrdInvalidCmt852 = "CmtOpt of Cmt852 is not map format and CmtName is not null";
        public static readonly string TodayOrdInvalidCmt853 = "CmtOpt of Cmt853 is not map format and CmtName is not null";
        public static readonly string TodayOrdInvalidCmt880 = "CmtOpt of Cmt880 is not null and CmtName is not null";
        public static readonly string TodayOrdDuplicateTodayOrd = "Duplicate RpNo and RpNoEdaNo";
        public static readonly string TodayOrdInvalidKohatuKbn = "Value of KohatuKbn is not invalid";
        public static readonly string TodayOrdInvalidDrugKbn = "Value of DrugKbn is not invalid";
        public static readonly string TodayOrdInvalidSuryoOfReffill = "Suryo must  more than refill setting";
        public static readonly string TodayOrdInvalidRowNo = "RowNo must more than 0";
        public static readonly string TodayOrdInvalidSinKouiKbn = "SinKouiKbn must more than 0 or equal 0";
        public static readonly string TodayOrdInvalidItemCd = "Length of ItemCd must less than 10 or equal 10";
        public static readonly string TodayOrdInvalidItemName = "Length of ItemName must less than 240 or equal 240";
        public static readonly string TodayOrdInvalidSuryo = "Invalid Suryo";
        public static readonly string TodayOrdInvalidUnitName = "Length of UnitName must less than 24 or equal 24";
        public static readonly string TodayOrdInvalidUnitSbt = "UnitSbt must more than 0 or equal 0 and less than 2 or equal 2";
        public static readonly string TodayOrdInvalidTermVal = "TermVal must more than 0 or equal 0";
        public static readonly string TodayOrdInvalidSyohoKbn = "SyohoKbn must more than 0 or equal 0 and less than 3 or equal 3";
        public static readonly string TodayOrdInvalidSyohoLimitKbn = "SyohoLimitKbn must more than 0 or equal 0 and less than 3 or equal 3";
        public static readonly string TodayOrdInvalidYohoKbn = "YohoKbn must more than 0 or equal 0 and less than 2 or equal 2";
        public static readonly string TodayOrdInvalidIsNodspRece = "IsNodspRece  must more than 0 or equal 0 and less than 1 or equal 1";
        public static readonly string TodayOrdInvalidIpnCd = "Length of IpnCd must less than 12 or equal 12";
        public static readonly string TodayOrdInvalidIpnName = "Length of IpnName must less than 120 or equal 120";
        public static readonly string TodayOrdInvalidJissiKbn = "JissiKbn must more than 0 or equal 0 and less than 1 or equal 1";
        public static readonly string TodayOrdInvalidJissiId = "JissiId must more than 0 equal 0";
        public static readonly string TodayOrdInvalidJissiMachine = "Length of JissiMachine must less than 60 or equal 60";
        public static readonly string TodayOrdInvalidReqCd = "Length of ReqCd must less than 10 or equal 10";
        public static readonly string TodayOrdInvalidCmtName = "Length of CmtName must less than 240 or equal 240";
        public static readonly string TodayOrdInvalidCmtOpt = "Length of CmtOpt must less than 38 or equal 38";
        public static readonly string TodayOrdInvalidFontColor = "Length of FontColor must less than 8 or equal 8";
        public static readonly string TodayOrdInvalidCommentNewline = "CommentNewline must more than 0 or equal 0 and less than 1 or equal 1";
        public static readonly string TodayOrdInvalidRpNo = "RpNo must more than 0";
        public static readonly string TodayOrdInvalidRpEdaNo = "RpEdaNo must more than 0";
        public static readonly string TodayOrdInvalidHokenPId = "HokenPId must more than 0";
        public static readonly string TodayOrdInvalidRpName = "Length of RpName must less than 240 or equal 240";
        public static readonly string TodayOrdInvalidInoutKbn = "InoutKbn must more than 0 or equal 0 and less than 1 or equal 1";
        public static readonly string TodayOrdInvalidSikyuKbn = "SikyuKbn must more than 0 or equal 0 and less than 1 or equal 1";
        public static readonly string TodayOrdInvalidSyohoSbt = "SyohoSbt must more than 0 or equal 0 and less than 2 or equal 2";
        public static readonly string TodayOrdInvalidSanteiKbn = "SanteiKbn must more than 0 or equal 0 and less than 2 or equal 2";
        public static readonly string TodayOrdInvalidTosekiKbn = "TosekiKbn must more than 0 or equal 0 and less than 2 or equal 2";
        public static readonly string TodayOrdInvalidDaysCnt = "DaysCnt must more than 0 or equal 0";
        public static readonly string TodayOrdInvalidSortNo = "SortNo must more than 0";
        public static readonly string TodayOrdInvalidId = "Id of OrdInf must more than 0 or equal 0";
        public static readonly string TodayOrdInvalidPtId = "PtId must more than 0";
        public static readonly string TodayOrdInvalidRaiinNo = "RaiinNo must more than 0";
        public static readonly string TodayOrdInvalidSinDate = "SinDate must more than 0";
        public static readonly string TodayOrdInvalidHpId = "HpId must more than 0";
        public static readonly string TodayOrdInvalidBunkatuLength = "Length of Bunkatu must lest than 10 or equal 10";
        public static readonly string TodayOrdInvalidIsDeleted = "IsDeleted must more than 0 or equal 0 and less than 2 or equal 2";
        public static readonly string TodayOrdInvalidInsertedExist = "This Rp has been exited";
        public static readonly string TodayOrdInvalidUpdatedNoExist = "This Rp hasn't been exited to update";
        public static readonly string TodayOrdInvalidAddedAutoItem = "Invalid Added Auto Item List";

        // SuperSetDetail
        public static readonly string InvalidSetByomeiId = "Invalid SetByomeiId, SetByomeiId > 0.";
        public static readonly string InvalidSikkanKbn = "Invalid SikkanKbn, SikkanKbn >.0";
        public static readonly string InvalidNanByoCd = "Invalid NanByoCd, NanByoCd > 0.";
        public static readonly string InvalidByomeiCdOrSyusyokuCd = "Invalid ByomeiCd or SyusyokuCd, ByomeiCd or SyusyokuCd not found.";
        public static readonly string SetCdNotExist = "SetCd Not Exist.";
        public static readonly string FullByomeiMaxlength160 = "Length of FullByomei must less than 160 or equal 160.";
        public static readonly string ByomeiCmtMaxlength80 = "Length of ByomeiCmt must less than 80 or equal 80.";
        public static readonly string RpNameMaxLength240 = "Length of SetOrder RpName must less than 240 or equal 240.";
        public static readonly string InvalidSetOrderInfId = "Id of SetOrderInf must more than 0 or equal 0";
        public static readonly string InvalidSetOrderInfRpNo = "SetOrder RpNo must more than 0 or equal 0";
        public static readonly string InvalidSetOrderInfRpEdaNo = "SetOrder RpEdaNo must more than 0 or equal 0";
        public static readonly string InvalidSetOrderInfKouiKbn = "SetOrder KouiKbn must more than 0 or equal 0";
        public static readonly string InvalidSetOrderInfInoutKbn = "SetOrder InoutKbn must more than 0 or equal 0";
        public static readonly string InvalidSetOrderInfSikyuKbn = "SetOrder SikyuKbn must more than 0 or equal 0";
        public static readonly string InvalidSetOrderInfSyohoSbt = "SetOrder SyohoSbt must more than 0 or equal 0";
        public static readonly string InvalidSetOrderInfSanteiKbn = "SetOrder SanteiKbn must more than 0 or equal 0";
        public static readonly string InvalidSetOrderInfTosekiKbn = "SetOrder TosekiKbn must more than 0 or equal 0";
        public static readonly string InvalidSetOrderInfDaysCnt = "SetOrder DaysCnt must more than 0 or equal 0";
        public static readonly string InvalidSetOrderInfSortNo = "SetOrder SortNo must more than 0 or equal 0";
        public static readonly string InvalidSetOrderSinKouiKbn = "SetOrderDetail SinKouiKbn must more than 0 or equal 0";
        public static readonly string ItemCdMaxLength10 = "Length of SetOrderDetail ItemCd must less than 10 or equal 10.";
        public static readonly string ItemNameMaxLength240 = "Length of SetOrderDetail ItemCd must less than 240 or equal 240.";
        public static readonly string UnitNameMaxLength24 = "Length of SetOrderDetail UnitName must less than 24 or equal 24.";
        public static readonly string InvalidSetOrderSuryo = "SetOrderDetail Suryo must more than 0 or equal 0";
        public static readonly string InvalidSetOrderUnitSBT = "SetOrderDetail UnitSBT must more than 0 or equal 0";
        public static readonly string InvalidSetOrderTermVal = "SetOrderDetail TermVal must more than 0 or equal 0";
        public static readonly string InvalidSetOrderKohatuKbn = "SetOrderDetail KohatuKbn must more than 0 or equal 0";
        public static readonly string InvalidSetOrderSyohoKbn = "SetOrderDetail SyohoKbn must more than 0 or equal 0";
        public static readonly string InvalidSetOrderSyohoLimitKbn = "SetOrderDetail SyohoLimitKbn must more than 0 or equal 0";
        public static readonly string InvalidSetOrderDrugKbn = "SetOrderDetail DrugKbn must more than 0 or equal 0";
        public static readonly string InvalidSetOrderYohoKbn = "SetOrderDetail YohoKbn must more than 0 or equal 0";
        public static readonly string Kokuji1MaxLength1 = "Length of SetOrderDetail Kokuji1 must less than 1 or equal 1.";
        public static readonly string Kokuji2MaxLength1 = "Length of SetOrderDetail Kokuji2 must less than 1 or equal 1.";
        public static readonly string InvalidSetOrderIsNodspRece = "SetOrderDetail IsNodspRece must more than 0 or equal 0";
        public static readonly string IpnCdMaxLength12 = "Length of SetOrderDetail IpnCd must less than 12 or equal 12.";
        public static readonly string IpnNameMaxLength120 = "Length of SetOrderDetail IpnName must less than 120 or equal 120.";
        public static readonly string BunkatuMaxLength10 = "Length of SetOrderDetail Bunkatu must less than 10 or equal 10.";
        public static readonly string CmtNameMaxLength240 = "Length of SetOrderDetail CmtName must less than 240 or equal 240.";
        public static readonly string CmtOptMaxLength38 = "Length of SetOrderDetail CmtOpt must less than 38 or equal 38.";
        public static readonly string FontColorMaxLength8 = "Length of SetOrderDetail FontColor must less than 8 or equal 8.";
        public static readonly string InvalidSetOrderCommentNewline = "SetOrderDetail CommentNewline must more than 0 or equal 0";
        public static readonly string RpNoOrRpEdaNoIsNotExist = "RpNo or RpEdaNo is not exist";

        // KaMst
        public static readonly string InvalidKaId = "Invalid KaId, KaId > 0";
        public static readonly string KaSnameMaxLength20 = "Length of KaSname must lest than 20 or equal 20";
        public static readonly string KaNameMaxLength40 = "Length of KaName must lest than 40 or equal 40";
        public static readonly string ReceKaCdNotFound = "ReceKaCd is NotFound";
        public static readonly string CanNotDuplicateKaId = "Can not duplicate KaId";

        //UketukeMst
        public static readonly string InvalidKbnId = "InvalidKbnId";
        public static readonly string InvalidKbnName = "InvalidKbnName";
        public static readonly string InputNoData = "InputNoData";
        public static readonly string UketukeListExistedInputData = "UketukeListExistedInputData";
        public static readonly string UketukeListInvalidExistedKbnId = "UketukeListInvalidExistedKbnId";

        //TodayOdr Field
        public static readonly string TodayOdrSuryo = "Suryo";
        public static readonly string TodayOdrCmt = "CmtOpt,CmtName";
        public static readonly string TodayOdrCmt842_830 = "CmtOpt";
        public static readonly string TodayOdrKohatuKbn = "KohatuKbn";
        public static readonly string TodayOdrDrugKbn = "DrugKbn";
        public static readonly string TodayOdrId = "Id";
        public static readonly string TodayOdrHpId = "HpId";
        public static readonly string TodayOdrRaiinNo = "RaiinNo";
        public static readonly string TodayOdrRpNo = "RpNo";
        public static readonly string TodayOdrRpEdaNo = "RpEdaNo";
        public static readonly string TodayOdrPtId = "PtId";
        public static readonly string TodayOdrSinDate = "SinDate";
        public static readonly string TodayOdrHokenPid = "HokenPId";
        public static readonly string TodayOdrRpName = "RpName";
        public static readonly string TodayOdrInOutKbn = "InOutKbn";
        public static readonly string TodayOdrSikyuKbn = "SikyuKbn";
        public static readonly string TodayOdrSyohoSbt = "SyohoSbt";
        public static readonly string TodayOdrSanteiKbn = "SanteiKbn";
        public static readonly string TodayOdrTosekiKbn = "TosekiKbn";
        public static readonly string TodayOdrDaysCnt = "DaysCnt";
        public static readonly string TodayOdrSortNo = "SortNo";
        public static readonly string TodayOdrRowNo = "RowNo";
        public static readonly string TodayOdrSinKouiKbn = "SinKouiKbn";
        public static readonly string TodayOdrItemCd = "ItemCd";
        public static readonly string TodayOdrItemName = "ItemName";
        public static readonly string TodayOdrUnitName = "UnitName";
        public static readonly string TodayOdrUnitSbt = "UnitSbt";
        public static readonly string TodayOdrTermVal = "TermVal";
        public static readonly string TodayOdrSyohoKbn = "SyohoKbn";
        public static readonly string TodayOdrSyohoLimitKbn = "SyohoLimitKbn";
        public static readonly string TodayOdrYohoKbn = "YohoKbn";
        public static readonly string TodayOdrIsNodspRece = "IsNodspRece";
        public static readonly string TodayOdrIpnCd = "IpnCd";
        public static readonly string TodayOdrIpnName = "IpnName";
        public static readonly string TodayOdrJissiKbn = "JissiKbn";
        public static readonly string TodayOdrJissiId = "JissiId";
        public static readonly string TodayOdrJissiMachine = "JissiMachine";
        public static readonly string TodayOdrReqCd = "ReqCd";
        public static readonly string TodayOdrBunkatu = "Bunkatu";
        public static readonly string TodayOdrCmtName = "CmtName";
        public static readonly string TodayOdrCmtOpt = "CmtOpt";
        public static readonly string TodayOdrFontColor = "FontColor";
        public static readonly string TodayOdrCommentNewline = "CommentNewline";
        public static readonly string TodayOdrIsDeleted = "IsDeleted";
        public static readonly string TodayOdrSuryoYohoKbn = "Suryo,YohoKbn";
        public static readonly string TodayOdrSuryoBunkatu = "Suryo,Bunkatu";
        public static readonly string TodayOdrPriceSuryo = "Suryo,Price";

        //Raiin Info TodayOdr
        public static readonly string RaiinInfTodayOdrInvalidSyosaiKbn = "Invalid SyosaiKbn";
        public static readonly string RaiinInfTodayOdrInvalidJikanKbn = "Invalid JikanKbn";
        public static readonly string RaiinInfTodayOdrInvalidHokenPid = "Invalid HokenPid";
        public static readonly string RaiinInfTodayOdrHokenPidNoExist = "HokenPid no exist";
        public static readonly string RaiinInfTodayOdrInvalidSanteiKbn = "Invalid SanteiKbn";
        public static readonly string RaiinInfTodayOdrInvalidTantoId = "Invalid TantoId";
        public static readonly string RaiinInfTodayOdrTatoIdNoExist = "TantoId no exist";
        public static readonly string RaiinInfTodayOdrInvalidKaId = "Invalid KaId";
        public static readonly string RaiinInfTodayOdrKaIdNoExist = "KaId no exist";
        public static readonly string RaiinInfTodayOdrInvalidUKetukeTime = "Invalid UKetukeTime";
        public static readonly string RaiinInfTodayOdrInvalidSinStartTime = "Invalid SinStartTime";
        public static readonly string RaiinInfTodayOdrInvalidSinEndTime = "Invalid SinEndTime";
        public static readonly string RaiinInfTodayOdrPtIdNoExist = "PtId no exist";
        public static readonly string RaiinInfTodayOdrHpIdNoExist = "HpId no exist";
        public static readonly string RaiinInfTodayOdrRaiinNoExist = "RaiinNo no exist";

        //Monshin
        public static readonly string InputDataDoesNotExists = "Input Data does not exist";

        //Alrgy Drug
        public static readonly string AddAlrgyDrugInvalidCmt = "Invalid Cmt";
        public static readonly string AddAlrgyDrugInvalidPtId = "Invalid PtId";
        public static readonly string AddAlrgyDrugHpIdNoExist = "No exist HpId";
        public static readonly string AddAlrgyDrugPtIdNoExist = "No exist PtId";
        public static readonly string AddAlrgyDrugInvalidItemCd = "Invalid ItemCd";
        public static readonly string AddAlrgyDrugInvalidSortNo = "Invalid SortNo";
        public static readonly string AddAlrgyDrugInvalidStartDate = "Invalid StartDate";
        public static readonly string AddAlrgyDrugInvalidEndDate = "Invalid EndDate";
        public static readonly string AddAlrgyDrugInvalidDrugName = "Invalid DrugName";
        public static readonly string AddAlrgyDrugDuplicate = "This drug has existed";
        public static readonly string AddAlrgyDrugInputNoData = "Hasn't input data";
        public static readonly string AddAlrgyDrugItemCd = "ItemCd no existed";

        //HokenSyamst
        public static readonly string InvalidHokenSyaNo = "HokenSyaNo is null or empty";

        //DetailHokenMst
        public static readonly string DetailHokenMstInvalidHokenEdaNo = "HokenEdaNo is not valid";
        public static readonly string DetailHokenMstInvalidHokenNo = "HokenNo is not valid";
        public static readonly string DetailHokenMstInvalidPrefNo = "PrefNo is not valid";

        //PostCode
        public static readonly string InvalidPostCode = "Invalid PostCode";

        // Export 
        public static readonly string PtInfNotFould = "Invalid PtId, PtInf Not Fould.";
        public static readonly string HokenNotFould = "Invalid HokenPid, Hoken Not Fould.";
        public static readonly string CanNotExportPdf = "Can not export file Pdf.";
        public static readonly string CanNotReturnPdfFile = "Can not return file Pdf.";

        //Valid Kohi
        public static readonly string InvalidKohiEmptyModel1 = "Invalid kohi1 empty model";
        public static readonly string InvalidKohiHokenMstEmpty1 = "Invalid kohi1 hokenMst empty model";
        public static readonly string InvalidFutansyaNoEmpty1 = "Invalid kohi1 futansyaNo empty";
        public static readonly string InvalidJyukyusyaNo1 = "Invalid kohi1 jyukyusyaNo empty";
        public static readonly string InvalidTokusyuNo1 = "Invalid kohi1 tokusyuNo empty";
        public static readonly string InvalidFutansyaNo01 = "Invalid kohi1 futansyaNo equal 0";
        public static readonly string InvalidKohiYukoDate1 = "Invalid kohi1 YukoDate";
        public static readonly string InvalidKohiHokenMstStartDate1 = "Invalid kohi1 hokenMst startDate";
        public static readonly string InvalidKohiHokenMstEndDate1 = "Invalid kohi1 hokenMst endDate";
        public static readonly string InvalidKohiConfirmDate1 = "Invalid kohi1 hokenMst confirmDate";
        public static readonly string InvalidMstCheckHBT1 = "Invalid check HBT";
        public static readonly string InvalidMstCheckDigitFutansyaNo1 = "Invalid kohi1 check degit futansyaNo";
        public static readonly string InvalidMstCheckDigitJyukyusyaNo1 = "Invalid kohi1 check degit jyukyusyaNo";
        public static readonly string InvalidMstCheckAge1 = "Invalid kohi1 check date hokenMst age";
        public static readonly string InvalidFutanJyoTokuNull1 = "Invalid kohi1 futansyaNo jyukyusyaNo tokusyuNo null";
        public static readonly string InvalidKohiEmptyModel2 = "Invalid kohi2 empty model";
        public static readonly string InvalidKohiHokenMstEmpty2 = "Invalid kohi2 hokenMst empty model";
        public static readonly string InvalidFutansyaNoEmpty2 = "Invalid kohi2 futansyaNo empty";
        public static readonly string InvalidJyukyusyaNo2 = "Invalid kohi2 jyukyusyaNo empty";
        public static readonly string InvalidTokusyuNo2 = "Invalid kohi2 tokusyuNo empty";
        public static readonly string InvalidFutansyaNo02 = "Invalid kohi2 futansyaNo equal 0";
        public static readonly string InvalidKohiYukoDate2 = "Invalid kohi2 YukoDate";
        public static readonly string InvalidKohiHokenMstStartDate2 = "Invalid kohi2 hokenMst startDate";
        public static readonly string InvalidKohiHokenMstEndDate2 = "Invalid kohi2 hokenMst endDate";
        public static readonly string InvalidKohiConfirmDate2 = "Invalid kohi2 hokenMst confirmDate";
        public static readonly string InvalidMstCheckHBT2 = "Invalid check HBT";
        public static readonly string InvalidMstCheckDigitFutansyaNo2 = "Invalid kohi2 check degit futansyaNo";
        public static readonly string InvalidMstCheckDigitJyukyusyaNo2 = "Invalid kohi2 check degit jyukyusyaNo";
        public static readonly string InvalidMstCheckAge2 = "Invalid kohi2 check date hokenMst age";
        public static readonly string InvalidFutanJyoTokuNull2 = "Invalid kohi2 futansyaNo jyukyusyaNo tokusyuNo null";
        public static readonly string InvalidKohiEmptyModel3 = "Invalid kohi3 empty model";
        public static readonly string InvalidKohiHokenMstEmpty3 = "Invalid kohi3 hokenMst empty model";
        public static readonly string InvalidFutansyaNoEmpty3 = "Invalid kohi3 futansyaNo empty";
        public static readonly string InvalidJyukyusyaNo3 = "Invalid kohi3 jyukyusyaNo empty";
        public static readonly string InvalidTokusyuNo3 = "Invalid kohi3 tokusyuNo empty";
        public static readonly string InvalidFutansyaNo03 = "Invalid kohi3 futansyaNo equal 0";
        public static readonly string InvalidKohiYukoDate3 = "Invalid kohi3 YukoDate";
        public static readonly string InvalidKohiHokenMstStartDate3 = "Invalid kohi3 hokenMst startDate";
        public static readonly string InvalidKohiHokenMstEndDate3 = "Invalid kohi3 hokenMst endDate";
        public static readonly string InvalidKohiConfirmDate3 = "Invalid kohi3 hokenMst confirmDate";
        public static readonly string InvalidMstCheckHBT3 = "Invalid check HBT";
        public static readonly string InvalidMstCheckDigitFutansyaNo3 = "Invalid kohi3 check degit futansyaNo";
        public static readonly string InvalidMstCheckDigitJyukyusyaNo3 = "Invalid kohi3 check degit jyukyusyaNo";
        public static readonly string InvalidMstCheckAge3 = "Invalid kohi3 check date hokenMst age";
        public static readonly string InvalidFutanJyoTokuNull3 = "Invalid kohi3 futansyaNo jyukyusyaNo tokusyuNo null";
        public static readonly string InvalidKohiEmptyModel4 = "Invalid kohi4 empty model";
        public static readonly string InvalidKohiHokenMstEmpty4 = "Invalid kohi4 hokenMst empty model";
        public static readonly string InvalidFutansyaNoEmpty4 = "Invalid kohi4 futansyaNo empty";
        public static readonly string InvalidJyukyusyaNo4 = "Invalid kohi4 jyukyusyaNo empty";
        public static readonly string InvalidTokusyuNo4 = "Invalid kohi4 tokusyuNo empty";
        public static readonly string InvalidFutansyaNo04 = "Invalid kohi4 futansyaNo equal 0";
        public static readonly string InvalidKohiYukoDate4 = "Invalid kohi4 YukoDate";
        public static readonly string InvalidKohiHokenMstStartDate4 = "Invalid kohi4 hokenMst startDate";
        public static readonly string InvalidKohiHokenMstEndDate4 = "Invalid kohi4 hokenMst endDate";
        public static readonly string InvalidKohiConfirmDate4 = "Invalid kohi4 hokenMst confirmDate";
        public static readonly string InvalidMstCheckHBT4 = "Invalid check HBT";
        public static readonly string InvalidMstCheckDigitFutansyaNo4 = "Invalid kohi4 check degit futansyaNo";
        public static readonly string InvalidMstCheckDigitJyukyusyaNo4 = "Invalid kohi4 check degit jyukyusyaNo";
        public static readonly string InvalidMstCheckAge4 = "Invalid kohi4 check date hokenMst age";
        public static readonly string InvalidFutanJyoTokuNull4 = "Invalid kohi4 futansyaNo jyukyusyaNo tokusyuNo null";
        public static readonly string InvalidKohiEmpty21 = "Invalid kohi2 kohi1 empty model";
        public static readonly string InvalidKohiEmpty31 = "Invalid kohi3 kohi1 empty model";
        public static readonly string InvalidKohiEmpty32 = "Invalid kohi3 kohi2 empty model";
        public static readonly string InvalidKohiEmpty41 = "Invalid kohi4 kohi1 empty model";
        public static readonly string InvalidKohiEmpty42 = "Invalid kohi4 kohi2 empty model";
        public static readonly string InvalidKohiEmpty43 = "Invalid kohi4 kohi3 empty model";
        public static readonly string InvalidDuplicateKohi1 = "Invalid duplicate kohi1 empty model";
        public static readonly string InvalidDuplicateKohi2 = "Invalid duplicate kohi2 empty model";
        public static readonly string InvalidDuplicateKohi3 = "Invalid duplicate kohi3 empty model";
        public static readonly string InvalidDuplicateKohi4 = "Invalid duplicate kohi4 empty model";

        // Invalid Insurance Other
        public static readonly string InvalidPatternOtherAge75 = "Warning hokenInf age >= 75 and hokensyaNo length 8 and start 39";
        public static readonly string InvalidPatternOtherAge65 = "Warning hokenInf age < 65 and hokensyaNo length 8 and start 39";
        public static readonly string InvalidCheckDuplicatePattern = "Warning pattern duplicate";
        public static readonly string InvalidInsuranceList = "Invalid Insurance List";

        //Message Error Common
        public static readonly string MInp00010 = "{0}を入力してください。";
        public static readonly string MConf01020 = "{0}ため、{1}が確定できません。";
        public static readonly string MEnt01020 = "既に登録されているため、{0}は登録できません。";
        public static readonly string MInp00041 = "{0}は {1}を入力してください。";
        public static readonly string MFree00030 = "{0}";
        public static readonly string MInp00070 = "{0}は {1}以下を入力してください。";
        public static readonly string MInp00040 = "{0}ため、{1}は登録できません。";
        public static readonly string MInp00160_1 = "{0}が入力されていません。";
        public static readonly string MEnt00040_1 = "補足コメントが全角20文字を超えています。";

        //Sup Message
        public static readonly string MDrug = "薬剤";
        public static readonly string MInjection = "手技";
        public static readonly string MUsage = "用法";
        public static readonly string MSupUsage1 = "用法";
        public static readonly string MSupUsage2 = "補助用法";
        public static readonly string MEnt01040 = "{0}ため、{1}は登録できません。";
        public static readonly string ErrorCaptionDrugOrInject = "行為や加算が登録されている";
        public static readonly string MQuantity = "数量";
        public static readonly string MTooLargeQuantity = "数量が大きすぎます。";
        public static readonly string MUsageQuantity = "用法の数量";
        public static readonly string MMaxQuantity = "999";
        public static readonly string MMaxLengthOfCmt = "コメントに対する入力値は３８文字以内にしてください。";
        public static readonly string MCmtOptOf830 = "文字情報";
        public static readonly string MCmt831 = "診療行為コード";
        public static readonly string MDateInfor850_1 = "年月日情報";
        public static readonly string MDateInfor850_2 = "年月情報";
        public static readonly string MTimeInfor851 = "時刻情報";
        public static readonly string MTimeInfor852 = "時間（分）情報";
        public static readonly string MDateTimeInfor853 = "日時情報（日、時間及び分を6桁）";
        public static readonly string MDateTimeInfor880 = "年月日情報及び数字情報を入力してください。" + "\r\n" + "※区切り文字「/」スラッシュを間に入力" + "\r\n" + @"　数字情報は数字または次の文字　．－＋≧≦＞＜±";
        public static readonly string MBunkatu = "分割調剤";
        public static readonly string MSumBunkatu = "分割調剤の合計";
        public static readonly string MCommonError = "無効なデータを受信しました。";
        public static readonly string MProcedure = "行為が未確定のため、入力が確定できません。\r\n・手技が入力されているか確認してください。";
        public static readonly string MDisease = "病名";
        public static readonly string MTenkiContinue = "転帰区分";
        public static readonly string MInp00110 = "{0}は {1}以降を入力してください。";
        public static readonly string MTenkiDate = "転帰日";
        public static readonly string MTenkiStartDate = "開始日";
        public static readonly string MTenkiStartDate_2 = "開始日に無効な日付を指定しました。";
        public static readonly string MNoInputData = "数量を入力してください。";
        public static readonly string MNoInputDataCmt = "数字情報を入力してください。";

        // Valid default settings 
        public static readonly string InvalidDefaultSettingDoctor = "Invalid DefaultSettingDoctor < 0";

        //Insurance Master Linkage
        public static readonly string InvalidDigit1 = "Invalid Digit 1";
        public static readonly string InvalidDigit2 = "Invalid Digit 2";
        public static readonly string InvalidDigit3 = "Invalid Digit 3";
        public static readonly string InvalidDigit4 = "Invalid Digit 4";
        public static readonly string InvalidDigit5 = "Invalid Digit 5";
        public static readonly string InvalidDigit6 = "Invalid Digit 6";
        public static readonly string InvalidDigit7 = "Invalid Digit 7";
        public static readonly string InvalidDigit8 = "Invalid Digit 8";
        public static readonly string InvalidHokenNo = "Invalid HokenNo";
        public static readonly string InvalidHokenEdaNo = "Invalid HokenEdaNo";

        //DeletePatient
        public static readonly string NotAllowDeletePatient = "This patient is not allowed to delete";


        //SwapHoken
        public static readonly string SwapHokenSourceInsuranceHasNotSelected = "Please select the source insurance.";
        public static readonly string SwapHokenDesInsuranceHasNotSelected = "Please select a destination insurance.";
        public static readonly string SwapHokenStartDateGreaterThanEndDate = "Enter the end date after the start date.";
        public static readonly string SwapHokenCantExecNotValidDate = "Cannot be executed because the source policy has never been used in StartDate ~ EndDate.";

        //Account Due
        public static readonly string InvalidNyukinKbn = "Invalid NyukinKbn.";
        public static readonly string InvalidSortNo = "Invalid SortNo, must more than or equal 0.";
        public static readonly string InvalidAdjustFutan = "Invalid AdjustFutan, must more than or equal 0.";
        public static readonly string InvalidNyukinGaku = "Invalid NyukinGaku, must more than or equal 0.";
        public static readonly string InvalidPaymentMethodCd = "Invalid PaymentMethodCd, must more than or equal 0.";
        public static readonly string InvalidNyukinDate = "Invalid NyukinDate, must more than or equal 0.";
        public static readonly string InvalidUketukeSbt = "Invalid UketukeSbt, must more than or equal 0.";
        public static readonly string NyukinCmtMaxLength100 = "Invalid UketukeSbt, max length is 100.";
        public static readonly string InvalidSeikyuGaku = "Invalid SeikyuGaku.";
        public static readonly string InvalidSeikyuAdjustFutan = "Invalid SeikyuAdjustFutan.";
        public static readonly string InvalidSeikyuTensu = "Invalid SeikyuTensu, must more than or equal 0.";
        public static readonly string NoItemChange = "No Item Change.";

        // Valid Pattern Expirated 
        public static readonly string InvalidPatternHokenPid = "Invalid Pattern HokenPid";
        public static readonly string InvalidPatternConfirmDate = "Invalid Pattern ConfirmDate";
        public static readonly string InvalidHokenInfStartDate = "Invalid HokenInf StartDate";
        public static readonly string InvalidHokenInfEndDate = "Invalid HokenInf EndDate";
        public static readonly string InvalidConfirmDateAgeCheck = "Invalid ConfirmDate Age Check";
        public static readonly string InvalidConfirmDateHoken = "Invalid ConfirmDate Hoken";
        public static readonly string InvalidHokenMstDate = "Invalid HokenMst Date";
        public static readonly string InvalidConfirmDateKohi1 = "Invalid Kohi1 ConfirmDate";
        public static readonly string InvalidMasterDateKohi1 = "Invalid Kohi1 MasterDate";
        public static readonly string InvalidConfirmDateKohi2 = "Invalid Kohi2 ConfirmDate";
        public static readonly string InvalidMasterDateKohi2 = "Invalid Kohi2 MasterDate";
        public static readonly string InvalidConfirmDateKohi3 = "Invalid Kohi3 ConfirmDate";
        public static readonly string InvalidMasterDateKohi3 = "Invalid Kohi3 MasterDate";
        public static readonly string InvalidConfirmDateKohi4 = "Invalid Kohi4 ConfirmDate";
        public static readonly string InvalidMasterDateKohi4 = "Invalid Kohi4 MasterDate";
        public static readonly string InvalidPatternIsExpirated = "Invalid Pattern Expirated";
        public static readonly string InvalidHasElderHoken = "Invalid Has ElderHoken";

        // Validate Rousai Jibai
        public static readonly string InvalidRodoBangoNull = "Invalid RodoBango Null";
        public static readonly string InvalidRodoBangoLengthNotEquals14 = "Invalid RodoBango Length Not Equals 14";
        public static readonly string InvalidCheckItemFirstListRousaiTenki = "Invalid Check Item First Of ListRousaiTenki";
        public static readonly string InvalidCheckRousaiTenkiSinkei = "Invalid Check RousaiTenki Sinkei";
        public static readonly string InvalidCheckRousaiTenkiTenki = "Invalid Check RousaiTenki Tenki";
        public static readonly string InvalidCheckRousaiTenkiEndDate = "Invalid Check RousaiTenki EndDate";
        public static readonly string InvalidCheckRousaiSaigaiKbnNotEquals1And2 = "Invalid Check RousaiSaigaiKbn Not Equals 1 And 2";
        public static readonly string InvalidCheckRousaiSyobyoDateEquals0 = "Invalid Check Rousai SyobyoDate Equals 0";
        public static readonly string InvalidCheckHokenKbnEquals13AndRousaiSyobyoCdIsNull = "Invalid Check HokenKbn Equals 13 And RousaiSyobyoCd Is Null";
        public static readonly string InvalidCheckRousaiRyoyoDate = "Invalid Check RousaiRyoyoDate";
        public static readonly string InvalidCheckDateExpirated = "Invalid Check Date Expirated";
        public static readonly string InvalidNenkinBangoIsNull = "Invalid NenkinBango Is Null";
        public static readonly string InvalidNenkinBangoLengthNotEquals9 = "Invalid NenkinBango Length Not Equals 9";
        public static readonly string InvalidKenkoKanriBangoIsNull = "Invalid KenkoKanri Bango Is Null";
        public static readonly string InvalidKenkoKanriBangoLengthNotEquals13 = "Invalid KenkoKanri Bango Length Not Equals 13";

        //Next Order
        public static readonly string InvalidRsvkrtNo = "Invalid RsvkrtNo";
        public static readonly string InvalidRsvkrtKbn = "Invalid RsvkrtKbn";
        public static readonly string InvalidRsvDate = "Invalid RsvDate";
        public static readonly string InvalidRsvkrtName = "Invalid RsvkrtName";
        public static readonly string InvalidRsvkrtIsDeleted = "Invalid RsvkrtName";


        // Document
        public static readonly string InvalidDocumentCategoryCd = "Invalid Document CategoryCd!";
        public static readonly string MoveDocCategoryNotFound = "Invalid Document MoveCategoryCd!";
        public static readonly string InvalidDocumentCategoryName = "Invalid Document CategoryName, CategoryName is required and not duplicate!";
        public static readonly string InvalidMoveInDocCategoryCd = "Invalid Document move in CategoryCd, CategoryCd is required and exist in DB!";
        public static readonly string InvalidMoveOutDocCategoryCd = "Invalid Document move out CategoryCd, CategoryCd is required and exist in DB!";
        public static readonly string InvalidDocInfFileName = "Invalid DocInf FileName, FileName is required!";
        public static readonly string InvalidFileInput = "Invalid File Input!";
        public static readonly string TemplateNotFound = "Template Not Found!";
        public static readonly string ExistFileTemplateName = "Exist FileTemplateName!";
        public static readonly string InvalidNewCategoryCd = "Invalid NewCategoryCd, CategoryCd is required and exist in DB!";
        public static readonly string InvalidOldCategoryCd = "Invalid OldCategoryCd, CategoryCd is required and exist in DB!";
        public static readonly string FileTemplateNotFould = "File template not fould!";
        public static readonly string FileTemplateIsExistInNewFolder = "File template is exist in new folder!";
        public static readonly string DocInfNotFound = "DocInf Not Found!";
        public static readonly string TemplateLinkIsNotExists = "TemplateLink is not exists!";
        public static readonly string InvalidExtentionFile = "Extention file is must .docx or .xlsx!";

        //Check Special Item InvalidCheckAge
        public static readonly string InvalidCheckAge = "Invalid Check Age";
        public static readonly string InvalidOdrInfDetail = "Invalid OdrInfDetail";
        public static readonly string InvalidIBirthDay = "Invalid IBirthDay";


        //Exception
        public static readonly string ExceptionError = "Exception error";

        //InsuranceScan
        public static readonly string SaveInsuranceScanFailedSaveToDb = "Failed save scan image to database";
        public static readonly string InvalidImageScan = "Image scan is invalid";
        public static readonly string OldScanImageIsNotFound = "Old scan image is not found";
        public static readonly string RemoveOldScanImageFailed = "Remove old scan image is failed";
        public static readonly string RemoveOldScanImageSuccessful = "Remove old scan image is succesful";

        //Drug Menu
        public static readonly string DrugMenuInvalidIndexMenu = "Invalid Menu Index";

        //PtGrpMaster
        public static readonly string InvalidInputGroupMst = "SortNo,GrpId,GrpName,GrpCode or GrpCodeCodeName is invalid";

        // SanteiInf
        public static readonly string InvalidAlertDays = "Invalid AlertDays!";
        public static readonly string InvalidAlertTerm = "Invalid AlertTerm!";
        public static readonly string InvalidKisanSbt = "Invalid KisanSbt!";
        public static readonly string InvalidKisanDate = "Invalid KisanDate!";
        public static readonly string InvalidByomei = "Invalid Byomei, Byomei is not exist!";
        public static readonly string InvalidHosokuComment = "Invalid HosokuComment, maxlength is 80!";
        public static readonly string ThisSanteiInfDoesNotAllowSanteiInfDetail = "This SanteiInf does not allow have SanteiInfDetail!";
        public static readonly string InvalidSanteiInfDetail = "Invalid SanteiInfDetail, SanteiInf does not contain some SanteiInfDetail!";

    }
}
