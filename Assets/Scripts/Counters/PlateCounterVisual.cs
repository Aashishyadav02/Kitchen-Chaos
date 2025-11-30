using System;
using System.Collections.Generic;
using UnityEngine;

public class PlateCounterVisual : MonoBehaviour
{
    [SerializeField] private PlatesCounter platesCounter;
    [SerializeField] private Transform counterTopPoint;
    [SerializeField] private Transform plateVisualPrefab;

    private List<GameObject> plateVisualsGameObjectsList;

    private void Awake()
    {
        plateVisualsGameObjectsList = new List<GameObject>();
    }
    private void Start()
    {
        platesCounter.OnPlateSpawned += PlatesCounter_OnPlateSpawned;
        platesCounter.OnPlateRemoved += PlatesCounter_OnPlateRemoved;
    }

    private void PlatesCounter_OnPlateRemoved(object sender, System.EventArgs e)
    {
        GameObject plateGameObject=plateVisualsGameObjectsList[plateVisualsGameObjectsList.Count - 1];
        plateVisualsGameObjectsList.Remove(plateGameObject);
        Destroy(plateGameObject);
    }

    private void PlatesCounter_OnPlateSpawned(object sender,System.EventArgs e)
    {
        Transform plateVisualTransform = Instantiate(plateVisualPrefab, counterTopPoint);
        float plateOffSetY = .1f;
        plateVisualTransform.localPosition=new Vector3(0,plateOffSetY * plateVisualsGameObjectsList.Count ,0) ;
        plateVisualsGameObjectsList.Add(plateVisualTransform.gameObject);
    }
}
