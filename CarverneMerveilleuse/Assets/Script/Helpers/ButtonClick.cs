using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;


public class ButtonClick : MonoBehaviour, IPointerClickHandler
{
    private float initialScaleX;
    private float initialScaleY;
    private float initialScaleZ;
    public ButtonBehaviors btn;

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

    public void OnPointerClick(PointerEventData eventData)
    {
        btn.enabled = false;
        StartCoroutine(Anim());
    }

    IEnumerator Anim()
    {
        float actualScaleX = transform.localScale.x;
        float actualScaleY = transform.localScale.y;
        float actualScaleZ = transform.localScale.z;
        transform.DOScale(new Vector3(initialScaleX - 0.05f, initialScaleY - 0.05f, initialScaleZ - 0.05f), 0.17f);
        yield return new WaitForSeconds(0.1f);
        transform.DOScale(new Vector3(actualScaleX, actualScaleY, actualScaleZ), 0.17f).OnComplete((() => btn.enabled = true));
    }
}
