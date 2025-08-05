using System.Collections;
using System.Collections.Generic;
using Mini_Game.Simon.Script;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SimonManager : Assembler
{
    [System.Serializable]
    public class SimonButton
    {
        public Button button;
        public Color buttonColor;
        public AudioClip sound;

        [Range(.5f, 5f)]
        public float weight = 1f;
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
    [SerializeField] private List<SimonRoundDisplay> roundDisplays = new();
    public TMP_Text targetText;
    public TMP_Text currentText;
    [SerializeField] private TextMeshProUGUI stateText;
    public string memorize;
    public string play;

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
        stateText.text = memorize;

        if (!endless)
        {
            //targetText.text = "Target: " + targetScore;

            foreach (var roundDisplay in roundDisplays)
            {
                roundDisplay.SetValid(false);
            }
        }
        else
            targetText.text = "";

        currentText.text = "Current: " + currentScore;

        yield return new WaitForSeconds(1f);
        yield return AddAndShowSequence();
    }

    IEnumerator AddAndShowSequence()
    {
        stateText.text = memorize;

        int newIndex = GetWeightedRandomButtonIndex();
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
        stateText.text = play;
    }

    private int GetWeightedRandomButtonIndex()
    {
        float totalWeight = 0f;
        foreach (var btn in simonButtons)
        {
            totalWeight += btn.weight;
        }

        float randomValue = Random.Range(0f, totalWeight);
        float cumulativeWeight = 0f;

        for (int i = 0; i < simonButtons.Length; i++)
        {
            cumulativeWeight += simonButtons[i].weight;
            if (randomValue <= cumulativeWeight)
            {
                return i;
            }
        }

        return simonButtons.Length - 1; // fallback
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

            OnAssembleurActivityEnd?.Invoke(false, this);
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
        
        ActivateValidation(sequence.Count - 1);
        
        yield return new WaitForSeconds(delayAfterSequence);

        if (!endless && sequence.Count >= targetScore)
        {
            Debug.Log("End MiniGame");

            UnActivate();
            OnAssembleurActivityEnd?.Invoke(true, this);
        }
        else
        {
            
            yield return AddAndShowSequence();
        }
    }
    
    private void ActivateValidation(int index)
    {
        if (index < 0 || index >= roundDisplays.Count)
        {
            Debug.LogError("Index out of range for roundDisplays.");
            return;
        }

        roundDisplays[index].SetValid(true);
    }

    public override void Activate()
    {
        StartCoroutine(Activation());
    }

    private IEnumerator Activation()
    {
        inputEnabled = false;
        canvas.SetActive(true);
        this.StopAllCoroutines();
        ResetButtons();
        StartCoroutine(StartNewGame());
        yield return null;
    }

    public override void UnActivate()
    {
        canvas.SetActive(false);
        this.StopAllCoroutines();
        ResetButtons();
    }
}
