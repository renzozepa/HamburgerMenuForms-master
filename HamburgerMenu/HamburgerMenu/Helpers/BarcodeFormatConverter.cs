using System;

using ZXing;
using System.Collections.Generic;
using System.Text;

namespace HamburgerMenu.Helpers
{
    public class BarcodeFormatConverter
    {
        public static string ConvertEnumToString(Enum eEnum) => Enum.GetName(eEnum.GetType(), eEnum);
        public static BarcodeFormat ConvertStringToEnum(string value) => (BarcodeFormat)Enum.Parse(typeof(BarcodeFormat), value);
    }
}
