using UnityEngine;
using UnityEngine.InputSystem;
#if UNITY_STANDALONE_WIN && !UNITY_EDITOR
using System;
using System.Runtime.InteropServices;
#endif

public class PointerUIElement : MonoBehaviour
{
	[SerializeField] GameObject objectToEnable;
	const string ENABLE_TRIGGER="Enable";
	const string DISABLE_TRIGGER="Disable";
	Animator animator;
	RectTransform rectTransform;
	bool bIsHovered;

#if UNITY_STANDALONE_WIN && !UNITY_EDITOR
	[StructLayout(LayoutKind.Sequential)]
	struct POINT
	{
		public int x;
		public int y;
	}
	[DllImport("user32.dll")]
	static extern bool GetCursorPos(out POINT lpPoint);

#endif
	void Awake()
	{
		rectTransform=GetComponent<RectTransform>();

		if(objectToEnable!=null)
			animator=objectToEnable.GetComponent<Animator>();

		if(animator!=null)
			animator.SetTrigger(DISABLE_TRIGGER);
	}
	void Update()
	{
		if(rectTransform==null || animator==null)
			return;
		Vector2 mousePosition;

#if UNITY_STANDALONE_WIN && !UNITY_EDITOR
		if(!GetCursorPos(out POINT cursor))
			return;
		mousePosition=new Vector2(cursor.x, Screen.height-cursor.y);
#else
		if (Mouse.current==null)
			return;
		mousePosition=Mouse.current.position.ReadValue();
#endif
		bool hovered=RectTransformUtility.RectangleContainsScreenPoint(rectTransform, mousePosition, Camera.main);
		if(hovered && !bIsHovered)
		{
			bIsHovered=true;
			animator.ResetTrigger(DISABLE_TRIGGER);
			animator.SetTrigger(ENABLE_TRIGGER);
		}
		else if(!hovered && bIsHovered)
		{
			bIsHovered=false;
			animator.ResetTrigger(ENABLE_TRIGGER);
			animator.SetTrigger(DISABLE_TRIGGER);
		}
	}
	void OnDisable()
	{
		if(animator!=null)
		{
			animator.ResetTrigger(ENABLE_TRIGGER);
			animator.SetTrigger(DISABLE_TRIGGER);
		}
		bIsHovered=false;
	}
}
