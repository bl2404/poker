using Microsoft.VisualStudio.TestTools.UnitTesting;
using webpoker.Models;
using webpoker.Enums;

namespace webpoker_UnitTests
{
    [TestClass]
    public class HandCheckerTests
    {
        [TestMethod]
        public void CheckForSinglePairTrue()
        {
            Card _flop1 = new Card(Figures.Clubs, Numbers.Nine);
            Card _flop2 = new Card(Figures.Diamonds, Numbers.Eight);
            Card _flop3 = new Card(Figures.Clubs, Numbers.J);
            Card _turn = new Card(Figures.Heart, Numbers.J);
            Card _river = new Card(Figures.Spades, Numbers.A);
            Card hand1 = new Card(Figures.Heart, Numbers.Six);
            Card hand2 = new Card(Figures.Diamonds, Numbers.Five);

            var handChecker = new HandChecker(_flop1, _flop2, _flop3, _turn, _river, hand1, hand2);
            var actual = handChecker.CheckForOnePair();

            Assert.AreEqual(true, actual);
        }

        [TestMethod]
        public void CheckForSinglePairFalse()
        {
            Card _flop1 = new Card(Figures.Clubs, Numbers.Nine);
            Card _flop2 = new Card(Figures.Diamonds, Numbers.Eight);
            Card _flop3 = new Card(Figures.Clubs, Numbers.J);
            Card _turn = new Card(Figures.Heart, Numbers.K);
            Card _river = new Card(Figures.Spades, Numbers.A);
            Card hand1 = new Card(Figures.Heart, Numbers.Six);
            Card hand2 = new Card(Figures.Diamonds, Numbers.Five);

            var handChecker = new HandChecker(_flop1, _flop2, _flop3, _turn, _river, hand1, hand2);
            var actual = handChecker.CheckForOnePair();

            Assert.AreEqual(false, actual);
        }

        [TestMethod]
        public void CheckForTwoPairsTrue()
        {
            Card _flop1 = new Card(Figures.Clubs, Numbers.Eight);
            Card _flop2 = new Card(Figures.Diamonds, Numbers.Eight);
            Card _flop3 = new Card(Figures.Clubs, Numbers.J);
            Card _turn = new Card(Figures.Heart, Numbers.J);
            Card _river = new Card(Figures.Spades, Numbers.A);
            Card hand1 = new Card(Figures.Heart, Numbers.Six);
            Card hand2 = new Card(Figures.Diamonds, Numbers.Five);

            var handChecker = new HandChecker(_flop1, _flop2, _flop3, _turn, _river, hand1, hand2);
            var actual = handChecker.CheckForTwoPairs();

            Assert.AreEqual(true, actual);
        }

        [TestMethod]
        public void CheckForTwoPairsFalse()
        {
            Card _flop1 = new Card(Figures.Clubs, Numbers.Nine);
            Card _flop2 = new Card(Figures.Diamonds, Numbers.Eight);
            Card _flop3 = new Card(Figures.Clubs, Numbers.J);
            Card _turn = new Card(Figures.Heart, Numbers.J);
            Card _river = new Card(Figures.Spades, Numbers.A);
            Card hand1 = new Card(Figures.Heart, Numbers.Six);
            Card hand2 = new Card(Figures.Diamonds, Numbers.Five);

            var handChecker = new HandChecker(_flop1, _flop2, _flop3, _turn, _river, hand1, hand2);
            var actual = handChecker.CheckForTwoPairs();

            Assert.AreEqual(false, actual);
        }

        [TestMethod]
        public void CheckForStraightTrue()
        {
            Card _flop1 = new Card(Figures.Clubs, Numbers.Nine);
            Card _flop2 = new Card(Figures.Diamonds, Numbers.Eight);
            Card _flop3 = new Card(Figures.Clubs, Numbers.J);
            Card _turn = new Card(Figures.Heart, Numbers.Seven);
            Card _river = new Card(Figures.Spades, Numbers.A);
            Card hand1 = new Card(Figures.Heart, Numbers.Six);
            Card  hand2 = new Card(Figures.Diamonds, Numbers.Five);

            var handChecker = new HandChecker(_flop1, _flop2, _flop3, _turn, _river, hand1, hand2);


            Assert.AreEqual(Hand.Straight, handChecker.Hand);
            Assert.AreEqual(Numbers.Nine, handChecker.HighCard.Number);
        }

        [TestMethod]
        public void CheckForStraightFalse()
        {
            Card _flop1 = new Card(Figures.Clubs, Numbers.Nine);
            Card _flop2 = new Card(Figures.Diamonds, Numbers.Eight);
            Card _flop3 = new Card(Figures.Clubs, Numbers.J);
            Card _turn = new Card(Figures.Heart, Numbers.K);
            Card _river = new Card(Figures.Spades, Numbers.A);
            Card hand1 = new Card(Figures.Heart, Numbers.Six);
            Card hand2 = new Card(Figures.Diamonds, Numbers.Five);

            var handChecker = new HandChecker(_flop1, _flop2, _flop3, _turn, _river, hand1, hand2);

            Assert.AreNotEqual(Hand.Straight, handChecker.Hand);
        }

        [TestMethod]
        public void CheckForThreeTrue()
        {
            Card _flop1 = new Card(Figures.Clubs, Numbers.Nine);
            Card _flop2 = new Card(Figures.Diamonds, Numbers.Eight);
            Card _flop3 = new Card(Figures.Clubs, Numbers.J);
            Card _turn = new Card(Figures.Heart, Numbers.J);
            Card _river = new Card(Figures.Spades, Numbers.A);
            Card hand1 = new Card(Figures.Heart, Numbers.Six);
            Card hand2 = new Card(Figures.Diamonds, Numbers.J);

            var handChecker = new HandChecker(_flop1, _flop2, _flop3, _turn, _river, hand1, hand2);
            var actual = handChecker.CheckForThree();

            Assert.AreEqual(true, actual);
        }

        [TestMethod]
        public void CheckForThreeFalse()
        {
            Card _flop1 = new Card(Figures.Clubs, Numbers.Nine);
            Card _flop2 = new Card(Figures.Diamonds, Numbers.Eight);
            Card _flop3 = new Card(Figures.Clubs, Numbers.J);
            Card _turn = new Card(Figures.Heart, Numbers.J);
            Card _river = new Card(Figures.Spades, Numbers.A);
            Card hand1 = new Card(Figures.Heart, Numbers.Six);
            Card hand2 = new Card(Figures.Diamonds, Numbers.Five);

            var handChecker = new HandChecker(_flop1, _flop2, _flop3, _turn, _river, hand1, hand2);
            var actual = handChecker.CheckForThree();

            Assert.AreEqual(false, actual);
        }

        [TestMethod]
        public void CheckForFourTrue()
        {
            Card _flop1 = new Card(Figures.Clubs, Numbers.Nine);
            Card _flop2 = new Card(Figures.Diamonds, Numbers.Eight);
            Card _flop3 = new Card(Figures.Clubs, Numbers.J);
            Card _turn = new Card(Figures.Heart, Numbers.J);
            Card _river = new Card(Figures.Spades, Numbers.J);
            Card hand1 = new Card(Figures.Heart, Numbers.Six);
            Card hand2 = new Card(Figures.Diamonds, Numbers.J);

            var handChecker = new HandChecker(_flop1, _flop2, _flop3, _turn, _river, hand1, hand2);
            var actual = handChecker.CheckForFour();

            Assert.AreEqual(true, actual);
        }

        [TestMethod]
        public void CheckForFourFalse()
        {
            Card _flop1 = new Card(Figures.Clubs, Numbers.Nine);
            Card _flop2 = new Card(Figures.Diamonds, Numbers.Eight);
            Card _flop3 = new Card(Figures.Clubs, Numbers.J);
            Card _turn = new Card(Figures.Heart, Numbers.J);
            Card _river = new Card(Figures.Spades, Numbers.J);
            Card hand1 = new Card(Figures.Heart, Numbers.Six);
            Card hand2 = new Card(Figures.Diamonds, Numbers.Five);

            var handChecker = new HandChecker(_flop1, _flop2, _flop3, _turn, _river, hand1, hand2);
            var actual = handChecker.CheckForFour();

            Assert.AreEqual(false, actual);
        }

        [TestMethod]
        public void CheckForColorTrue()
        {
            Card _flop1 = new Card(Figures.Diamonds, Numbers.Nine);
            Card _flop2 = new Card(Figures.Diamonds, Numbers.Eight);
            Card _flop3 = new Card(Figures.Clubs, Numbers.J);
            Card _turn = new Card(Figures.Diamonds, Numbers.J);
            Card _river = new Card(Figures.Spades, Numbers.J);
            Card hand1 = new Card(Figures.Diamonds, Numbers.Six);
            Card hand2 = new Card(Figures.Diamonds, Numbers.Five);

            var handChecker = new HandChecker(_flop1, _flop2, _flop3, _turn, _river, hand1, hand2);
            var actual = handChecker.CheckForColor();

            Assert.AreEqual(true, actual);
        }


        [TestMethod]
        public void CheckForColorFalse()
        {
            Card _flop1 = new Card(Figures.Diamonds, Numbers.Nine);
            Card _flop2 = new Card(Figures.Diamonds, Numbers.Eight);
            Card _flop3 = new Card(Figures.Clubs, Numbers.J);
            Card _turn = new Card(Figures.Heart, Numbers.K);
            Card _river = new Card(Figures.Spades, Numbers.A);
            Card hand1 = new Card(Figures.Diamonds, Numbers.Six);
            Card hand2 = new Card(Figures.Diamonds, Numbers.Five);

            var handChecker = new HandChecker(_flop1, _flop2, _flop3, _turn, _river, hand1, hand2);
            var actual = handChecker.CheckForColor();

            Assert.AreEqual(false, actual);
        }

        [TestMethod]
        public void CheckForFullTrue()
        {
            Card _flop1 = new Card(Figures.Diamonds, Numbers.Nine);
            Card _flop2 = new Card(Figures.Diamonds, Numbers.Nine);
            Card _flop3 = new Card(Figures.Clubs, Numbers.A);
            Card _turn = new Card(Figures.Heart, Numbers.J);
            Card _river = new Card(Figures.Spades, Numbers.K);
            Card hand1 = new Card(Figures.Diamonds, Numbers.Nine);
            Card hand2 = new Card(Figures.Diamonds, Numbers.A);

            var handChecker = new HandChecker(_flop1, _flop2, _flop3, _turn, _river, hand1, hand2);
            var actual = handChecker.CheckForFull();

            Assert.AreEqual(true, actual);
        }


        [TestMethod]
        public void CheckForFullFalse()
        {
            Card _flop1 = new Card(Figures.Diamonds, Numbers.Nine);
            Card _flop2 = new Card(Figures.Spades, Numbers.Nine);
            Card _flop3 = new Card(Figures.Clubs, Numbers.A);
            Card _turn = new Card(Figures.Heart, Numbers.J);
            Card _river = new Card(Figures.Spades, Numbers.K);
            Card hand1 = new Card(Figures.Heart, Numbers.Nine);
            Card hand2 = new Card(Figures.Diamonds, Numbers.Nine);

            var handChecker = new HandChecker(_flop1, _flop2, _flop3, _turn, _river, hand1, hand2);
            var actual = handChecker.CheckForFull();

            Assert.AreEqual(false, actual);
        }

        [TestMethod]
        public void CheckForPokerTrue()
        {
            Card _flop1 = new Card(Figures.Spades, Numbers.Two);
            Card _flop2 = new Card(Figures.Spades, Numbers.Six);
            Card _flop3 = new Card(Figures.Diamonds, Numbers.Ten);
            Card _turn = new Card(Figures.Diamonds, Numbers.J);
            Card _river = new Card(Figures.Diamonds, Numbers.Q);
            Card hand1 = new Card(Figures.Diamonds, Numbers.K);
            Card hand2 = new Card(Figures.Diamonds, Numbers.A);

            var handChecker = new HandChecker(_flop1, _flop2, _flop3, _turn, _river, hand1, hand2);
            var actual = handChecker.CheckForPoker();

            Assert.AreEqual(true, actual);
        }


        [TestMethod]
        public void CheckForPokerFalse()
        {
            Card _flop1 = new Card(Figures.Diamonds, Numbers.Nine);
            Card _flop2 = new Card(Figures.Spades, Numbers.Nine);
            Card _flop3 = new Card(Figures.Clubs, Numbers.A);
            Card _turn = new Card(Figures.Heart, Numbers.J);
            Card _river = new Card(Figures.Spades, Numbers.K);
            Card hand1 = new Card(Figures.Heart, Numbers.Nine);
            Card hand2 = new Card(Figures.Diamonds, Numbers.Nine);

            var handChecker = new HandChecker(_flop1, _flop2, _flop3, _turn, _river, hand1, hand2);
            var actual = handChecker.CheckForPoker();

            Assert.AreEqual(false, actual);
        }
    }
}
