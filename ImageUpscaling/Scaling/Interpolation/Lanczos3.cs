using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageUpscaling.Scaling.Interpolation
{
    /// <summary>
    /// Интерполяция Ланцоша с порядком 3
    /// </summary>
    class Lanczos3 : LanczosResampling
    {
        protected override int A => 3;
    }
}
