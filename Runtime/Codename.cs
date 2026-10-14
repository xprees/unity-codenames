namespace Xprees.Codenames
{
    /// Turns any seed (e.g. a random player id) into a stable, readable codename like <c>kind-edison</c>.
    /// The same seed always gives the same codename, on every platform. The codename cannot be turned back into the seed.
    public static class Codename
    {
        /// Returned for a null or empty seed.
        public const string Fallback = "anonymous";

        /// How many different codenames exist (collisions are expected for large populations - treat a codename as a label, not as an id).
        public static int CombinationCount => CodenameWords.Adjectives.Length * CodenameWords.Names.Length;

        public static string FromSeed(string seed)
        {
            if (string.IsNullOrEmpty(seed)) return Fallback;

            var hash = Mix(Fnv1A(seed));
            var adjectives = CodenameWords.Adjectives;
            var names = CodenameWords.Names;

            var adjectiveIndex = (int) ((uint) hash % (uint) adjectives.Length);
            var nameIndex = (int) ((uint) (hash >> 32) % (uint) names.Length);

            // Like Docker: Steve Wozniak is not boring.
            if (adjectives[adjectiveIndex] == "boring" && names[nameIndex] == "wozniak")
            {
                adjectiveIndex = (adjectiveIndex + 1) % adjectives.Length;
            }

            return string.Concat(adjectives[adjectiveIndex], "-", names[nameIndex]);
        }

        // FNV-1a 64-bit over the UTF-16 code units (two bytes each, low byte first) - no allocations, stable everywhere.
        private static ulong Fnv1A(string text)
        {
            var hash = 14695981039346656037UL;
            foreach (var c in text)
            {
                hash ^= (byte) (c & 0xFF);
                hash *= 1099511628211UL;
                hash ^= (byte) (c >> 8);
                hash *= 1099511628211UL;
            }

            return hash;
        }

        // splitmix64 finalizer - seeds that differ in a few characters (e.g. uuidv7 prefixes) still spread over all words.
        private static ulong Mix(ulong value)
        {
            value ^= value >> 30;
            value *= 0xBF58476D1CE4E5B9UL;
            value ^= value >> 27;
            value *= 0x94D049BB133111EBUL;
            value ^= value >> 31;
            return value;
        }
    }
}