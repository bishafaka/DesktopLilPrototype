using UnityEngine;

[CreateAssetMenu(fileName="Kitty", menuName="Animals/Kitty")]
public class Kitty : ScriptableObject
{
	public Sprite kittyIcon;
	public string kittyName;
	[TextArea(1, 3)]
	public string kittyDescription;
	public Item[] kittyFavourites;
}
