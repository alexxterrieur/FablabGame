using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SimonManager : Assembler
{
    [System.Serializable]
    public class SimonButton
    {
        public Button button;
        public Color buttonColor;
        public AudioClip sound;
    }

    [Header("Simon Buttons")]
    public SimonButton[] simonButtons;

    [Header("Couleurs")]
    public Color neutralColor = Color.gray;
    public Color wrongColor = Color.red;

    [Header("Timing")]
    public float activeTime = 0.4f;
    public float pauseTime = 0.2f;
    public float delayAfterSequence = 1.5f;

    [Header("GameMode")]
    public bool endless = false;
    public int targetScore = 5;

    [Header("Acceleration")]
    public float accelerationPerStep = 0.1f;
    public float minSpeedMultiplier = 0.3f;

    [Header("Sequence")]
    private List<int> sequence = new List<int>();
    private int currentScore = 0;
    private bool inputEnabled = false;

    [Header("Audio")]
    public AudioSource audioSource;

    [Header("UI")]
    public TMP_Text targetText;
    public TMP_Text currentText;

    [Header("Canvas")]
    [SerializeField] GameObject canvas;

    void ResetButtons()
    {
        foreach (var btn in simonButtons)
        {
            btn.button.GetComponent<Image>().color = neutralColor * btn.buttonColor;
        }
    }

    IEnumerator StartNewGame()
    {
        Debug.Log("Nouvelle partie");
        sequence.Clear();
        currentScore = 0;

        if (!endless)
            targetText.text = "Target: " + targetScore;
        else
            targetText.text = "";

        currentText.text = "Current: " + currentScore;


        yield return new WaitForSeconds(1f);
        yield return AddAndShowSequence();
    }

    IEnumerator AddAndShowSequence()
    {
        int newIndex = Random.Range(0, simonButtons.Length);
        sequence.Add(newIndex);
        Debug.Log("Nouvelle séquence longueur: " + sequence.Count);

        // Calcul de la vitesse
        int speedStep = sequence.Count / 5;
        float speedMultiplier = Mathf.Max(1.0f - accelerationPerStep * speedStep, minSpeedMultiplier);
        Debug.Log($"Speed multiplier: {speedMultiplier}");

        inputEnabled = false;
        ResetButtons();
        yield return new WaitForSeconds(0.5f);

        foreach (int index in sequence)
        {
            simonButtons[index].button.GetComponent<Image>().color = simonButtons[index].buttonColor;

            if (audioSource && simonButtons[index].sound)
                audioSource.PlayOneShot(simonButtons[index].sound);

            yield return new WaitForSeconds(activeTime * speedMultiplier);

            simonButtons[index].button.GetComponent<Image>().color = neutralColor * simonButtons[index].buttonColor;
            yield return new WaitForSeconds(pauseTime * speedMultiplier);
        }

        currentScore = 0;
        inputEnabled = true;
    }

    public void OnButtonPressed(int buttonIndex)
    {
        if (!inputEnabled)
            return;

        if (audioSource && simonButtons[buttonIndex].sound)
            audioSource.PlayOneShot(simonButtons[buttonIndex].sound);

        if (buttonIndex == sequence[currentScore])
        {
            currentScore++;

            if (currentScore >= sequence.Count)
            {
                inputEnabled = false;
                StartCoroutine(HandleEndOfSequence(buttonIndex));
            }
            else
            {
                StartCoroutine(FlashButton(buttonIndex, simonButtons[buttonIndex].buttonColor));
            }

            currentText.text = "Current: " + currentScore;
        }
        else
        {
            StartCoroutine(FlashButton(buttonIndex, wrongColor));

            OnAssembleurActivityEnd?.Invoke(false);
            UnActivate();

            if (!endless && sequence.Count >= targetScore)
            {
                Debug.Log("Victory");
            }
            else
            {
                Debug.Log("Fail");
                return;
            }

            StartCoroutine(StartNewGame());
        }
    }

    IEnumerator FlashButton(int index, Color color)
    {
        simonButtons[index].button.GetComponent<Image>().color = color;
        yield return new WaitForSeconds(activeTime);
        simonButtons[index].button.GetComponent<Image>().color = neutralColor * simonButtons[index].buttonColor;
    }

    IEnumerator HandleEndOfSequence(int buttonIndex)
    {
        yield return FlashButton(buttonIndex, simonButtons[buttonIndex].buttonColor);
        yield return new WaitForSeconds(delayAfterSequence);

        if (!endless && sequence.Count >= targetScore)
        {
            Debug.Log("End MiniGame");

            OnAssembleurActivityEnd?.Invoke(true);
            UnActivate();
            //PlayerPrefs.SetInt("Level2Completed", 1);
            //PlayerPrefs.Save();
            //SceneManager.LoadScene("MainMenu");
        }
        else
        {
            yield return AddAndShowSequence();
        }
    }

    public override void Activate()
    {
        StartCoroutine(Activation());
    }

    private IEnumerator Activation()
    {
        yield return new WaitForSeconds(2f);

        canvas.SetActive(true);
        this.StopAllCoroutines();
        ResetButtons();
        StartCoroutine(StartNewGame());
    }

    public override void UnActivate()
    {
        canvas.SetActive(false);
        this.StopAllCoroutines();
        ResetButtons();
    }
}