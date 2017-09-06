using UnityEngine;
using System.Collections;

public class InputCtrller : MonoBehaviour {
	
	// Update is called once per frame
	private void Update ()
    {
        Vector2 screenPos;
#if UNITY_EDITOR
        if(Input.GetMouseButtonUp(0))
        {
            screenPos = Input.mousePosition;
            onClickScreen(screenPos);
        }
#else
        Debug.LogError("complete this");
#endif
    }


    private void onClickScreen(Vector2 screenPos)
    {
        Debug.Log(screenPos);
    }
}
