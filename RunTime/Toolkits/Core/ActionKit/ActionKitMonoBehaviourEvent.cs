// ------------------------------------------------------------
// @file       ActionKitMonoBehaviourEvents.cs
// @brief
// @author     zheliku
// @Modified   2024-10-24 22:10:30
// @Copyright  Copyright (c) 2024, zheliku
// ------------------------------------------------------------

namespace Framework3.Toolkits.ActionKit
{
    using System;
    using System.Collections;
    using Core;
    using SingletonKit;
#if ODIN_INSPECTOR
    using Sirenix.OdinInspector;
#endif

    /// <summary>
    ///     用于 ActionKit 的 MonoBehaviour 事件
    /// </summary>
    public class ActionKitMonoBehaviourEvent : MonoSingleton<ActionKitMonoBehaviourEvent>
    {
    #if ODIN_INSPECTOR
        [ShowInInspector]
    #endif
        public readonly EasyEvent<bool> OnApplicationFocusEvent = new();

    #if ODIN_INSPECTOR
        [ShowInInspector]
    #endif
        public readonly EasyEvent<bool> OnApplicationPauseEvent = new();

    #if ODIN_INSPECTOR
        [ShowInInspector]
    #endif
        public readonly EasyEvent OnApplicationQuitEvent = new();

    #if ODIN_INSPECTOR
        [ShowInInspector]
    #endif
        public readonly EasyEvent OnFixedUpdate = new();

    #if ODIN_INSPECTOR
        [ShowInInspector]
    #endif
        public readonly EasyEvent OnGUIEvent = new();

    #if ODIN_INSPECTOR
        [ShowInInspector]
    #endif
        public readonly EasyEvent OnLateUpdate = new();
    #if ODIN_INSPECTOR
        [ShowInInspector]
    #endif
        public readonly EasyEvent OnUpdate = new();

        protected override void Update()
        {
            base.Update();

            OnUpdate?.Trigger();
        }

        private void FixedUpdate()
        {
            OnFixedUpdate?.Trigger();
        }

        private void LateUpdate()
        {
            OnLateUpdate?.Trigger();
        }

        private void OnGUI()
        {
            OnGUIEvent?.Trigger();
        }

        private void OnApplicationFocus(bool hasFocus)
        {
            OnApplicationFocusEvent?.Trigger(hasFocus);
        }

        private void OnApplicationPause(bool pauseStatus)
        {
            OnApplicationPauseEvent?.Trigger(pauseStatus);
        }

        protected override void OnApplicationQuit()
        {
            OnApplicationQuitEvent?.Trigger();
            base.OnApplicationQuit();
        }

        public void ExecuteCoroutine(IEnumerator coroutine, Action onFinish)
        {
            StartCoroutine(DoExecuteCoroutine(coroutine, onFinish));
        }

        private IEnumerator DoExecuteCoroutine(IEnumerator coroutine, Action onFinish)
        {
            yield return coroutine;
            onFinish?.Invoke();
        }
    }
}