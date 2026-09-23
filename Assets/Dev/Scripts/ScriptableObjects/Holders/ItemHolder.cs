using UnityEngine;
using UnityEngine.UI;

public class ItemHolder : MonoBehaviour
{
    [SerializeField] Item item;
    [SerializeField] Image image;
    public void SetItem(Item newItem)
    {
        item=newItem;
        image.sprite=item.itemIcon;
    }
}
