using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeBody : MonoBehaviour
{
    Camera cam;
    float camWidth, camHeight, deviceHeight, deviceWidth;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        camHeight = cam.orthographicSize;
        camWidth = cam.orthographicSize * cam.aspect;
        deviceHeight = Display.main.systemHeight;
        deviceWidth = Display.main.systemWidth;

        float headerSizeCam = camWidth * 2f / 5f;
        gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(
            deviceWidth * 5, 
            deviceHeight - deviceWidth / 5 * 2
        );
        Debug.Log(transform.position);
        transform.position -= new Vector3(0f, headerSizeCam, 0f);
        Debug.Log(transform.position);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void MoveTo(string pageName)
    {
        float pos = 0;
        if (pageName == "Store")
        {
            pos = camWidth * 2 * 2;
        }
        else if (pageName == "Events")
        {
            pos = camWidth * 2;
        }
        else if (pageName == "Home")
        {
            pos = 0f;
        }
        else if (pageName == "Challenges")
        {
            pos = -camWidth * 2;
        }
        else if (pageName == "Daily")
        {
            pos = -camWidth * 2 * 2;
        }

        StartCoroutine(SmoothMove(
            transform.position,
            new Vector3(pos, transform.position.y, transform.position.z),
            0.2f
        ));
    }

    IEnumerator SmoothMove(Vector3 pos1, Vector3 pos2, float duration)
    {
        for (float t = 0f; t < duration; t += Time.deltaTime)
        {
            transform.position = Vector3.Lerp(pos1, pos2, t / duration);
            yield return 0;
        }
        transform.position = pos2;
    }
}
