using System;
using UnityEngine;

namespace RedOwl.Engine
{
    public enum ImageFormat
    {
        PNG = 1,
        JPG = 2
    }

    internal static class TextureUtility
    {
        public static Texture2D Decode(string encodedImage)
        {
            if (encodedImage == null)
                return null;

            byte[] bytes = Convert.FromBase64String(encodedImage);
            return Decode(bytes);
        }

        private static Texture2D Decode(byte[] bytes)
        {
            if (bytes == null)
                return null;

            Texture2D texture = new Texture2D(1, 1, TextureFormat.RGB24, false);
            texture.LoadImage(bytes);
            texture.Apply();
            return texture;
        }

        public static string Encode(Texture2D texture, ImageFormat format = ImageFormat.PNG)
        {
            byte[] bytes = EncodeAsByteArray(texture, format);
            return bytes != null ? Convert.ToBase64String(bytes) : null;
        }

        private static byte[] EncodeAsByteArray(Texture2D texture, ImageFormat format = ImageFormat.PNG)
        {
            if (texture == null)
                return null;

            return format == ImageFormat.JPG ? texture.EncodeToJPG() : texture.EncodeToPNG();
        }
    }
}