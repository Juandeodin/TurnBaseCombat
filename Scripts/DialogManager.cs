using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour
{

    public Queue<string> sentences; // Queuque es una pila
    public Text nameText;
    public Text dialogueText;
    public Image icono;
    public Animator animator;
    public AudioSource audio;
    public float textSpeed = 0.02f;
    private float aleatorio = Random.value;
   
    // Start is called before the first frame update
    void Start()
    {
        sentences = new Queue<string>();
        
    }


    public void StartDialogue(Dialogo dialogue)
    {
        animator.SetBool("IsOpen", true);

        Debug.Log("Starting conversation with " + dialogue.name);
        nameText.text = dialogue.name;
        icono.sprite = dialogue.icon;


        sentences.Clear();

        foreach(string sentence in dialogue.sentences)
        {

            sentences.Enqueue(sentence);


        }
        DisplayNextSentence();
            
    }
    public void DisplayNextSentence()
    {
        if(sentences.Count == 0)
        {

            EndDialogue();
            return;
        }
        string sentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
        Debug.Log(sentence);

    }
    IEnumerator TypeSentence (string sentence)
    {
      
        dialogueText.text = "";
        foreach(char letter in sentence.ToCharArray())
        {
            aleatorio = Random.Range(0.7f, 0.9f);
            dialogueText.text += letter;
            audio.pitch = aleatorio;
            audio.Play();
            yield return new WaitForSeconds(textSpeed);
            

        }


    }
    void EndDialogue()
    {

        Debug.Log("Termino la conversación");
        animator.SetBool("IsOpen", false);

    }

    
}
