using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{

    public Button start;
    public Button exit;
    public Button instructions;
    public Button returnToHomeBtn;
    
    public Image instructionsImg;
    
    // Start is called before the first frame update
    void Start()
    {
        start.onClick.AddListener(StartClicked);
        exit.onClick.AddListener(ExitClicked);
        this.instructions.onClick.AddListener(ShowInstructions);
        instructionsImg.gameObject.SetActive(false);
        returnToHomeBtn.onClick.AddListener(HideInstructions);
    }
    void ShowInstructions()
    {
        instructionsImg.gameObject.SetActive(true);

    }
    void HideInstructions()
    {
        instructionsImg.gameObject.SetActive(false);
    }
    void StartClicked()
    {
        SceneManager.LoadScene(1);
    }
    void ExitClicked()
    {
        Application.Quit();
    }

    // Update is called once per frame
    void Update()
    {
       
    }
}
