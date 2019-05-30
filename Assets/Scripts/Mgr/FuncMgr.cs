using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class FuncMgr {
	public static Color getColorFromHex(String s)
    {
        float r = Convert.ToInt32(s.Substring(0, 2), 16);
        float g = Convert.ToInt32(s.Substring(2, 2), 16);
        float b = Convert.ToInt32(s.Substring(4, 2), 16);
        return new Color(r/255,g/255,b/255);
    }
}
