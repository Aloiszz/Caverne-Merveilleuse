using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerThrowAttack : MonoBehaviour
{
    public  int maxBounce = 3;
    public float distance;
    public LineRenderer lineRender;
    public LayerMask mask;

    public List<Vector3> points = new List<Vector3>();


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
        if (Input.GetKey(KeyCode.Mouse1))
        {
            IsWeaponDisable(true);
            
            lineRender.gameObject.SetActive(true);
            
            points.Clear();
            points.Add(transform.position);
        
            DoRay(PlayerController.instance.transform.position, PlayerAttackCollision.instance.difference, maxBounce, distance); 
            lineRender.positionCount = points.Count;
        
            lineRender.SetPositions(points.ToArray());
        }

        if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            IsWeaponDisable(false);
            
            lineRender.gameObject.SetActive(false);
        }
        
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
