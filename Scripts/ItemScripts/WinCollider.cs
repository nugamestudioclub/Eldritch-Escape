using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Animations;

public class WinCollider : MonoBehaviour
{
    public GameObject enemy;
    public GameObject enemyTP;
    public Camera cam;
    public Animator endgameAnim;
    public Button retry;
    public Button exit;

    // Start is called before the first frame update
    void Start()
    {
        retry.onClick.AddListener(Restart);
        exit.onClick.AddListener(LoadOut);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        cam.transform.parent = null;
        PlayerMovement player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        player.DisableInput();
        
        player.GetComponent<Rigidbody2D>().velocity = Vector2.down * 3f;
        enemy.transform.position = enemyTP.transform.position;
        StartCoroutine(WaitForLoadOut());
    }

    private IEnumerator WaitForLoadOut()
    {
        yield return new WaitForSeconds(2f);
        endgameAnim.Play("End");

        

        //SceneManager.LoadScene(0);
    }

    private void LoadOut()
    {
        SceneManager.LoadScene(0);
    }
    private void Restart()
    {
        SceneManager.LoadScene(1);
    }
}
