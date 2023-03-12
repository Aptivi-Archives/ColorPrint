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

namespace ColorPrint.Graphics
{
    internal static class Text
    {
        /// <summary>
        /// Renders the text to the console
        /// </summary>
        /// <param name="text">Text to be rendered</param>
        /// <param name="color">Text color</param>
        /// <param name="vars">Variables to format the text with</param>
        internal static void RenderTextLine(string text, Color color, params object[] vars) =>
            RenderTextLine(text, color, Color.Empty, vars);

        /// <summary>
        /// Renders the text to the console
        /// </summary>
        /// <param name="text">Text to be rendered</param>
        /// <param name="foregroundColor">Text color</param>
        /// <param name="backgroundColor">Text background color</param>
        /// <param name="vars">Variables to format the text with</param>
        internal static void RenderTextLine(string text, Color foregroundColor, Color backgroundColor, params object[] vars) =>
            RenderText($"{text}\n", foregroundColor, backgroundColor, vars);

        /// <summary>
        /// Renders the text to the console
        /// </summary>
        /// <param name="text">Text to be rendered</param>
        /// <param name="color">Text color</param>
        /// <param name="vars">Variables to format the text with</param>
        internal static void RenderText(string text, Color color, params object[] vars) =>
            RenderText(text, color, Color.Empty, vars);

        /// <summary>
        /// Renders the text to the console
        /// </summary>
        /// <param name="text">Text to be rendered</param>
        /// <param name="foregroundColor">Text color</param>
        /// <param name="backgroundColor">Text background color</param>
        /// <param name="vars">Variables to format the text with</param>
        internal static void RenderText(string text, Color foregroundColor, Color backgroundColor, params object[] vars)
        {
            // Store the old cursor position in case the position bug still occurs on Linux systems
            (int, int) cursorPos = Console.GetCursorPosition();
            Console.Write(foregroundColor.VTSequenceForeground);
            Console.Write(backgroundColor.VTSequenceBackground);
            Console.SetCursorPosition(cursorPos.Item1, cursorPos.Item2);

            // Now, write the text
            Console.Write(text, vars);
        }

        /// <summary>
        /// Renders the text to the console
        /// </summary>
        /// <param name="text">Text to be rendered</param>
        /// <param name="color">Text color</param>
        /// <param name="posX">The X position</param>
        /// <param name="posY">The Y position</param>
        /// <param name="vars">Variables to format the text with</param>
        internal static void RenderTextLine(string text, Color color, int posX, int posY, params object[] vars) =>
            RenderTextLine(text, color, Color.Empty, posX, posY, vars);

        /// <summary>
        /// Renders the text to the console
        /// </summary>
        /// <param name="text">Text to be rendered</param>
        /// <param name="foregroundColor">Text color</param>
        /// <param name="backgroundColor">Text background color</param>
        /// <param name="posX">The X position</param>
        /// <param name="posY">The Y position</param>
        /// <param name="vars">Variables to format the text with</param>
        internal static void RenderTextLine(string text, Color foregroundColor, Color backgroundColor, int posX, int posY, params object[] vars) =>
            RenderText($"{text}\n", foregroundColor, backgroundColor, posX, posY, vars);

        /// <summary>
        /// Renders the text to the console
        /// </summary>
        /// <param name="text">Text to be rendered</param>
        /// <param name="color">Text color</param>
        /// <param name="posX">The X position</param>
        /// <param name="posY">The Y position</param>
        /// <param name="vars">Variables to format the text with</param>
        internal static void RenderText(string text, Color color, int posX, int posY, params object[] vars) =>
            RenderText(text, color, Color.Empty, posX, posY, vars);

        /// <summary>
        /// Renders the text to the console
        /// </summary>
        /// <param name="text">Text to be rendered</param>
        /// <param name="foregroundColor">Text color</param>
        /// <param name="backgroundColor">Text background color</param>
        /// <param name="posX">The X position</param>
        /// <param name="posY">The Y position</param>
        /// <param name="vars">Variables to format the text with</param>
        internal static void RenderText(string text, Color foregroundColor, Color backgroundColor, int posX, int posY, params object[] vars)
        {
            // Store the old cursor position in case the position bug still occurs on Linux systems
            (int, int) cursorPos = Console.GetCursorPosition();
            Console.Write(foregroundColor.VTSequenceForeground);
            Console.Write(backgroundColor.VTSequenceBackground);
            Console.SetCursorPosition(cursorPos.Item1, cursorPos.Item2);

            // Now, write the text
            Console.SetCursorPosition(posX, posY);
            Console.Write(text, vars);
        }
    }
}
