using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AyudaBatalla : MonoBehaviour
{

    public static AyudaBatalla instance;
    private PersonajeBatalla jugador;
    private PersonajeBatalla enemigo;
    private PersonajeBatalla activo;
 
    private State state;
    
    private enum State
    {
        WaitingForPlayer,Busy,

    }
    public static AyudaBatalla GetInstance()
    {

        return instance;

    }
    [SerializeField] private Transform pfCharacterBattle;
  
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {

        jugador= SpawnCharacter(true);
        enemigo= SpawnCharacter(false);
        SetActive(jugador);
        state = State.WaitingForPlayer;

    }
    private void Update()
    {
        
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (state == State.WaitingForPlayer)
            {
                state = State.Busy;

                jugador.Attack(enemigo, () => {
                    ElegirSiguienteActivo();
                });
            }
             

            }
    
    }

    private PersonajeBatalla SpawnCharacter(bool isPlayerTeam)
    {
        Vector3 position;
        if (isPlayerTeam)
        {
            position = new Vector3(-1f, 0);
        }
        else
        {
            position = new Vector3(+1f, 0);
        }
         Transform characterTransform = Instantiate(pfCharacterBattle, position, Quaternion.identity);
        PersonajeBatalla personaje = characterTransform.GetComponent<PersonajeBatalla>();
        personaje.Setup(isPlayerTeam);
        return personaje;

    }
    private void SetActive (PersonajeBatalla personaje)
    {
        if(activo != null)
        {

            activo.HideSelectionCircle();

        }

        activo = personaje;
        activo.ShowSelectionCircle();


    }

    private void ElegirSiguienteActivo()
    {
        if (BattleOver())
        {

            return;

        }
        if (activo == jugador)
        {

            SetActive(enemigo);
            enemigo.Attack(jugador, () => {
                ElegirSiguienteActivo();
            });
        }
        else
        {

            SetActive(jugador);
            state = State.WaitingForPlayer;

        }


    }
    private bool BattleOver()
    {
        if (jugador.isDead())
        {
            VentanaBatalla.Show_Static("ENEMY WINS!");
            return true;

        }

        if (enemigo.isDead()){

            VentanaBatalla.Show_Static("PLAYER WINS!");
            return true;
        }
        return false;
    }
}
