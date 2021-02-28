using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveableEntity : MonoBehaviour
{
    public string GetUniqueID(){
        return System.Guid.NewGuid().ToString();
    }
}
