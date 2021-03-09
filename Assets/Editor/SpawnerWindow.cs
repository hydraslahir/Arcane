
using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class EntitySpawnerWindow : EditorWindow
{
    List<string> references;
    Vector3 localPosition;
    Vector3 localScale;

    void Awake(){
        references = new List<string>(){ "Cube", "Capsule", "Sphere","Cube","Cube"};
        localScale = new Vector3(1,1,1);
        localPosition = new Vector3(0,0,0);
    }

    [MenuItem("GameObject/Spawner",false,-10)]
    public static void ShowWindow(){
        EditorWindow ew = EditorWindow.GetWindow<EntitySpawnerWindow>(true, "Spawner");
        ew.minSize = new Vector2(300f,500f);
    }
    void OnGUI(){

        GUILayout.Label("Proprieties :");
        GUILayout.Space(10f);
        
        GUILayout.BeginHorizontal();
            GUILayout.Space(20f);    
            GUILayout.Label("Position :");
            localPosition = EditorGUILayout.Vector3Field("",localPosition);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
            GUILayout.Space(20f);   
            GUILayout.Label("Scale     :");
            localScale = EditorGUILayout.Vector3Field("",localScale);
        GUILayout.EndHorizontal();
        
        int vX =  (int)position.width/120 > 0 ?  (int)position.width/120 : 1;
        GeneratePromps(references, (int)position.width/120 );
        
    }

    void GeneratePromps(List<string> refe, int x){
        GUILayout.Label("Objects :");

        int i=0;
        for(;i < refe.Count;i+=x){
           GUILayout.BeginHorizontal();
            GUILayout.Space(20f);
           for(int j=0; j < x && (j+i) < refe.Count; j++){
                AddButton(refe[i+j]);
           }
           GUILayout.EndHorizontal();
        }   
    }

    void AddButton(string reference){
        GameObject go1 = Resources.Load("Objects/"+reference, typeof(GameObject)) as GameObject;
        Texture2D texture = Resources.Load("Textures/"+ reference, typeof(Texture2D)) as Texture2D;
        if(texture ==null){
            texture = AssetPreview.GetMiniThumbnail(go1);
        }
        if(GUILayout.Button(
            texture,
                GUILayout.Width(100),
                GUILayout.Height(100)))
            {               
                Material mat1 = Resources.Load("Color_mat", typeof(Material)) as Material;
                GameObject go = EntityManager.GenerateEntity(go1.name, go1, mat1);
                go.transform.position += this.localPosition;
                go.transform.localScale = this.localScale;
            }
    }
}
