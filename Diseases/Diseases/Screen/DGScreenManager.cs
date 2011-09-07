﻿using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;


using Diseases.Input;

namespace Diseases.Screen
{
    public class DGScreenManager : DrawableGameComponent
    {
        bool                        inputhandled    = false;
        bool                        isinitialized   = false;

        DGInputSequence             crashinput;

        SpriteBatch                 spritebatch;

        ContentManager              content;
        public ContentManager       Content
        {
            get { return this.content; }
        }

        DGInput                     input;
        public DGInput              Input
        {
            get { return this.input; }
        }

        List<DGScreen>              screens         = new List<DGScreen>();
        public List<DGScreen>       Screens
        {
            get { return this.screens; }
        }

        List<DGScreen>              tempscreens     = new List<DGScreen>();


        public                      DGScreenManager (DiseasesGame game)
            : base(game)
        {

        }

        public      override void   Initialize      ()
        {
            this.content = new ContentManager(this.Game.Services);
            this.content.RootDirectory = "content/assets";

            this.crashinput = new DGInputSequence(new Keys[] { Keys.LeftControl, Keys.F12 }, false, true);
            this.crashinput.inputname = "crashinput";

            base.Initialize();

            this.isinitialized = true;
        }

        protected   override void   LoadContent     ()
        {
            this.input = new DGInput();
            this.spritebatch = new SpriteBatch(this.Game.GraphicsDevice);

            foreach (DGScreen screen in this.screens)
                screen.LoadContent();

            base.LoadContent();
        }
        protected   override void   UnloadContent   ()
        {
            foreach (DGScreen screen in this.screens)
                screen.UnloadContent();

            base.UnloadContent();
        }

        public      override void   Update          (GameTime gameTime)
        {
            this.input.Update(gameTime);

            if (this.crashinput.Evaluate(this.input))
                throw new NullReferenceException("test crash input!");

            this.tempscreens.Clear();

            foreach (DGScreen screen in this.screens)
                this.tempscreens.Add(screen);

            while (this.tempscreens.Count > 0)
            {
                DGScreen screen = this.tempscreens[this.tempscreens.Count - 1];

                this.tempscreens.RemoveAt(this.tempscreens.Count - 1);

                screen.Update(gameTime);

                if (!this.inputhandled || screen.OverrideInput)
                {
                    screen.HandleInput(gameTime, input);

                    if (!screen.OverrideInput)
                        this.inputhandled = true;
                }
            }

            this.inputhandled = false;

            base.Update(gameTime);
        }
        public      override void   Draw            (GameTime gameTime)
        {
            foreach (DGScreen screen in this.screens)
            {
                this.spritebatch.Begin();

                screen.Render(this.spritebatch);

                this.spritebatch.End();
            }

            base.Draw(gameTime);
        }

        public      void            AddScreen       (DGScreen screen)
        {
            if (!this.screens.Contains(screen))
            {
                screen.ScreenManager = this;

                if (this.isinitialized)
                {
                    screen.LoadContent();
                }

                this.screens.Add(screen);
            }
        }
        public      void            RemoveScreen    (DGScreen screen)
        {
            if (this.screens.Contains(screen) && this.screens.Count > 1)
            {
                screen.UnloadContent();

                this.screens.Remove(screen);
            }
        }
    }
}