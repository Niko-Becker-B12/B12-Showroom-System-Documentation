using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;
using UnityEngine.Events;

namespace Showroom.WorldSpace
{

    public class ShowroomWorldSpaceRotatable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {

        public bool _dragged;

        public Camera camera;

        public List<Function> onBeginDragEvents = new List<Function>();
        public List<Function> onEndDragEvents = new List<Function>();

        public Transform Target;
        public float ArcballRadius = 0.2f; //The radius of the virtual Trackball   

        private BoxCollider _boxCollider;
        private float radius;
        private Vector2 center;

        private Vector3 v_down;
        private Vector3 v_drag;

        private Quaternion q_down;
        private Quaternion q_now;
        private Quaternion q_drag;

        Vector3 currentRot;

        public bool onlyYAxis = false;


        void Start()
        {

            UnityEvent onRotatedClickEvent = new UnityEvent();

            onRotatedClickEvent.AddListener(() =>
            {

                ShowroomManager.Instance.subLevelUI.rightMenuResetRotationButton.gameObject.SetActive(false);
                ShowroomManager.Instance.subLevelUI.rightMenuRotatedButton.gameObject.SetActive(true);

            });

            Function onRotatedClickEventFunction = new Function
            {
                functionName = onRotatedClickEvent,
                functionDelay = 0f
            };

            onBeginDragEvents.Add(onRotatedClickEventFunction);

            camera = Camera.main;

            if (Target == null)
                Target = transform;

            Arcball();

            _boxCollider = this.GetComponent<BoxCollider>();
            center = new Vector2();

        }

        public void OnBeginDrag(PointerEventData eventData)
        {

            _dragged = true;

            for (int i = 0; i < onBeginDragEvents.Count; i++)
                StartCoroutine(Invoke(onBeginDragEvents[i].functionName, onBeginDragEvents[i].functionDelay));

            q_now = Target.rotation;

            v_down = mouse_to_sphere(eventData.position);

            q_down.w = q_now.w;
            q_down.x = q_now.x;
            q_down.y = q_now.y;
            q_down.z = q_now.z;
            q_drag = Quaternion.identity;
            q_drag.w = 1;

        }

        public void OnDrag(PointerEventData eventData)
        {

            v_drag = mouse_to_sphere(eventData.position);

            float f_dot = Vector3.Dot(v_down, v_drag);
            Vector3 v_cross = Vector3.Cross(v_down, v_drag);

            //Here we add the Camera Quaternion to get proper results from all camera positions
            v_cross = camera.transform.rotation * v_cross;//getActiveCamera()


            //don't know if this distinction is required but i got it from the java implementations 
            if (v_cross.magnitude > Mathf.Epsilon)
            {
                q_drag.w = f_dot;
                q_drag.x = v_cross.x;
                q_drag.y = v_cross.y;
                q_drag.z = v_cross.z;

            }
            else
            {
                //Debug.Log("< Epsilon");
                q_drag.x = q_drag.y = q_drag.z = 0.0f;
                q_drag.w = 1.0f;
            }

        }

        public void OnEndDrag(PointerEventData eventData)
        {

            _dragged = false;

            for (int i = 0; i < onEndDragEvents.Count; i++)
                StartCoroutine(Invoke(onEndDragEvents[i].functionName, onEndDragEvents[i].functionDelay));

            v_drag = mouse_to_sphere(eventData.position);

            float f_dot = Vector3.Dot(v_down, v_drag);
            Vector3 v_cross = Vector3.Cross(v_down, v_drag);

            //Here we add the Camera Quaternion to get proper results from all camera positions
            v_cross = camera.transform.rotation * v_cross;//getActiveCamera()


            //don't know if this distinction is required but i got it from the java implementations 
            if (v_cross.magnitude > Mathf.Epsilon)
            {
                q_drag.w = f_dot;
                q_drag.x = v_cross.x;
                q_drag.y = v_cross.y;
                q_drag.z = v_cross.z;

            }
            else
            {
                //Debug.Log("< Epsilon");
                q_drag.x = q_drag.y = q_drag.z = 0.0f;
                q_drag.w = 1.0f;
            }

        }

        private void Update()
        {
            if (_dragged)
            {

                q_now = q_drag * q_down;

                if(!onlyYAxis)
                    Target.rotation = q_now;
                else
                {
                    currentRot = q_now.eulerAngles;

                    currentRot = new Vector3(0f, currentRot.y, 0f);

                    Target.eulerAngles = currentRot;
                }

            }

        }

        public void ResetRotation()
        {

            this.transform.DORotate(Vector3.zero, 1f);

        }

        IEnumerator Invoke(UnityEvent function, float delay)
        {

            yield return new WaitForSecondsRealtime(delay);

            function?.Invoke();

        }

        public Vector3 mouse_to_sphere(Vector3 mouseVec)
        {
            Vector3 v = new Vector3();

            center = camera.WorldToScreenPoint(Target.position);
            float dist = (camera.transform.position - Target.position).magnitude;
            //Nasty hack, not Mathematical correct
            radius = ArcballRadius * 100000f * (Mathf.Max(1 / dist, Mathf.Epsilon)) / camera.fieldOfView;

            v.x = (center.x - mouseVec.x) / radius;
            v.y = (center.y - mouseVec.y) / radius;

            float mag = v.x * v.x + v.y * v.y;

            if (mag > 1.0f)
            {
                v.Normalize();
            }
            else
            {
                v.z = Mathf.Sqrt(1.0f - mag);
            }

            //return (axis == -1) ? v : constrain_vector(v, axisSet[axis]);
            return v;
        }
        public void Arcball()
        {

            v_down = new Vector3();
            v_drag = new Vector3();

            q_now = new Quaternion();
            q_down = new Quaternion();
            q_drag = new Quaternion();
            q_drag.w = 1;
            q_down.w = 1;
            q_now.w = 1;

        }

    }

}