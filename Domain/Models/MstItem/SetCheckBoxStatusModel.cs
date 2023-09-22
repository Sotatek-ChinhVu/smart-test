namespace Domain.Models.MstItem
{
    public class SetCheckBoxStatusModel
    {
        public SetCheckBoxStatusModel(bool setKbnChecked1, bool setKbnChecked2, bool setKbnChecked3, bool setKbnChecked4, bool setKbnChecked5,
        bool setKbnChecked6, bool setKbnChecked7, bool setKbnChecked8, bool setKbnChecked9, bool setKbnChecked10, bool jihiChecked,
        bool kihonChecked, bool tokuChecked, bool yohoChecked, bool diffChecked)
        {
            SetKbnChecked1 = setKbnChecked1;
            SetKbnChecked2 = setKbnChecked2;
            SetKbnChecked3 = setKbnChecked3;
            SetKbnChecked4 = setKbnChecked4;
            SetKbnChecked5 = setKbnChecked5;
            SetKbnChecked6 = setKbnChecked6;
            SetKbnChecked7 = setKbnChecked7;
            SetKbnChecked8 = setKbnChecked8;
            SetKbnChecked9 = setKbnChecked9;
            SetKbnChecked10 = setKbnChecked10;
            JihiChecked = jihiChecked;
            KihonChecked = kihonChecked;
            TokuChecked = tokuChecked;
            YohoChecked = yohoChecked;
            DiffChecked = diffChecked;
        }
        public bool SetKbnChecked1 { get; private set; }

        public bool SetKbnChecked2 { get; private set; }

        public bool SetKbnChecked3 { get; private set; }

        public bool SetKbnChecked4 { get; private set; }

        public bool SetKbnChecked5 { get; private set; }

        public bool SetKbnChecked6 { get; private set; }

        public bool SetKbnChecked7 { get; private set; }

        public bool SetKbnChecked8 { get; private set; }

        public bool SetKbnChecked9 { get; private set; }

        public bool SetKbnChecked10 { get; private set; }

        public bool JihiChecked { get; private set; }

        public bool KihonChecked { get; private set; }

        public bool TokuChecked { get; private set; }

        public bool YohoChecked { get; private set; }

        public bool DiffChecked { get; private set; }
    }
}
