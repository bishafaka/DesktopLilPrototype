using UnityEngine;

public class CanvasManager : MonoBehaviour
{
//INVENTORY
	bool isInventoryOpen=false;
	const string OPEN_TOGGLE="Open";
	const string CLOSE_TOGGLE="Close";
//JOURNAL
	[SerializeField] GameObject journalPanel;

	public void EnablePanel(GameObject PanelToEnable)
	{
		if(!PanelToEnable.activeSelf)
			PanelToEnable.SetActive(true);
		PanelToEnable.transform.localPosition=Vector2.zero;
	}
	public void DisablePanel(GameObject PanelToDisable) => PanelToDisable.SetActive(false);
	public void OpenURL(string URL) => Application.OpenURL(URL);
	public void QuitGame() => Application.Quit();

//INVENTORY
	public void ToggleInventory(Animator _Animator)
	{
		if(isInventoryOpen)
			_Animator.SetTrigger(CLOSE_TOGGLE);
		else
			_Animator.SetTrigger(OPEN_TOGGLE);
		isInventoryOpen=!isInventoryOpen;
	}
//JOURNAL
	public void SetKittyEntry(Kitty _Kitty)
	{
		Entry entry=journalPanel.GetComponent<Entry>();
		if(entry!=null)
			entry.SetEntry(_Kitty);
	}
}
