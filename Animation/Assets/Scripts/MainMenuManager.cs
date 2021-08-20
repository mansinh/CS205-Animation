using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] UIGroup optionsMenu;
    [SerializeField] UIGroup mainMenu;
    [SerializeField] UIGroup quitDialog;
    [SerializeField] Material backgroundMaterial;
    [SerializeField] ScreenTransition transition;
    [SerializeField] float backgroundSpeedX = 0.1f;
    [SerializeField] float backgroundSpeedY = 0.1f;
    [SerializeField] Animator animController;
    [SerializeField] float transitionDuration = 0.3f;
    Vector2 offset = new Vector2(0,0);

    private void Start()
    {
        mainMenu.SlideIn();
    }
    //*****************************************************************************************************
    // On button presses
    //*****************************************************************************************************

    public void OnOptions()
    {
        mainMenu.SlideOut();
        optionsMenu.SlideIn();
    }

    public void OnBack() {
        mainMenu.SlideIn();
        optionsMenu.SlideOut();
    }

    public void OnStart() {
        animController.SetTrigger("cheer");
        StartCoroutine(Starting());
    }

    public void OnQuit()
    {
        animController.SetTrigger("pray");
        mainMenu.SlideOut();
        quitDialog.SlideIn();
    }

    public void QuitYes()
    {      
        animController.SetTrigger("rejected");
        StartCoroutine(Quitting());
    }

    public void QuitNo()
    {
        animController.SetTrigger("excited");
        quitDialog.SlideOut();
        mainMenu.SlideIn();
    }


    //*****************************************************************************************************
    // Transitions
    //*****************************************************************************************************
    
    // Fade to black then quit application/editor
    IEnumerator Quitting() {
        transition.FadeOut();
        yield return new WaitForSeconds(2);
        Application.Quit();
        //UnityEditor.EditorApplication.isPlaying = false;
    }

    // Fade to black then start game
    IEnumerator Starting()
    {
        transition.FadeOut();
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene("Desert");
    }

    private void Update()
    {
        // Scrolling background
        offset.x = Time.time * backgroundSpeedX;
        offset.y = Time.time * backgroundSpeedY;  
        backgroundMaterial.SetTextureOffset("_MainTex", offset);
    }
}
