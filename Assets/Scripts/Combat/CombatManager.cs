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
    [SerializeField] private Animator spellWheelAnimator;
    [SerializeField] private Animator animatorTextDamagePlayer;
    [SerializeField] private TextMeshProUGUI textDamagePlayer;
    [SerializeField] private float attackDamage;
    [SerializeField] private float magicDamage;
    [SerializeField] private ParticleSystem particleSystemLightning;

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

        textEnemyLevel.text = savedData.enemyLevel.ToString();

        switch (savedData.enemyName)
        {
            case "Werewolf":
                werewolfGO.SetActive(true);
                enemyAnimator = animatorWerewolf;
                enemy = werewolf;
                textEnemyName.text = "LOUP-GAROU";
                break;

            default:
                break;
        }        

        currentMana = 3;
        maxMana = currentMana;

        // On peuple le nombre de unité de mana dans la barre en fonction du mana max du joueur
        for (int i = 0; i < maxMana; i++)
        {
            GameObject instantiatedPrefab = Instantiate(manaEnergy, Vector3.zero, Quaternion.identity);
            instantiatedPrefab.transform.SetParent(manaContent.transform, false);
        }

        chrono = Time.time;
    }

    public void PlayerAttack()
    {
        StartCoroutine(PlayerTurn(false));
    }

    public void PlayerMagic()
    {
        StartCoroutine(PlayerTurn(true));
    }

    public void activeSpellWheel()
    {
        buttonMagic.SetActive(false);
        buttonAttack.SetActive(false);
        spellWheelAnimator.SetTrigger("activeSpellWheel");
    }

    public void desactiveSpellWheel()
    {
        spellWheelAnimator.SetTrigger("desactiveSpellWheel");
        buttonMagic.SetActive(true);
        buttonAttack.SetActive(true);
    }

    private void RemoveEnemyHealth(bool magic)
    {
        if (magic)
        {
            damages = magicDamage;
        }
        else
        {
            damages = attackDamage;
        }

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

    private void FinCombat(bool win)
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
    
    private IEnumerator PlayerTurn(bool magic)
    {
        buttonAttack.GetComponent<Button>().enabled = false;
        buttonMagic.GetComponent<Button>().enabled = false;
        animatorTextDamageEnemy.ResetTrigger("Damage");


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

            RemoveEnemyHealth(magic);

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
            // Animation de consommation de mana
            manaCostText.text = "-2";
            manaCostAnimator.SetTrigger("mana");
            RemoveMana(2);
            yield return new WaitForSeconds(0.4f);
            manaCostAnimator.ResetTrigger("mana");
            
            canvasPlayer.SetActive(false);
            spellWheelAnimator.SetTrigger("desactiveSpellWheel");

            // Lancement de l'attaque
            animatorPlayer.SetTrigger("Magic");
            yield return new WaitForSeconds(1);

            particleSystemLightning.Play();
            yield return new WaitForSeconds(0.1f);
            enemyAnimator.SetBool("Hurt", true);
            yield return new WaitForSeconds(1.8f);
            particleSystemLightning.Stop();
            yield return new WaitForSeconds(0.2f);

            RemoveEnemyHealth(magic);
            yield return new WaitForSeconds(0.2f);
            enemyAnimator.SetBool("Hurt", false);
            yield return new WaitForSeconds(0.3f);
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
            // On réactive les différents éléments d'UI
            if (currentMana < 2)
            {
                // On désactive le bouton de magie si le joueur n'a pas assez de mana pour lancer un sort
                buttonMagic.GetComponent<Button>().enabled = false;
                Image imageComponent = buttonMagic.GetComponent<Image>();
                Color newColor = imageComponent.color;
                newColor.a = 0.2f;
                imageComponent.color = newColor;
            }
            canvasPlayer.SetActive(true);

            if (!magic && currentMana < maxMana)
            {
                AddMana();
                manaCostText.text = "+1";
                manaCostAnimator.SetTrigger("mana");

                if (currentMana >= 2)
                {
                    // On réactive le bouton magie si le joueur a récupéré assez de mana
                    buttonMagic.GetComponent<Button>().enabled = true;
                    Image imageComponent = buttonMagic.GetComponent<Image>();
                    Color newColor = imageComponent.color;
                    newColor.a = 1f;
                    imageComponent.color = newColor;
                }

                yield return new WaitForSeconds(0.4f);
                manaCostAnimator.ResetTrigger("mana");
            }

            buttonAttack.GetComponent<Button>().enabled = true;
            if (currentMana >= 2) buttonMagic.GetComponent<Button>().enabled = true;
        }

        yield return null;
    }
}
