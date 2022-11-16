using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnim : MonoBehaviour
{

    [Header("Animator")]
    [SerializeField] private List<Animator>  animator;
    [SerializeField] private GameObject graphFace;
    [SerializeField] private GameObject graphProfile;
    [SerializeField] private GameObject graphDos;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Update()
    {
        GameAnimatinon();
        foreach (var i in animator)
        {
            i.SetFloat("speedX", PlayerController.instance.rb.velocity.x);
            i.SetFloat("speedY", PlayerController.instance.rb.velocity.y);
            
            if (PlayerController.instance.rb.velocity.x > 0.3f || PlayerController.instance.rb.velocity.x < -0.3f)
            {
                i.SetBool("isProfileRunning", true);
            }
            else
            {
                i.SetBool("isProfileRunning", false);
            }
        
            if (PlayerController.instance.rb.velocity.y > 0.3f || PlayerController.instance.rb.velocity.y < -0.3f)
            {
                i.SetBool("isFaceRunning", true);
            }
            else
            {
                i.SetBool("isFaceRunning", false);
            }
        }
        
        if (Input.GetKey(KeyCode.Q)) graphProfile.transform.localScale = new Vector3(-0.08f,0.08f,0.08f);
        if (Input.GetKey(KeyCode.D)) graphProfile.transform.localScale = new Vector3(0.08f,0.08f,0.08f);
    }

    // Update is called once per frame
    void LateUpdate()
    {
        
    }
    
    private void GameAnimatinon()
    {
        if (Input.GetKey(KeyCode.Z))
        {
            graphFace.SetActive(false);
            graphDos.SetActive(true);
            graphProfile.SetActive(false);

            animator[2].SetBool("Face", false);
            animator[2].SetBool("Profile", false);
        }
        if (Input.GetKey(KeyCode.Q))
        {
            graphFace.SetActive(false);
            graphDos.SetActive(false);
            graphProfile.SetActive(true);
            
            animator[1].SetBool("Face", false);
            animator[1].SetBool("Profile", true);
        }
        if (Input.GetKey(KeyCode.S))
        {
            graphFace.SetActive(true);
            graphDos.SetActive(false);
            graphProfile.SetActive(false);

            animator[0].SetBool("Face", true);
            animator[0].SetBool("Profile", false);
        }
        if (Input.GetKey(KeyCode.D))
        {
            graphFace.SetActive(false);
            graphDos.SetActive(false);
            graphProfile.SetActive(true);

            animator[1].SetBool("Face", false);
            animator[1].SetBool("Profile", true);
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            animator[0].SetBool("isLightAttacking", true);
        }
        
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            animator[0].SetBool("isLightAttacking", false);
        }
    }
}
