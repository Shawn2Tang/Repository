# Brief Introduct
Repository is a C# light weight generic data access layer which support extension.

# Usage

1 Create Table Todo in sql server
	CREATE TABLE Todo(
		[TodoId] [int] IDENTITY(1,1) NOT NULL,
		[Title] [varchar] (256) NULL,
		[Desc] [nvarchar] (512) NULL,
		[NullableInt] [int] NULL
	)

2 Create class TodoModel
	public class TodoModel
	{
		public int? TodoId { get; set; }
		public string Title { get; set;}
		public string Desc { get; set; }
		public int? NullableInt { get; set; }
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

# Extend

For example, implement MySql repository

1 Use Package Manager install mysql assembly: Install-Package MySql.Data

2 Create repository class which inherite from AbstractRepository and implement all abstract method in abstract class.

	public class MySqlRepository : AbstractRepository, IRepository
	{
		public override List<T> Get<T>(
			string dbConnString, 
			string sql, IList<IDataParameter> dataParameters = null, 
			int commandTimeout = 10000, 
			CommandBehavior commandBehavior = CommandBehavior.CloseConnection)
		{
			using (var conn = new MySqlConnection(dbConnString))
			{
				using (var cmd = new MySqlCommand(sql, conn))
				{
					return base.Get<T>(conn, cmd, dataParameters, commandTimeout, commandBehavior);
				}
			}
		}

		public override IEnumerable<T> Get<T>(
			string dbConnString, 
			string sql, 
			Func<IDataRecord, T> projector, 
			IList<IDataParameter> dataParameters = null, 
			int commandTimeout = 10000, 
			CommandBehavior commandBehavior = CommandBehavior.CloseConnection)
		{
			using (var conn = new MySqlConnection(dbConnString))
			{
				using (var cmd = new MySqlCommand(sql, conn))
				{
					return base.Get<T>(conn, cmd, projector, dataParameters, commandTimeout, commandBehavior);
				}
			}
		}

		public override async Task<IEnumerable<T>> GetAsync<T>(
			string dbConnString, 
			string sql, 
			CancellationToken cancellationToken, 
			IList<IDataParameter> dataParameters = null, 
			CommandBehavior commandBehavior = CommandBehavior.CloseConnection)
		{
			using (var conn = new MySqlConnection(dbConnString))
			{
				using (var command = new MySqlCommand(sql, conn))
				{
					return await base.GetAsync<T>(conn, command, cancellationToken, dataParameters, commandBehavior);
				}
			}
		}
	}