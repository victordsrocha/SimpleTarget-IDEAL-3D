using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class AgentAI : MonoBehaviour
{
    private Memory memory;
    private Decider decider;
    private Enacter enacter;
    private string mood;
    public bool nextStep;
    public static bool tryEnactComplete;
    public static Interaction EnactedInteractionGlobal;

    private StepManager stepManager;

    private void Start()
    {
        memory = GetComponent<Memory>();
        decider = GetComponent<Decider>();
        enacter = GetComponent<Enacter>();
        mood = null;
        nextStep = true;

        stepManager = FindObjectOfType<StepManager>();

        StartCoroutine(StepCoroutine());
    }

    private Interaction GetIntendedInteraction()
    {
        var anticipations = decider.Anticipate();
        var experiment = decider.SelectExperiment(anticipations);
        var intendedInteraction = experiment.IntendedInteraction;
        return intendedInteraction;
    }

    void TryEnactIntendedInteractionAndLearn(Interaction intendedInteraction)
    {
        var enactedInteraction = enacter.Enact(intendedInteraction);
        if (enactedInteraction != intendedInteraction)
        {
            // logica -> nao lembro se intendedInteraction.experiment é sempre igual ao experiment.intendedInteraction
            // estou assumindo que seja
            intendedInteraction.Experiment.EnactedInteractions.Add(enactedInteraction);
            //Debug.Log("enacted_interaction != intended_interaction");
        }

        //Debug.Log("Enacted " + enactedInteraction);
        decider.LearnCompositeInteraction(enactedInteraction);
        decider.enactedInteraction = enactedInteraction;
        nextStep = true;
    }

    IEnumerator TryEnactAndLearnCoroutine(Interaction intendedInteraction)
    {
        tryEnactComplete = false;
        StartCoroutine(enacter.EnactCoroutine(intendedInteraction));
        yield return new WaitUntil(() => tryEnactComplete);


        stepManager.enactedInteractionText.text = enacter.finalEnactedInteraction.Label;

        if (stepManager.stepByStep)
        {
            stepManager.frozen = true;
            yield return new WaitUntil(() => !stepManager.frozen);
        }


        Learn(intendedInteraction, enacter.finalEnactedInteraction);
        nextStep = true;
    }

    void Learn(Interaction intendedInteraction, Interaction enactedInteraction)
    {
        if (enactedInteraction != intendedInteraction)
        {
            // logica -> nao lembro se intendedInteraction.experiment é sempre igual ao experiment.intendedInteraction
            // estou assumindo que seja
            intendedInteraction.Experiment.EnactedInteractions.Add(enactedInteraction);
            //Debug.Log("enacted_interaction != intended_interaction");
        }

        //Debug.Log("Enacted " + enactedInteraction);
        decider.LearnCompositeInteraction(enactedInteraction);
        decider.enactedInteraction = enactedInteraction;
    }

    private void SetMood()
    {
        mood = decider.enactedInteraction.valence >= 0 ? "PLEASED" : "PAINED";
    }

    private IEnumerator StepCoroutine()
    {
        while (true)
        {
            nextStep = false;
            var intendedInteraction = GetIntendedInteraction();

            stepManager.intendedInteractionText.text = intendedInteraction.Label;
            stepManager.enactedInteractionText.text = "";

            if (stepManager.stepByStep)
            {
                stepManager.frozen = true;
                yield return new WaitUntil(() => !stepManager.frozen);
            }

            StartCoroutine(TryEnactAndLearnCoroutine(intendedInteraction));


            yield return new WaitUntil(() => nextStep);
        }
    }

    private int InteractionSize(Interaction interaction)
    {
        string label = interaction.Label;
        int size = 0;

        size += 1 * label.Count(x => x == '↑');
        size += 1 * label.Count(x => x == '→');
        size += 1 * label.Count(x => x == '↓');

        return size;
    }
}