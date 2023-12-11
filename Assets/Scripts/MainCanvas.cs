using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCanvas : MonoBehaviour
{
	public static MainCanvas Instance;

	private void Awake()
	{
		if (Instance != null) throw new Exception($"An instance of {name} already exists.");

		Instance = this;
	}

    private void OnDestroy()
    {
		if (Instance == this) Instance = null;
    }
}
