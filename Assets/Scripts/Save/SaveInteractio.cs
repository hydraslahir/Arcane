using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using System;
using System.IO;


namespace Save{
    public class SaveInteractio : MonoBehaviour
    {
        [SerializeField] bool saveInPersistentData = true;
        [SerializeField] string fullPath = "";
        [SerializeField] string loadFile= "save003.txt";
        [SerializeField] string saveFile= "save003.txt";


        void Awake(){
            fullPath = GetPath("",saveInPersistentData);
        }

        private void Update(){
            if(Input.GetKeyDown(KeyCode.S)){
                SaveScene(GetPath(saveFile,saveInPersistentData));
            }

            if(Input.GetKeyDown(KeyCode.L)){
                if(File.Exists(GetPath(loadFile,saveInPersistentData))){
                    LoadScene();        
                }else{
                    Debug.Log("The loading file doesn't exist");
                }
            }
        }
        public static void SaveScene(string path){
            SaveSystem.Save(path);
        }

        public void LoadScene(){
                LoadScene(GetPath(loadFile,saveInPersistentData));
        }
        
        public static void LoadScene(string wholePath){
            if(!File.Exists(wholePath)){ 
                Debug.Log("The given path(" + wholePath + ") doesn't exist");
            }else{
                EntityManager.DeleteAllEntity();
                SaveSystem.Load(wholePath);
            }
        }

/**
    Methode qui retourne le chemin persistant ou local du nom de fichier donné.
*/
        public static string GetPath(string app, bool persistent){
            string path = persistent ? 
                Path.GetFullPath(Path.Combine(Application.persistentDataPath,app)):
                Path.GetFullPath(Path.Combine(Application.dataPath,"Resources","saves",app));
            return path;

        }
    }
}