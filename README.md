# repository
Repository is a C# light weight generic data access layer which support extension.

How to use it?

1 Create Table Todo in sql server

    CREATE TABLE Todo(
        [TodoId] [int] IDENTITY(1,1) NOT NULL,
        [Title] [varchar] (256) NULL
    )

2 Create class TodoModel

    public class TodoModel
    {
        public int? TodoId { get; set; }
        public string Title { get; set;}
    }

3 Retrieve data from db by IRepository

    public class TodoBL
    {
        public void Retrieve()
        {
            IRepository su = new SqlRepository();
            SqlConnectionStringBuilder connBuilder = new SqlConnectionStringBuilder()
            {
                DataSource = "sql instance",
                InitialCatalog = "database name",
                IntegratedSecurity = true
            };
            var todos = su.Get<TodoModel>(
                connBuilder.ToString(),
                @"select * from Todo"
                );

            foreach (var todo in todos)
            {
                Console.WriteLine(todo.Title);
            }

            List<string> result = su.Get<string>(
                connBuilder.ToString(),
                @"select 'todo'"
                );

            foreach(var item in result)
            {
                Console.WriteLine(item);
            }
        }
    }
    
    How to extend it?
    
    1 Use Package Manager install mysql assembly: Install-Package MySql.Data
    2 Create repository class which inherite from AbstractRepository and implement all abstract method in abstract class.
    For example, implement MySql repository as follows:
    
    public class MySqlRepository : AbstractRepository, IRepository
    {
        public override List<T> Get<T>(
            string dbConnString, 
            string sql, IList<IDataParameter> dataParameters = null, 
            int commandTimeout = 10000, 
            CommandBehavior commandBehavior = CommandBehavior.CloseConnection)
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<T> Get<T>(
            string dbConnString, 
            string sql, 
            Func<IDataRecord, T> projector, 
            IList<IDataParameter> dataParameters = null, 
            int commandTimeout = 10000, 
            CommandBehavior commandBehavior = CommandBehavior.CloseConnection)
        {
            throw new NotImplementedException();
        }

        public override Task<IEnumerable<T>> GetAsync<T>(
            string dbConnString, 
            string sql, 
            CancellationToken cancellationToken, 
            IList<IDataParameter> dataParameters = null, 
            CommandBehavior commandBehavior = CommandBehavior.CloseConnection)
        {
            throw new NotImplementedException();
        }
    }
