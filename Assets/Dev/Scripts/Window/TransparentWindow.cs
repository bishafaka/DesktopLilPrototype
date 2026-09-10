#if UNITY_STANDALONE_WIN && !UNITY_EDITOR
using System;
using System.Runtime.InteropServices;
#endif
using UnityEngine;

public class TransparentWindow : MonoBehaviour
{
#if UNITY_STANDALONE_WIN && !UNITY_EDITOR
	[StructLayout(LayoutKind.Sequential)]
	struct POINT
	{
		public int x;
		public int y;
	}
	[StructLayout(LayoutKind.Sequential)]
	struct SIZE
	{
		public int cx;
		public int cy;
	}
	[StructLayout(LayoutKind.Sequential, Pack=1)]
	struct BLENDFUNCTION
	{
		public byte BlendOp;
		public byte BlendFlags;
		public byte SourceConstantAlpha;
		public byte AlphaFormat;
	}
	[DllImport("user32.dll")]
	static extern IntPtr GetActiveWindow();
	[DllImport("user32.dll", SetLastError=true)]
	static extern int GetWindowLong(IntPtr hWnd, int nIndex);
	[DllImport("user32.dll", SetLastError=true)]
	static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);
	[DllImport("user32.dll", SetLastError=true)]
	static extern bool UpdateLayeredWindow(IntPtr hWnd, IntPtr hdcDst, ref POINT pptDst, ref SIZE psize, IntPtr hdcSrc, ref POINT pptSrc, uint crKey, ref BLENDFUNCTION pblend, uint dwFlags);
	[DllImport("user32.dll")]
	static extern IntPtr GetDC(IntPtr hWnd);
	[DllImport("user32.dll")]
	static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);
	[DllImport("gdi32.dll")]
	static extern IntPtr CreateCompatibleDC(IntPtr hdc);
	[DllImport("gdi32.dll")]
	static extern bool DeleteDC(IntPtr hdc);
	[DllImport("gdi32.dll")]
	static extern IntPtr CreateDIBSection(IntPtr hdc, ref BITMAPINFO pbmi, uint usage, out IntPtr ppvBits, IntPtr hSection, uint offset);
	[DllImport("gdi32.dll")]
	static extern IntPtr SelectObject(IntPtr hdc, IntPtr hgdiobj);
	[DllImport("gdi32.dll")]
	static extern bool DeleteObject(IntPtr hObject);
	[StructLayout(LayoutKind.Sequential)]
	struct BITMAPINFO
	{
		public BITMAPINFOHEADER bmiHeader;
		public uint bmiColors;
	}
	[StructLayout(LayoutKind.Sequential)]
	struct BITMAPINFOHEADER
	{
		public uint biSize;
		public int biWidth;
		public int biHeight;
		public ushort biPlanes;
		public ushort biBitCount;
		public uint biCompression;
		public uint biSizeImage;
		public int biXPelsPerMeter;
		public int biYPelsPerMeter;
		public uint biClrUsed;
		public uint biClrImportant;
	}
	[DllImport("user32.dll")]
	static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X,	int Y, int cx, int cy, uint uFlags);
	static readonly IntPtr HWND_TOPMOST=new IntPtr(-1);

	const uint SWP_NOSIZE=0x0001;
	const uint SWP_NOMOVE=0x0002;
	const uint SWP_SHOWWINDOW=0x0040;
	const int GWL_STYLE=-16;
	const int GWL_EXSTYLE=-20;
	const int WS_POPUP=unchecked((int)0x80000000);
	const int WS_VISIBLE=0x10000000;
	const int WS_EX_LAYERED=0x00080000;
	const uint BI_RGB=0;
	const uint DIB_RGB_COLORS=0;
	const byte AC_SRC_OVER=0;
	const byte AC_SRC_ALPHA=1;
	const uint ULW_ALPHA=2;

	IntPtr hwnd;
	IntPtr screenDC;
	IntPtr memDC;
	IntPtr bitmap;
	IntPtr oldBitmap;
	IntPtr bitmapBits;
	Texture2D texture;
	RenderTexture renderTexture;
	int width;
	int height;
#endif
	void Start()
	{
#if UNITY_STANDALONE_WIN && !UNITY_EDITOR
		width=Screen.width;
		height=Screen.height;
		hwnd=GetActiveWindow();
		SetWindowPos(hwnd, HWND_TOPMOST, 0, 0, 0, 0, SWP_NOMOVE | SWP_NOSIZE | SWP_SHOWWINDOW);
		SetWindowLong(hwnd, GWL_STYLE, WS_POPUP | WS_VISIBLE);
		int exStyle=GetWindowLong(hwnd, GWL_EXSTYLE);
		SetWindowLong(hwnd, GWL_EXSTYLE, exStyle | WS_EX_LAYERED);
		renderTexture=new RenderTexture(width, height, 24, RenderTextureFormat.ARGB32);
		renderTexture.Create();
		Camera cam=Camera.main;
		cam.targetTexture=renderTexture;
		cam.clearFlags=CameraClearFlags.SolidColor;
		cam.backgroundColor=new Color(0f, 0f, 0f, 0f);
		texture=new Texture2D(width, height, TextureFormat.RGBA32, false);
		CreateBitmapWindows();
		UpdateWindow();

#endif
	}
#if UNITY_STANDALONE_WIN && !UNITY_EDITOR
	void CreateBitmapWindows()
	{
		screenDC=GetDC(IntPtr.Zero);
		memDC=CreateCompatibleDC(screenDC);
		BITMAPINFO info=new BITMAPINFO();
		info.bmiHeader.biSize=(uint)Marshal.SizeOf(typeof(BITMAPINFOHEADER));
		info.bmiHeader.biWidth=width;
		info.bmiHeader.biHeight=height;
		info.bmiHeader.biPlanes=1;
		info.bmiHeader.biBitCount=32;
		info.bmiHeader.biCompression=BI_RGB;
		bitmap=CreateDIBSection(screenDC, ref info, DIB_RGB_COLORS, out bitmapBits, IntPtr.Zero, 0);
		oldBitmap=SelectObject(memDC, bitmap);
	}
	void UpdateWindow()
	{
		RenderTexture previous=RenderTexture.active;
		RenderTexture.active=renderTexture;
		texture.ReadPixels(new Rect(0, 0, width, height), 0, 0, false);
		texture.Apply(false);
		RenderTexture.active=previous;
		byte[] pixels=texture.GetRawTextureData();
		for(int i=0; i<pixels.Length; i+=4)
		{
			byte r=pixels[i];
			pixels[i]=pixels[i+2];
			pixels[i+2]=r;
		}
		Marshal.Copy(pixels, 0, bitmapBits, pixels.Length);
		POINT destination=new POINT
		{
			x=0,
			y=0
		};
		POINT source=new POINT
		{
			x=0,
			y=0
		};
		SIZE size=new SIZE
		{
			cx=width,
			cy=height
		};
		BLENDFUNCTION blend=new BLENDFUNCTION
		{
			BlendOp=AC_SRC_OVER,
			BlendFlags=0,
			SourceConstantAlpha=255,
			AlphaFormat=AC_SRC_ALPHA
		};
		UpdateLayeredWindow(hwnd, screenDC, ref destination, ref size, memDC, ref source, 0, ref blend, ULW_ALPHA);
	}
	void LateUpdate()
	{
		UpdateWindow();
	}
	void OnDestroy()
	{
		if(oldBitmap!=IntPtr.Zero)
			SelectObject(memDC, oldBitmap);
		if(bitmap!=IntPtr.Zero)
			DeleteObject(bitmap);
		if(memDC!=IntPtr.Zero)
			DeleteDC(memDC);
		if(screenDC!=IntPtr.Zero)
			ReleaseDC(IntPtr.Zero, screenDC);
		if(renderTexture!=null)
			renderTexture.Release();
	}
#endif
}
