using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Manager.Interfaces;

public interface IDump
{
    int Order { get; }

    Task DumpAsync();
}