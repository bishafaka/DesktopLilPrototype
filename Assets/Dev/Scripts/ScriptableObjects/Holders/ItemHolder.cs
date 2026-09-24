using UnityEngine;
using UnityEngine.UI;

public class ItemHolder : MonoBehaviour
{
	[SerializeField] Item item;
	[SerializeField] Image image;
	public Item GetItem()
	{
		return item;
	}
	public void SetItem(Item newItem)
	{
		item=newItem;
		image.sprite=item.itemIcon;
	}
}
