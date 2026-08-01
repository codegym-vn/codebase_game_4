// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class GooglePlayTangle
    {
        private static byte[] data = System.Convert.FromBase64String("eQxzPGUs6L7fqvMnHpSqxfjLfhmgjEpAlMRalJmckPozHQpj0PMn7HpCLTScafxggQ5kzyYE6j5UQ2s98UPA4/HMx8jrR4lHNszAwMDEwcK3q9a3TbUR87lx+8nk1OAdYJrtQHJiIEaVNT60U1tT0lyYVr4QOGXwVIj4WjHQ3wAkqtALWSo0xCOQJemeDqpeov3faMnea81JYO/VjuIGMbRhBFvaNW8oLTDwNrqYl9CGjrug9R6AZfgux4APWGV+5IdZZmtY5pdI0/IEXljGTahGW8i48YbsXfkXKr6AGHqdESUaZ+d6WY6dLW2sr6tLiL2Wu5WD655UkAuQJeFnEt2wc3hDwM7B8UPAy8NDwMDBXCXAdqbOTordWqMmYV0PYMPCwMHA");
        private static int[] order = new int[] { 7,10,8,7,8,12,11,13,8,13,11,13,12,13,14 };
        private static int key = 193;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}
