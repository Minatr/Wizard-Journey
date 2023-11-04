using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelLoader : MonoBehaviour
{
    [Header("REFERENCES")]
    [SerializeField] private Animator cameraJoueurAnimator;
    [SerializeField] private Animator loadingScreenAnimator;
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private Slider slider;
    [SerializeField] private TextMeshProUGUI progressText;

    [Header("SAVE REFERENCES")]
    [SerializeField] private Transform player;

    private bool isLaunching = false;

    public void LoadCombat(EnemyAI enemyAI)
    {
        if (!isLaunching)
        {
            enemyAI.attacking = true;
            SaveSystem.instance.SaveData(SceneManager.GetActiveScene().name, player.position, enemyAI.enemyName, enemyAI.enemyLevel, SaveSystem.instance.getAllEnemies());
            StartCoroutine(LoadAsynchronouslyCombat());
            isLaunching = true;
        }
    }

    IEnumerator LoadAsynchronouslyCombat()
    {
        loadingScreen.SetActive(true);

        cameraJoueurAnimator.SetTrigger("StartCombatTransition");

        yield return new WaitForSeconds(1f);

        loadingScreenAnimator.SetTrigger("CombatTransition");

        yield return new WaitForSeconds(1);

        AsyncOperation operation = SceneManager.LoadSceneAsync("Combat");
        
        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / .9f);

            slider.value = progress;
            progressText.text = progress * 100f + " %";

            yield return null;
        }
    }
    
    public void LoadZone(string zoneName)
    {
        SaveSystem.instance.SaveGame();
        if (zoneName == "")
        {
            // Appel de la fonction à la fin d'un combat
            zoneName = SaveSystem.currentSave.previousScene;
            SaveSystem.currentSave.previousScene = "Combat";
        }
        else
        {
            // Appel de la fonction depuis une zone
            SaveSystem.currentSave.previousScene = SaveSystem.currentSave.actualScene;
        }
        StartCoroutine(LoadAsynchronouslyZone(zoneName));
    }

    IEnumerator LoadAsynchronouslyZone(string zoneName)
    {
        loadingScreen.SetActive(true);

        AsyncOperation operation = SceneManager.LoadSceneAsync(zoneName);

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / .9f);

            slider.value = progress;

            yield return null;
        }
    }
}
