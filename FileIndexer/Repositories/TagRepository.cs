using Dapper;
using FileIndexer.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileIndexer.Repositories
{
    public class TagRepository : Repository<Tag>
    {
        public TagRepository(ILogger<Repository<Tag>> logger, IDbConnection dbConnection) : base(logger, dbConnection)
        {
        }

        public long EnsureExists(Tag entity)
        {
            long? tagId = DbConnection.ExecuteScalar<long?>("SELECT Id FROM Tags WHERE Text = @Text", new { entity.Text });

            var recordId = tagId ?? Create(entity);

            return recordId;
        }

    }
}
