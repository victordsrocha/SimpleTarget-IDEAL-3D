using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Enacter : MonoBehaviour
{
    public Memory memory { get; set; }
    public EnvInterface EnvInterface { get; set; }
    public static bool nextPrimitiveAction;
    public Interaction finalEnactedInteraction;
    public Queue<Interaction> enactedInteractionsPrimitiveQueue;

    private StepManager stepManager;

    private void Start()
    {
        memory = GetComponent<Memory>();
        EnvInterface = GetComponent<EnvInterface>();
        nextPrimitiveAction = false;

        stepManager = FindObjectOfType<StepManager>();

        enactedInteractionsPrimitiveQueue = new Queue<Interaction>();
    }

    public Interaction Enact(Interaction intendedInteraction)
    {
        if (intendedInteraction.is_primitive())
        {
            var enactedPrimitiveInteraction = enactedInteractionsPrimitiveQueue.Dequeue();

            if (enactedPrimitiveInteraction==null)
            {
                throw new Exception();
            }
            
            return enactedPrimitiveInteraction;
            //return EnvInterface.EnactPrimitiveInteraction(intendedInteraction);
        }
        else
        {
            // Enact the pre-interaction
            var enactedPreInteraction = Enact(intendedInteraction.PreInteraction);
            if (enactedPreInteraction != intendedInteraction.PreInteraction)
            {
                // if the preInteraction failed then the enaction of the intendedInteraction is interrupted here
                return enactedPreInteraction;
            }
            else
            {
                // enact the post-interaction
                var enactedPostInteraction = Enact(intendedInteraction.PostInteraction);
                return memory.AddOrGetCompositeInteraction(enactedPreInteraction, enactedPostInteraction);
            }
        }
    }

    public IEnumerator EnactCoroutine(Interaction intendedInteraction)
    {
        stepManager.enactedInteractionText.text = "";
        Stack<Interaction> stackNextPrimitive = new Stack<Interaction>();
        stackNextPrimitive.Push(intendedInteraction);

        while (stackNextPrimitive.Count > 0)
        {
            var nextInteraction = stackNextPrimitive.Pop();
            if (nextInteraction.is_primitive())
            {
                nextPrimitiveAction = false;
                StartCoroutine(EnvInterface.InterfaceEnactCoroutine(nextInteraction));
                yield return new WaitUntil(() => nextPrimitiveAction);
                
                if (nextInteraction != EnvInterface.enactedPrimitiveInteraction)
                {
                    stepManager.enactedInteractionText.color = Color.red;
                    stepManager.enactedInteractionText.text += EnvInterface.enactedPrimitiveInteraction.Label + " ";
                    
                    break;
                }
                else
                {
                    stepManager.enactedInteractionText.color = Color.black;
                    stepManager.enactedInteractionText.text += EnvInterface.enactedPrimitiveInteraction.Label + " ";
                }
            }
            else
            {
                stackNextPrimitive.Push(nextInteraction.PostInteraction);
                stackNextPrimitive.Push(nextInteraction.PreInteraction);
            }
        }

        finalEnactedInteraction = Enact(intendedInteraction);
        AgentAI.tryEnactComplete = true;
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