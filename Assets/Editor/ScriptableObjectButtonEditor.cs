using System;
using UnityEngine;
using UnityEditor;
using System.Reflection;

[CustomEditor(typeof(ScriptableObject), true)]
public class ScriptableObjectButtonEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // 기본 인스펙터를 그립니다.
        DrawDefaultInspector();

        // 대상 객체의 타입을 가져옵니다.
        Type targetType = target.GetType();

        // 대상 객체의 모든 메서드를 가져옵니다.
        MethodInfo[] methods = targetType.GetMethods(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);

        foreach (MethodInfo method in methods)
        {
            // [Button] 애트리뷰트가 붙은 메서드를 찾습니다.
            var buttonAttribute = (ButtonAttribute)Attribute.GetCustomAttribute(method, typeof(ButtonAttribute));

            if (buttonAttribute != null)
            {
                // 버튼 레이블 설정
                string buttonLabel = string.IsNullOrEmpty(buttonAttribute.ButtonLabel) ? method.Name : buttonAttribute.ButtonLabel;

                // 버튼을 그립니다.
                if (GUILayout.Button(buttonLabel))
                {
                    // 메서드 호출
                    method.Invoke(target, null);
                }
            }
        }
    }
}
