using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UnityUtils
{
    public static class TransformExtensions
    {
        
        public static GameObject[] GetAllChildrenAsGameObject(this Transform transform)
        {
            int childCount = transform.childCount;

            GameObject[] children = new GameObject[childCount];

            for (int i = 0; i < childCount; i++)
            {
                children[i] = transform.GetChild(i).gameObject;
            }

            return children;
        }
    }
}
