using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructure.IUtils
{
    public interface IJwtTokenGenerator
    {
        string Generate(User user);
    }
}
