#if UNITY_EDITOR
using System;
using System.Reflection;

using UnityEngine;
using UnityEditor;


namespace DogScaffold
{
    /// <summary>
    /// An extension to Unity's built in TransformInspector. Adds the ability to zero out the local
    /// position, rotation, and scale of a transform via a button click, as well as surfacing the
    /// world space values of each.
    ///
    /// Source: https://forum.unity.com/threads/extending-instead-of-replacing-built-in-inspectors.407612/
    /// </summary>
    [CustomEditor(typeof(Transform), true)]
    [CanEditMultipleObjects]
    public class CustomTransformInspector : UnityEditor.Editor
    {
        UnityEditor.Editor defaultEditor;
        Transform transform;

        void OnEnable()
        {
            //Note: When this inspector is created, we also create the built-in inspector.
            defaultEditor = UnityEditor.Editor.CreateEditor(
                targets,
                Type.GetType("UnityEditor.TransformInspector, UnityEditor"));

            transform = target as Transform;
        }

        void OnDisable()
        {
            // Note: When OnDisable is called, the default editor we created should be destroyed to
            // avoid memory leakage. We also call any required methods like OnDisable.

            BindingFlags flags = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public;

            MethodInfo disableMethod = defaultEditor.GetType().GetMethod("OnDisable", flags);

            if (disableMethod != null)
            {
                disableMethod.Invoke(defaultEditor, null);
            }

            DestroyImmediate(defaultEditor);
        }

        public override void OnInspectorGUI()
        {
            // Note: Show Reset PSR buttons.
            EditorGUILayout.LabelField("Reset Local PSR", EditorStyles.boldLabel);
            if (GUILayout.Button("Reset All"))
            {
                ZeroAllLocal();
            }

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Reset Position"))
            {
                ZeroLocalPosition();
            }

            if (GUILayout.Button("Reset Rotation"))
            {
                ZeroLocalRotation();
            }

            if (GUILayout.Button("Reset Scale"))
            {
                ZeroLocalScale();
            }
            EditorGUILayout.EndHorizontal();

            // Note: Show default inspector.
            EditorGUILayout.LabelField("Local Space", EditorStyles.boldLabel);
            defaultEditor.OnInspectorGUI();

            // Note: Show World Space Transform information.
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("World Space", EditorStyles.boldLabel);

            GUI.enabled = false;
            Vector3 localPosition = transform.localPosition;
            transform.localPosition = transform.position;

            Quaternion localRotation = transform.localRotation;
            transform.localRotation = transform.rotation;

            Vector3 localScale = transform.localScale;
            transform.localScale = transform.lossyScale;

            defaultEditor.OnInspectorGUI();
            transform.localPosition = localPosition;
            transform.localRotation = localRotation;
            transform.localScale = localScale;
            GUI.enabled = true;
        }

        private void ZeroAllLocal()
        {
            ZeroLocalPosition();
            ZeroLocalRotation();
            ZeroLocalScale();
        }

        private void ZeroLocalPosition()
        {
            Undo.RecordObject(transform, "Reset local position");
            transform.localPosition = Vector3.zero;
        }

        private void ZeroLocalRotation()
        {
            Undo.RecordObject(transform, "Reset local rotation");
            transform.localEulerAngles = Vector3.zero;
        }

        private void ZeroLocalScale()
        {
            Undo.RecordObject(transform, "Reset local scale");
            transform.localScale = Vector3.one;
        }
    }
}
#endif
