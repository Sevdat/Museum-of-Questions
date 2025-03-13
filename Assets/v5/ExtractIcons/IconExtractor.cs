using System;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using UnityEngine;
using IWshRuntimeLibrary;

public class ExtractIcon: MonoBehaviour {
    // Constants for SHGetFileInfo
    private const uint SHGFI_ICON = 0x100;
    private const uint SHGFI_LARGEICON = 0x0; // Large icon
    private const uint SHGFI_SMALLICON = 0x1; // Small icon

    // Import SHGetFileInfo from shell32.dll
    [DllImport("shell32.dll", CharSet = CharSet.Auto)]
    private static extern IntPtr SHGetFileInfo(string pszPath, uint dwFileAttributes, ref SHFILEINFO psfi, uint cbSizeFileInfo, uint uFlags);

    // Import DestroyIcon from user32.dll
    [DllImport("user32.dll", SetLastError = true)]
    private static extern bool DestroyIcon(IntPtr hIcon);

    // SHFILEINFO structure
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    private struct SHFILEINFO
    {
        public IntPtr hIcon;
        public int iIcon;
        public uint dwAttributes;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
        public string szDisplayName;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
        public string szTypeName;
    }

    // public Sprite getGenericIcon(string path){
    //     Sprite iconSprite;
    //     Bitmap bitmap = new Bitmap(path);
    //     IntPtr iconHandle = bitmap.GetHicon();
    //     Icon icon = System.Drawing.Icon.FromHandle(iconHandle);
    //     Texture2D iconTexture = IconToTexture2D(icon);
    //     iconSprite = Sprite.Create(iconTexture, new Rect(0, 0, iconTexture.width, iconTexture.height), new Vector2(0.5f, 0.5f));
    //     DestroyIcon(iconHandle);
    //     return iconSprite;
    // }
    // Method to get the icon for any file type
    public Sprite getGenericIcon(string path){
        if (string.IsNullOrEmpty(path) || !System.IO.File.Exists(path)){
            Debug.LogError("File does not exist: " + path);
            return CreateDefaultSprite();
        }
        path = ResolveShortcutTarget(path);
        SHFILEINFO shinfo = new SHFILEINFO();

        // Get the icon handle for the file
        IntPtr hIcon = SHGetFileInfo(path, 0, ref shinfo, (uint)Marshal.SizeOf(shinfo), SHGFI_ICON | SHGFI_LARGEICON);

        if (hIcon != IntPtr.Zero){
            // Convert the icon to a .NET Icon object
            Icon fileIcon = Icon.FromHandle(shinfo.hIcon);

            // Convert the icon to a Texture2D
            Texture2D iconTexture = IconToTexture2D(fileIcon);

            // Destroy the icon handle to avoid memory leaks
            DestroyIcon(shinfo.hIcon);

            if (iconTexture != null){
                // Create a Sprite from the Texture2D
                Sprite iconSprite = Sprite.Create(
                    iconTexture,
                    new Rect(0, 0, iconTexture.width, iconTexture.height),
                    new Vector2(0.5f, 0.5f) // Pivot point (center)
                );

                return iconSprite;
            }
        }

        Debug.LogError("Failed to extract icon for file: " + path);
        return CreateDefaultSprite();
    }
    // Resolve the target path of a shortcut
    private string ResolveShortcutTarget(string shortcutPath){
        try {
            WshShell shell = new WshShell();
            IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(shortcutPath);
            return shortcut.TargetPath;
        }
        catch {
            return shortcutPath;
        }
    }
    private Texture2D IconToTexture2D(Icon icon) {
        using (Bitmap bitmap = icon.ToBitmap()){
            int width = bitmap.Width;
            int height = bitmap.Height;

            // Create a Texture2D with the same dimensions
            Texture2D texture = new Texture2D(width, height, TextureFormat.ARGB32, false);

            // Lock the bitmap data
            System.Drawing.Imaging.BitmapData data = bitmap.LockBits(
                new Rectangle(0, 0, width, height),
                System.Drawing.Imaging.ImageLockMode.ReadOnly,
                System.Drawing.Imaging.PixelFormat.Format32bppArgb
            );

            // Create a buffer to hold the pixel data
            byte[] pixelData = new byte[data.Stride * height];
            Marshal.Copy(data.Scan0, pixelData, 0, pixelData.Length);

            // Unlock the bitmap
            bitmap.UnlockBits(data);

            // Fix the pixel data (swap channels and flip rows)
            byte[] fixedPixelData = new byte[width * height * 4];
            for (int y = 0; y < height; y++){
                for (int x = 0; x < width; x++){
                    // Calculate the source and destination indices
                    int srcIndex = y * data.Stride + x * 4;
                    int dstIndex = ((height - 1 - y) * width + x) * 4;

                    // Swap BGRA to ARGB (Unity's expected format)
                    fixedPixelData[dstIndex + 0] = pixelData[srcIndex + 3]; // A
                    fixedPixelData[dstIndex + 1] = pixelData[srcIndex + 2]; // R
                    fixedPixelData[dstIndex + 2] = pixelData[srcIndex + 1]; // G
                    fixedPixelData[dstIndex + 3] = pixelData[srcIndex + 0]; // B
                }
            }

            // Load the fixed pixel data into the texture
            texture.LoadRawTextureData(fixedPixelData);
            texture.Apply();

            return texture;
        }
    }

    // Helper method to create a default sprite
    private Sprite CreateDefaultSprite()
    {
        return Sprite.Create(new Texture2D(1, 1), new Rect(0, 0, 1, 1), new Vector2(0.5f, 0.5f));
    }
}