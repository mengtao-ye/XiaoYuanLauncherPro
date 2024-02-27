using Game;
using UnityEngine;
using YFramework;

namespace Game
{
    public static class PhysicsModule
    {
        private static Ray mRay;
        private static RaycastHit mHit;
        private static Camera mCamera;
        /// <summary>
        /// 设置主相机
        /// </summary>
        /// <param name="camera"></param>
        public static void SetCamera(Camera camera) 
        {
            if (camera == null)
            {
                LogHelper.LogError("相机不能为空");
                return;
            }
            mCamera = camera;
        }
        /// <summary>
        /// 获取相机射到的对象
        /// </summary>
        public static GameObject RayCastTarget
        {
            get {
                if (mCamera == null)
                {
                    LogHelper.LogError("请设置相机");
                    return null;
                }
                mRay = mCamera.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(mRay, out mHit, PhysicsData.MAX_DISTANCE)) {
                    return mHit.collider.gameObject;
                }
                return null;
            }
        }

        /// <summary>
        /// 获取相机射到的位置
        /// </summary>
        public static Vector3 RayCastTargetPoint
        {
            get
            {
                if (mCamera == null)
                {
                    LogHelper.LogError("请设置相机");
                    return Vector3.zero;
                }
                mRay = mCamera.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(mRay, out mHit, PhysicsData.MAX_DISTANCE))
                {
                    return mHit.point;
                }
                return Vector3.zero;
            }
        }


        /// <summary>
        /// 获取相机射到的地面位置
        /// </summary>
        public static Vector3 RayCastTargetGroundPoint
        {
            get
            {
                if (mCamera == null)
                {
                    LogHelper.LogError("请设置相机");
                    return Vector3.zero;
                }
                mRay = mCamera.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(mRay, out mHit, PhysicsData.MAX_DISTANCE))
                {
                    if(mHit.collider.name == "Ground")
                    {
                        return mHit.point;
                    }
                }
                return Vector3.zero;
            }
        }
    }
}
