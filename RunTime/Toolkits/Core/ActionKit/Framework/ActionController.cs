// ------------------------------------------------------------
// @file       ActionController.cs
// @brief
// @author     zheliku
// @Modified   2024-10-24 20:10:37
// @Copyright  Copyright (c) 2024, zheliku
// ------------------------------------------------------------

namespace Framework3.Toolkits.ActionKit
{
    using UnityEngine.Pool;

    public class ActionController : IActionController
    {
        private static readonly ObjectPool<IActionController> s_pool = new(
            () => new ActionController(),
            null,
            controller =>
            {
                controller.UpdateMode = ActionUpdateMode.ScaledDeltaTime;
                controller.ActionID   = 0;
                controller.Action     = null;
            },
            null,
            true,
            10,
            100);

        public ulong ActionID { get; set; }

        public IAction Action { get; set; }

        public ActionUpdateMode UpdateMode { get; set; }

        public bool Paused
        {
            get => Action.Paused;
            set => Action.Paused = value;
        }

        public void Reset()
        {
            if (Action.ActionID == ActionID)
            {
                Action.Reset();
            }
        }

        public void Deinit()
        {
            if (Action != null && Action.ActionID == ActionID)
            {
                Action.Deinit();
            }
        }

        public void Recycle()
        {
            s_pool.Release(this);
        }

        public static IActionController Spawn()
        {
            return s_pool.Get() as ActionController;
        }
    }
}