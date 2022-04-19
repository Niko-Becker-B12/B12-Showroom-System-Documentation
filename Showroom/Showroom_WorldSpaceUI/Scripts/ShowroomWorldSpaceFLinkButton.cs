using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace Showroom
{

    public class ShowroomWorldSpaceFLinkButton : MonoBehaviour
    {

        public string fLinkUid;

        ButtonBehavior buttonBehavior;

        public List<Sprite> fLinkSprites;

        private void OnEnable()
        {

            buttonBehavior = GetComponentInChildren<ButtonBehavior>();

            if (fLinkUid != "" || !string.IsNullOrEmpty(fLinkUid))
                SetUpButton();

        }

        void SetUpButton()
        {

            string fLinkContentType = "";

            if(ShowroomSSPDataHandler.Instance != null && ShowroomSSPDataHandler.Instance.tempContentVO != null)
                fLinkContentType = ShowroomSSPDataHandler.Instance.tempContentVO.GetString("ContentType");

            Image buttonicon = buttonBehavior.transform.GetChild(0).GetComponent<Image>();


            switch (fLinkContentType)
            {

                case "":
                    buttonicon.sprite = fLinkSprites[0];
                    break;
                case "F-Link-Videoplayer":
                    buttonicon.sprite = fLinkSprites[1];
                    break;
                case "F-Link-Webplayer":
                    buttonicon.sprite = fLinkSprites[2];
                    break;
                case "F-Link-Storyapp":
                    buttonicon.sprite = fLinkSprites[3];
                    break;
                case "F-Link-Website":
                    buttonicon.sprite = fLinkSprites[4];
                    break;
                case "F-Link-Sublevel":
                    buttonicon.sprite = fLinkSprites[5];
                    break;

            }


            if (fLinkUid == "" && fLinkContentType == "")
                return;

            #region Functions

            UnityEvent onClickFLinkEvent = new UnityEvent();

            onClickFLinkEvent.AddListener(() =>
            {

                if (fLinkUid != "" || !string.IsNullOrEmpty(fLinkUid))
                {

                    if (fLinkContentType == "F-Link-Videoplayer")
                    {
                        if (ShowroomManager.Instance.showDebugMessages)
                            Debug.Log("Opening Videoplayer");

                        BrowserOverlayController.Instance.OpenWithUrl("https://d1367kmfi5z2s8.cloudfront.net/videoplayer/index.html?uid=" + fLinkUid + "&device=VCALev_0");
                    }
                    else if (fLinkContentType == "F-Link-Webplayer")
                    {
                        if (ShowroomManager.Instance.showDebugMessages)
                            Debug.Log("Opening Webplayer");

                        BrowserOverlayController.Instance.OpenWithUrl("https://d2vdrxov3sh01c.cloudfront.net/webplayer/index.html?uid=" + fLinkUid + "&device=VCALev_0");
                    }
                    else if (fLinkContentType == "F-Link-Storyapp")
                    {
                        if (ShowroomManager.Instance.showDebugMessages)
                            Debug.Log("Opening Storyapp");

                        BrowserOverlayController.Instance.OpenWithUrl("https://d2vdrxov3sh01c.cloudfront.net/storyapp/index.html?uid=" + fLinkUid + "&device=VCALev_0");
                    }
                    else if (fLinkContentType == "F-Link-Website")
                    {
                        if (ShowroomManager.Instance.showDebugMessages)
                            Debug.Log("Opening Website");

                        BrowserOverlayController.Instance.OpenWithUrl("https://d2vdrxov3sh01c.cloudfront.net/storyapp/index.html?uid=" + fLinkUid + "&device=VCALev_0");
                    }

                }

            });

            Function onClickFLinkEventFunction = new Function
            {
                functionName = onClickFLinkEvent,
                functionDelay = 0f
            };

            buttonBehavior.onMouseDown.Add(onClickFLinkEventFunction);

            #endregion

        }

    }

}