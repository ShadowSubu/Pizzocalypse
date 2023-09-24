using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public Dialogue playerDialogue;
    public Dialogue npcDialogue;

    private void Start()
    {
        CloseDialogues();
    }

    public void CloseDialogues()
    {
        playerDialogue.CloseDialogue();
        npcDialogue.CloseDialogue();
    }
}
