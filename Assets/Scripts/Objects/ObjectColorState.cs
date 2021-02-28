using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
    Ajoute une couleur initiale
*/
public class ObjectColorState : MonoBehaviour
{
    [SerializeField] Color initialColor= Color.white;
    
    void Start()
    {
        var mesh = GetComponent<MeshFilter>().mesh;
        if(mesh.colors.Length == 0){
            Color[] colors=  new Color[mesh.vertices.Length];
            for(int i= 0; i< colors.Length;i++){
                colors[i] = initialColor;
            }
            mesh.colors = colors;
        }
    }
}
