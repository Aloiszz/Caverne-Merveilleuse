using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeavyAttackCollision : MonoBehaviour
{
    public SpriteRenderer sprite;
    public Collider2D coll;
    public GameObject bloodPS;
    [Header("Singleton")]
    public static HeavyAttackCollision instance;
    
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
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("CAC") | col.CompareTag("Boss") | col.CompareTag("Dist") | col.CompareTag("Gros"))
        {
            PlayerLightAttack.instance.playerLightAttack.isStriking = true;
            col.GetComponent<Mechant>().ReceiveAOEDamage();
            CinemachineShake.instance.ShakeCamera(2,2,0.1f);
             int rand = Random.Range(1, 3);
            for (int i = 0; i < rand; i++)
            {
                Instantiate(bloodPS, col.transform.position, Quaternion.identity, RoomManager.instance.roomMemory[RoomManager.instance.roomMemoryIndex].transform);
            }
        }
    }
}
