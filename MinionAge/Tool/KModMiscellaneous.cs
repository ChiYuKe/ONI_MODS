using Klei.AI;
using KModTool;
using OxygenConsumingPlant.Tool;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KModTool
{
    public class KModMiscellaneous 
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




        public static void SetExpression(GameObject gameObject, Expression expression ,bool IsValidRemove = false)
        {
           
            // 获取 FaceGraph 组件
            FaceGraph faceGraph = gameObject.GetComponent<FaceGraph>();

            // 如果 FaceGraph 组件存在，则添加指定的表情
            if (faceGraph != null)
            {
                if (IsValidRemove)
                {
                    faceGraph.RemoveExpression(expression);
                   
                }
                else 
                {
                    faceGraph.AddExpression(expression);
                }
               
            }
            else
            {
                Debug.LogWarning("FaceGraph 组件未找到！");
            }
        }





        public static void PerformEmoteActionOnSingleGameObject(GameObject gameObject, float cooldownDuration, Emote specifiedEmote = null, bool PrintLog = false)
        {
            try
            {
                // 检查是否是有效的 GameObject
                if (gameObject == null)
                {
                    Debug.LogWarning("没有找到有效的 GameObject。");
                    return;
                }

                // 获取 MinionBrain 组件
                MinionBrain minionBrain = gameObject.GetComponent<MinionBrain>();
                if (minionBrain == null)
                {
                    return;
                }

                // 检查表情冷却状态
                if (!emoteCooldownStatus.ContainsKey(gameObject))
                {
                    emoteCooldownStatus[gameObject] = true;
                }

                if (emoteCooldownStatus[gameObject])
                {
                    // 获取 ChoreProvider 组件并执行表情
                    ChoreProvider choreProvider = gameObject.GetComponent<ChoreProvider>();
                    if (choreProvider != null)
                    {
                        Db db = Db.Get();
                        Emote emote = specifiedEmote ?? GetRandomEmote(db);

                        new EmoteChore(choreProvider, db.ChoreTypes.EmoteHighPriority, emote, 1, null);

                        // 启动冷却时间的协程
                        MonoBehaviour monoBehaviour = choreProvider.GetComponent<MonoBehaviour>();
                        if (monoBehaviour != null)
                        {
                            monoBehaviour.StartCoroutine(EmoteCooldown(gameObject, cooldownDuration));
                        }
                    }
                }

                // 如果启用了日志打印功能
                if (PrintLog)
                {
                    KModMinionUtils.PrintAllComponents(gameObject);
                }
            }
            catch (Exception ex)
            {
                Debug.LogError("执行表情动作时发生错误: " + ex.Message + "\n" + ex.StackTrace);
            }
        }

        private static Emote GetRandomEmote(Db db)
        {
            Emote[] emotes = new Emote[]
            {
                db.Emotes.Minion.ThumbsUp,
                db.Emotes.Minion.FingerGuns,
                db.Emotes.Minion.Cringe,
                db.Emotes.Minion.Radiation_Itch,
                db.Emotes.Minion.CloseCall_Fall,
                db.Emotes.Minion.ProductiveCheer
            };
            return emotes[UnityEngine.Random.Range(0, emotes.Length)];
        }






        public static void PerformEmoteActions(List<ScenePartitionerEntry> detectedEntries, float cooldownDuration, Emote specifiedEmote = null, bool printLog = false)
        {
            // 获取 Db 的引用，避免多次调用 Db.Get()
            Db db = Db.Get();

            // 遍历检测到的每个对象
            foreach (ScenePartitionerEntry scenePartitionerEntry in detectedEntries)
            {
                try
                {
                    // 获取 GameObject
                    GameObject gameObject = scenePartitionerEntry.obj as GameObject ?? (scenePartitionerEntry.obj as Pickupable)?.gameObject;

                    if (gameObject == null)
                    {
                        Debug.LogWarning("没有找到有效的 GameObject。");
                        continue;
                    }

                    // 获取 MinionBrain 和 ChoreProvider 组件
                    MinionBrain minionBrain = gameObject.GetComponent<MinionBrain>();
                    ChoreProvider choreProvider = gameObject.GetComponent<ChoreProvider>();

                    if (minionBrain == null || choreProvider == null)
                    {
                        continue; // 没有 MinionBrain 或 ChoreProvider 组件，跳过当前对象
                    }

                    // 检查冷却状态
                    if (!emoteCooldownStatus.ContainsKey(gameObject))
                    {
                        emoteCooldownStatus[gameObject] = true;
                    }

                    if (!emoteCooldownStatus[gameObject])
                    {
                        // 选择表情，默认是指定的表情或随机选择一个
                        if (specifiedEmote == null)
                        {
                            Emote[] emotes = new Emote[]
                            {
                                db.Emotes.Minion.ThumbsUp,
                                db.Emotes.Minion.FingerGuns,
                                db.Emotes.Minion.Cringe,
                                db.Emotes.Minion.Radiation_Itch,
                                db.Emotes.Minion.CloseCall_Fall,
                                db.Emotes.Minion.ProductiveCheer
                            };
                            specifiedEmote = emotes[UnityEngine.Random.Range(0, emotes.Length)];
                        }

                        // 创建 EmoteChore
                        new EmoteChore(choreProvider, db.ChoreTypes.EmoteHighPriority, specifiedEmote, 1, null);

                        // 启动冷却协程
                        MonoBehaviour component3 = choreProvider.GetComponent<MonoBehaviour>();
                        if (component3 != null)
                        {
                            component3.StartCoroutine(EmoteCooldown(gameObject, cooldownDuration));
                        }
                    }

                    // 如果需要打印日志，输出所有组件信息
                    if (printLog)
                    {
                        KModMinionUtils.PrintAllComponents(gameObject);
                    }
                }
                catch (Exception ex)
                {
                    Debug.LogError($"执行表情动作时发生错误: {ex.Message}\n{ex.StackTrace}");
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
