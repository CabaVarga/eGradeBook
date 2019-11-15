using eGradeBook.Models;
using eGradeBook.Repositories;
using eGradeBook.Utilities.WebApi;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace eGradeBook.Services
{
    public class FileResourcesService : IFileResourcesService
    {
        private IUnitOfWork db;

        public FileResourcesService(IUnitOfWork db) 
        {
            this.db = db;
        }

        public IEnumerable<FileResource> GetAllFileResources()
        {
            return db.FileResourcesRepository.Get();
        }

        public FileResource GetFileResource(int id)
        {
            return db.FileResourcesRepository.GetByID(id);
        }

        public FileResource CreateFileResource(FileResource FileResource)
        {
            db.FileResourcesRepository.Insert(FileResource);
            db.Save();

            return FileResource;
        }

        public FileResource UpdateFileResource(int id, int userId, string description, string path)
        {

            FileResource FileResource = db.FileResourcesRepository.GetByID(id);

            if (FileResource != null)
            {
                FileResource.UserId = userId;
                FileResource.Description = description;
                FileResource.Path = path;

                db.FileResourcesRepository.Update(FileResource);
                db.Save();
            }

            return FileResource;
        }

        public FileResource DeleteFileResource(int id)
        {
            FileResource FileResource = db.FileResourcesRepository.GetByID(id);

            if (FileResource != null)
            {
                db.FileResourcesRepository.Delete(FileResource);
                db.Save();
            }

            return FileResource;
        }


        #region File retrieval methods
        public byte[] RetrieveFileResourceDataAsByteArray(FileResource fileResource)
        {
            return File.ReadAllBytes(fileResource.Path);
        }

        public byte[] RetrieveFileResourceDataAsByteArray(int id)
        {
            return File.ReadAllBytes(db
                .FileResourcesRepository
                .GetByID(id)
                .Path);
        }

        public FileStream RetrieveDataAsFileStream(FileResource fileResource)
        {
            return File.OpenRead(fileResource.Path);
        }

        public FileStream RetrieveDataAsFileStream(int id)
        {
            return File.OpenRead(db
                .FileResourcesRepository
                .GetByID(id)
                .Path);
        }
        #endregion


    }
}