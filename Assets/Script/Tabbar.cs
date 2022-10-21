using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tabbar : MonoBehaviour
{
    public List<TabButton> tabButtons;
    List<Vector3> rawPos;
    public Transform tabbarText, blackBlock;
    public TabButton selectedTab;

    // Start is called before the first frame update
    void Start()
    {
        // init 
        rawPos = new List<Vector3>();
        Camera cam = Camera.main;

        float camHeight = cam.orthographicSize;
        float camWidth = cam.orthographicSize * cam.aspect;
        float deviceHeight = Display.main.systemHeight;
        float deviceWidth = Display.main.systemWidth;

        int numberOfButton = tabButtons.Count;
        float btnSizeCam = camWidth * 2 / numberOfButton;
        float btnSizeDevice = deviceWidth / numberOfButton;

        // Gen Buttons of Tabbar
        float pos = -camWidth;
        foreach (TabButton tabButton in tabButtons)
        {
            tabButton.gameObject.transform.position = new Vector3(
                pos,
                tabButton.gameObject.transform.position.y,
                tabButton.gameObject.transform.position.z
            );
            tabButton.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(btnSizeDevice, btnSizeDevice);
            pos += btnSizeCam;

            rawPos.Add(tabButton.gameObject.transform.Find("SVG Image").transform.position);
        }

        // set size for black block
        blackBlock = gameObject.transform.Find("BlackBlock");
        blackBlock.transform.position = new Vector3(
            -camWidth,
            blackBlock.transform.position.y,
            blackBlock.transform.position.z
        );
        blackBlock.GetComponent<RectTransform>().sizeDelta = new Vector2(btnSizeDevice, btnSizeDevice / 5);

        // set size for text in black block
        tabbarText = gameObject.transform.Find("TabbarText");
        tabbarText.transform.position = new Vector3(
            -camWidth,
            tabbarText.transform.position.y,
            tabbarText.transform.position.z
        );
        tabbarText.GetComponent<RectTransform>().sizeDelta = new Vector2(btnSizeDevice, btnSizeDevice / 5);

        // selected tab
        // selectedTab.GetComponent<Image>().color = Color.gray;
        selectedTab.transform.Find("SVG Image").transform.position += new Vector3(0, 0.1f, 0);
        tabbarText.GetComponent<Text>().text = selectedTab.gameObject.name;
        tabbarText.transform.position = selectedTab.transform.position;
        blackBlock.transform.position = selectedTab.transform.position;
        ResetTabs();
    }

    public void OnTabExit(TabButton button)
    {
        ResetTabs();
    }

    public void OnTapSelected(TabButton button)
    {
        selectedTab = button;
        ResetTabs();
        // button.GetComponent<Image>().color = Color.gray;

        for (int i = 0; i < rawPos.Count; i++)
        {
            tabButtons[i].transform.Find("SVG Image").transform.position = rawPos[i];
        }

        // move icon up
        Transform svgImage = button.transform.Find("SVG Image");
        svgImage.transform.position += new Vector3(0, 0.1f, 0);

        // tabbarText.transform.position = button.transform.position;
        tabbarText.GetComponent<Text>().text = button.gameObject.name;
        // blackBlock.transform.position = button.transform.position;
        StartCoroutine(smoothChangePos(
            tabbarText.transform.position,
            button.transform.position,
            0.1f
        ));
    }

    public void ResetTabs()
    {
        foreach (TabButton button in tabButtons)
        {
            if (selectedTab != null && button == selectedTab)
            {
                continue;
            }
            // button.GetComponent<Image>().color = Color.white;
        }
    }

    IEnumerator smoothChangePos(Vector3 pos1, Vector3 pos2, float duration)
    {
        for(float t = 0f; t < duration; t+=Time.deltaTime){
            tabbarText.transform.position = Vector3.Lerp(pos1, pos2, t/duration);
            blackBlock.transform.position = Vector3.Lerp(pos1, pos2, t/duration);
            yield return 0;
        }
        tabbarText.transform.position = pos2;
        blackBlock.transform.position = pos2;
    }
}
