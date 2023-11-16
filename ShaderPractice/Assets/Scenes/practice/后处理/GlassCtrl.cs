using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlassCtrl : MonoBehaviour
{
    public static bool takeShot = false;
    public bool showGlass = false;
    public GameObject glassObj;
    private bool realShowGlass = false;
    private int realShowIndex = 0;

    void Awake()
    {
        showGlass = false;
        glassObj.SetActive(false); 
    }

    //前一帧在截图，下一帧显示
    void Update()
    {
       if (realShowIndex >= 1)
        {
            glassObj.SetActive(true);
            realShowIndex = 0;
            return;
        }
        if (realShowGlass != showGlass)
        {
            realShowGlass = showGlass;
            if (showGlass)
            {
                GlassCtrl.takeShot = true;
                realShowIndex++;
            }
            else
            {
                glassObj.SetActive(false);
            }

        }
    }
}
