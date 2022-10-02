using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogTrigger : MonoBehaviour
{
    // Start is called before the first frame update
    public Dialogo dialogue;
    public void TriggerDialogue()
    {

        FindObjectOfType<DialogManager>().StartDialogue(dialogue);

    }


}
