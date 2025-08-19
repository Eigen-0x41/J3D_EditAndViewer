using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace GalaxyPlantInCrystal_KEIJI.Util
{
    class HashKey<HasherT> : IEquatable<HashKey<HasherT>>
        where HasherT : HashAlgorithm
    {
        public readonly HasherT Hasher;
        public readonly byte[] Value;

        /// <summary>
        /// ハッシュ値の保持とDictionary用の比較処理の実装。
        /// </summary>
        /// <param name="StrTarget">VertexShaderSource + FragmentShaderSource</param>
        /// <param name="Hasher">SHA3あたりを推奨。使用できない場合はSHA1を推奨。</param>
        public HashKey(string StrTarget, HasherT Hasher)
        {
            this.Hasher = Hasher;
            this.Value = Hasher.ComputeHash(Encoding.UTF8.GetBytes(StrTarget));
        }
        /// <summary>
        /// ハッシュ値の保持とDictionary用の比較処理の実装。
        /// </summary>
        /// <param name="StrTarget">VertexShaderSource + FragmentShaderSource</param>
        /// <param name="Hasher">SHA3あたりを推奨。使用できない場合はSHA1を推奨。</param>
        public HashKey(byte[] Target, HasherT Hasher)
        {
            this.Hasher = Hasher;
            this.Value = Hasher.ComputeHash(Target);
        }

        public bool Equals(HashKey<HasherT>? other)
        {
            if (other == null) return false;
            if (Value.Length != other.Value.Length)
            {
                return false;
            }
            foreach (var i in Enumerable.Range(0, Value.Length))
            {
                if (Value[i] != other.Value[i]) { return false; }
            }
            return true;
        }
    }
}
