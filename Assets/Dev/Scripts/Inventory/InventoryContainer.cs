using UnityEngine;
public class InventoryContainer : MonoBehaviour
{
	public GameObject inventoryItemPrefab;
	public InventorySlot[] InventorySlots;
	public bool AddItem(Item item)
	{
		for(int i=0; i<InventorySlots.Length; i++)
		{
			InventorySlot slot=InventorySlots[i];
			InventoryItem itemInSlot=slot.GetComponentInChildren<InventoryItem>();
			if(itemInSlot==null)
			{
				SpawnNewItem(item, slot);
				return true;
			}
		}
		return false;
	}
	void SpawnNewItem(Item item, InventorySlot slot)
	{
		GameObject newItemGO=Instantiate(inventoryItemPrefab, slot.transform);
		InventoryItem inventoryItem=newItemGO.GetComponent<InventoryItem>();
		inventoryItem.InitItem(item);
	}
}
