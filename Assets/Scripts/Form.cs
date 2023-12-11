using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;
using System;

public class Form : MonoBehaviour
{
	public static UnityEvent<string, int> ON_FormSave = new UnityEvent<string, int>();

	[SerializeField] private TMP_InputField _episodeNameField;
	[SerializeField] private TMP_InputField _viewCountField;

	public void SendForm()
	{
		int lViewCount = 0;

		if (!int.TryParse(_viewCountField.text, out lViewCount)) 
			throw new Exception("Form's view count cannot be cast to int. Check the value and try again.");

		ON_FormSave.Invoke(_episodeNameField.text.ToUpper(), lViewCount);
	}
}
