using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace Twendanishe.Business
{
    public class HashingService
    {
        public HashingService() { }

        /// <summary>
        /// HAshes the given password according to identity v3
        /// </summary>
        /// <param name="rawText">The text to hash</param>
        /// <returns>Hashed string</returns>
        public string Hash(string rawText)
        {
            using (var rng = RandomNumberGenerator.Create())
            {
                var salt = new byte[128 / 8];
                rng.GetBytes(salt);

                var pbkdf2Hash = KeyDerivation.Pbkdf2(rawText, salt, KeyDerivationPrf.HMACSHA256, 1000, 32);
                var identityV3Hash = new byte[1 + 4/*KeyDerivationPrf value*/ + 4/*Iteration count*/ + 4/*salt size*/ + 16 /*salt*/ + 32 /*password hash size*/];
                identityV3Hash[0] = 1;

                uint prf = (uint)KeyDerivationPrf.HMACSHA256; // or just 1
                byte[] prfAsByteArray = BitConverter.GetBytes(prf).Reverse().ToArray(); //you need System.Linq for this to work
                Buffer.BlockCopy(prfAsByteArray, 0, identityV3Hash, 1, 4);

                byte[] iterationCountAsByteArray = BitConverter.GetBytes((uint)10000).Reverse().ToArray();
                Buffer.BlockCopy(iterationCountAsByteArray, 0, identityV3Hash, 1 + 4, 4);

                byte[] saltSizeInByteArray = BitConverter.GetBytes((uint)16).Reverse().ToArray();
                Buffer.BlockCopy(saltSizeInByteArray, 0, identityV3Hash, 1 + 4 + 4, 4);

                Buffer.BlockCopy(salt, 0, identityV3Hash, 1 + 4 + 4 + 4, salt.Length);

                Buffer.BlockCopy(pbkdf2Hash, 0, identityV3Hash, 1 + 4 + 4 + 4 + salt.Length, pbkdf2Hash.Length);

                return Convert.ToBase64String(identityV3Hash);
            }
        }

        public bool Validate(string hashedText, string plainText)
        {
            var identityV3HashArray = Convert.FromBase64String(hashedText);

            var prfAsArray = new byte[4];
            Buffer.BlockCopy(identityV3HashArray, 1, prfAsArray, 0, 4);
            var prf = (KeyDerivationPrf)ConvertFromNetworOrder(prfAsArray);

            var iterationCountAsArray = new byte[4];
            Buffer.BlockCopy(identityV3HashArray, 5, iterationCountAsArray, 0, 4);
            var iterationCount = (int)ConvertFromNetworOrder(iterationCountAsArray);

            var saltSizeAsArray = new byte[4];
            Buffer.BlockCopy(identityV3HashArray, 9, saltSizeAsArray, 0, 4);
            //int because Buffer.BlockCopy expects an int in the offset parameter
            var saltSize = (int)ConvertFromNetworOrder(saltSizeAsArray);

            var salt = new byte[saltSize];
            Buffer.BlockCopy(identityV3HashArray, 13, salt, 0, saltSize);

            var savedHashedPassword = new byte[identityV3HashArray.Length - 1 - 4 - 4 - 4 - saltSize];
            Buffer.BlockCopy(identityV3HashArray, 13 + saltSize, savedHashedPassword, 0, savedHashedPassword.Length);

            var hashFromInputPassword = KeyDerivation.Pbkdf2(plainText, salt, prf, iterationCount, 32);

            return AreByteArraysEqual(hashFromInputPassword, savedHashedPassword);
        }

        private uint ConvertFromNetworOrder(byte[] reversedUint)
        {
            return BitConverter.ToUInt32(reversedUint.Reverse().ToArray(), 0);
        }

        /// <summary>
        /// Ensures quick responses from password comparisons
        /// </summary>
        /// <param name="array1">The first array to compare</param>
        /// <param name="array2">The second array to compare</param>
        /// <returns>True if compare, False otherwise.</returns>
        private static bool AreByteArraysEqual(byte[] array1, byte[] array2)
        {
            if (array1.Length != array2.Length) return false;

            var areEqual = true;
            for (var i = 0; i < array1.Length; i++)
            {
                areEqual &= (array1[i] == array2[i]);
            }

            return areEqual;
        }
    }
}
