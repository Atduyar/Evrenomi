using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices.ComTypes;
using Business.Abstract;
using Business.Constants;
using Business.Extensions;
using Business.Helpers;
using Core.DataAccess.Concrete;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.Dtos;
using Entities.Extensions;
using LinqKit;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Newtonsoft.Json;


namespace Business.Concrete
{
    public class BlogManager: IBlogService
    {
        private IBlogDal _blogDal;
        private IBlogEmojiCountViewService _blogEmojiCountViewService;
        private IBlogEmojiViewViewService _blogEmojiViewService;
        private IBlogEmojiService _blogEmojiService;
        private IDateTimeHelper _dateTimeHelper;
        private IUserService _userService;
        private IBlogTagService _blogTagService;

        public BlogManager(IBlogDal blogDal, IDateTimeHelper dateTimeHelper, IBlogEmojiCountViewService blogEmojiCountViewService, IBlogEmojiViewViewService blogEmojiViewService, IBlogEmojiService blogEmojiService, IUserService userService, IBlogTagService blogTagService)
        {
            _blogDal = blogDal;
            _dateTimeHelper = dateTimeHelper;
            _blogEmojiCountViewService = blogEmojiCountViewService;
            _blogEmojiViewService = blogEmojiViewService;
            _blogEmojiService = blogEmojiService;
            _userService = userService;
            _blogTagService = blogTagService;
        }

        
        public IDataResult<string> GetHtmlBlog(BlogDetailDto blogDetailDto, int userId)
        {
            GetBlogView(blogDetailDto.BlogId, userId);
            return new SuccessDataResult<string>(data: MakeHtml(blogDetailDto));
        }

        public void GetBlogView(int blogId, int userId)
        {
            if (!_blogEmojiViewService.UserViewed(blogId, userId).Success)//Eğer zaten görmüsse onu isaretleme
            {
                _blogEmojiService.Add(new BlogEmoji
                {
                    BlogId = blogId,
                    UserId = userId,
                    EmojiId = 1
                });
            }
        }

        public IDataResult<string> GetHtmlBlog(BlogDetailDto blogDetailDto)
        {
            return new SuccessDataResult<string>(data: MakeHtml(blogDetailDto));
        }

        public string MakeHtml(BlogDetailDto blogDetailDto)
        {

            string htmlStr = $@"<div id='topAT'>
<div id='headerAT'>
	<img src='{blogDetailDto.BlogTitlePhotoUrl}' class='img-responsive imgA'>
	<h2 id='titleAT'>{blogDetailDto.BlogTitle}</h2>
</div>
<div id='infoAT'>   
	<div id='profilAT'>
	<img id='ppAT'src='data:image/jpeg;base64,/9j/4AAQSkZJRgABAQAAAQABAAD/2wCEAAkGBw8QEA8PDg8QDRAPDxIVDg0PDxAQEA0PFhEWFhcVFhYYHiggGBolGxUWITEhJSwrLi4uFx8zODMsNygtLisBCgoKDg0OGhAQGy0dHSUtKy0tLy0tKy0tLSstLSstMi0rLSstLSstLSstLTctLSstLS0tLS0tLS0tLS0tLS0tLf/AABEIAOAA4QMBEQACEQEDEQH/xAAcAAEBAAIDAQEAAAAAAAAAAAAAAQMFAgQHBgj/xABAEAACAQMABQoDBQUIAwAAAAAAAQIDBBEFEiExUQYHEyJBYXGBkaEyUrEUcoKSwSNCYnOiJEN0ssLD0fEzU2P/xAAaAQEAAwEBAQAAAAAAAAAAAAAAAQMEBQIG/8QALBEBAAICAQMCBgICAwEAAAAAAAECAxEEEiExMkEFE1FhcYEi8CNCM7HRof/aAAwDAQACEQMRAD8A4HWcMAAAAAAAAgQBIAAgEAgAgQJQCECAQJQAQIBAOIEAgADYHt5AAAAAAAQIAkAARgQCACBAlAIQIBAlABAgEA4gQCACBANiWPIAAAAAECAJAAEYEAgAgQJQCECAQJQAQIBAOIEAgAgQCAbIseQAAAAQIAkAAQCAQCSklvaXe3giZ0mImfDBK8pL+8h+ZM8fMp9VkYck+0oryk/7yH5kh8yn1Pk5PpLLGae5p+Dyet78PExMeQIQJQAQIBAOIEAgAgQDiwGQNmWPIAAAQIAkAAQCAY61aMFmUlHxZ5taK+Zeq0tbxG2nutLSeyn1V8z+J/8ABkvyJntXs34+JWO9u7XTm5PMm5Pi3kzzMz5aoiI7R2QhIATxtWx8VsBPfy7VHSFWP72suEtvvvLa5rx91F+PS3tpsbbSMJ7H1JcHufgzTTNW3aezJk41qd47w7hczoQIBxAgEAECAcWBGEoBtSx4AAECAJAAEAgGq0lpLDcKe9fFPg+CMubPr+NW3Bxtx1WaaTbeW2297e1sx+W6I12gCQAAAAAIBs9G3jzqTeflb7O404cv+ssXJwRrrr+2zNTE4gQCACBAOLAjCUIEA2xa8AECAJAAEAgHU0lcdHTbXxPZHub7fQqzX6arsGPrvqfD5w5zrAAAAAAAAAABtrK8ylGT29kuPczfS/VG3JyU6bTDuFitABAgHFgRhKECAMgbYteECAJAAEAgEIGm09PbCPBN+rx+hk5M94hv4cdplqzK2vpeS/I6teJVZy6ChnZUazKrx1Fw73s8SnLminbzK7Hhm/f2fSaY5u6XQ/2OU+mjt/azTVZfLuSi+D9eKppyZ3/Lwttx41/Hy85uKE6cpU6kZU5xeJQksSi+9GuJie8MsxrtLgSgAAAAADLRew04Z7TDFyo/lEtha3X7sn4P9GaIljmHcJQgHFgRhKECARsCAbctVgSAAIBAIQAGj078cfufqzFyfVDo8P0T+Wfknof7ZdQpPPRrM6zX/rjjZ4ttLz7jHlv0V23Y6dVtPaqdOMUoxSjGKSjFLCiksJJHNdFyA1WnOT9teRxXh1kurWh1akPB9q7nlHumS1PDxfHW3l8Bpbm9uqbbt5RuodiyqdVLwex+T8jXXk1nz2Zbce0eO75e8sK9F4rUatHvqU5RXk2sMvi0T4lTNZjzDray4olDlShKbUYRc5PdGCcpPwSE9vJHdv7PkXpCrHXVDo1jYqs405S/C9q88FU56R7rYw3n2aO4oTpzlTqRcJweJwksOLLImJjcK5jXaUo9pfg8yycqP4xLKaWJ27a5x1Zbux8D1EomHbJeUYShAgEbAhAgG4LngAAQCAQgAIEtNp1daH3X9THyfMN/D9MvuOaqy1aNeu1tqVFBP+GEc/WT9Dk8q3eIdfjR2mX3JmaQAAAAYJ2dF7ZUqcu904v9CeqUdMMtOnGOyMVFcIpL6EbTpyA8751dHRToXUVhybpVH82FrQfopL04Gvi281ZeRXxZ8FR3+R0cPqczlej9sppYQDs29xjZLd2PgTEo07eSUIBGwIQIAyBuC54AIBAIQAECUA6el9G150lXhSlKlT1lUqJZUd2/tx39hi5V69UV33dDh0t02trs9D5v6Wro63/i6ST86sseyRxs8/5JdrB6IfRFS0AAAAAAAA+S5zoZsc/LXpv1Ul+pfxvWo5HoebR0bXjSVxKjONGTSjVccRk3ux3d+46eC1euY33crlVt0ROu22E1sAAAz0K+Nj3fQlDt5JQhAgEbAgG6LnhAIBCAAgSgEIHo/J2lFWlBYTUqeZJ7U9ZtvPqfO8uZnNb8vpeHWIwV/DtaNsoUKUKNPZCGtqLhFybS8s4KLWm07lorXpjTskPQAAAAAAAB0dMaMhdU1Sq7YdJCUo/OoS1tV9zxg9UtNZ3DzasWjUutyrpRdjdRaSUaEmljYnFa0ceaRZxpmM1fyp5cR8i34l40d984AAAGajWxse76EodpMIRsCAANyXPCAQgAIEoBCBAPQOR9wp2sY9tOUov11l7NHB59OnNM/Xu+h+H36sMR9OzdmNuAAAAAAAAAAD57l7eKlY1V21nGnFccvL/pUjVw6dWWPt3Y+dfpwz9+39/TyQ7bgAAAAAy0quNj3fQlDsoIRsDiBuy54QgAIEoBCBAIEtxyX0sreq1N/squFN/I1ul7vPj3GPmcf5tO3mGzhcj5N/5eJ8/+vQ4TUknFqSaymnlNdzODMTE6l9DExMbhQkAAAAAAAAk5qKbk1FJZcm0klxbERtEzry8o5caeV3WUaTzRo5UH2VJv4p+GxJefE7XEwfKrufMuDzeRGW+q+IfNmtjAAAAAAyUqmNnZ9AhnJQAboteACBKAQgQCBKACB9byCu//ADUH24qRX9Mv9Jy/iWP03/TrfC8nqp+32BynYAAAAAAAAPg+dG+WrQtlvbdWa4JZjD3cvynS+H083/TlfE8npp+/7/fZ5+dNyQAAAAAAADJTqY8PoEMuuuJKG8LXhAlAIQIBAlABAgHb0PfdBXp1eyMuv3weyXs8+RVnxfMxzVdx8vyskX/unqUWmk1tT3NdqPm31CgAAAAAAN8dnfwA8W5SaS+1XVWsnmLlq0v5cdkfXf5n0GDH8vHFf7t81yMvzck29vb8NYWqQAAAAAAAAAA+kLlaAQgQCBKACBAIBxA9K5J1XOzoOTy0pR8ozlFeyR8/zKxXPbX97Po+DabYK7+//wAltzM1gAAAAAfMc4GlXQtejhlTuW4KS/dhjr+bTx5mzhYuvJufEMPPyzTHqPM9nlR2XCAAAAAAAAAAAB9GXK0IEAgSgAgQCAcQIB6LyJf9jh3Tqf5mzhc//mn9PoPh3/BH5n/tvTG3AAAAAAfC86b6lp9+r9InR+Hebfpy/ifiv7eenUcgAAAAAAAAAAAH0RarQCBKAQAQIBxAgGGtXUcLfJ7IxW9t7iu+SKeVuPFbJ4ewWGh42dOFCMpT2a0pSx8b34x2bDicy3Vk39nf4dIpj6Y+rsGVqAAAAAA+f5caDjc2davrSjOypzqQSxqTjjM1LZn4YvGGjdwr9M2/TBzsXzIiNvI0zrVtFo7OLek0nUhLyAAAAAAAAAAH0JarQJQCAAIQOIEA6V7eOL1Yrb2t9hRly9PaGrBg646reHRo1MThKTzicW2+5pmOZ23xERGofpW7t1USa3rc+PcU5sXXH3XYsnRP2amcHF4aw12GCYmJ1LdExMbhxISAAAHOjSlN4is/ReJ6pSbTqHm1orG5XlRBUtG3/wDhK+3jJ0pJe7R0cdIpGmDJebzt+ekyyJmJ3Cq1YtGpZIyyaseTqYM2Lo/ClikAAAAAAAAAfQFrwgEAEDHUqKO94PNrxXy9Ux2v6YYJXkexN+xVPIr7NFeJf37Mcrx9iS8dpXPIn2hbHEr7yxSryfb6bCuct591tcGOPZ0qr6zK1zg0B+keTd509na1t7qW9Nv72os++QO9WoxmsSWeD7UeL0reO71W818NfW0fJbY9ZcNzMl+NaPT3aqciJ89nTkmtjWHwZnmNdpXxO/Cwg5PEU2+4mKzM6gmYjvLu0NHPfN4/hW/1NNONP+zPfkR/q2FOmorEVhGqtYrGoZbWm07l8nzp3fR6MrrOHWnTpx78zUpL8sZHpDwoDLQ3vwETpExE+WXVRZGW0KpwUn2TUPcZ/rCqeLHtKajPcZqq54148d0ZZExPeFFqzWdShKAAAAAb8teEAEDHVqKKbf8A2zze0VjcvWOk3tqGtnNt5ZgtabTuXWpSKxqHE8vQAA6s978QOIHtvNHfdJo5U28u3rVId+rJ9IvLrteQH2wGj5Uco42UYdVValR9Wlr6uIpPMm8PZnC8+4sx4+tdhwzkn6Pjrvl1WqY/YUo47dabeOB6ycKl/MtuPB0e7PYcvpU9k7aMl+9KFRxl6NMmnDrSNRLzk4/XO9vt9DaXo3dPpaLeE2pRksShLg17lV6TWdSw3xzSdS755eHl3PXfbLS2T3udWa8FqR+s/QDy0DJQ3+QHYAAAJJZPdL9MqsuOLx92M2ObMaQAAAAb4teAgQDoXtTLx2L6mPPfc6dDi49V6vq65Q1AAAB1GBAPQ+ZnSGpdV7dvZXpKUfv03u/LN/lA9buriNKE6lR6sIRcpPgkskxG51CYiZnUPGNM6SndV5157NZ9SPyQXwx9PfJ0KVisadbHSKV1DpHp7AN3yS019kuFKTxSqYjWXYlnZLyb9GyvLTqr91ObH11+714wOW8D5ydI9PpK4w8xo6tGH4F1v63MD5gDnR3oDsgAAADhNGnDbcaYeTTU9X1cC5mAAADfFjwgHCcsJvgiLTqNvVa9UxDVt528TnTO+7rxGo1AQkAAAOmAA2fJnSX2W8trjOFTrR13/wDOXVn/AEykB+i7m3p1YOnUjGpCXxRksqXaTEzHeExMxO4fP33ImynGfR03Sm4vUkp1MRljY3FvGMlsZ7x5X15N4nvO3ltSDi3F74tp42rKeDa6MTt2dE26q3FClLZGpWhGX3XJJ+xFp1WZebzqsy9dstB2lFp0renBrdPV1pL8TyzBN7T5ly7ZL28yz6VvY29CtXn8NGlOb79WLeDw8PzTVqynKU5vMpycpPjKTy36sDikBkpQec4AzgAAACSR7pbVleWvVSYYjY5gAAAb0seHEDr3kurjiyjkW1XTVxa7vv6OkY3QAAAABglW4L1Axt5AjA/QnITSf2rR9tUbzOMOjqcden1W344T8wN7OWE3wTYHhMpZbfF59TpuyyWtbo6lOot9OpCa8YyT/QiY3Gi0biYe5p52rtOa4z4Xnf0n0VjGhF4ldVUmu3oodeT9VBfiA8XA5Rm1uAyxrcUBlAAAAADE0baTusS5eWvTeYQ9PAAA3ZY8IB0ryWZY4Ix8id206PFrqm/qwFDSAAAADqzWG0BxAAency2k8SubOT+JKtTXesQqf7fowPStLVNS3ry+WjUfpBnqsbmHqkbtEPEUdF2AD2rQFx0lrbT3uVGGfvKKT90znXjVphyMkavMPH+djSnTX7pReYWtNQ7ukl15v3ivwnl4fFgAMlFbfADsAAAAABwmacM9phi5Ve8S4FzKAAN0WPCAa6rLLb7znXndpl18demkQ4nl7AAAABhrx3MDCAA2/JLSn2S9tq7eIRqJVX2dFPqyb8E8+QHu3Kypq2N0+NGS/N1f1PeL1wtwx/kh44dB1QD0zkhpSNPRcqtR9W1VbX8I5nj0kjFnjV3N5MayPDrq4lVqVKtR5nVnKc3xnKTk/dlLOxAAOxQjszxAyAAAAABxnuLcM6so5Nd02xmpzwABuSx4caksJvgjzadRMvVI6rRDXHOdgAAAAADjKOVgDqgADA9hWmftOgIzbzUSp0ar7deFWKbfe4pP8Rbh9cL+NH+SHxBudMAzX+mnS0fXs4vEri4pt/ylHM/eFNebMvJjvEsPLjvEvjzMxgFisvAHaQFAAAAACM9VnUxLzeN1mGI2uUAAP//Z'>
	<h2 id='ppNameAT'>{_userService.GetById(blogDetailDto.AuthorSummary.Id, Status.Per.User).Data.Nickname}</h2>
	</div>
	<p>
	Lorem Ipsum , baskı ve dizgi endüstrisinin basit bir metnidir. Lorem Ipsum, bilinmeyen bir matbaacının bir dizi çeşidini alıp bir tür numune kitabı yapmak için karıştırdığı 1500'lerden beri endüstrinin standart kukla metni olmuştur. Sadece beş yüzyıl
	</p>
</div>
</div>
</div>
<div id='contentAT'>   
	{blogDetailDto.BlogContent}
</div>";

            string cssjs = @"
<style type='text/css'>
	*{
		color: #ffffff;
	}
	body{
		margin: 0px;
		background:  #12151c;
	}
	#topAT{
		position: fixed;
		right: 0px;
		left: 0px;
		justify-content: center;
		height: 40%;
	}
	#headerAT{
		position: relative;
		height: 100%;
		justify-content: center;
	}
    #titleAT{
		position: absolute;
		bottom: 0px;
		left: 0px;
		right: 0px;
		color: #ffffff;
        padding-left: 10px;
        padding-bottom: 85px;
        //font-size: 40px;
		font-size: 7vw;
        line-height: 85px;
        letter-spacing: -1.5px;
    }
	#infoAT{
		position: fixed;
		border-radius: 75px;
		background: #12151c;
		height: 100%;
		width: 100 - 50px%;
		margin-top: -75px;
		padding: 50px 25px;
	}
	#ppAT{
		display: inline;
		border-radius: 50%;
		width:150px;
		height:150px;
	}
	#ppNameAT{
		position: relative;
		top: -30px;
		left: 10px;
		display: inline-block;
		border-radius: 50%;
	}
	#profilAT{
	}
	#contentAT{
		position: absolute;
		border-radius: 75px 75px 0px 0px;
		background: #091318;
		width: auto;
		margin-top: 1075px;
		padding: 50px 25px;
		min-height: 1000px;
	}
    .imgA{
		position: relative;
		display: block;
		max-width: 150%;
		min-width: 100%;
		height: 100%;
		width: auto;
		justify-content: center;
		margin-left: 50%;
		transform: translateX(-50%);
		z-index: -1;
    }
	p{
		/*font-size: 3vw;*/
		font-size: 2em;
	}
	p.h1{
		font-size: 9vw;
	}
	p.h2{
		font-size: 8vw;
	}
	p.h3{
		font-size: 7vw;
	}
	p.h4{
		font-size: 6vw;
	}
	p.h5{
		font-size: 5vw;
	}
	p.h6{
		font-size: 4vw;
	}
	p.center{
		text-align: center;
	}
</style>";

string js = @"<script>var json = " + $@"{JsonConvert.SerializeObject(blogDetailDto)}" +@"
console.log(json)

start();
function sleep(ms) {
  return new Promise(resolve => setTimeout(resolve, ms));
}
async function start(){
	await sleep(1000);
	var imgu = document.getElementById('headerAT').offsetHeight;
	var infa = document.getElementById('infoAT').children;
	var infau = 0;
	for (i = 0; i < infa.length; i++) {	
		var cs = window.getComputedStyle(infa[i]);
		infau = infau + infa[i].offsetHeight + parseInt(cs.marginTop.toString().slice(0, -2)) + parseInt(cs.marginBottom.toString().slice(0, -2));
	}
	var uzunluk = imgu - 75 + 50 + infau;
	document.getElementById('contentAT').style.marginTop = uzunluk + 'px';
}
</script>";

            return htmlStr + cssjs + js;
        }

        public IDataResult<Blog> GetById(int blogId,Status.Per per = Status.Per.User)
        {
            var blog = _blogDal.Get(p => p.Id == blogId);
            if (blog != null)
            {
                if ((((Status.GetBlogByIdFilter(per) ^ blog.BlogStatus) & Status.GetBlogByIdMask(per)) == 0))
                    return new SuccessDataResult<Blog>(blog);
                else
                    return new ErrorDataResult<Blog>(message:Messages.BlogNotAccessible);
            }
            return new ErrorDataResult<Blog>(message:Messages.BlogNotFound);
        }

        public IDataResult<List<BlogSummaryDto>> GetByAuthorId(int authorId, PageFilter pageFilter, Status.Per per = Status.Per.User)
        {
            //var blogs = _blogDal.GetPaged(pageFilter, b => (((Status.GetBlogByIdFilter(per) | b.BlogStatus) & (2147483647 - (Status.GetBlogByIdFilter(per) & b.BlogStatus))) & Status.GetBlogByIdMask(per)) == 0 && b.AuthorId == authorId);

            var blogs = _blogDal.GetList(b => (((Status.GetBlogByAuthorIdFilter(per) | b.BlogStatus) & (2147483647 - (Status.GetBlogByAuthorIdFilter(per) & b.BlogStatus))) & Status.GetBlogByAuthorIdMask(per)) == 0 && b.AuthorId == authorId);

            if (blogs == null)
            {
                return new ErrorDataResult<List<BlogSummaryDto>>(message: Messages.BlogNotFound);
            }

            var result = from b in blogs.OrderByDescending(b => b.Id).Skip(pageFilter.PageNumber * pageFilter.PageSize).Take(pageFilter.PageSize)
                select new BlogSummaryDto
                {
                    BlogId = b.Id,
                    BlogDate = _dateTimeHelper.SetTime(b.BlogDate),
                    BlogTitle = b.BlogTitle,
                    BlogSummary = b.BlogSummary,
                    BlogTitlePhotoUrl = b.BlogTitlePhotoUrl,
                    BlogTags = GetTags(b.Id).Data,
                    AuthorName = (per == Status.Per.System ?
                        (((b.BlogStatus & Status.Approval) == 0) ? "Onaylı - " : "") + (((b.BlogStatus & Status.Hidden) == 0) ? "Yayında" : "Gizli") :
                        _userService.GetById(b.AuthorId, Status.Per.User).Data.Nickname),
                    Views = _blogEmojiCountViewService.GetByBlogId(b.Id).Data.BlogCount,
                    Readed = false//_blogEmojiViewService.UserViewed(b.Id, userId).Success
                };

            return new SuccessDataResult<List<BlogSummaryDto>>(result.ToList());
        }

        public IDataResult<List<BlogSummaryDto>> GetByUserReaded(int userId, PageFilter pageFilter, Status.Per per = Status.Per.User)
        {
            var blogEmoji = _blogEmojiService.GetByUserIdAndEmojiId(userId, 1).Data.OrderByDescending(be => be.Id).Select(be => be.BlogId).Skip(pageFilter.PageNumber * pageFilter.PageSize).Take(pageFilter.PageSize);
            //var blogsResult = blogEmoji.Select(be => GetById(be));
            //var blogs = blogsResult.Where(b => b.Success).Select(b => b.Data);
            var blogs = blogEmoji.Select(be => GetById(be, per)).Where(b => b.Success).Select(b => b.Data);

            var result = from b in blogs
                select new BlogSummaryDto
                {
                    BlogId = b.Id,
                    BlogDate = _dateTimeHelper.SetTime(b.BlogDate),
                    BlogTitle = b.BlogTitle,
                    BlogSummary = b.BlogSummary,
                    BlogTitlePhotoUrl = b.BlogTitlePhotoUrl,
                    BlogTags = GetTags(b.Id).Data,
                    AuthorName = _userService.GetById(b.AuthorId, Status.Per.User).Data.Nickname,
                    Views = _blogEmojiCountViewService.GetByBlogId(b.Id).Data.BlogCount,
                    Readed = false //_blogEmojiViewService.UserViewed(b.Id, userId).Success
                };

            return new SuccessDataResult<List<BlogSummaryDto>>(result.ToList());
        }

        public IDataResult<List<Blog>> GetByAuthorId(int authorId, Status.Per per = Status.Per.User)
        {
            var blogs = _blogDal.GetListEnumerable(b => (((Status.GetBlogByUserReadedFilter(per) | b.BlogStatus) & (2147483647 - (Status.GetBlogByUserReadedFilter(per) & b.BlogStatus))) & Status.GetBlogByUserReadedMask(per)) == 0 && b.AuthorId == authorId);

            if (blogs == null)
            {
                return new ErrorDataResult<List<Blog>>(message: Messages.BlogNotFound);
            }

            return new SuccessDataResult<List<Blog>>(blogs.ToList());
        }

        public IDataResult<List<Blog>> GetByStatus(int mask, int filter)
        {
            var result = _blogDal.GetList(b => (((filter | b.BlogStatus) & (2147483647 - (filter & b.BlogStatus))) & mask) == 0);
            if (result == null)
            {
                return new ErrorDataResult<List<Blog>>(message: Messages.BlogNotFound);
            }
            return new SuccessDataResult<List<Blog>>(result);
        }

        public IDataResult<List<Blog>> GetList()
        {
            return new SuccessDataResult<List<Blog>>(_blogDal.GetList().ToList());
        }

        public IDataResult<List<BlogSummaryDto>> GetSearchList(List<string> text, Status.Per per = Status.Per.User)
        {
            var blogs = _blogDal.GetSearchList(text,b => (((Status.GetBlogBySearchFilter(per) ^ b.BlogStatus) & Status.GetBlogBySearchMask(per)) == 0));
            var result = from b in blogs
                select b.ToSummary(_userService.GetById(b.AuthorId, Status.Per.UnUser).Data.Nickname,
                    GetTags(b.Id).Data);
            return new SuccessDataResult<List<BlogSummaryDto>>(result.ToList());
        }

        public IDataResult<List<BlogSummaryDto>> GetListSummary()
        {
            var blogs = _blogDal.GetList().ToList();
            var result = from b in blogs
                select new BlogSummaryDto
                {
                    BlogId = b.Id,
                    BlogDate = _dateTimeHelper.SetTime(b.BlogDate),
                    BlogTitle = b.BlogTitle,
                    BlogSummary = b.BlogSummary,
                    BlogTitlePhotoUrl = b.BlogTitlePhotoUrl,
                    BlogTags = GetTags(b.Id).Data,
                    AuthorName = _userService.GetById(b.AuthorId, Status.Per.User).Data.Nickname,
                    Views = _blogEmojiCountViewService.GetByBlogId(b.Id).Data.BlogCount,
                    Readed = false
                };

            return new SuccessDataResult<List<BlogSummaryDto>>(result.ToList());
        }

        public IDataResult<List<Tag>> GetTags(int blogId)
        {
            return new SuccessDataResult<List<Tag>>(_blogDal.GetTag(blogId));
        }

        public IDataResult<int> Add(Blog blog)
        {
            return new SuccessDataResult<int>(_blogDal.Add(blog), Messages.BlogAdded);
        }

        public IResult Update(Blog blog)
        {
            _blogDal.Update(blog); 
            return new SuccessResult(Messages.BlogUpdated);
        }

        public IResult UpdateStatus(int blogId, int status, Status.Per per)
        {
            var blog = GetById(blogId, Status.Per.System).Data;

            if (blog == null)
            {
                return new ErrorResult(Messages.BlogNotFound);//kullanıcı bulunamadı --- id yanlıs olabilir
            }

            if (per == Status.Per.System || per == Status.Per.Admin)
            {
                blog.BlogStatus = status;
            }
            else
            {
                return new ErrorResult();//su anlık yok. kullanıcılar belirli ayarlar ı ayarlayabilecek sadece
            }

            _blogDal.Update(blog);
            return new SuccessResult();
        }

        public IResult Delete(Blog blog)
        {
            _blogDal.Delete(blog);
            return new SuccessResult(Messages.BlogDeleted);
        }

        public IDataResult<List<BlogSummaryDto>> GetPage(PageFilter pageFilter, int userId, out object metadata, Status.Per per)
        {   //"(({0} & 4) = 4)".SQL<bool>(t.AgeFilterId)                    (((Status.Neno ^ b.BlogStatus) & Status.DontView) == 0)      (x|y) & ~(x&y)  (0|b.BlogStatus) & ~(0&b.BlogStatus)
            //var blogs = _blogDal.GetPaged(pageFilter, b => (((Status.Neno ^ b.BlogStatus) & Status.DontView) == 0));
            //var blogs = _blogDal.GetPaged(pageFilter, b => (((Status.Neno | b.BlogStatus) & ~(Status.Neno & b.BlogStatus)) & Status.DontView) == 0);
            //var blogs = _blogDal.GetPaged(pageFilter, b => (b.BlogStatus & Status.DontView) == 0);
            //var blogs = _blogDal.GetPaged(pageFilter, b => b.BlogStatus < 1);

            //var blogs = _blogDal.GetPaged(pageFilter, b => (((Status.Neno | b.BlogStatus) & (2147483647 - (Status.Neno & b.BlogStatus))) & Status.DontView) == 0);
            var blogs = _blogDal.GetPaged(pageFilter, b => (((Status.GetBlogPageFilter(per) | b.BlogStatus) & (2147483647 - (Status.GetBlogPageFilter(per) & b.BlogStatus))) & Status.GetBlogPageMask(per)) == 0);

            metadata = new
            {
                blogs.TotalCount,
                blogs.PageSize,
                blogs.CurrentPage,
                blogs.TotalPages,
                blogs.HasNext,
                blogs.HasPrevious
            };

            var result = from b in blogs
                select new BlogSummaryDto
                {
                    BlogId = b.Id,
                    BlogDate = _dateTimeHelper.SetTime(b.BlogDate),
                    BlogTitle = b.BlogTitle,
                    BlogSummary = b.BlogSummary,
                    BlogTitlePhotoUrl = b.BlogTitlePhotoUrl,
                    BlogTags = GetTags(b.Id).Data,
                    AuthorName = _userService.GetById(b.AuthorId, Status.Per.User).Data.Nickname,
                    Views = _blogEmojiCountViewService.GetByBlogId(b.Id).Data.BlogCount,
                    Readed = _blogEmojiViewService.UserViewed(b.Id, userId).Success
                };
            return new SuccessDataResult<List<BlogSummaryDto>>(result.ToList());
        }
        public IDataResult<List<BlogSummaryDto>> GetPage(PageFilter pageFilter, out object metadata, Status.Per per)
        {//"(({0} & 4) = 4)".SQL<bool>(t.AgeFilterId)                    (((Status.Neno ^ b.BlogStatus) & Status.DontView) == 0)      (x|y) & ~(x&y)  (0|b.BlogStatus) & ~(0&b.BlogStatus)
         //var blogs = _blogDal.GetPaged(pageFilter, b => (((Status.Neno ^ b.BlogStatus) & Status.DontView) == 0));
         //var blogs = _blogDal.GetPaged(pageFilter, b => (((Status.Neno | b.BlogStatus) & ~(Status.Neno & b.BlogStatus)) & Status.DontView) == 0);
         //var blogs = _blogDal.GetPaged(pageFilter, b => (b.BlogStatus & Status.DontView) == 0);
         //var blogs = _blogDal.GetPaged(pageFilter, b => b.BlogStatus < 1);

         var blogs = _blogDal.GetPaged(pageFilter, b => (((Status.GetBlogPageFilter(per) | b.BlogStatus) & (2147483647 - (Status.GetBlogPageFilter(per) & b.BlogStatus))) & Status.GetBlogPageMask(per)) == 0);

            metadata = new
            {
                blogs.TotalCount,
                blogs.PageSize,
                blogs.CurrentPage,
                blogs.TotalPages,
                blogs.HasNext,
                blogs.HasPrevious
            };

            var result = from b in blogs
                select new BlogSummaryDto
                {
                    BlogId = b.Id,
                    BlogDate = _dateTimeHelper.SetTime(b.BlogDate),
                    BlogTitle = b.BlogTitle,
                    BlogSummary = b.BlogSummary,
                    BlogTitlePhotoUrl = b.BlogTitlePhotoUrl,
                    BlogTags = GetTags(b.Id).Data,
                    AuthorName = _userService.GetById(b.AuthorId, Status.Per.User).Data.Nickname,
                    Views = _blogEmojiCountViewService.GetByBlogId(b.Id).Data.BlogCount,
                    Readed = false
                };
            return new SuccessDataResult<List<BlogSummaryDto>>(result.ToList());
        }
        public IDataResult<List<BlogSummaryDto>> GetPage(PageFilter pageFilter, int userId, Status.Per per)
        {
            //var blogs = _blogDal.GetPaged(pageFilter,b=> (((Status.Neno ^ b.BlogStatus) & Status.DontView) == 0));
            var blogs = _blogDal.GetPaged(pageFilter, b => (((Status.GetBlogPageFilter(per) | b.BlogStatus) & (2147483647 - (Status.GetBlogPageFilter(per) & b.BlogStatus))) & Status.GetBlogPageMask(per)) == 0);
            //var blogs = _blogDal.GetPaged(pageFilter, b => (b.BlogStatus & Status.DontView) == 0);
            //var blogs = _blogDal.GetPaged(pageFilter, b => b.BlogStatus < 1);


            var result = from b in blogs
                select new BlogSummaryDto
                {
                    BlogId = b.Id,
                    BlogDate = _dateTimeHelper.SetTime(b.BlogDate),
                    BlogTitle = b.BlogTitle,
                    BlogSummary = b.BlogSummary,
                    BlogTitlePhotoUrl = b.BlogTitlePhotoUrl,
                    BlogTags = GetTags(b.Id).Data,
                    AuthorName = _userService.GetById(b.AuthorId, Status.Per.User).Data.Nickname,
                    Views = _blogEmojiCountViewService.GetByBlogId(b.Id).Data.BlogCount,
                    Readed = _blogEmojiViewService.UserViewed(b.Id, userId).Success
                };
            return new SuccessDataResult<List<BlogSummaryDto>>(result.ToList());
        }
        public IDataResult<List<BlogSummaryDto>> GetPage(PageFilter pageFilter, Status.Per per)
        {
            //var blogs = _blogDal.GetPaged(pageFilter,b=> (((Status.Neno ^ b.BlogStatus) & Status.DontView) == 0));
            var blogs = _blogDal.GetPaged(pageFilter, b => (((Status.GetBlogPageFilter(per) | b.BlogStatus) & (2147483647 - (Status.GetBlogPageFilter(per) & b.BlogStatus))) & Status.GetBlogPageMask(per)) == 0);
            //var blogs = _blogDal.GetPaged(pageFilter, b => (b.BlogStatus & Status.DontView) == 0);
            //var blogs = _blogDal.GetPaged(pageFilter, b => b.BlogStatus < 1);


            var result = from b in blogs
                select new BlogSummaryDto
                {
                    BlogId = b.Id,
                    BlogDate = _dateTimeHelper.SetTime(b.BlogDate),
                    BlogTitle = b.BlogTitle,
                    BlogSummary = b.BlogSummary,
                    BlogTitlePhotoUrl = b.BlogTitlePhotoUrl,
                    BlogTags = GetTags(b.Id).Data,
                    AuthorName = _userService.GetById(b.AuthorId, Status.Per.User).Data.Nickname,
                    Views = _blogEmojiCountViewService.GetByBlogId(b.Id).Data.BlogCount,
                    Readed = false
                };
            return new SuccessDataResult<List<BlogSummaryDto>>(result.ToList());
        }
    }
}