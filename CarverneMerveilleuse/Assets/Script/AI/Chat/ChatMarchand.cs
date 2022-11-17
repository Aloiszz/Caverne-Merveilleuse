using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
public class ChatMarchand : MonoBehaviour
{
    public PlayerController player;
    public float speed;
    public MerchantSecretDoor merchantRoom;
    private int rand;
    [SerializeField] private GameObject spawnpoint;

    public bool see;
    public bool isStillActive;
    [HideInInspector] public bool PlayerCameBack;
    public static ChatMarchand instance;

    // Start is called before the first frame update
    
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
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        merchantRoom = GameObject.FindGameObjectWithTag("MerchantSecretDoor").GetComponent<MerchantSecretDoor>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!isStillActive)
        {
            FindMerchantRoom();  
        }

        if (PlayerCameBack)
        {
            StartCoroutine(PlayerCameback());
            
        }
    }
    

    private void FindMerchantRoom()
    {
        if (see)
        {
            //gameObject.transform.position = Vector3.MoveTowards(transform.position, merchantRoom.transform.position, speed * Time.deltaTime);
        }
        else
        {
            gameObject.transform.position = Vector3.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
        }
    }


    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            RoomManager.instance.isShopRoom = true;
            
            GetComponent<Collider2D>().enabled = false;
            GetComponent<SpriteRenderer>().enabled = false;
        
            isStillActive = true;
            
            Shop();
            
        }
    }

    #region Shop Generation


    void Shop()
    {
        StartCoroutine(Mini_cinematic());
        
    }
    
    public void GenerateShopRoom()
    {
        RoomManager.instance.roomMemoryIndex++;
        RoomManager.instance.roomMemoryDirectionIndex++;
        transform.parent.gameObject.SetActive(false);
        
        rand = Random.Range(0, RoomManager.instance.bossRoom.Count);
        Instantiate(RoomManager.instance.shopRoom[rand], new Vector3(0,0,0),
            transform.rotation).transform.GetChild(0).GetComponentInChildren<RoomSpawnerV2>().colliderVierge = true;
    }
    
    public void TeleportPlayerToNextRoom()
    {
        PlayerController.instance.transform.position = GameObject.FindGameObjectWithTag("SpawnPoint").transform.position;
    }

    IEnumerator Mini_cinematic()
    {
        PlayerController.instance.enabled = false;
        //Play Anim 
        //Camera effetc
        
        yield return new WaitForSeconds(2);
        
        GenerateShopRoom();
        TeleportPlayerToNextRoom();
        AstarPath.active.Scan();
        
    }
    
    IEnumerator PlayerCameback()
    {
        PlayerController.instance.enabled = false;
        //Play Anim 
        //Camera effetc
        
        yield return new WaitForSeconds(2);   
        
        PlayerController.instance.enabled = true;
    }

    #endregion
    
}
