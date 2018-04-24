using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Util
{
    public class PlateConfig : MonoBehaviour
    {
        public enum PlateColor
        {
            HighScoreColor = 3,
            MiddleScoreColor = 2,
            LowScoreColor = 1
        }

        public enum PlateSize
        {
            Large = 3,
            Middle = 2,
            Small = 1
        }

        public enum PlateSpeed
        {
            Fast = 3,
            Middle = 2,
            Slow = 1
        }

        public PlateColor Color;
        public PlateSize Size;
        public PlateSpeed Speed;
        
        public int GetScore()
        {
            return (int) Size + (int) Speed + (int) Color;
        }

        public void Randomize()
        {
            Size = (PlateSize) Enum.GetValues(typeof(PlateSize)).GetValue(Random.Range(0, 3));
            Speed = (PlateSpeed) Enum.GetValues(typeof(PlateSpeed)).GetValue(Random.Range(0, 3));
            Color = (PlateColor) Enum.GetValues(typeof(PlateColor)).GetValue(Random.Range(0, 3));
        }
    }

    public static class PlateConfigExtension
    {
        public static float GetRawSizeScale(this PlateConfig.PlateSize size)
        {
            switch (size)
            {
                case PlateConfig.PlateSize.Large: return 1.25f;
                case PlateConfig.PlateSize.Middle: return 1f;
                case PlateConfig.PlateSize.Small: return 0.75f;
                default: return 1f;
            }
        }

        public static float GetRawSpeed(this PlateConfig.PlateSpeed speed)
        {
            switch (speed)
            {
                case PlateConfig.PlateSpeed.Fast: return 12.5f;
                case PlateConfig.PlateSpeed.Middle: return 10f;
                case PlateConfig.PlateSpeed.Slow: return 7.5f;
                default: return 10f;
            }
        }

        public static Color GetRawColor(this PlateConfig.PlateColor color)
        {
            switch (color)
            {
                case PlateConfig.PlateColor.HighScoreColor: return Color.red;
                case PlateConfig.PlateColor.MiddleScoreColor: return Color.yellow;
                case PlateConfig.PlateColor.LowScoreColor: return Color.green;
                default: return Color.yellow;
            }
        }
    }
}