using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    
    private bool canCrit;
    private bool canDashBuff;
    private PlayerController player;

    public static ItemManager instance;
    
    [Header("Arme Principal")]
    public float degatsAP = 20;
    [HideInInspector] public float buffATK;
    public float pushAP = 50;
    [HideInInspector] public float puissancePush;
    public float cooldownAP = 0.5f;
    [HideInInspector] public float endComboSoustracteur;
    public int recupereVieAP = 1;
    [HideInInspector] public int regenVie;
    public GameObject fente;
    public int nbCoupFente = 5;
    [HideInInspector] public bool isFenteAPGet;

    [Header("Arme Beyblade")] 
    public float pourcentagePortéeEnPlus = 20;
    [HideInInspector] public float valeurNouvelleRange;
    public float pourcentagePushABEnPlus = 40;
    [HideInInspector] public float buffPushAB;
    [HideInInspector] public bool canMoveWhileBeyblade;
    [HideInInspector] public bool beybladeInvinsible;

    [Header("Arme lancé de faux")] 
    public float pourcentagePortéeEnPlusALF = 20;
    public int nbRebondEnPlusALF = 1;
    public GameObject explosion;
    public float tailleExplosion = 4;
    [HideInInspector] public bool isExplosfALFGet;
    
    [Header("PV")]
    public int MaxPV = 2;
    public int pourcentageVieForSeuilPV = 10;
    public float pourcentageBuffATKDuSeuilPV;
    [HideInInspector] public float buffATKCritique;
    [HideInInspector] public int nbVieEnPlus;
    
    [Header("Dash")] 
    public float puissancePushDash;
    [HideInInspector] public bool isPushDashGet;
    [HideInInspector] public bool isDegatDashGet;
    public float pourcentageAttaqueEnPlusPostDash = 10;
    [HideInInspector] public float dashBuff;
    public float pourcentageSpeedEnPlusPostDash = 20;
    [HideInInspector] public float initialSpeed;
    public GameObject petrole;
    public int nbTachePetrole;
    public float secondAvantDisparitionPetrole;
    [HideInInspector] public bool isPetroleDashGet;

    [Header("Drop")] 
    public int nombreDentDropEnPlus = 2;
    [HideInInspector] public int dropSupp;
    public int pourcentageDropOrEnPlus;
    [HideInInspector] public int dropOrSupp;
    public int pourcentageDropCoeurEnPlus = 10;
    [HideInInspector] public int dropCoeurSupp;

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
            case "DegatsAP(Clone)":
                buffATK = degatsAP / 100;
                //Debug.Log("augmenter attaque");
                break;
            
            case "CooldownAP(Clone)":
                //Debug.Log("Baisse le cooldown");
                endComboSoustracteur = cooldownAP;
                break;
            
            case "PushAP(Clone)":
                //Debug.Log("Le 3ème coup push les ennemis");
                puissancePush = pushAP;
                break;
            
            case "PortéeAB(Clone)":
                //Debug.Log("Augmente la portée du coup");
                valeurNouvelleRange = pourcentagePortéeEnPlus / 100 + 1;
                HeavyAttackCollision.instance.transform.localScale *= valeurNouvelleRange;
                break;
            
            case "TourAB(Clone)":
                //Debug.Log("Permet de faire un tour en plus");
                PlayerHeavyAttack.instance.numberOfTurn[PlayerHeavyAttack.instance.numberOfTurnIndex] += 1;
                break;
            
            case "PushAB(Clone)":
                //Debug.Log("Push plus loins les ennemis");
                buffPushAB = pourcentagePushABEnPlus / 100 + 1;
                break;
            
            case "RebondALF(Clone)":
                //Debug.Log("Augmente le nombre de rebond");
                PlayerThrowAttack.instance.maxBounce += nbRebondEnPlusALF;
                break;
            
            case "ChargementALF(Clone)":
                Debug.Log("éduit le temps de chargement");
                break;
            
            case "TailleALF(Clone)":
                //Debug.Log("Augmente la taille de l'AOE");
                ThrowCollision.instance.transform.localScale = new Vector2(
                    ThrowCollision.instance.transform.localScale.x * (pourcentagePortéeEnPlusALF / 100 + 1),
                    ThrowCollision.instance.transform.localScale.y * (pourcentagePortéeEnPlusALF / 100 + 1));
                break;
            
            case "MaxPV(Clone)":
                //Debug.Log("Augmente le max PV");
                PlayerController.instance.life += MaxPV;
                break;
            
            case "SeuilPV(Clone)":
                //Debug.Log("Augmente les dégâts quand les PV passent en dessous d'un certain seuil");
                canCrit = true;
                break;
            
            case "BouclierPV(Clone)":
                Debug.Log("Pendant les première seconde, une lumière vous entours et vous sert de bouclier");
                break;
            
            case "DegatsDash(Clone)":
                //Debug.Log("Inflige des dégâts aux ennemis sur la trajectoire du dash");
                isDegatDashGet = true;
                break;
            
            case "NombreDash(Clone)":
                //Debug.Log("Permet de faire un dash en plus");
                PlayerController.instance.nbPossibleDash += 1;
                break;
            
            case "PushDash(Clone)":
                //Debug.Log("Push les ennemis sur la trajectoire du dash");
                isPushDashGet = true;
                break;
            
            case "PVDrop(Clone)":
                //Debug.Log("Augmente le taux de drop de PV");
                dropCoeurSupp = pourcentageDropCoeurEnPlus;
                break;
            
            case "ArgentDrop(Clone)":
                //Debug.Log("Augmente le taux de drop des dents en argent");
                dropOrSupp = pourcentageDropOrEnPlus;
                break;
            
            case "CommuneDrop(Clone)":
                //Debug.Log("Augmente le taux de drop des dents communes");
                dropSupp = nombreDentDropEnPlus;
                break;
            
            case "FenteAP(Clone)":
                //Debug.Log("Votre 3ème coup créer une fente devant vous qui inflige des dégâts");
                isFenteAPGet = true;
                break;
            
            case "RecupereVieAP(Clone)":
                //Debug.Log("A chaque fois que vous attaquez avec cette attaque, vous récupérez un pourcentage de vie en fonction des dégâts infligé");
                regenVie = recupereVieAP;
                break;
            
            case "InvincibleAB(Clone)":
                //Debug.Log("Vous ne subissez pas de dégâts quand vous tournez");
                beybladeInvinsible = true;
                break;
            
            case "DeplacerAB(Clone)":
                //Debug.Log("Vous pouvez vous déplacer quand vous tournez");
                canMoveWhileBeyblade = true;
                break;
            
            case "ExplosifALF(Clone)":
                //Debug.Log("Vos projectiles deviennent explosifs, ils infligent plus de dégâts et font des dégâts de zones quand ils touchent un ennemi");
                isExplosfALFGet = true;
                break;
            
            case "DiviseALF(Clone)":
                Debug.Log("Votre faux se divise maintenant en 2, la taille est aussi divisée en 2");
                break;
            
            case "RegenPV(Clone)":
                Debug.Log("Votre vie se régénère au fur et à mesure du temps pendant un combat");
                break;
            
            case "SecondeviePV(Clone)":
                //Debug.Log("Quand votre vie tombe à zéro, elle remonte jusqu'à la moitié de la jauge de vie (une seule fois par game)");
                nbVieEnPlus += 1;
                break;
            
            case "PetroleDash(Clone)":
                Debug.Log("Vous laissez un trait de pétrole derrière vous, les ennemis subissent des dégâts et sont ralentis quand ils se trouvent dessus");
                isPetroleDashGet = true;
                break;
            
            case "VitesseDegatsDash(Clone)":
                //Debug.Log("Votre vitesse de déplacement et vos dégâts sont augmentés après un dash");
                canDashBuff = true;
                break;
        }
    }

    private void Update()
    {
        if (player.life <= player.lifeDepard / pourcentageVieForSeuilPV && canCrit)
        {
            buffATKCritique = pourcentageBuffATKDuSeuilPV / 100;
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
            dashBuff = 0;
        }
    }

    
}
