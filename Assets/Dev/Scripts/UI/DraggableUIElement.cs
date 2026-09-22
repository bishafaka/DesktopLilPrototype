using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(RectTransform))]
public class DraggableUIElement : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
	[SerializeField] RectTransform elementToDrag;
	[SerializeField] bool disableOnStart=false;
	[SerializeField] bool putAsLastSibling=true;
	Canvas canvas;
	RectTransform canvasRect;
	Vector2 dragOffset;

	void Awake()
	{
		canvas=GetComponentInParent<Canvas>();
		if(canvas!=null)
			canvasRect=canvas.GetComponent<RectTransform>();
		if(elementToDrag==null)
			elementToDrag=GetComponent<RectTransform>();
		if(disableOnStart)
			gameObject.SetActive(false);
	}
    void OnEnable()
    {
        if (putAsLastSibling)
            elementToDrag.SetAsLastSibling();
    }
    void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
	{
		if(canvas==null || canvasRect==null || elementToDrag==null)
			return;
		if(putAsLastSibling)
			elementToDrag.SetAsLastSibling();
		Vector2 localMousePosition;
		if(RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, eventData.position, eventData.pressEventCamera, out localMousePosition))
			dragOffset=elementToDrag.anchoredPosition-localMousePosition;
	}
	void IDragHandler.OnDrag(PointerEventData eventData)
	{
		if(canvas==null || canvasRect==null || elementToDrag==null)
			return;
		Vector2 localMousePosition;
		if(!RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, eventData.position, eventData.pressEventCamera, out localMousePosition))
			return;
		Vector2 position=localMousePosition+dragOffset;
		Vector2 elementSize=elementToDrag.rect.size;
		float minX=canvasRect.rect.xMin+elementSize.x*elementToDrag.pivot.x;
		float maxX=canvasRect.rect.xMax-elementSize.x*(1f-elementToDrag.pivot.x);
		float minY=canvasRect.rect.yMin+elementSize.y*elementToDrag.pivot.y;
		float maxY=canvasRect.rect.yMax-elementSize.y*(1f-elementToDrag.pivot.y);
		position.x=Mathf.Clamp(position.x, minX, maxX);
		position.y=Mathf.Clamp(position.y, minY, maxY);
		elementToDrag.anchoredPosition=position;
	}
	void IEndDragHandler.OnEndDrag(PointerEventData eventData)
	{
		Debug.Log("EndDrag");
	}
}
