using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;


public class ButtonBehaviors : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private float initialScaleX;
    private float initialScaleY;
    private float initialScaleZ;

    // Start is called before the first frame update
    void Start()
    {
        initialScaleX = transform.localScale.x;
        initialScaleY = transform.localScale.y;
        initialScaleZ = transform.localScale.z;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        
        transform.DOScale(new Vector3(initialScaleX+ 0.05f, initialScaleY+ 0.05f, initialScaleZ+ 0.05f) , 0.17f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.DOScale(new Vector3(initialScaleX, initialScaleY, initialScaleZ), 0.17f);
    }
}
