namespace EventProcessor.Model;

public class BundleArgumentModel
{
    /// <summary>
    /// /RENKEI_CONF.RENKEI_ID
    /// </summary>
    public int RenkeiId { get; private set; }
    /// <summary>
    /// RENKEI_CONF.SEQ_NO
    /// </summary>
    public int SeqNo { get; private set; }
    /// <summary>
    /// RENKEI_PATH_CONF.EDA_NO
    /// </summary>
    public int EdaNo { get; private set; }
    /// <summary>
    /// RENKEI_CONF.PARAMETER
    /// </summary>
    public string Parameter { get; private set; } = string.Empty;
    /// <summary>
    /// RENKEI_CONF.PT_NUM_LENGTH
    /// </summary>
    public int PtNumLength { get; private set; }
    /// <summary>
    /// RENKEI_PATH_CONF.PATH
    /// </summary>
    public string Path { get; private set; } = string.Empty;
    /// <summary>
    /// RENKEI_PATH_CONF.CHAR_CD
    /// </summary>
    public int CharCd { get; private set; }

    /// <summary>
    /// 出力対象患者ID、未指定の場合、全患者が対象
    /// </summary>
    public List<long> PtIds { get; private set; } = new();
    /// <summary>
    /// 出力対象診療日 From
    /// </summary>
    public int FromDate { get; private set; }
    /// <summary>
    /// 出力対象診療日 To
    /// </summary>
    public int ToDate { get; private set; } = 99999999;
}
