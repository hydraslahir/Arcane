using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

public class Spawner : MonoBehaviour
{

    [System.Serializable]
    public class Possibility{
       [SerializeField] public string name;
       [SerializeField] public GameObject go;
    }

    [SerializeField] int possibilityIndex=0;
    [SerializeField] Possibility[] possibilities;
    [SerializeField] Material defaultMaterial;
    [SerializeField] Vector3 where = new Vector3(0,2,0);
//ui
    [SerializeField] TMP_Dropdown dropdown;
    

    void Awake(){
        //Fill dropdown options
        dropdown.ClearOptions ();
        List<string> options = new List<string>();
        foreach (var option in possibilities) {
            options.Add(option.name);
        }
        dropdown.ClearOptions ();
        dropdown.AddOptions(options);
    }

    void Update(){
        if(Input.GetKeyDown(KeyCode.Q)){
            SpawnEntity();
        }
    }

    public void ChangeIndex(int index){
        possibilityIndex = index;
    }

    public void SpawnEntity(){
        Possibility p = possibilities[possibilityIndex];
        GameObject go = EntityManager.GenerateEntity(p.name, p.go, defaultMaterial);
        go.transform.position = where;
    }
}
