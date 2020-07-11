using System;
using System.Collections.Generic;
using System.Text;

namespace Gravity.Interfaces
{
    public interface IUpdatable
    {
        public void Update(TimeSpan elapsedTime);
    }
}
