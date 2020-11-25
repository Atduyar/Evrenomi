### Hi there, I'm Ahmet Tarık DUYAR [Atduyar](api.atduyar.com/api/admin/testgetcomment) 👋

## Api Methods
| HTTP Methods|Url|Authorize|Response|Request|
|:---:|:---:|:---:|:---:|:---:|
|**AUTH**|
|**POST**|/auth/login||Token|UserForLoginDto|
|**POST**|/auth/register||Token|UserForRegisterDto|
|**ADMİN**|
|**POST**|/admin/setOperationClaimToUser|Admin|OK|operationClaimToUserDto|
|**POST**|/admin/deleteOperationClaimToUser|Admin|OK|operationClaimToUserDto|
|**TEST**|
|**GET**|/tests/admin|Admin|"Sen Admin Sin"||
|**GET**|/tests/auth|Authorize|"Sen Giris Yapmıs Sın"||
|**POST**|/tests/postUser|Authorize|"NickName = {Nickname} Email = {Email} :D"|UserForRegisterDto|
|**GET**|/tests/getUser|Authorize||UserForRegisterDto|
|**GET**|/tests/getUserId|Authorize||"Sen in id'in = {Id} :D"|
|**POST**|/tests/postComment||Comment|Comment|
|**GET**|/tests/getComment|||Comment|
|**GET**|/tests/ok|||Comment|
|**GET**|/tests/badRequest|||400(Comment)|
|**BLOGS**|
|**POST**|/blogs/getall||BlogSummaryDto||

---
<br />

## Api DTO(data transform object)

|Comment||Required|Max|Min|
|:---:|:---:|:---:|:---:|:---:|
|string|text||||

```
{
    "text":"Merhaba Dünya."
}
```

<br />

|UserForLogin||Required|Max|Min|
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
    "blogDate": "2020-01-11T00:00:00"
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

