using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XLua;

[Hotfix]
public class ResCheckPage : MonoBehaviour {

    public Slider slider;
    public Text sliderInfo;

    public float openWaitTime = 1f;
    public float hideWaitTime = 1f;

    public IEnumerator Open(Action action=null)
    {
        var guiAnis = GetComponentsInChildren<GUIAnim>();
        if (guiAnis.Length > 0)
        {
            foreach (var ani in guiAnis)
            {
                ani.MoveIn();
            }
        }

        yield return new WaitForSeconds(openWaitTime);

        if (action != null)
        {
            action.Invoke();
        }
    }

    public IEnumerator Hide(Action action=null)
    {
        transform.GetComponentsInChildren<GUIAnim>();
        var guiAnis = GetComponentsInChildren<GUIAnim>();
        if (guiAnis.Length > 0)
        {
            foreach (var ani in guiAnis)
            {
                ani.MoveOut();
            }
        }

        yield return new WaitForSeconds(hideWaitTime);

        if (action != null)
        {
            action.Invoke();
        }
    }

    public void SetSliderValue(float value)
    {
        slider.value = value;
    }

    public void SetSliderInfo(string str)
    {
        sliderInfo.text = str;
    }
}
