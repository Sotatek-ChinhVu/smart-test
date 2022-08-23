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
            KantokuMstData = new List<KantokuMstModel>();
            ByomeiMstAftercareData = new List<ByomeiMstAftercareModel>();
            HokenMstData = new List<HokenMstModel>();
            RoudouMst = new List<RoudouMstModel>();
        }

        public InsuranceMstModel(List<TokkiMstModel> listTokkiMstModel, Dictionary<int, string> hokenKogakuKbnDict, List<HokenMstModel> kohiHokenMstData, List<KantokuMstModel> kantokuMstData, List<ByomeiMstAftercareModel> byomeiMstAftercareData, List<HokenMstModel> hokenMstData, List<RoudouMstModel> roudouMst)
        {
            ListTokkiMstModel = listTokkiMstModel;
            HokenKogakuKbnDict = hokenKogakuKbnDict;
            KohiHokenMstData = kohiHokenMstData;
            KantokuMstData = kantokuMstData;
            ByomeiMstAftercareData = byomeiMstAftercareData;
            HokenMstData = hokenMstData;
            RoudouMst = roudouMst;
        }

        public List<TokkiMstModel> ListTokkiMstModel { get; private set; }

        public Dictionary<int, string> HokenKogakuKbnDict { get; private set; }

        public List<HokenMstModel> KohiHokenMstData { get; private set; }

        public List<KantokuMstModel> KantokuMstData { get; private set; }

        public List<ByomeiMstAftercareModel> ByomeiMstAftercareData { get; private set; }

        public List<HokenMstModel> HokenMstData { get; private set; }

        public List<RoudouMstModel> RoudouMst { get; private set; }

    }
}
