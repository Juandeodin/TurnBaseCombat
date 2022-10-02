using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using CodeMonkey.Utils;

public class PersonajeBatalla : MonoBehaviour
{
    public Animator animator;
    public float speed = 2f;
    public float slideSpeed = 10f;
    private float distancia = 0.2f;
    public float tiempoAtacar = 0.5f;
    public Vector3 posiciónInicial;
    private int a = 1;
    private int b = 1;


    private bool atacando = false;
    private Vector2 movement;
    private State state;
    private Vector3 slideTargetPosition;
    private Action onSlideComplete;
    private Action onAtackComplete;
    private Action animacionComplete;
    private GameObject selectionCircle;
    private HealthSystem healthSystem;
    [SerializeField] private HealthBar vida;
    private SpriteRenderer renderer;


    private enum State
    {
        Idle, Sliding, Busy

    }
    private void Awake()
    {
        state = State.Idle;
        selectionCircle = transform.Find("SelectionCircle").gameObject;
        renderer = GetComponent<SpriteRenderer>();



    }

    private void Start()
    {

    }

    public void Setup(bool isPlayerTeam) // le dice de que equipo son dandoles una animación a cada personaje
    {
        if (isPlayerTeam)
        {
            animator.SetBool("Iddle", true);



        }
        else
        {

            float localScalex = transform.localScale.x; //flip
            localScalex = localScalex * -1f;
            transform.localScale = new UnityEngine.Vector3(localScalex, transform.localScale.y, transform.localScale.z);


            animator.SetBool("Iddle", true);

        }
        healthSystem = new HealthSystem(100); //HACER QUE LA VIDA SEA DIFERENTE SEGÚN EL PERSONAJE
        healthSystem.OnHealthChanged += CambioVida;


    }
    public Vector3 GetPosition() //da la posición del personaje
    {

        return transform.position;

    }









    //}
    public void Attack(PersonajeBatalla targetCharacterBattle, Action onAttackComplete) //ATAQUE DEL PERSONAJE,HABRÍA QUE IMPLEMENTAR UNA MANERA DE METERLE MOVIMIENTOS DIFERENTES
    {

        Vector3 slideTargetPosition = targetCharacterBattle.GetPosition() + (GetPosition() - targetCharacterBattle.GetPosition()).normalized * 0.00005f;


        animator.SetBool("Iddle", false);
        // Slide to Target
        SlideToPosition(slideTargetPosition, () =>
        {
            // Arrived at Target, attack him

            state = State.Busy;
            Debug.Log("Me muevo");



            AnimacionAtaque(() =>
            {
                int DañoMovimiento = UnityEngine.Random.Range(20, 50);
                targetCharacterBattle.Damage(this, DañoMovimiento);
                SlideToPosition(posiciónInicial, () =>
                {
                    Debug.Log("Me muevo a la posición inicial");
                    // Slide back completed, back to idle
                    state = State.Idle;
                    animator.SetBool("Iddle", true);
                    onAttackComplete();
                });


            });


        });

    }
    private void Update()
    {

        if (a == b)
        {
            posiciónInicial = this.GetPosition();
            a = 2;

        }

        switch (state)
        {
            case State.Idle:
                break;
            case State.Busy:
                break;
            case State.Sliding:

                transform.position += (slideTargetPosition - GetPosition()) * slideSpeed * Time.deltaTime;



                if (Vector3.Distance(GetPosition(), slideTargetPosition) < distancia)
                {

                    onSlideComplete();
                }






                break;


        }

    }
    private void LateUpdate()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsTag("Ataque"))
        {
            atacando = true;
            Debug.Log("Atacando");
        }
        else if (atacando == true)
        {

            atacando = false;
            animacionComplete();


        }
    }


    private void SlideToPosition(Vector3 slideTargetPosition, Action onSlideComplete)
    {
        this.slideTargetPosition = slideTargetPosition;
        this.onSlideComplete = onSlideComplete;
        state = State.Sliding;
        if (slideTargetPosition.x > 0)
        {

        }
        else
        {

        }
    }

    private void SlideToPosition(Vector3 slideTargetPosition)
    {
        this.slideTargetPosition = slideTargetPosition;
        this.onSlideComplete = onSlideComplete;
        state = State.Sliding;
        if (slideTargetPosition.x > 0)
        {

        }
        else
        {

        }
    }

    public void Damage(PersonajeBatalla atacante, int damageAmount)
    {

        healthSystem.Damage(damageAmount);
        Vector3 direcciónAtaque = ((GetPosition() - atacante.GetPosition()).normalized);

        DamagePopup.Create(GetPosition(), damageAmount, false);
        StartCoroutine("VisualFeedback");




        if (healthSystem.IsDead())
        {

            animator.SetTrigger("Death");

        }


        Debug.Log("Vida: " + healthSystem.GetHealthAmount());
    }
    private void CambioVida(object sender, EventArgs e)
    {
        vida.SetSize(healthSystem.GetHealthPercent());


    }


    public bool isDead()
    {

        return healthSystem.IsDead();

    }

    private void AnimacionAtaque(Action animacionComplete)
    { // aqui habrá que mandarle Cambiar la animación según el ataque
        this.animacionComplete = animacionComplete;
        animator.SetTrigger("Atack");




    }
    public void HideSelectionCircle()
    {

        selectionCircle.SetActive(false);

    }
    public void ShowSelectionCircle()
    {

        selectionCircle.SetActive(true);

    }
    private IEnumerator VisualFeedback()
    {
        Color colorActual;
        colorActual = renderer.color;

        renderer.color = Color.red;

        yield return new WaitForSeconds(0.1f);

        renderer.color = colorActual;
    }
}
