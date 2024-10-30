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
        public async Task<UserModel?> Create(CreateUserDto CreateUserDto)
        {
            using NpgsqlConnection database = CreateConnection();
            string sqlQuery = "Select * From fun_user_create(" +
                "p_nombres := @nombres," +
                "p_usuario := @usuario," +
                "p_contrasena := @contrasena);";
            try
            {
                await database.OpenAsync();
                IEnumerable<UserModel?> result = await database.QueryAsync<UserModel?>(
                        sqlQuery,
                        param: new
                        {
                            nombres = CreateUserDto.Names,
                            usuario = CreateUserDto.UserName,
                            contrasena = CreateUserDto.Password
                        }
                    );
                await database.CloseAsync();
                return result.FirstOrDefault();
            }
            catch (Exception ex) 
            {
                return null;
            }
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

                IEnumerable<UserModel> result = await database.QueryAsync<UserModel, TareaModel, UserModel>(
                    sql: sqlQuery,
                    map: (user, task) => {
                        List<TareaModel> currentTasks = [];

                        userTasks.TryGetValue(user.IdUsuario, out currentTasks);

                        currentTasks ??= [];

                        if (currentTasks.Count == 0 && task != null)
                        {
                            currentTasks = [task];
                        }
                        else if (currentTasks.Count > 0 && task != null) {
                            currentTasks.Add(task);
                        }

                        userTasks[user.IdUsuario] = currentTasks;
                        return user;
                    },
                    splitOn: "idTarea"
                    );
                await database.CloseAsync();

                IEnumerable<UserModel> users = result.Distinct().Select(user => {
                    user.Tareas = userTasks[user.IdUsuario];
                    return user;
                });
                return users; 
            }
            catch (Exception e) {
                //[] esto representa una lista
                return[];
            }
        }
        #endregion

        #region FIndOne
        public async Task<UserModel?> FindOne(int userId)
        {
            NpgsqlConnection database = CreateConnection();
            string sqlQuery = "Select * from usuario where idusuario = @idusuario";

            try
            {
                await database.OpenAsync();
                UserModel? result = await database.QueryFirstOrDefaultAsync<UserModel>(
                    sqlQuery,
                    param: new
                    {
                        idusuario = userId
                    }
                    );
                await database.CloseAsync();
                return result;
            }
            catch (Exception ex) 
            {
                return null;
            }
        }
        #endregion

        #region Remove
        public async Task<UserModel?> Remove(int userId)
        {
            NpgsqlConnection database = CreateConnection();
            string sqlQuery = "select * from fun_user_remove(" +
                "p_idUsuario := @idusuario)";
            try
            {
                await database.OpenAsync();
                UserModel? result = await database.QueryFirstOrDefaultAsync<UserModel>(
                    sqlQuery,
                    param: new 
                    {
                        idusuario = userId
                    });
                await database.CloseAsync();
                return result;
            }
            catch (Exception e) 
            {
                return null;
            }
        }
        #endregion

        #region Update
        public async Task<UserModel?> Update(int iduser, UpdateUserDto updateUserDto)
        {
            NpgsqlConnection database = CreateConnection();
            string sqlQuery = "Select * from fun_user_update(" +
                "p_idusuario := @idusuario," +
                "p_nombres := @nombres," +
                "p_usuario := @usuario," +
                "P_contrasena := @contrasena);";

            try
            {
                await database.OpenAsync();
                var result = await database.QueryAsync<UserModel>(
                    sqlQuery,
                    param: new
                    {
                        idusuario = iduser,
                        nombres = updateUserDto.Names,
                        usuario = updateUserDto.UserName,
                        contrasena = updateUserDto.Password
                    }
                    );
                await database.CloseAsync();
                return result.FirstOrDefault();
            }
            catch (Exception ex) 
            {
                return null;
            }
        }
        #endregion
    }
}
