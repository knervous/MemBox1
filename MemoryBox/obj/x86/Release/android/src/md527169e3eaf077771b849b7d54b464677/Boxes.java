package md527169e3eaf077771b849b7d54b464677;


public class Boxes
	extends android.app.Activity
	implements
		mono.android.IGCUserPeer
{
	static final String __md_methods;
	static {
		__md_methods = 
			"n_onCreate:(Landroid/os/Bundle;)V:GetOnCreate_Landroid_os_Bundle_Handler\n" +
			"";
		mono.android.Runtime.register ("Test1.Boxes, Test1, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", Boxes.class, __md_methods);
	}


	public Boxes () throws java.lang.Throwable
	{
		super ();
		if (getClass () == Boxes.class)
			mono.android.TypeManager.Activate ("Test1.Boxes, Test1, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}


	public void onCreate (android.os.Bundle p0)
	{
		n_onCreate (p0);
	}

	private native void n_onCreate (android.os.Bundle p0);

	java.util.ArrayList refList;
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