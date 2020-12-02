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
        compositeInteraction.Weight++;

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

    private Interaction AddPrimitiveInteraction(string label)
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

        sumValence += -50 * label.Count(x => x == 'b'); // Bump

        sumValence += 15 * label.Count(x => x == '*'); // Appear
        sumValence += 10 * label.Count(x => x == '+'); // Closer
        sumValence += 50 * label.Count(x => x == 'x'); // Reached
        sumValence += -30 * label.Count(x => x == 'o'); // Disappear


        return sumValence;
    }

    private void InitSimpleTarget()
    {
        // criar uma função para automatizar a criação de listas de estados maiores

        var l1 = AddPrimitiveInteraction("↑m**");
        var l2 = AddPrimitiveInteraction("↑m*+");
        var l3 = AddPrimitiveInteraction("↑m*x");
        var l4 = AddPrimitiveInteraction("↑m*o");
        var l5 = AddPrimitiveInteraction("↑m*-");
        var l6 = AddPrimitiveInteraction("↑m+*");
        var l7 = AddPrimitiveInteraction("↑m++");
        var l8 = AddPrimitiveInteraction("↑m+x");
        var l9 = AddPrimitiveInteraction("↑m+o");
        var l10 = AddPrimitiveInteraction("↑m+-");
        var l11 = AddPrimitiveInteraction("↑mx*");
        var l12 = AddPrimitiveInteraction("↑mx+");
        var l13 = AddPrimitiveInteraction("↑mxx");
        var l14 = AddPrimitiveInteraction("↑mxo");
        var l15 = AddPrimitiveInteraction("↑mx-");
        var l16 = AddPrimitiveInteraction("↑mo*");
        var l17 = AddPrimitiveInteraction("↑mo+");
        var l18 = AddPrimitiveInteraction("↑mox");
        var l19 = AddPrimitiveInteraction("↑moo");
        var l20 = AddPrimitiveInteraction("↑mo-");
        var l21 = AddPrimitiveInteraction("↑m-*");
        var l22 = AddPrimitiveInteraction("↑m-+");
        var l23 = AddPrimitiveInteraction("↑m-x");
        var l24 = AddPrimitiveInteraction("↑m-o");
        var l25 = AddPrimitiveInteraction("↑m--");
        var l26 = AddPrimitiveInteraction("↑b**");
        var l27 = AddPrimitiveInteraction("↑b*+");
        var l28 = AddPrimitiveInteraction("↑b*x");
        var l29 = AddPrimitiveInteraction("↑b*o");
        var l30 = AddPrimitiveInteraction("↑b*-");
        var l31 = AddPrimitiveInteraction("↑b+*");
        var l32 = AddPrimitiveInteraction("↑b++");
        var l33 = AddPrimitiveInteraction("↑b+x");
        var l34 = AddPrimitiveInteraction("↑b+o");
        var l35 = AddPrimitiveInteraction("↑b+-");
        var l36 = AddPrimitiveInteraction("↑bx*");
        var l37 = AddPrimitiveInteraction("↑bx+");
        var l38 = AddPrimitiveInteraction("↑bxx");
        var l39 = AddPrimitiveInteraction("↑bxo");
        var l40 = AddPrimitiveInteraction("↑bx-");
        var l41 = AddPrimitiveInteraction("↑bo*");
        var l42 = AddPrimitiveInteraction("↑bo+");
        var l43 = AddPrimitiveInteraction("↑box");
        var l44 = AddPrimitiveInteraction("↑boo");
        var l45 = AddPrimitiveInteraction("↑bo-");
        var l46 = AddPrimitiveInteraction("↑b-*");
        var l47 = AddPrimitiveInteraction("↑b-+");
        var l48 = AddPrimitiveInteraction("↑b-x");
        var l49 = AddPrimitiveInteraction("↑b-o");
        var l50 = AddPrimitiveInteraction("↑b--");
        var l51 = AddPrimitiveInteraction("→m**");
        var l52 = AddPrimitiveInteraction("→m*+");
        var l53 = AddPrimitiveInteraction("→m*x");
        var l54 = AddPrimitiveInteraction("→m*o");
        var l55 = AddPrimitiveInteraction("→m*-");
        var l56 = AddPrimitiveInteraction("→m+*");
        var l57 = AddPrimitiveInteraction("→m++");
        var l58 = AddPrimitiveInteraction("→m+x");
        var l59 = AddPrimitiveInteraction("→m+o");
        var l60 = AddPrimitiveInteraction("→m+-");
        var l61 = AddPrimitiveInteraction("→mx*");
        var l62 = AddPrimitiveInteraction("→mx+");
        var l63 = AddPrimitiveInteraction("→mxx");
        var l64 = AddPrimitiveInteraction("→mxo");
        var l65 = AddPrimitiveInteraction("→mx-");
        var l66 = AddPrimitiveInteraction("→mo*");
        var l67 = AddPrimitiveInteraction("→mo+");
        var l68 = AddPrimitiveInteraction("→mox");
        var l69 = AddPrimitiveInteraction("→moo");
        var l70 = AddPrimitiveInteraction("→mo-");
        var l71 = AddPrimitiveInteraction("→m-*");
        var l72 = AddPrimitiveInteraction("→m-+");
        var l73 = AddPrimitiveInteraction("→m-x");
        var l74 = AddPrimitiveInteraction("→m-o");
        var l75 = AddPrimitiveInteraction("→m--");
        var l76 = AddPrimitiveInteraction("→b**");
        var l77 = AddPrimitiveInteraction("→b*+");
        var l78 = AddPrimitiveInteraction("→b*x");
        var l79 = AddPrimitiveInteraction("→b*o");
        var l80 = AddPrimitiveInteraction("→b*-");
        var l81 = AddPrimitiveInteraction("→b+*");
        var l82 = AddPrimitiveInteraction("→b++");
        var l83 = AddPrimitiveInteraction("→b+x");
        var l84 = AddPrimitiveInteraction("→b+o");
        var l85 = AddPrimitiveInteraction("→b+-");
        var l86 = AddPrimitiveInteraction("→bx*");
        var l87 = AddPrimitiveInteraction("→bx+");
        var l88 = AddPrimitiveInteraction("→bxx");
        var l89 = AddPrimitiveInteraction("→bxo");
        var l90 = AddPrimitiveInteraction("→bx-");
        var l91 = AddPrimitiveInteraction("→bo*");
        var l92 = AddPrimitiveInteraction("→bo+");
        var l93 = AddPrimitiveInteraction("→box");
        var l94 = AddPrimitiveInteraction("→boo");
        var l95 = AddPrimitiveInteraction("→bo-");
        var l96 = AddPrimitiveInteraction("→b-*");
        var l97 = AddPrimitiveInteraction("→b-+");
        var l98 = AddPrimitiveInteraction("→b-x");
        var l99 = AddPrimitiveInteraction("→b-o");
        var l100 = AddPrimitiveInteraction("→b--");
        var l101 = AddPrimitiveInteraction("↓m**");
        var l102 = AddPrimitiveInteraction("↓m*+");
        var l103 = AddPrimitiveInteraction("↓m*x");
        var l104 = AddPrimitiveInteraction("↓m*o");
        var l105 = AddPrimitiveInteraction("↓m*-");
        var l106 = AddPrimitiveInteraction("↓m+*");
        var l107 = AddPrimitiveInteraction("↓m++");
        var l108 = AddPrimitiveInteraction("↓m+x");
        var l109 = AddPrimitiveInteraction("↓m+o");
        var l110 = AddPrimitiveInteraction("↓m+-");
        var l111 = AddPrimitiveInteraction("↓mx*");
        var l112 = AddPrimitiveInteraction("↓mx+");
        var l113 = AddPrimitiveInteraction("↓mxx");
        var l114 = AddPrimitiveInteraction("↓mxo");
        var l115 = AddPrimitiveInteraction("↓mx-");
        var l116 = AddPrimitiveInteraction("↓mo*");
        var l117 = AddPrimitiveInteraction("↓mo+");
        var l118 = AddPrimitiveInteraction("↓mox");
        var l119 = AddPrimitiveInteraction("↓moo");
        var l120 = AddPrimitiveInteraction("↓mo-");
        var l121 = AddPrimitiveInteraction("↓m-*");
        var l122 = AddPrimitiveInteraction("↓m-+");
        var l123 = AddPrimitiveInteraction("↓m-x");
        var l124 = AddPrimitiveInteraction("↓m-o");
        var l125 = AddPrimitiveInteraction("↓m--");
        var l126 = AddPrimitiveInteraction("↓b**");
        var l127 = AddPrimitiveInteraction("↓b*+");
        var l128 = AddPrimitiveInteraction("↓b*x");
        var l129 = AddPrimitiveInteraction("↓b*o");
        var l130 = AddPrimitiveInteraction("↓b*-");
        var l131 = AddPrimitiveInteraction("↓b+*");
        var l132 = AddPrimitiveInteraction("↓b++");
        var l133 = AddPrimitiveInteraction("↓b+x");
        var l134 = AddPrimitiveInteraction("↓b+o");
        var l135 = AddPrimitiveInteraction("↓b+-");
        var l136 = AddPrimitiveInteraction("↓bx*");
        var l137 = AddPrimitiveInteraction("↓bx+");
        var l138 = AddPrimitiveInteraction("↓bxx");
        var l139 = AddPrimitiveInteraction("↓bxo");
        var l140 = AddPrimitiveInteraction("↓bx-");
        var l141 = AddPrimitiveInteraction("↓bo*");
        var l142 = AddPrimitiveInteraction("↓bo+");
        var l143 = AddPrimitiveInteraction("↓box");
        var l144 = AddPrimitiveInteraction("↓boo");
        var l145 = AddPrimitiveInteraction("↓bo-");
        var l146 = AddPrimitiveInteraction("↓b-*");
        var l147 = AddPrimitiveInteraction("↓b-+");
        var l148 = AddPrimitiveInteraction("↓b-x");
        var l149 = AddPrimitiveInteraction("↓b-o");
        var l150 = AddPrimitiveInteraction("↓b--");


        AddOrGetAbstractExperiment(l25);
        AddOrGetAbstractExperiment(l75);
        AddOrGetAbstractExperiment(l125);
    }
}