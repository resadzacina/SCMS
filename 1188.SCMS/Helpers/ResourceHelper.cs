using System;
using System.IO;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media.Imaging;
using System.Windows.Resources;

namespace _1188.SCMS.Helpers
{
    public class ResourceHelper
    {
        public static string ExecutingAssemblyName
        {
            get
            {
                string name = System.Reflection.Assembly.GetExecutingAssembly().FullName;
                return name.Substring( 0, name.IndexOf( ',' ) );
            }
        }

        public static Stream GetStream( string relativeUri, string assemblyName )
        {
            StreamResourceInfo res = Application.GetResourceStream( new Uri( assemblyName + ";component/" + relativeUri, UriKind.Relative ) );
            if (res == null)
            {
                res = Application.GetResourceStream( new Uri( relativeUri, UriKind.Relative ) );
            }
            if (res != null)
            {
                return res.Stream;
            }
            else
            {
                return null;
            }
        }

        public static Stream GetStream( string relativeUri )
        {
            return GetStream( relativeUri, ExecutingAssemblyName );
        }

        public static BitmapImage GetBitmap( string relativeUri, string assemblyName )
        {
            Stream s = GetStream( relativeUri, assemblyName );
            if (s == null) return null;
            using (s)
            {
                BitmapImage bmp = new BitmapImage();
                bmp.SetSource( s );
                return bmp;
            }
        }

        public static BitmapImage GetBitmap( string relativeUri )
        {
            return GetBitmap( relativeUri, ExecutingAssemblyName );
        }

        public static string GetString( string relativeUri, string assemblyName )
        {
            Stream s = GetStream( relativeUri, assemblyName );
            if (s == null) return null;
            using (StreamReader reader = new StreamReader( s ))
            {
                return reader.ReadToEnd();
            }
        }

        public static string GetString( string relativeUri )
        {
            return GetString( relativeUri, ExecutingAssemblyName );
        }

        public static FontSource GetFontSource( string relativeUri, string assemblyName )
        {
            Stream s = GetStream( relativeUri, assemblyName );
            if (s == null) return null;
            using (s)
            {
                return new FontSource( s );
            }
        }

        public static FontSource GetFontSource( string relativeUri )
        {
            return GetFontSource( relativeUri, ExecutingAssemblyName );
        }

        public static object GetXamlObject( string relativeUri, string assemblyName )
        {
            string str = GetString( relativeUri, assemblyName );
            if (str == null) return null;
            object obj = System.Windows.Markup.XamlReader.Load( str );
            return obj;
        }

        public static object GetXamlObject( string relativeUri )
        {
            return GetXamlObject( relativeUri, ExecutingAssemblyName );
        }

        public static byte[] GetBytesFromResource( string imagePath )
        {

            Stream s = GetStream( imagePath );
            byte[] buffer = new byte[s.Length];
            s.Read( buffer, 0, buffer.Length );
            return buffer;
        }

        public static BitmapImage GetImageFromByteArray( byte[] rawImageBytes )
        {
            BitmapImage imageSource = null;

            try
            {
                using (var stream = new MemoryStream( rawImageBytes ))
                {
                    stream.Seek( 0, SeekOrigin.Begin );
                    var b = new BitmapImage();
                    b.SetSource( stream );
                    imageSource = b;
                }
            }
            catch (System.Exception ex)
            {
            }

            return imageSource;
        }

        public static BitmapImage ByteArrayToBitmap( byte[] byteArray )
        {
            var stream = new MemoryStream( byteArray );
            var b = new BitmapImage();
            b.SetSource( stream );
            return b;
        }

        public static byte[] ToByteArray( BitmapImage bmpImage )
        {

            var bmp = new WriteableBitmap(bmpImage);

            // Init buffer
            int w = bmp.PixelWidth;
            int h = bmp.PixelHeight;
            int[] p = bmp.Pixels;
            int len = p.Length;
            byte[] result = new byte[4 * w * h];

            // Copy pixels to buffer
            for (int i = 0, j = 0; i < len; i++, j += 4)
            {
                int color = p[i];
                result[j + 0] = (byte)(color >> 24); // A
                result[j + 1] = (byte)(color >> 16); // R
                result[j + 2] = (byte)(color >> 8);  // G
                result[j + 3] = (byte)(color);       // B
            }

            return result;
        }


        //public static WriteableBitmap WriteableBitmapFromByteArray(byte[] buffer)
        //{
        //    // Create a new WriteableBitmap with the size of the original image
        //    var bmp = new WriteableBitmap( 159, 170 );

        //    // Fill WriteableBitmap from byte array (format ARGB)
        //   WriteableBitmap wbmp = bmp.FromByteArray( buffer );

        //    //using (MemoryStream ms = new MemoryStream())
        //    //{
        //    //    wbmp.SaveJpeg( ms, (int)image1.Width, (int)image1.Height, 0, 100 );
        //    //    BitmapImage bmp = new BitmapImage();
        //    //    bmp.SetSource(ms);
        //    //}

        //    return wbmp;


        //}

        //public static byte[] ByteArrayFromWritableBitmap(UIElement uiElement)
        //{
        //    // Render UIElement into WriteableBitmap
        //    WriteableBitmap bmp = new WriteableBitmap( uiElement, null );

        //    // Copy WriteableBitmap.Pixels into byte array (format ARGB)
        //    byte[] buffer = bmp.ToByteArray();

        //    return buffer;
        //}

    }
}
