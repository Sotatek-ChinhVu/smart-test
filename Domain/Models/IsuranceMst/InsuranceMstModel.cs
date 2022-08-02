using Domain.Models.Insurance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.IsuranceMst
{
    public class InsuranceMstModel
    {
        public InsuranceMstModel(List<TokkiMstModel> listTokkiMstModel, Dictionary<int, string> hokenKogakuKbnDict, List<HokenMstModel> kohi1MstData, List<HokenMstModel> kohi2MstData, List<HokenMstModel> kohi3MstData, List<HokenMstModel> kohi4MstData, List<KohiInfModel> kohiInfData, List<HokenInfModel> hokenInfData, List<KantokuMstModel> kantokuMstData, List<ByomeiMstAftercareModel> byomeiMstAftercareData, List<HokenMstModel> hokenMstData)
        {
            ListTokkiMstModel = listTokkiMstModel;
            this.hokenKogakuKbnDict = hokenKogakuKbnDict;
            Kohi1MstData = kohi1MstData;
            Kohi2MstData = kohi2MstData;
            Kohi3MstData = kohi3MstData;
            Kohi4MstData = kohi4MstData;
            KohiInfData = kohiInfData;
            HokenInfData = hokenInfData;
            KantokuMstData = kantokuMstData;
            ByomeiMstAftercareData = byomeiMstAftercareData;
            HokenMstData = hokenMstData;
        }

        public List<TokkiMstModel> ListTokkiMstModel { get; private set; }

        public Dictionary<int, string> hokenKogakuKbnDict { get; private set; }

        public List<HokenMstModel> Kohi1MstData { get; private set; }

        public List<HokenMstModel> Kohi2MstData { get; private set; }

        public List<HokenMstModel> Kohi3MstData { get; private set; }

        public List<HokenMstModel> Kohi4MstData { get; private set; }

        public List<KohiInfModel> KohiInfData { get; private set; }

        public List<HokenInfModel> HokenInfData { get; private set; }

        public List<KantokuMstModel> KantokuMstData { get; private set; }

        public List<ByomeiMstAftercareModel> ByomeiMstAftercareData { get; private set; }

        public List<HokenMstModel> HokenMstData { get; private set; }

    }
}
