using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class PropsGen : MonoBehaviour
{

    public List<GameObject> propsList;
    public GameObject[] props;
    
    private int rand;

    void Start()
    {

        foreach (var i in GameObject.FindGameObjectsWithTag("RoomProps"))
        {
            propsList.Add(i);
            i.SetActive(false);
            
            /*props = i.GetComponentsInChildren<GameObject>();  // récupération de tout les sprites enfant
 
            for(int k = 0; k < props.Length; k++)
            {
                props[k].SetActive(false);
            }*/
        }

        rand = Random.Range(0, propsList.Count);
        propsList[rand].SetActive(true);
        
        
        /*for(int u = 0; u < props.Length; u++)
        {
            props[u].SetActive(true);
        }*/
    }
    
}