using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/BrickColorScriptable", order = 1)]
public class BrickColorScriptable : ScriptableObject
{
    public Material[] listMaterial;
}