// ------------------------------------------------------------
// @file       InputActionExtension.cs
// @brief
// @author     zheliku
// @Modified   2024-12-26 23:12:59
// @Copyright  Copyright (c) 2024, zheliku
// ------------------------------------------------------------

namespace Framework3.Toolkits.InputKit
{
    using System;
    using EventKit;
    using FluentAPI;
    using UnityEngine;
    using UnityEngine.InputSystem;

    /// <summary>
    /// 请使用 InputAction 封装方法，用于更新 Inspector 面板中显示的 Mono
    /// </summary>
    public static class InputActionExtension
    {
        public static InputAction RegisterPerformed(this InputAction self, Action<InputAction.CallbackContext> action)
        {
            self.performed += action;
            InputMgr.GetInputActionTracker(self).PerformedActions.Add(action);
            return self;
        }

        public static InputAction UnregisterPerformed(this InputAction self, Action<InputAction.CallbackContext> action)
        {
            self.performed -= action;
            InputMgr.GetInputActionTracker(self).PerformedActions.Remove(action);
            return self;
        }

        public static InputAction UnregisterPerformedAll(this InputAction self)
        {
            var actions = InputMgr.GetInputActionTracker(self).PerformedActions;
            foreach (var action in actions)
            {
                self.performed -= action;
            }
            actions.Clear();
            return self;
        }

        public static InputAction RegisterStarted(this InputAction self, Action<InputAction.CallbackContext> action)
        {
            self.started += action;
            InputMgr.GetInputActionTracker(self).StartedActions.Add(action);
            return self;
        }

        public static InputAction UnregisterStarted(this InputAction self, Action<InputAction.CallbackContext> action)
        {
            self.started -= action;
            InputMgr.GetInputActionTracker(self).StartedActions.Remove(action);
            return self;
        }

        public static InputAction UnregisterStartedAll(this InputAction self)
        {
            var actions = InputMgr.GetInputActionTracker(self).StartedActions;
            foreach (var action in actions)
            {
                self.started -= action;
            }
            actions.Clear();
            return self;
        }

        public static InputAction RegisterCanceled(this InputAction self, Action<InputAction.CallbackContext> action)
        {
            self.canceled += action;
            InputMgr.GetInputActionTracker(self).CanceledActions.Add(action);
            return self;
        }

        public static InputAction UnregisterCanceled(this InputAction self, Action<InputAction.CallbackContext> action)
        {
            self.canceled -= action;
            InputMgr.GetInputActionTracker(self).CanceledActions.Remove(action);
            return self;
        }

        public static InputAction UnregisterCanceledAll(this InputAction self)
        {
            var actions = InputMgr.GetInputActionTracker(self).CanceledActions;
            foreach (var action in actions)
            {
                self.canceled -= action;
            }
            actions.Clear();
            return self;
        }

        public static InputAction UnregisterAll(this InputAction self)
        {
            self.UnregisterPerformedAll();
            self.UnregisterStartedAll();
            self.UnregisterCanceledAll();
            return self;
        }

        public static InputAction UnregisterPerformedWhenGameObjectDisabled(this InputAction self, GameObject target, Action<InputAction.CallbackContext>
                                                                            action,            int        priority = 0)
        {
            target.GetOrAddComponent<OnDisableEventTrigger>().OnDisableEvent.Register(() =>
            {
                self.UnregisterPerformed(action);
            }, priority);
            return self;
        }

        public static InputAction UnregisterPerformedWhenGameObjectDisabled(this InputAction self, Component target, Action<InputAction.CallbackContext>
                                                                            action,            int       priority = 0)
        {
            target.GetOrAddComponent<OnDisableEventTrigger>().OnDisableEvent.Register(() =>
            {
                self.UnregisterPerformed(action);
            }, priority);
            return self;
        }

        public static InputAction UnregisterPerformedWhenGameObjectDestroyed(this InputAction self, GameObject target, Action<InputAction.CallbackContext> action, float priority = 0)
        {
            target.GetOrAddComponent<OnDestroyEventTrigger>().OnDestroyEvent.Register(() =>
            {
                self.UnregisterPerformed(action);
            }, priority);
            return self;
        }

        public static InputAction UnregisterPerformedWhenGameObjectDestroyed(this InputAction self, Component target, Action<InputAction.CallbackContext> action, float priority = 0)
        {
            target.GetOrAddComponent<OnDestroyEventTrigger>().OnDestroyEvent.Register(() =>
            {
                self.UnregisterPerformed(action);
            }, priority);
            return self;
        }

        public static InputAction UnregisterPerformedAllWhenGameObjectDisabled(this InputAction self, GameObject target, float priority = 0)
        {
            target.GetOrAddComponent<OnDisableEventTrigger>().OnDisableEvent.Register(() =>
            {
                self.UnregisterPerformedAll();
            }, priority);
            return self;
        }

        public static InputAction UnregisterPerformedAllWhenGameObjectDisabled(this InputAction self, Component target, float priority = 0)
        {
            target.GetOrAddComponent<OnDisableEventTrigger>().OnDisableEvent.Register(() =>
            {
                self.UnregisterPerformedAll();
            }, priority);
            return self;
        }

        public static InputAction UnregisterPerformedAllWhenGameObjectDestroyed(this InputAction self, GameObject target, float priority = 0)
        {
            target.GetOrAddComponent<OnDestroyEventTrigger>().OnDestroyEvent.Register(() =>
            {
                self.UnregisterPerformedAll();
            }, priority);
            return self;
        }
        
        public static InputAction UnregisterPerformedAllWhenGameObjectDestroyed(this InputAction self, Component target, float priority = 0)
        {
            target.GetOrAddComponent<OnDestroyEventTrigger>().OnDestroyEvent.Register(() =>
            {
                self.UnregisterPerformedAll();
            }, priority);
            return self;
        }

        public static InputAction UnregisterStartedWhenGameObjectDisabled(this InputAction self, GameObject target, Action<InputAction.CallbackContext> action, float priority = 0)
        {
            target.GetOrAddComponent<OnDisableEventTrigger>().OnDisableEvent.Register(() =>
            {
                self.UnregisterStarted(action);
            }, priority);
            return self;
        }
        
        public static InputAction UnregisterStartedWhenGameObjectDisabled(this InputAction self, Component target, Action<InputAction.CallbackContext> action, float priority = 0)
        {
            target.GetOrAddComponent<OnDisableEventTrigger>().OnDisableEvent.Register(() =>
            {
                self.UnregisterStarted(action);
            }, priority);
            return self;
        }

        public static InputAction UnregisterStartedWhenGameObjectDestroyed(this InputAction self, GameObject target, Action<InputAction.CallbackContext> action, float priority = 0)
        {
            target.GetOrAddComponent<OnDestroyEventTrigger>().OnDestroyEvent.Register(() =>
            {
                self.UnregisterStarted(action);
            }, priority);
            return self;
        }
        
        public static InputAction UnregisterStartedWhenGameObjectDestroyed(this InputAction self, Component target, Action<InputAction.CallbackContext> action, float priority = 0)
        {
            target.GetOrAddComponent<OnDestroyEventTrigger>().OnDestroyEvent.Register(() =>
            {
                self.UnregisterStarted(action);
            }, priority);
            return self;
        }

        public static InputAction UnregisterStartedAllWhenGameObjectDisabled(this InputAction self, GameObject target, float priority = 0)
        {
            target.GetOrAddComponent<OnDisableEventTrigger>().OnDisableEvent.Register(() =>
            {
                self.UnregisterStartedAll();
            }, priority);
            return self;
        }
        
        public static InputAction UnregisterStartedAllWhenGameObjectDisabled(this InputAction self, Component target, float priority = 0)
        {
            target.GetOrAddComponent<OnDisableEventTrigger>().OnDisableEvent.Register(() =>
            {
                self.UnregisterStartedAll();
            }, priority);
            return self;
        }

        public static InputAction UnregisterStartedAllWhenGameObjectDestroyed(this InputAction self, GameObject target, float priority = 0)
        {
            target.GetOrAddComponent<OnDestroyEventTrigger>().OnDestroyEvent.Register(() =>
            {
                self.UnregisterStartedAll();
            }, priority);
            return self;
        }
        
        public static InputAction UnregisterStartedAllWhenGameObjectDestroyed(this InputAction self, Component target, float priority = 0)
        {
            target.GetOrAddComponent<OnDestroyEventTrigger>().OnDestroyEvent.Register(() =>
            {
                self.UnregisterStartedAll();
            }, priority);
            return self;
        }

        public static InputAction UnregisterCanceledWhenGameObjectDisabled(this InputAction self, GameObject target, Action<InputAction.CallbackContext> action, float priority = 0)
        {
            target.GetOrAddComponent<OnDisableEventTrigger>().OnDisableEvent.Register(() =>
            {
                self.UnregisterCanceled(action);
            }, priority);
            return self;
        }
        
        public static InputAction UnregisterCanceledWhenGameObjectDisabled(this InputAction self, Component target, Action<InputAction.CallbackContext> action, float priority = 0)
        {
            target.GetOrAddComponent<OnDisableEventTrigger>().OnDisableEvent.Register(() =>
            {
                self.UnregisterCanceled(action);
            }, priority);
            return self;
        }

        public static InputAction UnregisterCanceledWhenGameObjectDestroyed(this InputAction self, GameObject target, Action<InputAction.CallbackContext> action, float priority = 0)
        {
            target.GetOrAddComponent<OnDestroyEventTrigger>().OnDestroyEvent.Register(() =>
            {
                self.UnregisterCanceled(action);
            }, priority);
            return self;
        }
        
        public static InputAction UnregisterCanceledWhenGameObjectDestroyed(this InputAction self, Component target, Action<InputAction.CallbackContext> action, float priority = 0)
        {
            target.GetOrAddComponent<OnDestroyEventTrigger>().OnDestroyEvent.Register(() =>
            {
                self.UnregisterCanceled(action);
            }, priority);
            return self;
        }

        public static InputAction UnregisterCanceledAllWhenGameObjectDisabled(this InputAction self, GameObject target, float priority = 0)
        {
            target.GetOrAddComponent<OnDisableEventTrigger>().OnDisableEvent.Register(() =>
            {
                self.UnregisterCanceledAll();
            }, priority);
            return self;
        }
        
        public static InputAction UnregisterCanceledAllWhenGameObjectDisabled(this InputAction self, Component target, float priority = 0)
        {
            target.GetOrAddComponent<OnDisableEventTrigger>().OnDisableEvent.Register(() =>
            {
                self.UnregisterCanceledAll();
            }, priority);
            return self;
        }

        public static InputAction UnregisterCanceledAllWhenGameObjectDestroyed(this InputAction self, GameObject target, float priority = 0)
        {
            target.GetOrAddComponent<OnDestroyEventTrigger>().OnDestroyEvent.Register(() =>
            {
                self.UnregisterCanceledAll();
            }, priority);
            return self;
        }

        public static InputAction UnregisterCanceledAllWhenGameObjectDestroyed(this InputAction self, Component target, float priority = 0)
        {
            target.GetOrAddComponent<OnDestroyEventTrigger>().OnDestroyEvent.Register(() =>
            {
                self.UnregisterCanceledAll();
            }, priority);
            return self;
        }
        
        public static InputAction UnregisterAllWhenGameObjectDisabled(this InputAction self, GameObject target, float priority = 0)
        {
            target.GetOrAddComponent<OnDisableEventTrigger>().OnDisableEvent.Register(() =>
            {
                self.UnregisterAll();
            }, priority);
            return self;
        }
        
        public static InputAction UnregisterAllWhenGameObjectDisabled(this InputAction self, Component target, float priority = 0)
        {
            target.GetOrAddComponent<OnDisableEventTrigger>().OnDisableEvent.Register(() =>
            {
                self.UnregisterAll();
            }, priority);
            return self;
        }

        public static InputAction UnregisterAllWhenGameObjectDestroyed(this InputAction self, GameObject target, float priority = 0)
        {
            target.GetOrAddComponent<OnDestroyEventTrigger>().OnDestroyEvent.Register(() =>
            {
                self.UnregisterAll();
            }, priority);
            return self;
        }
        
        public static InputAction UnregisterAllWhenGameObjectDestroyed(this InputAction self, Component target, float priority = 0)
        {
            target.GetOrAddComponent<OnDestroyEventTrigger>().OnDestroyEvent.Register(() =>
            {
                self.UnregisterAll();
            }, priority);
            return self;
        }
    }
}