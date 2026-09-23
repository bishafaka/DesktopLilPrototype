using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryItem : DraggableUIElement
{
	[SerializeField] Image image;
	[HideInInspector] public Item currentObject;
	[HideInInspector] public Transform parentOnEndDrag;
	public void InitItem(Item newObject)
	{
		currentObject=newObject;
		image.sprite=newObject.itemIcon;
	}
	public override void Awake()
	{
		base.Awake();
		if(image==null)
			image=GetComponent<Image>();
	}
	public override void OnBeginDrag(PointerEventData eventData)
	{
		image.raycastTarget=false;
		parentOnEndDrag=transform.parent;
		transform.SetParent(canvasRect);
		base.OnBeginDrag(eventData);
	}
	public override void OnDrag(PointerEventData eventData)
	{
		Vector2 localMousePosition;
		if(!RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, eventData.position, eventData.pressEventCamera, out localMousePosition))
			return;
		Vector2 position=localMousePosition+dragOffset;
		elementToDrag.anchoredPosition=position;
	}
	public override void OnEndDrag(PointerEventData eventData)
	{
		image.raycastTarget=true;
		transform.SetParent(parentOnEndDrag);
		base.OnEndDrag(eventData);
	}
}
