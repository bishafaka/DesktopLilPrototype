using UnityEngine;

public class CanvasManager : MonoBehaviour
{
//INVENTORY
	bool isInventoryOpen=false;
	const string OPEN_TOGGLE="Open";
	const string CLOSE_TOGGLE="Close";

	public void EnablePanel(GameObject PanelToEnable)
	{
		if(!PanelToEnable.activeSelf)
			PanelToEnable.SetActive(true);
		PanelToEnable.transform.localPosition=Vector2.zero;
	}
	public void DisablePanel(GameObject PanelToDisable) => PanelToDisable.SetActive(false);

//INVENTORY
	public void ToggleInventory(Animator _Animator)
	{
		if(isInventoryOpen)
			_Animator.SetTrigger(CLOSE_TOGGLE);
		else
			_Animator.SetTrigger(OPEN_TOGGLE);
		isInventoryOpen=!isInventoryOpen;
	}
}
