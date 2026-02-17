using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class DialoguePanelBehavior : MonoBehaviour
{
    // ----------------Setup---------------------
    public static DialoguePanelBehavior Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
    }
    // ------------------------------------------

    [SerializeField] TMP_Text dialogueTextBox;
    [SerializeField] TMP_Text dialogueNameLabel;
    [SerializeField] Image characterImage;
    public void UpdateDialoguePanel(string dialogueText, string characterName, Sprite characterSprite)
    {
        dialogueNameLabel.text = characterName;
        dialogueTextBox.text = dialogueText;
        characterImage.sprite = characterSprite;
    }
}
