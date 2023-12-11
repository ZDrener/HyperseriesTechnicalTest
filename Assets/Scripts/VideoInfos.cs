using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class VideoInfos : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private TextMeshProUGUI _textEpisodeName;
    [SerializeField] private TextMeshProUGUI _textViewCount;

    private void Start()
    {
        Form.ON_FormSave.AddListener(ChangeInfos);
    }

    public void OpenClose()
    {
        _animator.SetTrigger("Trigger");
    }

    private void ChangeInfos(string pEpisodeName, int pViewCount)
    {
        _textEpisodeName.text = pEpisodeName;
        _textViewCount.text = $"{pViewCount}";
    }
    private void OnDestroy()
    {
        Form.ON_FormSave.RemoveListener(ChangeInfos);
    }
}
