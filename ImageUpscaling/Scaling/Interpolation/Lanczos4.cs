using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageUpscaling.Scaling.Interpolation
{
    /// <summary>
    /// Интерполяция Ланцоша с порядком 4
    /// </summary>
    class Lanczos4 : LanczosResampling
    {
        protected override int A => 4;
    }
}
