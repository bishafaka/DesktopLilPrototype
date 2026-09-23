using UnityEngine;
public class InventoryContainer : MonoBehaviour
{
	public GameObject inventoryItemPrefab;
	public InventorySlot[] InventorySlots;
	public bool AddItem(Object item)
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
	void SpawnNewItem(Object item, InventorySlot slot)
	{
		GameObject newItemGO=Instantiate(inventoryItemPrefab, slot.transform);
		InventoryItem inventoryItem=newItemGO.GetComponent<InventoryItem>();
		inventoryItem.InitItem(item);
	}
}
