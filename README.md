### Hi there, I'm Ahmet Tarık DUYAR [Evrenomi](https://api.evrenomi.com/api/tests/getComment) 👋

## Api Methods

**[AUTH](#AUTH)**<br />
**[ADMİN](#ADMİN)**<br />
**[AUTHORS](#AUTHORS)**<br />
**[BLOGS](#BLOGS)**<br />
**[USER](#USER)**<br />
**[TEST](#TEST)**<br />


## AUTH
| HTTP Methods|Url|Authorize|Response|Request|
|:---:|:---:|:---:|:---:|:---:|
|**POST**|/auth/login||[Token](#Token)|[UserForLoginDto](#UserForLoginDto)|
|**POST**|/auth/register||[Token](#Token)|[UserForRegisterDto](#UserForRegisterDto)|


## ADMİN
| HTTP Methods|Url|Authorize|Response|Request|
|:---:|:---:|:---:|:---:|:---:|
|**POST**|/admin/setOperationClaimToUser|Admin|OK|[OperationClaimToUserDto](#OperationClaimToUserDto)|
|**POST**|/admin/deleteOperationClaimToUser|Admin|OK|[OperationClaimToUserDto](#OperationClaimToUserDto)|
|**POST**|/admin/addAuthor|Admin||[AuthorForRegister](#AuthorForRegister)|
|**GET**|/admin/getUser|Admin|[UserDetailDto](#UserDetailDto)|?userId=(int)|
|**GET**|/admin/getBlog|Admin|[BlogDetailDto](#BlogDetailDto)|?blogId=(int)|
|**GET**|/admin/getAllUser|Admin|List<[UserSummaryDto](#UserSummaryDto)>||
|**GET**|/admin/getAllBlog|Admin|List<[BlogSummaryDto](#BlogSummaryDto)>||
|**GET**|/admin/getAllBlogByStatus|Admin|List<[Blog](#Blog)>|?status=(int)|

## AUTHORS
| HTTP Methods|Url|Authorize|Response|Request|
|:---:|:---:|:---:|:---:|:---:|
|**GET**|/authors/getAllAuthor|Auth|List<Author>||
|**GET**|/authors/getAuthor|Author|Author|?authorId=(int)|
|**GET**|/authors/getAuthor|Author|Author|?authorId=(int)|
|**POST**|/authors/getAuthorAccount|Auth|OK|[AuthorForRegister](#AuthorForRegister)|
|**POST**|/authors/postBlog|Author|OK|[BlogDetailDto](#BlogDetailDto)|

## BLOGS
| HTTP Methods|Url|Authorize|Response|Request|
|:---:|:---:|:---:|:---:|:---:|
|**GET**|/blogs/getBlog||HtmlPage|?id=(int)|
|**GET**|/blogs/getBlogComment||List<[CommentForBlog](#CommentForBlog)>|?blogId=(int)|
|**GET**|/blogs/getBlogCommentResponse||List<[CommentForBlog](#CommentForBlog)>|?blogCommentId=(int)|
|**POST**|/blogs/getbypage|Auth|List<[BlogSummaryDto](#BlogSummaryDto)>|[BlogPageFilter](#BlogPageFilter)|
|**POST**|/blogs/addBlogComment|Auth|OK|[AddCommentForBlog](#AddCommentForBlog)|
|**POST**|/blogs/updateBlogComment|Auth|OK|[CommentForBlog](#CommentForBlog)|
|**POST**|/blogs/deleteBlogComment|Auth|OK|[CommentForBlog](#CommentForBlog)|


## USER
| HTTP Methods|Url|Authorize|Response|Request|
|:---:|:---:|:---:|:---:|:---:|
|**GET**|/users/getMyProfil|Me|[UserDetailDto](#UserDetailDto)||
|**POST**|/users/updateUser|Me|[UserDetailDto](#UserDetailDto)|[UserDetailDto](#UserDetailDto)|
|**POST**|/users/updateUserPp|Me|ABCD1.jpg|IFormFile|


## TEST
| HTTP Methods|Url|Authorize|Response|Request|
|:---:|:---:|:---:|:---:|:---:|
|**GET**|/tests/admin|Admin|"Sen Admin Sin"||
|**GET**|/tests/auth|Authorize|"Sen Giris Yapmıs Sın"||
|**POST**|/tests/postUser|Authorize|"NickName = {Nickname} Email = {Email} :D"|[UserForRegisterDto](#UserForRegisterDto)|
|**GET**|/tests/getUser|Authorize||[UserForRegisterDto](#UserForRegisterDto)|
|**GET**|/tests/getUserId|Authorize||"Sen in id'in = {Id} :D"|
|**POST**|/tests/postComment||[Comment](#comment)|[Comment](#comment)|
|**GET**|/tests/getComment|||[Comment](#comment)|
|**GET**|/tests/okComment|||[Comment](#comment)|
|**GET**|/tests/badRequestComment|||400([Comment](#comment))|



---
---



<br />

## Api DTO(data transform object)<br />
**[Token](#Token)**<br />
**[Comment](#comment)**<br />
**[UserForLoginDto](#UserForLoginDto)**<br />
**[UserForRegisterDto](#UserForRegisterDto)**<br />
**[ErrorResponseDto](#ErrorResponseDto)**<br />
**[OperationClaim](#OperationClaim)**<br />
**[OperationClaimToUserDto](#OperationClaimToUserDto)**<br />
**[BlogDetailDto](#BlogDetailDto)**<br />
**[BlogSummaryDto](#BlogSummaryDto)**<br />
**[UserDetailDto](#UserDetailDto)**<br />
**[UserSummaryDto](#UserSummaryDto)**<br />

<br />

## Token
|Token||Required|Max|Min|
|:---:|:---:|:---:|:---:|:---:|
|string|token||||
|DateTime|expiration||||

```
{
    "token": "eyJhbGciOiJodH...",
    "expiration": "2020-11-25T18:09:24.5477551+03:00"
}
```

<br />

## Comment
|Comment||Required|Max|Min|
|:---:|:---:|:---:|:---:|:---:|
|string|text||||

```
{
    "text":"Merhaba Dünya."
}
```

<br />

## UserForLoginDto
|UserForLoginDto||Required|Max|Min|
|:---:|:---:|:---:|:---:|:---:|
|string|EmailOrNickname|Yes|45|3|
|string|Password|Yes|20|8|

```
{
    "emailOrNickname":"test@gmail.com",
    "password":"12345678",
}
```
<br />


## UserForRegisterDto
|UserForRegisterDto||Required|Max|Min|
|:---:|:---:|:---:|:---:|:---:|
|string|Nickname|Yes|20|3|
|string|Email|Yes|45|10|
|string|Password|Yes|20|8|

```
{
    "email":"test@gmail.com",
    "password":"12345678",
    "nickName":"Test"
}
```

<br />

## ErrorResponseDto
|ErrorResponseDto||Required|Max|Min|
|:---:|:---:|:---:|:---:|:---:|
|string|Operation||||
|string|ErrorMessages||||

```
{
    "operation": "Register",
    "errorMessages": "Kullanıcı Zaten Kayıtlı"
}
```


<br />

## OperationClaim
|OperationClaim||Required|Max|Min|
|:---:|:---:|:---:|:---:|:---:|
|int|Id||||
|string|Name||||

```
{
    "id": "1",
    "name": "Admin"
}
```

<br />

## OperationClaimToUserDto
|OperationClaimToUserDto||Required|Max|Min|
|:---:|:---:|:---:|:---:|:---:|
|UserForLoginDto|userForLoginDto||||
|OperationClaim|operationClaim||||

```
{
    "userForLoginDto":{
        "email":"test@gmail.com",
        "password":"12345678"
    },
    "operationClaim":{
        "id":1,
        "name":"Admin"
    }
}
```
<br />

## BlogDetailDto
|BlogDetailDto||Required|Max|Min|
|:---:|:---:|:---:|:---:|:---:|
|int|BlogId||||
|int|authorId||||
|string|blogDate||||
|string|blogTitle||||
|string|blogTitlePhotoUrl||||
|string|BlogTblogSideTitleags||||
|string|blogContent||||
|string|blogTags||||

```
{
    "blogId": 1,
    "authorId": 1,
    "blogDate": "2020-01-11T00:00:00",
    "blogTitle": "Baskık",
    "blogTitlePhotoUrl": "url",
    "blogSideTitle": "BaskıkYan",
    "blogContent": "Test yazisi 123",
    "blogTags": "Bilim"
}
```

<br />

## BlogSummaryDto
|BlogSummaryDto||Required|Max|Min|
|:---:|:---:|:---:|:---:|:---:|
|int|BlogId||||
|string|AuthorName||||
|string|BlogTitle||||
|string|BlogTitlePhotoUrl||||
|string|BlogTags||||
|string|BlogDate||||

```
{
    "blogId": 1,
    "authorName": "Azathoth",
    "blogTitle": "Baskık",
    "blogTitlePhotoUrl": "url",
    "blogTags": "bilim",
    "blogDate": "2020-01-11T00:00:00",
    "views": 3,
    "readed": true
}
```
<br />

## UserDetailDto
|UserDetailDto||Required|Max|Min|
|:---:|:---:|:---:|:---:|:---:|
|int|Id||||
|string|Nickname||||
|string|FirstName||||
|string|LastName||||
|string|Email||||
|string|AvatarUrl||||
|string|Description||||

```
{
    "Id": 1,
    "Nickname": "Azathoth",
    "FirstName": "Test",
    "LastName": "Metod",
    "Email": "test@gmail.com",
    "AvatarUrl": "url"
    "Description": "Ben bu siteye katıt oldum"
}
```

<br />

## UserSummaryDto
|UserSummaryDto||Required|Max|Min|
|:---:|:---:|:---:|:---:|:---:|
|int|Id||||
|string|Nickname||||
|string|AvatarUrl||||

```
{
    "Id": 1,
    "Nickname": "Azathoth",
    "AvatarUrl": "url"
}
```

<br />

## AuthorForRegister
|AuthorForRegister||Required|Max|Min|
|:---:|:---:|:---:|:---:|:---:|
|int|UserId||||
|string|AuthorName||||
|string|AuthorAvatarUrl||||
|string|AuthorDescription||||

```
{
    "UserId": 1,
    "AuthorName": "Azathoth",
    "AuthorAvatarUrl": "url",
    "AuthorDescription": "VOID"
}
```
<br />

## BlogPageFilter
|BlogPageFilter||Defualt|Required|Max|Min|
|:---:|:---:|:---:|:---:|:---:|:---:|
|int|PageSize||10|||
|int|PageNumber|1|No|50||

```
{
    "PageSize": 50,
    "PageNumber": 1,
}
```
<br />

## CommentForBlog
|CommentForBlog||Defualt|Required|Max|Min|
|:---:|:---:|:---:|:---:|:---:|:---:|
|int|CommentId|||||
|int|CommentResponse|0||50||
|string|UserId|||||
|string|UserAvatarUrl|||||
|string|UserNickname|||||
|string|CommentDate|||||
|string|Text|||||

```
{
    "commentId": 1,
    "commentResponse": 2,
    "userId": 1,
    "userAvatarUrl": "FDRE2.jpg",
    "userNickname": "Atduyar",
    "commentDate": "1 gün önce",
    "text": "Gerecekten ilginç bir konu 🤔🤔"
}
```
<br />

## AddCommentForBlog
|AddCommentForBlog||Defualt|Required|Max|Min|
|:---:|:---:|:---:|:---:|:---:|:---:|
|int|BlogId||||1|
|int|ParentBlogCommentId|null|No|||
|string|Text|||||

```
{
    "blogId": 1
    "ParentBlogCommentId": null
    "text": "Gerecekten ilginç bir konu 🤔🤔",
}
```


<br />
<br />
<br />

### My work:

[<img align="left" alt="My first flutter project" width="26px" src="http://www.atduyar.com/ckdepi/icons/Icon-512.png" />][ckdepi]
[<img align="left" alt="Giftnator" width="26px" src="https://pics.clipartpng.com/Gift_Box_in_Red_PNG_Clipart-276.png" />][giftnator]
[<img align="left" alt="Giftnator" width="26px" src="http://www.gstatic.com/android/market_images/web/favicon_v2.ico" />][playstore]

<br />

[website]: http://www.atduyar.com/wp/
[twitter]: https://twitter.com/atduyar
[youtube]: https://www.youtube.com/channel/UCC_A8qsGhbQYuCYqS82cgTA
[instagram]: https://www.instagram.com/atduyar/
[linkedin]: https://www.linkedin.com/in/ahmet-tar%C4%B1k-duyar-106051137/

[ckdepi]: http://www.atduyar.com/ckdepi/index.html#/
[giftnator]: http://www.atduyar.com/giftnator/
[playstore]: https://play.google.com/store/search?q=pub%3ANothingness&c=apps&gl=TR

