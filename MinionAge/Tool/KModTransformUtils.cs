using UnityEngine;


namespace KModTool
{
    public static class KModTransformUtils
    {

        public static bool TryGetPosition(GameObject gameObject, out float x, out float y)
        {
            x = 0f;
            y = 0f;
            bool flag = gameObject == null;
            bool flag2;
            if (flag)
            {
                global::Debug.LogError("【KMod】GameObject is null.");
                flag2 = false;
            }
            else
            {
                Transform component = gameObject.GetComponent<Transform>();
                bool flag3 = component == null;
                if (flag3)
                {
                    global::Debug.LogError("【KMod】GameObject does not have a Transform component.");
                    flag2 = false;
                }
                else
                {
                    x = component.position.x;
                    y = component.position.y;
                    flag2 = true;
                }
            }
            return flag2;
        }

        public static bool TryGetRotation(GameObject gameObject, out float zRotation)
        {
            zRotation = 0f;
            bool flag = gameObject == null;
            bool flag2;
            if (flag)
            {
                global::Debug.LogError("【KMod】GameObject is null.");
                flag2 = false;
            }
            else
            {
                Transform component = gameObject.GetComponent<Transform>();
                bool flag3 = component == null;
                if (flag3)
                {
                    global::Debug.LogError("【KMod】GameObject does not have a Transform component.");
                    flag2 = false;
                }
                else
                {
                    zRotation = component.eulerAngles.z;
                    flag2 = true;
                }
            }
            return flag2;
        }

        public static bool TryGetScale(GameObject gameObject, out float xScale, out float yScale)
        {
            xScale = 0f;
            yScale = 0f;
            bool flag = gameObject == null;
            bool flag2;
            if (flag)
            {
                global::Debug.LogError("【KMod】GameObject is null.");
                flag2 = false;
            }
            else
            {
                Transform component = gameObject.GetComponent<Transform>();
                bool flag3 = component == null;
                if (flag3)
                {
                    global::Debug.LogError("【KMod】GameObject does not have a Transform component.");
                    flag2 = false;
                }
                else
                {
                    xScale = component.localScale.x;
                    yScale = component.localScale.y;
                    flag2 = true;
                }
            }
            return flag2;
        }

        public static bool TrySetPosition(GameObject gameObject, float x, float y)
        {
            bool flag = gameObject == null;
            bool flag2;
            if (flag)
            {
                global::Debug.LogError("【KMod】GameObject is null.");
                flag2 = false;
            }
            else
            {
                Transform component = gameObject.GetComponent<Transform>();
                bool flag3 = component == null;
                if (flag3)
                {
                    global::Debug.LogError("【KMod】GameObject does not have a Transform component.");
                    flag2 = false;
                }
                else
                {
                    component.position = new Vector3(x, y, component.position.z);
                    flag2 = true;
                }
            }
            return flag2;
        }

        public static bool TrySetRotation(GameObject gameObject, float zRotation)
        {
            bool flag = gameObject == null;
            bool flag2;
            if (flag)
            {
                global::Debug.LogError("【KMod】GameObject is null.");
                flag2 = false;
            }
            else
            {
                Transform component = gameObject.GetComponent<Transform>();
                bool flag3 = component == null;
                if (flag3)
                {
                    global::Debug.LogError("【KMod】GameObject does not have a Transform component.");
                    flag2 = false;
                }
                else
                {
                    component.eulerAngles = new Vector3(component.eulerAngles.x, component.eulerAngles.y, zRotation);
                    flag2 = true;
                }
            }
            return flag2;
        }

        public static bool TrySetScale(GameObject gameObject, float xScale, float yScale)
        {
            bool flag = gameObject == null;
            bool flag2;
            if (flag)
            {
                global::Debug.LogError("【KMod】GameObject is null.");
                flag2 = false;
            }
            else
            {
                Transform component = gameObject.GetComponent<Transform>();
                bool flag3 = component == null;
                if (flag3)
                {
                    global::Debug.LogError("【KMod】GameObject does not have a Transform component.");
                    flag2 = false;
                }
                else
                {
                    component.localScale = new Vector3(xScale, yScale, component.localScale.z);
                    flag2 = true;
                }
            }
            return flag2;
        }
    }
}