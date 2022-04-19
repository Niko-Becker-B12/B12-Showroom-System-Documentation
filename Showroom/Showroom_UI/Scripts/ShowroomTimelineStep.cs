using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEditor;
using Sirenix.OdinInspector;
using UnityEngine.Playables;

namespace Showroom
{

    [Serializable]
    public class ShowroomTimelineStep
    {

        public PlayableDirector timeline;

        public List<Function> functions;

        public string infoText;

    }

}