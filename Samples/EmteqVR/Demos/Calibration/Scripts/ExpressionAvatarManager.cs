using System.Collections;
using UnityEngine;

namespace EmteqLabs
{
    public enum Expressions
    {
        Idle,
        SmileBothSides,
        SmileLeftSide,
        SmileRightSide,
        FrownBothSides,
        FrownLeftSide,
        FrownRightSide,
        SurpriseBothSides,
        SurpriseLeftSide,
        SurpriseRightSide,
        CloseBothEyes,
        CloseLeftEye,
        CloseRightEye,
        PuckerLips
    }

    public class ExpressionAvatarManager : MonoBehaviour
    {
        public Animator ExpressionAvatarAnimator;
        public Expressions SelectedExpression = Expressions.Idle;

        private AnimatorControllerParameter[] animatorParameters;

        public bool expressionSet = true;

        // Start is called before the first frame update
        void OnEnable()
        {
            ChangeExpression();
        }

        public void ChangeExpression()
        {
            switch (SelectedExpression)
            {
                case Expressions.Idle:
                    SetExpression("Idle");
                    break;
                case Expressions.SmileBothSides:
                    SetExpression("SmileBoth");
                    break;
                case Expressions.SmileLeftSide:
                    SetExpression("SmileLeft");
                    break;
                case Expressions.SmileRightSide:
                    SetExpression("SmileRight");
                    break;
                case Expressions.FrownBothSides:
                    SetExpression("FrownBoth");
                    break;
                case Expressions.FrownLeftSide:
                    SetExpression("FrownLeft");
                    break;
                case Expressions.FrownRightSide:
                    SetExpression("FrownRight");
                    break;
                case Expressions.SurpriseBothSides:
                    SetExpression("SurpriseBoth");
                    break;
                case Expressions.SurpriseLeftSide:
                    SetExpression("SurpriseLeft");
                    break;
                case Expressions.SurpriseRightSide:
                    SetExpression("SurpriseRight");
                    break;
                case Expressions.CloseBothEyes:
                    SetExpression("CloseEyesBoth");
                    break;
                case Expressions.CloseLeftEye:
                    SetExpression("CloseEyesLeft");
                    break;
                case Expressions.CloseRightEye:
                    SetExpression("CloseEyesRight");
                    break;
                case Expressions.PuckerLips:
                    SetExpression("PuckerLips");
                    break;
            }
        }

        private IEnumerator AnimateExpression(string expressionName)
        {
            float blendTreeAnimationFloat = 0;
            for (int i = 0; i <= 100; i++)
            {
                yield return new WaitForSeconds(0.005f);
                blendTreeAnimationFloat = (float) i / 100;
                ExpressionAvatarAnimator.SetFloat(expressionName, blendTreeAnimationFloat);
            }
        }

        private void SetExpression(string expressionName)
        {
            ExpressionAvatarAnimator.WriteDefaultValues();
            ExpressionAvatarAnimator.SetFloat(expressionName, 1);
        }
    }
}