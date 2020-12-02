using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Observation : MonoBehaviour
{
    public enum VisionState
    {
        Appear,
        Closer,
        Reached,
        Disappear,
        Unchanged
    }

    [SerializeField] private float debugDistanceLeft;
    [SerializeField] private VisionState debugVisionStateLeft;
    [SerializeField] private bool debugIsSeeingFood;

    // public Target food;

    public FieldOfView leftEyeFieldOfView, rightEyeFieldOfView;
    private Player player;

    private Eye leftEye, rightEye;


    private void Awake()
    {
        leftEye = new Eye(leftEyeFieldOfView);
        rightEye = new Eye(rightEyeFieldOfView);
        player = GetComponent<Player>();
    }

    public void UpdateObservation(bool move = false)
    {
        if (player.isFoodReached)
        {
            FoodReached(leftEye, rightEye);
        }
        else
        {
            UpdateEye(leftEye, move);
            UpdateEye(rightEye, move);
        }

        // for debug
        debugDistanceLeft = leftEye.lastDistance;
        debugVisionStateLeft = leftEye.lastVisionState;
        debugIsSeeingFood = leftEye.isSeeingFood;
    }

    private void FoodReached(Eye left, Eye right)
    {
        player.isFoodReached = false;
        
        left.lastVisionState = VisionState.Reached;
        right.lastVisionState = VisionState.Reached;

        left.lastDistance = float.PositiveInfinity;
        right.lastDistance = float.PositiveInfinity;

        left.isSeeingFood = false;
        right.isSeeingFood = false;
    }

    private void UpdateEye(Eye eye, bool move)
    {
        eye.fieldOfView.FindVisibleTargets();
        if (eye.fieldOfView.visibleTargets.Count > 0)
        {
            var foodTransform = eye.fieldOfView.visibleTargets[0];
            if (eye.isSeeingFood)
            {
                if (move)
                {
                    if (Distance(foodTransform) < eye.lastDistance)
                    {
                        eye.lastVisionState = VisionState.Closer;
                        eye.lastDistance = Distance(foodTransform);
                    }
                    else
                    {
                        eye.lastVisionState = VisionState.Unchanged;
                        eye.lastDistance = Distance(foodTransform);
                    }
                }
                else
                {
                    //Vector3 myPos = new Vector3(transform.position.x,0,transform.position.z);
                    //Vector3 foodPos = new Vector3(foodTransform.position.x,0, foodTransform.position.z);

                    //var foodDirection = (foodPos - myPos).normalized;

                    //Transform myTransformXZ = transform;

                    //var angle = Vector3.Angle()

                    eye.lastVisionState = VisionState.Unchanged;
                }
            }
            else
            {
                eye.lastVisionState = VisionState.Appear;
                eye.lastDistance = Distance(foodTransform);
            }

            eye.isSeeingFood = true;
        }
        else
        {
            if (eye.isSeeingFood)
            {
                eye.lastVisionState = VisionState.Disappear;
                eye.lastDistance = float.PositiveInfinity;
            }
            else
            {
                eye.lastVisionState = VisionState.Unchanged;
            }

            eye.isSeeingFood = false;
        }

        // por enquanto o Reached é ativado quando o olho está vendo a comida com menos de 3f de distancia
        // e a ultima ação foi mover para frente
        // talvez seja interessante mudar para quando a comida é efetivamente alcançada
        /*
        if (eye.isSeeingFood && Distance(eye.fieldOfView.visibleTargets[0]) < 3f && move)
        {
            eye.lastVisionState = VisionState.Reached;
        }
        */
    }

    private float Distance(Transform target)
    {
        var dstToTarget = Vector3.Distance(transform.position, target.position);
        return dstToTarget;
    }

    public string Result()
    {
        string result = "";

        if (player.collision)
        {
            result += "b";
        }
        else
        {
            result += "m";
        }

        result += leftEye.EyeResultObservation();
        result += rightEye.EyeResultObservation();

        return result;
    }

    public class Eye
    {
        public FieldOfView fieldOfView;
        public VisionState lastVisionState;
        public float lastDistance;
        public bool isSeeingFood;

        public Eye(FieldOfView fieldOfView)
        {
            this.fieldOfView = fieldOfView;
            lastVisionState = VisionState.Unchanged;
            lastDistance = float.PositiveInfinity;
            isSeeingFood = false;
        }

        public string EyeResultObservation()
        {
            switch (lastVisionState)
            {
                case VisionState.Appear:
                    return "*";
                case VisionState.Closer:
                    return "+";
                case VisionState.Reached:
                    return "x";
                case VisionState.Disappear:
                    return "o";
                case VisionState.Unchanged:
                    return "-";
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}