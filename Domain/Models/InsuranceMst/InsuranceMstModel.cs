using Domain.Models.Insurance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.InsuranceMst
{
    public class InsuranceMstModel
    {
        public InsuranceMstModel()
        {
            ListTokkiMstModel = new List<TokkiMstModel>();
            HokenKogakuKbnDict = new Dictionary<int, string>();
            KohiHokenMstData = new List<HokenMstModel>();
            KohiInfData = new List<KohiInfModel>();
            HokenInfData = new List<HokenInfModel>();
            KantokuMstData = new List<KantokuMstModel>();
            ByomeiMstAftercareData = new List<ByomeiMstAftercareModel>();
            HokenMstData = new List<HokenMstModel>();
        }

        public InsuranceMstModel(List<TokkiMstModel> listTokkiMstModel, Dictionary<int, string> hokenKogakuKbnDict, List<HokenMstModel> kohiHokenMstData, List<KohiInfModel> kohiInfData, List<HokenInfModel> hokenInfData, List<KantokuMstModel> kantokuMstData, List<ByomeiMstAftercareModel> byomeiMstAftercareData, List<HokenMstModel> hokenMstData)
        {
            ListTokkiMstModel = listTokkiMstModel;
            HokenKogakuKbnDict = hokenKogakuKbnDict;
            KohiHokenMstData = kohiHokenMstData;
            KohiInfData = kohiInfData;
            HokenInfData = hokenInfData;
            KantokuMstData = kantokuMstData;
            ByomeiMstAftercareData = byomeiMstAftercareData;
            HokenMstData = hokenMstData;
        }

        public List<TokkiMstModel> ListTokkiMstModel { get; private set; }

        public Dictionary<int, string> HokenKogakuKbnDict { get; private set; }

        public List<HokenMstModel> KohiHokenMstData { get; private set; }

        public List<KohiInfModel> KohiInfData { get; private set; }

        public List<HokenInfModel> HokenInfData { get; private set; }

        public List<KantokuMstModel> KantokuMstData { get; private set; }

        public List<ByomeiMstAftercareModel> ByomeiMstAftercareData { get; private set; }

        public List<HokenMstModel> HokenMstData { get; private set; }

    }
}
