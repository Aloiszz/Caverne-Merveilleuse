using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    [HideInInspector] public float buffATK;
    [HideInInspector] public float buffATKCritique;
    [HideInInspector] public int dropSupp;
    [HideInInspector] public int dropOrSupp;
    [HideInInspector] public int dropCoeurSupp;
    [HideInInspector] public int dashBuff;
    [HideInInspector] public float initialSpeed;
    [HideInInspector] public int nbVieEnPlus;
    [HideInInspector] public int regenVie;
    private bool canCrit;
    private bool canDashBuff;
    private PlayerController player;

    public static ItemManager instance;
    
    [Header("Arme Principal")]
    public float pourcentageBuffDegats = 20;
    public int nbPVRecupVie = 1;
    
    [Header("Arme Beyblade")]
    
    [Header("Arme lancé de faux")]
    
    [Header("PV")]
    public int nbPVEnPlus = 2;
    public int pourcentageVieForCritique = 10;
    public float pourcentageBuffATKDuSeuilVie;

    [Header("Dash")] 
    public int pourcentageAttaqueEnPlusPostDash = 10;
    public int pourcentageSpeedEnPlusPostDash = 20;

    [Header("Drop")] 
    public int nombreDentDropEnPlus = 2;
    public int pourcentageDropOrEnPlus;
    public int pourcentageDropCoeurEnPlus = 10;

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
    private void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        initialSpeed = player.speedMovement;
    }

    public void OnBuy(string itemName)
    {
        switch (itemName)
        {
            case "DegatsAP":
                buffATK = pourcentageBuffDegats / 100;
                Debug.Log("augmenter attaque");
                break;
            
            case "CooldownAP":
                Debug.Log("Baisse le cooldown");
                break;
            
            case "PushAP":
                Debug.Log("Le 3ème coup push les ennemis");
                break;
            
            case "PortéeAB":
                Debug.Log("Augmente la portée du coup");
                break;
            
            case "TourAB":
                Debug.Log("Permet de faire un tour en plus");
                break;
            
            case "PushAB":
                Debug.Log("Push plus loins les ennemis");
                break;
            
            case "RebondALF":
                Debug.Log("Augmente le nombre de rebond");
                break;
            
            case "ChargementALF":
                Debug.Log("éduit le temps de chargement");
                break;
            
            case "TailleALF":
                Debug.Log("Augmente la taille de l'AOE");
                break;
            
            case "MaxPV":
                Debug.Log("Augmente le max PV");
                player.lifeDepard += nbPVEnPlus;
                break;
            
            case "SeuilPV":
                Debug.Log("Augmente les dégâts quand les PV passent en dessous d'un certain seuil");
                canCrit = true;
                break;
            
            case "BouclierPV":
                Debug.Log("Pendant les première seconde, une lumière vous entours et vous sert de bouclier");
                break;
            
            case "DegatsDash":
                Debug.Log("Inflige des dégâts aux ennemis sur la trajectoire du dash");
                break;
            
            case "NombreDash":
                Debug.Log("Permet de faire un dash en plus");
                break;
            
            case "PushDash":
                Debug.Log("Push les ennemis sur la trajectoire du dash");
                break;
            
            case "PVDrop":
                Debug.Log("Augmente le taux de drop de PV");
                dropCoeurSupp = pourcentageDropCoeurEnPlus;
                break;
            
            case "ArgentDrop":
                Debug.Log("Augmente le taux de drop des dents en argent");
                dropOrSupp = pourcentageDropOrEnPlus;
                break;
            
            case "CommuneDrop":
                Debug.Log("Augmente le taux de drop des dents communes");
                dropSupp = nombreDentDropEnPlus;
                break;
            
            case "FenteAP":
                Debug.Log("Votre 3ème coup créer une fente devant vous qui inflige des dégâts");
                break;
            
            case "RecupereVieAP":
                Debug.Log("A chaque fois que vous attaquez avec cette attaque, vous récupérez un pourcentage de vie en fonction des dégâts infligé");
                regenVie = nbPVRecupVie;
                break;
            
            case "InvincibleAB":
                Debug.Log("Vous ne subissez pas de dégâts quand vous tournez");
                break;
            
            case "DeplacerAB":
                Debug.Log("Vous pouvez vous déplacer quand vous tournez");
                break;
            
            case "ExplosifALF":
                Debug.Log("Vos projectiles deviennent explosifs, ils infligent plus de dégâts et font des dégâts de zones quand ils touchent un ennemi");
                break;
            
            case "DiviseALF":
                Debug.Log("Votre faux se divise maintenant en 2, la taille est aussi divisée en 2");
                break;
            
            case "RegenPV":
                Debug.Log("Votre vie se régénère au fur et à mesure du temps pendant un combat");
                break;
            
            case "SecondeviePV":
                Debug.Log("Quand votre vie tombe à zéro, elle remonte jusqu'à la moitié de la jauge de vie (une seule fois par game)");
                nbVieEnPlus += 1;
                break;
            
            case "PetroleDash":
                Debug.Log("Vous laissez un trait de pétrole derrière vous, les ennemis subissent des dégâts et sont ralentis quand ils se trouvent dessus");
                break;
            
            case "VitesseDegatsDash":
                Debug.Log("Votre vitesse de déplacement et vos dégâts sont augmentés après un dash");
                canDashBuff = true;
                break;
        }
    }

    private void Update()
    {
        if (player.life <= player.lifeDepard / pourcentageVieForCritique && canCrit)
        {
            buffATKCritique = pourcentageBuffATKDuSeuilVie / 100;
        }
        else
        {
            buffATKCritique = 0;
        }

        if (!player.playerSO.isDash && canDashBuff)
        {
            player.speedMovement = initialSpeed * (pourcentageSpeedEnPlusPostDash / 100);
            dashBuff = pourcentageAttaqueEnPlusPostDash / 100;
        }
        else
        {
            player.speedMovement = initialSpeed;
            dashBuff = 0;
        }
    }

    
}
