using Microsoft.EntityFrameworkCore;
using Notes.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notes.Application.Interfaces;

public interface IApplicationDbContext
{
    DbSet<Note> Notes { get; set; }
    public Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
