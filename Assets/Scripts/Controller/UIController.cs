using BIGJ2023.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BIGJ2023.GameSystem
{
    public class UIController : MonoBehaviour
    {
        public static int WinnerIndex = -1;
        [SerializeField] private Sprite[] winnerSprites;
        [SerializeField] private Image winnerImg;
        [SerializeField] private string backToTitleParam;
        [SerializeField] private string continueParam;
        [SerializeField] private string defaultParam;
        private void Start()
        {
            if (WinnerIndex != -1)
            {
                Sprite sprite = winnerSprites[WinnerIndex];
                winnerImg.sprite = sprite;
            }
        }
        public void OnContinueButtonClicked()
        {
            GameFlow.Instance.Control.SetTrigger(continueParam);
        }

        public void OnExitButtonClicked()
        {
            Application.Quit();
        }


        public void OnPlayAgainButtonClicked()
        {
            GameFlow.Instance.Control.SetTrigger(continueParam);
        }

        public void OAnimFin()
        {
            GameFlow.Instance.Control.SetTrigger(defaultParam);
        }
    }
}

