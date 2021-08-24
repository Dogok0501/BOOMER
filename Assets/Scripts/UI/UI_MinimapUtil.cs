using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_MinimapUtil
{
    public static Vector2 WorldPosToMapPos(Vector3 worldPos, float worldWidth, float worldDepth, float uiMapWidth, float uiMapHeight)
    {
        Vector2 result = Vector2.zero;
        result.x = (worldPos.x * uiMapWidth) / worldWidth - 150;
        result.y = (worldPos.z * uiMapHeight) / worldDepth - 100 - 105;
        return result;
    }

    public static Vector3 MapPosToWorldPos (Vector2 uiPos, float worldWidth, float worldDepth, float uiMapWidth, float uiMapHeight)
    {
        Vector3 result = Vector3.zero;
        result.x = (uiPos.x * worldWidth) / uiMapWidth;
        result.z = (uiPos.y * worldWidth) / uiMapHeight;
        return result;
    }

    public static void MarkOnTheShootingGame(Transform world, Transform uiTransform, float worldWidth, float worldDepth, float uiMapWidth, float uiMapHeight)
    {
        uiTransform.localPosition = WorldPosToMapPos(world.position, worldWidth, worldDepth, uiMapWidth, uiMapHeight);
        float angelZ = Mathf.Atan2(world.forward.z, world.forward.x) * Mathf.Rad2Deg;
        uiTransform.eulerAngles = new Vector3(0, 0, angelZ - 90);
    }

    public static void MarkOnTheRPG(Transform world, Transform uiIcon, RectTransform uiBackground, float worldWidth, float worldDepth)
    {
        uiBackground.localPosition = WorldPosToMapPos(world.position, worldWidth, worldDepth, uiBackground.sizeDelta.x, uiBackground.sizeDelta.y) * - 1;
        float angleZ = Mathf.Atan2(world.forward.z, world.forward.x) * Mathf.Rad2Deg;
        uiIcon.eulerAngles = new Vector3(0, 0, angleZ - 90);
    }
}
