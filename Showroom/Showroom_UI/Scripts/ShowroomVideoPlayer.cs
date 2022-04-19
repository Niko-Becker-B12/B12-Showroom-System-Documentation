using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Video;
using DG.Tweening;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Showroom
{

    public class ShowroomVideoPlayer : MonoBehaviour
    {

        public ButtonBehavior playButtonRect;
        public ButtonBehavior pauseButtonRect;
        public ButtonBehavior restartButtonRect;
        public ButtonBehavior toggleSoundButtonRect;
        public ButtonBehavior toggleLayerModeButtonRect;
        public ButtonBehavior toggleSideModeButtonRect;

        public Slider sideBySideSlider;

        public RectTransform controlbarRect;
        public RectTransform layerPanelRect;
        public RectTransform sideBySidePanelRect;

        public Transform layerButtonParent;

        public CanvasGroup sideBySidePanelCanvasGroup;

        public CanvasGroup videoPlayerCanvasGroup;

        public TextMeshProUGUI timeRemainingText;

        public Slider timelineSlider;

        public List<ShowroomVideoLayer> videoClips = new List<ShowroomVideoLayer>();

        public List<VideoPlayer> videoPlayers = new List<VideoPlayer>();

        public GameObject videoPlayerPrefab;

        public Transform videoPlayerParent;

        public bool isOpen;

        public List<Function> onPlayFunctions = new List<Function>();
        public List<Function> onPauseFunctions = new List<Function>();
        public List<Function> onRestartFunctions = new List<Function>();

        public List<Function> onEndFunctions = new List<Function>();

        public Slider volumeSlider;

        string timeRemainingString;
        float currentTimeMin;
        float currentTimeSec;

        float clipLengthMin;
        float clipLengthSec;

        float controlbarTimer = 0f;

        bool isMuted;

        bool isPlaying;

        public List<Sprite> soundButtonSprites = new List<Sprite>();
        public List<Sprite> layerButtonSprites = new List<Sprite>();

        public InputActionReference mousePointerRef;

        bool controlbarVisible = true;

        bool layerPanelVisible = false;

        bool layerWasPreviouslyOpen = false;

        bool sideBySideModeActive = false;

        public GameObject videoLayerButtonPrefab;

        List<GameObject> layerButtons = new List<GameObject>();


        private void Start()
        {
            SetUpVideos();
            SetUpButtons();
            isOpen = true;
            //SpawnLayerButtons();






            SetUpControls();

        }

        void SetUpControls()
        {

            mousePointerRef.action.performed += MousePointerMoved;

        }

        private void MousePointerMoved(InputAction.CallbackContext obj)
        {

            controlbarTimer = 0f;

            if (!controlbarVisible)
            {

                controlbarTimer = 0f;

                MoveControlbarOnScreen();

                if (layerWasPreviouslyOpen)
                    MoveLayerPanelOnScreen();

            }

        }

        private void Update()
        {

            if(isOpen)
                GenerateTimeRemainingText();

            if (isPlaying)
            {

                timelineSlider.value = (float)videoPlayers[0].time;

                controlbarTimer += Time.deltaTime;

                if (controlbarTimer > 5f && controlbarVisible)
                {

                    MoveControlbarOffScreen();

                    if (layerPanelVisible)
                    {

                        MoveLayerPanelOffScreen();

                        layerWasPreviouslyOpen = true;

                    }
                        

                }

            }
            else
            {

                controlbarTimer = 0f;

                MoveControlbarOnScreen();

                if (!layerPanelVisible && layerWasPreviouslyOpen)
                    MoveLayerPanelOnScreen();

            }



            if (videoPlayers[0].frame == (int)videoPlayers[0].frameCount - 5)
            {

                isPlaying = false;

                timelineSlider.value = timelineSlider.maxValue;

                pauseButtonRect.gameObject.SetActive(false);
                playButtonRect.gameObject.SetActive(false);
                restartButtonRect.gameObject.SetActive(true);

                for(int i = 0; i < videoPlayers.Count; i++)
                {

                    videoPlayers[i].Stop();

                }

            }

        }

        void GenerateTimeRemainingText()
        {

            currentTimeMin = Mathf.FloorToInt((float)videoPlayers[0].time / 60);
            currentTimeSec = Mathf.FloorToInt((float)videoPlayers[0].time % 60);

            timeRemainingString = string.Format("{0:00}:{1:00} | {2:00}:{3:00}", currentTimeMin, currentTimeSec, clipLengthMin, clipLengthSec);

            timeRemainingText.text = timeRemainingString;

        }


        public void ToggleVideoPlayer()
        {

            isOpen = !isOpen;

            if(isOpen)
            {

                SetUpVideos();

                videoPlayerCanvasGroup.DOFade(1f, .5f)
                    .OnComplete(() =>
                    {

                        videoPlayerCanvasGroup.interactable = true;
                        videoPlayerCanvasGroup.blocksRaycasts = true;


                    });

                SetUpButtons();

            }
            else
            {

                videoPlayerCanvasGroup.DOFade(0f, .5f)
                    .OnComplete(() =>
                    {

                        videoPlayerCanvasGroup.interactable = false;
                        videoPlayerCanvasGroup.blocksRaycasts = false;


                    });

            }

        }

        void SetUpVideos()
        {

            foreach (Transform child in videoPlayerParent)
                Destroy(child.gameObject);

            for(int i = 0; i < videoClips.Count; i++)
            {

                GameObject newVideoPlane = Instantiate(videoPlayerPrefab, videoPlayerParent);

                VideoPlayer newVideoPlayer = newVideoPlane.GetComponent<VideoPlayer>();

                videoPlayers.Add(newVideoPlayer);

                newVideoPlayer.clip = videoClips[i].clip;

                RenderTexture newRT = new RenderTexture(1920, 1080, 16, RenderTextureFormat.ARGB32);
                newRT.Create();

                newVideoPlayer.targetTexture = newRT;

                newVideoPlayer.playOnAwake = false;

               //newVideoPlayer.Stop();

                newVideoPlane.GetComponent<RawImage>().texture = newRT;

                //if (i != 0)
                //    newVideoPlane.GetComponent<RawImage>().color = new Color32(255, 255, 255, 0);

            }

            clipLengthMin = Mathf.FloorToInt((float)videoClips[0].clip.length / 60);
            clipLengthSec = Mathf.FloorToInt((float)videoClips[0].clip.length % 60);

            PauseVideo();

            StartCoroutine(TempTurnOffForVideoLayer1());

        }

        IEnumerator TempTurnOffForVideoLayer1()
        {

            yield return new WaitForSecondsRealtime(.5f);

            videoPlayers[1].gameObject.SetActive(false);

            yield return null;

        }

        void SetUpButtons()
        {

            playButtonRect.onMouseDown.Clear();
            pauseButtonRect.onMouseDown.Clear();
            restartButtonRect.onMouseDown.Clear();
            toggleLayerModeButtonRect.onMouseDown.Clear();
            toggleSideModeButtonRect.onMouseDown.Clear();

            timelineSlider.value = 0f;
            timelineSlider.maxValue = (float)videoClips[0].clip.length;

            #region Play Button

            UnityEvent onClickPlayEvent = new UnityEvent();

            onClickPlayEvent.AddListener(() =>
            {

                PlayVideo();

            });

            Function onClickPlayEventFunction = new Function
            {
                functionName = onClickPlayEvent,
                functionDelay = 0f
            };

            playButtonRect.onMouseDown.Add(onClickPlayEventFunction);

            #endregion

            #region Pause Button

            UnityEvent onClickPauseEvent = new UnityEvent();

            onClickPauseEvent.AddListener(() =>
            {

                PauseVideo();

            });

            Function onClickPauseEventFunction = new Function
            {
                functionName = onClickPauseEvent,
                functionDelay = 0f
            };

            pauseButtonRect.onMouseDown.Add(onClickPauseEventFunction);

            #endregion

            #region Restart Button

            UnityEvent onClickRestartEvent = new UnityEvent();

            onClickRestartEvent.AddListener(() =>
            {

                for (int i = 0; i < videoPlayers.Count; i++)
                {

                    videoPlayers[i].Stop();
                    videoPlayers[i].time = 0f;

                    timelineSlider.value = 0f;

                    videoPlayers[i].Play();

                    isPlaying = true;

                }

                pauseButtonRect.gameObject.SetActive(true);
                playButtonRect.gameObject.SetActive(false);
                restartButtonRect.gameObject.SetActive(false);


            });

            Function onClickRestartEventFunction = new Function
            {
                functionName = onClickRestartEvent,
                functionDelay = 0f
            };

            restartButtonRect.onMouseDown.Add(onClickRestartEventFunction);

            #endregion

            #region Mute Button

            UnityEvent onClickMuteEvent = new UnityEvent();

            onClickMuteEvent.AddListener(() =>
            {

                ToggleMute();

            });

            Function onClickMuteEventFunction = new Function
            {
                functionName = onClickMuteEvent,
                functionDelay = 0f
            };

            toggleSoundButtonRect.onMouseDown.Add(onClickMuteEventFunction);

            #endregion

            #region Layer Button

            UnityEvent onClickLayerToggleEvent = new UnityEvent();

            onClickLayerToggleEvent.AddListener(() =>
            {

                //ToggleLayerPanel();

                Toggle2ndLayer();

            });

            Function onClickLayerToggleEventFunction = new Function
            {
                functionName = onClickLayerToggleEvent,
                functionDelay = 0f
            };

            toggleLayerModeButtonRect.onMouseDown.Add(onClickLayerToggleEventFunction);

            #endregion

            #region Side By Side Button

            UnityEvent onClickSideBySideToggleEvent = new UnityEvent();

            onClickSideBySideToggleEvent.AddListener(() =>
            {

                ToggleSideBySideMode();

                for(int i = 0; i < videoPlayers.Count; i++)
                {
                    if (i == 0 || i == 1)
                    {
                        videoPlayers[i].gameObject.SetActive(true);
                        layerButtons[i].transform.GetChild(2).GetComponent<Toggle>().SetIsOnWithoutNotify(true);
                        continue;
                    }
                    else
                    {

                        videoPlayers[i].gameObject.SetActive(false);
                        layerButtons[i].transform.GetChild(2).GetComponent<Toggle>().SetIsOnWithoutNotify(false);

                    }

                    

                }

            });

            Function onClickSideBySideToggleEventFunction = new Function
            {
                functionName = onClickSideBySideToggleEvent,
                functionDelay = 0f
            };

            toggleSideModeButtonRect.onMouseDown.Add(onClickSideBySideToggleEventFunction);

            #endregion

        }

        //void SpawnLayerButtons()
        //{
        //
        //    layerButtons.Clear();
        //
        //    for(int i = 0; i < videoClips.Count; i++)
        //    {
        //
        //        GameObject newLayerButton = Instantiate(videoLayerButtonPrefab, layerButtonParent);
        //
        //        //ButtonBehavior newLayerButtonBehavior = newLayerButton.GetComponent<ButtonBehavior>();
        //
        //        layerButtons.Add(newLayerButton);
        //
        //        Image newLayerButtonThumbnail = newLayerButton.transform.GetChild(0).GetComponent<Image>();
        //        TextMeshProUGUI newLayerButtonTitle = newLayerButton.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        //        Toggle newLayerButtonToggle = newLayerButton.transform.GetChild(2).GetComponent<Toggle>();
        //
        //        if (i == 0)
        //            newLayerButtonToggle.gameObject.SetActive(false);
        //
        //        newLayerButtonTitle.text = videoClips[i].clipName;
        //
        //        newLayerButtonThumbnail.sprite = videoClips[i].clipThumbnail;
        //
        //        int index = i;
        //
        //        newLayerButtonToggle.onValueChanged.AddListener((value) => 
        //        {
        //        
        //            ToggleVideoLayer(index, value);
        //        
        //        });
        //
        //    }
        //
        //}

        //public void ToggleVideoLayer(int index, bool isActive)
        //{
        //
        //    
        //    Debug.Log("Layer Index: " + (index).ToString());
        //
        //    //bool isActive = videoPlayers[index].gameObject.activeSelf;
        //
        //    videoPlayers[index].gameObject.SetActive(isActive);
        //
        //
        //    if (isActive)
        //    {
        //
        //        videoPlayers[index].time = timelineSlider.value;
        //
        //        if (isPlaying)
        //            videoPlayers[index].Play();
        //
        //    }
        //
        //}

        public void Toggle2ndLayer()
        {

            layerPanelVisible = !layerPanelVisible;

            videoPlayers[1].gameObject.SetActive(layerPanelVisible);

            if (layerPanelVisible)
            {
                //toggleLayerModeButtonRect.GetComponent<Image>().sprite = layerButtonSprites[0];
                toggleLayerModeButtonRect.GetComponent<Image>().color = new Color32(0, 255, 185, 255);

                videoPlayers[1].time = timelineSlider.value;

                if (isPlaying)
                    videoPlayers[1].Play();
            }
            else
            {

                //toggleLayerModeButtonRect.GetComponent<Image>().sprite = layerButtonSprites[1];

                toggleLayerModeButtonRect.GetComponent<Image>().color = Color.white;

            }

        }

        public void MoveControlbarOffScreen()
        {

            controlbarRect.DOAnchorPos(new Vector2(0f, -50f), .5f).OnComplete(() => controlbarVisible = false);

        }

        public void MoveControlbarOnScreen()
        {

            controlbarRect.DOAnchorPos(new Vector2(0f, 0f), .5f).OnComplete(() => controlbarVisible = true);

        }

        public void MoveLayerPanelOffScreen()
        {

            layerPanelRect.DOAnchorPos(new Vector2(450f, 25f), .5f).OnComplete(() => layerPanelVisible = false);

        }

        public void MoveLayerPanelOnScreen()
        {

            layerPanelRect.DOAnchorPos(new Vector2(0f, 25f), .5f).OnComplete(() => layerPanelVisible = true);

        }

        public void ToggleLayerPanel()
        {

            layerPanelVisible = !layerPanelVisible;

            if (layerPanelVisible)
                MoveLayerPanelOnScreen();
            else
            {

                layerWasPreviouslyOpen = false;

                MoveLayerPanelOffScreen();

            }
                

        }

        public void ToggleSideBySideMode()
        {

            sideBySideModeActive = !sideBySideModeActive;

            if (sideBySideModeActive)
            {

                sideBySideSlider.value = 0f;

                sideBySideSlider.DOValue(.5f, .5f);

                sideBySidePanelCanvasGroup.DOFade(1f, .5f)
                    .OnStart(() =>
                    {

                        sideBySidePanelCanvasGroup.blocksRaycasts = true;
                        sideBySidePanelCanvasGroup.interactable = true;


                    });

            }
            else
            {

                sideBySideSlider.DOValue(0f, .5f);

                sideBySidePanelCanvasGroup.DOFade(0f, .5f)
                                    .OnStart(() =>
                                    {

                                        sideBySidePanelCanvasGroup.blocksRaycasts = false;
                                        sideBySidePanelCanvasGroup.interactable = false;


                                    });

            }
                

        }

        public void OnSideBySideSlider()
        {

            float sliderValue = sideBySideSlider.value;

            float offset = sliderValue * 1797f;

            Rect newRect = new Rect(sliderValue, 0f, (1 - sliderValue), 1f);

            Vector2 newOffset = new Vector2(offset, 0f);

            for(int i = 0; i < videoClips.Count; i++)
            {

                if (i == 0)
                    continue;

                RawImage rawImage = videoPlayers[i].gameObject.GetComponent<RawImage>();

                rawImage.uvRect = newRect;

                rawImage.rectTransform.offsetMin = newOffset;

            }

        }

        public void ToggleMute()
        {

            isMuted = !isMuted;

            if(isMuted)
            {

                volumeSlider.interactable = false;

                toggleSoundButtonRect.gameObject.GetComponent<Image>().sprite = soundButtonSprites[1];

                for (int i = 0; i < videoPlayers.Count; i++)
                {

                    videoPlayers[i].SetDirectAudioVolume(0, 0f);

                }

            }
            else
            {

                volumeSlider.interactable = true;

                toggleSoundButtonRect.gameObject.GetComponent<Image>().sprite = soundButtonSprites[0];

                for (int i = 0; i < videoPlayers.Count; i++)
                {

                    videoPlayers[i].SetDirectAudioVolume(0, volumeSlider.value);

                }

            }



        }

        public void OnVolumeChanged()
        {

            for(int i = 0; i < videoPlayers.Count; i++)
            {
                videoPlayers[i].SetDirectAudioVolume(0, volumeSlider.value);
            }

            if(volumeSlider.value == 0f)
                toggleSoundButtonRect.gameObject.GetComponent<Image>().sprite = soundButtonSprites[1];
            else
                toggleSoundButtonRect.gameObject.GetComponent<Image>().sprite = soundButtonSprites[0];

        }

        public void OnTimelineChanged()
        {

            PauseVideo();

            currentTimeMin = Mathf.FloorToInt(timelineSlider.value / 60);
            currentTimeSec = Mathf.FloorToInt(timelineSlider.value % 60);

            timeRemainingString = string.Format("{0:00}:{1:00} | {2:00}:{3:00}", currentTimeMin, currentTimeSec, clipLengthMin, clipLengthSec);

            timeRemainingText.text = timeRemainingString;

        }

        public void OnTimelineChangedFinished()
        {

            for (int i = 0; i < videoPlayers.Count; i++)
            {
                videoPlayers[i].time = timelineSlider.value;
            }

            PlayVideo();

        }

        public void PauseVideo()
        {

            pauseButtonRect.gameObject.SetActive(false);
            playButtonRect.gameObject.SetActive(true);
            restartButtonRect.gameObject.SetActive(false);

            isPlaying = false;

            if (videoPlayers[0].isPlaying)
            {

                for (int i = 0; i < videoPlayers.Count; i++)
                    videoPlayers[i].Pause();

            }

        }

        public void PlayVideo()
        {

            pauseButtonRect.gameObject.SetActive(true);
            playButtonRect.gameObject.SetActive(false);
            restartButtonRect.gameObject.SetActive(false);

            isPlaying = true;

            if (!videoPlayers[0].isPlaying)
            {

                for (int i = 0; i < videoPlayers.Count; i++)
                    videoPlayers[i].Play();

            }

        }


    }

    [System.Serializable]
    public class ShowroomVideoLayer
    {

        public string clipName;

        public Sprite clipThumbnail;

        public VideoClip clip;

    }

}