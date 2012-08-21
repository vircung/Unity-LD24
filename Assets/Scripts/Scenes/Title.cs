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

    float bgTop;
    float bgLeft;
    float bgWidth;
    float bgHeight;

    float btOffset;
    float btTop;
    float btLeft;
    float btWidth;
    float btHeight;

    State state;

    // TODO : FFS UPDATE IT !!!!!!11!!1!!11
    string build = "Build 0.0.3";
    string instructions =
        "Welcome to the \"NomOm\" \n\n" +

       "You have to care for Herbivores (that green balls). \n" +
       "They eat plants wich look like green monkeys. \n" +
       "They are quite stupid so YOU have to care for them and \n" +
       "watch out for Carnivores. You can place pices of wall to protect your pets. \n" +
       "When Carnivore eats enough plants they lvl up! \n" +
       "So you can breed \"Super Carnivore\". \n" +
       "But this will take some time. \n\n\n" +

       "You can move camera with WSAD or arrows. \n " + 
       "Zoom in and zoom out with mouse wheel.\n" +
       "Place walls by clicking on the ground. \n" +
       "Remove walls by clicking on them.\n\n\n" +

       "There is no ending goal \n\n\n" +

       "I hope you'll have !!fun!! :) \n\n\n" +

       "Click to get back";

    float waitToLoadLevel = 1.0f;
    float volume = 0.1f;

    void Start()
    {
        state = State.Start;
        woot = Resources.Load("Sounds/woot") as AudioClip;
        omnomTexture = Resources.Load("Textures/omnom") as Texture;

        audio.volume = volume;
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

        GUI.Label(new Rect(btLeft, bgTop + bgHeight - btHeight, btWidth, btHeight), build);
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
