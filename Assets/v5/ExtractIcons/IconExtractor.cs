using System;
using System.Drawing; // For System.Drawing.Bitmap, System.Drawing.Icon, etc.
using System.Drawing.Imaging; // For PixelFormat
using System.Runtime.InteropServices;
using UnityEngine;
using IWshRuntimeLibrary;

public class ExtractIcon : MonoBehaviour
{
    [DllImport("shell32.dll", CharSet = CharSet.Auto)]
    private static extern IntPtr ExtractIconEx(string lpszFile, int nIconIndex, out IntPtr phiconLarge, out IntPtr phiconSmall, uint nIcons);

    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    private static extern bool DestroyIcon(IntPtr handle);

    void Start()
    {
        getIcon(@"C:\Users\Public\Desktop\Unity 2022.3.17f1.lnk");
    }

    public void getIcon(string path){
        path = ResolveShortcutTarget(path); // Path to the shortcut
        // Extract the icon from the target file
        IntPtr largeIconPtr;
        IntPtr smallIconPtr;
        ExtractIconEx(path, 0, out largeIconPtr, out smallIconPtr, 1);

        if (largeIconPtr != IntPtr.Zero){
            // Convert the icon to a Texture2D
            Texture2D iconTexture = IconToTexture2D(largeIconPtr);

            if (iconTexture != null){
                // Create a new GameObject to hold the SpriteRenderer
                GameObject iconObject = new GameObject("ExtractedIcon");
                SpriteRenderer spriteRenderer = iconObject.AddComponent<SpriteRenderer>();

                // Create a Sprite from the Texture2D
                Sprite iconSprite = Sprite.Create(iconTexture, new Rect(0, 0, iconTexture.width, iconTexture.height), new Vector2(0.5f, 0.5f));
                spriteRenderer.sprite = iconSprite;

                // Position the GameObject in the scene (optional)
                iconObject.transform.position = new Vector3(0, 0, 0);
            }

            // Clean up
            DestroyIcon(largeIconPtr);
        }
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

    // Convert an icon handle to a Texture2D
    private Texture2D IconToTexture2D(IntPtr iconHandle){
        using (Icon icon = Icon.FromHandle(iconHandle))
        using (Bitmap bitmap = new Bitmap(icon.Width, icon.Height, PixelFormat.Format32bppArgb))
        using (System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(bitmap)) {
            g.Clear(System.Drawing.Color.Transparent); // Set the background to transparent
            g.DrawIcon(icon, 0, 0); // Draw the icon

            // Apply mask-based transparency
            ApplyMaskTransparency(bitmap, icon);

            // Convert the Bitmap to a Texture2D
            return BitmapToTexture2D(bitmap);
        }
    }

    // Apply mask-based transparency to the Bitmap
    private void ApplyMaskTransparency(Bitmap bitmap, Icon icon){
        using (Bitmap mask = icon.ToBitmap()){
            for (int y = 0; y < bitmap.Height; y++){
                for (int x = 0; x < bitmap.Width; x++){
                    System.Drawing.Color maskPixel = mask.GetPixel(x, y);

                    // If the mask pixel is black, set the corresponding pixel in the bitmap to transparent
                    if (maskPixel.R == 0 && maskPixel.G == 0 && maskPixel.B == 0){
                        bitmap.SetPixel(x, y, System.Drawing.Color.Transparent);
                    }
                }
            }
        }
    }

    // Convert a Bitmap to a Texture2D
    private Texture2D BitmapToTexture2D(Bitmap bitmap){
        Texture2D texture = new Texture2D(bitmap.Width, bitmap.Height, TextureFormat.RGBA32, false);

        for (int y = 0; y < bitmap.Height; y++){
            for (int x = 0; x < bitmap.Width; x++){
                System.Drawing.Color pixel = bitmap.GetPixel(x, y);
                texture.SetPixel(x, bitmap.Height - 1 - y, new Color32(pixel.R, pixel.G, pixel.B, pixel.A));
            }
        }

        texture.Apply();
        return texture;
    }
}