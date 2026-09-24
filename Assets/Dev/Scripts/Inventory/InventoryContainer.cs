using UnityEngine;
public class InventoryContainer : MonoBehaviour
{
	public GameObject inventoryItemPrefab;
	public InventorySlot[] InventorySlots;
	public void AddItem(ItemHolder item)
	{
		for(int i=0; i<InventorySlots.Length; i++)
		{
			InventorySlot slot=InventorySlots[i];
			InventoryItem itemInSlot=slot.GetComponentInChildren<InventoryItem>();
			if(itemInSlot==null)
			{
				SpawnNewItem(item.GetItem(), slot);
				return;
			}
		}
		//return false;
	}
	void SpawnNewItem(Item item, InventorySlot slot)
	{
		GameObject newItemGO=Instantiate(inventoryItemPrefab, slot.transform);
		InventoryItem inventoryItem=newItemGO.GetComponent<InventoryItem>();
		inventoryItem.InitItem(item);
	}
}
