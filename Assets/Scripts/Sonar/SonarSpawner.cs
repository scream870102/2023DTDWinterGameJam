using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace BIGJ2023.GameSystem
{
    public class SonarSpawner : MonoBehaviour
    {
        [SerializeField] private Sonar _sonarPreafb;
        private void Start()
        {
            Sonar sonar = Instantiate(_sonarPreafb, transform);
            sonar.Init(gameObject);
        }
    }
}

