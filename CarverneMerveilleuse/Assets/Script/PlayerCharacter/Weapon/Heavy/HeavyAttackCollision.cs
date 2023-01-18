using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class HeavyAttackCollision : MonoBehaviour
{
    public SpriteRenderer sprite;
    public Collider2D coll;
    public GameObject bloodPS;
    [Header("Singleton")]
    public static HeavyAttackCollision instance;

    private int randAudioHit;
    private int randAudioNoHit;

    
    [Space] [SerializeField] private float scoreRagePoint = 1;
    
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
        randAudioHit = Random.Range(0, PlayerHeavyAttack.instance.audioSlashHit.Count);
        randAudioNoHit = Random.Range(0, PlayerHeavyAttack.instance.audioSlashNoHit.Count);
        
        if (col.CompareTag("CAC") | col.CompareTag("Boss") | col.CompareTag("Dist") | col.CompareTag("Gros"))
        {
            PlayerController.instance.Source.PlayOneShot(PlayerHeavyAttack.instance.audioSlashHit[randAudioHit], 0.3f);
            PlayerLightAttack.instance.playerLightAttack.isStriking = true;
            
            Score.instance.scoreRage += scoreRagePoint;
            
            col.GetComponent<Mechant>().ReceiveAOEDamage();
            CinemachineShake.instance.ShakeCamera(2,2,0.1f);
             int rand = Random.Range(1, 3);
            for (int i = 0; i < rand; i++)
            {
                Instantiate(bloodPS, col.transform.position, Quaternion.identity, RoomManager.instance.roomMemory[RoomManager.instance.roomMemoryIndex].transform);
            }
        }
        else
        {
            PlayerController.instance.Source.PlayOneShot(PlayerHeavyAttack.instance.audioSlashNoHit[randAudioNoHit], 0.3f);
        }
    }
}
