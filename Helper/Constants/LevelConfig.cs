namespace Helper.Constants
{
    public static class LevelConfig
    {
        public static Dictionary<int, List<string>> AgeSource =
            new Dictionary<int, List<string>>
            {
                {1, new List<string>() { "#ff99cc", "#ff5eaf", "禁忌"} },
                {2, new List<string>() { "#ff99cc", "#ff58ac", "原則禁忌"} },
                {3, new List<string>() { "#ff9999", "#ff5454", "禁忌が望ましい"} },
                {4, new List<string>() { "#ff9999", "#ff5454", "原則禁忌が望ましい"} },
                {5, new List<string>() { "#fabf8f", "#f6984a", "投与回避"} },
                {6, new List<string>() { "#fabf8f", "#f6984a", "有益性投与"} },
                {7, new List<string>() { "#ffff99", "#ffd01a", "慎重投与"} },
                {8, new List<string>() { "#ffffff", "#ffffff", ""} },
                {9, new List<string>() { "#ffffff", "#ffffff", ""} },
                {10, new List<string>() { "#ffff99", "#ffd01a", "安全性未確立"} },
                {0, new List<string>() { "#d8e4bc", "#c8c8c8", "情報なし" } },
                {99, new List<string>() { "#d8e4bc", "#c8c8c8", "収集中" } },
            };

        public static Dictionary<int, List<string>> DiseaseSource =
            new Dictionary<int, List<string>>
            {
                {1, new List<string>() { "#ff99cc", "#ff5eaf", "禁忌" } },
                {2, new List<string>() { "#ff9999", "#ff5454", "原則禁忌" } },
                {3, new List<string>() { "#ffff99", "#ffd01a", "慎重投与" } }
            };

        public static Dictionary<int, List<string>> FoodAllegySource =
            new Dictionary<int, List<string>>
            {
                {1, new List<string>() { "#ff99cc", "#ff5eaf", "禁忌" } },
                {2, new List<string>() { "#ff9999", "#ff5454", "原則禁忌" } },
                {3, new List<string>() { "#ffff99", "#ffd01a", "慎重投与" } }
            };

        public static Dictionary<int, List<string>> DrugAllegySource =
            new Dictionary<int, List<string>>
            {
                {0, new List<string>() { "#ff99cc", "#ff5dae", "同一薬剤" } },
                {1, new List<string>() { "#ff7777", "#ff5dae", "同一成分" } },
                {2, new List<string>() { "#ff7777", "#ff5dae", "同一成分" } },
                {3, new List<string>() { "#ff9999", "#ff6060", "類似成分" } },
                {4, new List<string>() { "#ffff99", "#ffc504", "同一系統" } }
            };

        public static Dictionary<int, List<string>> KinkiCommonSource =
            new Dictionary<int, List<string>>
            {
                {1, new List<string>() {"#ff99cc","#ff68b3", "禁忌" } },
                {2, new List<string>() { "#ff9999", "#ff6868", "原則禁忌" } },
                {3, new List<string>() { "#ffc493", "#f69c51", "重要注意" } },
                {4, new List<string>() { "#ffff99", "#ffe040", "注意" } }
            };

        public static Dictionary<int, List<string>> DuplicationCommonSource =
            new Dictionary<int, List<string>>
            {
                {0, new List<string>() { "#f0f0f0", "#cccdd0", "同一薬剤" } },
                {1, new List<string>() { "#ff99cc", "#ff5dae", "同一成分" } },
                {2, new List<string>() { "#ff99cc", "#ff5dae", "同一成分" } },
                {3, new List<string>() { "#ff9999", "#ff6060", "類似成分" } },
                {4, new List<string>() { "#ffff99", "#ffc504", "同一系統" } }
            };
    }
}

