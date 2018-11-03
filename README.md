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
