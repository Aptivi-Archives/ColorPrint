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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ColorPrint.Graphics
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
            {
                Console.SetCursorPosition(topLeftCornerPosLeft, top);
                Text.RenderText(spaces, boxColor, boxColor);
            }
        }
    }
}
