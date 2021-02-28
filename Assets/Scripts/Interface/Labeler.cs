using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SelectorNS;
using TMPro;

public class Labeler : MonoBehaviour
{
    [SerializeField] Selector SelectorReference;
    [SerializeField] TextMeshProUGUI  selectedLabel;

    [SerializeField] TMP_InputField[] scaleFields;

    private float amplitude =1;
    private Vector3 scale = new Vector3(1,1,1);

    private GameObject go;

    void Awake(){
        SetDefaultFields();
    }
    void Update()
    {
        GameObject local = SelectorReference.GetSelectedObject();
        if(local != go){
            amplitude =1;
            scale = local.transform.localScale;
        }
        go = local;
        if(go == null){
            //no object selected
            SetDefaultFields();
        }else{
            DisplayValues();
            Resize();
        }
    }

    public void DisplayValues(){
        selectedLabel.SetText(go.name);
        scaleFields[0].text = amplitude.ToString("#.00");
        scaleFields[1].text = scale.x.ToString("#.00");
        scaleFields[2].text = scale.y.ToString("#.00");
        scaleFields[3].text = scale.z.ToString("#.00");
        
    }    

    public void SetDefaultFields(){
        selectedLabel.SetText(" - - - ");
        foreach(var s in scaleFields){
            s.text ="1.0";
        }
    }

    public void ChangeScale(string w){
        try {amplitude = float.Parse(w);}
        catch{amplitude =1.0f;}
    }

    public void ChangeX(string x){
        float p = scale.x;
        try {scale.x = float.Parse(x);}
        catch{scale.x = p;}
    }
    
    public void ChangeY(string y){
        float p = scale.y;
        try {scale.y = float.Parse(y);}
        catch{scale.y = p;}
    }
    public void ChangeZ(string z){
        float p = scale.z;
        try {scale.z = float.Parse(z);}
        catch{scale.z = p;}
    }
    void Resize(){
        if(go == null) return;
        go.transform.localScale = scale*amplitude;
    }
}
