using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIUtil {

    public static void ShowGUIAnim(GameObject obj)
    {
        if (obj == null) return;
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
        if (obj == null) return;
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
