using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class TrackCheckpoints : MonoBehaviour
{
    public class CarCheckPointEventArgs : EventArgs
    {
        public Transform carTransform { get; set; }
    }
 
    public event EventHandler<CarCheckPointEventArgs> OnCarCorrectCheckpoint;
    public event EventHandler<CarCheckPointEventArgs> OnCarWrongCheckpoint;
 
    [SerializeField] private List<Transform> carTransformList;
 
    private List<Checkpoint> checkpointList;
    private List<int> nextCheckpointIndexList;
 
    private void Awake()
    {
        Transform trackCheckpointsTransform = transform.Find("TrackCheckpoints");
        checkpointList = new List<Checkpoint>();
 
        foreach(Transform checkpointAITransform in trackCheckpointsTransform)
        {
            Checkpoint checkpoint = checkpointAITransform.GetComponent<Checkpoint>();
            checkpoint.SetTrackCheckpoints(this);
            checkpointList.Add(checkpoint);
        }
 
        nextCheckpointIndexList = new List<int>();
        foreach(Transform carTransform in carTransformList)
        {
            nextCheckpointIndexList.Add(0);
        }
    }
 
    public void CarThroughCheckpoint(Checkpoint checkpoint, Transform carTransform)
    {
        int nextCheckpointIndex = nextCheckpointIndexList[carTransformList.IndexOf(carTransform)];
        if (checkpointList.IndexOf(checkpoint) == nextCheckpointIndex)
        {
            nextCheckpointIndexList[carTransformList.IndexOf(carTransform)] = (nextCheckpointIndex + 1) % checkpointList.Count;
            OnCarCorrectCheckpoint?.Invoke(this, new CarCheckPointEventArgs { carTransform = carTransform });
 
        } else
        {
            OnCarWrongCheckpoint?.Invoke(this, new CarCheckPointEventArgs { carTransform = carTransform });
        }
 
    }
 
    public void ResetCheckpoints(Transform carTransform)
    {
        nextCheckpointIndexList[carTransformList.IndexOf(carTransform)] = 0;
    }
 
    public Checkpoint GetNextCheckpoint(Transform carTransform)
    {
        return checkpointList[nextCheckpointIndexList[carTransformList.IndexOf(carTransform)]];
    }
}
