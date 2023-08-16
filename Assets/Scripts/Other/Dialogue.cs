using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Dialogue : MonoBehaviour
{
    public TextMeshProUGUI dialogueText;
    public Image speakerImage;

    public void OpenDialogue()
    {
        dialogueText.gameObject.SetActive(true);
        speakerImage.gameObject.SetActive(true);
    }

    public void CloseDialogue()
    {
        dialogueText.gameObject.SetActive(false);
        speakerImage.gameObject.SetActive(false);
    }
}
