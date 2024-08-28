using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class FunDateTimeAccess {

    static string[] cultureNames = { "en-US", "en-GB", "fr-FR", "de-DE", "ru-RU" };
    public static string GetCurrentTime(int cultureType = 1, int locationType = 0) {
        DateTime localDate = DateTime.Now;
        DateTime utcDate = DateTime.UtcNow;

        CultureInfo culture = new CultureInfo(cultureNames[cultureType]);
        
        if (locationType == 0) {
            return localDate.ToString(culture); // Local date and time
        } else if (locationType == 1) {
            return utcDate.ToString(culture); // UTC date and time
        } else {
            return culture.NativeName; // utcDate.Kind
        }
    }

    public static string GetTodayDate() {
        DateTime date1 = DateTime.Now;
        Console.WriteLine(date1.ToString());

//        System.DateTime
        // Get date-only portion of date, without its time.
        DateTime dateOnly = date1.Date;
        return dateOnly.ToString("d");
    }

    public static DateTime StringToDateTime(string timeString) {
        DateTime dt = DateTime.Parse(timeString, CultureInfo.CreateSpecificCulture(cultureNames[1]));
        return dt;
    }
    public static string DateTimeToString(DateTime dateTimeNow) {
        string str = dateTimeNow.Day + "/" + dateTimeNow.Month + "/" + dateTimeNow.Year + " " +
                    dateTimeNow.Hour + ":" + dateTimeNow.Minute + ":" + dateTimeNow.Second;
        return str;
    }

}
