using System.Collections.Generic;
using Core.Entities.Concrete;
using Core.Utilities.Results;
using Entities.Concrete;

namespace Business.Abstract
{
    public interface ITagService
    {
        IDataResult<Tag> GetById(int id);
        IDataResult<Tag> GetByName(string name);
        IDataResult<List<Tag>> GetList();
        IResult Add(Tag tag);
        IResult Update(Tag tag);
        IResult Delete(Tag tag);
    }
}