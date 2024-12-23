﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WEBAPI.Models
{
    public class UserModel
    {
        //representacion de la tabla de Usuario de la base de datos.
        public int IdUsuario { get; set; }
        public string Nombres { get; set; }
        public string Usuario { get; set; }
        public string Contrasena { get; set; }
        public List<TareaModel> Tareas { get; set; } = [];

        public override bool Equals(object? obj)
        {
            if (obj == null) return false;

            if (obj is UserModel user) return user.IdUsuario == IdUsuario;

            return false;
        }

        public override int GetHashCode()
        {
            return IdUsuario.GetHashCode();
        }

    }
}
