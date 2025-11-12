// ------------------------------------------------------------
// @file       IStateBasicUsageExample.cs
// @brief
// @author     zheliku
// @Modified   2024-10-20 23:10:25
// @Copyright  Copyright (c) 2024, zheliku
// ------------------------------------------------------------

namespace Framework3.Toolkits.FSMKit.Example._0.BasicUsage
{
    using TMPro;
    using UnityEngine;
#if ODIN_INSPECTOR
    using Sirenix.OdinInspector;
#endif

    public class IStateBasicUsageExample : MonoBehaviour
    {
        public enum States
        {
            A,
            B
        }

        [SerializeField]
        private TextMeshProUGUI _textInfo;
    #if ODIN_INSPECTOR
        [ShowInInspector]
    #endif
        private FSM<States> _fsm = new();

        private void Start()
        {
            Application.targetFrameRate = 60;

            _fsm.OnStateChanged((previousState, nextState) =>
            {
                Debug.Log($"{previousState} => {nextState}");
            });

            _fsm.State(States.A)
               .OnCondition(() => _fsm.CurrentStateId == States.B)
               .OnEnter(() =>
                {
                    Debug.Log("Enter A");
                    _textInfo.text = "Current State: A";
                })
               .OnUpdate(() =>
                {
                    if (_fsm.FrameCountOfCurrentState % 60 == 0)
                    {
                        Debug.Log("Heart beat");
                    }
                })
               .OnExit(() => { Debug.Log("Exit A"); });

            _fsm.State(States.B)
               .OnEnter(() =>
                {
                    Debug.Log("Enter B");
                    _textInfo.text = "Current State: B";
                })
               .OnCondition(() => _fsm.CurrentStateId == States.A)
               .OnExit(() => { Debug.Log("Exit A"); });

            _fsm.StartState(States.A);
        }

        private void Update()
        {
            _fsm.Update();
        }

        private void FixedUpdate()
        {
            _fsm.FixedUpdate();
        }

        private void OnDestroy()
        {
            _fsm.Clear();
        }

        private void OnGUI()
        {
            _fsm.OnGUI();
        }

        public void ChangeToStateA()
        {
            _fsm.ChangeState(States.A);
        }

        public void ChangeToStateB()
        {
            _fsm.ChangeState(States.B);
        }
    }
}