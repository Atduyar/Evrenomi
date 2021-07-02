using System.Collections.Generic;
using System.Linq;
using Business.Abstract;
using Business.Constants;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;

namespace Business.Concrete
{
    public class TagManager:ITagService
    {
        private ITagDal _tagDal;

        public TagManager(ITagDal tagDal)
        {
            _tagDal = tagDal;
        }

        public IDataResult<Tag> GetById(int id)
        {
            return new SuccessDataResult<Tag>(_tagDal.Get(bt => bt.Id == id));
        }

        public IDataResult<Tag> GetByName(string name)
        {
            return new SuccessDataResult<Tag>(_tagDal.Get(bt => bt.Name == name));
        }

        public IDataResult<List<Tag>> GetList()
        {
            return new SuccessDataResult<List<Tag>>(_tagDal.GetList().ToList());
        }

        public IResult Add(Tag tag)
        {
            _tagDal.Add(tag);
            return new SuccessResult(Messages.TagAdded);
        }

        public IResult Update(Tag tag)
        {
            _tagDal.Update(tag);
            return new SuccessResult(Messages.TagUpdated);
        }

        public IResult Delete(Tag tag)
        {
            _tagDal.Delete(tag);
            return new SuccessResult(Messages.TagDeleted);
        }
    }
}