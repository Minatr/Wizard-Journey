using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GestionSpell : MonoBehaviour
{
    [Header("UI REFERENCES")]
    [SerializeField] private Image imageSelectedSpell;
    [SerializeField] private Sprite iceSprite;
    [SerializeField] private Sprite fireSprite;
    [SerializeField] private Sprite lightningSprite;
    [SerializeField] private Sprite activatedButton;
    [SerializeField] private Sprite desactivatedButton;
    // Selected Spell
    [SerializeField] private TextMeshProUGUI nameSelectedSpell;
    [SerializeField] private TextMeshProUGUI textDamageSelectedSpell;
    [SerializeField] private TextMeshProUGUI manaCostSelectedSpell;
    // Ice Spell
    [SerializeField] private TextMeshProUGUI textDamageIceSpell;
    [SerializeField] private TextMeshProUGUI manaCostIceSpell;
    [SerializeField] private GameObject iceSpellButton;
    // Fire Spell
    [SerializeField] private TextMeshProUGUI textDamageFireSpell;
    [SerializeField] private TextMeshProUGUI manaCostFireSpell;
    [SerializeField] private GameObject fireSpellButton;
    // Lightning Spell
    [SerializeField] private TextMeshProUGUI textDamageLightningSpell;
    [SerializeField] private TextMeshProUGUI manaCostLightningSpell;
    [SerializeField] private GameObject lightningSpellButton;

    private SpellSave spellData;

    public void Start()
    {
        if (SaveSystem.currentSave != null) { InitSpells(); }
    }

    public void InitSpells()
    {
        spellData = SaveSystem.currentSave.spellSave;

        changeSpellHeader(spellData.currentOffensiveSpell);

        textDamageIceSpell.text = spellData.spellTypes["ice"].damage.ToString();
        manaCostIceSpell.text = spellData.spellTypes["ice"].cost.ToString();

        textDamageFireSpell.text = spellData.spellTypes["fire"].damage.ToString();
        manaCostFireSpell.text = spellData.spellTypes["fire"].cost.ToString();

        textDamageLightningSpell.text = spellData.spellTypes["lightning"].damage.ToString();
        manaCostLightningSpell.text = spellData.spellTypes["lightning"].cost.ToString();
    }


    public void ChangeSpell(string newSpell)
    {
        changeSpellHeader(newSpell);
    }

    private void changeSpellHeader(string newSpell)
    {
        SavedData savedData = SaveSystem.currentSave;

        switch (newSpell)
        {
            case "ice":
                imageSelectedSpell.sprite = iceSprite;
                textDamageSelectedSpell.text = spellData.spellTypes["ice"].damage.ToString();
                manaCostSelectedSpell.text = spellData.spellTypes["ice"].cost.ToString();
                nameSelectedSpell.text = "Glace";
                iceSpellButton.GetComponent<Button>().enabled = false;
                iceSpellButton.GetComponent<Image>().sprite = desactivatedButton;
                fireSpellButton.GetComponent<Button>().enabled = true;
                fireSpellButton.GetComponent<Image>().sprite = activatedButton;
                lightningSpellButton.GetComponent<Button>().enabled = true;
                lightningSpellButton.GetComponent<Image>().sprite = activatedButton;

                savedData.spellSave.currentOffensiveSpell = "ice";
                SaveSystem.instance.SaveGame();
                break;
            case "fire":
                imageSelectedSpell.sprite = fireSprite;
                textDamageSelectedSpell.text = spellData.spellTypes["fire"].damage.ToString();
                manaCostSelectedSpell.text = spellData.spellTypes["fire"].cost.ToString();
                nameSelectedSpell.text = "Feu";
                iceSpellButton.GetComponent<Button>().enabled = true;
                iceSpellButton.GetComponent<Image>().sprite = activatedButton;
                fireSpellButton.GetComponent<Button>().enabled = false;
                fireSpellButton.GetComponent<Image>().sprite = desactivatedButton;
                lightningSpellButton.GetComponent<Button>().enabled = true;
                lightningSpellButton.GetComponent<Image>().sprite = activatedButton;

                savedData.spellSave.currentOffensiveSpell = "fire";
                SaveSystem.instance.SaveGame();
                break;
            case "lightning":
                imageSelectedSpell.sprite = lightningSprite;
                textDamageSelectedSpell.text = spellData.spellTypes["lightning"].damage.ToString();
                manaCostSelectedSpell.text = spellData.spellTypes["lightning"].cost.ToString();
                nameSelectedSpell.text = "Foudre";
                iceSpellButton.GetComponent<Button>().enabled = true;
                iceSpellButton.GetComponent<Image>().sprite = activatedButton;
                fireSpellButton.GetComponent<Button>().enabled = true;
                fireSpellButton.GetComponent<Image>().sprite = activatedButton;
                lightningSpellButton.GetComponent<Button>().enabled = false;
                lightningSpellButton.GetComponent<Image>().sprite = desactivatedButton;

                savedData.spellSave.currentOffensiveSpell = "lightning";
                SaveSystem.instance.SaveGame();
                break;
        }
    }
}
