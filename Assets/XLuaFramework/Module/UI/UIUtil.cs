using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIUtil {

    public static void ShowGUIAnim(GameObject obj)
    {
        var guiAnis = obj.GetComponentsInChildren<GUIAnim>();
        if (guiAnis.Length > 0)
        {
            foreach (var ani in guiAnis)
            {
                ani.MoveIn();
            }
        }
    }

    public static void HideGUIAnim(GameObject obj)
    {
        var guiAnis = obj.GetComponentsInChildren<GUIAnim>();
        if (guiAnis.Length > 0)
        {
            foreach (var ani in guiAnis)
            {
                ani.MoveOut();
            }
        }
    }
}
