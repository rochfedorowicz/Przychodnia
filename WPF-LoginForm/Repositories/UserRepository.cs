using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using WPF_LoginForm.Models;

namespace WPF_LoginForm.Repositories
{
    public class UserRepository : RepositoryBase
    {
        public void Add(UserModel userModel)
        {
            throw new NotImplementedException();
        }

        public bool AuthenticateUser(NetworkCredential credential)
        {
            bool validUser;

            using (var pe = new PrzychodniaEntities())
            {
                var user = pe.Logowanie.Where(el => el.Loginn == credential.UserName && el.Haslo == credential.Password).SingleOrDefault();

                return user==null ? false : true;
            }
        }

        public void Edit(UserModel userModel)
        {
            throw new NotImplementedException();
        }
        public IEnumerable<UserModel> GetByAll()
        {
            throw new NotImplementedException();
        }
        public UserModel GetById(int id)
        {
            throw new NotImplementedException();
        }
        public Logowanie GetByUsername(string username)
        {
            using (var pe = new PrzychodniaEntities())
            {
                var loggedUser = pe.Logowanie.Where(el => el.Loginn == username).SingleOrDefault();

                return loggedUser;
            }
        }
        public void Remove(int id)
        {
            throw new NotImplementedException();
        }
    }
}
