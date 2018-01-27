using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class WarningMovementScript : MonoBehaviour
{
    public int SequenceOrder { get; set; }

    private RectTransform _rectTransform;
    // Use this for initialization
    void Start()
    {
        _rectTransform = GetComponent<RectTransform>();
    }
        
    // Update is called once per frame
    void Update()
    {
        if (gameObject.transform.position.x > SequenceOrder * _rectTransform.rect.width + _rectTransform.rect.width/2)
            gameObject.transform.Translate(Vector3.left * 5);
    }

    void OnMouseDown()
    {
    }

    public void Clicked()
    {
        Statics.Warnings.ForEach(g =>
        {
            var script = g.GetComponent<WarningMovementScript>();
            if (script.SequenceOrder > SequenceOrder)
                script.SequenceOrder--;
        });

        Statics.Warnings.Remove(gameObject);
        Destroy(gameObject);
        EventManager.CalamitiesChanged();
    }

    public void SetCalamity(Color calamity)
    {
        GetComponent<Image>().color = calamity;
    }
}
