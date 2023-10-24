namespace Domain.Models.MstItem
{
    public class PiImageModel
    {
        public PiImageModel(int hpId, int imageType, string itemCd, string fileName, bool isModified, bool isDeleted, string imagePath)
        {
            HpId = hpId;
            ImageType = imageType;
            ItemCd = itemCd;
            FileName = fileName;
            ImagePath = string.Empty;
            IsModified = isModified;
            IsDeleted = isDeleted;
            ImagePath = imagePath;
        }

        public PiImageModel(int hpId , string itemCd, int imageType)
        {
            HpId = hpId;
            ImageType = imageType;
            ItemCd = itemCd;
            FileName = string.Empty;
            ImagePath = string.Empty;
            IsModified = false;
            IsDeleted = false;
        }

        public PiImageModel()
        {
            ItemCd = string.Empty;
            FileName = string.Empty;
            ImagePath = string.Empty;
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

        public void SetImagePath(string value)
        {
            ImagePath = value;
        }

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
