using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Showroom.WorldSpace
{

    public class ShowroomWorldSpaceBillboard : MonoBehaviour
    {

        public Camera camera;

        public bool invertFace;

        public Vector3 upDirection;

        public bool rotateOnAllAxis;

        private void Awake()
        {
            if (camera == null) camera = Camera.main;
        }
        void Update()
        {
            if (camera == null) return;
            if (rotateOnAllAxis)
            {
                transform.LookAt(transform.position + camera.transform.rotation * Vector3.forward,
                    camera.transform.rotation * Vector3.up);
            }
            else
            {

                transform.rotation = Quaternion.LookRotation(Vector3.Cross(upDirection, Vector3.Cross(camera.transform.forward * (invertFace ? +1 : -1), upDirection)), upDirection);

            }
        }

    }

}