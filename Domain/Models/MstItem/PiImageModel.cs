namespace Domain.Models.MstItem
{
    public class PiImageModel
    {
        public PiImageModel(int hpId, int imageType, string itemCd, string fileName, string imagePath, bool isModified, bool isDeleted)
        {
            HpId = hpId;
            ImageType = imageType;
            ItemCd = itemCd;
            FileName = fileName;
            ImagePath = imagePath;
            IsModified = isModified;
            IsDeleted = isDeleted;
        }


        /// <summary>
        /// 医療機関識別ID
        /// 
        /// </summary>
        public int HpId { get; private set; }

        /// <summary>
        /// 包装剤形区分
        /// 0:剤形 1:包装
        /// </summary>
        public int ImageType { get; private set; }

        /// <summary>
        /// 項目コード
        /// 
        /// </summary>
        public string ItemCd { get; private set; }

        /// <summary>
        /// ファイル名
        /// 
        /// </summary>
        public string FileName { get; private set; }

        public string ImagePath { get; private set; }

        public bool IsNewModel
        {
            get => HpId == 0;
        }

        public bool IsModified { get; private set; }

        public bool IsDefaultModel
        {
            get => IsNewModel && string.IsNullOrEmpty(FileName);
        }

        public bool IsEnable
        {
            get => !string.IsNullOrEmpty(ImagePath);
        }

        public bool IsDeleted { get; private set; }
    }
}
