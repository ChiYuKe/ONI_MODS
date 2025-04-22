using Klei.AI;
using KModTool;
using OxygenConsumingPlant.Tool;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OxygenConsumingPlant
{
    public static class Miscellaneous
    {
        /// <summary>
        /// 调整植物监视单元中的温度。
        /// </summary>
        /// <param name="selfGameObject">当前对象的 GameObject。</param>
        /// <param name="plantMonitorCells">植物监视单元的列表。</param>
        /// <param name="targetTemperature">目标温度。</param>
        /// <param name="temperatureIncrement">温度增量。</param>
        /// <param name="tolerance">温度容忍度。</param>
        /// <param name="excludedTag">排除的标签。</param>
        public static void AdjustTemperature(GameObject selfGameObject, List<ScenePartitionerEntry> plantMonitorCells, float targetTemperature, float temperatureIncrement, float tolerance, Tag excludedTag)
        {
            try
            {
                // 获取自身的 PrimaryElement 组件
                PrimaryElement selfPrimaryElement = selfGameObject.GetComponent<PrimaryElement>();
                bool isSelfPrimaryElementNull = selfPrimaryElement == null;
                if (isSelfPrimaryElementNull)
                {
                    Debug.LogError("自身主要元素为空.");
                }
                else
                {
                    bool allTemperaturesWithinTolerance = true;
                    foreach (ScenePartitionerEntry scenePartitionerEntry in plantMonitorCells)
                    {
                        object obj = scenePartitionerEntry.obj;
                        GameObject plantGameObject = obj as GameObject;

                        // 检查对象是否为 GameObject
                        bool isPlantGameObjectNull = plantGameObject == null;
                        if (isPlantGameObjectNull)
                        {
                            Pickupable pickupable = obj as Pickupable;
                            bool isPickupableNotNull = pickupable != null;
                            if (isPickupableNotNull)
                            {
                                plantGameObject = pickupable.gameObject;
                            }
                        }

                        // 检查对象是否为 KPrefabID
                        bool isPlantGameObjectStillNull = plantGameObject == null;
                        if (isPlantGameObjectStillNull)
                        {
                            KPrefabID kprefabID = obj as KPrefabID;
                            bool isKPrefabIDNotNull = kprefabID != null;
                            if (isKPrefabIDNotNull)
                            {
                                plantGameObject = kprefabID.gameObject;
                            }
                        }
                        // TODO 判断有点多余，有时间再改一下
                        // 处理不为空的植物对象
                        bool isPlantGameObjectValid = plantGameObject != null;
                        if (isPlantGameObjectValid)
                        {
                            // 忽略自身的 GameObject
                            bool isSameAsSelf = plantGameObject == selfPrimaryElement.gameObject;
                            if (!isSameAsSelf)
                            {
                                KPrefabID plantKPrefabID = plantGameObject.GetComponent<KPrefabID>();
                                bool hasExcludedTag = plantKPrefabID != null && plantKPrefabID.HasTag(excludedTag);
                                if (!hasExcludedTag)
                                {
                                    PrimaryElement plantPrimaryElement = plantGameObject.GetComponent<PrimaryElement>();
                                    bool isPlantPrimaryElementNotNull = plantPrimaryElement != null;
                                    if (isPlantPrimaryElementNotNull)
                                    {
                                        // 检查温度差异是否超过容忍度
                                        bool isTemperatureDifferenceExceedsTolerance = Mathf.Abs(plantPrimaryElement.Temperature - targetTemperature) > tolerance;
                                        if (isTemperatureDifferenceExceedsTolerance)
                                        {
                                            // 调整温度
                                            bool isPlantTemperatureBelowTarget = plantPrimaryElement.Temperature < targetTemperature;
                                            if (isPlantTemperatureBelowTarget)
                                            {
                                                selfPrimaryElement.Temperature -= temperatureIncrement * ((float)plantMonitorCells.Count * 0.015f);
                                                plantPrimaryElement.Temperature += temperatureIncrement;
                                            }
                                            else
                                            {
                                                bool isPlantTemperatureAboveTarget = plantPrimaryElement.Temperature > targetTemperature;
                                                if (isPlantTemperatureAboveTarget)
                                                {
                                                    selfPrimaryElement.Temperature += temperatureIncrement * ((float)plantMonitorCells.Count * 0.015f);
                                                    plantPrimaryElement.Temperature -= temperatureIncrement;
                                                }
                                            }
                                            allTemperaturesWithinTolerance = false;
                                        }
                                    }
                                    else
                                    {
                                        Debug.LogWarning(string.Format("GameObject {0} 没有 PrimaryElement 组件.", obj));
                                    }
                                }
                            }
                        }
                        else
                        {
                            Debug.LogWarning(string.Format("对象引用 {0} 不是 GameObject、Pickupable 或 KPrefabID.", obj));
                        }
                    }

                    // 如果所有温度都在容忍范围内，可以执行一些操作（目前没有操作）
                    bool areAllTemperaturesWithinTolerance = allTemperaturesWithinTolerance;
                    if (areAllTemperaturesWithinTolerance)
                    {
                        // 可以在这里添加相关逻辑
                    }
                }
            }
            catch (Exception exception)
            {
                // 记录调整温度时的异常信息
                Debug.LogError("调整温度时发生异常: " + exception.Message);
            }
        }



        public static void PerformEmoteActions(List<ScenePartitionerEntry> detectedEntries, float cooldownDuration, Emote specifiedEmote = null, bool PrintLog = false)
        {
            foreach (ScenePartitionerEntry scenePartitionerEntry in detectedEntries)
            {
                try
                {
                    object obj = scenePartitionerEntry.obj;
                    GameObject gameObject = obj as GameObject;
                    bool flag = gameObject == null;
                    if (flag)
                    {
                        Pickupable pickupable = obj as Pickupable;
                        bool flag2 = pickupable != null;
                        if (flag2)
                        {
                            gameObject = pickupable.gameObject;
                        }
                    }
                    bool flag3 = gameObject != null;
                    if (flag3)
                    {
                        MinionBrain component = gameObject.GetComponent<MinionBrain>();
                        bool flag4 = component != null;
                        if (flag4)
                        {
                            bool flag5 = !emoteCooldownStatus.ContainsKey(gameObject);
                            if (flag5)
                            {
                                emoteCooldownStatus[gameObject] = true;
                            }
                            bool flag6 = emoteCooldownStatus[gameObject];
                            if (flag6)
                            {
                                ChoreProvider component2 = gameObject.GetComponent<ChoreProvider>();
                                bool flag7 = component2 != null;
                                if (flag7)
                                {
                                    Db db = Db.Get();
                                    Emote emote = specifiedEmote;
                                    bool flag8 = emote == null;
                                    if (flag8)
                                    {
                                        Emote[] array = new Emote[]
                                        {
                                            db.Emotes.Minion.ThumbsUp,
                                            db.Emotes.Minion.FingerGuns,
                                            db.Emotes.Minion.Cringe,
                                            db.Emotes.Minion.Radiation_Itch,
                                            db.Emotes.Minion.CloseCall_Fall,
                                            db.Emotes.Minion.ProductiveCheer
                                        };
                                        emote = array[global::UnityEngine.Random.Range(0, array.Length)];
                                    }
                                    new EmoteChore(component2, db.ChoreTypes.EmoteHighPriority, emote, 1, null);
                                    MonoBehaviour component3 = component2.GetComponent<MonoBehaviour>();
                                    bool flag9 = component3 != null;
                                    if (flag9)
                                    {
                                        component3.StartCoroutine(EmoteCooldown(gameObject, cooldownDuration));
                                    }
                                }
                            }
                        }
                        if (PrintLog)
                        {
                            KModMinionUtils.PrintAllComponents(gameObject);
                            //Component[] components = gameObject.GetComponents<Component>();
                            //foreach (Component component4 in components)
                            //{
                            //    Debug.Log("GameObject '" + gameObject.name + "' 的组件：" + component4.GetType().Name);
                            //}
                        }
                    }
                    else
                    {
                        Debug.LogWarning("没有找到有效的 GameObject。");
                    }
                }
                catch (Exception ex)
                {
                    Debug.LogError("执行表情动作时发生错误: " + ex.Message + "\n" + ex.StackTrace);
                }
            }
        }
        private static IEnumerator EmoteCooldown(GameObject gameObject, float cooldownDuration)
        {
            emoteCooldownStatus[gameObject] = false;
            yield return new WaitForSeconds(cooldownDuration);
            emoteCooldownStatus[gameObject] = true;
            yield break;
        }

        private static Dictionary<GameObject, bool> emoteCooldownStatus = new Dictionary<GameObject, bool>();
    }
}
