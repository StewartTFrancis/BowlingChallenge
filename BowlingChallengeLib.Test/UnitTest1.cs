using BowlingChallenge;
using System;
using Xunit;

namespace BowlingChallengeLib.Test
{
    public class ScoringTests
    {
        [Fact]
        public void ExampleTest()
        {
            var scorer = new BowlingScorer();
            var ret = scorer.CalcScore(new int[][] {
                new int[] { 4, 3 },
                new int[] { 7, 3 },
                new int[] { 5, 2 },
                new int[] { 8, 1 },
                new int[] { 4, 6 },
                new int[] { 2, 4 },
                new int[] { 8, 0 },
                new int[] { 8, 0 },
                new int[] { 8, 2 },
                new int[] { 10, 1, 7 }
            });

            Assert.Equal(110, ret.Score);
            Assert.Empty(ret.Errors);
        }

        [Fact]
        public void PerfectGameTest()
        {
            var scorer = new BowlingScorer();
            var ret = scorer.CalcScore(new int[][] {
                new int[] { 10, 0 },
                new int[] { 10, 0 },
                new int[] { 10, 0 },
                new int[] { 10, 0 },
                new int[] { 10, 0 },
                new int[] { 10, 0 },
                new int[] { 10, 0 },
                new int[] { 10, 0 },
                new int[] { 10, 0 },
                new int[] { 10, 10, 10 }
            });

            Assert.Equal(300, ret.Score);
            Assert.Empty(ret.Errors);
        }

        [Fact]
        public void RandomGameTest()
        {
            var scorer = new BowlingScorer();
            var ret = scorer.CalcScore(new int[][] {
                new int[] { 8, 2 },
                new int[] { 5, 4 },
                new int[] { 9, 0 },
                new int[] { 10, 0 },
                new int[] { 10, 0 },
                new int[] { 5, 5 },
                new int[] { 5, 3 },
                new int[] { 6, 3 },
                new int[] { 9, 1 },
                new int[] { 9, 1, 10 }
            });

            Assert.Equal(149, ret.Score);
            Assert.Empty(ret.Errors);
        }

        [Fact]
        public void ImpossibleFrameTest()
        {
            var scorer = new BowlingScorer();
            var ret = scorer.CalcScore(new int[][] {
                new int[] { 4, 7 },
                new int[] { 10, 0 },
                new int[] { 10, 0 },
                new int[] { 10, 0 },
                new int[] { 10, 0 },
                new int[] { 10, 0 },
                new int[] { 10, 0 },
                new int[] { 10, 0 },
                new int[] { 10, 0 },
                new int[] { 10, 10, 10 }
            });

            Assert.Collection(ret.Errors, err => { Assert.Equal(1, err.FrameNum); Assert.Equal("First two rolls cannot be over 10 except on 10th frame", err.Message); });
        }

        [Fact]
        public void ImpossibleGameTest()
        {
            var scorer = new BowlingScorer();
            var ret = scorer.CalcScore(new int[][] {
                new int[] { 4, 0 },
                new int[] { 10, 0 },
                new int[] { 10, 0 },
                new int[] { 10, 0 },
                new int[] { 10, 0 },
                new int[] { 10, 0 },
                new int[] { 10, 0 },
                new int[] { 10, 0 },
                new int[] { 10, 0 },
                new int[] { 10, 10, 10 },
                new int[] { 10, 0 }
            });

            Assert.Collection(ret.Errors, err => { Assert.Equal(11, err.FrameNum); Assert.Equal("Frame num needs to be between 1-10", err.Message); });
        }

        [Fact]
        public void ImpossibleRollTest()
        {
            var scorer = new BowlingScorer();
            var ret = scorer.CalcScore(new int[][] {
                new int[] { 11, 0 }
            });

            Assert.Collection(ret.Errors, err => { Assert.Equal(1, err.FrameNum); Assert.Equal("Roll has to be 0-10 inclusive", err.Message); },
                err => { Assert.Equal(1, err.FrameNum); Assert.Equal("First two rolls cannot be over 10 except on 10th frame", err.Message); }
            );
        }
    }
}
