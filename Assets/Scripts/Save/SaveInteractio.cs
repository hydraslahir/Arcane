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
        [SerializeField] string saveFile= "save001.txt";


        void Awake(){
            fullPath = GetPath();
        }

        private void Update(){
            if(Input.GetKeyDown(KeyCode.S)){
                this.GetComponent<SaveSystem>().Save(fullPath);
            }

            if(Input.GetKeyDown(KeyCode.L)){
                LoadScene();
            }
        }

        public void LoadScene(){
            EntityManager.DeleteAllEntity();
            this.GetComponent<SaveSystem>().Load(fullPath);
        }

        public string GetPath(){
            return saveInPersistentData ? 
                Path.GetFullPath(Path.Combine(Application.persistentDataPath,saveFile)):
                Path.GetFullPath(Path.Combine(Application.dataPath,"Resources","saves",saveFile));
        }
    }
}