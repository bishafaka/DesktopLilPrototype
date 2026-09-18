using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Entry : MonoBehaviour
{
	public Kitty entry;
	[SerializeField] Image iconEntry;
	[SerializeField] TextMeshProUGUI nameEntry;
	[SerializeField] TextMeshProUGUI descriptionEntry;
	[SerializeField] TextMeshProUGUI favouritesEntry;

#if UNITY_EDITOR
	void OnValidate()
	{
		SetEntry();
	}
#endif
	void OnEnable()
	{
		SetEntry();
	}
	public void SetEntry()
	{
		if(entry!=null)
		{
			SetName();
			SetIcon();
			SetFavourites();
			SetDescription();
		}
	}
	public void SetName() => nameEntry.text="Kitty\n"+entry.kittyName;
	public void SetDescription() => descriptionEntry.text=entry.kittyDescription;
	public void SetIcon() => iconEntry.sprite=entry.kittyIcon;
	public void SetFavourites()
	{
		string[] favouritesNames=new string[entry.kittyFavourites.Length];
		if(favouritesNames.Length>0)
		{
			for(int i=0; i<entry.kittyFavourites.Length; i++)
				favouritesNames[i]=entry.kittyFavourites[i].objectName;
			favouritesEntry.text="# ";
			for(int i=0; i<favouritesNames.Length; i++)
			{
				favouritesEntry.text+=favouritesNames[i];
				if(i<favouritesNames.Length-1)
					favouritesEntry.text+=", ";
			}
			favouritesEntry.text+=".";
		}
		else
			favouritesEntry.text="# None! Only comes to eat.";
	}
}
