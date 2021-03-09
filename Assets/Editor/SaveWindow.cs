using UnityEngine;
using UnityEditor;

using Save;

public class SaveWindow : EditorWindow
{
    //save
    private bool saveSelected = false;
    private bool saveAll = false;

    //load
    private string file = "kotenok.sav";
    private bool fileInPersistent = false;

    //foldout
    private bool saveFoldout = true;
    private bool loadFoldout = true;

    //styles
    GUIStyle errorStyle = new GUIStyle();

    [MenuItem("Save/save",false,-10)]
    public static void ShowWindow(){
        EditorWindow ew = EditorWindow.GetWindow<SaveWindow>(true, "Save");
        ew.minSize = new Vector2(300f,0f);
    }
void Start(){
    errorStyle.normal.textColor = Color.red;
}

void OnGUI(){
    
    GUILayout.Label("** Allow the save of objects states **");
    generateSave();

    generateLoad();
}

void generateSave(){
    GUILayout.Space(10f); 
    GUILayout.BeginHorizontal();
        GUILayout.Space(10f); 
        saveFoldout = EditorGUILayout.Foldout(saveFoldout,"Save");
    GUILayout.EndHorizontal();
    // Save Foldout
    if(saveFoldout){
        GUILayout.BeginHorizontal();
            GUILayout.Space(30f);
            GUILayout.BeginVertical();
                saveSelected = GUILayout.Toggle(saveSelected || saveAll, "Save selected");
               saveAll = GUILayout.Toggle(saveAll, "Save all GO");
            GUILayout.EndVertical();
        GUILayout.EndHorizontal();
        
        GUILayout.Space(10f); 
        
        GUILayout.BeginHorizontal();
            GUILayout.Space(30f); 
            if(GUILayout.Button("Save")){
            }
            GUILayout.Space(30f); 
        GUILayout.EndHorizontal();
    }
}
void generateLoad(){
    //File choice
    string[] types= {"Persistent","Local"};
    
    GUILayout.BeginHorizontal();
        GUILayout.Space(10f); 
        loadFoldout = EditorGUILayout.Foldout(loadFoldout,"Load");
    GUILayout.EndHorizontal();

    if(loadFoldout){
        GUILayout.BeginHorizontal();
            GUILayout.Space(30f);
            GUILayout.BeginVertical();
                // File name
                GUILayout.BeginHorizontal();
                    EditorGUIUtility.labelWidth = 40;
                    GUILayout.Label("File:");
                    file = EditorGUILayout.TextField(file);
                    GUILayout.Space(30f);
                GUILayout.EndHorizontal();
                //Error lookout
                if(InvalidFile(file) && file != ""){    
                    errorStyle.normal.textColor = Color.red;
                    EditorGUILayout.LabelField("   The file is invalid",errorStyle);
                }
                // Persitent or not
                GUILayout.BeginHorizontal();
                    GUILayout.Label("Location:");
                    int x = GUILayout.SelectionGrid(fileInPersistent ? 0 : 1,types,2,"toggle");
                    fileInPersistent =  x == 0 ? true : false;
                GUILayout.EndHorizontal();

            GUILayout.EndVertical();
        GUILayout.EndHorizontal();


        
        GUILayout.Space(10f); 
        
        GUILayout.BeginHorizontal();
            GUILayout.Space(30f); 
            if(InvalidFile(file)){ GUI.enabled = false;}
            if(GUILayout.Button("Load")){
                if(!Application.isPlaying){
                    if( EditorUtility.DisplayDialog("Validation for loading in Edit Mode",
                            "You are making changes in EDIT MODE\n" 
                    +       "Loading a file will destroy every unsaved object and replace them with the gamefile\n"
                    +       "Are you sure you want to load " + file + " ?", "Yes, I'm an expert", "Not sure"))
                    {
                        SaveInteractio.LoadScene(SaveInteractio.GetPath(file,fileInPersistent));
                    }
                }
            }
            GUI.enabled = true;
            GUILayout.Space(30f); 
        GUILayout.EndHorizontal();
    }    
}
    bool InvalidFile(string f){
        if(SaveInteractio.GetPath(f,fileInPersistent) != ""){
            return false;
        }
        return true;
    }
}
