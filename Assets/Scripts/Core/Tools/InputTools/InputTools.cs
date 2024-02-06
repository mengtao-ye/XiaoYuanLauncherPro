using UnityEngine;

namespace Game
{
    public static class InputTools
    {
        /// <summary>
        /// 一个手指放到屏幕上、时
        /// </summary>
        public static bool OneTouch 
        {
            get {
                return Input.touchCount == 1 || Input.GetMouseButton(0); 
            }
        }

        /// <summary>
        /// 是否触摸到屏幕上
        /// </summary>
        public static bool IsTouchScreen 
        {
            get {
                return Input.touchCount > 0 || Input.GetMouseButton(0);
            }
        }
        /// <summary>
        /// 获取屏幕水平滑动值
        /// </summary>
        public static float MouseX 
        {
            get
            {
#if UNITY_ANDROID && !UNITY_EDITOR
                if (OneTouch)
                {
                    return Input.touches[0].deltaPosition.x;
                }
#endif

#if UNITY_EDITOR
                if (OneTouch)
                {
                    return Input.GetAxis("Mouse X");
                }
#endif
                return 0;
            }
        }
        /// <summary>
        /// 获取屏幕垂直滑动值
        /// </summary>
        public static float MouseY
        {
            get
            {
#if UNITY_ANDROID && !UNITY_EDITOR
                if (OneTouch)
                {
                    return Input.touches[0].deltaPosition.y;
                }
#endif

#if UNITY_EDITOR
                if (OneTouch)
                {
                    return Input.GetAxis("Mouse Y");
                }
#endif
                return 0;
            }
        }
    }
}
