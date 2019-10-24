using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds;
using GoogleMobileAds.Api;

public class AdmobScript : MonoBehaviour {

    private BannerView bannerView;
    private AdRequest request;

    // Use this for initialization
    void Start() {
        string adUnitId = "ad_unit_id";
        bannerView = new BannerView(adUnitId, AdSize.Banner, AdPosition.Top);

        // Create an empty ad request.
        request = new AdRequest.Builder().Build();

        
    }

    public void showBanner()
    {
        bannerView.LoadAd(request);
    }

    public void hideBanner()
    {
        bannerView.Hide();
       // bannerView.Destroy();
    }

}
