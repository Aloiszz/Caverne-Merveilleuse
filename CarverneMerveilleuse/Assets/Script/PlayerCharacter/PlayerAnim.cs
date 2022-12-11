using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerAnim : MonoBehaviour
{

    [Header("Animator")]
    [SerializeField] private List<Animator>  animator;
    [SerializeField] private GameObject graphFace;
    [SerializeField] private GameObject graphProfile;
    [SerializeField] private GameObject graphDos;
    
    
    private void Update()
    {
        foreach (var i in animator)
        {
            i.SetFloat("speedX", PlayerController.instance.rb.velocity.x);
            i.SetFloat("speedY", PlayerController.instance.rb.velocity.y);
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (!PlayerController.instance.isDashing)
        {
            GameAnimation();
        }
        else
        {
            graphDos.SetActive(false);
            graphFace.SetActive(false);
            graphProfile.SetActive(false);
        }
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
    } // oublis cette version 
    
    
    private void GameAnimation()
    {
        if (PlayerAttackCollision.instance.rotationZ < -145 || PlayerAttackCollision.instance.rotationZ > 145) // profil G
        {
            if (PlayerController.instance.rb.velocity.x > 0.3f || PlayerController.instance.rb.velocity.x < -0.3f
                                                               || PlayerController.instance.rb.velocity.y > 0.3f || PlayerController.instance.rb.velocity.y < -0.3f)
            {
                animator[1].SetBool("isProfileRunning", true);
            }
            else
            {
                animator[1].SetBool("isProfileRunning", false);
            }
            
            ProfilGauche();
        }
        
        else if (PlayerAttackCollision.instance.rotationZ > -145 && PlayerAttackCollision.instance.rotationZ < -39) // Face
        {
            if (PlayerController.instance.rb.velocity.x > 0.3f || PlayerController.instance.rb.velocity.x < -0.3f
                                                               || PlayerController.instance.rb.velocity.y > 0.3f || PlayerController.instance.rb.velocity.y < -0.3f)
            {
                animator[0].SetBool("isFaceRunning", true);
            }
            else
            {
                animator[0].SetBool("isFaceRunning", false);
            }
            
            Face();
        }
        
        else if(PlayerAttackCollision.instance.rotationZ < 145 && PlayerAttackCollision.instance.rotationZ > 39) // dos
        {
            if (PlayerController.instance.rb.velocity.x > 0.3f || PlayerController.instance.rb.velocity.x < -0.3f
                                                               || PlayerController.instance.rb.velocity.y > 0.3f || PlayerController.instance.rb.velocity.y < -0.3f)
            {
                animator[2].SetBool("isFaceRunning", true);
            }
            else
            {
                animator[2].SetBool("isFaceRunning", false);
            }
            
            Dos();
        }
        
        else //Profil D
        {
            if (PlayerController.instance.rb.velocity.x > 0.3f || PlayerController.instance.rb.velocity.x < -0.3f
                                                               && PlayerController.instance.rb.velocity.y > 0.3f || PlayerController.instance.rb.velocity.y < -0.3f)
            {
                animator[1].SetBool("isProfileRunning", true);
            }
            else
            {
                animator[1].SetBool("isProfileRunning", false);
            }
            
            ProfilDroit();
        }
    }


    #region Profil

    void ProfilDroit()
    {
        graphFace.SetActive(false);
        graphDos.SetActive(false);
        graphProfile.SetActive(true);

        animator[1].SetBool("Face", false);
        animator[1].SetBool("Profile", true);
            
        graphProfile.transform.localScale = new Vector3(0.1f,0.1f,0.1f);
        //graphProfile.transform.DOScale(new Vector3(0.08f,0.08f,0.08f), .2f);

        ProfilLightAttack();
        ProfileHeavyAttack();
        ProfilThrowAttack();
    }

    void ProfilGauche()
    {
        graphFace.SetActive(false);
        graphDos.SetActive(false);
        graphProfile.SetActive(true);
            
        animator[1].SetBool("Face", false);
        animator[1].SetBool("Profile", true);
            
        graphProfile.transform.localScale = new Vector3(-0.1f,0.1f,0.1f);
        //graphProfile.transform.DOScale(new Vector3(-0.08f,0.08f,0.08f), .2f);

        ProfilLightAttack();
        ProfileHeavyAttack();
        ProfilThrowAttack();
    }

    void ProfilLightAttack()
    {
        if (PlayerLightAttack.instance.strikkingCombo1)
        {
            animator[1].SetBool("isLightAttacking", true);
        }
        else
        {
            animator[1].SetBool("isLightAttacking", false);
            
            if (PlayerLightAttack.instance.strikkingCombo2)
            {
                animator[1].SetBool("isLightAttacking2", true);
            }
            else
            {
                animator[1].SetBool("isLightAttacking2", false);
                
                if (PlayerLightAttack.instance.strikkingCombo3)
                {
                    animator[1].SetBool("isLightAttacking3", true);
                }
                else
                {
                    animator[1].SetBool("isLightAttacking3", false);
                }
            }
        }
    }
    
    void ProfileHeavyAttack()
    {
        /*if (PlayerHeavyAttack.instance.isCharge)
        {
            animator[1].SetBool("isChargeAttack", true);
        }
        else
        {
            animator[1].SetBool("isChargeAttack", false);
        }*/
    }
    
    void ProfilThrowAttack()
    {
        if (PlayerThrowAttack.instance.isThrow)
        {
            animator[1].SetBool("isThrowAttack", true);
            animator[1].SetBool("havingFaux", false);
        }
        else
        {
            animator[1].SetBool("isThrowAttack", false);
            animator[1].SetBool("havingFaux", true);
        }
    }

    #endregion

    #region Face

    void Face()
    {
        graphFace.SetActive(true);
        graphDos.SetActive(false);
        graphProfile.SetActive(false);

        animator[0].SetBool("Face", true);
        animator[0].SetBool("Profile", false);
        
        FaceLightAttack();
        FaceHeavyAttack();
        FaceThrowAttack();
    }
    

    void FaceLightAttack()
    {
        if (PlayerLightAttack.instance.strikkingCombo1)
        {
            animator[0].SetBool("isLightAttacking", true);
        }
        else
        {
            animator[0].SetBool("isLightAttacking", false);
            
            if (PlayerLightAttack.instance.strikkingCombo2)
            {
                animator[0].SetBool("isLightAttacking2", true);
            }
            else
            {
                animator[0].SetBool("isLightAttacking2", false);
                
                if (PlayerLightAttack.instance.strikkingCombo3)
                {
                    animator[0].SetBool("isLightAttacking3", true);
                }
                else
                {
                    animator[0].SetBool("isLightAttacking3", false);
                }
            }
        }
        
    }
    void FaceHeavyAttack()
    {
        /*if (PlayerHeavyAttack.instance.isCharge)
        {
            animator[0].SetBool("isChargeAttack", true);
        }
        else
        {
            animator[0].SetBool("isChargeAttack", false);
        }*/
    }

    void FaceThrowAttack()
    {
        if (PlayerThrowAttack.instance.isThrow)
        {
            animator[0].SetBool("isThrowAttack", true);
            animator[0].SetBool("havingFaux", false);
        }
        else
        {
            animator[0].SetBool("isThrowAttack", false);
            animator[0].SetBool("havingFaux", true);
        }
    }

    #endregion
    
    #region DOS

    void Dos()
    {
        graphFace.SetActive(false);
        graphDos.SetActive(true);
        graphProfile.SetActive(false);

        animator[2].SetBool("Face", false);
        animator[2].SetBool("Profile", false);

        DosLightAttack();
        DosHeavyAttack();
        DosThrowAttack();
    }
    
    void DosLightAttack()
    {
        if (PlayerLightAttack.instance.strikkingCombo1)
        {
            animator[2].SetBool("isLightAttacking", true);
        }
        else
        {
            animator[2].SetBool("isLightAttacking", false);
            
            if (PlayerLightAttack.instance.strikkingCombo2)
            {
                animator[2].SetBool("isLightAttacking2", true);
            }
            else
            {
                animator[2].SetBool("isLightAttacking2", false);
                
                if (PlayerLightAttack.instance.strikkingCombo3)
                {
                    animator[2].SetBool("isLightAttacking3", true);
                }
                else
                {
                    animator[2].SetBool("isLightAttacking3", false);
                }
            }
        }
    }
    
    void DosHeavyAttack()
    {
        /*if (PlayerHeavyAttack.instance.isCharge)
        {
            animator[2].SetBool("isChargeAttack", true);
        }
        else
        {
            animator[2].SetBool("isChargeAttack", false);
        }*/
    }
    
    void DosThrowAttack()
    {
        if (PlayerThrowAttack.instance.isThrow)
        {
            animator[2].SetBool("isThrowAttack", true);
            animator[2].SetBool("havingFaux", false);
        }
        else
        {
            animator[2].SetBool("isThrowAttack", false);
            animator[2].SetBool("havingFaux", true);
        }
    }

    #endregion
    
}
