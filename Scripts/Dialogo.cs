using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[System.Serializable]
public class Dialogo 
{
    public string name;
    public Sprite icon;
    [TextArea(3,10)]
    public string[] sentences;
 

}