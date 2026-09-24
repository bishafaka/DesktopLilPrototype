using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopContainer : MonoBehaviour
{
	[SerializeField] ItemHolder[] slotHolders;
	[SerializeField] Item[] items;
	Animator animator;
	int rollCount=0;
	const int MAX_ROLL_COUNT=5;
	Coroutine shuffleCoroutine;

	void Awake()
	{
		animator=GetComponent<Animator>();
	}
    void OnEnable()
    {
        StartShuffling();
    }
    void OnDisable()
    {
		StopShuffling();
    }
    public void Rolling()
	{
		rollCount++;
		if(rollCount<=MAX_ROLL_COUNT)
			return;
		animator.speed-=.1f;
		if(animator.speed<=0.0f)
		{
			StopShuffling();
			animator.SetTrigger("EndRoll");
			animator.speed=1.0f;
			rollCount=0;
		}
	}
	public void StartShuffling()
	{
		if(shuffleCoroutine!=null)
			return;
		shuffleCoroutine=StartCoroutine(ShuffleLoop());
		foreach(ItemHolder slot in slotHolders)
		{
			Button button=slot.gameObject.GetComponent<Button>();
			if(button!=null)
				button.enabled=false;
		}
    }
	public void StopShuffling()
    {
        if(shuffleCoroutine==null)
            return;
        StopCoroutine(shuffleCoroutine);
        shuffleCoroutine=null;
        foreach(ItemHolder slot in slotHolders)
        {
            Button button=slot.gameObject.GetComponent<Button>();
            if(button!=null)
                button.enabled=true;
        }
    }
    IEnumerator ShuffleLoop()
	{
		while(true)
		{
			Shuffle();
			yield return new WaitForSeconds(.1f);
		}
	}
	void Shuffle()
	{
		if(items.Length==0 || slotHolders.Length==0)
			return;
		List<Item> allItems=new List<Item>(items);
		for(int i=allItems.Count-1; i>0; i--)
		{
			int randomIndex=Random.Range(0, i+1);
			Item temp=allItems[i];
			allItems[i]=allItems[randomIndex];
			allItems[randomIndex]=temp;
		}
		for(int i=0; i<slotHolders.Length; i++)
			slotHolders[i].SetItem(allItems[i%allItems.Count]);
	}
}
