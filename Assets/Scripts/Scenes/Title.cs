using UnityEngine;
using System.Collections;

public class Title : MonoBehaviour
{
    enum State
    {
        Start,
        Huh,
        OmNOm,
    }

    public Texture omnomTexture;
    public AudioClip woot;

    private float bgTop;
    private float bgLeft;
    private float bgWidth;
    private float bgHeight;

    private float btOffset;
    private float btTop;
    private float btLeft;
    private float btWidth;
    private float btHeight;

    private State state;
    private string instructions = "Welcome to the \"The Eco\" \n\n" +
        "You have to care for Herbivores (that green balls). \n" +
        "They eat plants wich look like green monkeys. \n" +
        "They are quite stupid so YOU have to care for them and \n" +
        "watch out for Carnivores. You can place pices of wall to protect your pets. \n" +
        "When Carnivore eats enough plants they lvl up! \n" +
        "So you can breed \"Super Carnivore\". \n" +
        "But this will take some time. \n\n\n" +
        "I hope you'll have !!fun!! :) \n\n\n" +
        "Click to get back";


    private float waitToLoadLevel = 1.0f;


    void Start()
    {
        state = State.Start;

        bgLeft = Screen.width / 2 - omnomTexture.width / 2;
        bgTop = Screen.height / 2 - omnomTexture.height / 2;
        bgWidth = omnomTexture.width;
        bgHeight = omnomTexture.height;

        btOffset = 10;
        btTop = bgTop + btOffset;
        btLeft = bgLeft + btOffset;
        btWidth = (bgWidth - btOffset) / 2 - btOffset;
        btHeight = 40.0f;
    }

    void OnGUI()
    {
        GUI.DrawTexture(new Rect(bgLeft, bgTop, bgWidth, bgHeight), omnomTexture);

        switch (state)
        {
            case State.Start:
                StartState();
                break;
            case State.Huh:
                HuhState();
                break;
            case State.OmNOm:
                break;
        }
    }

    private void StartState()
    {
        if (GUI.Button(new Rect(btLeft, btTop, btWidth, btHeight), "Om Nom Nom Nom"))
        {
            StartCoroutine(StartLevel());
        }
        if (GUI.Button(new Rect(btLeft + btWidth + btOffset, btTop, btWidth, btHeight), "Huh ?"))
        {
            audio.clip = woot;
            audio.Play();
            state = State.Huh;
        }
    }

    private IEnumerator StartLevel()
    {
        audio.clip = woot;
        audio.Play();
        yield return new WaitForSeconds(waitToLoadLevel);
        Application.LoadLevel("Level1");
    }

    private void HuhState()
    {
        if (GUI.Button(new Rect(bgLeft + btOffset, bgTop + btOffset, bgWidth - btOffset * 2, bgHeight - btOffset * 2), instructions))
        {
            state = State.Start;
        }
    }
}
