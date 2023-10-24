using Domain.Models.SetMst;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.SetGenerationMst
{
    public class GetCountProcessModel
    {
        public GetCountProcessModel(int setMstsBackupedCount, int setKbnMstSourceCount, int setByomeisSourceCount, int setKarteInfsSourceCount, int setKarteImgInfsSourceCount, int setOdrInfsSourceCount, int setOdrInfDetailsSourceCount, int setOdrInfCmtSourceCount, Dictionary<int, SetMstModel> listSetMst, List<int> listDictContain)
        {
            SetMstsBackupedCount = setMstsBackupedCount;
            SetKbnMstSourceCount = setKbnMstSourceCount;
            SetByomeisSourceCount = setByomeisSourceCount;
            SetKarteInfsSourceCount = setKarteInfsSourceCount;
            SetKarteImgInfsSourceCount = setKarteImgInfsSourceCount;
            SetOdrInfsSourceCount = setOdrInfsSourceCount;
            SetOdrInfDetailsSourceCount = setOdrInfDetailsSourceCount;
            SetOdrInfCmtSourceCount = setOdrInfCmtSourceCount;
            ListSetMst = listSetMst;
            ListDictContain = listDictContain;
        }

        public GetCountProcessModel()
        {
            SetMstsBackupedCount = 0;
            SetKbnMstSourceCount = 0;
            SetByomeisSourceCount = 0;
            SetKarteInfsSourceCount = 0;
            SetKarteImgInfsSourceCount = 0;
            SetOdrInfsSourceCount = 0;
            SetOdrInfDetailsSourceCount = 0;
            SetOdrInfCmtSourceCount = 0;
            ListSetMst = new Dictionary<int, SetMstModel>();
            ListDictContain = new List<int>();
        }
        public int SetMstsBackupedCount { get; private set; }
        public int SetKbnMstSourceCount { get; private set; }
        public int SetByomeisSourceCount { get; private set; }
        public int SetKarteInfsSourceCount { get; private set; }
        public int SetKarteImgInfsSourceCount { get; private set; }
        public int SetOdrInfsSourceCount { get; private set; }
        public int SetOdrInfDetailsSourceCount { get; private set; }
        public int SetOdrInfCmtSourceCount { get; private set; }
        public Dictionary<int, SetMstModel> ListSetMst { get; private set; }
        public List<int> ListDictContain { get; private set; }
        public int TotalCount { 
            get {
                return SetMstsBackupedCount + SetKbnMstSourceCount + SetByomeisSourceCount + SetKarteInfsSourceCount + SetKarteImgInfsSourceCount + SetOdrInfsSourceCount + SetOdrInfDetailsSourceCount + SetOdrInfCmtSourceCount;
            } 
        }

    }
}
