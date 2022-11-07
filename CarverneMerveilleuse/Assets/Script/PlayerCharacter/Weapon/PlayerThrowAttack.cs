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
    
    private Quaternion deflectRotation;
    private Vector3 deflectDirection;

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

    Vector2 revertDir(bool topCollide, Vector2 dir)
    {
        if (!topCollide)
        {
            return new Vector2(dir.x, -dir.y);
        }
        else
        {
            return new Vector2(-dir.x, dir.y);
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
            
            if (raycastHit.collider != null)
            {
                Vector2 newDirection = Vector2.Reflect(direction.normalized, raycastHit.normal);
                //Debug.Log(raycastHit.normal);

                bounceLeft --;
                distance -= raycastHit.distance;
                
                points.Add(raycastHit.point);
                Debug.Log(raycastHit.transform.name);
                
                deflectRotation =
                    Quaternion.FromToRotation(-direction, raycastHit.normal);
            
                deflectDirection = deflectRotation * raycastHit.normal * this.distance;
                
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
        ThrowCollision.instance.rb.velocity = Vector3.zero;
        ThrowCollision.instance.rb.angularVelocity = 0;
        ThrowCollision.instance.bounceInt = 2;
        
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






/*
public class ArrowAim : MonoBehaviour
{
    [Header("Player")] public PlayerBehavior player;

    [Header("Values")] public Vector3 direction, directionDraw;
    public float distanceRayCast = 10f;
    public int inputIndex;

    private Quaternion deflectRotation;
    private Vector3 deflectDirection;
    private bool hit2Initialized;
    private float incrementCurve, graph;
    private bool playOnce;

    [Header("Materials")] [SerializeField] private GameObject outline;

    [Header("Components")] public ObjectBehavior controlledItem;
    public RaycastHit2D hit, hit2;
    public GameObject initialPosition;
    public float sizeY, sizeMultiplierX;
    
    [HideInInspector]public LineRenderer line;
    private bool pressedGrab;
    private GameObject lastHitActor;
    private LayerMask layerMask;
    private List<Transform> points;

    [SerializeField] private List<SpriteRenderer> mtrails;

    private void Start()
    {
        layerMask = ~ LayerMask.GetMask
            ("Player", "HideNotBlockRaycast");
        line = GetComponentInChildren<LineRenderer>();
        inputIndex = 1;
    }

    void Update()
    {
        direction = transform.TransformDirection(Mathf.Abs(player.angle), 0, 0);

        directionDraw = transform.TransformDirection
            (Mathf.Abs(player.angle) * 10f, 0, 0);

        if (transform.localScale.x < 0f)
        {
            direction.x = -direction.x;
        }

        hit = Physics2D.Raycast(initialPosition.transform.position,
            direction, distanceRayCast, layerMask);
        
        if (!hit2Initialized)
        {
            HitManager(hit);
        }
        
        
        if (hit.collider != null)
        {
            Vector2 x = mtrails[0].size;
            x = new Vector2(Vector2.Distance(initialPosition.transform.position, hit.point) * 2, sizeY);
            mtrails[0].size = x * sizeMultiplierX;
            
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            mtrails[0].transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

            mtrails[1].enabled = true;
        }
        else
        {
            Vector2 x2 = mtrails[0].size;
            x2 = new Vector2(distanceRayCast * 2,sizeY);
            mtrails[0].size = x2 * sizeMultiplierX;
            
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            mtrails[0].transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

            mtrails[1].enabled = false;
        }

        if (hit.collider != null && hit.collider.CompareTag("AimAssist") ||
            hit.collider.CompareTag("GrabTag"))
        {
            hit2Initialized = false;
            mtrails[1].enabled = false;
        }
        else if(hit.collider != null)
        {
            deflectRotation =
                Quaternion.FromToRotation(-direction, hit.normal);
            
            deflectDirection = deflectRotation * hit.normal * distanceRayCast;

            hit2 = Physics2D.Raycast(hit.point,
                deflectDirection, distanceRayCast, layerMask);
            
            mtrails[1].enabled = true;

            hit2Initialized = true;
            HitManager(hit2);
            
            if (hit2.collider != null)
            {
                Vector2 x = mtrails[1].size;
                x = new Vector2(Vector2.Distance(hit2.point, hit.point) * 2f, sizeY);
                mtrails[1].size = x * sizeMultiplierX;
                
                float angle = Mathf.Atan2(deflectDirection.y, deflectDirection.x) * Mathf.Rad2Deg;
                
                mtrails[1].transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
                mtrails[1].transform.position = new Vector3(hit.point.x, hit.point.y, 0);
            }
            else
            {
                Vector2 x = mtrails[1].size;
                x = new Vector2(distanceRayCast * 2f, sizeY);
                mtrails[1].size = x * sizeMultiplierX;
                
                float angle = Mathf.Atan2(deflectDirection.y, deflectDirection.x) * Mathf.Rad2Deg;
                
                mtrails[1].transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
                mtrails[1].transform.position = new Vector3(hit.point.x, hit.point.y, 0);
            }
        }
    }

    private void HitManager(RaycastHit2D hitParam)
    {
        if (hitParam.collider != null)
        {
            if (hitParam.collider != null && hitParam.collider.gameObject
                .CompareTag("GrabTag") || hitParam.collider.gameObject
                .CompareTag("AimAssist"))
            {
                foreach (Transform x in hitParam.collider.transform)
                {
                    if (x.name == "Outline")
                    {
                        outline = x.gameObject;
                        outline.gameObject.GetComponent<SpriteRenderer>().DOFade(1f, 0f);
                    }
                }

                lastHitActor = hitParam.collider.gameObject;
            }
        }
        else
        {
            if (lastHitActor != null && outline != null)
            {
                outline.gameObject.GetComponent<SpriteRenderer>().DOFade(0f, 0f);
            }
        }

        if (Input.GetAxis("Grab") != 0 && player.isPressed == false)
        {
            if (hitParam.collider != null && hitParam.collider.gameObject
                .GetComponentInParent<Rigidbody2D>() != null)
            {
                controlledItem = hitParam.collider.gameObject
                    .GetComponentInParent<ObjectBehavior>();

                controlledItem.StartControl();
                Invoke("Bool", player.timeBeforeBool);

                if (controlledItem.isActive == false)
                {
                    controlledItem.isActive = true;
                }
            }
        }
    }

    void Bool()
    {
        player.isPressed = true;
    }

    public void ResetOutlines()
    {
        if (lastHitActor != null && outline != null)
        {
            outline.gameObject.GetComponent<SpriteRenderer>().DOFade(0f, 0f);
        }
    }
    
    public void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(transform.position, directionDraw);
        
        if (hit.collider != null && hit.collider.CompareTag("GrabTag") == false)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(hit.point, deflectDirection);
        }
    }
}*/
