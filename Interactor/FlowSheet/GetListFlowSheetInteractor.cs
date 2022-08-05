using Domain.Constant;
using Domain.Models.FlowSheet;
using Helper.Common;
using Helper.Constants;
using Helper.Extendsions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase.FlowSheet.GetList;

namespace Interactor.FlowSheet
{
    public class GetListFlowSheetInteractor : IGetListFlowSheetInputPort
    {
        private const int MAX_CALENDAR_MONTH = 12;

        private readonly IFlowSheetRepository _flowsheetRepository;
        public GetListFlowSheetInteractor(IFlowSheetRepository repository)
        {
            _flowsheetRepository = repository;
        }
        public GetListFlowSheetOutputData Handle(GetListFlowSheetInputData inputData)
        {
            List<FlowSheetModel> resultList = new();
            var flowsheetList = _flowsheetRepository.GetListFlowSheet(inputData.HpId, inputData.PtId, inputData.SinDate, inputData.RaiinNo);
            var raiinListTags = _flowsheetRepository.GetRaiinListTags(inputData.HpId, inputData.PtId);
            var raiinListCmts = _flowsheetRepository.GetRaiinListCmts(inputData.HpId, inputData.PtId);
            var raiinListInf = _flowsheetRepository.GetRaiinListInfModels(inputData.HpId, inputData.PtId);
            var listRaiinNo = _flowsheetRepository.GetListRaiinNo(inputData.HpId, inputData.PtId, inputData.SinDate);
            foreach (FlowSheetModel model in flowsheetList)
            {
                var raiinTag = raiinListTags?.FirstOrDefault(tag => tag.RaiinNo == model.RaiinNo && tag.SinDate == model.SinDate);
                RaiinListTagModel raiinTagResult;
                if (raiinTag != null)
                {
                    raiinTagResult = new(raiinTag);
                }
                else
                {
                    bool isAddNew = true;
                    raiinTagResult = new(inputData.HpId, inputData.PtId, model.RaiinNo, model.SinDate, isAddNew);
                }
                var raiinCmt = raiinListCmts?.FirstOrDefault(cmt => cmt.RaiinNo == model.RaiinNo && cmt.SinDate == model.SinDate);
                RaiinListCmtModel raiinCmtResult;
                if (raiinCmt != null)
                {
                    raiinCmtResult = new(raiinCmt);
                }
                else
                {
                    var isAddNew = true;
                    raiinCmtResult = new(inputData.HpId, inputData.PtId, model.RaiinNo, model.SinDate, 9, string.Empty, isAddNew);
                }
                List<RaiinListInfModel> raiinList = new();
                var raiinListInfs = raiinListInf.FindAll(item => item.HpId == inputData.HpId
                                    && item.PtId == inputData.PtId
                                    && item.SinDate == model.SinDate
                                    && ((model.Status != 0 ? item.RaiinNo == model.RaiinNo : item.RaiinNo == 0) || (item.RaiinNo == 0 && item.RaiinListKbn == 4)))
                                    .OrderBy(item => item.SortNo)
                                    .ToList();
                foreach (var item in raiinListInfs)
                {
                    if (raiinList.Any(r => r.GrpId == item.GrpId)) continue;
                    bool isContainsFile = raiinListInfs?.FirstOrDefault(x => x.GrpId == item.GrpId && x.KbnCd == item.KbnCd && item.RaiinListKbn == 4) != null;
                    raiinList.Add(new(item, isContainsFile));
                }

                resultList.Add(new(model, raiinTagResult, raiinCmtResult, raiinList));
            }

            var raiinListMst = _flowsheetRepository.GetRaiinListMsts(inputData.HpId);

            var tempHolidayCollection = _flowsheetRepository.GetHolidayMst(inputData.HpId);
            List<CalendarGridModel> tempCalendarDataSource = new();
            List<CalendarGridModel> finalCalendarDataSource = new();
            DateTime now = CIUtil.IntToDate(inputData.SinDate);
            for (int i = 0; i < MAX_CALENDAR_MONTH; i++)
            {
                DateTime tempDate = now.AddMonths(-i);

                CalendarGridModel calendarGridVM = new(tempDate);
                tempCalendarDataSource.Insert(0, calendarGridVM);
            }
            foreach (CalendarGridModel model in tempCalendarDataSource)
            {
                var calendarResult = SetCalendarItemList(model, tempHolidayCollection, inputData.SinDate, flowsheetList, listRaiinNo);
                finalCalendarDataSource.Add(calendarResult);
            }
            return new GetListFlowSheetOutputData(flowsheetList, raiinListMst, finalCalendarDataSource);
        }
        public CalendarGridModel SetCalendarItemList(CalendarGridModel model, List<HolidayModel> tempHolidayCollection, int sinDate, List<FlowSheetModel> flowSheetList, List<RaiinDateModel> listRaiinNo)
        {
            CalendarGridModel finalModel;
            int startDate = model.MonthYear.Year * 10000 + model.MonthYear.Month * 100;
            int endDate = model.MonthYear.AddMonths(1).Year * 10000 + model.MonthYear.AddMonths(1).Month * 100;
            int finalSinDate;
            if (startDate < sinDate && endDate > sinDate)
            {
                finalSinDate = sinDate;
            }
            else
            {
                finalSinDate = 0;
            }

            var holidayList = tempHolidayCollection.Where(item => item.SinDate < endDate && item.SinDate > startDate)
                .Select(item => new HolidayModel(item.SinDate % 100, item.HolidayKbn, item.KyusinKbn, item.HolidayName)).ToList();

            List<KeyValuePair<int, int>> tempList = new();
            List<KeyValuePair<int, string>> tagList = new();
            List<KeyValuePair<int, RaiinStateDictObjectValue>> stateList = new();
            foreach (var item in flowSheetList)
            {
                if (item.SinDate < endDate && item.SinDate > startDate)
                {
                    tempList.Add(new KeyValuePair<int, int>(item.SinDate, item.SyosaisinKbn));
                    stateList.Add(new KeyValuePair<int, RaiinStateDictObjectValue>(item.SinDate, new(item.Status, listRaiinNo.FirstOrDefault(c => c.SinDate == item.SinDate)?.RaiinNo ?? 0)));
                    if (!string.IsNullOrEmpty(item.RaiinListTag.StarResource))
                    {
                        tagList.Add(new KeyValuePair<int, string>(item.SinDate, item.RaiinListTag.StarResource));
                    }
                }
            }
            finalModel = CreateCalendarDate(model, finalSinDate, holidayList, stateList, tagList, tempList);
            return new CalendarGridModel(finalModel);
        }
        private CalendarGridModel CreateCalendarDate(CalendarGridModel model, int sinDate, List<HolidayModel> holidays, 
                                                    List<KeyValuePair<int, RaiinStateDictObjectValue>> stateList,
                                                    List<KeyValuePair<int, string>> tagList, List<KeyValuePair<int, int>> tempList)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();

            if (model.Year == 0 || model.Month == 0) return new(model, sinDate, holidays, stateList, tagList, tempList);
            DateTime firstDay = new(model.Year, model.Month, 1);
            int rowCount = 0;

            while (rowCount < 6)
            {
                int days = firstDay.DayOfWeek - DayOfWeek.Monday + 1;
                DateTime start = firstDay.AddDays(-days);
                WeekOfMonthModel tempWeekOfMonth;
                WeekOfMonthModel finalWeekOfMonth;
                DayInfo dayInfo;
                DayInfo finalDayInfo;
                DayInfo monDayInfo = new();
                DayInfo tueDayInfo = new();
                DayInfo wedDayInfo = new();
                DayInfo thuDayInfo = new();
                DayInfo friDayInfo = new();
                DayInfo satDayInfo = new();
                DayInfo sunDayInfo = new();

                string dayOfDayInfo;
                if (rowCount < model.WeekOfMonths.Count)
                {
                    tempWeekOfMonth = model.WeekOfMonths[rowCount];
                }
                else
                {
                    tempWeekOfMonth = new();
                }

                for (int i = 0; i <= 6; i++)
                {
                    DateTime current = start.AddDays(i);
                    
                    switch (current.DayOfWeek)
                    {
                        case DayOfWeek.Monday:
                            dayInfo = tempWeekOfMonth.Mon;
                            break;
                        case DayOfWeek.Tuesday:
                            dayInfo = tempWeekOfMonth.Tue;
                            break;
                        case DayOfWeek.Wednesday:
                            dayInfo = tempWeekOfMonth.Wed;
                            break;
                        case DayOfWeek.Thursday:
                            dayInfo = tempWeekOfMonth.Thu;
                            break;
                        case DayOfWeek.Friday:
                            dayInfo = tempWeekOfMonth.Fri;
                            break;
                        case DayOfWeek.Saturday:
                            dayInfo = tempWeekOfMonth.Sat;
                            break;
                        case DayOfWeek.Sunday:
                            dayInfo = tempWeekOfMonth.Sun;
                            break;
                        default:
                            dayInfo = new();
                            break;
                    }

                    if (dayInfo == null)
                    {
                        dayInfo = new();
                    }
                    EmrCalendarDateColor foreGroundOfDayInfo;
                    DayInfo tempDayInfo = new();
                    DayInfo tempDayInfo2 = new();
                    DayInfo tempDayInfo3;
                    if (current.Month == model.Month)
                    {
                        dayOfDayInfo = current.Day.AsString();

                        tempDayInfo = UpdateDayInfo(dayInfo, model, dayOfDayInfo, holidays, stateList, tempList);

                        if (current.DayOfWeek == DayOfWeek.Saturday)
                        {
                            foreGroundOfDayInfo = EmrCalendarDateColor.BlueForeGround;
                            tempDayInfo2 = new(tempDayInfo, foreGroundOfDayInfo);
                        }
                        if (current.DayOfWeek == DayOfWeek.Sunday)
                        {
                            foreGroundOfDayInfo = EmrCalendarDateColor.BlueForeGround;
                            tempDayInfo2 = new(tempDayInfo, foreGroundOfDayInfo);
                        }
                    }
                    if (tempDayInfo2.Date != 0)
                    {
                        tempDayInfo3 = UpdateBorder(tempDayInfo2, model, holidays);
                        finalDayInfo = new(tempDayInfo3);
                        switch (current.DayOfWeek)
                        {
                            case DayOfWeek.Monday:
                                monDayInfo = new(finalDayInfo);
                                break;
                            case DayOfWeek.Tuesday:
                                tueDayInfo = new(finalDayInfo);
                                break;
                            case DayOfWeek.Wednesday:
                                wedDayInfo = new(finalDayInfo);
                                break;
                            case DayOfWeek.Thursday:
                                thuDayInfo = new(finalDayInfo);
                                break;
                            case DayOfWeek.Friday:
                                friDayInfo = new(finalDayInfo);
                                break;
                            case DayOfWeek.Saturday:
                                satDayInfo = new(finalDayInfo);
                                break;
                            case DayOfWeek.Sunday:
                                sunDayInfo = new(finalDayInfo);
                                break;
                        }
                    }
                    else
                    {
                        tempDayInfo3 = UpdateBorder(tempDayInfo, model, holidays);
                        finalDayInfo = new(tempDayInfo3);
                        switch (current.DayOfWeek)
                        {
                            case DayOfWeek.Monday:
                                monDayInfo = new(finalDayInfo);
                                break;
                            case DayOfWeek.Tuesday:
                                tueDayInfo = new(finalDayInfo);
                                break;
                            case DayOfWeek.Wednesday:
                                wedDayInfo = new(finalDayInfo);
                                break;
                            case DayOfWeek.Thursday:
                                thuDayInfo = new(finalDayInfo);
                                break;
                            case DayOfWeek.Friday:
                                friDayInfo = new(finalDayInfo);
                                break;
                            case DayOfWeek.Saturday:
                                satDayInfo = new(finalDayInfo);
                                break;
                            case DayOfWeek.Sunday:
                                sunDayInfo = new(finalDayInfo);
                                break;
                        }
                    }
                }
                finalWeekOfMonth = new(monDayInfo, tueDayInfo, wedDayInfo, thuDayInfo, friDayInfo, satDayInfo, sunDayInfo);
                if (rowCount < model.WeekOfMonths.Count)
                {
                    model.WeekOfMonths[rowCount] = finalWeekOfMonth;
                }
                else
                {
                    model.WeekOfMonths.Add(finalWeekOfMonth);
                }
                firstDay = start.AddDays(7);
                rowCount++;

                model = new(model);
            }
            watch.Stop();

            return model;
        }
        private DayInfo UpdateBorder(DayInfo dayInfo, CalendarGridModel model, List<HolidayModel> holidays)
        {
            if (model.HolidayModels == null) return dayInfo;
            EmrCalendarDateColor borderBrush = EmrCalendarDateColor.Default;
            bool isToday = false;
            int day = dayInfo.Day.AsInteger();
            int date = model.Year * 10000 + model.Month * 100 + day;
            var calendarHolidayModel = holidays.FirstOrDefault(item => item.SinDate == day);
            if (calendarHolidayModel != null && calendarHolidayModel.KyusinKbn == 1 && !(model.HighlightToday && date == model.SinDate))
            {
                borderBrush = EmrCalendarDateColor.RedBorder;
            }
            if (model.HighlightToday && date == model.SinDate)
            {
                isToday = true;
            }
            else
            {
                isToday = false;
            }
            return new DayInfo(dayInfo, borderBrush, isToday);
        }
        private DayInfo UpdateDayInfo(DayInfo dayInfo, CalendarGridModel model, string dayStr, List<HolidayModel> holidays,
                                                    List<KeyValuePair<int, RaiinStateDictObjectValue>> stateList, List<KeyValuePair<int, int>> tempList)
        {
            EmrCalendarDateColor finalBackGroundColor = EmrCalendarDateColor.Default;
            EmrCalendarDateColor finalForeGroundColor = EmrCalendarDateColor.NormalForeGround;

            int day = dayStr.AsInteger();
            int date = model.Year * 10000 + model.Month * 100 + day;

            int dateOfDayInfo = date;
            string toolTipOfDayInfo = string.Empty;

            if (holidays != null)
            {
                var calendarHolidayModel = holidays.FirstOrDefault(item => item.SinDate == day);

                if (calendarHolidayModel != null)
                {
                    switch (calendarHolidayModel.HolidayKbn)
                    {
                        case 1:
                            finalBackGroundColor = EmrCalendarDateColor.HolidayRed;
                            finalForeGroundColor = EmrCalendarDateColor.RedForeGround;
                            break;
                        case 2:
                            finalBackGroundColor = EmrCalendarDateColor.Holiday;
                            break;
                    }

                    if (!string.IsNullOrEmpty(calendarHolidayModel.HolidayName))
                    {
                        toolTipOfDayInfo = string.Format("{0} {1}", CIUtil.IntToDate(dayInfo.Date).ToString("MM/dd"), calendarHolidayModel.HolidayName);
                    }
                }
            }

            var dateSyosaiItems = tempList.Where(item => item.Key == date);
            var datetateItem = stateList.FirstOrDefault(item => item.Key == date);
            foreach (var dateSyosaiItem in dateSyosaiItems)
            {
                if (!dateSyosaiItem.Equals(default(KeyValuePair<int, int>)))
                {
                    if (!(!datetateItem.Equals(default(KeyValuePair<int, int>)) && date == model.SinDate && datetateItem.Value.Value < RaiinState.TempSave))
                    {
                        toolTipOfDayInfo = (string.IsNullOrEmpty(dayInfo.ToolTip) ? "" : dayInfo.ToolTip + Environment.NewLine) + SyosaiConst.FlowSheetCalendarDict[dateSyosaiItem.Value];
                    }

                    GetBackgroud(dateSyosaiItem);
                }
            }

            var dateSyosaiNoneStatus = tempList.FirstOrDefault(item => item.Key == date && item.Value == SyosaiConst.None);
            if (!dateSyosaiNoneStatus.Equals(default(KeyValuePair<int, int>))) GetBackgroud(dateSyosaiNoneStatus);
            var dateSyosaiJihiStatus = tempList.FirstOrDefault(item => item.Key == date && item.Value == SyosaiConst.Jihi);
            if (!dateSyosaiJihiStatus.Equals(default(KeyValuePair<int, int>))) GetBackgroud(dateSyosaiJihiStatus);
            var dateSyosaiSaisinStatus = tempList.FirstOrDefault(item => item.Key == date && item.Value == SyosaiConst.Saisin);
            if (!dateSyosaiSaisinStatus.Equals(default(KeyValuePair<int, int>))) GetBackgroud(dateSyosaiSaisinStatus);
            var dateSyosaiSyosinStatus = tempList.FirstOrDefault(item => item.Key == date && item.Value == SyosaiConst.Syosin);
            if (!dateSyosaiSyosinStatus.Equals(default(KeyValuePair<int, int>))) GetBackgroud(dateSyosaiSyosinStatus);

            if (model.HighlightToday && date == model.SinDate)
            {
                finalBackGroundColor = EmrCalendarDateColor.Default;
            }
            return new DayInfo(dateOfDayInfo, dayStr, toolTipOfDayInfo, finalForeGroundColor, finalBackGroundColor, EmrCalendarDateColor.Default, false);
        }
        public static EmrCalendarDateColor GetBackgroud(KeyValuePair<int, int> dateSyosaiItem)
        {
            EmrCalendarDateColor result;
            switch (dateSyosaiItem.Value)
            {
                case SyosaiConst.Syosin:
                case SyosaiConst.Syosin2:
                    result = EmrCalendarDateColor.HasFirstVisit;
                    break;
                case SyosaiConst.Saisin:
                case SyosaiConst.Saisin2:
                case SyosaiConst.SaisinDenwa:
                case SyosaiConst.SaisinDenwa2:
                    result = EmrCalendarDateColor.CalHasCallAgain;
                    break;
                case SyosaiConst.None:
                    result = EmrCalendarDateColor.CalHasData;
                    break;
                case SyosaiConst.NextOrder:
                    result = EmrCalendarDateColor.CalNextOrder;
                    break;
                case SyosaiConst.Jihi:
                    result = EmrCalendarDateColor.HasOwnExpense;
                    break;
                default:
                    result = EmrCalendarDateColor.Default;
                    break;
            }
            return result;
        }
    }
}
