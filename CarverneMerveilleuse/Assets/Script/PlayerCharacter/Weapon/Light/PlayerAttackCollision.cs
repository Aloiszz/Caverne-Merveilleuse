using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerAttackCollision : MonoBehaviour
{
    public SpriteRenderer sprite;
    public Collider2D coll;
    public Transform pivot;
    public float rotationZ;
    [HideInInspector] public Vector3 difference;
    
    [SerializeField]private GameObject bloodPS;
    [SerializeField]private GameObject bloodPSFloor;
    private int rand;
    
    public static PlayerAttackCollision instance;
    
    private void Awake()
    {
        if (instance != null && instance != this) 
        { 
            Destroy(this); 
        } 
        else 
        { 
            instance = this; 
        } 
    }
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        coll = GetComponent<Collider2D>();
        
        sprite.enabled = false;
        coll.enabled = false;
        
        pivot = GameObject.FindGameObjectWithTag("PivotAttackPoint").GetComponent<Transform>();;
    }

    private void FixedUpdate()
    {
        difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        difference.Normalize();
        rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        pivot.transform.rotation = Quaternion.Euler(0f, 0f, rotationZ);
 
        /*if (rotationZ < -90 || rotationZ > 90)
        {
            if(PlayerController.instance.gameObject.transform.eulerAngles.y == 0)
            {
                pivot.transform.localRotation = Quaternion.Euler(180, 0, -rotationZ);
            } 
            else if (PlayerController.instance.gameObject.transform.eulerAngles.y == 180) 
            {
                pivot.transform.localRotation = Quaternion.Euler(180, 180, -rotationZ);
            }
        }*/
    }


    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("CAC") | col.CompareTag("Boss") | col.CompareTag("Dist") | col.CompareTag("Gros"))
        {
            PlayerLightAttack.instance.playerLightAttack.isStriking = true;
            col.GetComponent<Mechant>().ReceiveLightDamage();
            CinemachineShake.instance.ShakeCamera(1.3f,2,0.2f);
            rand = Random.Range(1, 3);
            for (int i = 0; i < rand; i++)
            {
                Instantiate(bloodPS, col.transform.position, Quaternion.identity, 
                    RoomManager.instance.roomMemory[RoomManager.instance.roomMemoryIndex].transform);
                
            }
            Instantiate(bloodPSFloor, col.transform.position, Quaternion.identity, 
                RoomManager.instance.roomMemory[RoomManager.instance.roomMemoryIndex].transform);
        }
    }
}
