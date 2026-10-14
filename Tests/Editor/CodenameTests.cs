using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using NUnit.Framework;

namespace Xprees.Codenames.Tests
{
    public class CodenameTests
    {
        [TestCase(null)]
        [TestCase("")]
        public void EmptySeed_ReturnsFallback(string seed) => Assert.AreEqual(Codename.Fallback, Codename.FromSeed(seed));

        [Test]
        public void SameSeed_GivesSameCodename() => Assert.AreEqual(Codename.FromSeed("player-1"), Codename.FromSeed("player-1"));

        // Pins the algorithm: changing the hash or the word lists renames every player already stored on a server.
        [TestCase("player-test", "zen-khayyam")]
        [TestCase("settings-test", "gallant-mestorf")]
        [TestCase("a", "agitated-ganguly")]
        public void KnownSeeds_GiveStableCodenames(string seed, string expected) => Assert.AreEqual(expected, Codename.FromSeed(seed));

        [Test]
        public void Codename_IsLowercaseAdjectiveDashName()
        {
            var format = new Regex("^[a-z]+-[a-z]+$");
            for (var i = 0; i < 500; i++)
            {
                var codename = Codename.FromSeed(Guid.NewGuid().ToString());
                Assert.IsTrue(format.IsMatch(codename), codename);
            }
        }

        [Test]
        public void RandomIds_SpreadOverMostOfTheCombinations()
        {
            const int count = 3000;
            var seen = new HashSet<string>();
            for (var i = 0; i < count; i++) seen.Add(Codename.FromSeed(Guid.NewGuid().ToString()));

            // ~2830 distinct expected (birthday collisions in ~25.7k combinations); a broken hash would collapse far below this.
            Assert.Greater(seen.Count, 2500);
        }

        [Test]
        public void SeedsDifferingInLastCharacter_GiveDifferentCodenames()
        {
            var seen = new HashSet<string>();
            for (var c = 'a'; c <= 'z'; c++) seen.Add(Codename.FromSeed("0190-same-prefix-" + c));

            Assert.Greater(seen.Count, 20);
        }

        [Test]
        public void BoringWozniak_NeverHappens()
        {
            for (var i = 0; i < 20000; i++) Assert.AreNotEqual("boring-wozniak", Codename.FromSeed("seed-" + i));
        }
    }
}