namespace Reporting.Common
{
    public class SystemConfig
    {
        private static SystemConfig? instance;
        public static SystemConfig Instance {
            get
            {
                if (instance == null)
                {
                    instance = new SystemConfig();
                }
                return instance;
            }
        
        }

        private SystemConfig()
        {

        }

        public int SijisenRpName { get; set; }

        public int JyusinHyoRpName { get; set; }
        
        public int SijisenAlrgy { get; set; }

        public int JyusinHyoAlrgy { get; set; } 
        
        public int SijisenPtCmt { get; set; }

        public int JyusinHyoPtCmt { get; set; }
        
        public int SijisenKensaYokiZairyo { get; set; }

        public int JyusinHyoKensaYokiZairyo { get; set; }

        public string JyusinHyoRaiinKbn { get; set; } = string.Empty; 
    }
}
