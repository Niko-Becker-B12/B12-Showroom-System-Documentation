using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Sirenix.OdinInspector;
using UnityEngine.InputSystem;
using System;
using UnityEngine.Events;

namespace Showroom
{

    public class ShowroomNavigation : MonoBehaviour
    {

        #region Singleton


        public static ShowroomNavigation Instance;

        private void Awake()
        {

            if (Instance == null)
                Instance = this;
            else
                Destroy(this.gameObject);

        }


        #endregion

        [FoldoutGroup("Basic Character Settings")] public Camera playerCamera;
        [FoldoutGroup("Basic Character Settings")] public CharacterController playerController;
        [FoldoutGroup("Basic Character Settings")] public GameObject focusVolumeObj;
        [FoldoutGroup("Basic Character Settings/Gizmos")] public GameObject teleportGizmo;
        [FoldoutGroup("Basic Character Settings/Gizmos")] public GameObject deadZoneGizmo;

        [FoldoutGroup("Public Parameters/Movement")] [ReadOnly] public Vector3 teleportPosition;
        [FoldoutGroup("Public Parameters/Movement")] [ReadOnly] public Vector3 movePos;
        [FoldoutGroup("Public Parameters/Movement")] [ReadOnly] public Vector3 inputMovementVector;

        [FoldoutGroup("Public Parameters/Movement")] public float moveHeight = 1.7f;


        [FoldoutGroup("Public Parameters/Dragging")] [ReadOnly] public Vector3 inputDragStartPosition;
        [FoldoutGroup("Public Parameters/Dragging")] [ReadOnly] public Vector3 inputDragCurrentPointerPosition;
        [FoldoutGroup("Public Parameters/Dragging")] [ReadOnly] public Vector3 inputNewDragPosition;

        [FoldoutGroup("Public Parameters")] [ReadOnly] public bool allowedToTeleport = true;
        [FoldoutGroup("Public Parameters")] [ReadOnly] public bool allowedToWalk = true;
        [FoldoutGroup("Public Parameters")] [ReadOnly] public bool allowedToRotate = true;
        [FoldoutGroup("Public Parameters")] [ReadOnly] public bool dragModeActive = false;

        [FoldoutGroup("Public Parameters")] [ReadOnly] public bool ableToTeleport = true;
        [FoldoutGroup("Public Parameters")] [ReadOnly] public bool isWalking = false;
        [FoldoutGroup("Public Parameters")] [ReadOnly] public bool isTeleporting = false;
        [FoldoutGroup("Public Parameters")] [ReadOnly] public bool ableToRotate = true;

        [FoldoutGroup("Public Parameters/Camera Settings")] public Vector2 inputLookRotationVector = Vector2.zero;
        [FoldoutGroup("Public Parameters/Camera Settings")] public Vector2 lookRotation = Vector2.zero;
        [FoldoutGroup("Public Parameters/Camera Settings")] public float zoomFactor;

        [FoldoutGroup("Public Parameters/Camera Settings")] public Vector2 yAxisClamp = new Vector2(15f, 15f);
        [FoldoutGroup("Public Parameters/Camera Settings")] public float sensitivity = 2f;
        [FoldoutGroup("Public Parameters/Camera Settings")] bool cursorLocked = false;


        [FoldoutGroup("Input Settings")] [SerializeField] private InputActionReference movementInputRef;
        [FoldoutGroup("Input Settings")] [SerializeField] private InputActionReference mouseDragInputRef;
        [FoldoutGroup("Input Settings")] [SerializeField] private InputActionReference zoomInputRef;
        [FoldoutGroup("Input Settings")] [SerializeField] private InputActionReference teleportInputRef;


        [FoldoutGroup("Player Navigation Events")] public List<Function> onStartWalking = new List<Function>();
        [FoldoutGroup("Player Navigation Events")] public List<Function> onStartTeleporting = new List<Function>();
        [FoldoutGroup("Player Navigation Events")] public List<Function> onStartRotating = new List<Function>();
        [FoldoutGroup("Player Navigation Events")] public List<Function> onStopWalking = new List<Function>();
        [FoldoutGroup("Player Navigation Events")] public List<Function> onStopTeleporting = new List<Function>();
        [FoldoutGroup("Player Navigation Events")] public List<Function> onStopRotating = new List<Function>();
        [FoldoutGroup("Player Navigation Events")] public List<Function> onPlayerReset = new List<Function>();

        float tempHorizontalFOV;
        float tempZoomValue;


        private void Start()
        {
            if(playerCamera == null)
                playerCamera = transform.GetChild(0).GetComponent<Camera>();

            if (teleportGizmo == null)
                teleportGizmo = GameObject.Find("/--- Gizmos ---/Showroom_Navigation_Teleport_Gizmo");

            if (deadZoneGizmo == null)
                deadZoneGizmo = GameObject.Find("/--- Gizmos ---/Showroom_Navigation_DeadZone_Gizmo");


            float yRot = this.transform.localRotation.eulerAngles.y;
            float xRot = playerCamera.transform.localRotation.eulerAngles.x;

            TurnCameraTowards(new Vector2(xRot, yRot));


            teleportPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);

            float vFOVrad = playerCamera.fieldOfView * Mathf.Deg2Rad;
            float cameraHeightAt1 = Mathf.Tan(vFOVrad * .5f);
            tempHorizontalFOV = Mathf.Atan(cameraHeightAt1 * playerCamera.aspect) * 2f * Mathf.Rad2Deg;

            GetInput();

            GenerateDefaultEvents();

        }

        void GetInput()
        {

            movementInputRef.action.performed += MovementControl;
            movementInputRef.action.canceled += MovementControl;

            mouseDragInputRef.action.started += DragControl;
            mouseDragInputRef.action.performed += DragControl;
            mouseDragInputRef.action.canceled += DragControl;

            tempZoomValue = zoomInputRef.action.ReadValue<float>();

            teleportInputRef.action.performed += TeleportControl;
            //teleportInputRef.action.canceled += TeleportControl;

            //zoomInputRef.action.performed += ZoomControl;

        }

        public void GenerateDefaultEvents()
        {

            #region OnStartWalkingEvent

            UnityEvent onStartWalkingEvent = new UnityEvent();

            onStartWalkingEvent.AddListener(() =>
            {

                ShowroomManager.Instance.subLevelUI.generalMenuSubLevelHomeCamButton.gameObject.SetActive(false);
                ShowroomManager.Instance.subLevelUI.generalMenuHomeCamButton.gameObject.SetActive(true);

            });

            Function onStartWalkingEventFunction = new Function
            {
                functionName = onStartWalkingEvent,
                functionDelay = 0f
            };

            onStartWalking.Add(onStartWalkingEventFunction);

            #endregion

            #region OnStartRotatingEvent

            UnityEvent onStartRotatingEvent = new UnityEvent();

            onStartRotatingEvent.AddListener(() =>
            {

                ShowroomManager.Instance.subLevelUI.generalMenuSubLevelHomeCamButton.gameObject.SetActive(false);
                ShowroomManager.Instance.subLevelUI.generalMenuHomeCamButton.gameObject.SetActive(true);

            });

            Function onStartRotatingEventFunction = new Function
            {
                functionName = onStartRotatingEvent,
                functionDelay = 0f
            };

            onStartWalking.Add(onStartRotatingEventFunction);

            #endregion

            #region OnStartTeleportEvent

            UnityEvent onStartTeleportingEvent = new UnityEvent();

            onStartTeleportingEvent.AddListener(() =>
            {

                ShowroomManager.Instance.subLevelUI.generalMenuSubLevelHomeCamButton.gameObject.SetActive(false);
                ShowroomManager.Instance.subLevelUI.generalMenuHomeCamButton.gameObject.SetActive(true);

            });

            Function onStartTeleportingEventFunction = new Function
            {
                functionName = onStartTeleportingEvent,
                functionDelay = 0f
            };

            onStartWalking.Add(onStartTeleportingEventFunction);

            #endregion

        }

        private void Update()
        {
            if (allowedToRotate && ableToRotate && !dragModeActive)
                RotateCamera();

            if (!isTeleporting && allowedToWalk)
                MovePlayer();

            if (!isWalking && allowedToTeleport && ableToTeleport && isTeleporting)
                Teleport(teleportPosition);

            if (!isWalking && !isTeleporting && dragModeActive)
                DragMovement();


            tempZoomValue = zoomInputRef.action.ReadValue<float>();

            ZoomControl();

        }

        private void LateUpdate()
        {

            if (allowedToTeleport && ableToTeleport)
            {

                if (EventSystem.current.IsPointerOverGameObject())
                {
                    teleportGizmo?.SetActive(false);
                    deadZoneGizmo?.SetActive(false);
                    return;
                }

                RaycastHit hit;
                Ray ray = playerCamera.ScreenPointToRay(Mouse.current.position.ReadValue());

                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider && hit.transform.gameObject.tag == "GroundGrid")
                    {
                        teleportGizmo?.SetActive(true);
                        deadZoneGizmo?.SetActive(false);

                        if (teleportGizmo != null)
                            teleportGizmo.transform.position = hit.point;
                    }
                    else
                    {
                        teleportGizmo?.SetActive(false);


                        if (hit.collider && hit.transform.gameObject.tag == "DeadZone")
                        {
                            if (deadZoneGizmo != null)
                            {

                                deadZoneGizmo?.SetActive(true);
                                deadZoneGizmo?.transform.GetChild(2).gameObject.SetActive(true);

                                deadZoneGizmo.transform.position = hit.point;
                                deadZoneGizmo.transform.forward = hit.normal;
                            }
                        }
                        else
                            deadZoneGizmo?.SetActive(false);
                    }
                }
                else
                {
                    teleportGizmo?.SetActive(false);
                    deadZoneGizmo?.SetActive(false);
                }

            }

        }

        void MovePlayer()
        {

            Vector3 movementDirection = Quaternion.Euler(0, playerCamera.transform.eulerAngles.y, 0) * new Vector3(inputMovementVector.x * 2f * Time.deltaTime, 0, inputMovementVector.z * 2f * Time.deltaTime);

            if (inputMovementVector.magnitude > .1f)
            {

                isWalking = true;

                ShowroomManager.Instance.isAtUseCaseHomePos = false;
                ShowroomManager.Instance.subLevelUI.SwitchActiveCameraButton(-1);

                teleportGizmo.SetActive(false);
                deadZoneGizmo.SetActive(false);

                for (int i = 0; i < onStartWalking.Count; i++)
                {
                    StartCoroutine(Invoke(onStartWalking[i].functionName, onStartWalking[i].functionDelay));
                }

                if(ShowroomManager.Instance.showDebugMessages)
                    Debug.Log("Input Movement Vector: " + movementDirection);

            }
            else
                isWalking = false;

            playerController.Move(movementDirection);

        }

        void Teleport(Vector3 pos)
        {
            isTeleporting = true;

            ShowroomManager.Instance.isAtUseCaseHomePos = false;

            Vector3 offset = pos - transform.position;

            offset = offset.normalized * 1.1f * (Vector3.Distance(pos, transform.position));

            playerController.Move(offset * Time.deltaTime);

            if ((transform.position - teleportPosition).magnitude <= .5f)
                isTeleporting = false;

            ableToRotate = true;
        }

        void RotateCamera()
        {

            if (EventSystem.current.IsPointerOverGameObject())
                return;

            if (inputLookRotationVector.x > 0.01f || inputLookRotationVector.y > 0.01f || inputLookRotationVector.x < -0.01f || inputLookRotationVector.y < -0.01f)
            {

                ShowroomManager.Instance.subLevelUI.SwitchActiveCameraButton(-1);
                ShowroomManager.Instance.isAtUseCaseHomePos = false;

            }

            float vFOVrad = playerCamera.fieldOfView * Mathf.Deg2Rad;
            float cameraHeightAt1 = Mathf.Tan(vFOVrad * .5f);
            tempHorizontalFOV = Mathf.Atan(cameraHeightAt1 * playerCamera.aspect) * 2f * Mathf.Rad2Deg;


            float temp = tempHorizontalFOV - 91f;

            if (temp < -2f)
            {
                lookRotation.y += inputLookRotationVector.x * ((sensitivity / Mathf.Abs(temp)));
                lookRotation.x += -inputLookRotationVector.y * ((sensitivity / Mathf.Abs(temp)));
                lookRotation.x = Mathf.Clamp(lookRotation.x, -yAxisClamp.x, yAxisClamp.y);
                transform.eulerAngles = new Vector2(0, lookRotation.y);
                playerCamera.transform.localRotation = Quaternion.Euler(lookRotation.x, 0, 0);
            }
            else
            {
                lookRotation.y += inputLookRotationVector.x * ((sensitivity));
                lookRotation.x += -inputLookRotationVector.y * ((sensitivity));
                lookRotation.x = Mathf.Clamp(lookRotation.x, -yAxisClamp.x, yAxisClamp.y);
                transform.eulerAngles = new Vector2(0, lookRotation.y);
                playerCamera.transform.localRotation = Quaternion.Euler(lookRotation.x, 0, 0);
            }

        }

        public void DragMovement()
        {

            if (inputLookRotationVector.x > 0.01f || inputLookRotationVector.y > 0.01f || inputLookRotationVector.x < -0.01f || inputLookRotationVector.y < -0.01f)
            {

                ShowroomManager.Instance.subLevelUI.SwitchActiveCameraButton(-1);
                ShowroomManager.Instance.isAtUseCaseHomePos = false;

            }

            Vector2 boundaries = Vector2.zero;

            Transform dragStartPosTransform = null;

            if(ShowroomManager.Instance.useCaseIndex == -1)
            {

                boundaries = ShowroomManager.Instance.playerDragModeBoundaries;

                if (ShowroomManager.Instance.cameraPosIndex == -1)
                    dragStartPosTransform = ShowroomManager.Instance.SubLevelMainCamera.transform;
                else
                    dragStartPosTransform = ShowroomManager.Instance.cameraButtons[ShowroomManager.Instance.cameraPosIndex].cameraPositionButtonCamera.transform;

            }
            else
            {

                boundaries = ShowroomManager.Instance.useCases[ShowroomManager.Instance.useCaseIndex].playerDragModeBoundaries;

                if (ShowroomManager.Instance.cameraPosIndex == -1)
                    dragStartPosTransform = ShowroomManager.Instance.useCases[ShowroomManager.Instance.useCaseIndex].useCaseHomeCamera.transform;
                else
                    dragStartPosTransform = ShowroomManager.Instance.useCases[ShowroomManager.Instance.useCaseIndex].cameraButtons[ShowroomManager.Instance.cameraPosIndex].cameraPositionButtonCamera.transform;

            }

            float distance;

            teleportGizmo.SetActive(false);
            deadZoneGizmo.SetActive(false);


            float vFOVrad = playerCamera.fieldOfView * Mathf.Deg2Rad;
            float cameraHeightAt1 = Mathf.Tan(vFOVrad * .5f);
            tempHorizontalFOV = Mathf.Atan(cameraHeightAt1 * playerCamera.aspect) * 2f * Mathf.Rad2Deg;


            float temp = tempHorizontalFOV - 91f;

            Vector3 movementDirection = Vector3.zero;

            if (temp < -2f)
            {

                movementDirection = Quaternion.Euler(playerCamera.transform.eulerAngles.x, playerCamera.transform.eulerAngles.y, 0) * 
                    new Vector3(inputLookRotationVector.x * (sensitivity / 2 / Mathf.Abs(temp)) * Time.deltaTime, inputLookRotationVector.y * (sensitivity / 2 / Mathf.Abs(temp)) * Time.deltaTime, 0f);

            }
            else
            {

                movementDirection = Quaternion.Euler(playerCamera.transform.eulerAngles.x, playerCamera.transform.eulerAngles.y, 0) *
                    new Vector3(inputLookRotationVector.x * sensitivity / 2 * Time.deltaTime, inputLookRotationVector.y * sensitivity / 2 * Time.deltaTime, 0f);

            }

            if (boundaries.x <= -1f && boundaries.y <= -1f || boundaries.x == 0f && boundaries.y == 0f)
            {

            }
            else
            {

                Vector3 startPos = dragStartPosTransform.position;

                startPos.x = Mathf.Clamp(startPos.x + movementDirection.x, -boundaries.x, boundaries.x);

                startPos.y = Mathf.Clamp(startPos.y + movementDirection.y, -boundaries.y, boundaries.y);

            }

            playerController.Move(movementDirection);

        }

        public void TurnCameraTowards(Vector2 rot)
        {

            if (rot.x >= 270)
            {
                rot.x -= 360;
            }

            if (rot.x > 90)
            {
                rot.x = 90;
            }
            else if (rot.x < -90)
            {
                rot.x = -90;
            }

            lookRotation = new Vector2(rot.x, rot.y);
        }

        #region Controls


        public void DragControl(InputAction.CallbackContext context)
        {

            if (context.started)
            {

                if (EventSystem.current.IsPointerOverGameObject())
                    return;

                if (ShowroomManager.Instance.showDebugMessages)
                    Debug.Log("DragControl Pressed Down Event - called once when button pressed");

                inputLookRotationVector = mouseDragInputRef.action.ReadValue<Vector2>();

            }
            else if (context.performed)
            {

                if (EventSystem.current.IsPointerOverGameObject())
                    return;

                if (ShowroomManager.Instance.showDebugMessages)
                    Debug.Log("DragControl Hold Down - called continously till the button is released");

                inputLookRotationVector = mouseDragInputRef.action.ReadValue<Vector2>();

            }
            else if (context.canceled)
            {

                if (ShowroomManager.Instance.showDebugMessages)
                    Debug.Log("DragControl released");

                inputLookRotationVector = mouseDragInputRef.action.ReadValue<Vector2>();

            }

        }

        public void TeleportControl(InputAction.CallbackContext context)
        {

            if (EventSystem.current.IsPointerOverGameObject())
                return;

            if (!isWalking && ableToTeleport && allowedToTeleport)
            {

                RaycastHit hit;
                Ray ray = playerCamera.ScreenPointToRay(Mouse.current.position.ReadValue());

                if (Physics.Raycast(ray, out hit) && ableToTeleport)
                {

                    Debug.DrawLine(transform.position, hit.point, Color.green);

                    if (hit.collider && hit.transform.gameObject.tag == "GroundGrid" && ableToTeleport)
                    {
                        teleportGizmo.SetActive(false);

                        Vector3 hitPosition = new Vector3(hit.point.x, moveHeight, hit.point.z);

                        teleportPosition = hitPosition;

                        ableToRotate = false;
                        isTeleporting = true;

                        ShowroomManager.Instance.isAtUseCaseHomePos = false;
                        ShowroomManager.Instance.subLevelUI.SwitchActiveCameraButton(-1);


                        for (int i = 0; i < onStartTeleporting.Count; i++)
                        {
                            StartCoroutine(Invoke(onStartTeleporting[i].functionName, onStartTeleporting[i].functionDelay));
                        }

                    }
                }

            }

        }

        public void MovementControl(InputAction.CallbackContext context)
        {

            inputMovementVector = new Vector3(movementInputRef.action.ReadValue<Vector2>().x, 0f, movementInputRef.action.ReadValue<Vector2>().y);

        }

        public void ZoomControl()//(InputAction.CallbackContext context)
        {

            tempZoomValue = zoomInputRef.action.ReadValue<float>();

            zoomFactor = tempZoomValue;// * 50f;
            playerCamera.fieldOfView -= zoomFactor;
            playerCamera.fieldOfView = Mathf.Clamp(playerCamera.fieldOfView, 2f, 60f);


            if (tempZoomValue > 0.1f)
            {

                float vFOVrad = playerCamera.fieldOfView * Mathf.Deg2Rad;
                float cameraHeightAt1 = Mathf.Tan(vFOVrad * .5f);
                tempHorizontalFOV = Mathf.Atan(cameraHeightAt1 * playerCamera.aspect) * 2f * Mathf.Rad2Deg;

            }
            else if (tempZoomValue < -0.1f)
            {

                float vFOVrad = playerCamera.fieldOfView * Mathf.Deg2Rad;
                float cameraHeightAt1 = Mathf.Tan(vFOVrad * .5f);
                tempHorizontalFOV = Mathf.Atan(cameraHeightAt1 * playerCamera.aspect) * 2f * Mathf.Rad2Deg;

            }

            //if (context.performed)
            //{
            //
            //
            //
            //
            //    zoomFactor = tempZoomValue;// * 50f;
            //    playerCamera.fieldOfView -= zoomFactor;
            //    playerCamera.fieldOfView = Mathf.Clamp(playerCamera.fieldOfView, 2f, 60f);
            //
            //
            //    if (tempZoomValue > 0.1f)
            //    {
            //
            //        float vFOVrad = playerCamera.fieldOfView * Mathf.Deg2Rad;
            //        float cameraHeightAt1 = Mathf.Tan(vFOVrad * .5f);
            //        tempHorizontalFOV = Mathf.Atan(cameraHeightAt1 * playerCamera.aspect) * 2f * Mathf.Rad2Deg;
            //
            //    }
            //    else if (tempZoomValue < -0.1f)
            //    {
            //
            //        float vFOVrad = playerCamera.fieldOfView * Mathf.Deg2Rad;
            //        float cameraHeightAt1 = Mathf.Tan(vFOVrad * .5f);
            //        tempHorizontalFOV = Mathf.Atan(cameraHeightAt1 * playerCamera.aspect) * 2f * Mathf.Rad2Deg;
            //
            //    }
            //
            //}

        }

        #endregion


        IEnumerator Invoke(UnityEvent function, float delay)
        {

            yield return new WaitForSecondsRealtime(delay);

            function?.Invoke();

        }

    }

}