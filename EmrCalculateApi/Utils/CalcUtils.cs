namespace EmrCalculateApi.Utils
{
    class CalcUtils
    {
        /// <summary>
        /// チェック用保険区分を返す
        /// 健保、労災、自賠の場合、オプションにより、同一扱いにするか別扱いにするか決定
        /// 自費の場合、健保と自費を対象にする
        /// </summary>
        /// <param name="hokenKbn">
        /// 0-健保、1-労災、2-アフターケア、3-自賠、4-自費
        /// </param>
        /// <returns></returns>
        public static List<int> GetCheckHokenKbns(int hokenKbn, int hokensyuHandling)
        {
            List<int> results = new List<int>();

            if (hokensyuHandling == 0)
            {
                // 同一に考える
                if (hokenKbn <= 3)
                {
                    results.AddRange(new List<int> { 0, 1, 2, 3 });
                }
                else
                {
                    results.Add(hokenKbn);
                }
            }
            else if (hokensyuHandling == 1)
            {
                // すべて同一に考える
                results.AddRange(new List<int> { 0, 1, 2, 3, 4 });
            }
            else
            {
                // 別に考える
                results.Add(hokenKbn);
            }

            if (hokenKbn == 4)
            {
                results.Add(0);
            }

            return results;
        }

        public static List<int> GetCheckSanteiKbns(int hokenKbn, int hokensyuHandling)
        {
            List<int> results = new List<int> { 0 };

            if (hokensyuHandling == 0)
            {
                // 同一に考える
                if(hokenKbn == 4)
                {
                    //results.Add(2);
                }
            }
            else if (hokensyuHandling == 1)
            {
                // すべて同一に考える
                results.Add(2);
            }
            else
            {
                // 別に考える
            }

            return results;
        }
    }
}
