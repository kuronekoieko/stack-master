using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;

namespace UnityTemplate
{
    /// <summary>
    /// どこからでも使う便利そうなメソッドをまとめておく
    /// </summary>
    public class Utils : MonoBehaviour
    {
        public static bool TryGetValue<T>(T[] array, int index, out T value)
        {
            if (IsIndexOutOfRange(array, index))
            {
                value = default;
                return false;
            }
            else
            {
                value = array[index];
                return true;
            }
        }
        public static bool IsIndexOutOfRange<T>(T[] array, int index)
        {
            return index < 0 || array.Length < index + 1;
        }

        public static bool IsIndexOutOfRange<T>(List<T> list, int index)
        {
            return index < 0 || list.Count < index + 1;
        }

        public static bool IsShowInterstitial(int stageNum)
        {
            //51以降は必ず表示
            if (stageNum > 50) { return true; }

            //31~50
            //2の倍数のとき表示
            if (stageNum > 30) { return stageNum % 2 == 0; }

            //6~30
            //3の倍数のとき表示
            if (stageNum > 5) { return stageNum % 3 == 0; }

            //〜5は表示しない
            return false;
        }

        public static DateTime UserDateTimeToDateTime(UserDateTime udt)
        {
            DateTime dt = new DateTime(udt.year, udt.month, udt.day, udt.hour, udt.minute, udt.second);
            return dt;
        }

        public static UserDateTime DateTimeToUserDateTime(DateTime dt)
        {
            UserDateTime udt = new UserDateTime
            {
                year = dt.Year,
                month = dt.Month,
                day = dt.Day,
                hour = dt.Hour,
                minute = dt.Minute,
                second = dt.Second,
            };
            return udt;
        }

        /// <summary>
        /// 前回の受け取りから24時間経っていたら、ログインボーナス画面を開く
        /// </summary>
        public static void OpenLoginBonus()
        {
            DateTime receivedDateTime = Utils.UserDateTimeToDateTime(SaveData.i.receivedLoginBonusUserDateTime);
            if (isOpenLoginBonus(DateTime.Now, receivedDateTime))
            {
                Variables.screenState = ScreenState.Login;
            }
        }

        private static bool isOpenLoginBonus(DateTime nowDateTime, DateTime receivedDateTime)
        {
            // "1日0時間0分0秒"のTimeSpanを作成
            TimeSpan timeSpan = new TimeSpan(1, 0, 0, 0);
            return receivedDateTime + timeSpan <= nowDateTime;
        }
    }
}

