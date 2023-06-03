using System.Collections;
using UnityEngine;


/// <summary>
/// Храним глобальные состояния игры
/// </summary>
public class DataHolder : MonoBehaviour
{
    /// <summary>
    /// Состояние сундука
    /// </summary>
    public static bool CanActive = false;

    public static bool isLevelChanged;
    public static string lastLevel;
    /// <summary>
    /// Число участников группы
    /// </summary>
    public static int bandCount = 0;
    public static bool gitaristAdded = false;
    public static bool barabanshikAdded = false;
    public static bool pionistAdded = false;
    /// <summary>
    /// Число зрителей
    /// </summary>
    public static int watchers = 0;
    /// <summary>
    /// Булевая переменная, будет использоваться в квесте
    /// </summary>
    public static bool wasRoomCleaned = false;
    public static bool wasWifeSatisfacted = false;
    // Булевые переменные, нужные для того, чтобы диалоги нельзя было начать сначала
    public static bool wasFirstFriendDialog = false;
    public static bool wasFinalWifeDialog = false;
    public static bool wasSanichDialog = false;
    public static bool wasBarmanDialog = false;
    public static bool wasWorkersDialog = false;

    /// <summary>
    /// Добавляем в команду гитариста
    /// </summary>
    public static void AddGuitarist()
    {
        gitaristAdded = true;
        bandCount++;
    }

    /// <summary>
    /// Добавляем в команду барабанщика
    /// </summary>
    public static void AddBarabanshik()
    {
        barabanshikAdded = true;
        bandCount++;
    }

    /// <summary>
    /// Добавляем в команду пианиста
    /// </summary>
    public static void AddPionist()
    {
        pionistAdded = true;
        bandCount++;
    }

    /// <summary>
    /// Добавляем зрителей
    /// </summary>
    public static void AddWatchers()
    {
        watchers += 10;
    }
}
