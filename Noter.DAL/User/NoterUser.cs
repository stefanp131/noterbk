﻿using System.ComponentModel.DataAnnotations;

namespace Noter.DAL.User
{
    public class NoterUser
    {
        [Key]
        public string Id { get; set; }
        public string UserName { get; set; }
        public string NormalizedUserName { get; set; }
        public string PasswordHash { get; set; }
    }
}
