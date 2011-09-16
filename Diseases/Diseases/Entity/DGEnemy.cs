﻿using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using Diseases.Input;
using Diseases.Physics;
using Diseases.Graphics;

using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;

namespace Diseases.Entity
{
    public class DGEnemy : DGEntity
    {
        public bool dead = false;

        bool damaged = false;
        float cooldowndamage = 0;
        public int damagecounter = 0;

        Random randomizer;

        Vector2 fVector = Vector2.Zero;

        public DGEnemy(Random randomizer)
        {
            this.randomizer = randomizer;
        }
        protected override void Initialize()
        {
            this.restitution = 1.5f;
            this.speed = 1;
            this.sprite = new DGSpriteStatic("entities/enemy/idle");
            this.sprite.Scale = new Vector2(1.5f);
        }

        public override void LoadContent(ContentManager content, World physics)
        {
            base.LoadContent(content, physics);

            this.physics.Position = ConvertUnits.ToSimUnits(new Vector2(randomizer.Next(40, 760), randomizer.Next(40, 500)));
            this.physics.ApplyLinearImpulse(new Vector2(this.speed * (float)Math.Cos(randomizer.Next()), this.speed * (float)Math.Sin(randomizer.Next())));

            this.physics.CollisionCategories = Category.Cat3;
            this.physics.CollidesWith = Category.Cat3;

            this.bounds.X = (int)ConvertUnits.ToDisplayUnits(this.physics.Position.X);
            this.bounds.Y = (int)ConvertUnits.ToDisplayUnits(this.physics.Position.Y);
        }

        public override void Update(GameTime gametime)
        {
            base.Update(gametime);

            this.cooldowndamage += (float)gametime.ElapsedGameTime.TotalSeconds;

            if (this.cooldowndamage > 2)
            {
                this.damaged = false;

                this.cooldowndamage = 0;
            }

            if (this.damagecounter == 5)
                this.sprite.Tint = Color.Yellow;

            if (this.damagecounter == 8)
                this.sprite.Tint = Color.Orange;

            if (this.damagecounter == 10)
                this.dead = true;

            float fx = MathHelper.Clamp(this.physics.LinearVelocity.X, -this.speed, this.speed);
            float fy = MathHelper.Clamp(this.physics.LinearVelocity.Y, -this.speed, this.speed);

            if (fx > 0)
                fx = fx + (this.speed - fx);
            else
                fx = fx - (this.speed + fx);

            if (fy > 0)
                fy = fy + (this.speed - fy);
            else
                fy = fy - (this.speed + fy);

            this.fVector.X = fx;
            this.fVector.Y = fy;

            this.physics.LinearVelocity = this.fVector;
        }

        public void Damage()
        {
            if (!this.damaged)
            {
                this.damagecounter++;

                this.damaged = true;
            }
        }
    }
}
