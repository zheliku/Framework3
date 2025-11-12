// ------------------------------------------------------------
// @file       LosePanel.cs
// @brief
// @author     zheliku
// @Modified   2024-10-15 15:10:08
// @Copyright  Copyright (c) 2024, zheliku
// ------------------------------------------------------------

namespace Framework3.Core.Example._7.PointPointPoint.Scripts.View.UI
{
    using Command;
    using UnityEngine.UI;

    public class LosePanel : AbstractView
    {
        private Button _btnBack;

        protected override IArchitecture _Architecture
        {
            get => PointGame.Architecture;
        }

        protected override void Awake()
        {
            base.Awake();

            _btnBack = transform.Find("btnBack").GetComponent<Button>();

            _btnBack.onClick.AddListener(OnBtnBackClick);
        }

        private void OnBtnBackClick()
        {
            gameObject.SetActive(false);
            this.SendCommand<ReturnMenuCommand>();
        }
    }
}