using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UnityUtils
{
    public static class TransformExtensions
    {

        public static GameObject[] GetAllChildGameObject(this Transform transform)
        {
            int childCount = transform.childCount;

            GameObject[] children = new GameObject[childCount];

            for (int i = 0; i < childCount; i++)
            {
                children[i] = transform.GetChild(i).gameObject;
            }

            return children;
        }

        public static T[] GetAllChildren<T>(this Transform parent) where T : Component
        {
            int childCount = parent.childCount;

            var children = new List<T>();

            for (int i = 0; i < childCount; i++)
            { 
                if (parent.GetChild(i).TryGetComponent<T>(out var component))
                {
                    children.Add(component);
                }
            }

            if (children.Count == 0)
            {
                return new T[0];
            }

            return children.ToArray();
        }
    }
}