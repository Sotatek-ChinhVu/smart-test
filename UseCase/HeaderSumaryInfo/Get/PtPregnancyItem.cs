using Domain.Models.PtPregnancy;
using Helper.Common;
using Helper.Extendsions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UseCase.HeaderSumaryInfo.Get
{
    public class PtPregnancyItem : ObservableObject
    {
        public PtPregnancyModel PtPregnancy { get; private set; }

        public PtPregnancyItem(PtPregnancyModel ptPregnancy)
        {
            PtPregnancy = ptPregnancy;
        }


        private int _sinDay;
        public int SinDay { get => _sinDay; set => Set(ref _sinDay, value); }
        public long PtId
        {
            get => PtPregnancy.PtId;
        }

        public int PregnacyStartDate => PtPregnancy.StartDate;

        public string StartDate
        {
            get => CIUtil.SDateToShowSDate(PtPregnancy.StartDate);
            set
            {
                int startDate = CIUtil.ShowSDateToSDate(value);
                if (startDate > PtPregnancy.EndDate && PtPregnancy.EndDate > 0)
                {
                    startDate = 0;
                }
                PtPregnancy.StartDate = startDate;
                RaisePropertyChanged(() => StartDate);
            }
        }

        public int PregnacyEndDate => PtPregnancy.EndDate;

        public string EndDate
        {
            get => CIUtil.SDateToShowSDate(PtPregnancy.EndDate);
            set
            {
                int endDate = CIUtil.ShowSDateToSDate(value);
                if (endDate < PtPregnancy.StartDate || endDate == 0)
                {
                    endDate = 99999999;
                }
                PtPregnancy.EndDate = endDate;
                RaisePropertyChanged(() => EndDate);
                RaisePropertyChanged(() => OvulationWeek);
                RaisePropertyChanged(() => PeriodWeek);
            }
        }

        public int FullStartDate
        {
            get
            {
                if (PregnacyStartDate == 0) return 0;

                int startDateLength = PregnacyStartDate.AsString().Length;
                if (startDateLength == 8)
                {
                    //Format of StartDate is yyyymmdd
                    return PregnacyStartDate;
                }
                else if (startDateLength == 6)
                {
                    //Format of StartDate is yyyymm
                    //Need to convert to yyyymm01
                    return PregnacyStartDate * 100 + 1;
                }
                else
                {
                    return 0;
                }
            }
        }

        public int FullEndDate
        {
            get
            {
                if (PregnacyEndDate == 0) return 99999999;

                int endDateLength = PregnacyEndDate.AsString().Length;

                if (endDateLength == 8)
                {
                    //Format of EndDate is yyyymmdd
                    return PregnacyEndDate;
                }
                else if (endDateLength == 6)
                {
                    //Format of EndDate is yyyymm
                    //Need to convert to yyyymm31
                    return PregnacyEndDate * 100 + 31;
                }
                else
                {
                    return 99999999;
                }
            }
        }

        public DateTime? PeriodDate
        {
            get
            {
                if (PtPregnancy.PeriodDate == 0) return null;
                return CIUtil.IntToDate(PtPregnancy.PeriodDate);
            }
            set
            {
                if (value.HasValue)
                {
                    if (!PeriodDueDate.HasValue)
                    {
                        PtPregnancy.PeriodDueDate = value.Value.AddDays(280).ToString("yyyyMMdd").AsInteger();
                        RaisePropertyChanged(() => PeriodDueDate);
                    }
                    else
                    {
                        if (PeriodDate.HasValue)
                        {
                            var tempDueDate = PeriodDate.Value.AddDays(280);
                            if (tempDueDate == PeriodDueDate.Value)
                            {
                                PtPregnancy.PeriodDueDate = value.Value.AddDays(280).ToString("yyyyMMdd").AsInteger();
                                RaisePropertyChanged(() => PeriodDueDate);
                            }
                        }
                    }

                    PtPregnancy.PeriodDate = value.Value.ToString("yyyyMMdd").AsInteger();
                }
                else
                {
                    PtPregnancy.PeriodDate = 0;
                }

                RaisePropertyChanged(() => PeriodDate);
                RaisePropertyChanged(() => PeriodWeek);
            }
        }
        public DateTime? PeriodDueDate
        {
            get
            {
                if (PtPregnancy.PeriodDueDate == 0) return null;
                return CIUtil.IntToDate(PtPregnancy.PeriodDueDate);
            }
            set
            {
                if (value.HasValue)
                {
                    if (!PeriodDate.HasValue)
                    {
                        PtPregnancy.PeriodDate = value.Value.AddDays(-280).ToString("yyyyMMdd").AsInteger();
                        RaisePropertyChanged(() => PeriodDate);
                    }
                    PtPregnancy.PeriodDueDate = value.Value.ToString("yyyyMMdd").AsInteger();
                }
                else
                {
                    if (PeriodDate.HasValue)
                    {
                        PtPregnancy.PeriodDueDate = PeriodDate.Value.AddDays(280).ToString("yyyyMMdd").AsInteger();
                    }
                    else
                    {
                        PtPregnancy.PeriodDueDate = 0;
                    }
                }

                RaisePropertyChanged(() => PeriodDueDate);
                RaisePropertyChanged(() => PeriodWeek);
            }
        }


        public DateTime? OvulationDate
        {
            get
            {
                if (PtPregnancy.OvulationDate == 0) return null;
                return CIUtil.IntToDate(PtPregnancy.OvulationDate);
            }
            set
            {
                if (value.HasValue)
                {
                    if (!OvulationDueDate.HasValue)
                    {
                        PtPregnancy.OvulationDueDate = value.Value.AddDays(266).ToString("yyyyMMdd").AsInteger();
                        RaisePropertyChanged(() => OvulationDueDate);
                    }
                    else
                    {
                        if (OvulationDate.HasValue)
                        {
                            var tempDueDate = OvulationDate.Value.AddDays(266);
                            if (tempDueDate == OvulationDueDate.Value)
                            {
                                PtPregnancy.OvulationDueDate = value.Value.AddDays(266).ToString("yyyyMMdd").AsInteger();
                                RaisePropertyChanged(() => OvulationDueDate);
                            }
                        }
                    }

                    PtPregnancy.OvulationDate = value.Value.ToString("yyyyMMdd").AsInteger();
                }
                else
                {
                    PtPregnancy.OvulationDate = 0;
                }

                RaisePropertyChanged(() => OvulationDate);
                RaisePropertyChanged(() => OvulationWeek);
            }
        }

        public DateTime? OvulationDueDate
        {
            get
            {
                if (PtPregnancy.OvulationDueDate == 0) return null;
                return CIUtil.IntToDate(PtPregnancy.OvulationDueDate);
            }
            set
            {
                if (value.HasValue)
                {
                    if (!OvulationDate.HasValue)
                    {
                        PtPregnancy.OvulationDate = value.Value.AddDays(-266).ToString("yyyyMMdd").AsInteger();
                        RaisePropertyChanged(() => OvulationDate);
                    }
                    PtPregnancy.OvulationDueDate = value.Value.ToString("yyyyMMdd").AsInteger();
                }
                else
                {
                    if (OvulationDate.HasValue)
                    {
                        PtPregnancy.OvulationDueDate = OvulationDate.Value.AddDays(266).ToString("yyyyMMdd").AsInteger();
                    }
                    else
                    {
                        PtPregnancy.OvulationDueDate = 0;
                    }
                }

                RaisePropertyChanged(() => OvulationDueDate);
                RaisePropertyChanged(() => OvulationWeek);
            }
        }


        public int IsDelete
        {
            get => PtPregnancy.IsDeleted;
            set
            {
                if (PtPregnancy.IsDeleted == value) return;
                PtPregnancy.IsDeleted = value;
            }
        }

        public DateTime UpdateDate
        {
            get => PtPregnancy.UpdateDate;
            set
            {
                if (PtPregnancy.UpdateDate == value) return;
                PtPregnancy.UpdateDate = value;
            }
        }

        public int UpdateId
        {
            get => PtPregnancy.UpdateId;
            set
            {
                if (PtPregnancy.UpdateId == value) return;
                PtPregnancy.UpdateId = value;
            }
        }

        public string UpdateMachine
        {
            get => PtPregnancy.UpdateMachine;
            set
            {
                if (PtPregnancy.UpdateMachine == value) return;
                PtPregnancy.UpdateMachine = value;
            }
        }

        public string PeriodWeek
        {
            get
            {
                int startDate = PtPregnancy.PeriodDate;
                int endDate = PtPregnancy.EndDate;
                if (!PeriodDate.HasValue && PeriodDueDate.HasValue)
                {
                    startDate = PeriodDueDate.Value.AddDays(-280).ToString("yyyyMMdd").AsInteger();
                }
                return GetPeriodWeek(startDate, 0, endDate);
            }
        }

        public string OvulationWeek
        {
            get
            {
                int startDate = PtPregnancy.OvulationDate;
                int endDate = PtPregnancy.EndDate;
                if (!OvulationDate.HasValue && OvulationDueDate.HasValue)
                {
                    startDate = OvulationDueDate.Value.AddDays(-266).ToString("yyyyMMdd").AsInteger();
                }
                return GetPeriodWeek(startDate, 1, endDate);
            }
        }
        private string GetPeriodWeek(int startDay, int ovulation, int endDay = 0)
        {
            if (startDay == 0) return string.Empty;
            if (startDay >= SinDay)
            {
                return "0W0D";
            }
            if (endDay != 0 && endDay < startDay)
            {
                return "0W0D";
            }
            DateTime dtStartDay = CIUtil.IntToDate(startDay);
            dtStartDay = dtStartDay.AddDays(-14 * ovulation);

            DateTime dtToDay;
            if (SinDay > endDay && endDay > 0)
            {
                dtToDay = CIUtil.IntToDate(endDay);
            }
            else
            {
                dtToDay = CIUtil.IntToDate(SinDay);
            }

            int countDays = dtToDay.Subtract(dtStartDay).Days;
            if (countDays < 0)
            {
                countDays *= -1;
            }
            return (countDays / 7) + "W" + (countDays % 7) + "D";
        }

        public bool CheckDefaultValue()
        {
            return PtId == 0 && string.IsNullOrEmpty(StartDate) && string.IsNullOrEmpty(EndDate) && PeriodDate == null && OvulationDate == null && PeriodDueDate == null && OvulationDueDate == null;
        }

        private bool _modelModified = false;

        public bool ModelModified
        {
            get => _modelModified;
            set { Set(ref _modelModified, value); }
        }
    }
}
