package md5f9b7691a3024e3da489938deb6fbfb67;


public class JavaHolder
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"";
		mono.android.Runtime.register ("ReactiveUI.JavaHolder, ReactiveUI, Version=7.1.0.0, Culture=neutral, PublicKeyToken=null", JavaHolder.class, __md_methods);
	}


	public JavaHolder () throws java.lang.Throwable
	{
		super ();
		if (getClass () == JavaHolder.class)
			mono.android.TypeManager.Activate ("ReactiveUI.JavaHolder, ReactiveUI, Version=7.1.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}

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
