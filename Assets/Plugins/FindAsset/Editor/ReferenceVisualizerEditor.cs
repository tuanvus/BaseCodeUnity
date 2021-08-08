using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif

/*
 * MADE BY SEBASTIAN SCHUCHMANN
 * WEBSITE: sebastian-schuchmann.com
 * GITHUB: https://github.com/Sebastian-Schuchmann
 * Feel free to use this as you like. You dont have to credit me, but
 * it would be appreciated if you do! <3
 */


public class ReferenceVisualizerEditor : EditorWindow
{
    Vector2 ScrollPosition;
    bool showAllProperties;

    [MenuItem("Window/Reference Visualizer")]
    public static void ShowWindow()
    {
        GetWindow(typeof(ReferenceVisualizerEditor));
    }

    void OnGUI()
    {
        var GameObjects = FindSceneGameObjects();

        showAllProperties = GUILayout.Toggle(showAllProperties, "Show all Attributes");

        GUIStyle Headline = new GUIStyle();
        Headline.fontSize = 12;
        Headline.fontStyle = FontStyle.Bold;

        GUIStyle Small = new GUIStyle();
        Small.fontSize = 8;

        GUIStyle LayoutGroup = new GUIStyle();
        LayoutGroup.padding = new RectOffset(10, 10, 10, 10);

        GUIStyle MemberGroup = new GUIStyle();
        MemberGroup.padding = new RectOffset(20, 0, 0, 0);

        ScrollPosition = GUILayout.BeginScrollView(ScrollPosition, LayoutGroup);

        foreach (var GO in GameObjects)
        {
            if (GO.GetComponents<MonoBehaviour>() != null)
            {
                foreach (var Component in GO.GetComponents<MonoBehaviour>())
                {
                    if (HasObjectPublicReferences(Component) || showAllProperties)
                    {
                        GUILayout.Space(10);
                        GUILayout.Label(GO.name, Headline);

                        GUILayout.BeginVertical(MemberGroup);
                        GUILayout.Label("Type: " + Component.GetType(), Small);
                        foreach (var member in Component.GetType().GetMembers())
                        {
                            //Only Public Members edited in the Unity Editor
                            if (member.GetType().ToString() == "System.Reflection.MonoField")
                            {
                                //Convert to SerlizedObject to acces to Properties
                                SerializedObject serializedComponent = new SerializedObject(Component);
                                var Prop = serializedComponent.FindProperty(member.Name);

                                if (Prop != null)
                                {
                                    if (Prop.propertyType == SerializedPropertyType.ObjectReference || Prop.propertyType == SerializedPropertyType.Generic || showAllProperties)
                                    {
                                        //Color Changes for Null Elements
                                        if (!showAllProperties && Prop.propertyType != SerializedPropertyType.Generic)
                                        {
                                            if (Prop.objectReferenceValue == null)
                                                GUI.color = new Color(1f, 0.4f, 0.4f);
                                            else
                                                GUI.color = Color.white;
                                        } //We have to do extra checking for Lists and Arrays (Generics)
                                        if (Prop.propertyType == SerializedPropertyType.Generic && !showAllProperties)
                                        {
                                            //Reset Color
                                            GUI.color = Color.white;
                                            bool isReference = false;

                                            //Array/List is empty
                                            if (Prop.arraySize == 0)
                                            {
                                                //We insert an Element to check the type and then delete it right after
                                                Prop.InsertArrayElementAtIndex(0);
                                                isReference |= Prop.GetArrayElementAtIndex(0).propertyType == SerializedPropertyType.ObjectReference;
                                                Prop.DeleteArrayElementAtIndex(0);
                                            }
                                            else
                                            {
                                                //If the array is filled, we just check the first element
                                                isReference |= Prop.GetArrayElementAtIndex(0).propertyType == SerializedPropertyType.ObjectReference;
                                            }

                                            if (isReference)
                                                EditorGUILayout.PropertyField(Prop, true);
                                        }
                                        else
                                        {
                                            EditorGUILayout.PropertyField(Prop, true);
                                        }
                                    }
                                }
                                serializedComponent.ApplyModifiedProperties();
                            }
                        }
                        GUILayout.EndVertical();
                    }
                }
            }
        }

        GUILayout.EndScrollView();
    }

    private static bool HasObjectPublicReferences(MonoBehaviour Object)
    {
        foreach (var member in Object.GetType().GetMembers())
        {
            //Only Public Members edited in the Unity Editor
            if (member.GetType().ToString() == "System.Reflection.MonoField")
            {
                SerializedObject serializedComponent = new SerializedObject(Object);
                var Prop = serializedComponent.FindProperty(member.Name);

                if (Prop != null)
                {
                    if (Prop.propertyType == SerializedPropertyType.ObjectReference || Prop.propertyType == SerializedPropertyType.Generic)
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    private static IEnumerable<GameObject> FindSceneGameObjects()
    {
        return Resources
            .FindObjectsOfTypeAll(typeof(GameObject))
            .Cast<GameObject>()
            // Only get objects from the active scene.
            .Where(go => go.scene == SceneManager.GetActiveScene())
            // Ignore hidden objects.
            .Where(go => (go.hideFlags & HideFlags.HideInHierarchy) != HideFlags.HideInHierarchy);
    }
}
