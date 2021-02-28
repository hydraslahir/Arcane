using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor;

[ExecuteInEditMode]
public class Initializer : MonoBehaviour
{
    void Awake(){
        AddSetTag("GO");
    }
    void AddSetTag(string tag){
        SerializedObject tagManager = 
            new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
        SerializedProperty tags = tagManager.FindProperty("tags");
            
        //Verifie l'existance du tag GO
        for (int i = 0; i < tags.arraySize; i++)
        {
            SerializedProperty t = tags.GetArrayElementAtIndex(i);
            if (t.stringValue.Equals(tag)) return;
        }
        tags.InsertArrayElementAtIndex(0);
        tags.GetArrayElementAtIndex(0).stringValue = tag;

        tagManager.ApplyModifiedProperties();
    }
}
