using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject blackObject;
    [SerializeField] private Image blackScreen;
    void Start()
    {
        blackObject.SetActive(false);
    }
    void Update()
    {

    }

    public void newGame()
    {

        StartCoroutine(loadingGame());

    }
    private IEnumerator loadingGame()
    {
        blackObject.SetActive(true);
        float Duration = 3f;
        float elapsedTime = 0f;

        while (elapsedTime < Duration)
        {
            float elapsedPercent = elapsedTime / Duration;
            blackScreen.color = Color.Lerp(Color.clear, Color.black, elapsedPercent);

            yield return null;
            elapsedTime += Time.deltaTime;
        }

        blackScreen.color = Color.black;
        yield return new WaitForSeconds(2f);
        UnityEngine.SceneManagement.SceneManager.LoadScene("MarvinScene");
    }
}


