using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using ThisOtherThing.UI.Shapes;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using UnityEngine.Video;
using UnityEngine.Events;

namespace Showroom
{

    public class ShowroomManager : MonoBehaviour
    {

        #region Singleton

        public static ShowroomManager Instance;

        private void Awake()
        {
            if (Instance != null)
                Destroy(this.gameObject);
            else
                Instance = this;
        }

        #endregion


        [FoldoutGroup("General")] public ShowroomUI subLevelUI;

        [InfoBox("There is no Sub-Level Name set!", InfoMessageType.Error, "hasNoLevelName")]
        [FoldoutGroup("General")] public string subLevelName;

        [InfoBox("There is no Sub-Level Sub Title set!", InfoMessageType.Warning, "hasNoLevelSubTitle")]
        [FoldoutGroup("General")] public string subLevelSubTitle;

        [InfoBox("There is no Sub-Level ID set!", InfoMessageType.Error, "hasNoLevelID")]
        [FoldoutGroup("General")] public string subLevelID;


        [InfoBox("This Camera is used if there is no Use Case selected or there are no Use Cases.", InfoMessageType.Info, "moreThanOneUseCase")]
        [InfoBox("There is no Sub-Level Main Camera set!", InfoMessageType.Error, "hasNoSubLevelHomeCamera")]
        [FoldoutGroup("General")] public Camera SubLevelMainCamera;

        [FoldoutGroup("General")] public List<Function> onLevelLoaded = new List<Function>();


        [FoldoutGroup("General/Project Settings")] public bool isOnlyLevelInBuild = false;
        [FoldoutGroup("General/Project Settings")] public bool showDebugMessages = false;
        [FoldoutGroup("General/Project Settings")] public bool openSidebarMenu = true;


        [FoldoutGroup("General Menu Contents")] public bool subLevelHasGeneralMenu;
        [FoldoutGroup("General Menu Contents")] public Vector2 clickMeVFXRandomTime = new Vector2(10f, 45f);


        bool hasNoTransparentObjects()
        {
            if (transparentObjects.Count == 0)
                return true;
            else
                return false;
        }

        [FoldoutGroup("General Menu Contents")] [ShowIf("subLevelHasGeneralMenu")] public bool hasPlayButton;
        [FoldoutGroup("General Menu Contents")] [ShowIf("@this.subLevelHasGeneralMenu == true && this.hasPlayButton == true && this.playButtonIsRestartButton != true")] public bool playButtonIsPauseButton;
        [FoldoutGroup("General Menu Contents")] [ShowIf("@this.subLevelHasGeneralMenu == true && this.hasPlayButton == true && this.hasRestartButton != true && this.playButtonIsPauseButton != true")] public bool playButtonIsRestartButton;

        [FoldoutGroup("General Menu Contents")] [ShowIf("subLevelHasGeneralMenu")] public bool hasTransparencyButton;
        [FoldoutGroup("General Menu Contents")] [SerializeField] private Material transparencyMaterial;
        [FoldoutGroup("General Menu Contents")] [ShowIf("@this.subLevelHasGeneralMenu == true && hasTransparencyButton")] public List<MeshRenderer> transparentObjects = new List<MeshRenderer>();
        [FoldoutGroup("General Menu Contents")] [ShowIf("@this.subLevelHasGeneralMenu == true && hasTransparencyButton")] [SerializeField] public Animator xRayLinesObj;

        [FoldoutGroup("General Menu Contents")] [ShowIf("@this.subLevelHasGeneralMenu == true && playButtonIsRestartButton == false")] public bool hasRestartButton;

        [FoldoutGroup("General Menu Contents")] [ShowIf("subLevelHasGeneralMenu")] public bool hasResetCameraButton;

        [FoldoutGroup("General Menu Contents")] [ShowIf("subLevelHasGeneralMenu")] public bool hasCameraPosButton;

        [FoldoutGroup("General Menu Contents")] [ShowIf("subLevelHasGeneralMenu")] public bool hasDragModeButton;

        [FoldoutGroup("General Menu Contents")] [ShowIf("hasPlayButton")] public List<Function> playButtonFunctions = new List<Function>();
        [FoldoutGroup("General Menu Contents")] [ShowIf("@this.hasRestartButton || playButtonIsRestartButton")] public List<Function> resetButtonFunctions = new List<Function>();
        [FoldoutGroup("General Menu Contents")] [ShowIf("playButtonIsPauseButton")] public List<Function> pauseButtonFunctions = new List<Function>();
        [FoldoutGroup("General Menu Contents")] [ShowIf("hasTransparencyButton")] public List<Function> transparencyButtonFunctions = new List<Function>();
        [FoldoutGroup("General Menu Contents")] [ShowIf("hasResetCameraButton")] public List<Function> resetCameraButtonFunctions = new List<Function>();
        [FoldoutGroup("General Menu Contents")] [ShowIf("hasDragModeButton")] public List<Function> dragModeButtonFunctions = new List<Function>();
        [FoldoutGroup("General Menu Contents")] [ShowIf("hasResetCameraButton")] public List<Function> resetSubLevelCameraButtonFunctions = new List<Function>();

        [FoldoutGroup("General Menu Contents/Camera Position Button")] [ShowIf("hasCameraPosButton")] public List<AdditionalCameraPositionButtons> cameraButtons = new List<AdditionalCameraPositionButtons>();

        [HideInInspector] public int cameraPosIndex = -1;

        bool hasNoSubLevelHomeCamera()
        {
            return !SubLevelMainCamera;
        }

        bool hasNoLevelName()
        {
            if (string.IsNullOrEmpty(subLevelName))
                return true;
            else
                return false;
        }

        bool hasNoLevelSubTitle()
        {
            if (string.IsNullOrEmpty(subLevelSubTitle))
                return true;
            else
                return false;
        }

        bool hasNoLevelID()
        {
            if (string.IsNullOrEmpty(subLevelID))
                return true;
            else
                return false;
        }

        //[InfoBox("Every Sub Level Sidebar Button is one Use Case (These Sidebar Buttons are different to the ones used later on by the Use Cases)", InfoMessageType.Info)]
        //[BoxGroup("Sidebar Contents")] [Tooltip("Every Button represents One Use Case")] public bool subLevelHasSidebarButtons;
        //[BoxGroup("Sidebar Contents")] [ShowIf("subLevelHasSidebarButtons")] public List<UseCaseButtonEvent> subLevelButtons = new List<UseCaseButtonEvent>();

        [FoldoutGroup("Focus Menu Contents")] public bool hasRightSideMenu = false;

        [FoldoutGroup("Focus Menu Contents")] [ShowIf("hasRightSideMenu")] public Transform focusObjectPosition;
        [FoldoutGroup("Focus Menu Contents")] [ShowIf("hasRightSideMenu")] public List<Function> backButtonFunctions = new List<Function>();
        [FoldoutGroup("Focus Menu Contents")] [ShowIf("hasRightSideMenu")] public List<Function> resetRotationButtonFunctions = new List<Function>();


        [FoldoutGroup("Bullet Points")] [ReadOnly] public int activeBulletPointIndex = -1; //== -1 => No active BulletPoint
        [FoldoutGroup("Bullet Points")] public List<BulletPoint> bulletPoints = new List<BulletPoint>();


        [FoldoutGroup("Player Settings")] public bool playerIsAllowedToMove;
        [FoldoutGroup("Player Settings")] public bool playerIsAllowedToRotate;
        [FoldoutGroup("Player Settings")] public bool playerIsAllowedToTeleport;

        [FoldoutGroup("Player Settings")] [ShowIf("hasDragModeButton")] public Vector2 playerDragModeBoundaries = new Vector2(0, 0);

        [FoldoutGroup("Player Settings/Player Navigation Events")] public List<Function> onStartWalking = new List<Function>();
        [FoldoutGroup("Player Settings/Player Navigation Events")] public List<Function> onStartTeleporting = new List<Function>();
        [FoldoutGroup("Player Settings/Player Navigation Events")] public List<Function> onStartRotating = new List<Function>();
        [FoldoutGroup("Player Settings/Player Navigation Events")] public List<Function> onStopWalking = new List<Function>();
        [FoldoutGroup("Player Settings/Player Navigation Events")] public List<Function> onStopTeleporting = new List<Function>();
        [FoldoutGroup("Player Settings/Player Navigation Events")] public List<Function> onStopRotating = new List<Function>();
        [FoldoutGroup("Player Settings/Player Navigation Events")] public List<Function> onPlayerReset = new List<Function>();


        [FoldoutGroup("Timeline Stepper")] public bool hasTimelineStepper = false;

        [FoldoutGroup("Timeline Stepper")] [ReadOnly] public PlayableDirector currentTimeline;

        [FoldoutGroup("Timeline Stepper")] public bool timelineStepperAutoPlay = false;

        [FoldoutGroup("Timeline Stepper")] [ReadOnly] public int timelineStepperIndex = -1;
        [FoldoutGroup("Timeline Stepper")] [ReadOnly] public int timelineOldIndex = -1;
        [FoldoutGroup("Timeline Stepper")] [ReadOnly] public float currentTimelineTime = -1;
        [FoldoutGroup("Timeline Stepper")] [ShowIf("hasTimelineStepper")] public List<ShowroomTimelineStep> timelineSteps = new List<ShowroomTimelineStep>();


        [InfoBox("There are no Use Cases! Please add atleast one Use Case!", InfoMessageType.Error, "hasNoUseCases")]
        [FoldoutGroup("Use Cases")] [ShowIf("hasUseCases")] [ReadOnly] [SerializeField] public int useCaseIndex = -1;
        [FoldoutGroup("Use Cases")] [ShowIf("hasUseCases")] [SerializeField] public bool hasStandardUseCase;
        [FoldoutGroup("Use Cases")] [ShowIf("hasStandardUseCase")] [SerializeField] public int standardUseCaseIndex = 0;
        [FoldoutGroup("Use Cases")] public List<UseCase> useCases = new List<UseCase>();


        bool hasUseCases()
        {
            if (useCases.Count == 0)
                return false;
            else
                return true;
        }

        bool hasNoUseCases()
        {
            if (useCases.Count == 0)
                return true;
            else
                return false;
        }

        bool moreThanOneUseCase()
        {
            if (useCases.Count > 1)
                return true;
            else
                return false;
        }


        [FoldoutGroup("Fairtouch"), InfoBox("You're using Fairtouch Data and prefering it over the entered Data inside the Showroom Manager. This might not achieve the desired result!",
            InfoMessageType.Warning, "downloadFairtouchData")]

        [FoldoutGroup("Fairtouch")]
        public bool downloadFairtouchData = false;

        bool cantFindFairtouchData = false;
        [FoldoutGroup("Fairtouch"), InfoBox("Can't find a Sub-Level with that ID or Name! Please Check your entered Data!", InfoMessageType.Error, "cantFindFairtouchData")]

        [FoldoutGroup("Fairtouch")]//, Button]
        void GetFairtouchData()
        {

            ShowroomSSPDataHandler newHandler = new ShowroomSSPDataHandler();

            newHandler.GetFairtouchData();

        }


        [ReadOnly]
        public bool isTransparent;

        [ReadOnly]
        public bool isDragMode;

        private List<Material> materials = new List<Material>();

        [ReadOnly]
        public bool isAtUseCaseHomePos;

        [HideInInspector]
        public GameObject currentlyFocusedObj;

        private void Start()
        {

            subLevelUI = GameObject.Find("/--- User Interface ---").GetComponentInChildren<ShowroomUI>();

            StartCoroutine(MovePlayerOnStart());

            if (!hasStandardUseCase && subLevelHasGeneralMenu)
            {
                useCaseIndex = -1;
                subLevelUI.RemoveGeneralMenu();
            }

            if (!hasUseCases())
                useCaseIndex = -1;


            UnityEvent onLevelLoadedStandardEvents = new UnityEvent();

            onLevelLoadedStandardEvents.AddListener(() =>
            {

                SetPlayerProperties();

            });

            Function onLevelLoadedStandardEventsFunction = new Function
            {
                functionName = onLevelLoadedStandardEvents,
                functionDelay = 2f
            };


            for (int i = 0; i < onLevelLoaded.Count; i++)
            {
                StartCoroutine(Invoke(onLevelLoaded[i].functionName, onLevelLoaded[i].functionDelay));
            }

            SetPlayerProperties();

        }

        public void REMOVETHISLATERONDONTUSE()
        {

            isAtUseCaseHomePos = true;

        }

        //        void Start()
        //        {
        //
        //#if UNITY_EDITOR
        //            if(downloadFairtouchData && !isOnlyLevelInBuild)
        //                GetFairtouchData();
        //            else if(isOnlyLevelInBuild)
        //            {
        //
        //                ShowroomLoadingScreen loadingScreen = FindObjectOfType(typeof(ShowroomLoadingScreen)) as ShowroomLoadingScreen;
        //
        //                StartCoroutine(WaitForLoadingToFinish(loadingScreen));
        //            
        //            }   
        //#endif
        //
        //
        //
        //
        //            subLevelUI = GameObject.Find("/--- User Interface ---").GetComponentInChildren<ShowroomUI>();
        //
        //            StartCoroutine(MovePlayerOnStart());
        //
        //            if (!hasStandardUseCase && subLevelHasGeneralMenu)
        //            {
        //                useCaseIndex = -1;
        //                subLevelUI.RemoveGeneralMenu();
        //            }
        //
        //            if (!hasUseCases())
        //                useCaseIndex = -1;
        //
        //
        //            for (int i = 0; i < onLevelLoaded.Count; i++)
        //            {
        //                StartCoroutine(Invoke(onLevelLoaded[i].functionName, onLevelLoaded[i].functionDelay));
        //            }
        //
        //        }

        private void Update()
        {



        }

        IEnumerator WaitForLoadingToFinish(ShowroomLoadingScreen loadingScreen)
        {

            yield return new WaitForSecondsRealtime(5f);

            if (loadingScreen != null)
                loadingScreen.StartFadingLoadingScreen();

        }

        IEnumerator Invoke(UnityEvent function, float delay)
        {

            yield return new WaitForSecondsRealtime(delay);

            function?.Invoke();

        }

        IEnumerator MovePlayerOnStart()
        {

            if (!downloadFairtouchData || ShowroomSSPDataHandler.Instance == null)
                yield return new WaitForSecondsRealtime(1.5f);
            else if (downloadFairtouchData)
                yield return new WaitForSecondsRealtime(7f);

            if (hasStandardUseCase)
            {
                subLevelUI.spawnedHeadButtons[standardUseCaseIndex].OnMouseDown();
                subLevelUI.spawnedHeadButtons[standardUseCaseIndex].wasClicked = true;

                isAtUseCaseHomePos = true;

                SetPlayerProperties();
            }
            else if (!hasStandardUseCase && hasUseCases() || !hasUseCases())
                OnNewCamPos(SubLevelMainCamera.transform);

        }

        public void OnNewCamPos(Transform cameraPos)
        {

            ShowroomNavigation.Instance.ableToRotate = false;

            Vector3 pos = cameraPos.transform.position;

            float yRot = cameraPos.transform.eulerAngles.y;
            float xRot = cameraPos.transform.eulerAngles.x;

            Vector3 playerRot = new Vector3(0f, yRot, 0f);
            Vector3 cameraRot = new Vector3(-xRot, -yRot, 0f);

            ShowroomNavigation.Instance.TurnCameraTowards(new Vector2(xRot, yRot));

            ShowroomNavigation.Instance.transform.DOMove(pos, 1f).SetEase(Ease.InOutSine);
            ShowroomNavigation.Instance.transform.DORotate(playerRot, 1f).SetEase(Ease.InOutSine).SetOptions(true);
            ShowroomNavigation.Instance.playerCamera.transform.DORotate(-cameraRot, 1f).SetEase(Ease.InOutSine).OnComplete(() => FinishedMovingPlayer(false)).SetOptions(true);

            ShowroomNavigation.Instance.teleportPosition = pos;

            ShowroomNavigation.Instance.playerCamera.DOFieldOfView(cameraPos.GetComponent<Camera>().fieldOfView, 1f);

            //if (useCases[useCaseIndex].hasCameraPosButton)
            //{
            //
            //    subLevelUI.generalMenuCameraPosButton.GetComponent<Image>().sprite = subLevelUI.generalMenuCameraPosButtonSprites[0];
            //    subLevelUI.generalMenuCameraPosButton.GetComponentInChildren<TextMeshProUGUI>().gameObject.SetActive(false);
            //
            //}

            //subLevelUI.UpdateUI(true);

            //subLevelUI.generalMenuHomeCamButton.gameObject.SetActive(true);
            //subLevelUI.generalMenuSubLevelHomeCamButton.gameObject.SetActive(false);

            //subLevelUI.CloseBulletPointContainer();

            return;

        }

        void FinishedMovingPlayer(bool isAtHome)
        {

            isAtUseCaseHomePos = isAtHome;
            ShowroomNavigation.Instance.ableToRotate = true;
            ShowroomNavigation.Instance.ableToTeleport = true;

        }

        public void MoveToFixedPos(int i)
        {

            //-1 = Home Camera
            //x = any other Camera

            if (i == -1 && isAtUseCaseHomePos || i == -1 && !hasUseCases() || i == -1 && useCaseIndex == -1)
            {

                if (showDebugMessages)
                    Debug.Log("Returning to Sub-Level home camera");

                ShowroomNavigation.Instance.ableToRotate = false;

                Vector3 pos = SubLevelMainCamera.transform.position;

                float yRot = SubLevelMainCamera.transform.eulerAngles.y;
                float xRot = SubLevelMainCamera.transform.eulerAngles.x;

                Vector3 playerRot = new Vector3(0f, yRot, 0f);
                Vector3 cameraRot = new Vector3(-xRot, -yRot, 0f);

                ShowroomNavigation.Instance.TurnCameraTowards(new Vector2(xRot, yRot));

                ShowroomNavigation.Instance.transform.DOMove(pos, 1f).SetEase(Ease.InOutSine);
                ShowroomNavigation.Instance.transform.DORotate(playerRot, 1f).SetEase(Ease.InOutSine).SetOptions(true);
                ShowroomNavigation.Instance.playerCamera.transform.DORotate(-cameraRot, 1f).SetEase(Ease.InOutSine).OnComplete(() => FinishedMovingPlayer(true)).SetOptions(true);

                ShowroomNavigation.Instance.teleportPosition = pos;

                ShowroomNavigation.Instance.playerCamera.DOFieldOfView(SubLevelMainCamera.fieldOfView, 1f);

                if (hasCameraPosButton)
                {

                    subLevelUI.generalMenuCameraPosButton.GetComponent<Image>().sprite = subLevelUI.generalMenuCameraPosButtonSprites[0];
                    subLevelUI.generalMenuCameraPosButton.transform.GetChild(2).GetComponent<TextMeshProUGUI>().gameObject.SetActive(false);

                }

                if (useCaseIndex != -1)
                    SwitchUseCase(-1);

                subLevelUI.ResetAllSidebarHeadButtons(-1);

                subLevelUI.CloseBulletPointContainer();

                SetPlayerProperties();

                return;
            }
            if (i == -1)
            {

                if (showDebugMessages)
                    Debug.Log("Returning to Use-Case home camera");

                ShowroomNavigation.Instance.ableToRotate = false;

                Vector3 pos = useCases[useCaseIndex].useCaseHomeCamera.transform.position;

                float yRot = useCases[useCaseIndex].useCaseHomeCamera.transform.eulerAngles.y;
                float xRot = useCases[useCaseIndex].useCaseHomeCamera.transform.eulerAngles.x;

                Vector3 playerRot = new Vector3(0f, yRot, 0f);
                Vector3 cameraRot = new Vector3(-xRot, -yRot, 0f);

                ShowroomNavigation.Instance.TurnCameraTowards(new Vector2(xRot, yRot));

                ShowroomNavigation.Instance.transform.DOMove(pos, 1f).SetEase(Ease.InOutSine);
                ShowroomNavigation.Instance.transform.DORotate(playerRot, 1f).SetEase(Ease.InOutSine).SetOptions(true);
                ShowroomNavigation.Instance.playerCamera.transform.DORotate(-cameraRot, 1f).SetEase(Ease.InOutSine).OnComplete(() => FinishedMovingPlayer(true)).SetOptions(true);

                ShowroomNavigation.Instance.teleportPosition = pos;

                ShowroomNavigation.Instance.playerCamera.DOFieldOfView(useCases[useCaseIndex].useCaseHomeCamera.fieldOfView, 1f);

                if (useCases[useCaseIndex].hasCameraPosButton)
                {

                    subLevelUI.generalMenuCameraPosButton.GetComponent<Image>().sprite = subLevelUI.generalMenuCameraPosButtonSprites[0];
                    //subLevelUI.generalMenuCameraPosButton.GetComponentInChildren<TextMeshProUGUI>().gameObject.SetActive(false);

                }

                subLevelUI.CloseBulletPointContainer();

                return;
            }
            else
            {
                ShowroomNavigation.Instance.ableToRotate = false;

                Vector3 pos = useCases[useCaseIndex].useCaseCameras[i].transform.position;

                float yRot = useCases[useCaseIndex].useCaseCameras[i].transform.eulerAngles.y;
                float xRot = useCases[useCaseIndex].useCaseCameras[i].transform.eulerAngles.x;

                Vector3 playerRot = new Vector3(0f, yRot, 0f);
                Vector3 cameraRot = new Vector3(-xRot, -yRot, 0f);

                ShowroomNavigation.Instance.TurnCameraTowards(new Vector2(xRot, yRot));

                ShowroomNavigation.Instance.transform.DOMove(pos, 1f).SetEase(Ease.InOutSine);
                ShowroomNavigation.Instance.transform.DORotate(playerRot, 1f).SetEase(Ease.InOutSine).SetOptions(true);
                ShowroomNavigation.Instance.playerCamera.transform.DORotate(-cameraRot, 1f).SetEase(Ease.InOutSine).OnComplete(() => FinishedMovingPlayer(false)).SetOptions(true);

                ShowroomNavigation.Instance.teleportPosition = pos;

                ShowroomNavigation.Instance.playerCamera.DOFieldOfView(useCases[useCaseIndex].useCaseCameras[i].fieldOfView, 1f);

                if (useCases[useCaseIndex].hasCameraPosButton)
                {

                    subLevelUI.generalMenuCameraPosButton.GetComponent<Image>().sprite = subLevelUI.generalMenuCameraPosButtonSprites[0];
                    subLevelUI.generalMenuCameraPosButton.GetComponentInChildren<TextMeshProUGUI>().gameObject.SetActive(false);

                }

                //subLevelUI.CloseBulletPointContainer();

                return;
            }

        }

        public void SetPlayerProperties()
        {

            if (useCaseIndex == -1)
            {

                ShowroomNavigation.Instance.allowedToWalk = playerIsAllowedToMove;
                ShowroomNavigation.Instance.allowedToTeleport = playerIsAllowedToTeleport;
                ShowroomNavigation.Instance.allowedToRotate = playerIsAllowedToRotate;

            }
            else
            {

                ShowroomNavigation.Instance.allowedToWalk = useCases[useCaseIndex].playerIsAllowedToMove;
                ShowroomNavigation.Instance.allowedToTeleport = useCases[useCaseIndex].playerIsAllowedToTeleport;
                ShowroomNavigation.Instance.allowedToRotate = useCases[useCaseIndex].playerIsAllowedToRotate;

            }

        }

        public void SwitchUseCase(int index)
        {

            if (showDebugMessages)
                Debug.Log("Use Case Index: " + index);

            if (index == -1)
            {
                useCaseIndex = index;

                subLevelUI.RemoveGeneralMenu();
                SetPlayerProperties();

                return;
            }
            else
            {
                useCaseIndex = index;
                SetPlayerProperties();

                return;
            }


        }

        public void ToggleGameObjectStateFunction(GameObject go)
        {

            go.SetActive(!go.activeSelf);

        }

        public void ActivateGameObjectStateFunction(GameObject go)
        {

            go.SetActive(true);

        }

        public void DeactivateGameObjectStateFunction(GameObject go)
        {

            go.SetActive(false);

        }

        public void ToggleTransparency()
        {

            isTransparent = !isTransparent;

            if (materials.Count == 0)
            {

                for (int i = 0; i < transparentObjects.Count; i++)
                {

                    materials.Add(transparentObjects[i].materials[0]);

                }

            }
            for (int i = 0; i < useCases.Count; i++)
            {

                if (useCases[i].materials.Count == 0)
                {

                    for (int j = 0; j < useCases[i].transparentObjects.Count; j++)
                    {

                        useCases[i].materials.Add(useCases[i].transparentObjects[j].materials[0]);

                    }

                }

            }


            StartCoroutine(ChangeMaterial(isTransparent));

        }

        IEnumerator ChangeMaterial(bool isActive)
        {

            if (isActive)
            {
                if (xRayLinesObj != null)
                {

                    if (useCaseIndex != -1)
                    {

                        useCases[useCaseIndex].xRayLinesObj.gameObject.SetActive(true);
                        useCases[useCaseIndex].xRayLinesObj.CrossFadeInFixedTime("Activate Lines", 0.01f);

                    }
                    else
                    {

                        xRayLinesObj.gameObject.SetActive(true);
                        xRayLinesObj.CrossFadeInFixedTime("Activate Lines", 0.01f);

                    }

                }

            }

            yield return new WaitForSeconds(0.25f);

            if (isActive)
            {

                if (useCaseIndex != -1)
                {

                    for (int i = 0; i < useCases[useCaseIndex].transparentObjects.Count; i++)
                    {

                        useCases[useCaseIndex].transparentObjects[i].material = transparencyMaterial;

                    }

                }
                else
                {

                    for (int i = 0; i < transparentObjects.Count; i++)
                    {

                        transparentObjects[i].material = transparencyMaterial;

                    }

                }

            }
            else
            {

                if (xRayLinesObj != null)
                    xRayLinesObj.CrossFadeInFixedTime("Deactivate Lines", 0.01f);

                if (xRayLinesObj != null)
                    StartCoroutine(DeActivateLines());

                for (int i = 0; i < materials.Count; i++)
                {

                    transparentObjects[i].material = materials[i];

                }

                for (int i = 0; i < useCases.Count; i++)
                {

                    for (int j = 0; j < useCases[i].materials.Count; j++)
                    {

                        useCases[i].transparentObjects[j].material = materials[j];

                    }

                }




                //if (useCaseIndex != -1)
                //{
                //
                //    if (xRayLinesObj != null)
                //        useCases[useCaseIndex].xRayLinesObj.CrossFadeInFixedTime("Deactivate Lines", 0.01f);
                //
                //    for (int i = 0; i < useCases[useCaseIndex].transparentObjects.Count; i++)
                //    {
                //
                //        useCases[useCaseIndex].transparentObjects[i].material = materials[i];
                //
                //    }
                //
                //    if (xRayLinesObj != null)
                //        StartCoroutine(DeActivateLines());
                //
                //}
                //else
                //{
                //
                //    if (xRayLinesObj != null)
                //        xRayLinesObj.CrossFadeInFixedTime("Deactivate Lines", 0.01f);
                //
                //    for (int i = 0; i < transparentObjects.Count; i++)
                //    {
                //
                //        transparentObjects[i].material = materials[i];
                //
                //    }
                //
                //    if (xRayLinesObj != null)
                //        StartCoroutine(DeActivateLines());
                //
                //}
                //
                //for(int j = 0; j < useCases.Count; j++)
                //{
                //
                //    for (int i = 0; i < useCases[useCaseIndex].transparentObjects.Count; i++)
                //    {
                //
                //        useCases[useCaseIndex].transparentObjects[i].material = materials[i];
                //
                //    }
                //
                //}
                //
                //for (int i = 0; i < transparentObjects.Count; i++)
                //{
                //
                //    transparentObjects[i].material = materials[i];
                //
                //}

            }

            UpdateTransparencyButton();

        }

        IEnumerator DeActivateLines()
        {

            yield return new WaitForSeconds(.5f);

            xRayLinesObj.gameObject.SetActive(false);

        }

        public void EnableTransparency()
        {
            if (isTransparent)
                return;

            isTransparent = false;

            ToggleTransparency();

            UpdateTransparencyButton();

        }

        public void DisableTransparency()
        {
            if (!isTransparent)
                return;

            isTransparent = true;

            ToggleTransparency();

            UpdateTransparencyButton();

        }

        public void DisableTransparencyButton()
        {

            subLevelUI.generalMenuTransparencyButton.GetComponent<Button>().interactable = false;

            subLevelUI.generalMenuTransparencyButton.GetComponent<UnityEngine.EventSystems.EventTrigger>().enabled = false;

            UpdateTransparencyButton();

        }

        public void EnableTransparencyButton()
        {

            subLevelUI.generalMenuTransparencyButton.GetComponent<Button>().interactable = true;

            subLevelUI.generalMenuTransparencyButton.GetComponent<UnityEngine.EventSystems.EventTrigger>().enabled = true;

            UpdateTransparencyButton();

        }

        public void UpdateTransparencyButton()
        {

            if (isTransparent)
                subLevelUI.generalMenuTransparencyButton.GetComponent<Image>().sprite = subLevelUI.tranparencyButtons[0];
            else
                subLevelUI.generalMenuTransparencyButton.GetComponent<Image>().sprite = subLevelUI.tranparencyButtons[1];
        }

        public void ResetVideoPlayer(VideoPlayer videoPlayer)
        {

            videoPlayer.time = 0;

        }

        public void Quit()
        {

            if (isOnlyLevelInBuild)
            {

                if (showDebugMessages)
                    Debug.Log("Closing the Application!");

                Application.Quit();
            }
            else
                return;

        }

        public void FocusOntoObject(GameObject focusObj)
        {

            currentlyFocusedObj = focusObj;

            Showroom.ShowroomNavigation.Instance.ableToRotate = false;
            Showroom.ShowroomNavigation.Instance.ableToTeleport = false;
            Showroom.ShowroomNavigation.Instance.allowedToWalk = false;

            Showroom.ShowroomNavigation.Instance.focusVolumeObj.SetActive(true);
            Showroom.ShowroomNavigation.Instance.playerCamera.GetComponent<UnityEngine.EventSystems.PhysicsRaycaster>().enabled = true;

            subLevelUI.MoveCloseButtonOffScreen();
            subLevelUI.MoveSidebarOffScreen();
            subLevelUI.MoveGeneralMenuOffScreen();


            if (useCaseIndex == -1)
            {

                currentlyFocusedObj.transform.DOMove(focusObjectPosition.position, 1f)
                .OnComplete(() =>
                {

                    currentlyFocusedObj.GetComponent<Showroom.WorldSpace.ShowroomWorldSpaceRotatable>().enabled = true;
                    currentlyFocusedObj.GetComponent<BoxCollider>().enabled = true;

                });

            }
            else
            {

                currentlyFocusedObj.transform.DOMove(useCases[useCaseIndex].focusObjectPosition.position, 1f)
                .OnComplete(() =>
                {

                    currentlyFocusedObj.GetComponent<Showroom.WorldSpace.ShowroomWorldSpaceRotatable>().enabled = true;
                    currentlyFocusedObj.GetComponent<BoxCollider>().enabled = true;

                });

            }



        }

        public void UnfocusObject()
        {

            if (currentlyFocusedObj == null)
                return;

            currentlyFocusedObj.transform.DOLocalMove(Vector3.zero, 1f)
                .OnComplete(() =>
                {

                    currentlyFocusedObj.GetComponent<Showroom.WorldSpace.ShowroomWorldSpaceRotatable>().enabled = false;
                    currentlyFocusedObj.GetComponent<BoxCollider>().enabled = false;

                    currentlyFocusedObj = null;
                    Showroom.ShowroomNavigation.Instance.focusVolumeObj.SetActive(false);
                    Showroom.ShowroomNavigation.Instance.playerCamera.GetComponent<UnityEngine.EventSystems.PhysicsRaycaster>().enabled = false;

                    if (useCaseIndex == -1)
                    {

                        Showroom.ShowroomNavigation.Instance.ableToRotate = playerIsAllowedToRotate;
                        Showroom.ShowroomNavigation.Instance.ableToTeleport = playerIsAllowedToTeleport;
                        Showroom.ShowroomNavigation.Instance.allowedToWalk = playerIsAllowedToMove;

                    }
                    else
                    {

                        Showroom.ShowroomNavigation.Instance.ableToRotate = useCases[useCaseIndex].playerIsAllowedToRotate;
                        Showroom.ShowroomNavigation.Instance.ableToTeleport = useCases[useCaseIndex].playerIsAllowedToTeleport;
                        Showroom.ShowroomNavigation.Instance.allowedToWalk = useCases[useCaseIndex].playerIsAllowedToMove;

                    }

                });

            currentlyFocusedObj.transform.DOLocalRotate(Vector3.zero, 1f);

        }

        public void ResetFocusedObjRotation()
        {

            currentlyFocusedObj.transform.DOLocalRotate(Vector3.zero, .5f);

        }

        public void OpenTimelineStepper()
        {

            subLevelUI.MoveCloseButtonOffScreen();
            subLevelUI.MoveSidebarOffScreen();
            //subLevelUI.MoveGeneralMenuOffScreen();
            subLevelUI.OpenTimelineStepper();

        }

        public void CloseTimelineStepper()
        {

            subLevelUI.MoveCloseButtonOntoScreen();
            subLevelUI.MoveSidebarOntoScreen();
            //subLevelUI.MoveGeneralMenuOntoScreen();
            subLevelUI.CloseTimelineStepper();

            if (useCaseIndex == -1)
            {

                Showroom.ShowroomNavigation.Instance.ableToRotate = playerIsAllowedToRotate;
                Showroom.ShowroomNavigation.Instance.ableToTeleport = playerIsAllowedToTeleport;
                Showroom.ShowroomNavigation.Instance.allowedToWalk = playerIsAllowedToMove;

            }
            else
            {

                Showroom.ShowroomNavigation.Instance.ableToRotate = useCases[useCaseIndex].playerIsAllowedToRotate;
                Showroom.ShowroomNavigation.Instance.ableToTeleport = useCases[useCaseIndex].playerIsAllowedToTeleport;
                Showroom.ShowroomNavigation.Instance.allowedToWalk = useCases[useCaseIndex].playerIsAllowedToMove;

            }

        }

        public void PlayCurrentTimeline()
        {

            if (currentTimeline == null && timelineSteps.Count > 0 && timelineSteps[0].timeline != null)
            {

                subLevelUI.spawnedStepperButtons[0].OnMouseDown();

                return;

            }
            else if (currentTimeline == null && timelineSteps.Count == 0 || currentTimeline == null && timelineSteps[0].timeline == null)
                return;

            //currentTimeline.time = currentTimelineTime;

            currentTimeline.Play();// -> Resume is currently Broken, Unity-team doesn't know either

            //currentTimeline.playableGraph.GetRootPlayable(0).SetSpeed(1f);

            DOTween.Play("TimelineSlider");

        }

        public void PauseCurrentTimeline()
        {

            if (currentTimeline == null)
                return;

            currentTimeline.Pause();// -> Resume is currently Broken, Unity-team doesn't know either

            //currentTimeline.playableGraph.GetRootPlayable(0).SetSpeed(0f);

            //currentTimelineTime = (float)currentTimeline.time;

            DOTween.Pause("TimelineSlider");

        }

        public void RestartCurrentTimeline()
        {

            if (currentTimeline == null)
                return;

            currentTimeline.Stop();

            DOTween.Pause("TimelineSlider");

        }

        public void ToggleDragMode()
        {

            isDragMode = !isDragMode;

            ShowroomNavigation.Instance.dragModeActive = isDragMode;

        }

        public void DisableDragMode()
        {

            isDragMode = true;

            ToggleDragMode();
            UpdateDragModeButton();

        }

        public void UpdateDragModeButton()
        {


            if (isDragMode)
                subLevelUI.generalMenuDragModeButton.GetComponent<Image>().sprite = subLevelUI.dragModeButtons[0];
            else
                subLevelUI.generalMenuDragModeButton.GetComponent<Image>().sprite = subLevelUI.dragModeButtons[1];

        }

        public void PressUseCaseButtonWithoutNotice(int index)
        {

            subLevelUI.spawnedHeadButtons[index].OnMouseDown();

        }

        [System.Serializable]
        public class UseCase
        {
            [InfoBox("There is no Use Case Name set!", InfoMessageType.Error, "hasNoUseCaseName")]
            [FoldoutGroup("$useCaseName")] public string useCaseName;

            bool hasNoUseCaseName()
            {
                if (string.IsNullOrEmpty(useCaseName))
                    return true;
                else
                    return false;
            }

            [FoldoutGroup("$useCaseName/General Settings")]
            [InfoBox("There is no Home Camera Set!", InfoMessageType.Error, "hasNoHomeCamera")]
            [FoldoutGroup("$useCaseName/General Settings")] public Camera useCaseHomeCamera;

            bool hasNoHomeCamera()
            {
                return !useCaseHomeCamera;
            }

            [FoldoutGroup("$useCaseName/General Settings")] public List<Camera> useCaseCameras = new List<Camera>();

            [FoldoutGroup("$useCaseName/General Menu Contents")] public bool hasPlayButton;
            [FoldoutGroup("$useCaseName/General Menu Contents")] [ShowIf("@this.hasPlayButton == true && this.hasRestartButton != true && this.playButtonIsPauseButton != true")] public bool playButtonIsRestartButton;
            [FoldoutGroup("$useCaseName/General Menu Contents")] [ShowIf("@this.hasPlayButton == true && this.playButtonIsRestartButton != true")] public bool playButtonIsPauseButton;

            [FoldoutGroup("$useCaseName/General Menu Contents")] public bool hasTransparencyButton;
            [FoldoutGroup("$useCaseName/General Menu Contents")]
            [InfoBox("There are no Transparent Objects set!", InfoMessageType.Warning, "hasNoTransparentObjects && hasTransparencyButton")]
            [FoldoutGroup("$useCaseName/General Menu Contents")] [ShowIf("hasTransparencyButton")] public List<MeshRenderer> transparentObjects = new List<MeshRenderer>();
            [FoldoutGroup("$useCaseName/General Menu Contents")] [ShowIf("hasTransparencyButton")] [SerializeField] public Animator xRayLinesObj;

            bool hasNoTransparentObjects()
            {
                if (transparentObjects.Count == 0)
                    return true;
                else
                    return false;
            }

            [FoldoutGroup("$useCaseName/General Menu Contents")] [HideIf("playButtonIsRestartButton")] public bool hasRestartButton;

            [InfoBox("If a Use-Case has no Reset Camera Button, you won't be able to exit this Use-Case without using the Sidebar.", InfoMessageType.Warning, "hasNoResetCameraButton")]
            [FoldoutGroup("$useCaseName/General Menu Contents")] public bool hasResetCameraButton;

            [FoldoutGroup("$useCaseName/General Menu Contents")] public bool hasCameraPosButton;

            [FoldoutGroup("$useCaseName/General Menu Contents")] public bool hasDragModeButton;

            [FoldoutGroup("$useCaseName/General Menu Contents")] [ShowIf("hasPlayButton")] public List<Function> playButtonFunctions = new List<Function>();
            [FoldoutGroup("$useCaseName/General Menu Contents")] [ShowIf("@this.hasRestartButton || playButtonIsRestartButton")] public List<Function> resetButtonFunctions = new List<Function>();
            [FoldoutGroup("$useCaseName/General Menu Contents")] [ShowIf("playButtonIsPauseButton")] public List<Function> pauseButtonFunctions = new List<Function>();
            [FoldoutGroup("$useCaseName/General Menu Contents")] [ShowIf("hasTransparencyButton")] public List<Function> transparencyButtonFunctions = new List<Function>();
            [FoldoutGroup("$useCaseName/General Menu Contents")] [ShowIf("hasResetCameraButton")] public List<Function> resetCameraButtonFunctions = new List<Function>();
            [FoldoutGroup("$useCaseName/General Menu Contents")] [ShowIf("hasDragModeButton")] public List<Function> dragModeButtonFunctions = new List<Function>();
            [FoldoutGroup("$useCaseName/General Menu Contents")] [ShowIf("hasCameraPosButton")] public List<AdditionalCameraPositionButtons> cameraButtons = new List<AdditionalCameraPositionButtons>();


            [FoldoutGroup("$useCaseName/Focus Menu Contents")] public bool hasRightSideMenu = false;

            [FoldoutGroup("$useCaseName/Focus Menu Contents")] [ShowIf("hasRightSideMenu")] public Transform focusObjectPosition;
            [FoldoutGroup("$useCaseName/Focus Menu Contents")] [ShowIf("hasRightSideMenu")] public List<Function> backButtonFunctions = new List<Function>();
            [FoldoutGroup("$useCaseName/Focus Menu Contents")] [ShowIf("hasRightSideMenu")] public List<Function> resetRotationButtonFunctions = new List<Function>();

            bool hasNoResetCameraButton()
            {
                return !hasResetCameraButton;
            }

            [FoldoutGroup("$useCaseName/Bullet Points")] [ReadOnly] public int activeBulletPointIndex = -1; //== -1 => No active BulletPoint
            [FoldoutGroup("$useCaseName/Bullet Points")] public List<BulletPoint> bulletPoints = new List<BulletPoint>();


            [FoldoutGroup("$useCaseName/Player Settings")] public bool playerIsAllowedToMove;
            [FoldoutGroup("$useCaseName/Player Settings")] public bool playerIsAllowedToRotate;
            [FoldoutGroup("$useCaseName/Player Settings")] public bool playerIsAllowedToTeleport;

            [FoldoutGroup("$useCaseName/Player Settings")] [ShowIf("hasDragModeButton")] public Vector2 playerDragModeBoundaries = new Vector2(0, 0);

            [FoldoutGroup("$useCaseName/Player Settings/Player Navigation Events")] public List<Function> onStartWalking = new List<Function>();
            [FoldoutGroup("$useCaseName/Player Settings/Player Navigation Events")] public List<Function> onStartTeleporting = new List<Function>();
            [FoldoutGroup("$useCaseName/Player Settings/Player Navigation Events")] public List<Function> onStartRotating = new List<Function>();
            [FoldoutGroup("$useCaseName/Player Settings/Player Navigation Events")] public List<Function> onStopWalking = new List<Function>();
            [FoldoutGroup("$useCaseName/Player Settings/Player Navigation Events")] public List<Function> onStopTeleporting = new List<Function>();
            [FoldoutGroup("$useCaseName/Player Settings/Player Navigation Events")] public List<Function> onStopRotating = new List<Function>();
            [FoldoutGroup("$useCaseName/Player Settings/Player Navigation Events")] public List<Function> onPlayerReset = new List<Function>();


            [FoldoutGroup("$useCaseName/Timeline Stepper")] public bool hasTimelineStepper = false;
            [FoldoutGroup("$useCaseName/Timeline Stepper")] [ShowIf("hasTimelineStepper")] public bool timelineStepperAutoPlay = false;
            [FoldoutGroup("$useCaseName/Timeline Stepper")] [ShowIf("hasTimelineStepper")] public List<ShowroomTimelineStep> timelineSteps = new List<ShowroomTimelineStep>();

            [FoldoutGroup("$useCaseName/Sidebar Settings")] public UseCaseButtonEvent useCaseTopLevelButton;

            [FoldoutGroup("$useCaseName/Sidebar Settings")] public bool useCaseHasSidebarButtons;
            [FoldoutGroup("$useCaseName/Sidebar Settings")] [ShowIf("useCaseHasSidebarButtons")] public List<UseCaseButtonEvent> useCaseButtons = new List<UseCaseButtonEvent>();

            [HideInInspector]
            public List<Material> materials = new List<Material>();

        }
    }

    [System.Serializable]
    public class UseCaseButtonEvent
    {

        public string useCaseButtonName;
        public Sprite useCaseButtonSprite;
        public List<Function> useCaseButtonFunctions = new List<Function>();

    }

}