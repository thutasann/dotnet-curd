﻿using dotnet_crud.Common.Model;
using dotnet_crud.Utils;
using MySqlConnector;

namespace dotnet_crud.Repositories
{
    public class CrudRL : ICrudRL
    {
        public readonly IConfiguration _configuration;
        public readonly MySqlConnection _mySqlConnection;
        public readonly ILogger<CrudRL> _logger;

        public CrudRL(IConfiguration _configuration, ILogger<CrudRL> _logger)
        {
            this._configuration = _configuration;
            this._logger = _logger;
            _mySqlConnection = new MySqlConnection(_configuration["ConnectionStrings:MySqlDBConnection"]);
        }


        public async Task<AddInformationResponse> AddInformation(AddInformationRequest request)
        {

            _logger.LogInformation("Add Information Repository Layer Calling");
            AddInformationResponse response = new();
            response.IsSuccess = true;
            response.Message = "Successful";

            try
            {
                if(_mySqlConnection.State != System.Data.ConnectionState.Open)
                {
                    await _mySqlConnection.OpenAsync();
                }

                using (MySqlCommand sqlCommand = new MySqlCommand("INSERT INTO CrudApplication(UserName, EmailID, MobileNumber, Salary, Gender) VALUES (@UserName, @EmailID, @MobileNumber, @Salary, @Gender);", _mySqlConnection))
                {
                    sqlCommand.CommandType = System.Data.CommandType.Text;
                    sqlCommand.CommandTimeout = 180;
                    sqlCommand.Parameters.AddWithValue("@UserName", request.UserName);
                    sqlCommand.Parameters.AddWithValue("@EmailID", request.EmailID);
                    sqlCommand.Parameters.AddWithValue("@MobileNumber", request.MobileNumber);
                    sqlCommand.Parameters.AddWithValue("@Gender", request.Gender);
                    int Status = await sqlCommand.ExecuteNonQueryAsync();
                    if (Status <= 0)
                    {
                        response.IsSuccess = false;
                        response.Message = "AddInformation Query Not Executed";
                        _logger.LogError("AddInformation Query Not Executed");
                    }
                }
            }
            catch (Exception e)
            {
                response.IsSuccess = false;
                response.Message = "From Repository " + e.Message;
                Console.WriteLine(e);
            }
            finally
            {
                await _mySqlConnection.CloseAsync();
                await _mySqlConnection.DisposeAsync();
            }
            return response;

        }
    }
}

