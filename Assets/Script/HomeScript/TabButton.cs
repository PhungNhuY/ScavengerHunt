using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

// [RequireComponent(typeof(Image))]
public class TabButton : MonoBehaviour, IPointerClickHandler, IPointerExitHandler 
{
    public Tabbar tabbar;
    public HomeBody homeBody;

    public void OnPointerClick(PointerEventData eventData){
        tabbar.OnTapSelected(this);
        homeBody.MoveTo(gameObject.name);
    }

    public void OnPointerExit(PointerEventData eventData){
        tabbar.OnTabExit(this);
    }
}
