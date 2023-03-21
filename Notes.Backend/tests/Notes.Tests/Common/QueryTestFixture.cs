using AutoMapper;
using Notes.Application.Interfaces;
using Notes.Application.Mappings;
using Notes.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notes.Tests.Common
{
    public class QueryTestFixture : IDisposable
    {
        public QueryTestFixture() 
        {
            Context = ApplicationDbContextFactory.Create();
            var configurationMapperBuilder = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AssemblyMappingProfile(typeof(IApplicationDbContext).Assembly));
            });

            Mapper = configurationMapperBuilder.CreateMapper();
        }

        public ApplicationDbContext Context;
        public IMapper Mapper;

        public void Dispose()
        {
            ApplicationDbContextFactory.Destroy(Context);
        }
    }

    [CollectionDefinition("QueryCollection")]
    public class QueryColletion : ICollectionFixture<QueryTestFixture> { }
}
