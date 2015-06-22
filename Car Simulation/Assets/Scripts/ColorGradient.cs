using System.Collections;
using UnityEngine;
using System;

public static class ColorGradient{
    static float Normalize(float value, float min, float max)
    {
        return ((value - min) / (max - min));
    }
    public static Color MonoChrome(float value)
    {
        if (value == 0)
        {
            return new Color { a = 0, r = 1, g = 1, b = 1 };
        }
        return new Color { a = 1, r = value, g = value, b = value };
    }
    public static Color MonoChrome(float value, float min, float max)
    {
        if (value == 0)
        {
            return new Color { a = 0, r = 1, g = 1, b = 1 };
        }
        value = Mathf.Clamp(value, min, max);
        value = Normalize(value, min, max);
        return new Color { a = value, r = value, g = value, b = value };
    }
    public static Color TwoColorGradentRG(float value)
    {
        if (value == 0)
        {
            return new Color { a = 0, r = 1, g = 1, b = 1 };
        }
        return new Color { a = value, r = value, g = 1-value, b = 0 };
    }
    public static Color TwoColorGradentRG(float value, float min, float max)
    {
        if (value == 0)
        {
            return new Color { a = 0, r = 1, g = 1, b = 1 };
        }
        value = Mathf.Clamp(value, min, max);
        value = Normalize(value, min, max);
        return new Color { a = value, r = value, g = 1 - value, b = 0 };
    }
    public static Color TwoColorGradentRB(float value)
    {
        if (value == 0)
        {
            return new Color { a = 0, r = 1, g = 1, b = 1 };
        }
        return new Color { a = 1, r = value, g = 0, b = 1 - value };
    }
    public static Color TwoColorGradentRB(float value, float min, float max)
    {
        if (value == 0)
        {
            return new Color { a = 0, r = 1, g = 1, b = 1 };
        }
        float val = (value - min) / (max - min);
        return new Color { a = 1, r = val, g = 0, b = 1-val };
    }
    public static Color FullColorGradient(float value, float min, float max)
    {
        value = Mathf.Clamp(value, min, max);
        value = Normalize(value, min, max);
        float alpha = value;
        //float alpha = Mathf.Pow(Mathf.Sin(value), 10) + Mathf.Pow(Mathf.Sin(value), 2);//sin(x)^10+sin(x^2)
        //////////////////////////float alpha = (Mathf.Pow(Mathf.Sin(value), 3) * 1.3f)+0.05f;//sin(x^3+0.2)*1.3-0.2
        //float alpha = (float)Math.Sinh(value*0.25f) + (float)Math.Sin(value * 2);
        if (value == 0)
        {
            return new Color { a = alpha, r = 1, g = 1, b = 1 };
        }
        if (value <= 0.25f)
        {
            return new Color { a = alpha, r = 0, g = (Normalize(value, 0f, 0.25f)), b = 1 };
        }
        if (value <= 0.5f)
        {
            return new Color { a = alpha, r = 0, g = 1, b = 1 - (Normalize(value, 0.25f, 0.5f)) };
        }
        if (value <= 0.75f)
        {
            return new Color { a = alpha, r = (Normalize(value, 0.5f, 0.75f)), g = 1, b = 0 };
        }
        if (value < 1f)
        {
            return new Color { a = alpha, r = 1, g = 1 - (Normalize(value, 0.75f, 1f)), b = 0 };
        }
        return new Color { a = 1, r = 1, g = 0 , b = 0 };
    }
    public static Color FullColorBWR(float value, float min, float max)
    {
        value = Mathf.Clamp(value, min, max);
        value = Normalize(value, min, max);
        float alpha = value;
        if (value == 0)
        {
            return new Color { a = alpha, r = 1, g = 1, b = 1 };
        }
        if (value <= 0.33f)
        {
            return new Color { a = alpha, r = (Normalize(value, 0f, 0.33f)), g = (Normalize(value, 0f, 0.33f)), b = 1 };
        }
        if (value <= 0.66f)
        {
            return new Color { a = alpha, r = 1, g = 1, b = 1 - (Normalize(value, 0.33f, 0.66f)) };
        }
        if (value < 1f)
        {
            return new Color { a = alpha, r = 1, g = 1 - (Normalize(value, 0.66f, 1f)), b = 0 };
        }
        return new Color { a = 1, r = 1, g = 0, b = 0 };
    }
    public static Color FullColorWBR(float value, float min, float max)
    {
        value = Mathf.Clamp(value, min, max);
        value = Normalize(value, min, max);
        float alpha = value;
        if (value == 0)
        {
            return new Color { a = alpha, r = 1, g = 1, b = 1 };
        }
        if (value <= 0.5f)
        {
            return new Color { a = alpha, r = 1-(Normalize(value, 0f, 0.5f)), g = 1-(Normalize(value, 0f, 0.5f)), b = 1 };
        }
        if (value <= 1f)
        {
            return new Color { a = alpha, r = (Normalize(value, 0.5f, 1f)), g = 0, b = 1 - (Normalize(value, 0.5f, 1f)) };
        }
        return new Color { a = 1, r = 1, g = 0, b = 0 };
    }
    public static Color FullColorWGB(float value, float min, float max)
    {
        value = Mathf.Clamp(value, min, max);
        value = Normalize(value, min, max);
        float alpha = value;
        if (value == 0)
        {
            return new Color { a = alpha, r = 1, g = 1, b = 1 };
        }
        if (value <= 0.5f)
        {
            return new Color { a = alpha, r = 1 - (Normalize(value, 0f, 0.5f)), g = 1, b = 1 - (Normalize(value, 0f, 0.5f)) };
        }
        if (value <= 1f)
        {
            return new Color { a = alpha, r = 0, g = 1 - (Normalize(value, 0f, 0.5f)), b = 0 };
        }
        return new Color { a = 1, r = 0, g = 0, b = 0 };
    }
    public static Color FullColorCGY(float value, float min, float max)
    {
        value = Mathf.Clamp(value, min, max);
        value = Normalize(value, min, max);
        float alpha = value;
        if (value == 0)
        {
            return new Color { a = alpha, r = 1, g = 1, b = 1 };
        }
        if (value <= 1f)
        {
            return new Color { a = alpha, r =(Normalize(value, 0f, 1f)), g = 1, b = 1 - (Normalize(value, 0f, 1f)) };
        }
        return new Color { a = 1, r = 0, g = 0, b = 0 };
    }
    
}
