using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartGame : MonoBehaviour
{
    [Header("REFERENCES")]
    [SerializeField] private TextMeshProUGUI touchToStart;

    [Header("ZONE TRANSITION REFERENCES")]
    [SerializeField] private GameObject sliderGO;
    [SerializeField] private Slider slider;
    [SerializeField] private TextMeshProUGUI sliderText;

    [Header("COMBAT TRANSITION REFERENCES")]
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private Animator loadingScreenAnimator;
    [SerializeField] private Slider sliderCombat;
    [SerializeField] private TextMeshProUGUI progressText;

    private SavedData savedData;
    
    private bool launched = false;

    private string sceneToLoad;

    private void Start()
    {
        savedData = SaveSystem.instance.LoadData();
        if (savedData != null)
        {
            sceneToLoad = savedData.actualScene;
        }
        else
        {
            sceneToLoad = "Zone1";
        }
    }

    void Update()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
        {
            if (!launched)
            {
                if (sceneToLoad == "Combat")
                {
                    StartCoroutine(LoadCombat());
                }
                else
                {
                    StartCoroutine(LoadAsynchronouslyZone(sceneToLoad));
                }
                launched = true;
            }
        }
    }

    IEnumerator LoadAsynchronouslyZone(string zoneNumber)
    {
        touchToStart.enabled = false;
        sliderGO.SetActive(true);

        AsyncOperation operation = SceneManager.LoadSceneAsync(zoneNumber);

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / .9f);

            slider.value = progress;
            sliderText.text = progress * 100f + " %";

            yield return null;
        }
    }

    IEnumerator LoadCombat()
    {
        loadingScreen.SetActive(true);

        loadingScreenAnimator.SetTrigger("CombatTransition");

        yield return new WaitForSeconds(1);

        AsyncOperation operation = SceneManager.LoadSceneAsync("Combat");

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / .9f);

            sliderCombat.value = progress;
            progressText.text = progress * 100f + " %";

            yield return null;
        }
    }
}
