// ------------------------------------------------------------
// @file       StartPanel.cs
// @brief
// @author     zheliku
// @Modified   2024-10-15 14:10:43
// @Copyright  Copyright (c) 2024, zheliku
// ------------------------------------------------------------

namespace Framework3.Core.Example._7.PointPointPoint.Scripts.View.UI
{
    using Command;
    using Model;
    using TMPro;
    using UnityEngine.UI;

    public class StartPanel : AbstractView
    {
        private Button _btnBuyLife;

        private Button _btnStart;

        private IGameModel _gameModel;

        private TextMeshProUGUI _txtBestScoreValue;
        private TextMeshProUGUI _txtGoldValue;
        private TextMeshProUGUI _txtLifeValue;

        protected override IArchitecture _Architecture
        {
            get => PointGame.Architecture;
        }

        protected override void Awake()
        {
            base.Awake();

            _gameModel = this.GetModel<IGameModel>();

            _btnStart   = transform.Find("btnStart").GetComponent<Button>();
            _btnBuyLife = transform.Find("btnBuyLife").GetComponent<Button>();

            _txtBestScoreValue = transform.Find("BestScore/Value").GetComponent<TextMeshProUGUI>();
            _txtLifeValue      = transform.Find("Life/Value").GetComponent<TextMeshProUGUI>();
            _txtGoldValue      = transform.Find("Gold/Value").GetComponent<TextMeshProUGUI>();

            _gameModel.Gold.RegisterWithInitValue(OnGoldValueChanged).UnregisterWhenGameObjectDestroyed(gameObject);
            _gameModel.Life.RegisterWithInitValue(OnLifeValueChanged).UnregisterWhenGameObjectDestroyed(gameObject);

            _btnStart.onClick.AddListener(OnBtnStartClick);
            _btnBuyLife.onClick.AddListener(OnBtnBuyLifeClick);

            _txtBestScoreValue.text = _gameModel.BestScore.Value.ToString();
        }

        private void OnDestroy()
        {
            _gameModel = null;
        }

        private void OnGoldValueChanged(int oldGold, int gold)
        {
            _btnBuyLife.gameObject.SetActive(gold > 0);
            _txtGoldValue.text = gold.ToString();
        }

        private void OnLifeValueChanged(int oldLife, int life)
        {
            _txtLifeValue.text = life.ToString();
        }

        private void OnBtnStartClick()
        {
            gameObject.SetActive(false);
            this.SendCommand<StartGameCommand>();
        }

        private void OnBtnBuyLifeClick()
        {
            this.SendCommand<BuyLifeCommand>();
        }
    }
}