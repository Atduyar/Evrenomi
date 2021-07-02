using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Business.Abstract;
using Business.Constants;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.Dtos;
using Entities.Extensions;

namespace Business.Concrete
{
    public class AuthorManager:IAuthorService
    {
        private IAuthorDal _authorDal;
        private IUserService _userService;

        public AuthorManager(IAuthorDal authorDal, IUserService userService)
        {
            _authorDal = authorDal;
            _userService = userService;
        }

        public IDataResult<List<Author>> GetList()
        {
            return new SuccessDataResult<List<Author>>(_authorDal.GetList().ToList());
        }

        public IResult Add(Author author)
        {
            var user = _userService.GetById(author.UserId,Status.Per.Me);
            if (user.Data == null)
            {
                return new ErrorResult(Messages.UserNotFound);
            }

            var checkAuthor = GetByUserId(author.UserId);
            if (checkAuthor.Data == null)
            {
                return new ErrorResult(Messages.AuthorAlreadyExists);
            }
            author.AuthorStatus = 0;

            _authorDal.Add(author);
            return new SuccessResult(Messages.AuthorAdded);
        }

        public IDataResult<Author> GetById(int authorId)
        {
            return new SuccessDataResult<Author>(_authorDal.Get(a => a.Id == authorId));
        }

        public IDataResult<Author> GetByName(string authorName)
        {
            return new SuccessDataResult<Author>(_authorDal.Get(a => a.AuthorName == authorName));
        }

        public IDataResult<Author> GetByUserId(int userId)
        {
            return new SuccessDataResult<Author>(_authorDal.Get(a => a.UserId == userId));
        }

        public IDataResult<List<AuthorSummaryDto>> GetListSummary()
        {
            var authors = new SuccessDataResult<List<Author>>(_authorDal.GetList().ToList()).Data;

            var result = from a in authors
                select a.ToSummary();
            return new SuccessDataResult<List<AuthorSummaryDto>>(result.ToList());
        }

        public IResult AuthorExists(AuthorForRegister authorForRegister, int userId)
        {
            if (!GetByUserId(userId).Success)
            {
                return new ErrorResult(Messages.AuthorNotFound);
            }

            if (!GetByName(authorForRegister.AuthorName).Success)
            {
                return new ErrorResult(Messages.AuthorNotFound);
            }

            return new SuccessResult(Messages.AuthorAlreadyExists);
        }
    }
}