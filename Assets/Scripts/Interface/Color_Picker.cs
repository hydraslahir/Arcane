using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using TMPro;
public class Color_Picker : MonoBehaviour
{
    int r=0,g=0,b=0;
    [SerializeField] Slider rs;
    [SerializeField] Slider gs;
    [SerializeField] Slider bs;

    [SerializeField] TMP_InputField ri;
    [SerializeField] TMP_InputField gi;
    [SerializeField] TMP_InputField bi;

    void Awake(){
        UpdateDisplay();
    }

    public void ChangeRedSlider(float value){
        r = (int)value;
        UpdateDisplay();
    }

    public void ChangeRed(string value){
        if(value == ""){
            r = 0;
        }else{
            r = int.Parse(value);
        }
        UpdateDisplay();
    }

    public void ChangeBlueSlider(float value){
        b = (int)value;
        UpdateDisplay();
    }

    public void ChangeBlue(string value){
        if(value == ""){
            b = 0;
        }else{
            b = int.Parse(value);
        }
        UpdateDisplay();
    }

    public void ChangeGreenSlider(float value){
        g = (int)value;
        UpdateDisplay();
    }

    public void ChangeGreen(string value){
        if(value == ""){
            g = 0;
        }else{
            g = int.Parse(value);
        }
        UpdateDisplay();
    }


    void UpdateDisplay(){
        rs.value = r;
        gs.value = g;
        bs.value = b;

        ri.text = r.ToString();
        gi.text = g.ToString();
        bi.text = b.ToString();
    }

    public Color GetColor(){
        return new Color(r/255f,g/255f,b/255f);
    }
}
