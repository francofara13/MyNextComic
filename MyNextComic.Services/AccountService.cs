using MyNextComic.Contracts.Account;
using MyNextComic.Contracts.Entities;
using MyNextComic.Data;
using MyNextComic.Data.Mappers;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Security.Cryptography;
using System.Transactions;

namespace MyNextComic.Services
{
    public class AccountService
    {
        public UserServiceResponse InsertUser(User user)
        {
            var result = new UserServiceResponse();
            using (TransactionScope scope = new TransactionScope())
            {
                MyNextComicEntities context = null;
                try
                {
                    context = new MyNextComicEntities();
                    context.Configuration.AutoDetectChangesEnabled = false;

                    if (!(context.Users.Any(x => x.Username == user.UserName)))
                    {
                        var Id = context.Users.OrderByDescending(x => x.Id).Select(x => x.Id).First();
                        context.Users.Add(new Users()
                        {
                            Id = Id + 1,
                            Username = user.UserName,
                            Password = HashPassword(user.Password),
                            Email = user.Email
                        });
                        context.SaveChanges();
                        context.Dispose();

                        result.Success = true;
                        result.ErrorMessage = "";
                    }
                    else
                    {
                        context.Dispose();
                        result.Success = false;
                        result.ErrorMessage = "El nombre de usuario ingresado no se encuentra disponible";
                    }
                }
                catch (DbEntityValidationException e)
                {
                    result.Success = false;
                    result.ErrorMessage = e.Message;
                }

                scope.Complete();
            }

            return result;
        }

        public UserServiceResponse LogInUser(User user)
        {
            var result = new UserServiceResponse();

            using (TransactionScope scope = new TransactionScope())
            {
                MyNextComicEntities context = null;
                try
                {
                    context = new MyNextComicEntities();
                    context.Configuration.AutoDetectChangesEnabled = false;

                    var storedUser = context.Users.Where(x => x.Username == user.UserName).FirstOrDefault();
                    if (storedUser != null)
                    {
                        if (VerifyPassword(storedUser.Password, user.Password))
                        {
                            result.Success = true;
                            result.ErrorMessage = "";
                        }
                        else
                        {
                            result.Success = false;
                            result.ErrorMessage = "Contraseña inválida";
                        }
                    }
                    else
                    {
                        context.Dispose();
                        result.Success = false;
                        result.ErrorMessage = "No se encontró ningun usuario con ese nombre";
                    }
                }
                catch (DbEntityValidationException e)
                {
                    result.Success = false;
                    result.ErrorMessage = e.Message;
                }

                scope.Complete();
            }

            return result;
        }

        public User GetUserData(string userName)
        {
            var result = new User();

            using (TransactionScope scope = new TransactionScope())
            {
                MyNextComicEntities context = null;
                try
                {
                    context = new MyNextComicEntities();
                    context.Configuration.AutoDetectChangesEnabled = false;

                    var storedUser = context.Users.Where(x => x.Username == userName).FirstOrDefault();
                    if (storedUser != null)
                    {
                        result.UserId = storedUser.Id;
                        result.UserName = storedUser.Username;
                        result.Email = storedUser.Email;

                        var preferences = context.Preferences.Where(x => x.UserID == storedUser.Id).ToList();
                        var comicsIds = preferences.Select(x => x.ItemID ).ToList();
                        if (preferences.Count() > 0)
                        {
                            var ratedComics = context.Comics.Where(x => comicsIds.Any(y => x.Id_Comic == y)).ToList();
                            var mapper = new MyNextComicMapper();
                            var issues = new List<Issue>();
                            foreach (var comic in ratedComics)
                            {
                                var issue = mapper.MapIssue(comic);
                                issue.Rating = preferences.Where(x => x.ItemID == comic.Id_Comic).FirstOrDefault().Value;
                                issues.Add(issue);
                            }

                            result.ComicList = issues.AsEnumerable();
                        }
                    }
                    else
                    {
                        context.Dispose();
                    }
                }
                catch (DbEntityValidationException e)
                {
                }

                scope.Complete();
            }

            return result;
        }

        private string HashPassword(string password)
        {
            var result = string.Empty;

            byte[] salt;
            new RNGCryptoServiceProvider().GetBytes(salt = new byte[16]);

            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000);
            byte[] hash = pbkdf2.GetBytes(20);

            byte[] hashBytes = new byte[36];
            Array.Copy(salt, 0, hashBytes, 0, 16);
            Array.Copy(hash, 0, hashBytes, 16, 20);

            result = Convert.ToBase64String(hashBytes);

            return result;
        }

        private bool VerifyPassword(string storedPassword, string password)
        {
            bool result = true;
            string savedPasswordHash = storedPassword;
            byte[] hashBytes = Convert.FromBase64String(savedPasswordHash);
            byte[] salt = new byte[16];
            Array.Copy(hashBytes, 0, salt, 0, 16);
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000);
            byte[] hash = pbkdf2.GetBytes(20);
            for (int i = 0; i < 20; i++)
            {
                if (hashBytes[i + 16] != hash[i])
                {
                    result = false;
                }
            }
            return result;
        }
    }
}
