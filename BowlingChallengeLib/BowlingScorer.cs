using BowlingChallenge.Models;
using System;
using System.Collections.Generic;

namespace BowlingChallenge
{
    public class BowlingScorer
    {
        /// <summary>
        /// Takes a 2D array of rolls and calculates the score
        /// 
        /// </summary>
        /// <param name="frames">2D array of frames -> rolls</param>
        /// <returns>ScoreReturn has the score and then a list of ValidationErrors. If there are any validation errors the score is not valid.</returns>
        public ScoreReturn CalcScore(IList<IList<int>> frames)
        {
            var score = new ScoreReturn() { Errors = new List<ValidationError>() };

            // Grab the last two (so we can calculate spare/strike in one pass)
            var lastTwoRolls = new int[] { 0, 0 };

            // Just give these meaningful names to avoid any future mistakes
            int[] frameVals = null;
            int currFrameScore = 0;

            // Walk backwards
            for(var frameIndex = frames.Count - 1; frameIndex >= 0; frameIndex--)
            {
                frameVals = SafeFrame(frames[frameIndex]);

                // Get any validation errors for this frame.
                score.Errors.AddRange(ValidFrame(frames[frameIndex], frameIndex + 1));

                currFrameScore = frameVals[0] + frameVals[1];

                
                

                if (frameVals[0] == 10) // Strike
                {
                    score.Score += currFrameScore + lastTwoRolls[0] + lastTwoRolls[1];

                    lastTwoRolls[1] = lastTwoRolls[0];
                    lastTwoRolls[0] = frameVals[0];
                }
                else
                {
                    if (frameVals[0] + frameVals[1] == 10) // Spare
                        score.Score += currFrameScore + lastTwoRolls[0];
                    else
                        score.Score += currFrameScore;

                    lastTwoRolls[1] = frameVals[1];
                    lastTwoRolls[0] = frameVals[0];
                }

                // If we're on the last frame we can have a third roll
                // We also have to worry about the last rolls getting primed
                if (frameIndex == 9)
                {
                    score.Score += frameVals[2];

                    lastTwoRolls[1] = frameVals[1];
                    lastTwoRolls[0] = frameVals[0];
                }

            }

            return score;
        }

        /// <summary>
        /// Defensive normalization for smaller than expected frames, allows us to avoid multiple length checks later
        /// </summary>
        /// <param name="frame">The current frame</param>
        /// <returns>Always returns a int[] w/ three values and will return 0 if the roll doesn't exist in the frame</returns>
        protected int[] SafeFrame(IList<int> frame)
        {
            var returnRolls = new int[] { 0,0,0 };

            if (frame.Count > 0)
                returnRolls[0] = frame[0];
            
            if (frame.Count > 1)
                returnRolls[1] = frame[1];

            if (frame.Count > 2)
                returnRolls[2] = frame[2];

            return returnRolls;
        }

        /// <summary>
        /// Builds validation errors to return to caller
        /// </summary>
        /// <param name="frame">Current frame</param>
        /// <param name="frameNum">Frame number, starts at 1</param>
        /// <returns>List of ValidationErrors found</returns>
        protected IList<ValidationError> ValidFrame(IList<int> frame, int frameNum)
        {
            var errors = new List<ValidationError>();

            // Check that we've got a valid Frame number
            if(frameNum < 1 || frameNum > 10)
                errors.Add(new ValidationError() { Message = "Frame num needs to be between 1-10", FrameNum = frameNum, RollNum = 0 });

            // Check we don't have too many rolls
            if (frameNum == 10 && frame.Count > 3)
                errors.Add(new ValidationError() { Message = "Frame 10 cannot have more than 3 rolls", FrameNum = frameNum, RollNum = 4 });
            else if(frameNum != 10 && frame.Count > 2)
                errors.Add(new ValidationError() { Message = $"Frame {frameNum} cannot have more than 2 rolls", FrameNum = frameNum, RollNum = 3 });

            // Check none are greater than 10
            for (var i = 0; i < frame.Count; i++)
            {
                if(frame[i] < 0 || frame[i] > 10)
                    errors.Add(new ValidationError() { Message = "Roll has to be 0-10 inclusive", FrameNum = frameNum, RollNum = i });
            }

            // Check if we have two rolls they aren't >10 combined
            if(frameNum != 10 && frame[0] + (frame.Count > 1 ? frame[1] : 0) > 10)
                errors.Add(new ValidationError() { Message = "First two rolls cannot be over 10 except on 10th frame", FrameNum = frameNum, RollNum = 2 });

            return errors;
        }
    }
}
