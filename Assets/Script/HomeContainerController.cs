using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeContainerController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MoveTo(string pageName){
        float pos = 0;
        if(pageName == "Home"){
            pos = 0;
        }else if(pageName == "Events"){
            pos = 4.6f;
        }else if(pageName == "Store"){
            pos = 9.2f;
        }else if(pageName == "Challenges"){
            pos = -4.6f;
        }else if(pageName == "Daily"){
            pos = -9.2f;
        }
        StartCoroutine(SmoothMove(
            gameObject.transform.position,
            new Vector3(pos, gameObject.transform.position.y, gameObject.transform.position.y),
            0.2f
        ));
    }

    IEnumerator SmoothMove(Vector3 pos1, Vector3 pos2, float duration){
        for (float t = 0f; t < duration; t += Time.deltaTime)
        {
            gameObject.transform.position = Vector3.Lerp(pos1, pos2, t / duration);
            yield return 0;
        }
        gameObject.transform.position = pos2;
    }
}
