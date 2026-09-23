using UnityEngine;

public class Item : ScriptableObject
{
	public string itemName;
	public Sprite itemIcon;
	public bool isItemPlaced=false;
	public Vector2 itemPos;
}
