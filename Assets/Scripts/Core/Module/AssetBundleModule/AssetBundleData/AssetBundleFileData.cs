using System.Collections.Generic;
using YFramework;
using static YFramework.Utility;

namespace Game
{
    public class AssetBundleFileData : IDataConverter
    {
        public long size;//总文件大小
        public int version;//版本
        public IList<string> fileNames;//文件名称
        public AssetBundleFileData()
        {
            fileNames = new List<string>();
        }

        public byte[] ToBytes()
        {
            IListData<byte[]> list = ClassPool<ListData<byte[]>>.Pop();
            list.Add(size.ToBytes());
            list.Add(version.ToBytes());
            byte[] fileNamesBytes = ListTools.GetBytes(fileNames);
            if (fileNamesBytes != null)
            {
                list.Add(fileNamesBytes);
            }
            byte[] returnBytes = list.list.ToBytes();
            list.Recycle();
            return returnBytes;
        }

        public void ToValue(byte[] data)
        {
            if (data.IsNullOrEmpty()) return;
            IListData<byte[]> list = data.ToListBytes();
            size = list[0].ToLong();
            version = list[1].ToInt();
            if (list.Count == 3)
            {
                fileNames = ListTools.ToListString(list[2]);
            }
            list.Recycle();
        }
    }
}
