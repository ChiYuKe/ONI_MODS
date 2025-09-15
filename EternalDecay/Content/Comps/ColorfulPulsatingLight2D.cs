using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

public class ColorfulPulsatingLight2D : KMonoBehaviour
{
    // 脉动速度
    public float PulseSpeed = 1.0f;

    // 颜色变化范围
    public Color[] Colors = new Color[]
    {
        Color.red,
        Color.green,
        Color.blue,
        Color.yellow,
        Color.magenta,
        Color.cyan
    };

    // 亮度变化范围
    public float MinIntensity = 500f;
    public float MaxIntensity = 1500f;

    // 灯光半径变化范围
    public float MinRadius = 3f;
    public float MaxRadius = 10f;

    private Light2D baseLight;
    private int currentColorIndex = 0;
    private float pulseTimer = 0f;

    protected override void OnPrefabInit()
    {
        base.OnPrefabInit();
        baseLight = gameObject.AddComponent<Light2D>();
        baseLight.Color = Colors[currentColorIndex];
        baseLight.Lux = (int)MinIntensity;
        baseLight.Range = MinRadius; 
        baseLight.shape = LightShape.Circle;
        baseLight.drawOverlay = true;
    }

    protected override void OnSpawn()
    {
        base.OnSpawn();
        pulseTimer = 0f;
    }

    protected override void OnCleanUp()
    {
        base.OnCleanUp();
    }

    private void Update()
    {
        if (baseLight == null)
            return;

        // 更新脉动计时器
        pulseTimer += Time.deltaTime * PulseSpeed;

        // 颜色变化
        if (pulseTimer >= 1f)
        {
            pulseTimer = 0f;
            currentColorIndex = (currentColorIndex + 1) % Colors.Length;
            baseLight.Color = Colors[currentColorIndex];
        }

        // 亮度变化
        float intensity = Mathf.Lerp(MinIntensity, MaxIntensity, Mathf.PingPong(Time.time * PulseSpeed, 1f));
        baseLight.Lux = (int)intensity;

        // 灯光半径变化
        float radius = Mathf.Lerp(MinRadius, MaxRadius, Mathf.PingPong(Time.time * PulseSpeed, 1f));
        baseLight.Range = radius;

        baseLight.FullRefresh();
    }

   
}
