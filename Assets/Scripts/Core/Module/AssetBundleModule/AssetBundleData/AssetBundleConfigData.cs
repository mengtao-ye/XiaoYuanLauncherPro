using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using YFramework;
using static YFramework.Utility;

namespace Game
{
    [System.Serializable]
    public class AssetBundleConfigData : IDataConverter
    {
        [XmlElement("ABList")]
        public List<ABBase> ABList;

        public byte[] ToBytes()
        {
            return ABList.ToBytes();
        }

        public void ToValue(byte[] data)
        {
            IListData<ABBase> abBase = data.ToListDataBytes<ABBase>();
            ABList = new List<ABBase>(abBase.Count);
            for (int i = 0; i < abBase.Count; i++)
            {
                ABList.Add(abBase[i]);
            }
        }
    }

    [System.Serializable]
    public class ABBase : IDataConverter
    {
        [XmlAttribute("Path")]
        public string Path { get; set; }
        [XmlAttribute("ABName")]
        public string ABName { get; set; }
        [XmlAttribute("AssetName")]
        public string AssetName { get; set; }
        [XmlAttribute("CRC")]
        public ulong CRC { get; set; }
        [XmlElement("Dependence")]
        public List<string> Dependence { get; set; }

        public byte[] ToBytes()
        {
            IListData<byte[]> list = ClassPool<ListData<byte[]>>.Pop();
            list.Add(Path.ToBytes());
            list.Add(ABName.ToBytes());
            list.Add(AssetName.ToBytes());
            list.Add(CRC.ToBytes());
            if (Dependence.IsNullOrEmpty())
            {
                list.Add(BytesConst.NULL_BYTES);
            }
            else 
            {
                IListData<byte[]> listData = ClassPool<ListData<byte[]>>.Pop();
                for (int i = 0; i < Dependence.Count; i++)
                {
                    listData.Add(Dependence[i].ToBytes());
                }
                list.Add(listData.list.ToBytes());
                listData.Recycle();
            }
           
            byte[] data = list.list.ToBytes();
            list.Recycle();
            return data;
        }

        public void ToValue(byte[] data)
        {
            IListData<byte[]> list = data.ToListBytes();
            Path = list[0].ToStr();
            ABName = list[1].ToStr();
            AssetName = list[2].ToStr();
            CRC = list[3].ToULong();
            if (list[4].IsNullOrEmpty())
            {
                Dependence = null;
            }
            else 
            {
                Dependence = new List<string>();
                IListData<byte[]> dependenceList = list[4].ToListBytes();
                for (int i = 0; i < dependenceList.Count; i++)
                {
                    Dependence.Add(dependenceList[i].ToStr());
                }
            }
            list.Recycle();
        }
    }
}
