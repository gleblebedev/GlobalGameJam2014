using System;

namespace Game.Model
{
    public interface INonControlledCreature
    {
        Basis Position { get; }

        float Pitch { get; set; }

        void Update(TimeSpan dt);

    }
}