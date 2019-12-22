using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageUpscaling.Scaling.Interpolation
{
    /// <summary>
    /// Интерполяция Ланцоша с порядком 2
    /// </summary>
    class Lanczos2 : LanczosResampling
    {
        protected override int A => 2;
    }
}
