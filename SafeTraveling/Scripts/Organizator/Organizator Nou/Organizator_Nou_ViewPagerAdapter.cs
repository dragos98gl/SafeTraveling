using Android.Support.V4.App;
using Android.Views;
using SafeTraveling;
using Android.Widget;
using Android.App;


class Organizator_Nou_ViewPagerAdapter:FragmentStatePagerAdapter
{
	int LayoutsCount = 1;
	Activity context;

	public Organizator_Nou_ViewPagerAdapter(Android.Support.V4.App.FragmentManager fm,Activity context):base(fm)
	{
		this.context = context;
	}

	public override int Count {
		get {
			return LayoutsCount;
		}
	}

	public override Android.Support.V4.App.Fragment GetItem (int position)
	{
		return new Organizator_Nou_ViewPagerFragment (position,context);
	}
}

class Organizator_Nou_ViewPagerFragment:Android.Support.V4.App.Fragment
{
	int[] Layouturi = new int[] {
		Resource.Layout.Organizator_Nou_Page1
	};

	int position;
	Activity context;

	public override void OnCreate (Android.OS.Bundle savedInstanceState)
	{
		base.OnCreate (savedInstanceState);
	}

	public Organizator_Nou_ViewPagerFragment(int position,Activity context)
	{
		this.position = position;
		this.context = context;
	}

	public override Android.Views.View OnCreateView (Android.Views.LayoutInflater inflater, Android.Views.ViewGroup container, Android.OS.Bundle savedInstanceState)
	{
		View v = inflater.Inflate (Layouturi[position],null,true);

		switch (position) {
		case 0:
			{
				Button Finalizare = v.FindViewById <Button> (Resource.Id.button1);

				Finalizare.Click += (object sender, System.EventArgs e) => {
					context.StartActivity (typeof(Organizator_Main));
				};
			}
			break;
		}

		return v;
	}
}