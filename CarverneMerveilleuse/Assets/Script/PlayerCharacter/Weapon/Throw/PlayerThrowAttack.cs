using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerThrowAttack : MonoBehaviour
{

    public SO_Player_ThrowAttack SO_Player_Throw; 
    
    public  int maxBounce ;
    [SerializeField]private float distance;
    [SerializeField]private LineRenderer lineRender;
    [SerializeField]private LayerMask mask;
    [HideInInspector]public bool is_F_Pressed;
    
    private Quaternion deflectRotation;
    private Vector3 deflectDirection;

    public List<Vector3> points = new List<Vector3>();

    public bool isThrow;
    [HideInInspector]public bool isInGrosProjo;

    [HideInInspector]public List<float> ThrowSpeed;
    [HideInInspector] public int ThrowSpeedIndex;

    [HideInInspector]public List<float> ThrowDamage;
    [HideInInspector] public int ThrowDamageIndex;

    [SerializeField] private GameObject FlecheDeVise;
    public GameObject PS_eclatDeFaux;

    
    private float lerp = 0f;
    [SerializeField] private float duration = 2f;
    private float fauxScaleActual = 1;
    [SerializeField] private float fauxScaleBase = 1;
    [SerializeField] private float fauxScaleMax = 1.5f;
    
    
    [Header("Cinemachine Schake")] 
    [SerializeField]private float intensityAmplitude;
    [SerializeField]private float intensityFrequency;
    [SerializeField]private float intensityTime;
    
    
    public static PlayerThrowAttack instance;
    
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
    public void Start()
    {
        FlecheDeVise.SetActive(false);
        lineRender = GetComponentInChildren<LineRenderer>();
        //lineRender.gameObject.SetActive(false);
        SecureSO();
    }

    public void SecureSO()
    {
        maxBounce = SO_Player_Throw.maxBounce;
        distance = SO_Player_Throw.distance;
        
        ThrowSpeed = SO_Player_Throw.ThrowSpeed;
        ThrowSpeedIndex = SO_Player_Throw.ThrowSpeedIndex;

        ThrowDamage = SO_Player_Throw.ThrowDamage;
        ThrowDamageIndex = SO_Player_Throw.ThrowDamageIndex;
    }

    private void Update()
    {
        /*if (Input.GetKey(KeyCode.Mouse1) && !isThrow)
        {
            Aim();
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                Debug.Log("Throw Weapon");
                
                isThrow = true;
            }
        }*/ // Input de base 
        if (Input.GetKey(KeyCode.Mouse1) && !isThrow)
        {
            Aim();
        }
        
        if (Input.GetKeyUp(KeyCode.Mouse1) && !isThrow)
        {
            Debug.Log("Throw Weapon");
            isThrow = true;
        }

        if (isThrow)
        {
            FlecheDeVise.SetActive(false);
            if (PointCollission.instance.bounceInt >= 2)
            {
                if(Input.GetKeyDown(KeyCode.Mouse1) || Input.GetKeyDown(KeyCode.Mouse0)) // verif pour faire revenir la faux
                {
                    Debug.Log("return weapon");
                    is_F_Pressed = true;
                }
            }
        }
        
        IsThrow();
    }

    private void FixedUpdate()
    {
        FlecheDeVise.transform.rotation = Quaternion.Euler(0f, 0f, PlayerAttackCollision.instance.rotationZ);
    }

    void Aim() // quand on vise
    {
        FlecheDeVise.SetActive(true);
        IsWeaponDisable(true);
            
        lineRender.gameObject.SetActive(true);
            
        points.Clear();
        points.Add(transform.position);
        
        DoRay(PlayerController.instance.transform.position, PlayerAttackCollision.instance.difference, maxBounce, distance);
        
        lineRender.positionCount = points.Count;
        
        lineRender.SetPositions(points.ToArray());

        Fleche();
    }
    
    void Fleche() 
    {
        lerp += Time.deltaTime / duration;
        fauxScaleActual = (float)Mathf.Lerp (fauxScaleBase, fauxScaleMax, lerp);
        
        FlecheDeVise.transform.localScale = new Vector3(fauxScaleActual, 1, 1);
    }
    
    void IsThrow()
    {
        if (isThrow)
        {
            IsWeaponDisable(true);
            ThrowCollision.instance.laFaux.SetActive(true); 
            if (!isInGrosProjo)
            {
                PlayerController.instance.speedMovement = 110;
            }
            ThrowCollision.instance.IsWeaponActive(true); // Collission et sprite visible
            PointCollission.instance.IsWeaponActive(true); // Collission et sprite visible
            lineRender.gameObject.SetActive(false);

            if (is_F_Pressed) // Fait revenir la faux
            {
                ReturnWeapon();
            }

            if (!PointCollission.instance.verifPremierTouch)
            {
                PointCollission.instance.ThrowWeapon();
            }
            
        }
        else
        {
            if (!isInGrosProjo && !ItemManager.instance.isInBuffDash)
            {
                Debug.Log("La speed quoi");
                PlayerController.instance.speedMovement = PlayerController.instance.playerSO.speedMovement;
            }
            
            //ThrowCollision.instance.gameObject.transform.position = PlayerController.instance.transform.position;
            PointCollission.instance.gameObject.transform.position = PlayerController.instance.transform.position;
            IsWeaponDisable(false);
            ThrowCollision.instance.IsWeaponActive(false); // Collission et sprite non visible
            PointCollission.instance.IsWeaponActive(false);  // Collission et sprite non visible
        }
    } // quand le faux est lancé

    
    void DoRay(Vector3 origin, Vector3 direction, int bounceLeft, float distance)
    {
        /*if (bounceLeft > 0)
        {
            RaycastHit2D raycastHit = Physics2D.Raycast(origin, direction, distance, mask);
            
            if (raycastHit.collider != null)
            {
                Debug.DrawLine(origin,direction, Color.green);
                
                Vector3 newDirection = Vector3.Reflect(direction, raycastHit.normal);
                
                Debug.DrawLine(raycastHit.point, newDirection,Color.green);
                
                bounceLeft --;
                distance -= raycastHit.distance;
                
                points.Add(raycastHit.point);   
                DoRay(raycastHit.point, newDirection, bounceLeft, distance);
            }
            else
            {
                Debug.DrawLine(origin,direction, Color.red);
            }   
        }*/
        
        if (bounceLeft > 0)
        {
            RaycastHit2D raycastHit = Physics2D.Raycast(origin, direction, distance, mask);;
            
            if (raycastHit.collider != null)
            {
                Vector2 newDirection = Vector2.Reflect(direction.normalized, raycastHit.normal);
                //Debug.Log(raycastHit.normal);

                bounceLeft --;
                distance -= raycastHit.distance;
                
                points.Add(raycastHit.point);
                //Debug.Log(raycastHit.transform.name);

                DoRay(raycastHit.point, newDirection, bounceLeft, distance);

            }
            else
            {
                //Debug.DrawRay(origin, direction, Color.red);
                points.Add(direction * distance + origin);
            }
        }
        
    }

    public void ReturnWeapon()
    {
        is_F_Pressed = true;
        points.Clear();
        PointCollission.instance.rb.velocity = Vector3.zero;
        PointCollission.instance.rb.angularVelocity = 0;
        PointCollission.instance.bounceInt = 1;
        
        //StartCoroutine(WaitForReturnWeapon());
        
        /*ThrowCollision.instance.gameObject.transform.DOMove(PlayerController.instance.transform.position, 0.2f)
            .SetEase(Ease.OutQuint);*/

        PointCollission.instance.transform.position = Vector3.MoveTowards(PointCollission.instance.transform.position,
            PlayerController.instance.transform.position, Time.deltaTime * ThrowSpeed[ThrowDamageIndex]);

        if (PointCollission.instance.transform.position == PlayerController.instance.transform.position)
        {
            isThrow = false;
            is_F_Pressed = false;
            PointCollission.instance.verifPremierTouch = false;
            ThrowCollision.instance.laFaux.SetActive(false);

            fauxScaleActual = fauxScaleBase;
            lerp = 0;
            
            CinemachineShake.instance.ShakeCamera(intensityAmplitude,intensityFrequency,intensityTime);
        }
    }


    public void ReturnWeaponImmediate()
    {
        is_F_Pressed = true;
        points.Clear();

        PointCollission.instance.transform.position = PlayerController.instance.transform.position;
        isThrow = false;
        is_F_Pressed = false;
        PointCollission.instance.verifPremierTouch = false;
        ThrowCollision.instance.laFaux.SetActive(false);

        fauxScaleActual = fauxScaleBase;
        lerp = 0;
    }

    IEnumerator WaitForReturnWeapon()
    {
        yield return new WaitForSeconds(0.2f);
    }


    void IsWeaponDisable(bool verif)
    {
        if (verif)
        {
            PlayerHeavyAttack.instance.enabled = false;
            PlayerLightAttack.instance.enabled = false;
        }
        else
        {
            PlayerHeavyAttack.instance.enabled = true;
            PlayerLightAttack.instance.enabled = true;
        }
    }
}
