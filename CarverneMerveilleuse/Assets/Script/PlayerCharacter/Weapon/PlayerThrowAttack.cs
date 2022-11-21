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
    private bool is_F_Pressed;
    
    private Quaternion deflectRotation;
    private Vector3 deflectDirection;

    public List<Vector3> points = new List<Vector3>();

    public bool isThrow;
    public bool isInGrosProjo;

    public List<float> ThrowSpeed;
    [HideInInspector] public int ThrowSpeedIndex;

    public List<float> ThrowDamage;
    [HideInInspector] public int ThrowDamageIndex;
    

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
        if (Input.GetKey(KeyCode.Mouse1) && !isThrow)
        {
            Aim();
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                Debug.Log("Throw Weapon");
                
                isThrow = true;
            }
        }

        if (isThrow)
        {
            ThrowCollision.instance.laFaux.SetActive(true);
            if (!isInGrosProjo)
            {
                PlayerController.instance.speedMovement = 110;
            }
            ThrowCollision.instance.IsWeaponActive(true);
            PointCollission.instance.IsWeaponActive(true);
            lineRender.gameObject.SetActive(false);
            
            if(Input.GetKey(KeyCode.F))
            {
                Debug.Log("return weapon");
                is_F_Pressed = true;
            }

            if (is_F_Pressed)
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
            if (!isInGrosProjo)
            {
                PlayerController.instance.speedMovement = PlayerController.instance.playerSO.speedMovement;
            }
            
            //ThrowCollision.instance.gameObject.transform.position = PlayerController.instance.transform.position;
            PointCollission.instance.gameObject.transform.position = PlayerController.instance.transform.position;
            IsWeaponDisable(false);
            ThrowCollision.instance.IsWeaponActive(false);
            PointCollission.instance.IsWeaponActive(false);
        }
    }


    void Aim()
    {
        IsWeaponDisable(true);
            
        lineRender.gameObject.SetActive(true);
            
        points.Clear();
        points.Add(transform.position);
        
        DoRay(PlayerController.instance.transform.position, PlayerAttackCollision.instance.difference, maxBounce, distance); 
        lineRender.positionCount = points.Count;
        
        lineRender.SetPositions(points.ToArray());
    }

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
        }
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
