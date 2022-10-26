using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HomePanel : MonoBehaviour
{
    int numberOfElement;
    GridLayoutGroup gridLayoutGroup;
    float spacing, cellSize;
    // Start is called before the first frame update
    void Start()
    {
        Camera cam = Camera.main;

        numberOfElement = gameObject.transform.childCount;
        gridLayoutGroup = gameObject.GetComponent<GridLayoutGroup>();
        float camHeight = cam.orthographicSize;
        float camWidth = cam.orthographicSize * cam.aspect;
        float deviceHeight = Display.main.systemHeight;
        float deviceWidth = Display.main.systemWidth;

        spacing = deviceWidth / 20;
        cellSize = deviceWidth/2 - spacing - spacing/2;

        gridLayoutGroup.cellSize = new Vector2(cellSize, cellSize);
        gridLayoutGroup.spacing = new Vector2(spacing, spacing);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
