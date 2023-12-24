﻿namespace dotnet_crud.Utils
{
	public class SqlQueries
	{
        public static IConfiguration _sqlQueryConfiguration = new ConfigurationBuilder()
           .AddXmlFile("SqlQueries.xml", true, true)
           .Build();

        public static string AddInformation { get { return _sqlQueryConfiguration["AddInformation"]; } }
    }
}

