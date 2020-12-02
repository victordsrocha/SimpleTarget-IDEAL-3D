using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvInterface : MonoBehaviour
{
    //public SimpleTarget env { get; set; }
    private Memory memory { get; set; }
    private Player player;
    private Observation observation;
    private string resultSensor;
    public static bool getObservation;
    public Interaction enactedPrimitiveInteraction;
    private Enacter enacter;

    private void Start()
    {
        memory = GetComponent<Memory>();
        player = GetComponent<Player>();
        observation = GetComponent<Observation>();
        enacter = GetComponent<Enacter>();

        /*
         * ao invés de pensar em uma classe enviroment eu deveria comunicar o envInterface somente com as classes de "motores do meu robô"
         * ex: classe que move meu robo, classe que controla a visao, etc
         * não será preciso uma classes para ambiente de fato, isso ficara implicito na engine
         */
        // var env = playerMovement; // mudar nomenclarura depois
        //env = new SimpleTarget();
    }
    
    public Interaction EnactPrimitiveInteraction(Interaction intendedInteraction)
    {
        enactedPrimitiveInteraction = null;
        StartCoroutine(InterfaceEnactCoroutine(intendedInteraction));

        // TODO
        // enactedPrimitiveInteraction é atualizado dentro da corrotina

        return enactedPrimitiveInteraction;
    }

    public IEnumerator InterfaceEnactCoroutine(Interaction intendedInteraction)
    {
        getObservation = false;
        string result = null;
        char act = intendedInteraction.Label[0];
        
        switch (act)
        {
            case '↑':
                player.RotateLeft();
                break;
            case '→':
                player.MoveForward();
                break;
            case '↓':
                player.RotateRight();
                break;
        }
        
        yield return new WaitUntil(() => getObservation);

        result += act;
        result += observation.Result();
        
        // TODO preciso trocar getprimitiveinteraction por GetOrCreate para permitir plasticidade
        // deste modo também não seria preciso iniciar o sistema já com todas as possíveis int. primitivas
        // previamente declaradas!
        var enactedInteraction = memory.GetPrimitiveInteraction(result);
        Debug.Log("primitive enacted interaction: " + enactedInteraction);

        enactedPrimitiveInteraction = enactedInteraction;
        enacter.enactedInteractionsPrimitiveQueue.Enqueue(enactedInteraction);
        Enacter.nextPrimitiveAction = true;
    }

    /*
    private IEnumerator EnactCoroutine(Interaction intendedInteraction)
    {
        // manda fazer a ação
        
        // espera pelo mesmo tempo que ação precisa pra ser executada (timeBetwennACtion)
        
        // na verdade acho que posso fazer um waituntil perguntando "já pegou a observação?"
        
        // coleta observação
    }
    */
}