using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] GameObject optionsMenu;
    [SerializeField] GameObject mainMenu;
    [SerializeField] GameObject quitDialog;
    [SerializeField] Material backgroundMaterial;
    [SerializeField] Image fadeToBlackImage;
    [SerializeField] float backgroundSpeedX = 0.1f;
    [SerializeField] float backgroundSpeedY = 0.1f;
    [SerializeField] Animator animController;
    [SerializeField] Light light;
    [SerializeField] float transitionDuration = 0.3f;
    Vector2 offset = new Vector2(0,0);

    //*****************************************************************************************************
    // On button presses
    //*****************************************************************************************************

    public void OnOptions()
    {
        StartCoroutine(SlideTo(mainMenu.transform, new Vector3(-100,0,0), transitionDuration));
        StartCoroutine(SlideTo(optionsMenu.transform, new Vector3(0, 0, 0), transitionDuration));
    }

    public void OnBack() {
        StartCoroutine(SlideTo(mainMenu.transform, new Vector3(0, 0, 0), transitionDuration));
        StartCoroutine(SlideTo(optionsMenu.transform, new Vector3(-100, 0, 0), transitionDuration));
    }

    public void OnStart() {
        animController.SetTrigger("cheer");
        StartCoroutine(Starting());
    }

    public void OnQuit()
    {
        animController.SetTrigger("pray");
        StartCoroutine(SlideTo(quitDialog.transform, new Vector3(0, -20, 0), transitionDuration));
    }

    public void QuitYes()
    {      
        animController.SetTrigger("rejected");
        StartCoroutine(Quitting());
    }

    public void QuitNo()
    {
        animController.SetTrigger("excited");
        StartCoroutine(SlideTo(quitDialog.transform, new Vector3(0, -200, 0), transitionDuration));
    }


    //*****************************************************************************************************
    // Transitions
    //*****************************************************************************************************
    
    // Fade to black then quit application/editor
    IEnumerator Quitting() {    
        StartCoroutine(FadeToBlack());
        yield return new WaitForSeconds(2);
        Application.Quit();
        UnityEditor.EditorApplication.isPlaying = false;
    }

    // Fade to black then start game
    IEnumerator Starting()
    {     
        StartCoroutine(FadeToBlack());
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene("Desert");
    }

    //*****************************************************************************************************
    // Animations
    //*****************************************************************************************************

    // Fade screen to black
    IEnumerator FadeToBlack() {
        for (float i = 0; i <= 1; i += 0.1f) {
         
            fadeToBlackImage.color = new Color(0, 0, 0, i);
            light.intensity = (1 - i)*2;
            yield return new WaitForSeconds(0.1f);
        }
        fadeToBlackImage.color = new Color(0, 0, 0, 1);
        light.intensity = 0;
    }

    // Slide a gameobject to a position
    IEnumerator SlideTo(Transform t, Vector3 endPosition, float duration)
    {
        Vector3 startPosition = t.transform.localPosition;
        for (float i = 0; i <= duration; i += Time.fixedDeltaTime)
        {
            t.localPosition = Vector3.Lerp(startPosition, endPosition, Mathf.Pow(i / duration, 3));
            yield return new WaitForFixedUpdate();
        }
    }

    private void Update()
    {
        // Scrolling background
        offset.x = Time.time * backgroundSpeedX;
        offset.y = Time.time * backgroundSpeedY;  
        backgroundMaterial.SetTextureOffset("_MainTex", offset);
    }
}
