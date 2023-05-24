namespace Domain.Models.RaiinListSetting
{
    public class RaiinListItemModel
    {
        public int HpId { get; private set; }

        public int GrpId { get; private set; }

        public int KbnCd { get; private set; }

        public string ItemCd{ get; private set; }

        public string ItemCdForQuery { get; private set; }

        public long SeqNo { get; private set; }
;
        public string InputName { get; private set; }

        public string OldInputName { get; private set; }

        public int IsExclude { get; private set; }

        public bool IsAddNew { get; private set; }

        public int IsDeleted { get; private set; }

        public Exclusion Exclusion
        {
            get
            {
                switch (IsExclude)
                {
                    case 0:
                        return Exclusions[IsExclude];
                    case 1:
                        return Exclusions[IsExclude];
                    default:
                        return new Exclusion()
                        {
                            Id = -1
                        };
                }
            }
            set
            {
                if (Set(ref _exclusion, value))
                {
                    if (_exclusion != null)
                    {
                        IsExclude = _exclusion.Id;
                    }
                    else
                    {
                        IsExclude = 0;
                    }
                    IsModify = true;
                }
                else
                {
                    _exclusion = new Exclusion()
                    {
                        Id = -1
                    };
                    IsExclude = -1;
                    RaisePropertyChanged(() => IsExclude);
                }
            }
        }

        public List<Exclusion> Exclusions { get; set; } = new List<Exclusion>()
        {
            new Exclusion() { Id = 0, Content = " " },
            new Exclusion() { Id = 1, Content = "●" }
        };

        
        public bool IsModify { get; private set; }

        public bool CheckDefaultValue()
        {
            return IsAddNew && string.IsNullOrWhiteSpace(ItemCd) && IsExclude == 0;
        }
    }
}
