using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerAttackCollision3 : MonoBehaviour
{
    public SpriteRenderer sprite;
    public Collider2D coll;
    public Transform pivot;
    public float rotationZ;
    [HideInInspector] public Vector3 difference;

    [HideInInspector] public bool Combo_Validate;

    [SerializeField]private GameObject bloodPS;
    [SerializeField]private GameObject bloodPSFloor;
    private int rand;
    
    private int randAudioHit;
    private int randAudioNoHit;
    
    [Space] [SerializeField] private float scoreRagePoint = 2;
    
    public static PlayerAttackCollision3 instance;
    
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
    }


    private void OnTriggerEnter2D(Collider2D col)
    {
        randAudioHit = Random.Range(0, PlayerLightAttack.instance.audioSlashHit.Count);
        randAudioNoHit = Random.Range(0, PlayerLightAttack.instance.audioSlashNoHit.Count);
        
        if (col.CompareTag("CAC") | col.CompareTag("Boss") | col.CompareTag("Dist") | col.CompareTag("Gros"))
        {
            PlayerController.instance.Source.PlayOneShot(PlayerLightAttack.instance.audioSlashHit[randAudioHit],0.5f);
            PlayerLightAttack.instance.playerLightAttack.isStriking = true;
            
            Score.instance.scoreRage += scoreRagePoint;
            
            col.GetComponent<Mechant>().ReceiveLightDamage();

            CinemachineCameraZoom.instance.CameraZoom(8.5f, 0.05f, 0.4f);
            CinemachineShake.instance.ShakeCamera(3,3,0.5f);
            
            rand = Random.Range(1, 3);
            for (int i = 0; i < rand; i++)
            {
                Instantiate(bloodPS, col.transform.position, Quaternion.identity,
                    RoomManager.instance.roomMemory[RoomManager.instance.roomMemoryIndex].transform);
                
                Instantiate(bloodPSFloor, col.transform.position, Quaternion.identity, 
                    RoomManager.instance.roomMemory[RoomManager.instance.roomMemoryIndex].transform);
            }
            
        }
        else
        {
            PlayerController.instance.Source.PlayOneShot(PlayerLightAttack.instance.audioSlashNoHit[randAudioNoHit],0.5f);
        }
    }
}
