using UnityEngine;
using System.Collections;

namespace KModTool
{
    public class KModDelayedActionExecutor : KMonoBehaviour
    {
        private static KModDelayedActionExecutor instance;

        public static KModDelayedActionExecutor Instance
        {
            get
            {
                if (instance == null)
                {
                    GameObject go = new GameObject("KModDelayedActionExecutor");
                    instance = go.AddComponent<KModDelayedActionExecutor>();
                    DontDestroyOnLoad(go);
                }
                return instance;
            }
        }

        public void ExecuteAfterDelay(float delay, System.Action action)
        {
            StartCoroutine(DelayedExecution(delay, action));
        }

        private IEnumerator DelayedExecution(float delay, System.Action action)
        {
            yield return new WaitForSeconds(delay);
            action?.Invoke();
        }
    }
}
