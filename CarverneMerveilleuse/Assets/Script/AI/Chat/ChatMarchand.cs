using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;
public class ChatMarchand : MonoBehaviour
{
    [Header("Fonctionnement du potit chat")]
    public PlayerController player;
    public float speed;
    public MerchantSecretDoor merchantRoom;
    private int rand;
    [SerializeField] private GameObject spawnpoint;

    public bool see;
    public bool isStillActive;
    [HideInInspector] public bool PlayerCameBack;

    public int numberofroom;
    private List<GameObject> LesChats;
    public Collider2D coll;
    private Rigidbody2D rb;

    public static ChatMarchand instance;

    [Header("Animator")] 
    public Animator Animator;
    public GameObject graph;
    public Animator camera;
    
    [Header("Audio")] 
    public AudioSource Source;
    public AudioClip audioEnterTheCat;
    public AudioClip audioExitTheCat;
    public AudioClip audioInTheMerchantRoom;
    
    public CinemachineVirtualCamera _virtualCamera;
    
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
        enabled = false;
        coll = GetComponent<Collider2D>();
        coll.enabled = false;
        rb = GetComponent<Rigidbody2D>();
        
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        //merchantRoom = GameObject.FindGameObjectWithTag("MerchantSecretDoor").GetComponent<MerchantSecretDoor>();
        
        numberofroom = RoomManager.instance.roomMemoryIndex;
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
            Debug.Log("PlayerCameBack");
            StartCoroutine(PlayerCameback());
            PlayerCameBack = false;
            isStillActive = false;
        }
        
        
    }


    private void FindMerchantRoom()
    {
        if (see)
        {
            //gameObject.transform.position = Vector3.MoveTowards(transform.position, merchantRoom.transform.position, speed * Time.deltaTime);
            Animator.SetBool("Found", true);
            Animator.SetBool("isRunning", false);
        }
        else
        {
            Animator.SetBool("Found", false);
            Animator.SetBool("isRunning", true);
            /*gameObject.transform.position =
                Vector3.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);*/

            rb.AddForce((PlayerController.instance.transform.position - transform.position).normalized * 15);
            if (rb.velocity.x > 0.3f)
            {
                graph.transform.DOScale(new Vector3(-0.1f,0.1f,0.1f), .2f);
            }
            else if (rb.velocity.x < -0.3f)
            {
                graph.transform.DOScale(new Vector3(0.1f,0.1f,0.1f), .2f);
            }
        }
    }


    /*private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.CompareTag("Player"))
        {
            RoomManager.instance.isShopRoom = true;
            
            //GetComponent<Collider2D>().enabled = false;
            //GetComponent<SpriteRenderer>().enabled = false;
            
            isStillActive = true;
            
            //Shop();
            
        }
    }*/


    public void CatDesapear()
    {
        ChatMarchand.instance.transform.parent =
            RoomManager.instance.roomMemory[RoomManager.instance.roomMemoryIndex].transform;
        ChatMarchand.instance.transform.position = new Vector3(74, 0, 0);
        //ChatMarchand.instance.enabled = false;
        ChatMarchand.instance.coll.enabled = false;
            
        RoomManager.instance.canThePotitChatSpawn = false;
    }

    #region Shop Generation


    public void Shop()
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
        camera.enabled = true;
        camera.SetTrigger("DutchEnter");
        Source.PlayOneShot(audioEnterTheCat);
        SceneManager.instance.playModeCG_.DOFade(0, 0.75f);
        Introduction.instance._MoneyPanel.DOFade(0, 0.75f);
        
        yield return new WaitForSeconds(2);
        Source.PlayOneShot(audioInTheMerchantRoom);
        SceneManager.instance.playModeCG_.DOFade(1, 1.25f);
        Introduction.instance._MoneyPanel.DOFade(1, 1.25f);
        camera.SetTrigger("InShop");
        _virtualCamera.m_Lens.OrthographicSize = 10;
        AudioManager.instance.PlayShop();
        GenerateShopRoom();
        TeleportPlayerToNextRoom();
        AstarPath.active.Scan();
        ChatCircleMerchantWay.instance.gameObject.GetComponent<Collider2D>().enabled = true;
    }
    
    public IEnumerator PlayerCameback()
    {
        PlayerController.instance.enabled = false;
        //Play Anim 
        //Camera effetc
        camera.enabled = true;
        camera.SetTrigger("DutchExit");
        Source.PlayOneShot(audioExitTheCat);
        
        SceneManager.instance.playModeCG_.DOFade(0, 0.25f);
        Introduction.instance._MoneyPanel.DOFade(0, 0.25f);
        
        yield return new WaitForSeconds(2);
        camera.enabled = false;

        SceneManager.instance.playModeCG_.DOFade(1, .75f);
        Introduction.instance._MoneyPanel.DOFade(1, .75f);
        PlayerController.instance.enabled = true;
        
        AudioManager.instance.PlayCave();
    }

    #endregion
    
}
