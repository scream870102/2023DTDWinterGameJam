using BIGJ2023.Common;
using Scream.UniMO.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BIGJ2023.GameSystem
{
    public class GameController : MonoBehaviour
    {
        [SerializeField] private string triggerParam = "Result";
        private void Awake()
        {
            DomainEvents.Register<OnWinnerDetermine>(OnWinnerDetemineEvent);
        }

        private void OnDestroy()
        {
            DomainEvents.UnRegister<OnWinnerDetermine>(OnWinnerDetemineEvent);
        }

        private void OnWinnerDetemineEvent(OnWinnerDetermine param)
        {
            int winnerIndex = PlayerManager.Instance.GetPlayerIndex(param.Winner);
            UIController.WinnerIndex = winnerIndex;
            GameFlow.Instance.Control.SetTrigger(triggerParam);
        }
    }
}

