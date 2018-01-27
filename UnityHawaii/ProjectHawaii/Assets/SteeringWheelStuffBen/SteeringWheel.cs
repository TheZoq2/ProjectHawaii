using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteeringWheel : MonoBehaviour {

    private float wheelAngle;
    private float rollBackTimer = 0;
    private float rollBackTimerStartValue = 3f;

	// Use this for initialization
	void Start () {
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
	}
	
	// Update is called once per frame
	void Update () {
		if(rollBackTimer > 0)
        {
            rollBackTimer -= Time.deltaTime;
            RollBack();
        }
	}

    private void OnMouseDown()
    {
        CancelRollBack();
        var dir = Camera.main.WorldToScreenPoint(transform.position);
        dir = Input.mousePosition - dir;
        wheelAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        wheelAngle -= Mathf.Atan2(transform.right.y, transform.right.x) * Mathf.Rad2Deg;
    }

    private void OnMouseUp()
    {
        Debug.Log("Starting rollback");
        rollBackTimer = rollBackTimerStartValue;
    }

    private void OnMouseDrag()
    {
        Vector3 dir = Camera.main.WorldToScreenPoint(transform.position);
        dir = Input.mousePosition - dir;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - wheelAngle;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    private void RollBack()
    {
        wheelAngle = Mathf.Lerp(0, wheelAngle, rollBackTimer / rollBackTimerStartValue);
        transform.rotation = Quaternion.AngleAxis(wheelAngle, Vector3.forward);
    }

    private void CancelRollBack()
    {
        rollBackTimer = 0;
    }
}
