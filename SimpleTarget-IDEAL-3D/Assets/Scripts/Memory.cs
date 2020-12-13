using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class Memory : MonoBehaviour
{
    public Dictionary<string, Interaction> KnownInteractions;
    public Dictionary<string, Experiment> KnownExperiments;
    private int interactionLabelSize;
    [SerializeField] private float forgettingRate = 0.2f;

    private void Start()
    {
        KnownInteractions = new Dictionary<string, Interaction>();
        KnownExperiments = new Dictionary<string, Experiment>();
        InitSimpleTarget();
        interactionLabelSize = 4; // lembrar de atualizar quando for mudar
    }

    public Interaction AddOrGetCompositeInteraction(Interaction preInteraction, Interaction postInteraction)
    {
        string label = "(" + preInteraction.Label + postInteraction.Label + ")";
        Interaction interaction;
        if (!KnownInteractions.ContainsKey(label))
        {
            interaction = AddOrGetInteraction(label);
            interaction.PreInteraction = preInteraction;
            interaction.PostInteraction = postInteraction;
            interaction.valence = preInteraction.valence + postInteraction.valence;
            AddOrGetAbstractExperiment(interaction);
        }
        else
        {
            interaction = KnownInteractions[label];
        }

        return interaction;
    }

    public Interaction AddOrGetAndReinforceCompositeInteraction(Interaction preInteraction, Interaction postInteraction)
    {
        var compositeInteraction = AddOrGetCompositeInteraction(preInteraction, postInteraction);
        compositeInteraction.Weight += 1f;

        /*
        if (compositeInteraction.Weight == 1)
        {
            Debug.Log("Learn " + compositeInteraction);
        }
        else
        {
            Debug.Log("reinforce " + compositeInteraction);
        }
        */


        return compositeInteraction;
    }

    public void DecrementAndForgetSchemas(List<Interaction> enactedInteractions)
    {
        foreach (var knownInteraction in KnownInteractions.Values)
        {
            if (!knownInteraction.is_primitive())
            {
                if (!enactedInteractions.Contains(knownInteraction))
                {
                    // estou dividindo pela valencia como uma forma provisoria de garantir que 
                    // memorias muito boas ou muito ruins sejam mais dificilemtne esquecidas
                    if (knownInteraction.valence > 1 || knownInteraction.valence < -1)
                    {
                        knownInteraction.Weight -= forgettingRate / Mathf.Abs(knownInteraction.valence);
                    }
                    else
                    {
                        knownInteraction.Weight -= forgettingRate;
                    }
                }


                // When a schema reaches a weight of 0 it is deleted from memory
                if (knownInteraction.Weight < 0.01f)
                {
                    // KnownInteractions.Remove(knownInteraction.Label);
                    knownInteraction.Weight = 0f;

                    /* Não é possível excluir, uma interação esquecida pode ser pré-interação de uma outra */
                }
            }
        }
    }

    private Interaction AddOrGetInteraction(string label)
    {
        if (!KnownInteractions.ContainsKey(label))
        {
            Interaction interaction = new Interaction(label);
            KnownInteractions.Add(label, interaction);
        }

        return KnownInteractions[label];
    }

    private Experiment AddOrGetAbstractExperiment(Interaction interaction)
    {
        string label = interaction.Label.Replace('e', 'E').Replace('r', 'R').Replace(')', '|');
        if (!KnownExperiments.ContainsKey(label))
        {
            var abstractExperiment = new Experiment(label);
            abstractExperiment.IntendedInteraction = interaction;
            interaction.Experiment = abstractExperiment;
            KnownExperiments.Add(label, abstractExperiment);
        }

        return KnownExperiments[label];
    }

    public Interaction GetOrAddPrimitiveInteraction(string label)
    {
        Interaction interaction;
        if (!KnownInteractions.ContainsKey(label))
        {
            interaction = AddOrGetInteraction(label);
            interaction.valence = CalcValence(label);
        }
        else
        {
            interaction = KnownInteractions[label];
        }

        return interaction;
    }

    public Interaction GetPrimitiveInteraction(string label)
    {
        return KnownInteractions[label];
    }

    private int CalcValence(string label)
    {
        /*sum_valence = 0
        sum_valence += label.count('^') * (-5)
        sum_valence += label.count('>') * (-5)
        sum_valence += label.count('v') * (-5)
        sum_valence += label.count('.') * (-1)
        sum_valence += label.count('-') * (+0)
        sum_valence += label.count('*') * (+10)
        sum_valence += label.count('+') * (+30)
        sum_valence += label.count('x') * (+50)
        sum_valence += label.count('o') * (-30)

        if label[0] == '>':
        sum_valence += label.count('w') * (-200)
        else:
        sum_valence += label.count('w') * (-200)

        return sum_valence*/

        int sumValence = 0;

        //sumValence += -0 * label.Count(x => x == '↑'); // Rotate Left
        //sumValence += -0 * label.Count(x => x == '→'); // Forward
        //sumValence += -0 * label.Count(x => x == '↓'); // Rotate Right

        //sumValence += -0 * label.Count(x => x == '-'); // Unchanged

        // Bump
        if ((label.Count(x => x == 'b') > 0 && label[0] == '→') ||
            label[4] == 'b' || label[5] == 'b')
        {
            sumValence += -8;
        }
        else
        {
            sumValence += 0;
        }

        sumValence += 15 * label.Count(x => x == '*'); // Appear
        sumValence += 10 * label.Count(x => x == '+'); // Closer
        sumValence += 10 * label.Count(x => x == 'x'); // Reached
        sumValence += -15 * label.Count(x => x == 'o'); // Disappear


        return sumValence;
    }

    private void InitSimpleTarget()
    {
        // ▶ ▷ △ ▲ ▼ ▽ ◀ ◁ ◇ ◈ ◆ ↑ ↓

        var left = GetOrAddPrimitiveInteraction("↑eeeeeeee--");
        var forward = GetOrAddPrimitiveInteraction("→eeeeeeee--");
        var right = GetOrAddPrimitiveInteraction("↓eeeeeeee--");

        AddOrGetAbstractExperiment(left);
        AddOrGetAbstractExperiment(forward);
        AddOrGetAbstractExperiment(right);
    }
}