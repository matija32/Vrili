package md5f9b7691a3024e3da489938deb6fbfb67;


public class ReactiveFragment
	extends android.app.Fragment
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onPause:()V:GetOnPauseHandler\n" +
			"n_onResume:()V:GetOnResumeHandler\n" +
			"";
		mono.android.Runtime.register ("ReactiveUI.ReactiveFragment, ReactiveUI, Version=7.1.0.0, Culture=neutral, PublicKeyToken=null", ReactiveFragment.class, __md_methods);
	}


	public ReactiveFragment () throws java.lang.Throwable
	{
		super ();
		if (getClass () == ReactiveFragment.class)
			mono.android.TypeManager.Activate ("ReactiveUI.ReactiveFragment, ReactiveUI, Version=7.1.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}


	public void onPause ()
	{
		n_onPause ();
	}

	private native void n_onPause ();


	public void onResume ()
	{
		n_onResume ();
	}

	private native void n_onResume ();

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
