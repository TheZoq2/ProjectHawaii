using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MadGrid : MonoBehaviour {

    GridLayoutGroup grid;

	void Start () {
        grid = GetComponent<GridLayoutGroup>();
    }

    private void Update()
    {
        var dopeNum = (transform as RectTransform).rect.width / 2f;
        grid.cellSize = new Vector2(dopeNum, dopeNum);
    }


}
