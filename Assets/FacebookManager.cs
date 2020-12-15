using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Facebook.Unity;

public class FacebookManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        if (!FB.IsInitialized)
        {
            FB.Init(() =>
            {
                if (FB.IsInitialized)
                    FB.ActivateApp();
            },
            isGameShown =>
            {
                if (!isGameShown)
                    Time.timeScale = 0;
                else
                    Time.timeScale = 1;
            });
        }
        else
            FB.ActivateApp();
    }

    public void OnSharing()
    {
        // FB.ShareLink(new System.Uri("http://google.vn"), "TESTING TITLE", "TESTING DESCRIPTION");
        FB.FeedShare("", new System.Uri("http://google.vn"), "TESTING_LINK_NAME", "TESTING_CAPTION", "TESTING_LINK_DESCRIPTION");
    }

    protected void HandleResult(IResult result)
    {
        if (result == null)
        {
            // LogView.AddLog("Null Response\n");
            return;
        }

        // Some platforms return the empty string instead of null.
        if (!string.IsNullOrEmpty(result.Error))
        {
            // handle error case here.
        }
        else if (result.Cancelled)
        {
            // a dialog was cancelled.
        }
        else if (!string.IsNullOrEmpty(result.RawResult))
        {
            // success case! Do something useful with this.
        }
        else
        {
            // we got an empty response
        }
    }
}
