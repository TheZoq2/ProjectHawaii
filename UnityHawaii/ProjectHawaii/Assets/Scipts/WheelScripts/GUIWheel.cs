using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class GUIWheel : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler{

    private float wheelAngle;
    private float rollBackTimer = 0;
    private float rollBackTimerStartValue = 3f;

    public float wheelRotationGoal = 90;
    public float GoalPrecision = 10;
    public float CurrentRotation = 0;
    private bool isRotating = false;
	
    public Text text;

    public UnityEvent onValueChange;

    private void Start()
    {
        text.text = "0.0";
    }

    // Update is called once per frame
    void Update () {
        if (rollBackTimer > 0)
        {
            rollBackTimer -= Time.deltaTime;
            //RollBack();
        }
        if (isRotating)
        {
            if (transform.localRotation.eulerAngles.z  > wheelRotationGoal - GoalPrecision && 
                transform.localRotation.eulerAngles.z  < wheelRotationGoal + GoalPrecision)
            {
                PositionCorrect();
            }
            else
            {
                PositionInCorrect();
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        //CancelRollBack();

        var dir = Input.mousePosition - transform.position;
        wheelAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        wheelAngle -= Mathf.Atan2(transform.right.y, transform.right.x) * Mathf.Rad2Deg;
        
        isRotating = true;
        
    }
    
    public void OnDrag(PointerEventData eventdata)
    {
        float oldRotation = transform.rotation.eulerAngles.z;

        Vector3 dir = Input.mousePosition - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - wheelAngle ;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        CurrentRotation = transform.localRotation.eulerAngles.z;
        //text.text = CurrentRotation.ToString();
        
        TableControlsManager.instance.SetWheel(CurrentRotation);
        text.text = CurrentRotation.ToString();
        onValueChange.Invoke();

    }

    public void OnPointerUp(PointerEventData eventdata)
    {
        isRotating = false;
        //StartRollBack();
    }

    public void PositionCorrect()
    {

    }

    public void PositionInCorrect()
    {

    }

    /*
    private void StartRollBack()
    {
        rollBackTimer = rollBackTimerStartValue;
    }
    
    private void RollBack()
    {
        wheelAngle = Mathf.Lerp(-90, wheelAngle, rollBackTimer / rollBackTimerStartValue);
        transform.rotation = Quaternion.AngleAxis(wheelAngle, Vector3.forward);
    }

    private void CancelRollBack()
    {
        rollBackTimer = 0;
    }*/
}
