using Dapper;
using Npgsql;
using System.Collections;
using WEBAPI.Data.Interfaces;
using WEBAPI.DTOs.User;
using WEBAPI.Models;

namespace WEBAPI.Data.Services
{
    public class UserService : IUserService
    {
        private PostgressqlConfiguration _postgresConfig;
        public UserService(PostgressqlConfiguration postgresConfig) => _postgresConfig = postgresConfig;

        public NpgsqlConnection CreateConnection() => new NpgsqlConnection(_postgresConfig.Connection);

        #region Create
        public Task<UserModel?> Create(CreateUserDto CreateUserDto)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region FindAll
        //tipos genericos <>
        public async Task<IEnumerable<UserModel>> FindAll()
        {
            string sqlQuery = "Select * From view_usuario";

            using NpgsqlConnection database = CreateConnection();

            Dictionary<int, List<TareaModel>> userTasks = [];

            try
            {
                await database.OpenAsync();

                IEnumerable<UserModel> users = await database.QueryAsync<UserModel, TareaModel, UserModel>(
                    sql: sqlQuery,
                    map: (user, task) => {
                        List<TareaModel> currentTasks = userTasks[user.IdUsuario] ?? [];
                        currentTasks.Add (task);
                        userTasks[user.IdUsuario] = currentTasks;
                        return user;
                    },
                    splitOn: "idTarea"
                    );
                await database.CloseAsync();

                return users; 
            }
            catch (Exception e) {
                //[] esto representa una lista
                return[];
            }
        }
        #endregion

        #region FIndOne
        public Task<UserModel?> FindOne(int userId)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Remove
        public Task<UserModel?> Remove(int userId)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Update
        public Task<UserModel?> Update(UpdateUserDto updateUserDto)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
