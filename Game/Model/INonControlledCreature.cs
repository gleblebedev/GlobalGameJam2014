using System;
using System.Collections.Generic;

namespace Game.Model
{
    public interface INonControlledCreature
    {
        Basis Position { get; }

        float Pitch { get; set; }

		void Update(TimeSpan dt, IEnumerable<IControlledCreature> players);

    }
}