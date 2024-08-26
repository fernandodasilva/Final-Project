using UnityEngine;
using System;

namespace ENDB
{
    /// <summary>
    /// Use this attribute to draw preview icon for sprites and textures  
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class DatabaseAssetPreview : PropertyAttribute 
    {
        public int width;
        public int height;
        
        public DatabaseAssetPreview(int width, int height)
        {
            this.width = width;
            this.height = height;
        }
    }
}