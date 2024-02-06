using UnityEngine;

namespace Game
{
    /// <summary>
    /// 层级数据
    /// </summary>
    public enum LayerMaskData
    {

        Default = 1,//默认层级
        TransparentFX = 2,//人物移动层级
        IgnoreRaycast = 4,//射线检测忽略层级
        MipMapLayer = 8,//小地图相机层级
        Water = 16,//水层级
        UI = 32,//UI层级

        /// <summary>
        /// 所有层级
        /// </summary>
        FullLayerMask  = Default | TransparentFX| IgnoreRaycast| MipMapLayer| Water| UI,
        /// <summary>
        /// 默认相机视图层
        /// </summary>
        DefaultLayerMask = Default | TransparentFX | IgnoreRaycast| Water | UI,
     
      
    }
}
