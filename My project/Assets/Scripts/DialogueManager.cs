using Ink.Runtime;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System.Collections;
using Ink.UnityIntegration;
using UnityEngine.Events;

/// <summary>
/// resources: https://www.youtube.com/watch?v=vY0Sk93YUhA&list=PL3viUl9h9k78KsDxXoAzgQ1yRjhm7p8kl&index=3
/// </summary>

public class DialogueManager : MonoBehaviour
{
    public UnityEvent<Story> OnExitDialogue;

    [Header("Globals Ink File")]
    [SerializeField] [Tooltip("Reference to the Ink file containing global variables")] private InkFile _globalsInkFile;

    [Header("Dialogue UI")]
    [SerializeField] private GameObject _dialoguePanel;
    [SerializeField] private TMP_Text _dialogueText;
    [SerializeField] private GameObject _speakerPanel;
    [SerializeField] private TMP_Text _speakerNameText;

    [Header("Choices UI")]
    [SerializeField] private GameObject[] choices;
    private TMP_Text[] choicesText;

    private Story _currentStory;
    public bool _dialogueIsPlaying { get; private set; }
    bool _canContinueToNextLine = true;

    private DialogueVariableObserver _dialogueVariables;

    // Tags -------------------------------------
    private const string SPEAKER_TAG = "characterName";
    // ------------------------------------------

    // Declaring Singleton ----------------------
    private static DialogueManager instance;
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }

        _dialogueVariables = new DialogueVariableObserver(_globalsInkFile.filePath);
    }
    public static DialogueManager GetInstance()
    {
        return instance;
    }
    // ------------------------------------------

    private void Start()
    {
        _dialogueIsPlaying = false;
        _dialoguePanel.SetActive(false);

        choicesText = new TMP_Text[choices.Length];
        int index = 0;
        foreach (GameObject choice in choices) // populate choicesText array with the text components of the choice buttons
        {
            choicesText[index] = choice.GetComponentInChildren<TMP_Text>();
            index++;
        }
    }

    private void Update()
    {
        if (!_dialogueIsPlaying) return; // don't update if there is no dialogue playing

        // check if the input is pressed (could not be assed to figure this out with input system rn)

        if (_canContinueToNextLine)
        {
            if (Keyboard.current.spaceKey.wasPressedThisFrame || Mouse.current.leftButton.wasPressedThisFrame)
            {
                ContinueStory();
            }
        }
    }

    public void EnterDialogueMode(TextAsset inkJSON)
    {
        _currentStory = new Story(inkJSON.text);
        _dialogueIsPlaying = true;
        _dialoguePanel.SetActive(true);

        _dialogueVariables.StartListening(_currentStory);

        _speakerNameText.text = "???";
        _speakerPanel.SetActive(false);

        ContinueStory();
    }

    private void ExitDialogueMode()
    {
        _dialogueVariables.StopListening(_currentStory);

        _dialogueIsPlaying = false;
        _dialoguePanel.SetActive(false);
        _dialogueText.text = "";

        OnExitDialogue?.Invoke(_currentStory);
    }

    private void ContinueStory()
    {
        if (_currentStory.canContinue)
        {
            _dialogueText.text = _currentStory.Continue();
            DisplayChoices();
            HandleTags(_currentStory.currentTags);
        }
        else
        {
            ExitDialogueMode();
        }
    }
    private void HandleTags(List<string> currentTags)
    {
        foreach (string tag in currentTags)
        {
            string[] splitTag = tag.Split(':');
            if (splitTag.Length != 2)
            {
                Debug.LogWarning("Tag could not be parsed: " + tag);
                continue;
            }
            string tagKey = splitTag[0].Trim();
            string tagValue = splitTag[1].Trim();
            switch (tagKey)
            {
                case SPEAKER_TAG:
                    // handle speaker name change (e.g. update name text above dialogue panel)
                    Debug.Log("Speaker tag found with value: " + tagValue);
                    
                    if (tagValue.ToLower() == "narrator")
                    {
                        _speakerPanel.SetActive(false);
                        break;
                    }

                    _speakerPanel.SetActive(true);
                    _speakerNameText.text = tagValue;
                    break;
                default:
                    Debug.LogWarning("Tag key not recognized: " + tagKey);
                    break;
            }
        }
    }

    private void HandleVariableChanges()
    {

    }

    private void DisplayChoices()
    {
        List<Choice> currentChoices = _currentStory.currentChoices;

        if (currentChoices.Count > choices.Length)
        {
            Debug.LogError("More choices were given than the UI can support. Number of choices given: " + currentChoices.Count);
        }

        if (currentChoices.Count > 0)
        {
            _canContinueToNextLine = false;
        }
        else
        {
            _canContinueToNextLine = true;
        }

            // loop through choice game objects and display according to the choices in the current story
            int index = 0;
        // enable and initialize the choices up to the amount of choices in the current story
        foreach (Choice choice in currentChoices)
        {
            choices[index].gameObject.SetActive(true);
            choicesText[index].text = choice.text;
            index++;
        }
        // disable the remaining choice game objects (if not needed)    
        for (int i = index; i < choices.Length; i++)
        {
            choices[i].gameObject.SetActive(false);
        }

        //StartCoroutine(SelectFirstChoice());
    }

    private IEnumerator SelectFirstChoice()
    {
        EventSystem.current.SetSelectedGameObject(null);
        yield return new WaitForEndOfFrame();
        EventSystem.current.SetSelectedGameObject(choices[0].gameObject);
    }

    public void MakeChoice(int choiceIndex)
    {
        _currentStory.ChooseChoiceIndex(choiceIndex);
        ContinueStory();
    }

    public Ink.Runtime.Object GetVariableState(string variableName)
    {
        Ink.Runtime.Object variableValue = null;
        _dialogueVariables.variables.TryGetValue(variableName, out variableValue);
        if (variableValue == null)
        {
            Debug.LogWarning("Variable was not found: " + variableName);
        }
        return variableValue;
    }
}
