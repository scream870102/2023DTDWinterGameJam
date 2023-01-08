using BIGJ2023.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BIGJ2023
{
    public class PressAnyKeyController : MonoBehaviour
    {
        [SerializeField] private string triggerParam;
        private void Update()
        {
            if (Input.anyKeyDown)
            {
                GameFlow.Instance.Control.SetTrigger(triggerParam);
            }
        }
    }
}

