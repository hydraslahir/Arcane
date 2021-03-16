using UnityEngine;
using UnityEditor;

using Save;
using System.IO;

public class SaveWindow : EditorWindow
{
    //save
    private string saveFile = "save001.txt";
    //save what
    private bool saveSelected = false; //TODO
    private bool saveAll = false;
    private bool saveFileInPersistent = false;

    //load
    private string loadFile= "save002.txt";
    private bool loadFileInPersistent = false;

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
        generateFileInput(ref saveFile,ref saveFileInPersistent,false);
/*
        GUILayout.BeginHorizontal();
            GUILayout.Space(30f);
            GUILayout.BeginVertical();
                saveSelected = GUILayout.Toggle(saveSelected || saveAll, "Save selected");
                saveAll = GUILayout.Toggle(saveAll, "Save all GO");
            GUILayout.EndVertical();
        GUILayout.EndHorizontal();
  */      
        GUILayout.Space(10f); 
        
        GUILayout.BeginHorizontal();
            GUILayout.Space(30f); 
            if(GUILayout.Button("Save")){
                if(!Application.isPlaying){
                    if( EditorUtility.DisplayDialog("Create/override file",
                        "You will create/override the file \" " + saveFile + " \".\n"+
                        "Are you sure you want to continue ?"
                        , "Save me", "Not sure"))
                    {
                        SaveInteraction.SaveScene(SaveInteraction.GetPath(saveFile,saveFileInPersistent));
                    }
                
            }
            }
            GUILayout.Space(30f); 
        GUILayout.EndHorizontal();
    }
}
void generateLoad(){
    GUILayout.BeginHorizontal();
        GUILayout.Space(10f); 
        loadFoldout = EditorGUILayout.Foldout(loadFoldout,"Load");
    GUILayout.EndHorizontal();

    if(loadFoldout){
        generateFileInput(ref loadFile,ref loadFileInPersistent,true);
        GUILayout.Space(10f); 
        
        GUILayout.BeginHorizontal();
            GUILayout.Space(30f); 
            Debug.Log(loadFile);
            if(InvalidFile(loadFile)){ GUI.enabled = false;}
            if(GUILayout.Button("Load")){
                if(!Application.isPlaying){
                    if( EditorUtility.DisplayDialog("Validation for loading in Edit Mode",
                            "You are making changes in EDIT MODE\n" 
                    +       "Loading a file will destroy every unsaved object and replace them with the gamefile\n"
                    +       "Are you sure you want to load " + loadFile+ " ?", "Yes, I'm an expert", "Not sure"))
                    {
                        SaveInteraction.LoadScene(SaveInteraction.GetPath(loadFile,loadFileInPersistent));
                    }
                }else{
                    SaveInteraction.LoadScene(SaveInteraction.GetPath(loadFile,loadFileInPersistent));
                }
            }
            GUI.enabled = true;
            GUILayout.Space(30f); 
        GUILayout.EndHorizontal();
    }    
}
/** Genere un input de fichier
*   File : [...givenName...]
*   Location : [.] Persistent [x] Local
* Le premier argument détermine quelle @Variable sera utilisée pour ranger et afficher le texte
* Le deuxieme détermine celui pour la persistance
*/
void generateFileInput(ref string fileVariable, ref bool isPersistent, bool validate){
    //File choice
    string[] types= {"Persistent","Local"};
            GUILayout.BeginHorizontal();
            GUILayout.Space(30f);
            GUILayout.BeginVertical();
                // File name
                GUILayout.BeginHorizontal();
                    EditorGUIUtility.labelWidth = 40;
                    GUILayout.Label("File:");
                    fileVariable = EditorGUILayout.TextField(fileVariable);
                    GUILayout.Space(30f);
                GUILayout.EndHorizontal();
                //Error lookout
                if(validate && ( InvalidFile(fileVariable) || fileVariable == "")){    
                    errorStyle.normal.textColor = Color.red;
                    EditorGUILayout.LabelField("   The file is invalid",errorStyle);
                }
                // Persitent or not
                GUILayout.BeginHorizontal();
                    GUILayout.Label("Location:");
                    int x = GUILayout.SelectionGrid(isPersistent ? 0 : 1,types,2,"toggle");
                    isPersistent =  x == 0 ? true : false;
                GUILayout.EndHorizontal();

            GUILayout.EndVertical();
        GUILayout.EndHorizontal();
}
    bool InvalidFile(string f){
        if(File.Exists(SaveInteraction.GetPath(f,loadFileInPersistent))){
            return false;
        }
        return true;
    }
}
