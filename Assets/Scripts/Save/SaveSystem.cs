using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
namespace Save{
    public class SaveSystem : MonoBehaviour
    {
        public static void Save(string path){
            Debug.Log("Saving file");
            using (FileStream fs = File.Open(path, FileMode.Create)){
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(fs,EntityManager.GetAllEntity());
            }
        }

        public static void Load(string path){
            Debug.Log("Loading file");
            using (FileStream fs = File.Open(path, FileMode.Open)){
               BinaryFormatter formatter = new BinaryFormatter();
               EntityManager.RestaureAllEntity((Dictionary<string,object>)formatter.Deserialize(fs));
            }
        }
    }
}