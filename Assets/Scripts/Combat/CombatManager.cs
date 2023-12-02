using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CombatManager : MonoBehaviour
{
    [Header("UI REFERENCES")]
    [SerializeField] private GameObject victoryScreen;
    [SerializeField] private GameObject defeatScreen;
    [SerializeField] private TextMeshProUGUI chronoTextWin;
    [SerializeField] private TextMeshProUGUI chronoTextLose;

    [Header("PLAYER REFERENCES")]
    [SerializeField] private Transform player;
    [SerializeField] private Animator animatorPlayer;
    [SerializeField] private GameObject canvasPlayer;
    [SerializeField] private Slider sliderViePlayer;
    [SerializeField] private GameObject manaContent;
    [SerializeField] private GameObject manaEnergy;
    [SerializeField] private Animator manaCostAnimator;
    [SerializeField] private TextMeshProUGUI manaCostText;
    [SerializeField] private GameObject buttonAttack;
    [SerializeField] private GameObject buttonMagic;
    [SerializeField] private Animator animatorTextDamagePlayer;
    [SerializeField] private TextMeshProUGUI textDamagePlayer;
    [SerializeField] private ParticleSystem particleSystemLightning;

    [Header("SPELL REFERENCES")]
    [SerializeField] private Animator spellWheelAnimator;
    [SerializeField] private GameObject buttonOffensiveSpell;
    [SerializeField] private GameObject buttonUtilitySpell;
    [SerializeField] private Image imageOffensiveSpell;
    [SerializeField] private Image imageUtilitySpell;
    [SerializeField] private Sprite lightningSpell;
    [SerializeField] private Sprite fireSpell;
    [SerializeField] private Sprite iceSpell;
    [SerializeField] private Sprite shieldSpell;
    [SerializeField] private Sprite healSpell;
    [SerializeField] private Sprite buffAttackSpell;
    [SerializeField] private TextMeshProUGUI offensiveSpellCostText;
    [SerializeField] private TextMeshProUGUI utilitySpellCostText;
    private int offensiveSpellCost;
    private int utilitySpellCost;

    [Header("ENEMY REFERENCES")]
    [SerializeField] private Slider sliderVieEnnemi;
    [SerializeField] private GameObject healthBarEnemy;
    [SerializeField] private Animator animatorTextDamageEnemy;
    [SerializeField] private TextMeshProUGUI textDamageEnemy;
    [SerializeField] private TextMeshProUGUI textEnemyName;
    [SerializeField] private TextMeshProUGUI textEnemyLevel;

    [Header("WEREWOLF REFERENCES")]
    [SerializeField] private GameObject werewolfGO;
    [SerializeField] private Transform werewolf;
    [SerializeField] private Animator animatorWerewolf;

    private Vector3 playerDestinationAttack;
    private Vector3 enemyDestinationAttack;
    private Transform enemy;
    private Animator enemyAnimator;

    private List<Transform> transparentChildren;

    private float damages;
    private int currentMana;
    private int maxMana;
    private float chrono;


    private void Awake()
    {
        SavedData savedData = SaveSystem.currentSave;
        SaveSystem.instance.SaveGame();

        /* ----- Gestion du chargement de l'ennemi ----- */
        textEnemyLevel.text = savedData.enemyLevel.ToString();

        switch (savedData.enemyName)
        {
            case "Werewolf":
                werewolfGO.SetActive(true);
                enemyAnimator = animatorWerewolf;
                enemy = werewolf;
                textEnemyName.text = "LOUP-GAROU";
                break;
        }

        /* ----- Gestion de l'initialisation du mana ----- */
        currentMana = SaveSystem.currentSave.playerManaMax;
        maxMana = currentMana;

        // On peuple le nombre de unité de mana dans la barre en fonction du mana max du joueur
        for (int i = 0; i < maxMana; i++)
        {
            GameObject instantiatedPrefab = Instantiate(manaEnergy, Vector3.zero, Quaternion.identity);
            instantiatedPrefab.transform.SetParent(manaContent.transform, false);
        }

        /* ----- Gestion du chargement des spells offensifs ----- */
        switch (savedData.spellSave.currentOffensiveSpell)
        {
            case "lightning":
                imageOffensiveSpell.sprite = lightningSpell;
                offensiveSpellCostText.text = savedData.spellSave.spellTypes["lightning"].cost.ToString();
                offensiveSpellCost = savedData.spellSave.spellTypes["lightning"].cost;
                break;
            case "fire":
                imageOffensiveSpell.sprite = fireSpell;
                offensiveSpellCostText.text = savedData.spellSave.spellTypes["fire"].cost.ToString();
                offensiveSpellCost = savedData.spellSave.spellTypes["fire"].cost;
                break;
            case "ice":
                imageOffensiveSpell.sprite = iceSpell;
                offensiveSpellCostText.text = savedData.spellSave.spellTypes["ice"].cost.ToString();
                offensiveSpellCost = savedData.spellSave.spellTypes["ice"].cost;
                break;
        }

        /* ----- Gestion du chargement des spells utilitaires ----- */
        switch (savedData.spellSave.currentUtilitySpell)
        {
            case "shield":
                imageUtilitySpell.sprite = shieldSpell;
                utilitySpellCostText.text = savedData.spellSave.spellTypes["shield"].cost.ToString();
                utilitySpellCost = savedData.spellSave.spellTypes["shield"].cost;
                break;
            case "heal":
                imageUtilitySpell.sprite = healSpell;
                utilitySpellCostText.text = savedData.spellSave.spellTypes["heal"].cost.ToString();
                utilitySpellCost = savedData.spellSave.spellTypes["heal"].cost;
                break;
            case "buffAttack":
                imageUtilitySpell.sprite = buffAttackSpell;
                utilitySpellCostText.text = savedData.spellSave.spellTypes["buffAttack"].cost.ToString();
                utilitySpellCost = savedData.spellSave.spellTypes["buffAttack"].cost;
                break;
            
        }

        chrono = Time.time;
    }

    public void PlayerAttack()
    {
        StartCoroutine(PlayerTurn(false, ""));
    }

    public void PlayerMagic(string spellType)
    {
        StartCoroutine(PlayerTurn(true, spellType));
    }

    public void activeSpellWheel()
    {
        buttonMagic.SetActive(false);
        buttonAttack.SetActive(false);
        if (currentMana >= offensiveSpellCost && currentMana >= utilitySpellCost) spellWheelAnimator.SetTrigger("activeSpellWheel_BothON");
        else if (currentMana >= offensiveSpellCost && currentMana < utilitySpellCost) spellWheelAnimator.SetTrigger("activeSpellWheel_TopON");
        else if (currentMana < offensiveSpellCost && currentMana >= utilitySpellCost) spellWheelAnimator.SetTrigger("activeSpellWheel_BottomON");
        else spellWheelAnimator.SetTrigger("activeSpellWheel_BothOFF");
    }

    public void desactiveSpellWheel()
    {
        spellWheelAnimator.SetTrigger("desactiveSpellWheel");
        buttonMagic.SetActive(true);
        buttonAttack.SetActive(true);
    }

    private void RemoveEnemyHealth(int damages)
    {
        textDamageEnemy.text = "- " + damages;
        animatorTextDamageEnemy.SetTrigger("Damage");

        sliderVieEnnemi.value -= damages;

        if (sliderVieEnnemi.value == 0)
        {
            enemyAnimator.SetTrigger("Die");
        }
    }

    private void RemovePlayerHealth(float damages)
    {
        sliderViePlayer.value -= damages;

        if (sliderViePlayer.value == 0)
        {
            animatorPlayer.SetTrigger("Die");
        }
    }

    private void RemoveMana(int mana)
    {
        currentMana -= mana;

        // On va chercher les deux dernières barre d'unité de mana colorées
        transparentChildren = new List<Transform>();
        foreach (Transform child in manaContent.transform)
        {
            Image childImage = child.GetComponent<Image>();

            if (childImage.color.a == 1.0f)
            {
                // On l'ajoute dans un tableau de deux éléments
                transparentChildren.Add(child);
                if (transparentChildren.Count > mana)
                {
                    transparentChildren.RemoveAt(0);
                }
            }
            else
            {
                // Dès qu'on rencontre un enfant avec une couleur d'unité de mana nulle, on arrete de parcourir la boucle car tous les suivants seront dans le même cas
                break;
            }
            
        }

        // On parcours les enfants qu'on a isolé et on change leur transparence pour simuler la suppression d'une unité de mana
        foreach (Transform child in transparentChildren)
        {
            Image imageComponent = child.GetComponent<Image>();
            Color newColor = imageComponent.color;
            newColor.a = 0f;
            imageComponent.color = newColor;
        }
    }

    private void AddMana()
    {
        currentMana += 1;

        // On va chercher la première unité de mana non colorée, et on la réactive en la recolorant
        foreach (Transform child in manaContent.transform)
        {
            Image childImage = child.GetComponent<Image>();

            // Vérifiez si la couleur a une transparence maximale (alpha à 1.0).
            if (childImage.color.a == 0f)
            {
                Image imageComponent = child.GetComponent<Image>();
                Color newColor = imageComponent.color;
                newColor.a = 1f;
                imageComponent.color = newColor;
                break;
            }
        }
    }

    public void FinCombat(bool win)
    {
        chrono = Time.time - chrono;
        int minute = Mathf.FloorToInt(chrono / 60);
        int second = (int)(chrono % 60);

        if (win)
        {
            chronoTextWin.text = minute + " min " + second + " s";
            victoryScreen.SetActive(true);
        }
        else
        {
            chronoTextLose.text = minute + " min " + second + " s";
            defeatScreen.SetActive(true);
        }
    }
    
    private IEnumerator PlayerTurn(bool magic, string spellType)
    {
        animatorTextDamageEnemy.ResetTrigger("Damage");
        buttonOffensiveSpell.GetComponent<Button>().enabled = false;
        buttonUtilitySpell.GetComponent<Button>().enabled = false;

        // S'il s'agit d'une attaque au corps à corps
        if (!magic)
        {
            canvasPlayer.SetActive(false);

            playerDestinationAttack = new Vector3(63.9f, 6.27f, 28f);

            // On active la course du sprite
            animatorPlayer.SetTrigger("speedRight");
            animatorPlayer.SetFloat("speed", 5f);

            // On déplace le sprite vers sa position d'attaque au corps à corps
            while (player.position != playerDestinationAttack)
            {
                player.position = Vector3.MoveTowards(player.position, playerDestinationAttack, Time.deltaTime * 15);
                yield return null;
            }

            // On désactive la course du sprite
            animatorPlayer.SetFloat("speed", 0f);
            animatorPlayer.ResetTrigger("speedRight");

            // Attaque du joueur
            animatorPlayer.SetTrigger("Attack");

            yield return new WaitForSeconds(1.2f);

            RemoveEnemyHealth(20);

            enemyAnimator.SetBool("Hurt", true);

            yield return new WaitForSeconds(0.4f);

            enemyAnimator.SetBool("Hurt", false);

            // On prépare les données pour renvoyer le joueur à sa place d'origine
            playerDestinationAttack = new Vector3(54.11f, 6.27f, 21.35f);
            animatorPlayer.SetTrigger("speedRight");
            animatorPlayer.SetFloat("speed", 5f);

            // On déplace le sprite à sa place d'origine
            while (player.position != playerDestinationAttack)
            {
                player.position = Vector3.MoveTowards(player.position, playerDestinationAttack, Time.deltaTime * 20);
                yield return null;
            }

            // On désactive la course du srpite
            animatorPlayer.SetFloat("speed", 0f);
            animatorPlayer.ResetTrigger("speedRight");

            yield return new WaitForSeconds(0.25f);

        }
        // S'il s'agit d'une attaque à distance
        else
        {
            int manaToConsume = spellType == "offensif" ? offensiveSpellCost : utilitySpellCost;
            // Animation de consommation de mana
            manaCostText.text = "-" + manaToConsume;
            manaCostAnimator.SetTrigger("mana");
            RemoveMana(manaToConsume);
            yield return new WaitForSeconds(0.39f);
            spellWheelAnimator.SetTrigger("desactiveSpellWheel");
            yield return new WaitForSeconds(0.01f);
            manaCostAnimator.ResetTrigger("mana");

            buttonMagic.SetActive(true);
            buttonAttack.SetActive(true);

            canvasPlayer.SetActive(false);

            // Lancement du sort
            if (spellType == "offensif")
            {
                if (SaveSystem.currentSave.spellSave.currentOffensiveSpell == "lightning")
                {
                    animatorPlayer.SetTrigger("Magic");
                    yield return new WaitForSeconds(1);

                    particleSystemLightning.Play();
                    yield return new WaitForSeconds(0.1f);
                    enemyAnimator.SetBool("Hurt", true);
                    yield return new WaitForSeconds(1.8f);
                    particleSystemLightning.Stop();
                    yield return new WaitForSeconds(0.2f);

                    RemoveEnemyHealth(SaveSystem.currentSave.spellSave.spellTypes["lightning"].damage);
                    yield return new WaitForSeconds(0.2f);
                    enemyAnimator.SetBool("Hurt", false);
                    yield return new WaitForSeconds(0.3f);
                }
                else if (SaveSystem.currentSave.spellSave.currentOffensiveSpell == "fire")
                {

                }
                else if (SaveSystem.currentSave.spellSave.currentOffensiveSpell == "ice")
                {

                }
            } 
            else
            {
                if (SaveSystem.currentSave.spellSave.currentUtilitySpell == "shield")
                {

                }
                else if (SaveSystem.currentSave.spellSave.currentUtilitySpell == "heal")
                {

                }
                else if (SaveSystem.currentSave.spellSave.currentUtilitySpell == "buffAttack")
                {

                }
            }
        }

        if (sliderVieEnnemi.value == 0f)
        {
            FinCombat(true);
        }
        else
        {
            StartCoroutine(EnemyTurn(magic));
        }

        yield return null;
    }

    private IEnumerator EnemyTurn(bool magic)
    {
        enemyDestinationAttack = new Vector3(56.2f, 6.22f, 18.91f);
        enemyAnimator.ResetTrigger("Hurt");

        // On active la course du sprite
        enemyAnimator.SetFloat("Speed", 5f);

        // On déplace le sprite vers sa position d'attaque au corps à corps
        while (enemy.position != enemyDestinationAttack)
        {
            enemy.position = Vector3.MoveTowards(enemy.position, enemyDestinationAttack, Time.deltaTime * 20);
            yield return null;
        }

        // On désactive la course du srpite
        enemyAnimator.SetFloat("Speed", 0f);

        // Attaque de l'ennemi
        enemyAnimator.SetTrigger("Attack");

        textDamagePlayer.text = "- 5";
        for (int i = 0; i < 3; i++)
        {
            yield return new WaitForSeconds(0.4f);
            animatorTextDamagePlayer.ResetTrigger("Damage");

            // Dégats sur le joueur
            animatorPlayer.SetTrigger("Hurt");
            RemovePlayerHealth(5f);
            animatorTextDamagePlayer.SetTrigger("Damage");
        }

        yield return new WaitForSeconds(0.1f);

        enemyDestinationAttack = new Vector3(66.56f, 6.22f, 25.91f);

        // On active la course du sprite
        enemyAnimator.SetFloat("Speed", 5f);

        // On déplace le sprite vers sa position d'attaque au corps à corps
        while (enemy.position != enemyDestinationAttack)
        {
            enemy.position = Vector3.MoveTowards(enemy.position, enemyDestinationAttack, Time.deltaTime * 15);
            yield return null;
        }

        // On désactive la course du srpite
        enemyAnimator.SetFloat("Speed", 0f);

        if (sliderViePlayer.value == 0f)
        {
            FinCombat(false);
        }
        else
        {
            // On réactive ou désactive les différents éléments d'UI en fonction du mana restant
            canvasPlayer.SetActive(true);

            // Ajout de mana si le joueur n'a pas fait de magie
            if (!magic && currentMana < maxMana)
            {
                AddMana();
                manaCostText.text = "+1";
                manaCostAnimator.SetTrigger("mana");

                yield return new WaitForSeconds(0.4f);
                manaCostAnimator.ResetTrigger("mana");
            }

            if (currentMana < offensiveSpellCost)
            {
                // On désactive le bouton de sort offensif si le joueur n'a pas assez de mana pour le lancer
                buttonOffensiveSpell.GetComponent<Button>().enabled = false;
            }
            else
            {
                // On réactive le bouton magie si le joueur a récupéré assez de mana
                buttonOffensiveSpell.GetComponent<Button>().enabled = true;
            }

            if (currentMana < utilitySpellCost)
            {
                // On désactive le bouton de sort utilitaire si le joueur n'a pas assez de mana pour le lancer
                buttonUtilitySpell.GetComponent<Button>().enabled = false;
            }
            else
            {
                // On réactive le bouton magie si le joueur a récupéré assez de mana
                buttonUtilitySpell.GetComponent<Button>().enabled = true;
            }
        }

        yield return null;
    }
}
