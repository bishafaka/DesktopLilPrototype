using UnityEngine;

//[CreateAssetMenu(fileName="Object", menuName="Shop/Object")]
public class Object : ScriptableObject
{
	public string objectName;
	public string objectDiscription;
	public Sprite objectIcon;
	public bool isObjectPlaced=false;
	public Vector2 objectPosition;
}
