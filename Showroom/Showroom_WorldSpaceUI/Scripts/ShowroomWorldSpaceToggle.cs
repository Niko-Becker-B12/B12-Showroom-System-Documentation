using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ThisOtherThing.UI.Shapes;
using DG.Tweening;

namespace Showroom.WorldSpace
{

    public class ShowroomWorldSpaceToggle : MonoBehaviour
    {

        public Ellipse toggleIconEllipse;
        public RectTransform toggleIconRect;
        public Toggle toggleObj;

        private void OnEnable()
        {

            toggleIconEllipse = this.GetComponentInChildren<Ellipse>();
            toggleIconRect = toggleIconEllipse.rectTransform;
            toggleObj = this.GetComponent<Toggle>();

        }

        public void Toggle()
        {

            //toggleObj.isOn = !toggleObj.isOn;

            if (toggleObj.isOn)
            {

                toggleIconRect.DOAnchorPos(new Vector2(23f, 0.9f), .5f);

                toggleIconEllipse.ShapeProperties.DrawFill = true;
                toggleIconEllipse.ForceMeshUpdate();

            }
            else
            {

                toggleIconRect.DOAnchorPos(new Vector2(-22.75f, 0.9f), .5f);

                toggleIconEllipse.ShapeProperties.DrawFill = false;
                toggleIconEllipse.ForceMeshUpdate();

            }

        }

        public void SetOn()
        {

            toggleObj.isOn = true;

            Toggle();

        }

        public void SetOff()
        {

            toggleObj.isOn = false;

            Toggle();

        }

    }

}