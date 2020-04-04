using System;
using System.Windows.Media.Imaging;
using System.Collections.Generic;
using System.Text;

namespace Uno
{
    public static class Utilities
    {
        /// <summary>
        /// takes a passed name and turns it into a URI in the resources. 
        /// </summary>
        /// <param name="resouceNane"></param>
        /// <returns></returns>
        
        
        public static Uri GetResourceUri(string resouceNane)
        {
            Uri resoureUri = new Uri("pack://application:,,,/Resources/" + resouceNane + ".png", UriKind.RelativeOrAbsolute);
            return resoureUri;
        }


        public static BitmapImage GetPlayerCardImage (string pCardName)
        {
            Uri uri = GetResourceUri(pCardName);
            BitmapImage bitmapImage = new BitmapImage(uri);
            return bitmapImage;
        }
    }
}
