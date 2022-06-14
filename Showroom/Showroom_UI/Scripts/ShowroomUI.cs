using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using ThisOtherThing.UI.Shapes;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.Playables;
using UnityEngine.UI;

namespace Showroom
{

    public class ShowroomUI : MonoBehaviour
    {
        [BoxGroup("Showroom Manager")][SerializeField] private ShowroomManager showroomManager;

        [BoxGroup("Scale Settings")][Tooltip("Value is given in Diagonal-Inch")][SerializeField] float screenScaleValue;
        [BoxGroup("Scale Settings")][SerializeField] CanvasScaler showroomUICanvasScaler;
        [BoxGroup("Scale Settings")][SerializeField] AnimationCurve scaleFactor;
        [BoxGroup("Scale Settings")][SerializeField] Vector2 currentResolution;

        [BoxGroup("Sidebar Settings")][SerializeField] private RectTransform sidebarRect;
        [BoxGroup("Sidebar Settings")][SerializeField] private RectTransform sidebarParentRect;
        [BoxGroup("Sidebar Settings")][SerializeField] private RectTransform closeButtonRect;
        [BoxGroup("Sidebar Settings")][SerializeField] private RectTransform burgerButtonHeaderRect;
        [BoxGroup("Sidebar Settings")][SerializeField] private RectTransform burgerButtonOutsideRect;
        [BoxGroup("Sidebar Settings")][SerializeField] private CanvasGroup burgerButtonOutsideCanvasGroup;
        //[BoxGroup("Sidebar Settings")][SerializeField] private Vector2 sidebarOpenSize = new Vector2(450f, 220f);
        [BoxGroup("Sidebar Settings")][SerializeField] private Vector2 sidebarOpenPos = new Vector2(145f, -172f);
        [BoxGroup("Sidebar Settings")][SerializeField] private Vector2 sidebarClosePos = new Vector2(-845f, -172f);
        [BoxGroup("Sidebar Settings")] private bool sidebarIsOpen = false;

        [BoxGroup("Sidebar Settings")][SerializeField] private GameObject sidebarButtonPrefab;
        [BoxGroup("Sidebar Settings")][SerializeField] private RectTransform sidebarButtonParent;

        [BoxGroup("Sidebar Settings")][SerializeField] private TextMeshProUGUI titleText;
        [BoxGroup("Sidebar Settings")][SerializeField] private TextMeshProUGUI subTitleText;

        [BoxGroup("Sidebar Settings")][SerializeField] private Vector2 closeButtonOnScreenPos = new Vector2(145f, -172f);
        [BoxGroup("Sidebar Settings")][SerializeField] private Vector2 closeButtonOffScreenPos = new Vector2(-845f, -172f);

        [BoxGroup("General Menu Settings")][SerializeField] private RectTransform generalMenuRect;
        [BoxGroup("General Menu Settings")][SerializeField] private CanvasGroup generalMenuCanvasGroup;
        [BoxGroup("General Menu Settings")][SerializeField] private Vector2 generalMenuOpenPos = new Vector2(0f, 72f);
        [BoxGroup("General Menu Settings")][SerializeField] private Vector2 generalMenuClosedPos = new Vector2(0f, -72f);
        [BoxGroup("General Menu Settings")] private bool generalMenuIsOpen = false;

        [BoxGroup("General Menu Settings")][SerializeField] public ButtonBehavior generalMenuPlayButton;
        [BoxGroup("General Menu Settings")][SerializeField] public ButtonBehavior generalMenuReplayButton;
        [BoxGroup("General Menu Settings")][SerializeField] public ButtonBehavior generalMenuPauseButton;
        [BoxGroup("General Menu Settings")][SerializeField] public ButtonBehavior generalMenuHomeCamButton;
        [BoxGroup("General Menu Settings")][SerializeField] public ButtonBehavior generalMenuSubLevelHomeCamButton;
        [BoxGroup("General Menu Settings")][SerializeField] public ButtonBehavior generalMenuTransparencyButton;
        [BoxGroup("General Menu Settings")][SerializeField] public Sprite[] tranparencyButtons;
        [BoxGroup("General Menu Settings")][SerializeField] public ButtonBehavior generalMenuCameraPosButton;
        [BoxGroup("General Menu Settings")][SerializeField] public TextMeshProUGUI generalMenuCameraPosButtonText;
        [BoxGroup("General Menu Settings")][SerializeField] public Transform generalMenuCameraPosDropdown;
        [BoxGroup("General Menu Settings")][SerializeField] public Transform generalMenuCameraPosDropdownButtonParent;
        [BoxGroup("General Menu Settings")][SerializeField] public GameObject generalMenuCameraPosButtonPrefab;
        [BoxGroup("General Menu Settings")][SerializeField] public Sprite[] generalMenuCameraPosButtonSprites;
        [BoxGroup("General Menu Settings")] public List<ButtonBehavior> generalMenuCameraPosButtons = new List<ButtonBehavior>();
        [BoxGroup("General Menu Settings")][SerializeField] public ButtonBehavior generalMenuDragModeButton;
        [BoxGroup("General Menu Settings")][SerializeField] public Sprite[] dragModeButtons;

        private bool onlyOneGeneralMenuButtonActive = false;

        [BoxGroup("Right Menu Settings")][SerializeField] private RectTransform rightMenuRect;
        [BoxGroup("Right Menu Settings")][SerializeField] private CanvasGroup rightMenuCanvasGroup;
        [BoxGroup("Right Menu Settings")][SerializeField] private Vector2 rightMenuOpenPos = new Vector2(0f, 72f);
        [BoxGroup("Right Menu Settings")][SerializeField] private Vector2 rightMenuClosedPos = new Vector2(0f, -72f);
        [BoxGroup("Right Menu Settings")] private bool rightMenuIsOpen = false;

        [BoxGroup("Right Menu Settings")][SerializeField] public ButtonBehavior rightMenuBackButton;
        [BoxGroup("Right Menu Settings")][SerializeField] public ButtonBehavior rightMenuResetRotationButton;
        [BoxGroup("Right Menu Settings")][SerializeField] public ButtonBehavior rightMenuRotatedButton;


        [BoxGroup("Bullet Point Menu Settings")][SerializeField] private RectTransform bulletPointMenuRect;
        [BoxGroup("Bullet Point Menu Settings")][SerializeField] private CanvasGroup bulletPointMenuCanvasGroup;
        [BoxGroup("Bullet Point Menu Settings")][SerializeField] private Vector2 bulletPointMenuOpenSize = new Vector2(450f, 400);
        [BoxGroup("Bullet Point Menu Settings")][SerializeField] private Vector2 bulletPointMenuClosedSize = new Vector2(450f, -400f);
        [BoxGroup("Bullet Point Menu Settings")] private bool bulletPointMenuIsOpen = false;
        [BoxGroup("Bullet Point Menu Settings")] public TextMeshProUGUI bulletPointMenuHeadline;
        [BoxGroup("Bullet Point Menu Settings")] public TextMeshProUGUI bulletPointMenuSubHeadline;
        [BoxGroup("Bullet Point Menu Settings")] public TextMeshProUGUI bulletPointMenuText;
        [BoxGroup("Bullet Point Menu Settings")] public CanvasGroup bulletPointMenuToggleButton;


        [BoxGroup("Tool Tip Settings")][SerializeField] private RectTransform tooltipPanelRect;
        [BoxGroup("Tool Tip Settings")][SerializeField] private ThisOtherThing.UI.Shapes.Rectangle tooltipPanelShape;
        [BoxGroup("Tool Tip Settings")] public TextMeshProUGUI tooltipText;

        [BoxGroup("Tool Tip Settings/World Space Tool Tip")][SerializeField] private RectTransform tooltip3DPanelRect;
        [BoxGroup("Tool Tip Settings/World Space Tool Tip")][SerializeField] private ThisOtherThing.UI.Shapes.Rectangle tooltip3DPanelShape;
        [BoxGroup("Tool Tip Settings/World Space Tool Tip")] public TextMeshProUGUI tooltip3DPanelText;

        [BoxGroup("Tool Tip Settings")]
        [InfoBox("Tooltips are largly done, but Tooltip-Attributes are still very limited and not User-friendly. Keep that in mind!", InfoMessageType.Warning)]

        [BoxGroup("Tool Tip Settings")][SerializeField] public List<TooltipAttributes> tooltipAttributes = new List<TooltipAttributes>();

        [BoxGroup("Timeline Stepper")][SerializeField] private RectTransform timelineStepperRect;
        [BoxGroup("Timeline Stepper")][SerializeField] private Transform timelineStepperParent;

        [BoxGroup("Timeline Stepper")][SerializeField] private Slider timelineStepperSlider;

        [BoxGroup("Timeline Stepper")][SerializeField] private RectTransform timelineStepperRightChevronRect;
        [BoxGroup("Timeline Stepper")][SerializeField] private RectTransform timelineStepperLeftChevronRect;

        [BoxGroup("Timeline Stepper")][SerializeField] private Sprite timelineStepperButtonSpriteClicked;

        [BoxGroup("Timeline Stepper")][SerializeField] private GameObject timelineStepPointPrefab;
        [BoxGroup("Timeline Stepper")][SerializeField] private GameObject timelineStepChevronPrefab;
        [BoxGroup("Timeline Stepper")][SerializeField] private GameObject timelineStepEndMarkerPrefab;

        [BoxGroup("Timeline Stepper")][SerializeField] private Vector2 timelineStepperOpenPos = new Vector2(0f, -50f);
        [BoxGroup("Timeline Stepper")][SerializeField] private Vector2 timelineStepperClosedPos = new Vector2(0f, 100);

        [BoxGroup("Timeline Stepper")][SerializeField] private bool timelineStepperIsOpen = false;

        [BoxGroup("Timeline Stepper")][SerializeField] private CanvasGroup blackFade;
        [BoxGroup("Timeline Stepper")][SerializeField] private bool usesBlackFade;



        [HideInInspector]
        public List<UseCaseButtonEvent> tempButtons = new List<UseCaseButtonEvent>();

        [HideInInspector]
        public List<ButtonBehavior> spawnedButtons = new List<ButtonBehavior>();


        [HideInInspector]
        public List<UseCaseButtonEvent> tempHeadButtons = new List<UseCaseButtonEvent>();

        [HideInInspector]
        public List<ButtonBehavior> spawnedHeadButtons = new List<ButtonBehavior>();


        [HideInInspector]
        public List<UseCaseButtonEvent> tempGeneralCamButtons = new List<UseCaseButtonEvent>();

        [HideInInspector]
        public List<ButtonBehavior> spawnedGeneralCamButtons = new List<ButtonBehavior>();

        [HideInInspector]
        public List<ButtonBehavior> spawnedStepperButtons = new List<ButtonBehavior>();

        [BoxGroup("Scale Settings")]
        [Button]
        void UpdateScreenSize()
        {

            showroomUICanvasScaler.referenceResolution = currentResolution * scaleFactor.Evaluate(screenScaleValue);

        }

        private void Start()
        {

            showroomManager = ShowroomManager.Instance;

            currentResolution = new Vector2(Screen.width, Screen.height);


            if (!showroomManager.downloadFairtouchData || ShowroomSSPDataHandler.Instance == null)
            {

                GetNeededInfo();

                UpdateUI();

                //if (showroomManager.openSidebarMenu)
                //    if (sidebarIsOpen)
                //        sidebarRect.DOSizeDelta(sidebarOpenSize, .5f)
                //            .SetDelay(0f)
                //            .OnStart(() => LayoutRebuilder.ForceRebuildLayoutImmediate(sidebarButtonParent))
                //    .OnComplete(() => LayoutRebuilder.ForceRebuildLayoutImmediate(sidebarButtonParent));

                if (showroomManager.openSidebarMenu)
                    if (sidebarIsOpen)
                        sidebarRect.gameObject.GetComponent<CanvasGroup>().DOFade(1f, .01f);
                    else
                        sidebarRect.gameObject.GetComponent<CanvasGroup>().DOFade(0f, .01f);

            }
            else if (showroomManager.downloadFairtouchData)
            {

                //if (showroomManager.openSidebarMenu)
                //    if (sidebarIsOpen)
                //        sidebarRect.DOSizeDelta(sidebarOpenSize, .5f)
                //            .SetDelay(0f)
                //            .OnStart(() => LayoutRebuilder.ForceRebuildLayoutImmediate(sidebarButtonParent))
                //    .OnComplete(() => LayoutRebuilder.ForceRebuildLayoutImmediate(sidebarButtonParent));

                if (showroomManager.openSidebarMenu)
                    if (sidebarIsOpen)
                        sidebarRect.gameObject.GetComponent<CanvasGroup>().DOFade(1f, .01f);
                    else
                        sidebarRect.gameObject.GetComponent<CanvasGroup>().DOFade(0f, .01f);

            }

        }

        public void GetNeededInfo()
        {

            titleText.text = showroomManager.subLevelName;
            subTitleText.text = showroomManager.subLevelSubTitle;

            foreach (Transform child in sidebarButtonParent)
                Destroy(child.gameObject);

            for (int i = 0; i < showroomManager.useCases.Count; i++)
            {

                if (showroomManager.useCases[i].useCaseTopLevelButton != null)
                {

                    tempHeadButtons.Add(showroomManager.useCases[i].useCaseTopLevelButton);

                    GameObject newTopButton = Instantiate(sidebarButtonPrefab, sidebarButtonParent);
                    ButtonBehavior newTopButtonBehavior = newTopButton.GetComponent<ButtonBehavior>();
                    TextMeshProUGUI newTopButtonText = newTopButton.GetComponentInChildren<TextMeshProUGUI>();

                    Image buttonIcon = newTopButton.transform.GetChild(3).GetComponent<Image>();
                    Image buttonChevron = newTopButton.transform.GetChild(4).GetComponent<Image>();

                    spawnedHeadButtons.Add(newTopButtonBehavior);

                    newTopButtonBehavior.onMouseDown.AddRange(tempHeadButtons[i].useCaseButtonFunctions);
                    newTopButtonText.text = tempHeadButtons[i].useCaseButtonName;

                    newTopButton.name = tempHeadButtons[i].useCaseButtonName + "_UseCaseButton";

                    if (tempHeadButtons[i].useCaseButtonSprite != null)
                    {
                        buttonIcon.gameObject.SetActive(true);
                        buttonIcon.sprite = tempHeadButtons[i].useCaseButtonSprite;
                    }
                    else
                        buttonIcon.gameObject.SetActive(false);

                    if (showroomManager.useCases[i].useCaseHasSidebarButtons && showroomManager.useCases[i].useCaseButtons.Count != 0)
                        buttonChevron.gameObject.SetActive(true);
                    else
                        buttonChevron.gameObject.SetActive(false);


                    AddStandardHeadButtonBehavior(newTopButtonBehavior, i);

                }
                else
                {

                    tempHeadButtons.Insert(i, null);
                    spawnedHeadButtons.Insert(i, null);

                }


                for (int j = 0; j < showroomManager.useCases[i].useCaseButtons.Count; j++)
                {

                    tempButtons.Add(showroomManager.useCases[i].useCaseButtons[j]);

                    int curIndex = tempButtons.IndexOf(showroomManager.useCases[i].useCaseButtons[j]);

                    GameObject newButton = Instantiate(sidebarButtonPrefab, sidebarButtonParent);
                    Button newButtonButton = newButton.GetComponent<Button>();
                    ButtonBehavior newButtonBehavior = newButton.GetComponent<ButtonBehavior>();
                    TextMeshProUGUI newButtonText = newButton.GetComponentInChildren<TextMeshProUGUI>();

                    Image buttonIcon = newButton.transform.GetChild(3).GetComponent<Image>();

                    spawnedButtons.Add(newButtonBehavior);

                    newButtonBehavior.onMouseDown.AddRange(tempButtons[curIndex].useCaseButtonFunctions);
                    newButtonText.text = tempButtons[curIndex].useCaseButtonName;
                    newButton.name = "IndentedUseCaseButton_" + curIndex + "_" + i;

                    if (showroomManager.useCases[i].useCaseButtons[j].useCaseButtonSprite != null)
                    {
                        buttonIcon.gameObject.SetActive(true);
                        buttonIcon.sprite = showroomManager.useCases[i].useCaseButtons[j].useCaseButtonSprite;
                    }
                    else
                        buttonIcon.gameObject.SetActive(false);


                    ColorBlock cb = newButtonButton.colors;

                    cb.normalColor = new Color32(0, 31, 57, 255);
                    cb.selectedColor = new Color32(0, 31, 57, 255);
                    cb.highlightedColor = new Color32(0, 31, 57, 255);
                    cb.disabledColor = new Color32(0, 31, 57, 255);

                    newButtonButton.colors = cb;

                    newButton.transform.GetChild(1).gameObject.SetActive(false);
                    newButton.transform.GetChild(2).gameObject.SetActive(false);

                    AddStandardButtonBehavior(newButtonBehavior, curIndex, i);

                    newButton.SetActive(false);

                }

            }


            #region old Code
            //if (showroomManager.subLevelHasSidebarButtons) 
            //{
            //
            //    for (int i = 0; i < showroomManager.subLevelButtons.Count; i++)
            //    {
            //
            //        tempHeadButtons.Add(showroomManager.subLevelButtons[i]);
            //
            //        GameObject newHeadButton = Instantiate(sidebarButtonPrefab, sidebarButtonParent);
            //        ButtonBehavior newHeadButtonBehavior = newHeadButton;
            //        TextMeshProUGUI newHeadButtonText = newHeadButton.GetComponentInChildren<TextMeshProUGUI>();
            //
            //        spawnedHeadButtons.Add(newHeadButtonBehavior);
            //
            //        newHeadButtonBehavior.onMouseDown.AddRange(tempHeadButtons[i].useCaseButtonFunctions);
            //        newHeadButtonText.text = tempHeadButtons[i].useCaseButtonName;
            //
            //        AddStandardHeadButtonBehavior(newHeadButtonBehavior, i);
            //
            //        for (int j = 0; j < showroomManager.useCases[i].useCaseButtons.Count; j++)
            //        {
            //
            //            tempButtons.Add(showroomManager.useCases[i].useCaseButtons[j]);
            //
            //            GameObject newButton = Instantiate(sidebarButtonPrefab, sidebarButtonParent);
            //            ButtonBehavior newButtonBehavior = newButton;
            //            TextMeshProUGUI newButtonText = newButton.GetComponentInChildren<TextMeshProUGUI>();
            //
            //            spawnedButtons.Add(newButtonBehavior);
            //
            //            newButtonBehavior.onMouseDown.AddRange(tempButtons[j].useCaseButtonFunctions);
            //            newButtonText.text = tempButtons[j].useCaseButtonName;
            //
            //            AddStandardButtonBehavior(newButtonBehavior, j);
            //
            //        }
            //
            //    }
            //
            //}
            //else
            //{
            //
            //    for (int i = 0; i < showroomManager.useCases.Count; i++)
            //    {
            //
            //        tempButtons.AddRange(showroomManager.useCases[i].useCaseButtons);
            //
            //    }
            //
            //    //for (int i = 0; i < showroomManager.useCases.Count; i++)
            ////{
            ////
            //    //    tempButtons.AddRange(showroomManager.useCases[i].useCaseButtons);
            ////    
            //    //    GameObject newButton = Instantiate(sidebarButtonPrefab, sidebarButtonParent);
            //    //    ButtonBehavior newButtonBehavior = newButton;
            //    //    TextMeshProUGUI newButtonText = newButton.GetComponentInChildren<TextMeshProUGUI>();
            ////    
            //    //    spawnedButtons.Add(newButtonBehavior);
            ////    
            //    //    newButtonBehavior.onMouseDown.AddRange(tempButtons[i].useCaseButtonFunctions);
            //    //    newButtonText.text = tempButtons[i].useCaseButtonName;
            ////    
            //    //    AddStandardButtonBehavior(newButtonBehavior, i);
            ////
            ////}
            //
            //}
            #endregion

            AddBehaviorToGeneralMenuButtons();
        }

        void AddStandardButtonBehavior(ButtonBehavior button, int index, int useCaseIndex)
        {

            #region OnHover Event

            UnityEvent onHoverEvent = new UnityEvent();

            onHoverEvent.AddListener(delegate
            {
                this.OnHover(button.transform);
            });

            Function onHoverFunction = new Function
            {
                functionName = onHoverEvent,
                functionDelay = 0f
            };

            button.onMouseEnter.Add(onHoverFunction);

            #endregion

            #region OnStopHover Event

            UnityEvent onStopHoverEvent = new UnityEvent();

            onStopHoverEvent.AddListener(delegate
            {
                this.StopHover(button.transform);
            });

            Function onStopHoverFunction = new Function
            {
                functionName = onStopHoverEvent,
                functionDelay = 0f
            };

            button.onMouseExit.Add(onStopHoverFunction);

            #endregion

            #region OnClick Event

            UnityEvent onClickEvent = new UnityEvent();

            onClickEvent.AddListener(() =>
            {

                this.ResetAllSidebarButtons(index);

                this.OnClickButton(button.transform);

                showroomManager.isAtUseCaseHomePos = false;

                //this.OnClickButton(spawnedHeadButtons[useCaseIndex].transform);

                //spawnedHeadButtons[useCaseIndex].ResetClick();

            });

            Function onClickFunction = new Function
            {
                functionName = onClickEvent,
                functionDelay = 0f
            };

            button.onMouseDown.Add(onClickFunction);

            #endregion

        }

        void AddStandardHeadButtonBehavior(ButtonBehavior button, int index)
        {

            #region OnHover Event

            UnityEvent onHoverEvent = new UnityEvent();

            onHoverEvent.AddListener(delegate
            {
                this.OnHover(button.transform);
            });

            Function onHoverFunction = new Function
            {
                functionName = onHoverEvent,
                functionDelay = 0f
            };

            button.onMouseEnter.Add(onHoverFunction);

            #endregion

            #region OnStopHover Event

            UnityEvent onStopHoverEvent = new UnityEvent();

            onStopHoverEvent.AddListener(delegate
            {
                this.StopHover(button.transform);
            });

            Function onStopHoverFunction = new Function
            {
                functionName = onStopHoverEvent,
                functionDelay = 0f
            };

            button.onMouseExit.Add(onStopHoverFunction);

            #endregion

            #region OnReset

            UnityEvent onResetEvent = new UnityEvent();

            onResetEvent.AddListener(delegate
            {
                Image buttonChevron = button.transform.GetChild(4).GetComponent<Image>();

                buttonChevron.rectTransform.DOLocalRotate(new Vector3(0f, 0f, 0f), 8f / 60f);

            });

            Function onResetFunction = new Function
            {
                functionName = onResetEvent,
                functionDelay = 0f
            };

            button.onButtonReset.Add(onResetFunction);

            #endregion

            #region OnClick Event

            UnityEvent onClickEvent = new UnityEvent();

            onClickEvent.AddListener(() =>
            {
                this.OnClickButton(button.transform);


                showroomManager.DisableTransparency();


                this.ResetAllSidebarHeadButtons(index);

                RemoveGeneralMenu();

                showroomManager.SwitchUseCase(index);

                Image buttonChevron = button.transform.GetChild(4).GetComponent<Image>();

                buttonChevron.rectTransform.DOLocalRotate(new Vector3(0f, 0f, -90f), 8f / 60f);

                int startIndex = 0;

                for (int i = 0; i < showroomManager.useCases.Count; i++)
                {

                    if (i == index)
                    {

                        for (int buttonIndex = 0; buttonIndex < showroomManager.useCases[i].useCaseButtons.Count; buttonIndex++)
                        {

                            //Rework

                            spawnedButtons[buttonIndex + startIndex].gameObject.SetActive(true);

                        }

                    }
                    else
                    {

                        startIndex += showroomManager.useCases[i].useCaseButtons.Count;

                    }

                }

                showroomManager.DisableDragMode();

                //Vector2 tempSidebarOpenSize = new Vector2(sidebarOpenSize.x, 55f * (tempHeadButtons.Count + showroomManager.useCases[index].useCaseButtons.Count + 50f));
                //
                //if (showroomManager.openSidebarMenu)
                //    sidebarRect.DOSizeDelta(tempSidebarOpenSize, .5f)
                //    .OnStart(() => LayoutRebuilder.ForceRebuildLayoutImmediate(sidebarButtonParent))
                //    .OnComplete(() => LayoutRebuilder.ForceRebuildLayoutImmediate(sidebarButtonParent));

                LayoutRebuilder.ForceRebuildLayoutImmediate(sidebarButtonParent);
                LayoutRebuilder.ForceRebuildLayoutImmediate(sidebarButtonParent);


                showroomManager.isAtUseCaseHomePos = false;
                showroomManager.MoveToFixedPos(-1);

            });

            Function onClickFunction = new Function
            {
                functionName = onClickEvent,
                functionDelay = 0f
            };

            button.onMouseDown.Add(onClickFunction);

            #endregion

        }

        public void ColorTMBlack(Transform button)
        {

            TextMeshProUGUI text = button.GetChild(0).GetComponent<TextMeshProUGUI>();

            text.color = Color.black;
            text.DOColor(Color.black, .1f);

        }

        public void ColorTMWhite(Transform button)
        {

            TextMeshProUGUI text = button.GetChild(0).GetComponent<TextMeshProUGUI>();

            text.color = Color.white;
            text.DOColor(Color.white, .1f);

        }

        void AddBehaviorToGeneralMenuButtons()
        {

            generalMenuPlayButton.onMouseDown.Clear();
            generalMenuReplayButton.onMouseDown.Clear();
            generalMenuPauseButton.onMouseDown.Clear();
            generalMenuTransparencyButton.onMouseDown.Clear();
            generalMenuHomeCamButton.onMouseDown.Clear();
            generalMenuSubLevelHomeCamButton.onMouseDown.Clear();
            generalMenuCameraPosButton.onMouseDown.Clear();
            generalMenuDragModeButton.onMouseDown.Clear();

            rightMenuBackButton.onMouseDown.Clear();
            rightMenuRotatedButton.onMouseDown.Clear();
            rightMenuResetRotationButton.onMouseDown.Clear();


            if (showroomManager.useCaseIndex == -1 && showroomManager.subLevelHasGeneralMenu)
            {

                if (showroomManager.hasCameraPosButton)
                {

                    generalMenuCameraPosButtonText.gameObject.SetActive(false);

                    for (int i = 0; i < generalMenuCameraPosButtons.Count; i++)
                    {

                        int index = i;

                        UnityEvent onClickEventCameraPosButton = new UnityEvent();

                        onClickEventCameraPosButton.AddListener(() =>
                        {
                            SwitchActiveCameraButton(index);

                        });

                        Function onClickFunctionPosButton = new Function
                        {
                            functionName = onClickEventCameraPosButton,
                            functionDelay = 0f
                        };


                        generalMenuCameraPosButtons[i].onMouseDown.AddRange(showroomManager.cameraButtons[i].cameraPositionButtonFunctions);
                        generalMenuCameraPosButtons[i].onMouseDown.Add(onClickFunctionPosButton);

                    }

                }

                #region Transparency Button

                UnityEvent onClickEventTransparency = new UnityEvent();

                onClickEventTransparency.AddListener(() =>
                {
                    showroomManager.ToggleTransparency();
                    showroomManager.UpdateTransparencyButton();
                });

                Function onClickFunctionTransparency = new Function
                {
                    functionName = onClickEventTransparency,
                    functionDelay = 0f
                };

                generalMenuTransparencyButton.onMouseDown.Add(onClickFunctionTransparency);

                generalMenuTransparencyButton.onMouseDown.AddRange(showroomManager.transparencyButtonFunctions);

                #endregion

                #region Home Button

                UnityEvent onClickEventHome = new UnityEvent();

                onClickEventHome.AddListener(() =>
                {
                    ResetAllCameraButtons();

                    showroomManager.DisableDragMode();

                    showroomManager.MoveToFixedPos(-1);
                    ResetAllSidebarButtons(-1);
                    tooltipPanelRect.gameObject.SetActive(false);

                    generalMenuHomeCamButton.gameObject.SetActive(false);
                    generalMenuSubLevelHomeCamButton.gameObject.SetActive(true);

                });

                Function onClickFunctionHome = new Function
                {
                    functionName = onClickEventHome,
                    functionDelay = 0f
                };

                generalMenuHomeCamButton.onMouseDown.AddRange(showroomManager.resetCameraButtonFunctions);

                generalMenuHomeCamButton.onMouseDown.Add(onClickFunctionHome);


                #endregion

                #region Play Button

                UnityEvent onClickEventPlay = new UnityEvent();

                onClickEventPlay.AddListener(() =>
                {

                    if (showroomManager.playButtonIsPauseButton)
                    {

                        generalMenuPlayButton.gameObject.SetActive(false);
                        generalMenuPauseButton.gameObject.SetActive(true);

                        generalMenuPauseButton.GetComponent<Button>().Select();

                        tooltipPanelRect.gameObject.SetActive(false);

                    }
                    else if (showroomManager.playButtonIsRestartButton)
                    {

                        generalMenuPlayButton.gameObject.SetActive(false);
                        generalMenuReplayButton.gameObject.SetActive(true);

                        generalMenuReplayButton.GetComponent<Button>().Select();

                        tooltipPanelRect.gameObject.SetActive(false);

                    }

                });

                Function onClickEventPlayFunction = new Function
                {
                    functionName = onClickEventPlay,
                    functionDelay = 0f
                };

                generalMenuPlayButton.onMouseDown.AddRange(showroomManager.playButtonFunctions);
                generalMenuPlayButton.onMouseDown.Add(onClickEventPlayFunction);


                #endregion

                #region Pause Button

                UnityEvent onClickEventPause = new UnityEvent();

                onClickEventPause.AddListener(() =>
                {

                    if (showroomManager.playButtonIsPauseButton)
                    {

                        generalMenuPlayButton.gameObject.SetActive(true);
                        generalMenuPauseButton.gameObject.SetActive(false);

                        generalMenuPauseButton.GetComponent<Button>().Select();

                        tooltipPanelRect.gameObject.SetActive(false);

                    }

                });

                Function onClickEventPauseFunction = new Function
                {
                    functionName = onClickEventPause,
                    functionDelay = 0f
                };

                generalMenuPauseButton.onMouseDown.AddRange(showroomManager.pauseButtonFunctions);
                generalMenuPauseButton.onMouseDown.Add(onClickEventPauseFunction);


                #endregion

                #region Restart Button

                UnityEvent onClickEventRestart = new UnityEvent();

                onClickEventRestart.AddListener(() =>
                {

                    if (showroomManager.playButtonIsRestartButton)
                    {

                        generalMenuPlayButton.gameObject.SetActive(true);
                        generalMenuReplayButton.gameObject.SetActive(false);

                        generalMenuReplayButton.GetComponent<Button>().Select();

                        tooltipPanelRect.gameObject.SetActive(false);

                    }

                });

                Function onClickEventRestartFunction = new Function
                {
                    functionName = onClickEventRestart,
                    functionDelay = 0f
                };

                generalMenuReplayButton.onMouseDown.AddRange(showroomManager.resetButtonFunctions);
                generalMenuReplayButton.onMouseDown.Add(onClickEventRestartFunction);


                #endregion

                #region Sub-Level Home Button

                UnityEvent onClickEventSubLevelHome = new UnityEvent();

                onClickEventSubLevelHome.AddListener(() =>
                {
                    ResetAllCameraButtons();

                    showroomManager.DisableDragMode();

                    tooltipPanelRect.gameObject.SetActive(false);
                    showroomManager.MoveToFixedPos(-1);
                    ResetAllSidebarButtons(-1);

                });

                Function onClickFunctionSubLevelHome = new Function
                {
                    functionName = onClickEventSubLevelHome,
                    functionDelay = 0f
                };

                generalMenuSubLevelHomeCamButton.onMouseDown.AddRange(showroomManager.resetSubLevelCameraButtonFunctions);

                generalMenuSubLevelHomeCamButton.onMouseDown.Add(onClickFunctionSubLevelHome);

                #endregion

                #region Drag Mode Button

                UnityEvent onClickDragModeEvent = new UnityEvent();

                onClickDragModeEvent.AddListener(() =>
                {
                    showroomManager.ToggleDragMode();
                    showroomManager.UpdateDragModeButton();
                });

                Function onClickDragModeEventFunction = new Function
                {
                    functionName = onClickDragModeEvent,
                    functionDelay = 0f
                };

                generalMenuDragModeButton.onMouseDown.AddRange(showroomManager.dragModeButtonFunctions);

                generalMenuDragModeButton.onMouseDown.Add(onClickDragModeEventFunction);

                #endregion

                #region Right Side Menu Buttons


                UnityEvent onRotatedClickEvent = new UnityEvent();

                onRotatedClickEvent.AddListener(() =>
                {

                    //showroomManager.ResetFocusedObjRotation();

                    rightMenuResetRotationButton.gameObject.SetActive(true);
                    rightMenuRotatedButton.gameObject.SetActive(false);

                    if (showroomManager.showDebugMessages)
                        Debug.Log("Pressed the Focus-Menu reset rotation button!");

                });

                Function onRotatedClickEventFunction = new Function
                {
                    functionName = onRotatedClickEvent,
                    functionDelay = 0f
                };

                rightMenuRotatedButton.onMouseDown.AddRange(showroomManager.resetRotationButtonFunctions);

                rightMenuRotatedButton.onMouseDown.Add(onRotatedClickEventFunction);


                UnityEvent onRightMenuBackButtonEvent = new UnityEvent();

                onRightMenuBackButtonEvent.AddListener(() =>
                {

                    ToggleRightMenu(false);

                    MoveCloseButtonOntoScreen();
                    MoveGeneralMenuOntoScreen();
                    MoveSidebarOntoScreen();

                    if (showroomManager.showDebugMessages)
                        Debug.Log("Pressed the Focus-Menu back button!");

                    showroomManager.UnfocusObject();

                });

                Function onRightMenuBackButtonEventFunction = new Function
                {
                    functionName = onRightMenuBackButtonEvent,
                    functionDelay = 0f
                };

                rightMenuBackButton.onMouseDown.AddRange(showroomManager.backButtonFunctions);

                rightMenuBackButton.onMouseDown.Add(onRightMenuBackButtonEventFunction);


                #endregion

            }
            else if (showroomManager.useCaseIndex != -1)
            {


                #region Transparency Button

                UnityEvent onClickEventTransparency = new UnityEvent();

                onClickEventTransparency.AddListener(() =>
                {
                    showroomManager.ToggleTransparency();
                    showroomManager.UpdateTransparencyButton();
                });

                Function onClickFunctionTransparency = new Function
                {
                    functionName = onClickEventTransparency,
                    functionDelay = 0f
                };

                generalMenuTransparencyButton.onMouseDown.Add(onClickFunctionTransparency);

                generalMenuTransparencyButton.onMouseDown.AddRange(showroomManager.useCases[showroomManager.useCaseIndex].transparencyButtonFunctions);

                #endregion

                #region Home Button

                UnityEvent onClickEventHome = new UnityEvent();

                onClickEventHome.AddListener(() =>
                {
                    showroomManager.MoveToFixedPos(-1);
                    //ResetAllSidebarButtons(-1);
                    tooltipPanelRect.gameObject.SetActive(false);

                    ResetAllCameraButtons();

                    showroomManager.DisableDragMode();

                    generalMenuHomeCamButton.gameObject.SetActive(false);
                    generalMenuSubLevelHomeCamButton.gameObject.SetActive(true);

                });

                Function onClickFunctionHome = new Function
                {
                    functionName = onClickEventHome,
                    functionDelay = 0f
                };

                generalMenuHomeCamButton.onMouseDown.AddRange(showroomManager.useCases[showroomManager.useCaseIndex].resetCameraButtonFunctions);

                generalMenuHomeCamButton.onMouseDown.Add(onClickFunctionHome);

                #endregion

                #region Play Button

                UnityEvent onClickEventPlay = new UnityEvent();

                onClickEventPlay.AddListener(() =>
                {

                    if (showroomManager.useCases[showroomManager.useCaseIndex].playButtonIsPauseButton)
                    {

                        generalMenuPlayButton.gameObject.SetActive(false);
                        generalMenuPauseButton.gameObject.SetActive(true);

                        generalMenuPauseButton.GetComponent<Button>().Select();

                        tooltipPanelRect.gameObject.SetActive(false);

                    }
                    else if (showroomManager.useCases[showroomManager.useCaseIndex].playButtonIsRestartButton)
                    {

                        generalMenuPlayButton.gameObject.SetActive(false);
                        generalMenuReplayButton.gameObject.SetActive(true);

                        generalMenuReplayButton.GetComponent<Button>().Select();

                        tooltipPanelRect.gameObject.SetActive(false); tooltipPanelRect.gameObject.SetActive(true);

                    }

                });

                Function onClickEventPlayFunction = new Function
                {
                    functionName = onClickEventPlay,
                    functionDelay = 0f
                };

                generalMenuPlayButton.onMouseDown.AddRange(showroomManager.useCases[showroomManager.useCaseIndex].playButtonFunctions);
                generalMenuPlayButton.onMouseDown.Add(onClickEventPlayFunction);


                #endregion

                #region Pause Button

                UnityEvent onClickEventPause = new UnityEvent();

                onClickEventPause.AddListener(() =>
                {

                    if (showroomManager.useCases[showroomManager.useCaseIndex].playButtonIsPauseButton)
                    {

                        generalMenuPlayButton.gameObject.SetActive(true);
                        generalMenuPauseButton.gameObject.SetActive(false);

                        generalMenuPauseButton.GetComponent<Button>().Select();

                        tooltipPanelRect.gameObject.SetActive(false); tooltipPanelRect.gameObject.SetActive(true);

                    }

                });

                Function onClickEventPauseFunction = new Function
                {
                    functionName = onClickEventPause,
                    functionDelay = 0f
                };

                generalMenuPauseButton.onMouseDown.AddRange(showroomManager.useCases[showroomManager.useCaseIndex].pauseButtonFunctions);
                generalMenuPauseButton.onMouseDown.Add(onClickEventPauseFunction);


                #endregion

                #region Restart Button

                UnityEvent onClickEventRestart = new UnityEvent();

                onClickEventRestart.AddListener(() =>
                {

                    if (showroomManager.useCases[showroomManager.useCaseIndex].playButtonIsRestartButton)
                    {

                        generalMenuPlayButton.gameObject.SetActive(true);
                        generalMenuReplayButton.gameObject.SetActive(false);

                        generalMenuReplayButton.GetComponent<Button>().Select();

                        tooltipPanelRect.gameObject.SetActive(false); tooltipPanelRect.gameObject.SetActive(true);

                    }

                });

                Function onClickEventRestartFunction = new Function
                {
                    functionName = onClickEventRestart,
                    functionDelay = 0f
                };

                generalMenuReplayButton.onMouseDown.AddRange(showroomManager.useCases[showroomManager.useCaseIndex].resetButtonFunctions);
                generalMenuReplayButton.onMouseDown.Add(onClickEventRestartFunction);


                #endregion

                #region Sub-Level Home Button

                UnityEvent onClickEventSubLevelHome = new UnityEvent();

                onClickEventSubLevelHome.AddListener(() =>
                {

                    showroomManager.DisableDragMode();

                    showroomManager.MoveToFixedPos(-1);
                    ResetAllSidebarButtons(-1);

                });

                Function onClickFunctionSubLevelHome = new Function
                {
                    functionName = onClickEventSubLevelHome,
                    functionDelay = 0f
                };

                generalMenuSubLevelHomeCamButton.onMouseDown.AddRange(showroomManager.resetSubLevelCameraButtonFunctions);

                generalMenuSubLevelHomeCamButton.onMouseDown.Add(onClickFunctionSubLevelHome);

                #endregion

                #region Drag Mode Button

                UnityEvent onClickDragModeEvent = new UnityEvent();

                onClickDragModeEvent.AddListener(() =>
                {
                    showroomManager.ToggleDragMode();
                    showroomManager.UpdateDragModeButton();
                });

                Function onClickDragModeEventFunction = new Function
                {
                    functionName = onClickDragModeEvent,
                    functionDelay = 0f
                };

                generalMenuDragModeButton.onMouseDown.AddRange(showroomManager.useCases[showroomManager.useCaseIndex].dragModeButtonFunctions);

                generalMenuDragModeButton.onMouseDown.Add(onClickDragModeEventFunction);

                #endregion

                #region Right Side Menu Buttons


                UnityEvent onRotatedClickEvent = new UnityEvent();

                onRotatedClickEvent.AddListener(() =>
                {

                    rightMenuResetRotationButton.gameObject.SetActive(true);
                    rightMenuRotatedButton.gameObject.SetActive(false);

                    showroomManager.ResetFocusedObjRotation();

                });

                Function onRotatedClickEventFunction = new Function
                {
                    functionName = onRotatedClickEvent,
                    functionDelay = 0f
                };

                rightMenuRotatedButton.onMouseDown.AddRange(showroomManager.useCases[showroomManager.useCaseIndex].resetRotationButtonFunctions);

                rightMenuRotatedButton.onMouseDown.Add(onRotatedClickEventFunction);


                UnityEvent onRightMenuBackButtonEvent = new UnityEvent();

                onRightMenuBackButtonEvent.AddListener(() =>
                {

                    ToggleRightMenu(false);

                    MoveCloseButtonOntoScreen();
                    MoveGeneralMenuOntoScreen();
                    MoveSidebarOntoScreen();

                    showroomManager.UnfocusObject();

                });

                Function onRightMenuBackButtonEventFunction = new Function
                {
                    functionName = onRightMenuBackButtonEvent,
                    functionDelay = 0f
                };

                rightMenuBackButton.onMouseDown.AddRange(showroomManager.useCases[showroomManager.useCaseIndex].backButtonFunctions);

                rightMenuBackButton.onMouseDown.Add(onRightMenuBackButtonEventFunction);


                #endregion

                if (showroomManager.useCases[showroomManager.useCaseIndex].hasCameraPosButton)
                {

                    generalMenuCameraPosButtonText.gameObject.SetActive(false);

                    for (int i = 0; i < generalMenuCameraPosButtons.Count; i++)
                    {

                        int index = i;

                        UnityEvent onClickEventCameraPosButton = new UnityEvent();

                        onClickEventCameraPosButton.AddListener(() =>
                        {
                            SwitchActiveCameraButton(index);

                        });

                        Function onClickFunctionPosButton = new Function
                        {
                            functionName = onClickEventCameraPosButton,
                            functionDelay = 0f
                        };


                        generalMenuCameraPosButtons[i].onMouseDown.AddRange(showroomManager.useCases[showroomManager.useCaseIndex].cameraButtons[i].cameraPositionButtonFunctions);
                        generalMenuCameraPosButtons[i].onMouseDown.Add(onClickFunctionPosButton);

                    }

                }

            }

        }

        public void UpdateUI(bool updatingGeneralMenu = false)
        {

            if (showroomManager.showDebugMessages)
                Debug.Log("Updating UI");

            if (showroomManager.useCaseIndex == -1)
            {

                generalMenuPlayButton.gameObject.SetActive(showroomManager.hasPlayButton);

                generalMenuPauseButton.gameObject.SetActive(false);

                generalMenuTransparencyButton.gameObject.SetActive(showroomManager.hasTransparencyButton);

                if (showroomManager.hasRestartButton && !showroomManager.playButtonIsRestartButton)
                    generalMenuReplayButton.gameObject.SetActive(true);
                else
                    generalMenuReplayButton.gameObject.SetActive(false);

                generalMenuHomeCamButton.gameObject.SetActive(false);//showroomManager.hasResetCameraButton);
                generalMenuSubLevelHomeCamButton.gameObject.SetActive(showroomManager.hasResetCameraButton);

                generalMenuCameraPosButton.gameObject.SetActive(showroomManager.hasCameraPosButton);

                generalMenuDragModeButton.gameObject.SetActive(showroomManager.hasDragModeButton);


                if (showroomManager.hasCameraPosButton)
                {

                    int cameraButtonAmount = showroomManager.cameraButtons.Count;

                    if (updatingGeneralMenu)
                    {
                        foreach (Transform child in generalMenuCameraPosDropdownButtonParent.transform)
                        {

                            Destroy(child.gameObject);

                        }
                    }

                    generalMenuCameraPosButtons.Clear();

                    for (int i = 0; i < cameraButtonAmount; i++)
                    {

                        GameObject newCameraPosButton = Instantiate(generalMenuCameraPosButtonPrefab, generalMenuCameraPosDropdownButtonParent);

                        TextMeshProUGUI newCameraPosButtonText = newCameraPosButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>();

                        newCameraPosButtonText.text = (i + 1).ToString();

                        generalMenuCameraPosButtons.Add(newCameraPosButton.GetComponent<ButtonBehavior>());

                    }

                }



            }
            else
            {

                generalMenuPlayButton.gameObject.SetActive(showroomManager.useCases[showroomManager.useCaseIndex].hasPlayButton);

                generalMenuPauseButton.gameObject.SetActive(false);

                generalMenuTransparencyButton.gameObject.SetActive(showroomManager.useCases[showroomManager.useCaseIndex].hasTransparencyButton);

                if (showroomManager.useCases[showroomManager.useCaseIndex].hasRestartButton && !showroomManager.useCases[showroomManager.useCaseIndex].playButtonIsRestartButton)
                    generalMenuReplayButton.gameObject.SetActive(true);
                else
                    generalMenuReplayButton.gameObject.SetActive(false);

                generalMenuHomeCamButton.gameObject.SetActive(false);//showroomManager.useCases[showroomManager.useCaseIndex].hasResetCameraButton);
                generalMenuSubLevelHomeCamButton.gameObject.SetActive(showroomManager.useCases[showroomManager.useCaseIndex].hasResetCameraButton);

                generalMenuCameraPosButton.gameObject.SetActive(showroomManager.useCases[showroomManager.useCaseIndex].hasCameraPosButton);

                generalMenuDragModeButton.gameObject.SetActive(showroomManager.useCases[showroomManager.useCaseIndex].hasDragModeButton);


                if (showroomManager.useCases[showroomManager.useCaseIndex].hasCameraPosButton)
                {

                    int cameraButtonAmount = showroomManager.useCases[showroomManager.useCaseIndex].cameraButtons.Count;

                    if (updatingGeneralMenu)
                    {
                        foreach (Transform child in generalMenuCameraPosDropdownButtonParent.transform)
                        {

                            Destroy(child.gameObject);

                        }
                    }

                    generalMenuCameraPosButtons.Clear();

                    for (int i = 0; i < cameraButtonAmount; i++)
                    {

                        GameObject newCameraPosButton = Instantiate(generalMenuCameraPosButtonPrefab, generalMenuCameraPosDropdownButtonParent);

                        TextMeshProUGUI newCameraPosButtonText = newCameraPosButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>();

                        newCameraPosButtonText.text = (i + 1).ToString();

                        generalMenuCameraPosButtons.Add(newCameraPosButton.GetComponent<ButtonBehavior>());

                    }

                }

            }


            if (!generalMenuPlayButton.gameObject.activeSelf &&
                !generalMenuReplayButton.gameObject.activeSelf &&
                !generalMenuHomeCamButton.gameObject.activeSelf &&
                !generalMenuSubLevelHomeCamButton.gameObject.activeSelf &&
                !generalMenuTransparencyButton.gameObject.activeSelf &&
                !generalMenuPauseButton.gameObject.activeSelf &&
                !generalMenuDragModeButton.gameObject.activeSelf)
            {
                generalMenuCanvasGroup.alpha = 0;
                generalMenuIsOpen = false;
            }
            else if (generalMenuPlayButton.gameObject.activeSelf ||
                generalMenuReplayButton.gameObject.activeSelf ||
                generalMenuHomeCamButton.gameObject.activeSelf ||
                generalMenuSubLevelHomeCamButton.gameObject.activeSelf ||
                generalMenuTransparencyButton.gameObject.activeSelf ||
                generalMenuPauseButton.gameObject.activeSelf
                || generalMenuDragModeButton.gameObject.activeSelf)
            {
                generalMenuCanvasGroup.alpha = 1;
                generalMenuIsOpen = true;
            }

            int indexOfActiveChildren = 0;

            foreach (Transform child in generalMenuRect)
            {
                if (child.gameObject.activeSelf)
                    indexOfActiveChildren++;
            }

            if (indexOfActiveChildren > 2)
                onlyOneGeneralMenuButtonActive = false;
            else
                onlyOneGeneralMenuButtonActive = true;


            if (showroomManager.openSidebarMenu)
            {

                sidebarIsOpen = true;

                //if (showroomManager.useCaseIndex == -1)
                //    sidebarOpenSize = new Vector2(sidebarOpenSize.x, 55f * (tempHeadButtons.Count + 30f));// + tempButtons.Count));
                //else
                //    sidebarOpenSize = new Vector2(sidebarOpenSize.x, 55f * (tempHeadButtons.Count + showroomManager.useCases[showroomManager.useCaseIndex].useCaseButtons.Count));// + 30f));

                //if (sidebarIsOpen && showroomManager.openSidebarMenu)
                //    sidebarRect.DOSizeDelta(sidebarOpenSize, .5f)
                //    .OnStart(() => LayoutRebuilder.ForceRebuildLayoutImmediate(sidebarButtonParent))
                //    .OnComplete(() => LayoutRebuilder.ForceRebuildLayoutImmediate(sidebarButtonParent));

                if (sidebarIsOpen && showroomManager.openSidebarMenu)
                {

                    LayoutRebuilder.ForceRebuildLayoutImmediate(sidebarButtonParent);
                    LayoutRebuilder.ForceRebuildLayoutImmediate(sidebarButtonParent);

                }

            }


            if (generalMenuIsOpen)
                if (updatingGeneralMenu)
                {
                    generalMenuRect.DOAnchorPos(generalMenuOpenPos, .5f);
                    AddBehaviorToGeneralMenuButtons();
                }
                else if (showroomManager.subLevelHasGeneralMenu)
                    if (showroomManager.isOnlyLevelInBuild)
                        generalMenuRect.DOAnchorPos(generalMenuOpenPos, .5f).SetDelay(2f);
                    else
                        generalMenuRect.DOAnchorPos(generalMenuOpenPos, .5f).SetDelay(2f);



        }

        public void RemoveGeneralMenu()
        {

            generalMenuRect.DOAnchorPos(generalMenuClosedPos, .5f).OnComplete(() => UpdateUI(true));

        }

        public void OnHover(Transform t)
        {

            if (showroomManager.showDebugMessages)
                Debug.Log("Hovering Button: " + t.name);

            t.GetChild(1).GetComponent<PixelLine>().color = new Color32(0, 255, 185, 255);
            t.GetChild(2).GetComponent<PixelLine>().color = new Color32(0, 255, 185, 255);

        }

        public void StopHover(Transform t)
        {

            if (showroomManager.showDebugMessages)
                Debug.Log("Stopped Hovering Button: " + t.name);

            t.GetChild(1).GetComponent<PixelLine>().color = new Color32(76, 76, 104, 255);
            t.GetChild(2).GetComponent<PixelLine>().color = new Color32(76, 76, 104, 255);

        }

        public void OnClickButton(Transform t)
        {
            if (showroomManager.showDebugMessages)
                Debug.Log("Clicked Button: " + t.name);

            t.GetChild(0).GetComponent<TextMeshProUGUI>().color = new Color32(0, 255, 185, 255);

            t.GetChild(3).GetComponent<Image>().color = Color.white;

        }

        public void ResetButton(Transform t)
        {
            if (showroomManager.showDebugMessages)
                Debug.Log("Resetting Button: " + t.name);

            t.GetChild(0).GetComponent<TextMeshProUGUI>().color = Color.white;
            t.GetChild(1).GetComponent<PixelLine>().color = new Color32(76, 76, 104, 255);
            t.GetChild(2).GetComponent<PixelLine>().color = new Color32(76, 76, 104, 255);

            t.GetChild(3).GetComponent<Image>().color = new Color32(76, 76, 104, 32);

        }

        public void ResetAllSidebarButtons(int j)
        {

            for (int i = 0; i < spawnedButtons.Count; i++)
            {
                if (j != -1)
                    if (i == j)
                        continue;

                spawnedButtons[i].ResetClick();
                ResetButton(spawnedButtons[i].transform);

            }

        }

        public void HighlightSidebarButton(int index)
        {



            for (int i = 0; i < spawnedButtons.Count; i++)
            {
                if (index != -1)
                    if (i == index)
                    {

                        if (spawnedButtons[i].gameObject.activeSelf)
                            spawnedButtons[i].OnMouseDown();
                        else
                        {

                            ResetAllSidebarHeadButtons(-1);

                            string[] s = spawnedButtons[i].gameObject.name.Split('_');

                            int newUseCaseIndex = int.Parse(s[2]);

                            showroomManager.SwitchUseCase(newUseCaseIndex);

                            Image buttonChevron = spawnedHeadButtons[newUseCaseIndex].transform.GetChild(4).GetComponent<Image>();

                            buttonChevron.rectTransform.DOLocalRotate(new Vector3(0f, 0f, -90f), 8f / 60f);

                            spawnedHeadButtons[newUseCaseIndex].transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = new Color32(0, 255, 185, 255);

                            spawnedHeadButtons[newUseCaseIndex].transform.GetChild(3).GetComponent<Image>().color = Color.white;



                            spawnedButtons[i].gameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = new Color32(0, 255, 185, 255);
                            spawnedButtons[i].gameObject.transform.GetChild(3).GetComponent<Image>().color = Color.white;


                            int startIndex = 0;

                            for (int x = 0; x < showroomManager.useCases.Count; x++)
                            {

                                if (x == newUseCaseIndex)
                                {

                                    for (int buttonIndex = 0; buttonIndex < showroomManager.useCases[x].useCaseButtons.Count; buttonIndex++)
                                    {

                                        //Rework

                                        Debug.Log(buttonIndex + " | " + x);

                                        spawnedButtons[buttonIndex + startIndex].gameObject.SetActive(true);

                                    }

                                }
                                else
                                {

                                    startIndex += showroomManager.useCases[x].useCaseButtons.Count;

                                }

                            }

                        }

                        continue;

                    }


                ResetButton(spawnedButtons[i].transform);

            }

        }

        public void ResetAllSidebarHeadButtons(int j)
        {

            for (int i = 0; i < spawnedHeadButtons.Count; i++)
            {
                if (j != -1)
                    if (i == j)
                        continue;

                spawnedHeadButtons[i].ResetClick();
                ResetButton(spawnedHeadButtons[i].transform);

            }

            for (int i = 0; i < spawnedButtons.Count; i++)
            {

                spawnedButtons[i].ResetClick();
                ResetButton(spawnedButtons[i].transform);

                spawnedButtons[i].gameObject.SetActive(false);

            }

            int startIndex = 0;

            for (int i = 0; i < showroomManager.useCases.Count; i++)
            {

                if (i == j)
                {

                    for (int buttonIndex = 0; buttonIndex < showroomManager.useCases[i].useCaseButtons.Count; buttonIndex++)
                    {

                        //Rework

                        spawnedButtons[buttonIndex + startIndex].gameObject.SetActive(true);

                    }

                }
                else
                {

                    startIndex += showroomManager.useCases[i].useCaseButtons.Count;

                }

            }

            //if (j == -1 && showroomManager.openSidebarMenu)
            //    sidebarRect.DOSizeDelta(sidebarOpenSize, .5f)
            //    .OnStart(() => LayoutRebuilder.ForceRebuildLayoutImmediate(sidebarButtonParent))
            //        .OnComplete(() => LayoutRebuilder.ForceRebuildLayoutImmediate(sidebarButtonParent));

            if (j == -1 && showroomManager.openSidebarMenu)
            {

                LayoutRebuilder.ForceRebuildLayoutImmediate(sidebarButtonParent);
                LayoutRebuilder.ForceRebuildLayoutImmediate(sidebarButtonParent);

            }

        }

        public void ToggleRightMenu(bool isActive)
        {

            Vector2 newRightMenuPos;

            if (isActive)
            {

                newRightMenuPos = rightMenuOpenPos;

                FadeCanvasGroupOut(burgerButtonOutsideCanvasGroup);

            }
            else
            {

                newRightMenuPos = rightMenuClosedPos;

                FadeCanvasGroupOut(burgerButtonOutsideCanvasGroup);

            }


            rightMenuRect.DOAnchorPos(newRightMenuPos, 1f)
                .OnComplete(() =>
                {

                    FinishedMovingRightMenu(isActive);

                    rightMenuIsOpen = isActive;

                });
        }

        public void OpenBulletPointContainer()
        {

            if (bulletPointMenuIsOpen)
                return;

            if (showroomManager.showDebugMessages)
                Debug.Log("Opening BulletPoint Menu");

            bulletPointMenuIsOpen = true;

            LayoutRebuilder.ForceRebuildLayoutImmediate(bulletPointMenuRect);

            bulletPointMenuRect.DOAnchorPos(bulletPointMenuOpenSize, 1f);

        }

        public void CloseBulletPointContainer()
        {

            if (!bulletPointMenuIsOpen)
                return;

            if (showroomManager.showDebugMessages)
                Debug.Log("Closing BulletPoint Menu");

            bulletPointMenuIsOpen = false;

            bulletPointMenuRect.DOAnchorPos(bulletPointMenuClosedSize, 1f);

        }

        public void UpdateBulletPointContainer(int index)
        {

            if (index != -1)
            {

                if (showroomManager.useCaseIndex == -1)
                {

                    if (showroomManager.bulletPoints[index] == null)
                    {

                        FadeCanvasGroupOut(bulletPointMenuToggleButton);

                        return;
                    }

                    if (bulletPointMenuIsOpen)
                    {

                        CloseBulletPointContainer();

                        StartCoroutine(UpdateBulletPointText(showroomManager.bulletPoints[index], true));

                    }
                    else
                        StartCoroutine(UpdateBulletPointText(showroomManager.bulletPoints[index], true));

                }
                else
                {

                    if (showroomManager.useCases[showroomManager.useCaseIndex].bulletPoints[index] == null)
                    {

                        FadeCanvasGroupOut(bulletPointMenuToggleButton);

                        return;
                    }

                    if (bulletPointMenuIsOpen)
                    {

                        CloseBulletPointContainer();

                        StartCoroutine(UpdateBulletPointText(showroomManager.useCases[showroomManager.useCaseIndex].bulletPoints[index], true));

                    }
                    else
                        StartCoroutine(UpdateBulletPointText(showroomManager.useCases[showroomManager.useCaseIndex].bulletPoints[index], true));

                }

            }
            else
            {

                CloseBulletPointContainer();

            }

        }

        IEnumerator UpdateBulletPointText(BulletPoint bulletPoint, bool openAfterwards)
        {

            if (showroomManager.showDebugMessages)
                Debug.Log("Updating text | " + bulletPoint + " | " + openAfterwards);

            yield return new WaitForSecondsRealtime(.5f);

            bulletPointMenuHeadline.text = bulletPoint.bulletPointTitle;
            bulletPointMenuSubHeadline.text = bulletPoint.bulletPointSubTitle;


            string sspTextString = bulletPoint.bulletPointText.Replace("*", "•").Replace("\\n\\n", "\n\n<indent=0%>").Replace("\\n", "\n<indent=7%>");

            string displayReadyString = sspTextString.Replace("•", "<color=#00ffb9>•   </color>");


            bulletPointMenuText.text = displayReadyString;


            if (string.IsNullOrEmpty(bulletPoint.bulletPointText))
                bulletPointMenuText.gameObject.SetActive(false);
            else
                bulletPointMenuText.gameObject.SetActive(true);

            if (string.IsNullOrEmpty(bulletPoint.bulletPointSubTitle))
                bulletPointMenuSubHeadline.gameObject.SetActive(false);
            else
                bulletPointMenuSubHeadline.gameObject.SetActive(true);

            if (string.IsNullOrEmpty(bulletPoint.bulletPointTitle))
                bulletPointMenuHeadline.gameObject.SetActive(false);
            else
                bulletPointMenuHeadline.gameObject.SetActive(true);


            if (bulletPointMenuHeadline.gameObject.activeSelf || bulletPointMenuSubHeadline.gameObject.activeSelf)
            {
                bulletPointMenuHeadline.transform.parent.gameObject.SetActive(true);
            }
            else if (!bulletPointMenuHeadline.gameObject.activeSelf && !bulletPointMenuSubHeadline.gameObject.activeSelf)
            {
                bulletPointMenuHeadline.transform.parent.gameObject.SetActive(false);
            }

            FadeCanvasGroupIn(bulletPointMenuToggleButton);

            LayoutRebuilder.ForceRebuildLayoutImmediate(bulletPointMenuRect);

            if (openAfterwards)
                OpenBulletPointContainer();

        }

        void FinishedMovingRightMenu(bool isActive)
        {

            if (!isActive)
            {

                rightMenuBackButton.gameObject.SetActive(true);
                rightMenuResetRotationButton.gameObject.SetActive(true);
                rightMenuRotatedButton.gameObject.SetActive(false);

            }

        }

        public void FadeCanvasGroupIn(CanvasGroup canvas)
        {

            canvas.DOFade(1f, .5f).OnComplete(() => FinishFadingGroup(canvas, true));

        }

        public void FadeCanvasGroupOut(CanvasGroup canvas)
        {

            canvas.DOFade(0f, .5f).OnComplete(() => FinishFadingGroup(canvas, false));

        }

        void FinishFadingGroup(CanvasGroup canvas, bool isActive)
        {

            canvas.blocksRaycasts = isActive;
            canvas.interactable = isActive;

        }

        public void MoveSidebarOffScreen()
        {

            //Vector2 offScreenPos = new Vector2(-500f, -50f);

            sidebarParentRect.DOAnchorPos(sidebarClosePos, 1f);

            FadeCanvasGroupIn(burgerButtonOutsideCanvasGroup);

            bool removeGeneralMenu = true;

            if (showroomManager.useCaseIndex != -1)
            {

                if (showroomManager.useCases[showroomManager.useCaseIndex].hasTimelineStepper)
                {
                    if (!timelineStepperIsOpen)
                        OpenTimelineStepper();
                    else
                        ScaleTimelineStepperUp();

                    removeGeneralMenu = false;
                }

            }
            else
            {

                if (showroomManager.hasTimelineStepper)
                {
                    if (!timelineStepperIsOpen)
                        OpenTimelineStepper();
                    else
                        ScaleTimelineStepperUp();

                    removeGeneralMenu = false;
                }

            }

            if (removeGeneralMenu)
                MoveGeneralMenuOffScreen();

        }

        public void MoveSidebarOntoScreen()
        {

            //Vector2 offScreenPos = new Vector2(50f, -50f);

            sidebarParentRect.DOAnchorPos(sidebarOpenPos, 1f);

            FadeCanvasGroupOut(burgerButtonOutsideCanvasGroup);

            if (timelineStepperIsOpen)
                ScaleTimelineStepperDown();

            MoveGeneralMenuOntoScreen();

        }

        public void MoveGeneralMenuOffScreen()
        {

            generalMenuRect.DOAnchorPos(generalMenuClosedPos, 1f);

        }

        public void MoveGeneralMenuOntoScreen()
        {

            generalMenuRect.DOAnchorPos(generalMenuOpenPos, 1f);

        }

        public void MoveCloseButtonOffScreen()
        {

            //closeButtonRect.DOAnchorPos(closeButtonOffScreenPos, 1f);

        }

        public void MoveCloseButtonOntoScreen()
        {

            closeButtonRect.DOAnchorPos(closeButtonOnScreenPos, 1f);

        }

        public void EnableCameraDropDown()
        {

            generalMenuCameraPosDropdown.gameObject.GetComponent<CanvasGroup>().alpha = 1f;

            if (showroomManager.useCaseIndex == -1)
            {

                if (showroomManager.cameraButtons.Count > 5)
                {

                    generalMenuCameraPosDropdown.GetChild(0).GetChild(0).GetComponent<VerticalLayoutGroup>().enabled = false;
                    generalMenuCameraPosDropdown.GetChild(0).GetChild(0).GetComponent<LayoutElement>().enabled = true;

                }
                else
                {

                    generalMenuCameraPosDropdown.GetChild(0).GetChild(0).GetComponent<VerticalLayoutGroup>().enabled = true;
                    generalMenuCameraPosDropdown.GetChild(0).GetChild(0).GetComponent<LayoutElement>().enabled = false;

                }

            }
            else
            {

                if (showroomManager.cameraButtons.Count > 5)
                {

                    generalMenuCameraPosDropdown.GetChild(0).GetChild(0).GetComponent<VerticalLayoutGroup>().enabled = false;
                    generalMenuCameraPosDropdown.GetChild(0).GetChild(0).GetComponent<LayoutElement>().enabled = true;

                }
                else
                {

                    generalMenuCameraPosDropdown.GetChild(0).GetChild(0).GetComponent<VerticalLayoutGroup>().enabled = true;
                    generalMenuCameraPosDropdown.GetChild(0).GetChild(0).GetComponent<LayoutElement>().enabled = false;

                }

            }

        }

        public void DisableCameraDropDown()
        {

            generalMenuCameraPosDropdown.gameObject.GetComponent<CanvasGroup>().alpha = 0f;

        }

        public void EnableTooltip(RectTransform pos)
        {

            tooltipPanelRect.gameObject.SetActive(true);

            Vector2 screenSpacePos = pos.position;

            if (Mouse.current.position.ReadValue().x >= (currentResolution.x * .85f))
            {

                tooltipPanelShape.RoundedProperties.BLRadius = 16f;
                tooltipPanelShape.RoundedProperties.TLRadius = 16f;
                tooltipPanelShape.RoundedProperties.BRRadius = 0f;
                tooltipPanelShape.RoundedProperties.TRRadius = 0f;


                if (Mouse.current.position.ReadValue().y >= (currentResolution.y * .8f))
                    tooltipPanelRect.pivot = new Vector2(1.375f, 0.5f);
                else
                    tooltipPanelRect.pivot = new Vector2(1.12f, 0.5f);

                screenSpacePos = new Vector2(screenSpacePos.x, screenSpacePos.y);
            }
            else
            {

                if (onlyOneGeneralMenuButtonActive)
                {

                    tooltipPanelRect.pivot = new Vector2(-0.11f, 0.5f);

                    tooltipPanelShape.RoundedProperties.BLRadius = 0f;
                    tooltipPanelShape.RoundedProperties.TLRadius = 0f;
                    tooltipPanelShape.RoundedProperties.BRRadius = 16f;
                    tooltipPanelShape.RoundedProperties.TRRadius = 16f;

                }
                else
                {

                    tooltipPanelRect.pivot = new Vector2(0.5f, -0.86f);

                    tooltipPanelShape.RoundedProperties.BLRadius = 16f;
                    tooltipPanelShape.RoundedProperties.TLRadius = 16f;
                    tooltipPanelShape.RoundedProperties.BRRadius = 16f;
                    tooltipPanelShape.RoundedProperties.TRRadius = 16f;

                }

                screenSpacePos = new Vector2(screenSpacePos.x, screenSpacePos.y);
            }

            tooltipPanelRect.position = screenSpacePos;

        }

        public void UpdateToolTipText(string text)
        {

            #region Tooltip Code Builder Function


            string tempAttributeName = "";


            for (int i = 0; i < tooltipAttributes.Count; i++)
            {

                if (string.Equals(text, tooltipAttributes[i].tooltipName))
                {

                    if (tooltipAttributes[i].useCodeSnippet)
                    {

                        //Black magic to determine what text to display

                        switch (tooltipAttributes[i].tooltipName)
                        {
                            case "None":
                                tempAttributeName = tooltipAttributes[i].tooltipMessage;
                                break;
                            case "$Home":
                                if (showroomManager.isAtUseCaseHomePos || showroomManager.useCaseIndex == -1)
                                    tempAttributeName = tooltipAttributes[i].variable1ReturnValue;
                                else
                                    tempAttributeName = tooltipAttributes[i].tooltipMessageReturnValue;
                                break;
                            case "$Transparent":
                                if (!showroomManager.isTransparent)
                                    tempAttributeName = tooltipAttributes[i].variable1ReturnValue;
                                else
                                    tempAttributeName = tooltipAttributes[i].tooltipMessageReturnValue;
                                break;

                        }

                    }
                    else
                        tempAttributeName = tooltipAttributes[i].tooltipMessage;


                    break;
                }
                else
                    continue;

            }

            if (tempAttributeName == "" || string.IsNullOrEmpty(tempAttributeName))
                tooltipText.text = text;
            else
                tooltipText.text = tempAttributeName;


            #endregion


            LayoutRebuilder.ForceRebuildLayoutImmediate(tooltipPanelRect);

        }

        public void Enable3DTooltip(Transform pos)
        {

            tooltip3DPanelRect.gameObject.SetActive(true);

            tooltip3DPanelShape.RoundedProperties.BLRadius = 12f;
            tooltip3DPanelShape.RoundedProperties.TLRadius = 12f;
            tooltip3DPanelShape.RoundedProperties.BRRadius = 0f;
            tooltip3DPanelShape.RoundedProperties.TRRadius = 0f;

            tooltip3DPanelRect.pivot = new Vector2(1f, 0.5f);
            tooltip3DPanelRect.localScale = pos.localScale;

            tooltip3DPanelRect.position = pos.position;

        }

        public void Update3DTooltipPanel(string text)
        {

            tooltip3DPanelText.text = text;

            LayoutRebuilder.ForceRebuildLayoutImmediate(tooltip3DPanelRect);

        }

        public void SwitchActiveCameraButton(int buttonIndex)
        {

            int index = buttonIndex;


            if (index != -1)
            {

                generalMenuCameraPosButton.GetComponent<Image>().sprite = generalMenuCameraPosButtonSprites[1];
                generalMenuCameraPosButton.transform.GetChild(2).GetComponent<TextMeshProUGUI>().gameObject.SetActive(true);
                generalMenuCameraPosButton.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "<color=#ffffff>" + (index + 1).ToString() + "</color>";


                for (int i = 0; i < generalMenuCameraPosButtons.Count; i++)
                {

                    generalMenuCameraPosButtons[i].GetComponent<Image>().sprite = generalMenuCameraPosButtonSprites[0];
                    generalMenuCameraPosButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = "<color=#2d373c>" + (i + 1).ToString() + "</color>";

                }


                generalMenuCameraPosButtons[index].GetComponent<Image>().sprite = generalMenuCameraPosButtonSprites[1];
                generalMenuCameraPosButtons[index].GetComponentInChildren<TextMeshProUGUI>().text = "<color=#ffffff>" + (index + 1).ToString() + "</color>";

                if (showroomManager.useCaseIndex != -1)
                {

                    showroomManager.OnNewCamPos(showroomManager.useCases[showroomManager.useCaseIndex].cameraButtons[buttonIndex].cameraPositionButtonCamera.transform);

                }
                else
                {

                    showroomManager.OnNewCamPos(showroomManager.cameraButtons[buttonIndex].cameraPositionButtonCamera.transform);

                }

                generalMenuCameraPosDropdown.gameObject.GetComponent<CanvasGroup>().alpha = 0f;

            }
            else
            {

                generalMenuCameraPosButton.GetComponent<Image>().sprite = generalMenuCameraPosButtonSprites[0];
                generalMenuCameraPosButton.transform.GetChild(2).GetComponent<TextMeshProUGUI>().gameObject.SetActive(false);

                ResetAllCameraButtons();

            }



        }

        public void ResetAllCameraButtons()
        {

            //generalMenuCameraPosButton.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "<color=#ffffff>" + (index + 1).ToString() + "</color>";

            for (int i = 0; i < generalMenuCameraPosButtons.Count; i++)
            {

                generalMenuCameraPosButtons[i].GetComponent<Image>().sprite = generalMenuCameraPosButtonSprites[0];
                generalMenuCameraPosButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = "<color=#2d373c>" + (i + 1).ToString() + "</color>";

            }

        }

        public void OpenTimelineStepper()
        {

            timelineStepperRect.DOAnchorPos(timelineStepperOpenPos, .5f).OnComplete(() => timelineStepperIsOpen = true);

            if (showroomManager.showDebugMessages)
                Debug.Log("Opening Timeline Stepper UI!");

            UpdateStepperUI();

        }

        public void CloseTimelineStepper()
        {

            timelineStepperRect.DOAnchorPos(timelineStepperClosedPos, .5f).OnComplete(() => timelineStepperIsOpen = false);

        }

        public void ScaleTimelineStepperUp()
        {

            float spacing = 74;

            DOTween.To(() => spacing, x => spacing = x, 216, 1).OnUpdate(() =>
            {

                timelineStepperParent.gameObject.GetComponent<HorizontalLayoutGroup>().spacing = spacing;

            });

            timelineStepperRect.DOSizeDelta(new Vector2(1380f, 156f), .5f);

            for (int i = 0; i < spawnedStepperButtons.Count; i++)
            {

                spawnedStepperButtons[i].transform.GetChild(1).GetComponent<CanvasGroup>().DOFade(1f, .5f);

            }

        }

        public void ScaleTimelineStepperDown()
        {

            float spacing = 216;

            DOTween.To(() => spacing, x => spacing = x, 74, 1).OnUpdate(() =>
            {

                timelineStepperParent.gameObject.GetComponent<HorizontalLayoutGroup>().spacing = spacing;

            });

            timelineStepperRect.DOSizeDelta(new Vector2(750f, 156f), .5f);

            for (int i = 0; i < spawnedStepperButtons.Count; i++)
            {

                spawnedStepperButtons[i].transform.GetChild(1).GetComponent<CanvasGroup>().DOFade(0f, .5f);

            }

        }

        [Button]
        public void ToggleTimelineStepper()
        {

            timelineStepperIsOpen = !timelineStepperIsOpen;

            if (timelineStepperIsOpen)
            {

                OpenTimelineStepper();

            }
            else
            {

                CloseTimelineStepper();

            }

        }

        public void TimelineStepperChevron(bool isRight)
        {

            if (isRight)
            {

                timelineStepperParent.GetComponent<RectTransform>().DOAnchorPosX(-262f, .5f).SetRelative();

            }
            else
            {

                timelineStepperParent.GetComponent<RectTransform>().DOAnchorPosX(262f, .5f).SetRelative();

            }

        }

        public void TimelineCheckPos(Scrollbar scrollbar)
        {

            if (showroomManager.timelineSteps.Count < 5)
                return;

            if (scrollbar.value == 0)
            {

                timelineStepperLeftChevronRect.gameObject.SetActive(false);
                timelineStepperRightChevronRect.gameObject.SetActive(true);

            }
            else if (scrollbar.value >= .95f)
            {

                timelineStepperLeftChevronRect.gameObject.SetActive(true);
                timelineStepperRightChevronRect.gameObject.SetActive(false);

            }
            else if (scrollbar.value < .95f && scrollbar.value > 0)
            {

                timelineStepperLeftChevronRect.gameObject.SetActive(true);
                timelineStepperRightChevronRect.gameObject.SetActive(true);

            }

        }

        public void TimelineStepPointOnHover(Transform stepButton)
        {

            Ellipse buttonEllipse = stepButton.GetComponent<Ellipse>();
            TextMeshProUGUI buttonText = stepButton.GetComponentInChildren<TextMeshProUGUI>();

            buttonEllipse.Sprite = null;

            buttonEllipse.ShapeProperties.DrawOutline = false;

            buttonEllipse.ShapeProperties.FillColor = new Color32(0, 255, 185, 255);

            buttonEllipse.ForceMeshUpdate();

            string[] s = buttonText.text.Split('>');

            int index = int.Parse(s[1]);

            buttonText.text = "<color=#333353>" + (index).ToString();

            return;

        }

        public void TimelineStepPointOnExit(Transform stepButton)
        {

            Ellipse buttonEllipse = stepButton.GetComponent<Ellipse>();
            TextMeshProUGUI buttonText = stepButton.GetComponentInChildren<TextMeshProUGUI>();

            string[] s = buttonText.text.Split('>');

            int index = int.Parse(s[1]);

            if (showroomManager.showDebugMessages)
                Debug.Log("Timeline Stepper Index: " + (index - 1).ToString() + " | Showroom Manager Timeline Index: " + (showroomManager.timelineStepperIndex));

            if (index - 1 > showroomManager.timelineStepperIndex)
            {

                //Not Watched
                if (showroomManager.showDebugMessages)
                    Debug.Log("Button is set to not watched");

                buttonText.text = "<color=#333353>" + (index).ToString();

                buttonEllipse.Sprite = null;

                buttonEllipse.ShapeProperties.DrawOutline = false;

                buttonEllipse.ShapeProperties.FillColor = new Color32(1, 31, 57, 255);

                buttonEllipse.ForceMeshUpdate();

                return;

            }
            else if (index - 1 < showroomManager.timelineStepperIndex)
            {

                //Watched
                if (showroomManager.showDebugMessages)
                    Debug.Log("Button is set to watched");

                buttonText.text = "<color=#00ffb9>" + (index).ToString();

                buttonEllipse.Sprite = null;

                buttonEllipse.ShapeProperties.DrawOutline = true;

                buttonEllipse.ShapeProperties.FillColor = new Color32(1, 31, 57, 255);

                buttonEllipse.ForceMeshUpdate();

                return;

            }
            else if (index - 1 == showroomManager.timelineStepperIndex)
            {

                //Current
                if (showroomManager.showDebugMessages)
                    Debug.Log("Button is set to Active");

                buttonText.text = "<color=#333353>" + (index).ToString();

                buttonEllipse.Sprite = timelineStepperButtonSpriteClicked;

                buttonEllipse.ShapeProperties.DrawOutline = false;

                buttonEllipse.ShapeProperties.FillColor = new Color32(255, 255, 255, 255);

                buttonEllipse.ForceMeshUpdate();

                return;

            }



        }

        public void UpdateStepperUI()
        {

            if (showroomManager.showDebugMessages)
                Debug.Log("Updating Timeline Stepper UI!");

            for (int i = 0; i < spawnedStepperButtons.Count; i++)
            {

                Destroy(spawnedStepperButtons[i].gameObject);

            }

            spawnedStepperButtons.Clear();


            if (showroomManager.useCaseIndex == -1)
            {

                if (showroomManager.hasTimelineStepper)
                {

                    //showroomManager.timelineSteps.Count

                    timelineStepperSlider.maxValue = showroomManager.timelineSteps.Count;// - 1;

                    if (showroomManager.timelineSteps.Count > 5)
                    {

                        timelineStepperRightChevronRect.gameObject.SetActive(true);
                        timelineStepperLeftChevronRect.gameObject.SetActive(true);

                    }
                    else
                    {

                        timelineStepperRightChevronRect.gameObject.SetActive(false);
                        timelineStepperLeftChevronRect.gameObject.SetActive(false);

                    }


                    for (int i = 0; i < showroomManager.timelineSteps.Count; i++)
                    {

                        int currentTimelineStepperButtonIndex = i;

                        if (showroomManager.showDebugMessages)
                            Debug.Log("Spawned Timeline Stepper Button: " + currentTimelineStepperButtonIndex);

                        GameObject newTimelineStepper = Instantiate(timelineStepPointPrefab, timelineStepperParent);

                        ButtonBehavior newTimelineStepperButton = newTimelineStepper.GetComponent<ButtonBehavior>();

                        TextMeshProUGUI newTimelineStepperText = newTimelineStepper.transform.GetChild(0).GetComponent<TextMeshProUGUI>();

                        TextMeshProUGUI newTimelineStepperInfoText = newTimelineStepper.transform.GetChild(1).GetComponent<TextMeshProUGUI>();

                        Ellipse newTimelineStepperEllipse = newTimelineStepper.GetComponent<Ellipse>();


                        spawnedStepperButtons.Add(newTimelineStepperButton);


                        newTimelineStepperText.text = "<color=#333353>" + (currentTimelineStepperButtonIndex + 1).ToString();

                        newTimelineStepperInfoText.text = showroomManager.timelineSteps[currentTimelineStepperButtonIndex].infoText;


                        #region Button Functions

                        #region OnClick

                        UnityEvent onClickEventTimelineStepperButton = new UnityEvent();

                        onClickEventTimelineStepperButton.AddListener(() =>
                        {

                            int f = currentTimelineStepperButtonIndex;

                            if (showroomManager.showDebugMessages)
                                Debug.Log("Clicked Timeline Stepper Button: " + currentTimelineStepperButtonIndex);

                            timelineStepperSlider.DOValue(currentTimelineStepperButtonIndex, 1f);

                            Ellipse buttonEllipse = spawnedStepperButtons[f].GetComponent<Ellipse>();
                            TextMeshProUGUI buttonText = spawnedStepperButtons[f].GetComponentInChildren<TextMeshProUGUI>();

                            buttonEllipse.Sprite = timelineStepperButtonSpriteClicked;

                            buttonEllipse.ShapeProperties.DrawOutline = false;

                            buttonEllipse.ShapeProperties.FillColor = new Color32(255, 255, 255, 255);

                            buttonEllipse.ForceMeshUpdate();

                            buttonText.text = "<color=#333353>" + (f + 1).ToString();

                            for (int j = (currentTimelineStepperButtonIndex + 1); j < showroomManager.timelineSteps.Count; j++)
                            {

                                Ellipse spawnedEllipse = spawnedStepperButtons[j].GetComponent<Ellipse>();
                                TextMeshProUGUI spawnedText = spawnedStepperButtons[j].GetComponentInChildren<TextMeshProUGUI>();

                                spawnedEllipse.Sprite = null;

                                spawnedEllipse.ShapeProperties.DrawOutline = false;

                                spawnedText.text = "<color=#333353>" + (j + 1).ToString();

                                spawnedEllipse.ShapeProperties.FillColor = new Color32(1, 31, 57, 255);

                                spawnedEllipse.ForceMeshUpdate();

                                if (showroomManager.showDebugMessages)
                                    Debug.Log("Setting Button: " + j + " to not watched");


                            }

                            for (int j = currentTimelineStepperButtonIndex - 1; j > -1; j--)
                            {

                                Ellipse spawnedEllipse = spawnedStepperButtons[j].GetComponent<Ellipse>();
                                TextMeshProUGUI spawnedText = spawnedStepperButtons[j].GetComponentInChildren<TextMeshProUGUI>();

                                spawnedEllipse.Sprite = null;

                                spawnedEllipse.ShapeProperties.DrawOutline = true;

                                spawnedText.text = "<color=#00ffb9>" + (j + 1).ToString();

                                spawnedEllipse.ShapeProperties.FillColor = new Color32(1, 31, 57, 255);

                                spawnedEllipse.ForceMeshUpdate();

                                if (showroomManager.showDebugMessages)
                                    Debug.Log("Setting Button: " + j + " to already watched");

                            }

                            if (showroomManager.timelineSteps[currentTimelineStepperButtonIndex].timeline != null)
                            {

                                showroomManager.timelineSteps[currentTimelineStepperButtonIndex].timeline.Stop();
                                //showroomManager.timelineSteps[currentTimelineStepperButtonIndex].timeline.Evaluate();
                                //
                                showroomManager.timelineSteps[currentTimelineStepperButtonIndex].timeline.Play();

                            }

                            //Reverse Timeline (only the currently active one-> save time + performance)

                            int lastIndex = showroomManager.timelineStepperIndex;

                            showroomManager.timelineStepperIndex = currentTimelineStepperButtonIndex;

                            TimelineStepperButtonSetUp(currentTimelineStepperButtonIndex, lastIndex);

                            if (usesBlackFade)
                                blackFade.DOFade(1f, .25f).OnComplete(() => blackFade.DOFade(0f, .25f).SetDelay(.75f));

                        });

                        Function onClickEventTimelineStepperButtonFunction = new Function
                        {
                            functionName = onClickEventTimelineStepperButton,
                            functionDelay = 0f
                        };

                        newTimelineStepperButton.onMouseDown.AddRange(showroomManager.timelineSteps[i].functions);

                        newTimelineStepperButton.onMouseDown.Add(onClickEventTimelineStepperButtonFunction);

                        #endregion

                        #region OnHover

                        UnityEvent onHoverEventTimelineStepperButton = new UnityEvent();

                        onHoverEventTimelineStepperButton.AddListener(() =>
                        {

                            int f = currentTimelineStepperButtonIndex;

                            this.TimelineStepPointOnHover(spawnedStepperButtons[f].transform);

                        });

                        Function onHoverEventTimelineStepperButtonFunction = new Function
                        {
                            functionName = onHoverEventTimelineStepperButton,
                            functionDelay = 0f
                        };

                        newTimelineStepperButton.onMouseEnter.Add(onHoverEventTimelineStepperButtonFunction);

                        #endregion

                        #region onExit

                        UnityEvent onExitEventTimelineStepperButton = new UnityEvent();

                        onExitEventTimelineStepperButton.AddListener(() =>
                        {

                            int f = currentTimelineStepperButtonIndex;

                            this.TimelineStepPointOnExit(spawnedStepperButtons[f].transform);

                        });

                        Function onExitEventTimelineStepperButtonFunction = new Function
                        {
                            functionName = onExitEventTimelineStepperButton,
                            functionDelay = 0f
                        };

                        newTimelineStepperButton.onMouseExit.Add(onExitEventTimelineStepperButtonFunction);

                        #endregion

                        #endregion

                    }

                    if (showroomManager.timelineStepperAutoPlay)
                    {

                        if (generalMenuPlayButton.gameObject.activeSelf)
                            generalMenuPlayButton.OnMouseDown();

                    }

                    GameObject timeLineStepperEndMarker = Instantiate(timelineStepEndMarkerPrefab, timelineStepperParent);

                }

            }
            else
            {

                if (showroomManager.useCases[showroomManager.useCaseIndex].hasTimelineStepper)
                {

                    if (showroomManager.showDebugMessages)
                        Debug.Log("Use Case has Timeline Stepper!" + showroomManager.useCaseIndex);

                    timelineStepperSlider.maxValue = showroomManager.useCases[showroomManager.useCaseIndex].timelineSteps.Count; // - 1;

                    if (showroomManager.useCases[showroomManager.useCaseIndex].timelineSteps.Count > 5)
                    {

                        timelineStepperRightChevronRect.gameObject.SetActive(true);
                        timelineStepperLeftChevronRect.gameObject.SetActive(true);

                    }
                    else
                    {

                        timelineStepperRightChevronRect.gameObject.SetActive(false);
                        timelineStepperLeftChevronRect.gameObject.SetActive(false);

                    }


                    for (int i = 0; i < showroomManager.useCases[showroomManager.useCaseIndex].timelineSteps.Count; i++)
                    {

                        int currentTimelineStepperButtonIndex = i;

                        if (showroomManager.showDebugMessages)
                            Debug.Log("Spawned Timeline Stepper Button: " + currentTimelineStepperButtonIndex);

                        GameObject newTimelineStepper = Instantiate(timelineStepPointPrefab, timelineStepperParent);

                        ButtonBehavior newTimelineStepperButton = newTimelineStepper.GetComponent<ButtonBehavior>();

                        TextMeshProUGUI newTimelineStepperText = newTimelineStepper.transform.GetChild(0).GetComponent<TextMeshProUGUI>();

                        TextMeshProUGUI newTimelineStepperInfoText = newTimelineStepper.transform.GetChild(1).GetComponent<TextMeshProUGUI>();

                        Ellipse newTimelineStepperEllipse = newTimelineStepper.GetComponent<Ellipse>();


                        spawnedStepperButtons.Add(newTimelineStepperButton);


                        newTimelineStepperText.text = "<color=#333353>" + (currentTimelineStepperButtonIndex + 1).ToString();

                        newTimelineStepperInfoText.text = showroomManager.useCases[showroomManager.useCaseIndex].timelineSteps[currentTimelineStepperButtonIndex].infoText;


                        #region Button Functions

                        #region OnClick

                        UnityEvent onClickEventTimelineStepperButton = new UnityEvent();

                        onClickEventTimelineStepperButton.AddListener(() =>
                        {

                            int f = currentTimelineStepperButtonIndex;

                            if (showroomManager.showDebugMessages)
                                Debug.Log("Clicked Timeline Stepper Button: " + currentTimelineStepperButtonIndex);

                            timelineStepperSlider.DOValue(currentTimelineStepperButtonIndex, 1f);

                            Ellipse buttonEllipse = spawnedStepperButtons[f].GetComponent<Ellipse>();
                            TextMeshProUGUI buttonText = spawnedStepperButtons[f].GetComponentInChildren<TextMeshProUGUI>();

                            buttonEllipse.Sprite = timelineStepperButtonSpriteClicked;

                            buttonEllipse.ShapeProperties.DrawOutline = false;

                            buttonEllipse.ShapeProperties.FillColor = new Color32(255, 255, 255, 255);

                            buttonEllipse.ForceMeshUpdate();

                            buttonText.text = "<color=#333353>" + (f + 1).ToString();

                            for (int j = (currentTimelineStepperButtonIndex + 1); j < showroomManager.useCases[showroomManager.useCaseIndex].timelineSteps.Count; j++)
                            {

                                Ellipse spawnedEllipse = spawnedStepperButtons[j].GetComponent<Ellipse>();
                                TextMeshProUGUI spawnedText = spawnedStepperButtons[j].GetComponentInChildren<TextMeshProUGUI>();

                                spawnedEllipse.Sprite = null;

                                spawnedEllipse.ShapeProperties.DrawOutline = false;

                                spawnedText.text = "<color=#333353>" + (j + 1).ToString();

                                spawnedEllipse.ShapeProperties.FillColor = new Color32(1, 31, 57, 255);

                                spawnedEllipse.ForceMeshUpdate();

                                if (showroomManager.showDebugMessages)
                                    Debug.Log("Setting Button: " + j + " to not watched");


                            }

                            for (int j = currentTimelineStepperButtonIndex - 1; j > -1; j--)
                            {

                                Ellipse spawnedEllipse = spawnedStepperButtons[j].GetComponent<Ellipse>();
                                TextMeshProUGUI spawnedText = spawnedStepperButtons[j].GetComponentInChildren<TextMeshProUGUI>();

                                spawnedEllipse.Sprite = null;

                                spawnedEllipse.ShapeProperties.DrawOutline = true;

                                spawnedText.text = "<color=#00ffb9>" + (j + 1).ToString();

                                spawnedEllipse.ShapeProperties.FillColor = new Color32(1, 31, 57, 255);

                                spawnedEllipse.ForceMeshUpdate();

                                if (showroomManager.showDebugMessages)
                                    Debug.Log("Setting Button: " + j + " to already watched");

                            }

                            if (showroomManager.useCases[showroomManager.useCaseIndex].timelineSteps[currentTimelineStepperButtonIndex].timeline != null)
                            {

                                showroomManager.useCases[showroomManager.useCaseIndex].timelineSteps[currentTimelineStepperButtonIndex].timeline.Stop();
                                //showroomManager.timelineSteps[currentTimelineStepperButtonIndex].timeline.Evaluate();
                                //
                                showroomManager.useCases[showroomManager.useCaseIndex].timelineSteps[currentTimelineStepperButtonIndex].timeline.Play();

                            }

                            //Reverse Timeline (only the currently active one-> save time + performance)

                            int lastIndex = showroomManager.timelineStepperIndex;

                            showroomManager.timelineStepperIndex = currentTimelineStepperButtonIndex;

                            TimelineStepperButtonSetUp(currentTimelineStepperButtonIndex, lastIndex);

                            if (usesBlackFade)
                                blackFade.DOFade(1f, .25f).OnComplete(() => blackFade.DOFade(0f, .25f).SetDelay(.75f));

                        });

                        Function onClickEventTimelineStepperButtonFunction = new Function
                        {
                            functionName = onClickEventTimelineStepperButton,
                            functionDelay = 0f
                        };

                        newTimelineStepperButton.onMouseDown.AddRange(showroomManager.useCases[showroomManager.useCaseIndex].timelineSteps[i].functions);

                        newTimelineStepperButton.onMouseDown.Add(onClickEventTimelineStepperButtonFunction);

                        #endregion

                        #region OnHover

                        UnityEvent onHoverEventTimelineStepperButton = new UnityEvent();

                        onHoverEventTimelineStepperButton.AddListener(() =>
                        {

                            int f = currentTimelineStepperButtonIndex;

                            this.TimelineStepPointOnHover(spawnedStepperButtons[f].transform);

                        });

                        Function onHoverEventTimelineStepperButtonFunction = new Function
                        {
                            functionName = onHoverEventTimelineStepperButton,
                            functionDelay = 0f
                        };

                        newTimelineStepperButton.onMouseEnter.Add(onHoverEventTimelineStepperButtonFunction);

                        #endregion

                        #region onExit

                        UnityEvent onExitEventTimelineStepperButton = new UnityEvent();

                        onExitEventTimelineStepperButton.AddListener(() =>
                        {

                            int f = currentTimelineStepperButtonIndex;

                            this.TimelineStepPointOnExit(spawnedStepperButtons[f].transform);

                        });

                        Function onExitEventTimelineStepperButtonFunction = new Function
                        {
                            functionName = onExitEventTimelineStepperButton,
                            functionDelay = 0f
                        };

                        newTimelineStepperButton.onMouseExit.Add(onExitEventTimelineStepperButtonFunction);

                        #endregion

                        #endregion

                    }

                    if (showroomManager.useCases[showroomManager.useCaseIndex].timelineStepperAutoPlay)
                    {

                        //spawnedStepperButtons[0].OnMouseDown();
                        if (generalMenuPlayButton.gameObject.activeSelf)
                            generalMenuPlayButton.OnMouseDown();

                    }

                    GameObject timeLineStepperEndMarker = Instantiate(timelineStepEndMarkerPrefab, timelineStepperParent);

                }

            }

        }

        public void TimelineStepperButtonSetUp(int newIndex, int lastIndex, bool watchedToNewIndex = false)
        {

            Debug.Log("NewIndex " + newIndex + " | lastIndex " + lastIndex);

            if (showroomManager.useCaseIndex == -1)
            {

                if (showroomManager.showDebugMessages)
                    Debug.Log("Timeline Stepper Button " + newIndex + "was pressed!");

                bool isRewinding = false;

                if (showroomManager.showDebugMessages)
                    Debug.Log("LastIndex: " + lastIndex + " | New Index: " + newIndex + " | Timeline Slider Value: " + timelineStepperSlider.value);

                showroomManager.timelineOldIndex = lastIndex;

                if (newIndex > lastIndex)
                {

                    isRewinding = false;

                    if ((lastIndex + 1) == newIndex && timelineStepperSlider.value >= lastIndex && timelineStepperSlider.value >= lastIndex + .9f)
                        watchedToNewIndex = true;

                    if (showroomManager.showDebugMessages && watchedToNewIndex == false)
                        Debug.Log("Timeline is Fastforwarding");
                    else if (showroomManager.showDebugMessages && watchedToNewIndex)
                        Debug.Log("Timeline is Playing to Next Stepper Point");

                }
                else if (newIndex <= lastIndex)
                {

                    isRewinding = true;

                    if (showroomManager.showDebugMessages)
                        Debug.Log("Timeline is Rewinding");

                }

                if (showroomManager.currentTimeline != null)
                {

                    double timelineTime = showroomManager.currentTimeline.time;

                    if (isRewinding)
                    {

                        Debug.Log("Rewinding");

                        DOTween.To(() => timelineTime, x => timelineTime = x, 0, 1f)
                            .OnStart(() =>
                            {

                                showroomManager.currentTimeline.timeUpdateMode = DirectorUpdateMode.Manual;

                            })
                            .OnUpdate(() =>
                            {

                                showroomManager.currentTimeline.time = timelineTime;

                                showroomManager.currentTimeline.Evaluate();

                            })
                            .OnComplete(() =>
                            {

                                DOTween.Kill("TimelineSlider");

                                showroomManager.timelineStepperIndex = newIndex;
                                showroomManager.currentTimeline = showroomManager.timelineSteps[newIndex].timeline;

                                showroomManager.currentTimeline.Stop();

                                showroomManager.currentTimeline.timeUpdateMode = DirectorUpdateMode.GameTime;

                                if (generalMenuPlayButton.gameObject.activeSelf)
                                    generalMenuPlayButton.OnMouseDown();
                                else
                                    showroomManager.currentTimeline.Play();

                                double newTimelineDuration = showroomManager.currentTimeline.duration;
                                timelineStepperSlider.DOValue(newIndex + 1, (float)newTimelineDuration).SetId("TimelineSlider")
                                    .OnComplete(() =>
                                    {

                                        spawnedStepperButtons[newIndex + 1].OnMouseDown();

                                    });

                            });

                    }
                    else
                    {

                        if (watchedToNewIndex)
                        {

                            Debug.Log("Watched to new Index");

                            DOTween.Kill("TimelineSlider");

                            showroomManager.timelineStepperIndex = newIndex;
                            showroomManager.currentTimeline = showroomManager.timelineSteps[newIndex].timeline;

                            showroomManager.currentTimeline.Stop();

                            showroomManager.currentTimeline.timeUpdateMode = DirectorUpdateMode.GameTime;

                            if (generalMenuPlayButton.gameObject.activeSelf)
                                generalMenuPlayButton.OnMouseDown();
                            else
                                showroomManager.currentTimeline.Play();

                            double newTimelineDuration = showroomManager.currentTimeline.duration;
                            timelineStepperSlider.DOValue(newIndex + 1, (float)newTimelineDuration).SetId("TimelineSlider")
                                .SetDelay(.25f)
                                .OnComplete(() =>
                                {

                                    spawnedStepperButtons[newIndex + 1].OnMouseDown();

                                });

                        }
                        else
                        {

                            Debug.Log("Switch to a new Stepper Timeline");

                            double maxTimelineTime = showroomManager.currentTimeline.duration;

                            DOTween.To(() => timelineTime, x => timelineTime = x, maxTimelineTime, .5f)
                                .SetDelay(.5f)
                                .OnStart(() =>
                                {

                                    showroomManager.currentTimeline.timeUpdateMode = DirectorUpdateMode.Manual;

                                })
                                .OnUpdate(() =>
                                {

                                    showroomManager.currentTimeline.time = timelineTime;

                                    showroomManager.currentTimeline.Evaluate();

                                })
                                .OnComplete(() =>
                                {

                                    DOTween.Kill("TimelineSlider");

                                    showroomManager.timelineStepperIndex = newIndex;
                                    showroomManager.currentTimeline = showroomManager.timelineSteps[newIndex].timeline;

                                    showroomManager.currentTimeline.Stop();

                                    showroomManager.currentTimeline.timeUpdateMode = DirectorUpdateMode.GameTime;

                                    if (generalMenuPlayButton.gameObject.activeSelf)
                                        generalMenuPlayButton.OnMouseDown();
                                    else
                                        showroomManager.currentTimeline.Play();

                                    double newTimelineDuration = showroomManager.currentTimeline.duration;
                                    timelineStepperSlider.DOValue(newIndex + 1, (float)newTimelineDuration).SetId("TimelineSlider")
                                        .SetDelay(.25f)
                                        .OnComplete(() =>
                                        {

                                            spawnedStepperButtons[newIndex + 1].OnMouseDown();

                                        });

                                });

                        }

                    }

                }
                else
                {

                    Debug.Log("New Timeline");

                    showroomManager.currentTimeline = showroomManager.timelineSteps[showroomManager.timelineStepperIndex].timeline;

                    if (generalMenuPlayButton.gameObject.activeSelf)
                        generalMenuPlayButton.OnMouseDown();
                    else if (showroomManager.currentTimeline != null)
                        showroomManager.currentTimeline.Play();

                    DOTween.Kill("TimelineSlider");

                    double newTimelineDuration = showroomManager.currentTimeline.duration;
                    timelineStepperSlider.DOValue(newIndex + 1, (float)newTimelineDuration).SetId("TimelineSlider")
                        .SetDelay(.25f)
                        .OnComplete(() =>
                        {

                            spawnedStepperButtons[newIndex + 1].OnMouseDown();

                        });

                }

            }
            else
            {

                if (showroomManager.showDebugMessages)
                    Debug.Log("Timeline Stepper Button " + newIndex + "was pressed!");

                bool isRewinding = false;

                if (showroomManager.showDebugMessages)
                    Debug.Log("LastIndex: " + lastIndex + " | New Index: " + newIndex + " | Timeline Slider Value: " + timelineStepperSlider.value);

                showroomManager.timelineOldIndex = lastIndex;

                if (newIndex > lastIndex)
                {

                    isRewinding = false;

                    if ((lastIndex + 1) == newIndex && timelineStepperSlider.value >= lastIndex && timelineStepperSlider.value >= lastIndex + .9f)
                        watchedToNewIndex = true;

                    if (showroomManager.showDebugMessages && watchedToNewIndex == false)
                        Debug.Log("Timeline is Fastforwarding");
                    else if (showroomManager.showDebugMessages && watchedToNewIndex)
                        Debug.Log("Timeline is Playing to Next Stepper Point");

                }
                else if (newIndex <= lastIndex)
                {

                    isRewinding = true;

                    if (showroomManager.showDebugMessages)
                        Debug.Log("Timeline is Rewinding");

                }

                if (showroomManager.currentTimeline != null)
                {

                    double timelineTime = showroomManager.currentTimeline.time;

                    if (isRewinding)
                    {

                        Debug.Log("Rewinding");

                        DOTween.To(() => timelineTime, x => timelineTime = x, 0, 1f)
                            .OnStart(() =>
                            {

                                showroomManager.currentTimeline.timeUpdateMode = DirectorUpdateMode.Manual;

                            })
                            .OnUpdate(() =>
                            {

                                showroomManager.currentTimeline.time = timelineTime;

                                showroomManager.currentTimeline.Evaluate();

                            })
                            .OnComplete(() =>
                            {

                                DOTween.Kill("TimelineSlider");

                                showroomManager.timelineStepperIndex = newIndex;
                                showroomManager.currentTimeline = showroomManager.useCases[showroomManager.useCaseIndex].timelineSteps[newIndex].timeline;

                                showroomManager.currentTimeline.Stop();

                                showroomManager.currentTimeline.timeUpdateMode = DirectorUpdateMode.GameTime;

                                if (generalMenuPlayButton.gameObject.activeSelf)
                                    generalMenuPlayButton.OnMouseDown();
                                else
                                    showroomManager.currentTimeline.Play();

                                double newTimelineDuration = showroomManager.currentTimeline.duration;
                                timelineStepperSlider.DOValue(newIndex + 1, (float)newTimelineDuration).SetId("TimelineSlider")
                                    .OnComplete(() =>
                                    {

                                        spawnedStepperButtons[newIndex + 1].OnMouseDown();

                                    });

                            });

                    }
                    else
                    {

                        if (watchedToNewIndex)
                        {

                            Debug.Log("Watched to new Index");

                            DOTween.Kill("TimelineSlider");

                            showroomManager.timelineStepperIndex = newIndex;
                            showroomManager.currentTimeline = showroomManager.useCases[showroomManager.useCaseIndex].timelineSteps[newIndex].timeline;

                            showroomManager.currentTimeline.Stop();

                            showroomManager.currentTimeline.timeUpdateMode = DirectorUpdateMode.GameTime;

                            if (generalMenuPlayButton.gameObject.activeSelf)
                                generalMenuPlayButton.OnMouseDown();
                            else
                                showroomManager.currentTimeline.Play();

                            double newTimelineDuration = showroomManager.currentTimeline.duration;
                            timelineStepperSlider.DOValue(newIndex + 1, (float)newTimelineDuration).SetId("TimelineSlider")
                                .SetDelay(.25f)
                                .OnComplete(() =>
                                {

                                    spawnedStepperButtons[newIndex + 1].OnMouseDown();

                                });

                        }
                        else
                        {

                            Debug.Log("Switch to a new Stepper Timeline");

                            double maxTimelineTime = showroomManager.currentTimeline.duration;

                            DOTween.To(() => timelineTime, x => timelineTime = x, maxTimelineTime, .5f)
                                .SetDelay(.5f)
                                .OnStart(() =>
                                {

                                    showroomManager.currentTimeline.timeUpdateMode = DirectorUpdateMode.Manual;

                                })
                                .OnUpdate(() =>
                                {

                                    showroomManager.currentTimeline.time = timelineTime;

                                    showroomManager.currentTimeline.Evaluate();

                                })
                                .OnComplete(() =>
                                {

                                    DOTween.Kill("TimelineSlider");

                                    showroomManager.timelineStepperIndex = newIndex;
                                    showroomManager.currentTimeline = showroomManager.useCases[showroomManager.useCaseIndex].timelineSteps[newIndex].timeline;

                                    showroomManager.currentTimeline.Stop();

                                    showroomManager.currentTimeline.timeUpdateMode = DirectorUpdateMode.GameTime;

                                    if (generalMenuPlayButton.gameObject.activeSelf)
                                        generalMenuPlayButton.OnMouseDown();
                                    else
                                        showroomManager.currentTimeline.Play();

                                    double newTimelineDuration = showroomManager.currentTimeline.duration;
                                    timelineStepperSlider.DOValue(newIndex + 1, (float)newTimelineDuration).SetId("TimelineSlider")
                                        .SetDelay(.25f)
                                        .OnComplete(() =>
                                        {

                                            spawnedStepperButtons[newIndex + 1].OnMouseDown();

                                        });

                                });

                        }

                    }

                }
                else
                {

                    Debug.Log("New Timeline");

                    showroomManager.currentTimeline = showroomManager.useCases[showroomManager.useCaseIndex].timelineSteps[showroomManager.timelineStepperIndex].timeline;

                    if (generalMenuPlayButton.gameObject.activeSelf)
                        generalMenuPlayButton.OnMouseDown();
                    else if (showroomManager.currentTimeline != null)
                        showroomManager.currentTimeline.Play();

                    DOTween.Kill("TimelineSlider");

                    double newTimelineDuration = showroomManager.currentTimeline.duration;
                    timelineStepperSlider.DOValue(newIndex + 1, (float)newTimelineDuration).SetId("TimelineSlider")
                        .SetDelay(.25f)
                        .OnComplete(() =>
                        {

                            spawnedStepperButtons[newIndex + 1].OnMouseDown();

                        });

                }

            }

        }

        public void QuitButtonOnHover()
        {

            Rectangle rectangle = closeButtonRect.GetComponent<ThisOtherThing.UI.Shapes.Rectangle>();

            rectangle.RoundedProperties.TRRadius = 27f;
            rectangle.RoundedProperties.BRRadius = 27f;
            rectangle.RoundedProperties.BLRadius = 12f;
            rectangle.RoundedProperties.TLRadius = 12f;

            rectangle.ForceMeshUpdate();
            rectangle.SetAllDirty();

        }

        public void QuitButtonOnExit()
        {

            Rectangle rectangle = closeButtonRect.GetComponent<ThisOtherThing.UI.Shapes.Rectangle>();

            rectangle.RoundedProperties.TRRadius = 27f;
            rectangle.RoundedProperties.BRRadius = 27f;
            rectangle.RoundedProperties.BLRadius = 27f;
            rectangle.RoundedProperties.TLRadius = 27f;

            rectangle.ForceMeshUpdate();
            rectangle.SetAllDirty();

        }

        public void OnQuitButton()
        {

            showroomManager.Quit();

        }

    }

    [System.Serializable]
    public class AdditionalCameraPositionButtons
    {

        public Camera cameraPositionButtonCamera;

        public List<Function> cameraPositionButtonFunctions = new List<Function>();

    }

}