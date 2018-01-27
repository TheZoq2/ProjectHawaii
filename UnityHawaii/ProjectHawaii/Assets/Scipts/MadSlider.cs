using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MadSlider : MonoBehaviour {

    [SerializeField] RectTransform[] points;
    [SerializeField] RectTransform mover;


    [SerializeField, ReadOnly] int currentId;

    public void SetIsland(int id)
    {
        mover.position = points[id].position;
        currentId = id;
    }
}
