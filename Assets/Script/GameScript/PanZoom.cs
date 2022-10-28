using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.IO;

public class PanZoom : MonoBehaviour
{
    Vector3 touchStart;
    [SerializeField] private Camera cam;
    [SerializeField] private SpriteRenderer mapRenderer;
    private float minCamSize, maxCamSize;
    private float mapMinX, mapMaxX, mapMinY, mapMaxY;
    public string url;

    // Start is called before the first frame update
    IEnumerator Start()
    {

        //---------------------------------------------------------------------------
        ///url = Application.dataPath + "/StreamingAssets/shareImage.png";
        url = Path.Combine(Application.streamingAssetsPath, "001.png");

        byte[] imgData;
        Texture2D tex = new Texture2D(2, 2);

        //Check if we should use UnityWebRequest or File.ReadAllBytes
        if (url.Contains("://") || url.Contains(":///"))
        {
            UnityWebRequest www = UnityWebRequest.Get(url);
            yield return www.SendWebRequest();
            imgData = www.downloadHandler.data;
        }
        else
        {
            imgData = File.ReadAllBytes(url);
        }
        Debug.Log(imgData.Length);

        //Load raw Data into Texture2D 
        tex.LoadImage(imgData);

        //Convert Texture2D to Sprite
        Vector2 pivot = new Vector2(0.5f, 0.5f);
        Sprite sprite = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), pivot, 100.0f);

        //Apply Sprite to SpriteRenderer
        // SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        mapRenderer.sprite = sprite;
        //---------------------------------------------------------------------------
        Debug.Log(mapRenderer.bounds.size.x + " " + mapRenderer.bounds.size.y);
        mapMinX = mapRenderer.transform.position.x - mapRenderer.bounds.size.x / 2f;
        mapMaxX = mapRenderer.transform.position.x + mapRenderer.bounds.size.x / 2f;

        mapMinY = mapRenderer.transform.position.y - mapRenderer.bounds.size.y / 2f;
        mapMaxY = mapRenderer.transform.position.y + mapRenderer.bounds.size.y / 2f;
        // Debug.Log(mapRenderer.transform.position.x + "--" + mapRenderer.transform.position.y);
        // Debug.Log(mapRenderer.bounds.size.x + "--" + mapRenderer.bounds.size.y);

        // set maxCamSize bằng chiều cao của ảnh
        maxCamSize = mapRenderer.bounds.size.y / 2f;
        // minCamSize bằng 1/4 maxCamSize => zoom max đc to gấp 4 lần ảnh
        // minCamSize = maxCamSize / 4f;
        minCamSize = maxCamSize / 10f;

        // init camSize
        cam.orthographicSize = maxCamSize;

        // cam pos in top left
        Vector3 temp = new Vector3(-1000, 1000, -10);
        cam.transform.position = ClampCamera(cam.transform.position + temp);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            touchStart = cam.ScreenToWorldPoint(Input.mousePosition);
        }

        if (Input.touchCount == 2)
        {
            // chạm 2 ngón tay
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

            float prevMagnitude = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float currentMagnitude = (touchZero.position - touchOne.position).magnitude;

            float difference = currentMagnitude - prevMagnitude;
            zoom(difference * 0.01f);
        }
        else if (Input.GetMouseButton(0))
        {
            // chỉ cham một ngón tay
            Vector3 difference = touchStart - cam.ScreenToWorldPoint(Input.mousePosition);
            // cam.transform.position += difference;
            cam.transform.position = ClampCamera(cam.transform.position + difference);
        }
        // zoom(Input.GetAxis("Mouse ScrollWheel"));
    }

    void zoom(float increment)
    {
        cam.orthographicSize = Mathf.Clamp(cam.orthographicSize - increment, minCamSize, maxCamSize);
        cam.transform.position = ClampCamera(cam.transform.position);

        // Debug.Log("cam height " + cam.orthographicSize);
        // Debug.Log("cam width " + cam.orthographicSize * cam.aspect);
    }

    private Vector3 ClampCamera(Vector3 targetPosition)
    {
        float camHeight = cam.orthographicSize;
        float camWidth = cam.orthographicSize * cam.aspect;

        float minX = mapMinX + camWidth;
        float maxX = mapMaxX - camWidth;
        float minY = mapMinY + camHeight;
        float maxY = mapMaxY - camHeight;

        float newX = Mathf.Clamp(targetPosition.x, minX, maxX);
        float newY = Mathf.Clamp(targetPosition.y, minY, maxY);

        return new Vector3(newX, newY, targetPosition.z);
    }

    public void moveTo(int pos)
    {
        // cam.orthographicSize = maxCamSize / 4f;
        StartCoroutine(smoothCamScale(
            cam.orthographicSize, 
            maxCamSize / 4f, 
            1f
        ));
        if (pos == 1)
        {
            // cam.transform.position = new Vector3(mapMinX / 2f, mapMaxY / 2f, cam.transform.position.z);
            StartCoroutine(smoothCamTrans(
                cam.transform.position,
                new Vector3(mapMinX / 2f, mapMaxY / 2f, cam.transform.position.z),
                1f
            ));
        }
        else if (pos == 2)
        {
            // cam.transform.position = new Vector3(mapMaxX / 2f, mapMaxY / 2f, cam.transform.position.z);
            StartCoroutine(smoothCamTrans(
                cam.transform.position,
                new Vector3(mapMaxX / 2f, mapMaxY / 2f, cam.transform.position.z),
                1f
            ));
        }
        else if (pos == 3)
        {
            // cam.transform.position = new Vector3(mapMaxX / 2f, mapMinY / 2f, cam.transform.position.z);
            StartCoroutine(smoothCamTrans(
                cam.transform.position,
                new Vector3(mapMaxX / 2f, mapMinY / 2f, cam.transform.position.z),
                1f
            ));
        }
        else if (pos == 4)
        {
            // cam.transform.position = new Vector3(mapMinX / 2f, mapMinY / 2f, cam.transform.position.z);
            StartCoroutine(smoothCamTrans(
                cam.transform.position,
                new Vector3(mapMinX / 2f, mapMinY / 2f, cam.transform.position.z),
                1f
            ));
        }
    }

    IEnumerator smoothCamTrans(Vector3 pos1, Vector3 pos2, float duration)
    {
        for (float t = 0f; t < duration; t += Time.deltaTime)
        {
            transform.position = Vector3.Lerp(pos1, pos2, t / duration);
            yield return 0;
        }
        cam.transform.position = pos2;
    }

    IEnumerator smoothCamScale(float size1, float size2, float duration)
    {
        for (float t = 0f; t < duration; t += Time.deltaTime)
        {
            cam.orthographicSize = Mathf.Lerp(size1, size2, t / duration);
            yield return 0;
        }
        cam.orthographicSize = size2;
    }
}