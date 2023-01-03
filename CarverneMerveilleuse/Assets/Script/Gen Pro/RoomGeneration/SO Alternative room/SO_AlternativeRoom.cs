using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "AlternativeRoom", menuName ="ScriptableObjects/new AlternativeRoom", order = 1)]

public class SO_AlternativeRoom : ScriptableObject
{
    [Header("----------Alternative Room----------")]

    public List<GameObject> roomTemplateTop;
    public List<GameObject> roomTemplateDown;
    public List<GameObject> roomTemplateRight;
    public List<GameObject> roomTemplateLeft;
}
