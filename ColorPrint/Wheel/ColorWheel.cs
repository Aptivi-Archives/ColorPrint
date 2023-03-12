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

using ColorPrint.Graphics;
using ColorSeq;
using System;

namespace ColorPrint.Wheel
{
    internal static class ColorWheel
    {
        private static double wheelSeverity = 0.60;
        private static ColorType wheelColorMode = ColorType.TrueColor;
        private static int wheelR = 0, wheelG = 128, wheelB = 0;
        private static ConsoleColors wheelColor255 = ConsoleColors.Green;
        private static ConsoleColor wheelColor16 = ConsoleColor.Green;
        private static Color wheelColor = new(wheelR, wheelG, wheelB);
        private static int wheelRgbIndicator = 0; // R = 0, G = 1, B = 2

        internal static void RenderWheel()
        {
            Console.CursorVisible = false;
            Console.Clear();

            // Wait until the wheelColor is populated
            while (wheelColor is null) ;

            // Get the box sizes to fit the console. Four boxes are needed to represent all the modes:
            //   - Normal color
            //   - Protanopia color
            //   - Deuteranopia color
            //   - Tritanopia color
            int topLeftCornerPosLeft = 2;
            int topLeftCornerPosTop = 1;
            int boxWidth = (Console.WindowWidth / 4) - 2;
            int boxHeight = (Console.WindowHeight / 3) - 2;
            int boxNum;

            // Render all the colors based on the current wheel color
            Color wheelColorProtan, wheelColorDeutan, wheelColorTritan;
            ColorTools.EnableColorTransformation = true;
            ColorTools.ColorDeficiencySeverity = wheelSeverity;
            ColorTools.ColorDeficiency = ColorSeq.Accessibility.Deficiency.Protan;
            wheelColorProtan = new Color(wheelColor.PlainSequence);
            ColorTools.ColorDeficiency = ColorSeq.Accessibility.Deficiency.Deutan;
            wheelColorDeutan = new Color(wheelColor.PlainSequence);
            ColorTools.ColorDeficiency = ColorSeq.Accessibility.Deficiency.Tritan;
            wheelColorTritan = new Color(wheelColor.PlainSequence);
            ColorTools.ColorDeficiency = ColorSeq.Accessibility.Deficiency.Protan;
            ColorTools.EnableColorTransformation = false;

            // Render all the boxes now
            for (boxNum = 0; boxNum < 4; boxNum++) 
            {
                int topLeftCornerPosLeftCurrent = ((boxWidth + 2) * boxNum) + topLeftCornerPosLeft;
                Color color = boxNum == 1 ? wheelColorProtan :
                              boxNum == 2 ? wheelColorDeutan :
                              boxNum == 3 ? wheelColorTritan :
                              wheelColor;
                string mode = boxNum == 1 ? $"Protanopia [{wheelSeverity:n2}]" :
                              boxNum == 2 ? $"Deuteranopia [{wheelSeverity:n2}]" :
                              boxNum == 3 ? $"Tritanopia [{wheelSeverity:n2}]" :
                              "Normal";
                Box.MakeBox(topLeftCornerPosLeftCurrent, topLeftCornerPosTop, topLeftCornerPosLeftCurrent + boxWidth - 1, boxHeight, color);
                Text.RenderText(mode, color, topLeftCornerPosLeftCurrent + (boxWidth / 2) - (mode.Length / 2), boxHeight + 2);
            }

            // Write some color information
            Text.RenderText("\n\n", new Color(ConsoleColors.White));
            Text.RenderText("Color RGB sequence: ", new Color(ConsoleColors.White));
            Text.RenderTextLine($"Normal: {wheelColor.PlainSequence} - Protanopia: {wheelColorProtan.PlainSequence} - Deuteranopia: {wheelColorDeutan.PlainSequence} - Tritanopia: {wheelColorTritan.PlainSequence}", new Color(ConsoleColors.Gray));
            Text.RenderText("HTML code:          ", new Color(ConsoleColors.White));
            Text.RenderTextLine($"Normal: {wheelColor.Hex} - Protanopia: {wheelColorProtan.Hex} - Deuteranopia: {wheelColorDeutan.Hex} - Tritanopia: {wheelColorTritan.Hex}", new Color(ConsoleColors.Gray));
            Text.RenderText("Color type:         ", new Color(ConsoleColors.White));
            Text.RenderTextLine($"Normal: {wheelColor.Type} - Protanopia: {wheelColorProtan.Type} - Deuteranopia: {wheelColorDeutan.Type} - Tritanopia: {wheelColorTritan.Type}", new Color(ConsoleColors.Gray));
            Text.RenderText("Bright?             ", new Color(ConsoleColors.White));
            Text.RenderTextLine($"Normal: {wheelColor.IsBright} - Protanopia: {wheelColorProtan.IsBright} - Deuteranopia: {wheelColorDeutan.IsBright} - Tritanopia: {wheelColorTritan.IsBright}", new Color(ConsoleColors.Gray));

            // Write the RGB adjuster
            string adjusterTop = "  ^  ";
            string adjusterBottom = "  v  ";
            int adjusterLength = 5;
            int greenAdjusterPos = (Console.WindowWidth / 2) - ((adjusterLength + 2) / 2) + 1;
            int blueAdjusterPos = greenAdjusterPos + 7;
            int redAdjusterPos = greenAdjusterPos - 7;
            int adjusterTopTop = Console.WindowHeight - 8;
            int adjusterInfoTop = Console.WindowHeight - 6;
            int adjusterBottomTop = Console.WindowHeight - 4;
            if (wheelColorMode == ColorType.TrueColor)
            {
                Color redColor = new(255, 0, 0);
                Color greenColor = new(0, 255, 0);
                Color blueColor = new(0, 0, 255);
                Text.RenderText(adjusterTop,        wheelRgbIndicator == 0 ? Color.Empty : redColor, wheelRgbIndicator == 0 ? redColor : Color.Empty, redAdjusterPos, adjusterTopTop);
                Text.RenderText($"{wheelColor.R}",  redColor, redAdjusterPos + ($"{wheelColor.R}".Length == 1 ? 2 : $"{wheelColor.R}".Length / 2), adjusterInfoTop);
                Text.RenderText(adjusterBottom,     wheelRgbIndicator == 0 ? Color.Empty : redColor, wheelRgbIndicator == 0 ? redColor : Color.Empty, redAdjusterPos, adjusterBottomTop);
                Text.RenderText(adjusterTop,        wheelRgbIndicator == 1 ? Color.Empty : greenColor, wheelRgbIndicator == 1 ? greenColor : Color.Empty, greenAdjusterPos, adjusterTopTop);
                Text.RenderText($"{wheelColor.G}",  greenColor, greenAdjusterPos + ($"{wheelColor.G}".Length == 1 ? 2 : $"{wheelColor.G}".Length / 2), adjusterInfoTop);
                Text.RenderText(adjusterBottom,     wheelRgbIndicator == 1 ? Color.Empty : greenColor, wheelRgbIndicator == 1 ? greenColor : Color.Empty, greenAdjusterPos, adjusterBottomTop);
                Text.RenderText(adjusterTop,        wheelRgbIndicator == 2 ? Color.Empty : blueColor, wheelRgbIndicator == 2 ? blueColor : Color.Empty, blueAdjusterPos, adjusterTopTop);
                Text.RenderText($"{wheelColor.B}",  blueColor, blueAdjusterPos + ($"{wheelColor.B}".Length == 1 ? 2 : $"{wheelColor.B}".Length / 2), adjusterInfoTop);
                Text.RenderText(adjusterBottom,     wheelRgbIndicator == 2 ? Color.Empty : blueColor, wheelRgbIndicator == 2 ? blueColor : Color.Empty, blueAdjusterPos, adjusterBottomTop);
            }
            else if (wheelColorMode == ColorType._255Color)
            {
                Text.RenderText(adjusterTop, Color.Empty, wheelColor, greenAdjusterPos, adjusterTopTop);
                Text.RenderText($"{wheelColor255} [{(int)wheelColor255}]", wheelColor, (Console.WindowWidth / 2) - ($"{wheelColor255} [{(int)wheelColor255}]".Length == 1 ? 2 : $"{wheelColor255} [{(int)wheelColor255}]".Length / 2), adjusterInfoTop);
                Text.RenderText(adjusterBottom, Color.Empty, wheelColor, greenAdjusterPos, adjusterBottomTop);
            }
            else
            {
                Text.RenderText(adjusterTop, Color.Empty, wheelColor, greenAdjusterPos, adjusterTopTop);
                Text.RenderText($"{wheelColor16}  [{(int)wheelColor16}]", wheelColor, (Console.WindowWidth / 2) - ($"{wheelColor16} [{(int)wheelColor16}]".Length == 1 ? 2 : $"{wheelColor16} [{(int)wheelColor16}]".Length / 2), adjusterInfoTop);
                Text.RenderText(adjusterBottom, Color.Empty, wheelColor, greenAdjusterPos, adjusterBottomTop);
            }
            Text.RenderText("\n\n", new Color(ConsoleColors.White));

            // Write the bound keys list
            string keysStr = "[ESC] Exit | [-> | <-] RGB | [CTRL + -> | CTRL + <-] Severity | [TAB] Mode | [UP | DOWN] Color";
            Text.RenderText(keysStr, new Color(ConsoleColors.White), (Console.WindowWidth / 2) - (keysStr.Length / 2), Console.CursorTop);
        }

        internal static void UpdateColor()
        {
            switch (wheelColorMode)
            {
                case ColorType.TrueColor:
                    wheelColor = new Color(wheelR, wheelG, wheelB);
                    break;
                case ColorType._255Color:
                    wheelColor = new Color(wheelColor255);
                    break;
                case ColorType._16Color:
                    wheelColor = new Color((ConsoleColors)wheelColor16);
                    break;
            }
        }

        internal static void HandleUserInput(ConsoleKeyInfo cki)
        {
            switch (cki.Key)
            {
                // Switch color mode
                case ConsoleKey.Tab:
                    if (wheelColorMode == ColorType._16Color)
                        wheelColorMode = ColorType.TrueColor;
                    else
                        wheelColorMode++;
                    break;
                case ConsoleKey.UpArrow:
                    IncrementColor();
                    break;
                case ConsoleKey.DownArrow:
                    DecrementColor();
                    break;
                case ConsoleKey.RightArrow:
                    if (cki.Modifiers.HasFlag(ConsoleModifiers.Control))
                        IncrementSeverity();
                    else
                        IncrementRgbIndicator();
                    break;
                case ConsoleKey.LeftArrow:
                    if (cki.Modifiers.HasFlag(ConsoleModifiers.Control))
                        DecrementSeverity();
                    else
                        DecrementRgbIndicator();
                    break;
            }
        }

        private static void IncrementRgbIndicator()
        {
            wheelRgbIndicator++;
            if (wheelRgbIndicator > 2)
                wheelRgbIndicator = 0;
        }

        private static void DecrementRgbIndicator()
        {
            wheelRgbIndicator--;
            if (wheelRgbIndicator < 0)
                wheelRgbIndicator = 2;
        }

        private static void IncrementSeverity()
        {
            wheelSeverity += 0.01;
            if (wheelSeverity > 1)
                wheelSeverity = 0;
        }

        private static void DecrementSeverity()
        {
            wheelSeverity -= 0.01;
            if (wheelSeverity < 0)
                wheelSeverity = 1;
        }

        private static void IncrementColor()
        {
            switch (wheelColorMode)
            {
                case ColorType.TrueColor:
                    switch (wheelRgbIndicator)
                    {
                        case 0:
                            wheelR++;
                            if (wheelR > 255)
                                wheelR = 0;
                            break;
                        case 1:
                            wheelG++;
                            if (wheelG > 255)
                                wheelG = 0;
                            break;
                        case 2:
                            wheelB++;
                            if (wheelB > 255)
                                wheelB = 0;
                            break;
                    }
                    break;
                case ColorType._255Color:
                    if (wheelColor255 == ConsoleColors.Grey93)
                        wheelColor255 = ConsoleColors.Black;
                    else
                        wheelColor255++;
                    break;
                case ColorType._16Color:
                    if (wheelColor16 == ConsoleColor.White)
                        wheelColor16 = ConsoleColor.Black;
                    else
                        wheelColor16++;
                    break;
            }
        }

        private static void DecrementColor()
        {
            switch (wheelColorMode)
            {
                case ColorType.TrueColor:
                    switch (wheelRgbIndicator)
                    {
                        case 0:
                            wheelR--;
                            if (wheelR < 0)
                                wheelR = 255;
                            break;
                        case 1:
                            wheelG--;
                            if (wheelG < 0)
                                wheelG = 255;
                            break;
                        case 2:
                            wheelB--;
                            if (wheelB < 0)
                                wheelB = 255;
                            break;
                    }
                    break;
                case ColorType._255Color:
                    if (wheelColor255 == ConsoleColors.Black)
                        wheelColor255 = ConsoleColors.Grey93;
                    else
                        wheelColor255--;
                    break;
                case ColorType._16Color:
                    if (wheelColor16 == ConsoleColor.Black)
                        wheelColor16 = ConsoleColor.White;
                    else
                        wheelColor16--;
                    break;
            }
        }
    }
}
