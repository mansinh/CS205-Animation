using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUD : MonoBehaviour
{
    [SerializeField] ScreenTransition transition;
    [SerializeField] UIGroup quitDialog;
    [SerializeField] float transitionDuration = 0.3f;
    bool isPaused = false; 

    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !isPaused) {
           
            OnQuit();
        }
    }

    public void OnQuit()
    {
        isPaused = true;
        Time.timeScale = 0;
        quitDialog.SlideIn();
    }

    public void QuitYes()
    {
        StartCoroutine(Quitting());
        isPaused = false;
        Time.timeScale = 1;
    }

    public void QuitNo()
    {

        quitDialog.SlideOut();
        isPaused = false;
        Time.timeScale = 1;
    }

    //*****************************************************************************************************
    // Transitions
    //*****************************************************************************************************

    // Fade to black then quit application/editor
    IEnumerator Quitting()
    {
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
        
    }

}
