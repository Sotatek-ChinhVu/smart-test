using Domain.Models.Reception;
using Domain.Models.RsvFrameMst;
using Domain.Models.RsvGrpMst;
using Domain.Models.RsvInfo;

namespace UseCase.HeaderSumaryInfo.Get
{
    public class RsvInfItem
    {
        public RsvInfoModel? RsvInf { get; }
        public RsvFrameMstModel? RsvFrameMst { get; }
        public RsvGrpMstModel? RsvGrpMst { get; }
        public ReceptionRowModel? RaiinInfModel { get; }


        public RsvInfItem(RsvInfoModel? rsvInf, RsvFrameMstModel? rsvFrameMst, RsvGrpMstModel? rsvGrpMst, ReceptionRowModel? raiinInfModel)
        {
            RsvInf = rsvInf;
            RsvFrameMst = rsvFrameMst;
            RsvGrpMst = rsvGrpMst;
            RaiinInfModel = raiinInfModel;
        }

        /// <summary>
        /// 医療機関識別ID
        /// 
        /// </summary>
        public int HpId
        {
            get { return RsvInf?.HpId ?? 0; }
        }

        /// <summary>
        /// 予約枠ID
        /// 
        /// </summary>
        public int RsvFrameId
        {
            get { return RsvInf?.RsvFrameId ?? 0; }
        }

        /// <summary>
        /// 診療日
        /// 
        /// </summary>
        public int SinDate
        {
            get
            {
                if (RsvInf != null)
                {
                    return RsvInf.SinDate;
                }
                else if (RaiinInfModel != null)
                {
                    return RaiinInfModel.SinDate;
                }
                return 0;
            }
        }

        /// <summary>
        /// 開始時間
        /// 
        /// </summary>
        public int StartTime
        {
            get { return RsvInf?.StartTime ?? 0; }
        }

        /// <summary>
        /// 予約番号
        /// 
        /// </summary>
        public long RaiinNo
        {
            get { return RsvInf?.RaiinNo ?? 0; }
        }

        /// <summary>
        /// 患者ID
        /// 
        /// </summary>
        public long PtId
        {
            get { return RsvInf?.PtId ?? 0; }
        }

        /// <summary>
        /// 予約種別コード
        /// 
        /// </summary>
        public int RsvSbt
        {
            get { return RsvInf?.RsvSbt ?? 0; }
        }

        /// <summary>
        /// 担当医師コード
        /// 
        /// </summary>
        public int TantoId
        {
            get { return RsvInf?.TantoId ?? 0; }
        }

        /// <summary>
        /// 診療科コード
        /// 
        /// </summary>
        public int KaId
        {
            get { return RsvInf?.KaId ?? 0; }
        }

        public string RsvFrameName
        {
            get
            {
                if (RsvFrameMst != null)
                {
                    return RsvFrameMst.RsvFrameName;
                }
                return string.Empty;
            }
        }

        public string RsvGrpName
        {
            get
            {
                if (RsvGrpMst != null)
                {
                    return RsvGrpMst.RsvGrpName;
                }
                return string.Empty;
            }
        }

        public string UketukeTime
        {
            get
            {
                if (RaiinInfModel != null)
                {
                    return RaiinInfModel.UketukeTime;
                }
                return string.Empty;
            }
        }
    }
}
