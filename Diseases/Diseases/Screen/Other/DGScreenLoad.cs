﻿using System;
using System.Diagnostics;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Microsoft.Xna.Framework.Content;

using Diseases.Input;
using Diseases.Graphics;

namespace Diseases.Screen.Other
{
    public class DGScreenLoad : DGScreen
    {
        IDGSprite background;

        public                  DGScreenLoad    (IDGSprite loadScreen)
        {
            background = loadScreen;
        }

        public override void    LoadContent     ()
        {
            background.LoadContent(this.ScreenManager.Content);

            base.LoadContent();
        }
        public override void    UnloadContent   ()
        {
            background.UnloadContent();

            base.UnloadContent();
        }

        public override void    Update          (GameTime gametime)
        {
            background.Update(gametime);
        }
        public override void    Render          (SpriteBatch batch)
        {
            background.Render(batch);
        }
    }
}
