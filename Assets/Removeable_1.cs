using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Removeable_1 : MonoBehaviour
{
    void Update(){
        if(Input.GetMouseButtonDown(0)){
            Debug.Log("chaton");
        }
    }

    void OnSceneGUI()
    {
        if (Event.current.type == EventType.MouseDown && Event.current.button == 0)
        {
          Debug.Log("Left-Mouse Down");
        }
        if (Event.current.type == EventType.MouseUp && Event.current.button == 0)
        {
          Debug.Log("Left-Mouse Up");
        }
        if (Event.current.type == EventType.MouseUp && Event.current.button == 1)
        {
          Debug.Log("Right-Click Up");
        }
    }
}
