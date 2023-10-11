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
    [SerializeField] private SaveSystem saveSystem;
    [SerializeField] private Transform player;

    private bool isLaunching = false;

    public void LoadCombat(string enemyName, int enemyLevel)
    {
        if (!isLaunching)
        {
            saveSystem.SaveData(player.position, enemyName, enemyLevel);
            StartCoroutine(LoadAsynchronouslyCombat());
            isLaunching = true;
        }
    }

    IEnumerator LoadAsynchronouslyCombat()
    {
        cameraJoueurAnimator.SetTrigger("StartCombatTransition");

        yield return new WaitForSeconds(1f);

        loadingScreen.SetActive(true);

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
    
    public void LoadZone()
    {
        string zoneName = "Zone1";
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
