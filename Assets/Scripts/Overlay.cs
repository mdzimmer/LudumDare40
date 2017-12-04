using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Overlay : MonoBehaviour {
    public GameObject display;
    public Text cont;
    public Text message;

    State state;
    //float heldTime = 0.0f;
    GameManager gm;

    string WELCOME_MESSAGE = "Welcome to <game title>!\nIn a future where the world has flooded a roaming flotilla of Vikings take what they want. As chieften you must spend this plunder before it fills your hulls and sinks your ship.\nLeft click to interact, Right click to move the camera.";
    string END_MESSAGE = "\n\nThe wealth of your people was too much for bouyancy to handle. Perhaps the next generation of conquerers will find your sunken wealth and laugh at the drowned.";
    //string CONTINUE_MESSAGE = "Left Click to continue...";
    float HOLD_REQUIRED = 1.0f;

	// Use this for initialization
	void Start () {
        gm = GameManager.GetManager();
        GoToWelcome();
	}
	
	// Update is called once per frame
	void Update () {
        if (state == State.WELCOME || state == State.END)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (state == State.WELCOME)
                {
                    GoToGame();
                }
                else if (state == State.END)
                {
                    Debug.Log("load end");
                    SceneManager.LoadScene("test_scene", LoadSceneMode.Single);
                }
            }
        }
	}

    public void GoToEnd()
    {
        display.SetActive(true);
        cont.gameObject.SetActive(true);
        message.gameObject.SetActive(true);
        message.text = END_MESSAGE;
        StartCoroutine(DoGoToEnd());
    }

    void GoToWelcome()
    {
        message.text = WELCOME_MESSAGE;
        state = State.WELCOME;
    }

    void GoToGame()
    {
        state = State.OFF;
        StartCoroutine(DoGoToGame());
    }

    IEnumerator DoGoToGame()
    {
        float alpha = 1.0f;
        float elapsedTime = 0.0f;
        Image displayImage = display.GetComponent<Image>();
        while (alpha > 0.0f)
        {
            elapsedTime += Time.deltaTime;
            alpha = Mathf.Clamp(1.0f - (elapsedTime / 0.5f), 0.0f, 1.0f);
            Color displayColor = displayImage.color;
            Color messageColor = message.color;
            Color continueColor = cont.color;
            displayColor.a = alpha;
            messageColor.a = alpha;
            continueColor.a = alpha;
            displayImage.color = displayColor;
            message.color = messageColor;
            cont.color = continueColor;
            yield return new WaitForEndOfFrame();
        }
        display.SetActive(false);
        cont.gameObject.SetActive(false);
        message.gameObject.SetActive(false);
        gm.BeginGame();
    }

    IEnumerator DoGoToEnd()
    {
        float alpha = 0.0f;
        float elapsedTime = 0.0f;
        Image displayImage = display.GetComponent<Image>();
        while (alpha < 1.0f)
        {
            elapsedTime += Time.deltaTime;
            alpha = Mathf.Clamp(elapsedTime / 0.5f, 0.0f, 1.0f);
            Color displayColor = displayImage.color;
            Color messageColor = message.color;
            Color continueColor = cont.color;
            displayColor.a = alpha;
            messageColor.a = alpha;
            continueColor.a = alpha;
            displayImage.color = displayColor;
            message.color = messageColor;
            cont.color = continueColor;
            yield return new WaitForEndOfFrame();
        }
        state = State.END;
    }

    enum State
    {
        WELCOME,
        OFF,
        END
    }
}
