﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GUI.App.Source.InputSubsystem
{
    /// <summary>
    /// Represents a set of methods to calculating and displaying FPS (frames per second).
    /// </summary>
    internal class FPSCounter
    {
        /// <summary>
        /// Gets the FPS (frames per second).
        /// </summary>
        public int FPS { get; private set; }

        /// <summary>
        /// Represents the position of FPS text.
        /// </summary>
        public Vector2 FPSPosition { get; set; }

        /// <summary>
        /// Represents the color of FPS text.
        /// </summary>
        public Color FPSColor { get; set; }

        private int _currentSecond;
        private int _currentFPS;
        private bool _displayFPS;

        private SpriteFont _fpsFont;

        /// <summary>
        /// Initializes a new instance of the <see cref="FPSCounter"/> class.
        /// </summary>
        public FPSCounter()
        {
            FPS = 0;

            FPSPosition = new Vector2(5, 5);
            FPSColor = Color.DarkGreen;

            _currentSecond = DateTime.Now.Second;
            _currentFPS = 0;
            _displayFPS = false;
        }

        /// <summary>
        /// Loads resources. Must be called before first use.
        /// </summary>
        /// <param name="contentManager">Monogame content manager.</param>
        public void LoadContent(ContentManager contentManager)
        {
            _fpsFont = contentManager.Load<SpriteFont>("Fonts\\FPSFont");
        }

        /// <summary>
        /// Processes all logic related to the FPS counter.
        /// </summary>
        public void Logic()
        {
            var second = DateTime.Now.Second;

            if (_currentSecond != second)
            {
                FPS = _currentFPS;
                _currentFPS = 0;
            }

            _currentSecond = second;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inputManager"></param>
        public void Input(InputManager inputManager)
        {
            if (inputManager.IsKeyJustPressed(Keys.Tab))
            {
                _displayFPS = !_displayFPS;
            }
        }

        public void AddFrame()
        {
            _currentFPS++;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (_displayFPS)
            {
                var text = "FPS: " + FPS;

                spriteBatch.DrawString(_fpsFont, text, FPSPosition, FPSColor);
            }
        }
    }
}
