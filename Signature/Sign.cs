using System;
using System.Collections.Generic;
using System.Text;

namespace Signature
{
    static class Sign
    {
        private static readonly int _signLength = 4; // in bytes
        public static ulong SignValue;
        public static ulong RealSingValue;

        private static bool CheckOnCorrect(ulong p, ulong q, ulong d, ulong e)
        {
            if (!(FastMath.Ferma(p) && FastMath.Ferma(q)))
                return false;
            
            ulong fr = (p - 1) * (q - 1);

            if (e <= 1 || e >= fr)
                return false;

            if (FastMath.Gcd(e, fr) != 1)
                return false;

            if ((d * e) % fr != 1)
                return false;

            return true;
        }

        private static List<byte> SignToByte(long number)
        {
            var stringBuilder = new StringBuilder(Convert.ToString(number, 2));

            while (stringBuilder.Length % 8 != 0)
                stringBuilder.Insert(0, "0");

            var bytes = new List<byte>();

            string bits = stringBuilder.ToString();

            for (int i = 0; i < stringBuilder.Length; i += 8)
            {
                bytes.Add(Convert.ToByte(bits.Substring(i, 8), 2));
            }

            while (bytes.Count != _signLength)
                bytes.Insert(0, 0);

            return bytes;
        }

        private static ulong ByteToSign(List<byte> signBytes) 
        {
            var stringBuilder = new StringBuilder();
            foreach (var item in signBytes)
                stringBuilder.Append(Convert.ToString(item, 2).PadLeft(8, '0'));

            string bits = stringBuilder.ToString();

            return Convert.ToUInt64(bits, 2);
        }

        private static ulong HashFunction(byte[] initFile, ulong H, ulong r)
        {
            ulong sign = H;

            foreach (var item in initFile)
                sign = ((sign + item) * (sign + item)) % r;

            return sign;
        }

        public static bool Subscribe(string loadPath, string savePath, ulong p, ulong q, ulong d, ulong e, ulong H = 100)
        {
            if (string.IsNullOrEmpty(savePath) || string.IsNullOrEmpty(loadPath))
                return false;

            if (!CheckOnCorrect(p, q, d, e))
                return false;

            var fileToSubscribe = System.IO.File.ReadAllBytes(loadPath);

            ulong r = p * q;
            ulong sign = HashFunction(fileToSubscribe, H, r);

            sign = FastMath.FastPow(sign, d, r);

            var subscribedFile = new List<byte>();
            subscribedFile.AddRange(fileToSubscribe);
            subscribedFile.AddRange(SignToByte((long)sign));

            System.IO.File.WriteAllBytes(savePath, subscribedFile.ToArray());

            SignValue = sign;

            return true;
        }

        public static bool CheckSign(string loadPath, ulong e, ulong r, ulong H = 100)
        {
            if (string.IsNullOrEmpty(loadPath))
                return false;

            var fileToCheck = new List<byte>(System.IO.File.ReadAllBytes(loadPath));
            var signBytes = new List<byte>();

            for (int i = _signLength; i > 0; i--)
            {
                signBytes.Add(fileToCheck[fileToCheck.Count - i]);
                fileToCheck.RemoveAt(fileToCheck.Count - i);
            }

            ulong realSign = ByteToSign(signBytes);
            realSign = FastMath.FastPow(realSign, e, r);
            ulong hashSign = HashFunction(fileToCheck.ToArray(), H, r);

            RealSingValue = realSign;
            SignValue = hashSign;

            if (realSign == hashSign)
                return true;
            return false;
        }
    }
}
