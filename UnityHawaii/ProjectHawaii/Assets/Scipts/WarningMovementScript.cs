using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class WarningMovementScript : MonoBehaviour
{
    public int SequenceOrder { get; set; }

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
