using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class ThrowProtect : MonoBehaviour
{

    public BoxCollider2D coll;
    public static ThrowProtect instance;
    
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(instance);
        }
        instance = this;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerThrowAttack.instance.isThrow)
        {
            coll.size.x.DOFloat(1.6f, 0);
            coll.size.y.DOFloat(1.2f, 0);
        }
        else
        {
            coll.size.x.DOFloat(0.6f, 0);
            coll.size.y.DOFloat(.4f, 0);
        }
    }
}
