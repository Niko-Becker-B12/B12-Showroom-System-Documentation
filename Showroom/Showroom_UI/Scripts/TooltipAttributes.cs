using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;


namespace Showroom
{
    [System.Serializable]
    public class TooltipAttributes
    {

        [SerializeField] public string tooltipName;
        [SerializeField] public bool useCodeSnippet;
        [HideIf("useCodeSnippet")] [SerializeField] public string tooltipMessage;
        //[ShowIf("useCodeSnippet")] [SerializeField] [TextArea(5, 25)] private string tooltipAttribute;


        private List<string> variableNames
        {
            get
            {
                return new List<string>()
                {
                    "None",
                    "Player is at Sub-Level Home",
                    "Player is at Use-Case Home"
                };
            }
        }

        private List<string> variableCalculations
        {
            get
            {
                return new List<string>()
                {
                    "==",
                    "!="
                };
            }
        }

        private List<string> variableCompare
        {
            get
            {
                return new List<string>()
                {
                    "True",
                    "False"
                };
            }
        }


        [ShowIf("useCodeSnippet")] [FoldoutGroup("Code Builder")] [Header("If")]
        [ShowIf("useCodeSnippet")] [FoldoutGroup("Code Builder")] [ValueDropdown("variableNames")] public string variable1;
        [ShowIf("useCodeSnippet")] [FoldoutGroup("Code Builder")] [ValueDropdown("variableCalculations")] public string variable1Calculator;
        [ShowIf("useCodeSnippet")] [FoldoutGroup("Code Builder")] [ValueDropdown("variableCompare")] public string variable1CalculatorCompareValue;
        [ShowIf("useCodeSnippet")] [FoldoutGroup("Code Builder")] [SerializeField] public string variable1ReturnValue;

        [ShowIf("useCodeSnippet")] [FoldoutGroup("Code Builder")] [Header("Else")]
        [ShowIf("useCodeSnippet")] [FoldoutGroup("Code Builder")] [SerializeField] public string tooltipMessageReturnValue;


    }
}