using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Save;
public class SceneLoadOnStart : MonoBehaviour
{
    [SerializeField] bool loadSaveOnLoad = true;
    [SerializeField] SaveInteraction saver;
    void Awake(){
        if(loadSaveOnLoad){
            saver.LoadScene(); 
        }      
    }
}
