using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Views.Animations;

namespace SafeTraveling
{
    [Activity(ScreenOrientation = Android.Content.PM.ScreenOrientation.Landscape)]
    public class Organizator_Trip_NewQuestionPool : Activity
    {
        RelativeLayout IntroducereIntrebare;
        RelativeLayout SetareVariante;
        RelativeLayout SetareDurata;
        FrameLayout Container;

        Organizator_Trip_NewQuestionPool_IntroducereIntrebare IntroducereIntrebare_Fragment;
        Organizator_Trip_NewQuestionPool_SetareVariante SetareVariante_Fragment;
        Organizator_Trip_NewQuestionPool_SetareDurata SetareDurata_Fragment;

        protected override void OnCreate(Bundle bundle)
        {
            RequestWindowFeature(WindowFeatures.NoTitle);
            Window.SetFlags(WindowManagerFlags.Fullscreen, WindowManagerFlags.Fullscreen);

            base.OnCreate(bundle);

            SetContentView(Resource.Layout.Organizator_Trip_NewQuestionPool);

            IntroducereIntrebare = FindViewById<RelativeLayout>(Resource.Id.relativeLayout2);
            SetareVariante = FindViewById<RelativeLayout>(Resource.Id.relativeLayout3);
            SetareDurata = FindViewById<RelativeLayout>(Resource.Id.relativeLayout4);
            Container = FindViewById<FrameLayout>(Resource.Id.frameLayout1);
            TextView TView_IntroducereIntrebare = FindViewById<TextView>(Resource.Id.textView1);
            TextView TView_SetareVariante = FindViewById<TextView>(Resource.Id.textView2);
            TextView TView_SetareDurata = FindViewById<TextView>(Resource.Id.textView3);

            SetTypeface.Normal.SetTypeFace(this, TView_IntroducereIntrebare);
            SetTypeface.Normal.SetTypeFace(this, TView_SetareVariante);
            SetTypeface.Normal.SetTypeFace(this, TView_SetareDurata);

            Animation Slide_Speed1 = AnimationUtils.LoadAnimation(this, Resource.Animation.SlideRightToLeft_Speed1);
            Animation Slide_Speed2 = AnimationUtils.LoadAnimation(this, Resource.Animation.SlideRightToLeft_Speed2);
            Animation Slide_Speed3 = AnimationUtils.LoadAnimation(this, Resource.Animation.SlideRightToLeft_Speed3);

            IntroducereIntrebare.StartAnimation(Slide_Speed1);
            SetareVariante.StartAnimation(Slide_Speed2);
            SetareDurata.StartAnimation(Slide_Speed3);

            IntroducereIntrebare.Click += IntroducereIntrebare_Click;
            SetareVariante.Click += SetareVariante_Click;
            SetareDurata.Click += SetareDurata_Click;

            IntroducereIntrebare_Fragment = new Organizator_Trip_NewQuestionPool_IntroducereIntrebare();
            SetareVariante_Fragment =new Organizator_Trip_NewQuestionPool_SetareVariante();
            SetareDurata_Fragment = new Organizator_Trip_NewQuestionPool_SetareDurata(IntroducereIntrebare_Fragment,SetareVariante_Fragment);

            FragmentTransaction transact = FragmentManager.BeginTransaction();
            transact.Add(Container.Id, IntroducereIntrebare_Fragment, "IntroducereIntrebare");
            transact.Add(Container.Id, SetareVariante_Fragment, "SetareVariante");
            transact.Add(Container.Id, SetareDurata_Fragment, "SetareDurata");
            transact.Hide(IntroducereIntrebare_Fragment);
            transact.Hide(SetareVariante_Fragment);
            transact.Hide(SetareDurata_Fragment);
            transact.Commit();
        }

        void IntroducereIntrebare_Click(object sender, EventArgs e)
        {
            ImageView CancelBtn = FindViewById<ImageView>(Resource.Id.imageView1);

            if (!CancelBtn.Visibility.Equals(ViewStates.Visible))
            {
                Window.SetFlags(WindowManagerFlags.NotTouchable, WindowManagerFlags.NotTouchable);

                ShowFragment(IntroducereIntrebare_Fragment);

                int[] Position = new int[2];
                IntroducereIntrebare.GetLocationOnScreen(Position);

                AlphaAnimation alphaAnim = new AlphaAnimation(0.0f, 1.0f);
                alphaAnim.Duration = 1000;

                alphaAnim.AnimationEnd += ((object sender1, Animation.AnimationEndEventArgs e1) =>
                {
                    RelativeLayout.LayoutParams parameters = new RelativeLayout.LayoutParams(RelativeLayout.LayoutParams.MatchParent, RelativeLayout.LayoutParams.MatchParent);
                    parameters.AddRule(LayoutRules.Below, Resource.Id.relativeLayout2);
                    parameters.SetMargins(40, 0, 40, 40);

                    Container.LayoutParameters = parameters;

                    Animation ScaleAnim = AnimationUtils.LoadAnimation(this, Resource.Animation.ScaleOutAnim);
                    ScaleAnim.AnimationEnd += ((object sender2, Animation.AnimationEndEventArgs e2) =>
                    {
                        Window.ClearFlags(WindowManagerFlags.NotTouchable);
                    });

                    Container.StartAnimation(ScaleAnim);
                });

                SetareDurata.Visibility = ViewStates.Gone;
                SetareVariante.Visibility = ViewStates.Gone;

                CancelBtn.StartAnimation(alphaAnim);

                CancelBtn.Visibility = ViewStates.Visible;
            }

            if (!CancelBtn.HasOnClickListeners)
                CancelBtn.Click += ((object sender1, EventArgs e1) =>
                {
                    Window.SetFlags(WindowManagerFlags.NotTouchable, WindowManagerFlags.NotTouchable);
                    
                    Animation ScaleAnim = AnimationUtils.LoadAnimation(this, Resource.Animation.ScaleInAnim);
                    ScaleAnim.FillAfter = true;
                    Container.StartAnimation(ScaleAnim);

                    ScaleAnim.AnimationEnd += ((object sender2, Animation.AnimationEndEventArgs e2) =>
                    {
                        SetareDurata.Visibility = ViewStates.Visible;
                        SetareVariante.Visibility = ViewStates.Visible;

                        RelativeLayout.LayoutParams parameters = new RelativeLayout.LayoutParams(0, 0);
                        Container.LayoutParameters = parameters;

                        CancelBtn.Visibility = ViewStates.Gone;
                        Window.ClearFlags(WindowManagerFlags.NotTouchable);
                    });
                });
        }

        void SetareVariante_Click(object sender, EventArgs e)
        {
            ImageView CancelBtn = FindViewById<ImageView>(Resource.Id.imageView2);

            if (!CancelBtn.Visibility.Equals(ViewStates.Visible))
            {
                Window.SetFlags(WindowManagerFlags.NotTouchable, WindowManagerFlags.NotTouchable);

                ShowFragment(SetareVariante_Fragment);

                int[] Position = new int[2];
                SetareVariante.GetLocationOnScreen(Position);

                AlphaAnimation alphaAnim = new AlphaAnimation(0.0f, 1.0f);
                alphaAnim.Duration = 1000;

                alphaAnim.AnimationEnd += ((object sender1, Animation.AnimationEndEventArgs e1) =>
                {
                    RelativeLayout.LayoutParams parameters = new RelativeLayout.LayoutParams(RelativeLayout.LayoutParams.MatchParent, RelativeLayout.LayoutParams.MatchParent);
                    parameters.AddRule(LayoutRules.Below, Resource.Id.relativeLayout3);
                    parameters.SetMargins(40, 0, 40, 40);

                    Container.LayoutParameters = parameters;

                    Animation ScaleAnim = AnimationUtils.LoadAnimation(this, Resource.Animation.ScaleOutAnim);
                    ScaleAnim.AnimationEnd += ((object sender2, Animation.AnimationEndEventArgs e2) =>
                    {
                        Window.ClearFlags(WindowManagerFlags.NotTouchable);
                    });

                    Container.StartAnimation(ScaleAnim);
                });

                IntroducereIntrebare.Visibility = ViewStates.Gone;
                SetareDurata.Visibility = ViewStates.Gone;

                CancelBtn.StartAnimation(alphaAnim);

                CancelBtn.Visibility = ViewStates.Visible;
            }

            if (!CancelBtn.HasOnClickListeners)
                CancelBtn.Click += ((object sender1, EventArgs e1) =>
                {
                    Window.SetFlags(WindowManagerFlags.NotTouchable, WindowManagerFlags.NotTouchable);

                    Animation ScaleAnim = AnimationUtils.LoadAnimation(this, Resource.Animation.ScaleInAnim);
                    ScaleAnim.FillAfter = true;
                    Container.StartAnimation(ScaleAnim);

                    ScaleAnim.AnimationEnd += ((object sender2, Animation.AnimationEndEventArgs e2) =>
                    {
                        IntroducereIntrebare.Visibility = ViewStates.Visible;
                        SetareDurata.Visibility = ViewStates.Visible;

                        RelativeLayout.LayoutParams parameters = new RelativeLayout.LayoutParams(0, 0);
                        Container.LayoutParameters = parameters;

                        CancelBtn.Visibility = ViewStates.Gone;
                        Window.ClearFlags(WindowManagerFlags.NotTouchable);
                    });
                });
        }

        void SetareDurata_Click(object sender, EventArgs e)
        {
            ImageView CancelBtn = FindViewById<ImageView>(Resource.Id.imageView3);

            if (!CancelBtn.Visibility.Equals(ViewStates.Visible))
            {
                Window.SetFlags(WindowManagerFlags.NotTouchable, WindowManagerFlags.NotTouchable);

                ShowFragment(SetareDurata_Fragment);

                int[] Position = new int[2];
                SetareDurata.GetLocationOnScreen(Position);

                AlphaAnimation alphaAnim = new AlphaAnimation(0.0f, 1.0f);
                alphaAnim.Duration = 1000;

                alphaAnim.AnimationEnd += ((object sender1, Animation.AnimationEndEventArgs e1) =>
                {
                    RelativeLayout.LayoutParams parameters = new RelativeLayout.LayoutParams(RelativeLayout.LayoutParams.MatchParent, RelativeLayout.LayoutParams.MatchParent);
                    parameters.AddRule(LayoutRules.Below, Resource.Id.relativeLayout4);
                    parameters.SetMargins(40, 0, 40, 40);

                    Container.LayoutParameters = parameters;

                    Animation ScaleAnim = AnimationUtils.LoadAnimation(this, Resource.Animation.ScaleOutAnim);
                    ScaleAnim.AnimationEnd += ((object sender2, Animation.AnimationEndEventArgs e2) =>
                        {
                            Window.ClearFlags(WindowManagerFlags.NotTouchable);
                        });

                    Container.StartAnimation(ScaleAnim);
                });

                IntroducereIntrebare.Visibility = ViewStates.Gone;
                SetareVariante.Visibility = ViewStates.Gone;

                CancelBtn.StartAnimation(alphaAnim);

                CancelBtn.Visibility = ViewStates.Visible;
            }

            if (!CancelBtn.HasOnClickListeners)
                CancelBtn.Click += ((object sender1, EventArgs e1) =>
                {
                    Window.SetFlags(WindowManagerFlags.NotTouchable, WindowManagerFlags.NotTouchable);

                    Animation ScaleAnim = AnimationUtils.LoadAnimation(this, Resource.Animation.ScaleInAnim);
                    ScaleAnim.FillAfter = true;
                    Container.StartAnimation(ScaleAnim);

                    ScaleAnim.AnimationEnd += ((object sender2, Animation.AnimationEndEventArgs e2) =>
                    {
                        IntroducereIntrebare.Visibility = ViewStates.Visible;
                        SetareVariante.Visibility = ViewStates.Visible;

                        RelativeLayout.LayoutParams parameters = new RelativeLayout.LayoutParams(0, 0);
                        Container.LayoutParameters = parameters;

                        CancelBtn.Visibility = ViewStates.Gone;
                        Window.ClearFlags(WindowManagerFlags.NotTouchable);
                    });
                });
        }

        void ShowFragment(Fragment frag)
        {
            FragmentTransaction transact = FragmentManager.BeginTransaction();

            transact.Hide(IntroducereIntrebare_Fragment);
            transact.Hide(SetareVariante_Fragment);
            transact.Hide(SetareDurata_Fragment);

            transact.Show(frag);
            
            transact.Commit();
        }
    }
}

