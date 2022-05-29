using UnityEditor;
using UnityEngine;

namespace R60N.Utility
{
    [CustomPropertyDrawer(typeof(MinMaxFloat))]
    public class MinMaxFloatDrawer : PropertyDrawer
    {
        float[] minMax = new float[2];
        GUIContent[] subLabels = new[] { new GUIContent("Min"), new GUIContent("Max") };

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            var minProp = property.FindPropertyRelative("min");
            var maxProp = property.FindPropertyRelative("max");

            minMax[0] = minProp.floatValue;
            minMax[1] = maxProp.floatValue;

            EditorGUI.MultiFloatField(position, label, subLabels, minMax);

            if (minMax[0] > minMax[1])
            {
                if (minProp.floatValue != minMax[0])
                {
                    maxProp.floatValue = minMax[0];
                    minProp.floatValue = minMax[0];
                }
                else
                {
                    minProp.floatValue = minMax[1];
                    maxProp.floatValue = minMax[1];
                }
            }
            else
            {
                minProp.floatValue = minMax[0];
                maxProp.floatValue = minMax[1];
            }

            EditorGUI.EndProperty();
        }
    }
}
