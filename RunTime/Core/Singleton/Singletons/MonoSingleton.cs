// ------------------------------------------------------------
// @file       MonoSingleton.cs
// @brief
// @author     zheliku
// @Modified   2024-10-20 18:10:48
// @Copyright  Copyright (c) 2024, zheliku
// ------------------------------------------------------------

namespace Framework3.Core
{
    public abstract class MonoSingleton<TMonoSingleton> : AbstractView, ISingleton where TMonoSingleton : MonoSingleton<TMonoSingleton>
    {
        /// <summary>
        /// 静态实例
        /// </summary>
        protected static TMonoSingleton s_instance;

        /// <summary>
        /// 静态属性：封装相关实例对象
        /// </summary>
        public static TMonoSingleton Instance
        {
            get
            {
                if (s_instance == null)
                {
                    s_instance = SingletonCreator.CreateMonoSingleton<TMonoSingleton>();
                }

                return s_instance;
            }
        }

        /// <summary>
        /// 实现接口的单例初始化
        /// </summary>
        public virtual void OnSingletonInit()
        { }

        /// <summary>
        /// 资源释放
        /// </summary>
        public virtual void Dispose()
        {
            Destroy(gameObject);
        }

        protected virtual void Update()
        {
#if UNITY_EDITOR

            // 强制刷新 Inspector GUI
            UnityEditor.EditorUtility.SetDirty(this);
#endif
        }

        /// <summary>
        /// 应用程序退出：释放当前对象并销毁相关 GameObject
        /// </summary>
        protected virtual void OnApplicationQuit()
        {
            if (s_instance == null)
                return;

            Destroy(s_instance.gameObject);
            s_instance = null;
        }

        /// <summary>
        /// 释放当前对象
        /// </summary>
        protected virtual void OnDestroy()
        {
            s_instance = null;
        }
        
        /// <summary>
        /// 默认没有 _Architecture
        /// </summary>
        protected override IArchitecture _Architecture { get => null; }
    }
}