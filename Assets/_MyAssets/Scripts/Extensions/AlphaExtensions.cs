using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public static class ColorExtensions
{
    public static void SetAlpha(this Image self, float a)
    {
        var color = self.color;
        color.a = a;
        self.color = color;
    }

    public static void SetAlpha(this Material self, float a)
    {
        var color = self.color;
        color.a = a;
        self.color = color;
    }

    public static void SetAlpha(this SpriteRenderer self, float a)
    {
        var color = self.color;
        color.a = a;
        self.color = color;
    }

    public static void SetAlpha(this Text self, float a)
    {
        var color = self.color;
        color.a = a;
        self.color = color;
    }

    public static void SetAlpha(this TextMeshPro self, float a)
    {
        var color = self.color;
        color.a = a;
        self.color = color;
    }

    public static void SetAlpha(this TextMeshProUGUI self, float a)
    {
        var color = self.color;
        color.a = a;
        self.color = color;
    }
}
