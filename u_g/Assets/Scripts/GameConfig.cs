using UnityEngine;
using System.Collections;

public class GameConfig
{
    public static string getBundlePath()
    {
#if UNITY_EDITOR
        return Application.streamingAssetsPath + "/../../../u_b/Assets/StreamingAssets";
#endif
    }


}
