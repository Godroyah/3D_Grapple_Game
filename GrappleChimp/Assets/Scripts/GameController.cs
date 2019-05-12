using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameController : MonoBehaviour {

    private BoxCollider winnaBox;
    public bool won = false;
    public float winnaCountDown = 30.0f;
    //public Animator levelAnim;
    public Text winText;

    // Use this for initialization
    void Start()
    {
        winnaBox = GetComponent<BoxCollider>();
        winText.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (won == true)
        {
            winnaCountDown -= 0.1f;
            winText.enabled = true;
            if (winnaCountDown <= 0.0f)
                SceneManager.LoadScene("MainMenu");
        }
    }

    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Player")
            won = true;
        winnaBox.enabled = false;
    }
}
