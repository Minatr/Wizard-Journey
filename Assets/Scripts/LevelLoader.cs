using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelLoader : MonoBehaviour
{
    [Header("REFERENCES")]

    [SerializeField]
    private Animator cameraJoueurAnimator;

    [SerializeField]
    private Animator loadingScreenAnimator;

    [SerializeField]
    private GameObject loadingScreen;

    [SerializeField]
    private Slider slider;

    [SerializeField]
    private TextMeshProUGUI progressText;


    public void LoadCombat()
    {
        StartCoroutine(LoadAsynchronouslyCombat());
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
}
