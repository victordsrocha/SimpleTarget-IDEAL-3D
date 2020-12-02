using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvInterface : MonoBehaviour
{
    private Memory memory { get; set; }
    private Player _player;
    private Observation _observation;
    private Enacter _enacter;
    public bool getObservation;
    public Interaction enactedPrimitiveInteraction;

    private void Start()
    {
        memory = GetComponent<Memory>();
        _player = GetComponent<Player>();
        _observation = GetComponent<Observation>();
        _enacter = GetComponent<Enacter>();
    }

    public IEnumerator InterfaceEnactCoroutine(Interaction intendedInteraction)
    {
        // ▶ ▷ △ ▲ ▼ ▽ ◀ ◁ ◇ ◈ ◆ ↑ ↓
        
        getObservation = false;
        string result = null;
        char act = intendedInteraction.Label[0];
        
        switch (act)
        {
            case '↑':
                _player.RotateLeft();
                break;
            case '→':
                _player.MoveForward();
                break;
            case '↓':
                _player.RotateRight();
                break;
        }
        
        yield return new WaitUntil(() => getObservation);

        result += act;
        result += _observation.Result(act);

        var enactedInteraction = memory.GetOrAddPrimitiveInteraction(result);
        
        Debug.Log("primitive enacted interaction: " + enactedInteraction);

        enactedPrimitiveInteraction = enactedInteraction;
        _enacter.enactedInteractionsPrimitiveQueue.Enqueue(enactedInteraction);
        Enacter.nextPrimitiveAction = true;
    }
}