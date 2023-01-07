using System.Collections;
using System.Collections.Generic;
using Scream.UniMO.Common;
using UnityEngine;
using UnityEngine.Events;

namespace BIGJ2023.Common
{
	public class GameFlow : MonoBehaviour
	{
		#region INSTANCE
		public static GameFlow Instance { get; private set; } = null;

		private bool CheckInstance()
		{
			if (Instance == null)
			{
				Instance = this;
				DontDestroyOnLoad(gameObject);
				return true;
			}
			else
			{
				DestroyImmediate(gameObject);
				return false;
			}
		}
		#endregion
		public Animator Control { get; private set; } = null;
		private void Awake()
		{
			if (!CheckInstance())
			{
				return;
			}
			Control = GetComponent<Animator>();
		}

	}
}
