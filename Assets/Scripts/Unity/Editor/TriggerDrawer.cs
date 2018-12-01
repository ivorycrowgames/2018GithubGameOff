using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEditor;

[CustomPropertyDrawer(typeof(ObjectAsName))]
public class ObjectAsNameDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        if (property.propertyType == SerializedPropertyType.String)
        {
            ObjectAsName attribute = this.attribute as ObjectAsName;
            Object objectReference = null;
            string objectName = property.stringValue;
            if (objectName.Length > 0)
            {
                var go = GetObjectInSceneByName(objectName);
                objectReference = go ? go.GetComponent(attribute.type): null;
            }

            objectReference = EditorGUI.ObjectField(position, label, objectReference, attribute.type, true);
            if (objectReference)
            {
                if (objectReference.name != property.stringValue)
                {
                    if (objectReference)
                    {
                        Undo.RecordObject(objectReference, "Changed "+ attribute.type.ToString() + " " + objectName);
                    }

                    property.stringValue = objectReference.name;
                }
            }
            else
            {
                property.stringValue = "";
            }
        }
        else
        {
            EditorGUI.LabelField(position, label.text, "Use [ObjectAsName] with strings.");
        }
    }

    private GameObject GetObjectInSceneByName(string name)
    {
        foreach (GameObject go in Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[])
        {
            if (go.hideFlags != HideFlags.None)
            {
                continue;
            }

            if (PrefabUtility.GetPrefabType(go) == PrefabType.Prefab || PrefabUtility.GetPrefabType(go) == PrefabType.ModelPrefab)
            {
                continue;
            }

            if (go.name == name)
            {
                return go;
            }
        }

        return null;
    }
}