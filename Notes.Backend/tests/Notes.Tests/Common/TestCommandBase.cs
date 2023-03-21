using Notes.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notes.Tests.Common;

public class TestCommandBase : IDisposable
{
    protected readonly ApplicationDbContext Context;

    public TestCommandBase()
    {
        Context = ApplicationDbContextFactory.Create();
    }

    public void Dispose()
    {
        ApplicationDbContextFactory.Destroy(Context);
    }
}
