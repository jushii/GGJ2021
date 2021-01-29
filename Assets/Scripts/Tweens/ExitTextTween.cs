using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class ExitTextTween : MonoBehaviour
{
    [SerializeField] private Color tweenFromColor;
    [SerializeField] private Color tweenToColor;
    [SerializeField] private TextMeshProUGUI label;
    
    void Start()
    {
        label.DOColor(tweenToColor, 2.0f).SetLoops(-1, LoopType.Yoyo);
    }
}
