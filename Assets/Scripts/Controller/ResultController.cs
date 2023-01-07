using BIGJ2023.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BIGJ2023.GameSystem
{
    public class ResultController : MonoBehaviour
    {
        private const string _triggerParam = "Reset";

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                GameFlow.Instance.Control.SetTrigger(_triggerParam);
            }
        }
    }
}

