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
    private int _primitiveStep;

    private void Start()
    {
        memory = GetComponent<Memory>();
        _player = GetComponent<Player>();
        _observation = GetComponent<Observation>();
        _enacter = GetComponent<Enacter>();
        _primitiveStep = 1;
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

        //realizar registro de cada interação primitiva com sua valencia e seu turno
        AddPrimitiveRecord(_primitiveStep.ToString(), intendedInteraction.Label, memory.CalcValence(intendedInteraction.Label).ToString(),
            enactedInteraction.Label, memory.CalcValence(enactedInteraction.Label).ToString(), "primitiveStepsRecord.csv");
        _primitiveStep++;
    }

    public static void AddPrimitiveRecord(string primitiveStep, string primitiveIntended, string intendedValence,
        string primitiveEnacted, string enactedValence, string filepath)
    {
        try
        {
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(@filepath, true))
            {
                file.WriteLine(primitiveStep + "," + primitiveIntended + "," + intendedValence + "," +
                               primitiveEnacted + "," + enactedValence);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}