using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using System;
using System.IO;


namespace Save{
[ExecuteAlways]
    public class SaveInteractio : MonoBehaviour
    {
        [SerializeField] bool saveInPersistentData = true;
        [SerializeField] string fullPath = "";
        [SerializeField] string loadFile= "save003.txt";
        [SerializeField] string saveFile= "save003.txt";


        void Awake(){
            fullPath = GetPath("");
        }

        private void Update(){
            if(Input.GetKeyDown(KeyCode.S)){
                this.GetComponent<SaveSystem>().Save(GetPath(saveFile));
            }

            if(Input.GetKeyDown(KeyCode.L)){
                if(File.Exists(GetPath(loadFile))){
                    LoadScene();
                }else{
                    Debug.Log("The loading file doesn't exist");
                }
            }
        }

        public void LoadScene(){
            EntityManager.DeleteAllEntity();
            this.GetComponent<SaveSystem>().Load(GetPath(loadFile));
        }

        public string GetPath(string app){
            return saveInPersistentData ? 
                Path.GetFullPath(Path.Combine(Application.persistentDataPath,app)):
                Path.GetFullPath(Path.Combine(Application.dataPath,"Resources","saves",app));
        }
    }
}