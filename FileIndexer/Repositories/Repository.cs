using Dapper.Contrib.Extensions;
using FileIndexer.Models;
using Microsoft.Extensions.Logging;
using System.Data;

namespace FileIndexer.Repositories
{
    public interface IRepository<T> where T : EntityBase
    {
        public string Name { get; }

        //T GetById(int id);
        long Create(T entity);
        //void Update(T entity);
        //void Delete(T entity);
    }
    public class Repository<T> : IRepository<T> where T : EntityBase
    {
        public string Name { get; }
        internal readonly ILogger<Repository<T>> Logger;
        internal readonly IDbConnection DbConnection;

        public Repository(ILogger<Repository<T>> logger, IDbConnection dbConnection)
        {
            Name = nameof(Repository<T>);
            Logger = logger;
            DbConnection = dbConnection;

            Logger.Log(LogLevel.Information, $"{Name} started");
        }

        //public T GetById(int id)
        //{
        //    //Code to retrieve an entity by Id
        //}

        public long Create(T entity)
        {
            //Code to create the entity
            return DbConnection.Insert(entity);
        }

        //public void Update(T entity)
        //{
        //    //Code to update an entity
        //}


        //public void Delete(T entity)
        //{
        //    //Code for deleting an entity
        //}
    }
}
