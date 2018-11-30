using Assets.LFramework.ConstDefine;

namespace Assets.LFramework.Utility
{
    public class EncrytUtil
    {
        //加密
        public static byte[] Encryption(byte[] data)
        {
            int length = data.Length;
            int strCount = 0;
            string pstr = AppConst.SECRET_KEY;
            for (int i = 0; i < length; ++i)
            {
                if (strCount >= pstr.Length)
                    strCount = 0;
                data[i] ^= (byte)pstr[strCount++];
            }
            return data;
        }

        //解密
        public static byte[] Decryption(byte[] data)
        {
            return Encryption(data);
        }
    }
}