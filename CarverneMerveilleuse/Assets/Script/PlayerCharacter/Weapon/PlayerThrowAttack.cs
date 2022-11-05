using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerThrowAttack : MonoBehaviour
{
    public  int maxBounce = 3;
    public float distance;
    public LineRenderer lineRender;
    public LayerMask mask;

    public List<Vector3> points = new List<Vector3>();

    public bool isThrow;

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
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Mouse1) && !isThrow)
        {
            Aim();
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                Debug.Log("Throw Weapon");
                ThrowCollision.instance.ThrowWeapon();
                isThrow = true;
            }
        }

        if (isThrow)
        {
            PlayerController.instance.speedMovement = 110;
            ThrowCollision.instance.IsWeaponActive(true);
            lineRender.gameObject.SetActive(false);
            
            if(Input.GetKey(KeyCode.F))
            {
                Debug.Log("return weapon");
                ReturnWeapon();
            }
        }
        else
        {
            PlayerController.instance.SecureSO();
            ThrowCollision.instance.gameObject.transform.position = PlayerController.instance.transform.position;
            IsWeaponDisable(false);
            ThrowCollision.instance.IsWeaponActive(false);
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
            
            if (Physics2D.Raycast(origin, direction, distance, mask))
            {
                Debug.DrawRay(origin, direction * raycastHit.distance, Color.green);
                Vector3 newDirection = Vector3.Reflect(direction, raycastHit.normal);
                Debug.DrawRay(raycastHit.point, newDirection,Color.green);
                bounceLeft --;
                distance -= raycastHit.distance;
                
                points.Add(raycastHit.point);
                
                DoRay(raycastHit.point, newDirection, bounceLeft, distance);
                
            }
            else
            {
                Debug.DrawRay(origin, direction* distance, Color.red);
                points.Add(direction * distance + origin);
            }
        }
        
    }

    void ReturnWeapon()
    {
        ThrowCollision.instance.rb.velocity = Vector3.zero;
        ThrowCollision.instance.rb.angularVelocity = 0;
        
        StartCoroutine(WaitForReturnWeapon());
        ThrowCollision.instance.gameObject.transform.DOMove(PlayerController.instance.transform.position, 0.2f)
            .SetEase(Ease.OutQuint);
    }

    IEnumerator WaitForReturnWeapon()
    {
        yield return new WaitForSeconds(0.2f);
        isThrow = false;
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
