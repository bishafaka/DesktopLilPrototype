using UnityEngine.EventSystems;

public class Container : DraggableUIElement
{
	public override void Awake()
	{
		base.Awake();
		gameObject.SetActive(false);
	}
	void OnEnable()
	{
		elementToDrag.SetAsLastSibling();
	}
	public override void OnBeginDrag(PointerEventData eventData)
	{
		elementToDrag.SetAsLastSibling();
		base.OnBeginDrag(eventData);
	}
}
