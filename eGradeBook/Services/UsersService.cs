﻿using eGradeBook.Models;
using eGradeBook.Models.Dtos;
using eGradeBook.Repositories;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace eGradeBook.Services
{
    public class UsersService : IUsersService
    {
        private IUnitOfWork db;

        public UsersService(IUnitOfWork unitOfWork)
        {
            db = unitOfWork;
        }

        public async Task<IdentityResult> RegisterAdmin(UserDTO userModel)
        {
            AdminUser user = new AdminUser
            {
                UserName = userModel.UserName,
                FirstName = userModel.FirstName,
                LastName = userModel.LastName
            };

            return await db.AuthRepository.RegisterAdminUser(user, userModel.Password);
        }

        public async Task<IdentityResult> RegisterStudent(UserDTO userModel)
        {
            StudentUser user = new StudentUser
            {
                UserName = userModel.UserName,
                FirstName = userModel.FirstName,
                LastName = userModel.LastName
            };

            return await db.AuthRepository.RegisterStudentUser(user, userModel.Password);
        }

        public async Task<IdentityResult> RegisterTeacher(UserDTO userModel)
        {
            TeacherUser user = new TeacherUser
            {
                UserName = userModel.UserName,
                FirstName = userModel.FirstName,
                LastName = userModel.LastName
            };

            return await db.AuthRepository.RegisterTeacherUser(user, userModel.Password);
        }

        public async Task<IdentityResult> RegisterParent(UserDTO userModel)
        {
            ParentUser user = new ParentUser
            {
                UserName = userModel.UserName,
                FirstName = userModel.FirstName,
                LastName = userModel.LastName
            };

            return await db.AuthRepository.RegisterParentUser(user, userModel.Password);
        }

        public async Task<IdentityResult> RegisterClassMaster(UserDTO userModel)
        {
            ClassMasterUser user = new ClassMasterUser
            {
                UserName = userModel.UserName,
                FirstName = userModel.FirstName,
                LastName = userModel.LastName
            };

            return await db.AuthRepository.RegisterClassMasterUser(user, userModel.Password);
        }
    }
}