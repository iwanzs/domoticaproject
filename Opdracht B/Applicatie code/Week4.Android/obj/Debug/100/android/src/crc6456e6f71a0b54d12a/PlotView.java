package crc6456e6f71a0b54d12a;


public class PlotView
	extends android.view.View
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onKeyDown:(ILandroid/view/KeyEvent;)Z:GetOnKeyDown_ILandroid_view_KeyEvent_Handler\n" +
			"n_onTouchEvent:(Landroid/view/MotionEvent;)Z:GetOnTouchEvent_Landroid_view_MotionEvent_Handler\n" +
			"n_onDraw:(Landroid/graphics/Canvas;)V:GetOnDraw_Landroid_graphics_Canvas_Handler\n" +
			"";
		mono.android.Runtime.register ("OxyPlot.Xamarin.Android.PlotView, OxyPlot.Xamarin.Android", PlotView.class, __md_methods);
	}


	public PlotView (android.content.Context p0)
	{
		super (p0);
		if (getClass () == PlotView.class)
			mono.android.TypeManager.Activate ("OxyPlot.Xamarin.Android.PlotView, OxyPlot.Xamarin.Android", "Android.Content.Context, Mono.Android", this, new java.lang.Object[] { p0 });
	}


	public PlotView (android.content.Context p0, android.util.AttributeSet p1)
	{
		super (p0, p1);
		if (getClass () == PlotView.class)
			mono.android.TypeManager.Activate ("OxyPlot.Xamarin.Android.PlotView, OxyPlot.Xamarin.Android", "Android.Content.Context, Mono.Android:Android.Util.IAttributeSet, Mono.Android", this, new java.lang.Object[] { p0, p1 });
	}


	public PlotView (android.content.Context p0, android.util.AttributeSet p1, int p2)
	{
		super (p0, p1, p2);
		if (getClass () == PlotView.class)
			mono.android.TypeManager.Activate ("OxyPlot.Xamarin.Android.PlotView, OxyPlot.Xamarin.Android", "Android.Content.Context, Mono.Android:Android.Util.IAttributeSet, Mono.Android:System.Int32, mscorlib", this, new java.lang.Object[] { p0, p1, p2 });
	}


	public PlotView (android.content.Context p0, android.util.AttributeSet p1, int p2, int p3)
	{
		super (p0, p1, p2, p3);
		if (getClass () == PlotView.class)
			mono.android.TypeManager.Activate ("OxyPlot.Xamarin.Android.PlotView, OxyPlot.Xamarin.Android", "Android.Content.Context, Mono.Android:Android.Util.IAttributeSet, Mono.Android:System.Int32, mscorlib:System.Int32, mscorlib", this, new java.lang.Object[] { p0, p1, p2, p3 });
	}


	public boolean onKeyDown (int p0, android.view.KeyEvent p1)
	{
		return n_onKeyDown (p0, p1);
	}

	private native boolean n_onKeyDown (int p0, android.view.KeyEvent p1);


	public boolean onTouchEvent (android.view.MotionEvent p0)
	{
		return n_onTouchEvent (p0);
	}

	private native boolean n_onTouchEvent (android.view.MotionEvent p0);


	public void onDraw (android.graphics.Canvas p0)
	{
		n_onDraw (p0);
	}

	private native void n_onDraw (android.graphics.Canvas p0);

	private java.util.ArrayList refList;
	public void monodroidAddReference (java.lang.Object obj)
	{
		if (refList == null)
			refList = new java.util.ArrayList ();
		refList.add (obj);
	}

	public void monodroidClearReferences ()
	{
		if (refList != null)
			refList.clear ();
	}
}
