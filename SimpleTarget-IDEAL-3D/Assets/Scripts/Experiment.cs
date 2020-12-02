using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Experiment
{
    public string Label { get; set; }
    public HashSet<Interaction> EnactedInteractions { get; set; }
    public Interaction IntendedInteraction { get; set; }

    public Experiment(string label)
    {
        this.Label = label;
        EnactedInteractions = new HashSet<Interaction>();
        IntendedInteraction = null;
    }
}