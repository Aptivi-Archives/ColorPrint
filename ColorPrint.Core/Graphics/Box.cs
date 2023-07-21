/*
 * MIT License
 * 
 * Copyright (c) 2023 Aptivi
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 */

using ColorSeq;
using System;
using System.Linq;

namespace ColorPrint.Core.Graphics
{
    internal static class Box
    {
        /// <summary>
        /// Renders a box to the console
        /// </summary>
        /// <param name="topLeftCornerPosLeft">X-position of the top left corner of the box (a.k.a. start x position)</param>
        /// <param name="topLeftCornerPosTop">Y-position of the top left corner of the box (a.k.a. start y position)</param>
        /// <param name="bottomRightCornerPosLeft">X-position of the bottom right corner of the box (a.k.a. end x position)</param>
        /// <param name="bottomRightCornerPosTop">Y-position of the bottom right corner of the box (a.k.a. end y position)</param>
        /// <param name="boxColor">Box color</param>
        internal static void MakeBox(int topLeftCornerPosLeft, int topLeftCornerPosTop,
                                     int bottomRightCornerPosLeft, int bottomRightCornerPosTop,
                                     Color boxColor)
        {
            // Check to see if the end positions are greater than the start
            if (bottomRightCornerPosLeft < topLeftCornerPosLeft)
                (topLeftCornerPosLeft, bottomRightCornerPosLeft) = (bottomRightCornerPosLeft, topLeftCornerPosLeft);
            if (bottomRightCornerPosTop < topLeftCornerPosTop)
                (topLeftCornerPosTop, bottomRightCornerPosTop) = (bottomRightCornerPosTop, topLeftCornerPosTop);

            // Get how many spaces to repeat (for accelerated rendering)
            int startEndDifference = bottomRightCornerPosLeft - topLeftCornerPosLeft;
            string spaces = new(' ', startEndDifference);

            // Actually render the box
            for (int top = topLeftCornerPosTop; top <= bottomRightCornerPosTop; top++)
                Text.RenderText(spaces, boxColor, boxColor, topLeftCornerPosLeft, top);
        }

        /// <summary>
        /// Renders a box border to the console
        /// </summary>
        /// <param name="topLeftCornerPosLeft">X-position of the top left corner of the box (a.k.a. start x position)</param>
        /// <param name="topLeftCornerPosTop">Y-position of the top left corner of the box (a.k.a. start y position)</param>
        /// <param name="bottomRightCornerPosLeft">X-position of the bottom right corner of the box (a.k.a. end x position)</param>
        /// <param name="bottomRightCornerPosTop">Y-position of the bottom right corner of the box (a.k.a. end y position)</param>
        /// <param name="boxColor">Box color</param>
        /// <param name="boxBorderColor">Box border color</param>
        internal static void MakeBoxBorder(int topLeftCornerPosLeft, int topLeftCornerPosTop,
                                           int bottomRightCornerPosLeft, int bottomRightCornerPosTop,
                                           Color boxColor, Color boxBorderColor)
        {
            // Check to see if the end positions are greater than the start
            if (bottomRightCornerPosLeft < topLeftCornerPosLeft)
                (topLeftCornerPosLeft, bottomRightCornerPosLeft) = (bottomRightCornerPosLeft, topLeftCornerPosLeft);
            if (bottomRightCornerPosTop < topLeftCornerPosTop)
                (topLeftCornerPosTop, bottomRightCornerPosTop) = (bottomRightCornerPosTop, topLeftCornerPosTop);

            // Get how many border characters to repeat (for accelerated rendering)
            int startEndDifference = bottomRightCornerPosLeft - topLeftCornerPosLeft;
            string borderChars = new('═', startEndDifference - 1);

            // Actually render the box border
            Text.RenderText('╔' + borderChars + '╗', boxBorderColor, boxColor, topLeftCornerPosLeft, topLeftCornerPosTop);
            Text.RenderText('╚' + borderChars + '╝', boxBorderColor, boxColor, topLeftCornerPosLeft, bottomRightCornerPosTop);
            for (int top = topLeftCornerPosTop + 1; top < bottomRightCornerPosTop; top++)
            {
                Text.RenderText("║", boxBorderColor, boxColor, topLeftCornerPosLeft, top);
                Text.RenderText("║", boxBorderColor, boxColor, bottomRightCornerPosLeft, top);
            }
        }

        /// <summary>
        /// Renders an infobox to the console
        /// </summary>
        /// <param name="boxColor">Box color</param>
        /// <param name="boxTextColor">Box text color</param>
        /// <param name="text">Text to be rendered</param>
        /// <param name="vars">Variables to format the text with</param>
        internal static void MakeInfoBox(Color boxColor, Color boxTextColor,
                                         string text, params object[] vars)
        {
            // Determine the box size according to the console size and the length of text
            string finalInfoRendered = string.Format(text, vars);
            string[] splitLines = finalInfoRendered.ToString().SplitNewLines();
            int maxWidth = splitLines.Max((str) => str.Length);
            if (maxWidth >= Console.WindowWidth)
                maxWidth = Console.WindowWidth - 4;
            int maxHeight = splitLines.Length;
            if (maxHeight >= Console.WindowHeight)
                maxHeight = Console.WindowHeight - 4;
            int maxRenderWidth = Console.WindowWidth - 6;
            int borderX = (Console.WindowWidth / 2) - (maxWidth / 2);
            int borderY = (Console.WindowHeight / 2) - (maxHeight / 2);

            // Make a box
            MakeBoxBorder(borderX, borderY, Console.WindowWidth - (maxWidth / 2), Console.WindowHeight - (maxHeight / 2), boxColor, boxTextColor);
            MakeBox(borderX + 1, borderY + 1, Console.WindowWidth - (maxWidth / 2), Console.WindowHeight - (maxHeight / 2) - 1, boxColor);

            // Render text inside the box
            for (int i = 0; i < splitLines.Length; i++)
            {
                var line = splitLines[i];
                Text.RenderText(line.Truncate(maxRenderWidth), boxTextColor, boxColor, borderX + 1, borderY + 1 + i);
                if (i % maxHeight == 0 && i > 0)
                    Console.ReadKey(true);
            }

            // Wait until the user presses any key to close the box
            Text.RenderText("", new Color(ConsoleColors.White));
            Console.ReadKey(true);
        }

        private static string[] SplitNewLines(this string target) =>
            target.Replace(Convert.ToChar(13).ToString(), "")
               .Split(Convert.ToChar(10));

        private static string Truncate(this string target, int threshold)
        {
            if (target is null)
                throw new ArgumentNullException(nameof(target));

            // Try to truncate string. If the string length is bigger than the threshold, it'll be truncated to the length of
            // the threshold, putting three dots next to it. We don't use ellipsis marks here because we're dealing with the
            // terminal, and some terminals and some monospace fonts may not support that character, so we mimick it by putting
            // the three dots.
            if (target.Length > threshold)
                return target.Substring(0, threshold - 1) + "...";
            else
                return target;
        }
    }
}
